# AI → Backend: Analysis Contract
# What the AI sends, how the backend stores and computes it

**Version:** 1.0  
**Last Updated:** 2026-05-28  
**Audience:** AI Team + Backend Team

---

## Overview

The AI app runs the full voice pipeline autonomously. Once a conversation ends (or at defined intervals), the AI **pushes all analysis data to the backend**. The backend stores it, aggregates it, and makes it queryable by the frontend.

The backend does **not** run its own sentiment or NLP — all intelligence comes from the AI. The backend is purely a structured store.

---

## What the AI Computes and Sends

The AI is responsible for producing:

| Data | When Sent | Who Computes It |
|------|-----------|-----------------|
| Turn transcripts (customer + agent) | During or after session | AI (STT) |
| Per-turn sentiment score + label | With each turn | AI (sentiment model) |
| Per-turn intent detected | With each turn | AI (intent model) |
| Per-turn action taken (order, ticket…) | With each turn | AI (extracted from business logic result) |
| Session-level summary (text) | On session end | AI (LLM summarization) |
| Session-level overall sentiment | On session end | AI (aggregated from turns) |
| Main topics list | On session end | AI (topic extraction) |
| Key moments list | On session end | AI (LLM highlights) |
| Escalation flag + reason | On session end or mid-session | AI (escalation model) |
| Models used | On session end | AI (telemetry) |

---

## Payload: Single Turn

