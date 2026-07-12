# Frontend Dashboard Specification

This document defines what frontend should display based on the currently implemented backend APIs.

## 1) General

- **Auth**: Use JWT Bearer token in `Authorization` header.
- **Error handling**:
  - `401`: redirect to login
  - `403`: show not authorized page/state
  - `404`: show not found
  - `500`: show retry UI
- **Rendering rules**:
  - Handle null/optional fields safely.
  - Show `-` or `Not available yet` for missing values.
  - Use loading skeletons for cards/charts/tables.

---

## 2) Owner Dashboard (Business-scoped)

> Routes under `api/Dashboard/*`  
> Policy: `OwnerOrAdmin`  
> Business scope comes from token claim (`BusinessId`).

### 2.1 APIs

- `GET /api/Dashboard/summary`
- `GET /api/Dashboard/analytics`
- `GET /api/Dashboard/full`
- `GET /api/Dashboard/audit-logs/recent?count=20`
- `GET /api/Dashboard/audit-logs/statistics`
- `GET /api/Dashboard/audit-logs/customer/{customerId}`

### 2.2 UI Sections and Fields

1. **Business Header**
   - `businessName`
   - `businessType`

2. **Setup Status**
   - `isSetupComplete`
   - `setupStepsCompleted[]`
   - `setupStepsPending[]`

3. **Settings Snapshot**
   - `hasSettings`
   - `chatbotEnabled`
   - `welcomeMessage`
   - `agentVoice`

4. **Quick Stats**
   - `totalMenuItems`
   - `totalMenuCategories`
   - `totalKnowledgeBaseItems`
   - `totalFAQs`

5. **Analytics KPIs**
   - Orders: `totalOrders`, `totalRevenue`, `averageOrderValue`, `pendingOrders`, `completedOrders`
   - Customers: `totalCustomers`, `newCustomersLast30Days`
   - Tickets: `totalTickets`, `openTickets`, `closedTickets`, `inProgressTickets`, `averageTicketResolutionTime`
   - Feedback: `averageRating`, `totalFeedbacks`, `positiveFeedbacks`, `negativeFeedbacks`
   - Sentiment: `positiveSentiments`, `negativeSentiments`, `neutralSentiments`, `averageSentimentScore`
   - Interactions: `totalInteractions`, `activeInteractions`
   - Menu: `totalMenuItems`, `availableMenuItems`

6. **Recent Activity**
   - `lastKnowledgeBaseUpdate`
   - `lastOrderDate`
   - `lastTicketDate`
   - `lastFeedbackDate`
   - `lastAuditLogDate`

7. **Audit Logs**
   - Table fields:
     - `action`
     - `entity`
     - `entityId`
     - `createdAt`
     - `userName`
   - Statistics widgets:
     - `totalActions`
     - `actionsLast24Hours`
     - `actionsLast7Days`
     - `actionsLast30Days`
     - `actionsByEntity`
     - `actionsByType`
     - `mostActiveUsers`
     - `recentCriticalActions`

---

## 3) Admin Dashboard (Platform-scoped)

> Routes under `api/AdminDashboard/*`  
> Policy: `AdminOnly`  
> Scope: all businesses across platform.

### 3.1 APIs (Implemented)

#### Core
- `GET /api/AdminDashboard/summary`
- `GET /api/AdminDashboard/top-businesses?count=10`
- `GET /api/AdminDashboard/full?topBusinessesCount=10`

#### Monitoring and Charts
- `GET /api/AdminDashboard/alerts`
- `GET /api/AdminDashboard/revenue-trend?months=12`
- `GET /api/AdminDashboard/orders-by-status`
- `GET /api/AdminDashboard/tickets-by-priority`
- `GET /api/AdminDashboard/business-health?top=20&sort=desc`
- `GET /api/AdminDashboard/sentiment-trend?days=30`

#### Admin Actions
- `POST /api/AdminDashboard/business/{businessId}/suspend`
- `POST /api/AdminDashboard/business/{businessId}/activate`
- `POST /api/AdminDashboard/business/{businessId}/verify`
- `POST /api/AdminDashboard/business/{businessId}/unverify`

