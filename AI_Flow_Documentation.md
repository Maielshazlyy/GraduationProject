# Voice AI Agent — Flow & Integration Notes

**For:** Backend Team

---

## 1. Overview

This document describes the current Python AI agent: its files, the call flow, and the parts the Backend will need to handle once the AI is deployed to a server.

---

## 2. Project Files

| File | Role |
|------|------|
| `main.py` | Entry point. Starts the AI agent. |
| `config.py` | Configuration (API keys, audio settings). |
| `agent_responder.py` | Core engine: handles the live call (WebSocket with OpenAI, tool calling, audio streaming, event handling). |
| `call_logger.py` | Collects all conversation turns and audio during the call. |
| `call_analyzer.py` | After the call ends, sends the transcript to GPT-4o-mini and produces structured analysis (summary, sentiment, intents, actions, key moments). |
| `backend_uploader.py` | Sends the final data (transcript + analysis + audio files) to the Backend via `POST /api/calls`. Retries on failure. |
| `create_index_new.py` | One-time script: builds the FAISS vector index from `menu.json`. |
| `menu.json` | Source menu data for the restaurant. |
| `faiss_menu_index/` | Auto-generated vector database used for menu lookups (RAG). |
| `calls/` | Auto-generated folder containing saved call data and audio. |
| `pending_uploads/` | Auto-generated folder for calls that failed to upload (retried later). |

---

## 3. Call Flow

### Phase 1: Startup

1. `main.py` runs `AgentResponder.start()`.
2. The agent loads the FAISS menu index (for RAG).
3. The agent initializes `CallLogger`, `CallAnalyzer`, and `BackendUploader`.
4. The agent opens a WebSocket connection to the OpenAI Realtime API.
5. The agent sends initial session config: voice settings, instructions, and the `search_menu` tool definition.

### Phase 2: During the Call

For every customer turn:

1. Customer's audio is captured and streamed to OpenAI.
2. OpenAI transcribes it → `call_logger.add_customer_message()` stores the transcript.
3. The AI decides what to do:
   - If the question is about the menu/prices/items → it calls the `search_menu` tool.
   - The agent queries the local FAISS index and sends the result back.
4. OpenAI generates the spoken response.
5. The audio response is streamed back and played.
6. The response transcript is saved → `call_logger.add_agent_message()`.

All audio chunks (customer + AI) are saved by `call_logger` throughout the call.

### Phase 3: After the Call Ends

When the call ends, `_finalize_call()` runs:

1. **Save** — `call_logger.save()` writes:
   - `transcript.json` (all turns + metadata)
   - `customer.wav`, `agent.wav`, `full_call_stereo.wav` (audio files)

2. **Analyze** — `call_analyzer.analyze()` sends the transcript to GPT-4o-mini and returns:
   - `summary` (EN + AR)
   - `overallSentiment`
   - `mainTopics`
   - `intentsDetected`
   - `actionsPerformed`
   - `escalationRequired`
   - `keyMoments`
   - `modelsUsed`

3. **Upload** — `backend_uploader.upload()` sends everything to the Backend via `POST /api/calls`:
   - `call_data` (JSON)
   - `analysis` (JSON)
   - 3 WAV files
   
   If upload fails → saved to `pending_uploads/` for later retry.

---

## 4. Data Sent to Backend

**Endpoint:** `POST /api/calls`

**Format:** `multipart/form-data`

| Field | Type | Content |
|-------|------|---------|
| `call_data` | JSON string | Full call metadata + all turns |
| `analysis` | JSON string | GPT analysis output |
| `uploaded_at` | string | ISO 8601 timestamp |
| `customer_audio` | WAV file | Customer voice only |
| `agent_audio` | WAV file | AI voice only |
| `stereo_audio` | WAV file | Combined stereo (L=customer, R=AI) |

The exact structure of `call_data` and `analysis` is in the JSON sample previously shared with the team.

---

## 5. What Backend Will Need to Handle on Server Deployment

When the AI moves from the local machine to a server, several things that currently work locally will require Backend support:

### 5.1 Call Triggering

**Local now:** The AI runs once per session, started manually.

**On server:** The Backend must tell the AI when to start a new call. The AI will need an endpoint such as:

```
POST /start_call
{
  "businessId": "...",
  "customerId": "...",
  "meetingLink": "..."
}
```

### 5.2 Multi-Tenancy (Business + Customer IDs)

**Local now:** No `businessId` or `customerId` — single restaurant, single operator.

**On server:** Backend must:

- Pass `businessId` with each call request.
- Pass `customerId` (or caller identifier) with each call request.
- Provide a way for the AI to load the correct menu / FAISS index per business.

### 5.3 Joining the Call

**Local now:** A human operator opens Teams and joins the meeting before starting the AI.

**On server:** No human operator exists. The Backend must handle joining the meeting on behalf of the AI and route the audio stream to/from the AI.

### 5.4 Audio Stream Source

**Local now:** Audio comes from the operator's machine.

**On server:** Audio must reach the AI through an API or stream provided by the Backend (since servers have no physical audio devices).

### 5.5 Authentication

**Local now:** No auth required between AI and Backend (testing only).

**On server:** Backend will need to issue an API token / service credential for the AI to authenticate when uploading data and receiving instructions.

### 5.6 Lifecycle Management

**Local now:** AI runs as a single process and shuts down with Ctrl+C.

**On server:** Backend must manage:
- Starting AI instances for new calls.
- Stopping them when calls end.
- Handling failures and restarts.

---

## 6. Summary

- **Currently working locally:** Full pipeline (call → transcript → analysis → upload).
- **What stays the same on server:** Core logic of `agent_responder.py`, `call_logger.py`, `call_analyzer.py`, `backend_uploader.py`.
- **What Backend needs to handle:** Call initiation, audio routing, tenant context (business + customer IDs), authentication, and lifecycle management.
