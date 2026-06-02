# Analysis Contract — Final (Backend ↔ AI)

> Last updated after full contract review + AI team Q&A.
> This file is the **single source of truth** for the Analysis integration.
> Ignore all previous contract files.

---

## Overview

Post-session analysis runs **after** a conversation ends.
The backend sends the full message history to the AI, which returns sentiment, intents, topics, and key moments.
Results are stored in the DB and surfaced on the business dashboard.

**Trigger:** Automatically called by the backend when `EndInteraction` is invoked.
**Mode:** Fire-and-forget background task — does not block the customer-facing flow.
**Batch size:** 1 session per request (current version).

---

## Endpoint

**Direction:** Backend → AI
`POST /api/v1/analysis/chat-batch`

---

## Request Body

```json
{
  "businessId": "string",
  "sessions": [
    {
      "sessionId": "string",
      "messages": [
        { "role": "customer",   "text": "عايز أطلب بيتزا" },
        { "role": "assistant",  "text": "تمام، أي نوع بيتزا تفضل؟" }
      ]
    }
  ]
}
```

### Field Notes

| Field | Notes |
|-------|-------|
| `businessId` | camelCase |
| `sessions[].sessionId` | Maps to `Interaction.InteractionId` in backend DB |
| `messages[].role` | Exactly `"customer"` or `"assistant"` — no other values accepted |
| `messages[].text` | Non-empty string — backend filters blank messages before sending |

### Backend Pre-send Rules

- Empty / whitespace messages are excluded before sending.
- If all messages are empty after filtering, the request is **not sent**.
- Messages are sent in **chronological order** (oldest first).
- If an analysis already exists for this `sessionId`, the backend skips storing the result (idempotent).

---

## Response Body

```json
{
  "businessId": "string",
  "results": [
    {
      "sessionId": "string",

      "summary": "string",
      "summaryAr": "string",

      "overallSentiment": {
        "score": 0.75,
        "label": "Positive"
      },

      "mainIntent": "CreateOrder",

      "intentsDetected": [
        { "name": "CreateOrder",       "count": 3 },
        { "name": "AskAboutProducts",  "count": 1 }
      ],

      "mainTopics": ["بيتزا", "توصيل"],

      "keyMoments": [
        "العميل طلب بيتزا مارجريتا",
        "تأكيد الطلب بنجاح"
      ]
    }
  ]
}
```

---

## Response Field Details

### Summaries

| Field | Notes |
|-------|-------|
| `summary` | English summary — concise, 1–3 sentences, max ~500 characters |
| `summaryAr` | Arabic summary — same length guideline |

### Sentiment

| Field | Type | Notes |
|-------|------|-------|
| `overallSentiment.score` | `double` | Range: `-1.0` (Very Negative) → `0.0` (Neutral) → `+1.0` (Very Positive) |
| `overallSentiment.label` | `string` | Exactly `"Positive"`, `"Neutral"`, or `"Negative"` |
| **Fallback** | — | If undetermined: `{ "score": 0.0, "label": "Neutral" }` |

### Intents

| Field | Notes |
|-------|-------|
| `mainIntent` | The dominant intent. Always a `string` — never `null`. Use `"Unknown"` if undetermined. |
| `mainIntent` value | Always equals `intentsDetected[0].name` |
| `intentsDetected` | Full distribution sorted by `count` descending |
| `intentsDetected[].name` | Must be one of the supported values listed below |
| `intentsDetected[].count` | Number of times this intent appeared in the session |
| **Fallback** | If undetermined: `"mainIntent": "Unknown"` + `[{ "name": "Unknown", "count": 1 }]` |

**Supported intent values:**

```
CreateOrder       ModifyOrder       CancelOrder
AskAboutProducts  AskAboutPrice     Complaint
RequestHumanAgent Compliment        Greeting
Farewell          GeneralQuestion   Unknown
```

### Topics & Key Moments

| Field | Type | Notes |
|-------|------|-------|
| `mainTopics` | `string[]` | Main topics discussed. Can be `[]` for very short or unclear sessions. |
| `keyMoments` | `string[]` | Important moments as human-readable sentences. Can be `[]` for trivial sessions (e.g., just a greeting). |

### Minimum valid response (1-message session — e.g. customer said "أهلا" and left)

```json
{
  "sessionId": "session-001",
  "summary": "Customer greeted the assistant but did not continue the conversation.",
  "summaryAr": "العميل ألقى التحية ولم يكمل المحادثة.",
  "overallSentiment": { "score": 0.0, "label": "Neutral" },
  "mainIntent": "Greeting",
  "intentsDetected": [{ "name": "Greeting", "count": 1 }],
  "mainTopics": [],
  "keyMoments": []
}
```

---

## Field Naming — camelCase (Critical)

**All response fields use camelCase.** Backend deserialization depends on exact field names.

| Correct ✅ | Wrong ❌ |
|-----------|---------|
| `businessId` | `BusinessId` |
| `sessionId` | `SessionId` |
| `summaryAr` | `SummaryAr` |
| `overallSentiment` | `OverallSentiment` |
| `mainIntent` | `MainIntent` |
| `intentsDetected` | `IntentsDetected` |
| `mainTopics` | `MainTopics` |
| `keyMoments` | `KeyMoments` |

> If PascalCase is returned, the backend will silently receive empty/default values and the dashboard will show no data.

---

## What the Backend Does with the Response

| AI Field | Stored in DB |
|----------|-------------|
| `summary` | `InteractionAnalysis.Summary` |
| `summaryAr` | `InteractionAnalysis.SummaryAr` |
| `overallSentiment.score` | `InteractionAnalysis.SentimentScore` |
| `overallSentiment.label` | `InteractionAnalysis.SentimentLabel` |
| `mainIntent` | `InteractionAnalysis.MainIntent` |
| `intentsDetected` | `InteractionAnalysis.IntentsDetectedJson` (JSON array) |
| `mainTopics` | `InteractionAnalysis.MainTopicsJson` (JSON array) |
| `keyMoments` | `InteractionAnalysis.KeyMomentsJson` (JSON array) |

### Dashboard Sections Powered by Analysis

| Dashboard section | Uses |
|------------------|------|
| Sentiment Analysis | `SentimentLabel`, `SentimentScore` |
| Chat Analysis → Top Intents | `IntentsDetectedJson` |
| Chat Analysis → Top Topics | `MainTopicsJson` |
| Chat Analysis → Top Key Moments | `KeyMomentsJson` |
| Chat Analysis → Recent Sessions | `Summary`, `SummaryAr`, `MainIntent`, `SentimentLabel`, `SentimentScore` |