### 3.2 UI Sections and Fields

1. **Platform Overview Cards** (from `summary`)
   - Businesses:
     - `totalBusinesses`
     - `activeBusinesses`
     - `newBusinessesLast30Days`
   - Orders/Revenue:
     - `totalOrders`
     - `totalRevenue`
     - `pendingOrders`
     - `completedOrders`
     - `cancelledOrders`
   - Tickets/Interactions:
     - `totalTickets`
     - `openTickets`
     - `escalatedTickets`
     - `totalInteractions`
     - `activeInteractions`
   - Experience:
     - `totalFeedbacks`
     - `averageRating`
     - `positiveSentiments`
     - `negativeSentiments`
     - `neutralSentiments`
     - `averageSentimentScore`
   - Audit:
     - `totalAuditLogs`
     - `auditLogsLast24Hours`
     - `lastAuditLogDate`
   - Recency:
     - `lastOrderDate`
     - `lastTicketDate`
     - `lastFeedbackDate`

2. **Top Businesses Table** (from `top-businesses`)
   - `businessId`
   - `businessName`
   - `isActive`
   - `ordersCount`
   - `revenue`
   - `openTicketsCount`
   - `customersCount`

3. **Alerts Panel** (from `alerts`)
   - `id`
   - `type`
   - `severity` (`Critical`, `High`, `Medium`, `Low`)
   - `businessId`
   - `businessName`
   - `message`
   - `createdAt`

4. **Charts**
   - Revenue trend (`revenue-trend`):
     - `period` (format: `yyyy-MM`)
     - `revenue`
     - `ordersCount`
   - Orders by status (`orders-by-status`):
     - `status`
     - `count`
   - Tickets by priority (`tickets-by-priority`):
     - `priority`
     - `count`
   - Sentiment trend (`sentiment-trend`):
     - `date` (format: `yyyy-MM-dd`)
     - `positive`
     - `negative`
     - `neutral`

5. **Business Health Ranking** (from `business-health`)
   - `businessId`
   - `businessName`
   - `healthScore` (0-100)
   - `averageRating`
   - `negativeSentimentRatio`
   - `cancellationRate`
   - `openTicketsCount`
   - `escalatedTicketsCount`
   - Notes:
     - `sort=desc` => best health first
     - `sort=asc` => worst health first

6. **Moderation Actions**
   - Suspend/Activate business
   - Verify/Unverify business
   - Refresh table/cards after success
   - Show success toast + graceful error toast

---

## 4) Suggested Frontend Page Map

### Owner App
- `OwnerDashboardPage`
  - source: `GET /api/Dashboard/full`
- `OwnerActivityPage`
  - source: recent logs + statistics
- `OwnerCustomerActivityPage`
  - source: `GET /api/Dashboard/audit-logs/customer/{customerId}`

### Admin App
- `AdminOverviewPage`
  - source: summary + top businesses + alerts
- `AdminAnalyticsPage`
  - source: revenue/order/ticket/sentiment chart endpoints
- `AdminHealthPage`
  - source: business-health
- `AdminModerationPage`
  - source: top businesses + action endpoints

---

## 5) Customer-Facing APIs (Optional for shared frontend)

### Chat
- `GET /api/CustomerChat/capabilities/{businessId}`
- `POST /api/CustomerChat/message`
- `POST /api/CustomerChat/recommendations`

### Voice (current REST placeholder)
- `POST /api/CustomerVoice/session/initialize`
- `POST /api/CustomerVoice/message`
- `POST /api/CustomerVoice/feedback`
- `POST /api/CustomerVoice/interaction/{interactionId}/interrupt`
- `GET /api/CustomerVoice/settings/{businessId}`

---

## 6) QA Checklist for Frontend

- [ ] Token attached to all protected endpoints.
- [ ] Handles 401/403/404/500 states.
- [ ] Handles null values without crash.
- [ ] Zero-data charts render correctly.
- [ ] Admin actions update UI state after success.
- [ ] Pagination/filter behavior tested for log tables.