Sent via `POST /api/conversations/sessions/{sessionId}/turns`

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
        "score": 0.55,
        "label": "Positive"
      },
      "intentDetected": "CreateOrder",
      "confidence": 0.93,
      "actionTaken": null
    },
    {
      "turnIndex": 2,
      "timestamp": "2026-05-28T14:00:09Z",
      "speaker": "Agent",
      "transcript": "تمام! سجلت لك برجر كبير. عايز تضيف بطاطس أو مشروب؟",
      "audioLengthMs": 3200,
      "language": "ar",
      "dialect": "Egyptian",
      "sentiment": null,
      "intentDetected": null,
      "confidence": null,
      "actionTaken": {
        "type": "OrderCreated",
        "referenceId": "ord-456",
        "details": {
          "items": ["Big Burger"],
          "totalPrice": 50,
          "currency": "EGP"
        }
      }
    }
  ]
}
```

### Field Rules

| Field | Type | Required | Notes |
|-------|------|----------|-------|
| `turnIndex` | int | ✅ | Sequential from 1, never repeated in a session |
| `timestamp` | ISO 8601 | ✅ | UTC |
| `speaker` | `"Customer"` or `"Agent"` | ✅ | Exactly these two values |
| `transcript` | string | ✅ | Plain text, no markdown |
| `audioLengthMs` | int | ✅ | Duration of the audio clip for this turn |
| `language` | `"ar"` or `"en"` | ✅ | ISO 639-1 |
| `dialect` | string | ❌ | `"Egyptian"`, `"Levantine"`, `"Standard Arabic"`, `"English"` |
| `sentiment.score` | float -1.0 to 1.0 | ✅ if speaker=Customer | Null for Agent turns |
| `sentiment.label` | `"Positive"` `"Negative"` `"Neutral"` | ✅ if speaker=Customer | Null for Agent turns |
| `intentDetected` | string | ✅ if speaker=Customer | See intent list below. Null for Agent turns |
| `confidence` | float 0.0 to 1.0 | ✅ if intentDetected set | Intent confidence score |
| `actionTaken` | object | ❌ | Only on Agent turns when a business action happened |
| `actionTaken.type` | string | ✅ if present | See action types below |
| `actionTaken.referenceId` | string | ✅ if present | The created entity ID (orderId, ticketId…) |
| `actionTaken.details` | object | ❌ | Free-form context about the action |

### Supported Intent Values

```
CreateOrder
ModifyOrder
CancelOrder
AskAboutOrderStatus
AskAboutProducts
AskAboutPrice
AskAboutWorkingHours
AskAboutLocation
Complaint
RequestHumanAgent
Compliment
GeneralQuestion
Greeting
Farewell
Unknown
```

### Supported Action Types

```
OrderCreated
OrderModified
OrderCancelled
TicketCreated
TicketUpdated
FAQAnswered
EscalatedToHuman
RecommendationMade
```

---

## Payload: Session End Analysis

Sent via `POST /api/conversations/sessions/{sessionId}/end`

```json
{
  "endedAt": "2026-05-28T14:12:30Z",
  "durationSeconds": 750,
  "analysis": {
    "summary": "Customer placed an order for a large burger. AI upsold fries successfully. Customer was satisfied throughout. No escalation needed.",
    "summaryAr": "العميل طلب برجر كبير. تم عرض الإضافات ووافق العميل على البطاطس. العميل كان راضياً طوال المحادثة. لم تكن هناك حاجة للتصعيد.",
    "overallSentiment": {
      "score": 0.72,
      "label": "Positive",
      "breakdown": {
        "positive": 8,
        "neutral": 3,
        "negative": 1
      }
    },
    "mainTopics": [
      "Food Order",
      "Upsell — Sides",
      "Pricing"
    ],
    "intentsDetected": [
      { "intent": "CreateOrder", "count": 2 },
      { "intent": "AskAboutPrice", "count": 1 },
      { "intent": "Farewell", "count": 1 }
    ],
    "actionsPerformed": [
      {
        "type": "OrderCreated",
        "referenceId": "ord-456",
        "turnIndex": 2
      },
      {
        "type": "RecommendationMade",
        "referenceId": null,
        "turnIndex": 4
      }
    ],
    "escalationRequired": false,
    "escalationReason": null,
    "keyMoments": [
      {
        "turnIndex": 2,
        "description": "Order placed — Big Burger 50 EGP"
      },
      {
        "turnIndex": 4,
        "description": "Customer accepted upsell — Fries added"
      }
    ],
    "modelsUsed": [
      "whisper-large-v3",
      "gpt-4o",
      "tts-1-hd"
    ],
    "languageSwitches": 0,
    "avgCustomerTurnLengthMs": 2100,
    "avgAgentResponseDelayMs": 800
  }
}
```

### Analysis Field Rules

| Field | Type | Required | Notes |
|-------|------|----------|-------|
| `summary` | string | ✅ | English, 1-3 sentences |
| `summaryAr` | string | ✅ | Arabic, 1-3 sentences |
| `overallSentiment.score` | float | ✅ | Average of all customer turn scores |
| `overallSentiment.label` | string | ✅ | Majority label across customer turns |
| `overallSentiment.breakdown` | object | ✅ | Count of turns per label |
| `mainTopics` | string[] | ✅ | 1-5 topics, human readable |
| `intentsDetected` | array | ✅ | All unique intents and how many times each appeared |
| `actionsPerformed` | array | ✅ | All actions the agent took during the session |
| `escalationRequired` | bool | ✅ | |
| `escalationReason` | string | ❌ | Required if `escalationRequired: true` |
| `keyMoments` | array | ✅ | 1-5 notable moments with turn reference |
| `modelsUsed` | string[] | ✅ | All model IDs used during the session |
| `languageSwitches` | int | ✅ | How many times language changed mid-session |
| `avgCustomerTurnLengthMs` | int | ✅ | Average audio length of customer turns |
| `avgAgentResponseDelayMs` | int | ✅ | Average time from end of customer speech to start of agent response |

---

## How the Backend Stores It

### Database Tables

```
ConversationSessions
  SessionId           UNIQUEIDENTIFIER  PK
  BusinessId          UNIQUEIDENTIFIER  FK → Businesses
  Channel             NVARCHAR(50)      'VoiceDesktop' | 'WebChat'
  Status              NVARCHAR(20)      'Active' | 'Closed' | 'Escalated'
  StartedAt           DATETIME2
  EndedAt             DATETIME2
  DurationSeconds     INT
  CustomerIdentifier  NVARCHAR(200)     nullable (phone, email, or null for anon)
  OverallSentimentScore FLOAT
  OverallSentimentLabel NVARCHAR(20)
  EscalationRequired  BIT
  EscalationReason    NVARCHAR(500)
  SummaryEn           NVARCHAR(1000)
  SummaryAr           NVARCHAR(1000)
  MainTopicsJson      NVARCHAR(MAX)
  KeyMomentsJson      NVARCHAR(MAX)
  ModelsUsedJson      NVARCHAR(MAX)
  LanguageSwitches    INT
  AvgCustomerTurnMs   INT
  AvgAgentDelayMs     INT
  CreatedAt           DATETIME2
  UpdatedAt           DATETIME2

