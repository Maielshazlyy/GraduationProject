# AI ↔ Backend Contract — Version 2.0
# Desktop Voice AI + Separated Frontend Architecture

**Last Updated:** 2026-05-28  
**Status:** Active — Replaces V1.0

---

## Architecture Overview

The system now has three **independent** communication channels:

```
┌─────────────────────────────────────────────────────────────────────┐
│                                                                       │
│   ┌──────────────────┐     Live Audio       ┌─────────────────────┐  │
│   │                  │ ◄──── WebRTC/WS ────► │                     │  │
│   │  Desktop AI App  │                       │  Customer / Meeting  │  │
│   │  (Voice Agent)   │                       │       Device        │  │
│   └────────┬─────────┘                       └─────────────────────┘  │
│            │                                                           │
│            │  REST API (async push + context pull)                    │
│            │                                                           │
│   ┌────────▼─────────┐                                               │
│   │                  │                                               │
│   │   .NET Backend   │                                               │
│   │   (Storage &     │                                               │
│   │    Analytics)    │                                               │
│   └────────▲─────────┘                                               │
│            │                                                           │
│            │  REST API (auth, history, dashboard)                     │
│            │                                                           │
│   ┌────────┴─────────┐                                               │
│   │                  │                                               │
│   │    Frontend      │                                               │
│   │  (Dashboard &    │                                               │
│   │   Management)    │                                               │
│   └──────────────────┘                                               │
│                                                                       │
└─────────────────────────────────────────────────────────────────────┘
```

### What each party owns:

| Party | Owns | Does NOT touch |
|-------|------|----------------|
| **Desktop AI App** | Mic capture, STT, multi-model reasoning, TTS, voice output | Frontend UI, database |
| **Backend** | Data storage, business context, analytics, auth | Audio processing, voice generation |
| **Frontend** | Dashboard, conversation history, settings UI | Audio pipeline, AI models |

---

## Channel 1 — AI ↔ Backend (The Contract)

The AI is the **caller**. The backend is a **data store and context provider**.

### 1.1 — AI Pulls Business Context (before/during conversation)

**When:** AI calls this once at the start of a conversation session (or on a cache miss).

```
GET /api/chatbot/context/{businessId}
Authorization: Bearer {ai_service_token}
```

**Response:**
```json
{
  "businessId": "biz-123",
  "businessName": "Burger Palace",
  "workingHours": [
    { "dayOfWeek": "Monday", "openTime": "09:00", "closeTime": "23:00" }
  ],
  "menuCategories": [
    {
      "id": "cat-1",
      "name": "Burgers",
      "items": [
        { "id": "item-1", "name": "Big Burger", "price": 50, "available": true, "description": "..." }
      ]
    }
  ],
  "knowledgeBase": [
    { "id": "kb-1", "question": "Do you deliver?", "answer": "Yes, within 5km." }
  ],
  "agentSettings": {
    "agentName": "Maya",
    "defaultLanguage": "ar",
    "supportedLanguages": ["ar", "en"],
    "escalationPhoneNumber": "+201001234567"
  }
}
```

---

### 1.2 — AI Opens a Conversation Session

**When:** AI detects a new meeting/call has started.

```
POST /api/conversations/sessions
Authorization: Bearer {ai_service_token}
Content-Type: application/json
```

**Request Body:**
```json
{
  "businessId": "biz-123",
  "channel": "VoiceDesktop",
  "startedAt": "2026-05-28T14:00:00Z",
  "customerIdentifier": "phone:+201001234567",
  "meetingContext": "walk-in"
}
```

**Response:**
```json
{
  "sessionId": "session-abc-123",
  "businessId": "biz-123",
  "status": "Active"
}
```

The AI must store `sessionId` and include it in all subsequent calls for this conversation.

---

### 1.3 — AI Pushes a Conversation Turn (real-time or batched)

**When:** After each exchange (customer speaks → AI responds). Can be sent in real-time or buffered and sent every N turns.

```
POST /api/conversations/sessions/{sessionId}/turns
Authorization: Bearer {ai_service_token}
Content-Type: application/json
```

