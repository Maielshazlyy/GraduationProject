# AI Integration Plan — Digital Employee Backend
## Objective: Connect .NET Backend ↔ AI FastAPI Server ASAP

**AI Server URL:** `https://anyway-remix-puzzling.ngrok-free.dev`  
**Status confirmed:** `/metrics` (GET) is live under "Health" section  
**Your backend has 4 placeholder services that need real HTTP calls to that server.**

---

## 🔴 What's Broken Right Now (The 4 Placeholders)

| Service File | Problem | Impact |
|---|---|---|
| `IntentDetectionService.cs` | Keyword-only detection — no AI | Wrong intents, broken ordering flow |
| `ResponseGenerationService.cs` | Returns `"[AI placeholder]"` string | Customers see garbage text |
| `SentimentService.cs` | Not implemented | Sentiment never analyzed |
| `CustomerVoiceService.cs` | STT/TTS not wired | Voice channel completely broken |

---

## ✅ Step 1 — Confirm AI Endpoint Paths (30 minutes)

Before writing code, open the live Swagger UI and note every endpoint:

```
https://anyway-remix-puzzling.ngrok-free.dev/docs
```

You need to confirm the **exact paths** for these 5 contracts:

| Contract | Expected path (confirm) | Method |
|---|---|---|
| Intent Detection | `/detect-intent` or `/intent` | POST |
| Response Generation | `/generate-response` or `/respond` | POST |
| Sentiment Analysis | `/analyze-sentiment` or `/sentiment` | POST |
| Speech-to-Text | `/speech-to-text` or `/stt` | POST |
| Text-to-Speech | `/text-to-speech` or `/tts` | POST |

> Also download `/openapi.json` to see exact request/response schemas.

---

## ✅ Step 2 — Add AI Config to appsettings.json (5 minutes)

Add this block to `digital employee/appsettings.json`:

```json
"AI": {
    "BaseUrl": "https://anyway-remix-puzzling.ngrok-free.dev",
    "TimeoutSeconds": 10
}
```

And add to `render.yaml` env vars:
```yaml
- key: AI__BaseUrl
  sync: false   # set in Render dashboard — will change if ngrok URL changes
```

> ⚠️ The ngrok-free URL changes every time. For production, the AI team must deploy  
> their server to a stable URL (Railway, Render, Hugging Face Spaces, etc.)

---

## ✅ Step 3 — Register HttpClient in Program.cs (10 minutes)

In `Program.cs`, after the DI registrations section, add:

```csharp
// ── AI API Client ──────────────────────────────────────
var aiBaseUrl = builder.Configuration["AI:BaseUrl"]
    ?? throw new InvalidOperationException("AI:BaseUrl is not configured");

builder.Services.AddHttpClient("AiApi", client =>
{
    client.BaseAddress = new Uri(aiBaseUrl);
    client.Timeout = TimeSpan.FromSeconds(
        builder.Configuration.GetValue<int>("AI:TimeoutSeconds", 10));
    client.DefaultRequestHeaders.Add("Accept", "application/json");
});

builder.Services.AddScoped<IAiApiClient, AiApiClient>();
```

---

## ✅ Step 4 — Create AiApiClient (New File, ~100 lines)

**File:** `Service layer/Services/AiApiClient.cs`

This single class handles ALL calls to the AI FastAPI server.

```csharp
// Interface
public interface IAiApiClient
{
    Task<DetectedIntentResultDTO> DetectIntentAsync(string businessId, string interactionId, IEnumerable<string> messages);
    Task<string> GenerateResponseAsync(ResponseGenerationContextDTO context);
    Task<(double score, string label)> AnalyzeSentimentAsync(string text, string language);
    Task<string> SpeechToTextAsync(string audioBase64, string audioFormat);
    Task<(string audioBase64, string audioFormat)> TextToSpeechAsync(string text, VoiceSettingsDTO settings, string? dialect);
}

// Implementation
public class AiApiClient : IAiApiClient
{
    private readonly HttpClient _http;
    
    public AiApiClient(IHttpClientFactory factory)
    {
        _http = factory.CreateClient("AiApi");
    }
    
    public async Task<DetectedIntentResultDTO> DetectIntentAsync(...)
    {
        // POST /detect-intent
        var payload = new { business_id = businessId, interaction_id = interactionId, messages = messages };
        var resp = await _http.PostAsJsonAsync("/detect-intent", payload);
        resp.EnsureSuccessStatusCode();
        var result = await resp.Content.ReadFromJsonAsync<DetectedIntentResultDTO>();
        return result ?? new DetectedIntentResultDTO { Intent = "GeneralQuestion", Confidence = 0.0 };
    }
    
    // ... same pattern for the other 4 methods
}
```

---

## ✅ Step 5 — Replace IntentDetectionService (15 minutes)

**File:** `Service layer/Services/IntentDetectionService.cs`

Replace the entire keyword logic with:

```csharp
public class IntentDetectionService : IIntentDetectionService
{
    private readonly IAiApiClient _ai;
    
    public IntentDetectionService(IAiApiClient ai) => _ai = ai;
    
    public Task<DetectedIntentResultDTO> DetectIntentAsync(
        string businessId, string interactionId, IEnumerable<string> recentMessages)
    {
        // Delegate 100% to AI server
        return _ai.DetectIntentAsync(businessId, interactionId, recentMessages);
    }
}
```

