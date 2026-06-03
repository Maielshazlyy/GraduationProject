# Report Generation — Frontend Contract

---

## Endpoint

```
POST /api/businesses/{businessId}/reports/generate
Authorization: Bearer <token>
Content-Type: application/json
```

Replace `{businessId}` with the business ID from the logged-in owner's profile.

---

## Request Body

```json
{
  "from": "2026-06-01T00:00:00Z",
  "to": "2026-06-30T23:59:59Z",
  "language": "ar"
}
```

| Field | Type | Required | Description |
|-------|------|----------|-------------|
| `from` | ISO 8601 DateTime | Yes | Start of the report period |
| `to` | ISO 8601 DateTime | Yes | End of the report period |
| `language` | string | No | Language preference (default: `"ar"`) |

---

## Success Response — `200 OK`

```json
{
  "businessId": "d52f7fcc-8b2e-4b03-ac4c-c16760b80468",
  "period": {
    "from": "2026-06-01T00:00:00Z",
    "to": "2026-06-30T23:59:59Z"
  },
  "reportTitle": "Customer Interaction Analysis Report",
  "summary": "English summary of the report.",
  "summaryAr": "ملخص التقرير بالعربي.",
  "highlights": [
    "Average sentiment score is -0.5, indicating negative feedback.",
    "One complaint was recorded regarding a cold food issue."
  ],
  "highlightsAr": [
    "متوسط درجة الشعور -0.5، مما يدل على ملاحظات سلبية.",
    "تم تسجيل شكوى واحدة بخصوص مشكلة الطعام البارد."
  ],
  "problems": [
    {
      "title": "Cold Food Issue",
      "description": "A customer received their order cold, leading to dissatisfaction.",
      "severity": "medium",
      "evidence": [
        "One complaint about cold food was recorded."
      ]
    }
  ],
  "recommendations": [
    {
      "title": "Improve Food Temperature Control",
      "description": "Ensure that food is served at the correct temperature.",
      "priority": "high",
      "expectedImpact": "Reduce complaints and improve overall customer experience.",
      "suggestedOwner": "Kitchen Manager"
    },
    {
      "title": "Enhance Customer Service Training",
      "description": "Train staff to handle complaints effectively.",
      "priority": "medium",
      "expectedImpact": "Improve customer retention and satisfaction.",
      "suggestedOwner": "Customer Service Manager"
    }
  ],
  "suggestedActions": [
    "Review food preparation and delivery processes.",
    "Conduct staff training sessions on customer service.",
    "Implement a feedback loop to monitor food quality."
  ],
  "riskLevel": "medium"
}
```

### Response Fields

| Field | Type | Description |
|-------|------|-------------|
| `businessId` | string | The business this report belongs to |
| `period` | object | The date range of the report |
| `reportTitle` | string | Title of the report |
| `summary` | string | English summary |
| `summaryAr` | string | Arabic summary |
| `highlights` | string[] | Key highlights in English |
| `highlightsAr` | string[] | Key highlights in Arabic |
| `problems` | object[] | Detected problems |
| `problems[].title` | string | Problem title |
| `problems[].description` | string | Problem description |
| `problems[].severity` | string | `low` / `medium` / `high` / `critical` |
| `problems[].evidence` | string[] | Supporting evidence |
| `recommendations` | object[] | AI-generated recommendations |
| `recommendations[].title` | string | Recommendation title |
| `recommendations[].description` | string | Recommendation description |
| `recommendations[].priority` | string | `low` / `medium` / `high` / `critical` |
| `recommendations[].expectedImpact` | string | Expected impact if applied |
| `recommendations[].suggestedOwner` | string | Who should action this |
| `suggestedActions` | string[] | Quick action items |
| `riskLevel` | string | Overall risk: `low` / `medium` / `high` / `critical` |

---

## Error Responses

### No Analysis Data — `200 OK`
Returned when no interaction analyses exist for the selected period.
```json
{
  "message": "No analysis data available for this period.",
  "report": null
}
```
**UI:** Show a message — "لا توجد بيانات تحليل لهذه الفترة."

---

### Invalid Date Range — `400 Bad Request`
```json
{
  "message": "Invalid date range. 'from' must be before 'to'."
}
```
**UI:** Show validation error on the date range picker.

---

### Unauthorized — `401`
Token missing or expired. Redirect to login.

---

### Forbidden — `403`
Owner is trying to generate a report for a business that is not theirs.

---

### AI Service Unavailable — `502`
```json
{
  "message": "Report generation failed. Please try again.",
  "details": "..."
}
```
**UI:** Show error message with a retry button.

---

## UI Flow

1. Owner selects a **date range** (from / to)
2. Owner clicks **Generate Report**
3. Frontend sends `POST` request with the date range
4. Show **loading spinner** while waiting
5. On success, display the report sections:
   - Report Title
   - Arabic Summary (`summaryAr`)
   - Highlights (`highlightsAr`)
   - Problems (with severity badge)
   - Recommendations (with priority badge)
   - Suggested Actions
   - Risk Level badge
6. **Download PDF** button — generate PDF client-side using `jsPDF` or `pdfmake`, no extra backend call needed

---

## Notes

- The report is generated fresh on every request — no caching on the backend for MVP
- PDF generation happens **entirely client-side** — do not call any additional endpoint for PDF
- The `language` field is included in the request but the response always contains both Arabic and English fields
- The owner token must be passed in the `Authorization` header — the backend verifies that the token's business matches the `{businessId}` in the URL
- Response time may take a few seconds — always show a loading state