**Request Body:**
```json
{
  "turns": [
    {
      "turnIndex": 1,
      "timestamp": "2026-05-28T14:00:05Z",
      "speaker": "Customer",
      "transcript": "عايز أطلب برجر كبير",
      "audioLengthMs": 2400,
      "language": "ar",
      "dialect": "Egyptian",
      "sentiment": {
        "score": 0.6,
        "label": "Positive"
      }
    },
    {
      "turnIndex": 2,
      "timestamp": "2026-05-28T14:00:08Z",
      "speaker": "Agent",
      "transcript": "تمام! سجلت لك برجر كبير. عايز تضيف بطاطس أو مشروب؟",
      "audioLengthMs": 3100,
      "language": "ar",
      "dialect": "Egyptian",
      "sentiment": null,
      "intentDetected": "CreateOrder",
      "actionTaken": {
        "type": "OrderCreated",
        "referenceId": "ord-456",
        "details": { "items": ["Big Burger"], "totalPrice": 50 }
      }
    }
  ]
}
```

**Response:** `204 No Content`

---

### 1.4 — AI Closes a Conversation Session

**When:** Meeting/call ends or AI detects silence/disconnect.

```
POST /api/conversations/sessions/{sessionId}/end
Authorization: Bearer {ai_service_token}
Content-Type: application/json
```

**Request Body:**
```json
{
  "endedAt": "2026-05-28T14:12:30Z",
  "durationSeconds": 750,
  "analysis": {
    "summary": "Customer ordered a large burger. Satisfied with service. No escalation needed.",
    "summaryAr": "العميل طلب برجر كبير. راضي عن الخدمة. لا حاجة للتصعيد.",
    "overallSentiment": {
      "score": 0.72,
      "label": "Positive"
    },
    "mainTopics": ["Food Order", "Upsell Attempt"],
    "intentsDetected": ["CreateOrder", "AskAboutProducts"],
    "actionsPerformed": [
      { "type": "OrderCreated", "referenceId": "ord-456" }
    ],
    "escalationRequired": false,
    "escalationReason": null,
    "keyMoments": [
      { "turnIndex": 1, "description": "Order placed" }
    ],
    "modelsUsed": ["whisper-large-v3", "gpt-4o", "tts-1-hd"]
  }
}
```

**Response:**
```json
{
  "sessionId": "session-abc-123",
  "status": "Closed",
  "storedAt": "2026-05-28T14:12:31Z"
}
```

---

### 1.5 — AI Reports an Error / Escalation

**When:** AI cannot handle something and needs to flag it to a human agent via the backend.

```
POST /api/conversations/sessions/{sessionId}/escalate
Authorization: Bearer {ai_service_token}
Content-Type: application/json
```

**Request Body:**
```json
{
  "escalatedAt": "2026-05-28T14:05:00Z",
  "reason": "Customer requesting refund - AI cannot authorize",
  "urgency": "High",
  "lastTranscript": "عايز أرجع فلوسي، الطلب غلط",
  "turnIndex": 7
}
```

**Response:** `200 OK` — Backend creates a ticket and notifies an agent.

---

## Channel 2 — Frontend ↔ Backend (Unchanged Role, Extended Data)

The frontend connects to the backend for everything **except live audio**. It never talks to the AI app directly.

### New endpoints the frontend now needs:

#### Get all conversation sessions for a business
```
GET /api/conversations/sessions?businessId={id}&from={date}&to={date}
Authorization: Bearer {owner_jwt}
```

**Response:**
```json
{
  "sessions": [
    {
      "sessionId": "session-abc-123",
      "startedAt": "2026-05-28T14:00:00Z",
      "endedAt": "2026-05-28T14:12:30Z",
      "durationSeconds": 750,
      "channel": "VoiceDesktop",
      "overallSentiment": "Positive",
      "sentimentScore": 0.72,
      "totalTurns": 12,
      "actionsPerformed": ["OrderCreated"],
      "escalated": false
    }
  ],
  "total": 1
}
```

#### Get full transcript of a session
```
GET /api/conversations/sessions/{sessionId}/transcript
Authorization: Bearer {owner_jwt}
```

**Response:**
```json
{
  "sessionId": "session-abc-123",
  "turns": [
    {
      "turnIndex": 1,
      "speaker": "Customer",
      "transcript": "عايز أطلب برجر كبير",
      "timestamp": "2026-05-28T14:00:05Z",
      "sentiment": { "score": 0.6, "label": "Positive" }
    }
  ],
  "analysis": {
    "summary": "Customer ordered a large burger...",
    "mainTopics": ["Food Order"],
    "escalationRequired": false
  }
}
```

#### Get sentiment analytics for a business
```
GET /api/conversations/analytics?businessId={id}&from={date}&to={date}
Authorization: Bearer {owner_jwt}
```