---

## ✅ Step 6 — Replace ResponseGenerationService (15 minutes)

**File:** `Service layer/Services/ResponseGenerationService.cs`

```csharp
public class ResponseGenerationService : IResponseGenerationService
{
    private readonly IAiApiClient _ai;
    
    public ResponseGenerationService(IAiApiClient ai) => _ai = ai;
    
    public Task<string> GenerateResponseAsync(ResponseGenerationContextDTO context)
    {
        return _ai.GenerateResponseAsync(context);
    }
}
```

---

## ✅ Step 7 — Implement SentimentService (15 minutes)

**File:** `Service layer/Services/SentimentService.cs`  
Check what this currently contains, then update `AnalyzeSentimentAsync` to call `_ai.AnalyzeSentimentAsync(...)` and map the result to the `Sentiment` domain model.

---

## ✅ Step 8 — Wire CustomerVoiceService STT/TTS (30 minutes)

**File:** `Service layer/Services/CustomerVoiceService.cs`

Replace the STT placeholder with `_ai.SpeechToTextAsync(audioBase64, audioFormat)` and the TTS placeholder with `_ai.TextToSpeechAsync(text, settings, dialect)`.

---

## ✅ Step 9 — Register AiApiClient in DI (5 minutes)

In Program.cs, replace:
```csharp
builder.Services.AddScoped<IIntentDetectionService, IntentDetectionService>();
builder.Services.AddScoped<IResponseGenerationService, ResponseGenerationService>();
```
with the same lines **plus** the new `IAiApiClient`:
```csharp
builder.Services.AddScoped<IAiApiClient, AiApiClient>();
// existing lines stay the same — they now inject IAiApiClient via constructor
```

---

## ✅ Step 10 — End-to-End Test (30 minutes)

Using Postman or Swagger at `https://digital-employee-api.onrender.com/swagger`:

### Test 1: Chat flow
```json
POST /api/CustomerChat/message
{
  "businessId": "<your-test-business-id>",
  "channel": "WebChat",
  "message": "عايز أطلب برجر كبير"
}
```
**Expected:** `replyText` is a natural Arabic sentence (not the placeholder string)

### Test 2: Sentiment
```json
POST /api/Sentiment/analyze
{
  "messageId": "test-001",
  "text": "الخدمة ممتازة شكراً",
  "language": "ar"
}
```
**Expected:** `{ "label": "Positive", "score": > 0.3 }`

### Test 3: Intent detection
```json
POST /api/CustomerChat/message
{
  "businessId": "<id>",
  "message": "I want to speak with a human agent"
}
```
**Expected:** `ticketId` is returned (escalation triggered)

---

## 🚨 Key Risk: Ngrok URL Will Change

The URL `anyway-remix-puzzling.ngrok-free.dev` is a **free ngrok tunnel** — it resets every time the AI team restarts their server. This will break production.

**Fix (ask AI team to do ONE of these):**
1. Use **ngrok paid** with a fixed subdomain (`myapp.ngrok.io`) — $8/month
2. Deploy AI server to **Render.com** (free tier) — permanent URL
3. Deploy to **Railway.app** (free tier) — permanent URL
4. Deploy to **Hugging Face Spaces** (free for Python/FastAPI) — permanent URL

Until then, store the URL in Render env var `AI__BaseUrl` and update it whenever it changes.

---

## 📋 Claude Code Prompt to Use

Open Claude Code in your terminal at the project root and paste this:

```
I have a .NET 10 solution at the current directory. I need you to integrate 4 placeholder service files with a real AI FastAPI server.

AI server base URL comes from config key "AI:BaseUrl".
The 4 services to replace are:
1. "Service layer/Services/IntentDetectionService.cs" - replace keyword logic with HTTP POST to /detect-intent
2. "Service layer/Services/ResponseGenerationService.cs" - replace placeholder with HTTP POST to /generate-response  
3. "Service layer/Services/SentimentService.cs" - implement AnalyzeSentimentAsync with HTTP POST to /analyze-sentiment
4. "Service layer/Services/CustomerVoiceService.cs" - wire ConvertAudioToTextAsync (POST /speech-to-text) and ConvertTextToAudioAsync (POST /text-to-speech)

First create "Service layer/Services/AiApiClient.cs" with interface IAiApiClient and class AiApiClient using IHttpClientFactory.
Then register the HttpClient in "digital employee/Program.cs" using builder.Configuration["AI:BaseUrl"].
Then update each of the 4 services to inject IAiApiClient and delegate to it.
All AI calls must have try/catch — on failure, IntentDetection returns GeneralQuestion with Confidence 0.0, ResponseGeneration returns a fallback string in Arabic/English, Sentiment returns Neutral with Score 0.0.

The DTO types are in "Service layer/DTOS/Chat/CustomerChatDTOs.cs" - use them directly.
```

---

## ⏱ Time Estimate

| Step | Time |
|---|---|
| Confirm AI endpoint paths from /docs | 30 min |
| Steps 2–9 (code changes) | 2–3 hours |
| Testing | 1 hour |
| **Total** | **~4 hours** |

With Claude Code doing the coding, steps 2–9 can be done in **under 1 hour**.