ConversationTurns
  TurnId              UNIQUEIDENTIFIER  PK
  SessionId           UNIQUEIDENTIFIER  FK → ConversationSessions
  TurnIndex           INT
  Speaker             NVARCHAR(20)      'Customer' | 'Agent'
  Transcript          NVARCHAR(MAX)
  AudioLengthMs       INT
  Language            NVARCHAR(10)
  Dialect             NVARCHAR(50)
  Timestamp           DATETIME2
  SentimentScore      FLOAT             nullable
  SentimentLabel      NVARCHAR(20)      nullable
  IntentDetected      NVARCHAR(50)      nullable
  IntentConfidence    FLOAT             nullable
  ActionTakenJson     NVARCHAR(MAX)     nullable

ConversationIntentStats  (aggregated per session close, for fast queries)
  StatId              UNIQUEIDENTIFIER  PK
  SessionId           UNIQUEIDENTIFIER  FK
  BusinessId          UNIQUEIDENTIFIER  FK
  Intent              NVARCHAR(50)
  Count               INT
  Date                DATE
```

### Aggregation on Session Close

When `POST /sessions/{id}/end` is received, the backend runs:

1. Saves all fields from the analysis payload into `ConversationSessions`
2. Upserts rows in `ConversationIntentStats` (one row per unique intent in this session)
3. Updates the business's running sentiment average in `BusinessAnalytics` cache table
4. Creates a `Ticket` record if `escalationRequired: true`

---

## Mid-Session Escalation

If the AI needs to escalate **during** a session (not waiting for end):

```
POST /api/conversations/sessions/{sessionId}/escalate
Authorization: Bearer {service_token}

{
  "escalatedAt": "2026-05-28T14:07:00Z",
  "urgency": "High",
  "reason": "Customer requesting refund — AI cannot authorize",
  "lastCustomerTranscript": "عايز أرجع فلوسي، الطلب غلط",
  "turnIndex": 9
}
```

Backend immediately:
- Updates session `Status` → `"Escalated"`
- Creates a `Ticket` with priority=High linked to the session
- (Future) Sends notification to agent dashboard

---

## Error Cases and Fallbacks

| Scenario | AI Must Do | Backend Behavior |
|----------|-----------|-----------------|
| Can't compute sentiment for a turn | Send `sentiment: null` | Stored as null, excluded from averages |
| Can't identify intent | Send `intentDetected: "Unknown"` with `confidence: 0.0` | Counted under "Unknown" in stats |
| Session end fails (500) | Retry after 30s, then 60s, then 120s | Returns 503 with `Retry-After` header |
| Session end never arrives | Backend auto-closes sessions open > 4 hours | Status set to "AutoClosed", analysis fields null |
| Backend unreachable | AI queues data locally, retries on reconnect | N/A |

---

## Versioning

The AI must send the contract version in every request header:

```
X-AI-Contract-Version: 2.0
```

If the backend receives an unknown version it returns `400` with:
```json
{ "error": "Unsupported contract version", "supportedVersions": ["2.0"] }
```