**Response:**
```json
{
  "totalSessions": 42,
  "avgSentimentScore": 0.65,
  "sentimentBreakdown": {
    "Positive": 30,
    "Neutral": 8,
    "Negative": 4
  },
  "avgDurationSeconds": 480,
  "totalOrdersCreated": 35,
  "escalationRate": 0.05,
  "topIntents": [
    { "intent": "CreateOrder", "count": 35 },
    { "intent": "AskAboutProducts", "count": 20 }
  ]
}
```

---

## Channel 3 — Frontend ↔ AI (Direct, Separated)

The frontend and AI communicate directly for the **live audio experience only**. The backend is not in this path at all.

```
Frontend (Desktop App UI) ◄──── WebRTC or WebSocket ────► AI Engine
```

**What flows here:**
- Live audio stream (microphone input)
- Real-time transcript display (if UI shows captions)
- AI voice output (speaker playback)
- Connection status signals (started/ended meeting)

**What does NOT flow here:**
- Authentication tokens for the backend
- Database calls
- Business data (AI fetches that from backend directly using its own service token)

### Handshake between Frontend and AI:
```json
{
  "action": "start_session",
  "businessId": "biz-123",
  "aiServiceUrl": "ws://ai-engine.local:8765",
  "sessionToken": "{short_lived_token_from_backend}"
}
```

The frontend gets a short-lived session token from the backend (`POST /api/conversations/sessions/token?businessId={id}`), passes it to the AI app, and the AI uses it to authenticate its own REST calls to the backend.

---

## Authentication Model

| Caller | Token Type | How Obtained | Used For |
|--------|-----------|--------------|----------|
| AI App → Backend | Service Token (long-lived) | Pre-configured in AI app settings | Pushing conversation data, pulling context |
| Frontend → Backend | JWT (short-lived, 1h) | `POST /api/Auth/login` | All management APIs |
| Frontend → AI | Session Token (very short-lived, per session) | `POST /api/conversations/sessions/token` | Authorizing the AI to write to this session |

---

## What the Backend Stores (Data Model)

```
ConversationSession
├── SessionId (PK)
├── BusinessId (FK)
├── Channel ("VoiceDesktop", "WebChat")
├── Status ("Active", "Closed", "Escalated")
├── StartedAt
├── EndedAt
├── DurationSeconds
├── CustomerIdentifier (optional, phone/email/anonymous)
├── OverallSentimentScore
├── OverallSentimentLabel
├── EscalationRequired
├── CreatedAt
│
├── ConversationTurns[]
│   ├── TurnId (PK)
│   ├── SessionId (FK)
│   ├── TurnIndex
│   ├── Speaker ("Customer" | "Agent")
│   ├── Transcript
│   ├── AudioLengthMs
│   ├── Language
│   ├── Dialect
│   ├── Timestamp
│   ├── SentimentScore
│   ├── SentimentLabel
│   ├── IntentDetected
│   └── ActionTakenJson
│
└── ConversationAnalysis (one-to-one)
    ├── SessionId (FK)
    ├── Summary
    ├── SummaryAr
    ├── MainTopicsJson
    ├── IntentsDetectedJson
    ├── ActionsPerformedJson
    ├── KeyMomentsJson
    ├── EscalationReason
    └── ModelsUsedJson
```

---

## What Changed from V1

| Aspect | V1 | V2 |
|--------|----|----|
| **AI role** | Called by backend per message | Autonomous — runs its own pipeline |
| **Voice handling** | Backend received audio, forwarded to AI | AI owns the full audio pipeline |
| **Backend role** | Orchestrator (called AI for each step) | Data sink + context provider |
| **Intent Detection** | Backend called AI per message | AI handles internally, reports outcome |
| **Response Generation** | Backend called AI, sent result to frontend | AI speaks directly in the meeting |
| **Sentiment Analysis** | Backend called AI per message | AI computes and pushes to backend |
| **Frontend ↔ AI** | Not connected directly | Direct WebRTC/WS for live audio |
| **Storage trigger** | Backend stored after each message | AI pushes turns in batches or at end |

---

## Error Handling

| Scenario | AI Does | Backend Does |
|----------|---------|--------------|
| Backend unreachable | Continues conversation locally, queues data for retry | N/A |
| Context fetch fails | Uses cached context or safe defaults | Returns 503 with retry-after header |
| Turn push fails | Retries up to 3x with exponential backoff | Returns 429/503, AI queues |
| Session end fails | Retries, logs locally | Backend auto-closes stale sessions after 2h |

---

## Performance Targets

| Operation | Target |
|-----------|--------|
| Context pull | < 500ms |
| Turn push (per batch) | < 300ms |
| Session open/close | < 200ms |
| Analytics query (frontend) | < 1s |
