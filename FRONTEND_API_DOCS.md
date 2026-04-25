## Frontend API Reference (from Postman)

- **Source**: `DigitalEmployee_Postman_Collection.json`
- **Base URL**: `{{baseUrl}}`
- **Auth**: Most protected endpoints require `Authorization: Bearer {{token}}`.

> Note: Postman collections usually don't store example responses.
> This document includes request bodies from the collection, and **best-effort response examples** (based on URL/method + common DTO patterns).
> For exact response fields, use Swagger or capture real responses from a running backend.

## 01 - Auth

### Register Admin [PUBLIC]

- **Method**: `POST`
- **URL**: `{{baseUrl}}/api/Auth/register-admin`
- **Body example**:

```json
{
  "fullName": "Super Admin",
  "email": "admin@app.com",
  "password": "Admin@123"
}
```
- **Response example**:

```json
{
  "message": "User registered successfully.",
  "userId": "<userId>"
}
```

### Register Owner [PUBLIC]

- **Method**: `POST`
- **URL**: `{{baseUrl}}/api/Auth/register-owner`
- **Body example**:

```json
{
  "fullName": "Business Owner",
  "email": "owner@app.com",
  "password": "Owner@123"
}
```
- **Response example**:

```json
{
  "message": "User registered successfully.",
  "userId": "<userId>"
}
```

### Register Agent [PUBLIC - requires businessId]

- **Method**: `POST`
- **URL**: `{{baseUrl}}/api/Auth/register`
- **Body example**:

```json
{
  "fullName": "Ahmed Agent",
  "email": "agent@app.com",
  "password": "Agent@123",
  "businessId": "{{businessId}}"
}
```
- **Response example**:

```json
{
  "message": "User registered successfully.",
  "userId": "<userId>"
}
```

### Login

- **Method**: `POST`
- **URL**: `{{baseUrl}}/api/Auth/login`
- **Body example**:

```json
{
  "email": "admin@app.com",
  "password": "Admin@123"
}
```
- **Response example**:

```json
{
  "token": "<jwt>",
  "expiresInMinutes": 60,
  "user": {
    "id": "<userId>",
    "fullName": "Business Owner",
    "email": "owner@app.com",
    "role": "Owner"
  }
}
```

### Google Login

- **Method**: `POST`
- **URL**: `{{baseUrl}}/api/Auth/google-login`
- **Body example**:

```json
{
  "idToken": "<google_id_token_here>"
}
```
- **Response example**:

```json
{
  "token": "<jwt>",
  "expiresInMinutes": 60,
  "user": {
    "id": "<userId>",
    "fullName": "Business Owner",
    "email": "owner@app.com",
    "role": "Owner"
  }
}
```

### Promote to Owner [Admin only]

- **Method**: `POST`
- **URL**: `{{baseUrl}}/api/Auth/promote-to-owner`
- **Body example**:

```json
"{{userId}}"
```
- **Response example**:

```json
{
  "message": "Success"
}
```

### Promote to Admin [Admin only]

- **Method**: `POST`
- **URL**: `{{baseUrl}}/api/Auth/promote-to-admin`
- **Body example**:

```json
"{{userId}}"
```
- **Response example**:

```json
{
  "message": "Success"
}
```


## 02 - Business

### Get All Businesses [Owner/Admin]

- **Method**: `GET`
- **URL**: `{{baseUrl}}/api/Business`
- **Response example**:

```json
{
  "id": "<id>",
  "message": "Details object (see Swagger for full fields)."
}
```

### Get Business by ID [Owner/Admin]

- **Method**: `GET`
- **URL**: `{{baseUrl}}/api/Business/{{businessId}}`
- **Response example**:

```json
{
  "id": "<id>",
  "message": "Details object (see Swagger for full fields)."
}
```

### Create Business [Owner/Admin]

- **Method**: `POST`
- **URL**: `{{baseUrl}}/api/Business`
- **Body example**:

```json
{
  "name": "The Italian Place",
  "type": "Restaurant",
  "address": "123 Tahrir Square, Cairo",
  "phone": "+201012345678",
  "email": "info@italianplace.com",
  "website": "https://italianplace.com",
  "facebookUrl": null,
  "instagramUrl": null,
  "city": "Cairo",
  "country": "Egypt",
  "latitude": 30.0444,
  "longitude": 31.2357,
  "description": "Authentic Italian cuisine in the heart of Cairo",
  "cuisineType": "Italian",
  "priceRange": "$$",
  "logoUrl": null,
  "coverImageUrl": null,
  "hasDelivery": true,
  "hasTakeout": true,
  "hasParking": false,
  "hasWiFi": true,
  "hasOutdoorSeating": true,
  "acceptsReservations": true,
  "paymentMethods": "Cash,Visa,Mastercard",
  "workingHours": [
    {
      "dayOfWeek": 0,
      "openTime": null,
      "closeTime": null,
      "isClosed": true
    },
    {
      "dayOfWeek": 1,
      "openTime": "09:00",
      "closeTime": "22:00",
      "isClosed": false
    },
    {
      "dayOfWeek": 2,
      "openTime": "09:00",
      "closeTime": "22:00",
      "isClosed": false
    },
    {
      "dayOfWeek": 3,
      "openTime": "09:00",
      "closeTime": "22:00",
      "isClosed": false
    },
    {
      "dayOfWeek": 4,
      "openTime": "09:00",
      "closeTime": "23:00",
      "isClosed": false
    },
    {
      "dayOfWeek": 5,
      "openTime": "10:00",
      "closeTime": "23:00",
      "isClosed": false
    },
    {
      "dayOfWeek": 6,
      "openTime": "10:00",
      "closeTime": "21:00",
      "isClosed": false
    }
  ]
}
```
- **Response example**:

```json
{
  "message": "Success"
}
```

### Onboard Business [Public]

- **Method**: `POST`
- **URL**: `{{baseUrl}}/api/Business/onboard`
- **Body example**:

```json
{
  "name": "The Italian Place",
  "type": "Restaurant",
  "address": "123 Tahrir Square, Cairo",
  "phone": "+201012345678",
  "email": "info@italianplace.com",
  "website": "https://italianplace.com",
  "city": "Cairo",
  "country": "Egypt",
  "latitude": 30.0444,
  "longitude": 31.2357,
  "description": "Authentic Italian cuisine in the heart of Cairo",
  "cuisineType": "Italian",
  "priceRange": "$$",
  "hasDelivery": true,
  "hasTakeout": true,
  "hasParking": false,
  "hasWiFi": true,
  "hasOutdoorSeating": true,
  "acceptsReservations": true,
  "paymentMethods": "Cash,Visa,Mastercard",
  "workingHours": [
    {
      "dayOfWeek": 0,
      "openTime": null,
      "closeTime": null,
      "isClosed": true
    },
    {
      "dayOfWeek": 1,
      "openTime": "09:00",
      "closeTime": "22:00",
      "isClosed": false
    },
    {
      "dayOfWeek": 2,
      "openTime": "09:00",
      "closeTime": "22:00",
      "isClosed": false
    },
    {
      "dayOfWeek": 3,
      "openTime": "09:00",
      "closeTime": "22:00",
      "isClosed": false
    },
    {
      "dayOfWeek": 4,
      "openTime": "09:00",
      "closeTime": "23:00",
      "isClosed": false
    },
    {
      "dayOfWeek": 5,
      "openTime": "10:00",
      "closeTime": "23:00",
      "isClosed": false
    },
    {
      "dayOfWeek": 6,
      "openTime": "10:00",
      "closeTime": "21:00",
      "isClosed": false
    }
  ],
  "agentName": "Luigi",
  "agentTone": "Friendly",
  "welcomeMessage": "Welcome to The Italian Place! How can we help you today?",
  "knowledgeBaseItems": [],
  "menuCategories": [
    {
      "name": "Main Dishes",
      "description": "Signature plates",
      "displayOrder": 1
    }
  ],
  "menuItems": [
    {
      "name": "Margherita Pizza",
      "description": "Tomato, mozzarella, fresh basil",
      "price": 89.99,
      "menuCategoryName": "Main Dishes",
      "isAvailable": true
    }
  ],
  "planName": "Monthly",
  "price": 49.99,
  "cardHolderName": "John Doe",
  "cardNumber": "4111111111111111",
  "cardExpiryMonth": 12,
  "cardExpiryYear": 2028,
  "cardCvv": "123"
}
```
- **Response example**:

```json
{
  "message": "Success"
}
```

### Update Business [Owner/Admin]

- **Method**: `PUT`
- **URL**: `{{baseUrl}}/api/Business/{{businessId}}`
- **Body example**:

```json
{
  "name": "The Italian Place Updated",
  "phone": "+201012345678",
  "address": "456 New Street, Cairo",
  "description": "Updated description",
  "hasDelivery": true,
  "hasWiFi": true
}
```
- **Response example**:

```json
{
  "message": "Success"
}
```

### Delete Business [Admin]

- **Method**: `DELETE`
- **URL**: `{{baseUrl}}/api/Business/{{businessId}}`
- **Response example**:

```json
{
  "message": "Deleted"
}
```


## 03 - User

### Get All Users [Admin]

- **Method**: `GET`
- **URL**: `{{baseUrl}}/api/User`
- **Response example**:

```json
{
  "id": "<id>",
  "message": "Details object (see Swagger for full fields)."
}
```

### Get Users by Business [Owner/Admin]

- **Method**: `GET`
- **URL**: `{{baseUrl}}/api/User/business/{{businessId}}`
- **Response example**:

```json
{
  "id": "<id>",
  "message": "Details object (see Swagger for full fields)."
}
```

### Get User by ID

- **Method**: `GET`
- **URL**: `{{baseUrl}}/api/User/{{userId}}`
- **Response example**:

```json
{
  "id": "<id>",
  "message": "Details object (see Swagger for full fields)."
}
```

### Get User by Email [Admin]

- **Method**: `GET`
- **URL**: `{{baseUrl}}/api/User/email/agent@app.com`
- **Response example**:

```json
{
  "id": "<id>",
  "message": "Details object (see Swagger for full fields)."
}
```

### Update User [Admin]

- **Method**: `PUT`
- **URL**: `{{baseUrl}}/api/User/{{userId}}`
- **Body example**:

```json
{
  "fullName": "Updated Name",
  "phone": "+201012345678"
}
```
- **Response example**:

```json
{
  "message": "Success"
}
```

### Assign Role [Admin]

- **Method**: `POST`
- **URL**: `{{baseUrl}}/api/User/{{userId}}/assign-role`
- **Body example**:

```json
{
  "userId": "{{userId}}",
  "newRole": "Agent"
}
```
- **Response example**:

```json
{
  "message": "Success"
}
```

### Create Human Employee / Agent [Owner/Admin]

- **Method**: `POST`
- **URL**: `{{baseUrl}}/api/User/agents`
- **Body example**:

```json
{
  "fullName": "New Agent",
  "email": "newagent@app.com",
  "password": "Agent@123",
  "phone": "+201098765432"
}
```
- **Response example**:

```json
{
  "message": "Success"
}
```

### Delete User [Admin]

- **Method**: `DELETE`
- **URL**: `{{baseUrl}}/api/User/{{userId}}`
- **Response example**:

```json
{
  "message": "Deleted"
}
```


## 04 - Customer

### Get All Customers

- **Method**: `GET`
- **URL**: `{{baseUrl}}/api/Customer`
- **Response example**:

```json
{
  "id": "<id>",
  "message": "Details object (see Swagger for full fields)."
}
```

### Get Customers by Business

- **Method**: `GET`
- **URL**: `{{baseUrl}}/api/Customer/business/{{businessId}}`
- **Response example**:

```json
{
  "id": "<id>",
  "message": "Details object (see Swagger for full fields)."
}
```

### Get Customer by ID

- **Method**: `GET`
- **URL**: `{{baseUrl}}/api/Customer/{{customerId}}`
- **Response example**:

```json
{
  "id": "<id>",
  "message": "Details object (see Swagger for full fields)."
}
```

### Get Customer by Email

- **Method**: `GET`
- **URL**: `{{baseUrl}}/api/Customer/email/customer@example.com`
- **Response example**:

```json
{
  "id": "<id>",
  "message": "Details object (see Swagger for full fields)."
}
```

### Create Customer [Owner/Admin]

- **Method**: `POST`
- **URL**: `{{baseUrl}}/api/Customer`
- **Body example**:

```json
{
  "fullName": "Ahmed Ali",
  "email": "ahmed@gmail.com",
  "phone": "+201098765432",
  "businessId": "{{businessId}}"
}
```
- **Response example**:

```json
{
  "message": "Success"
}
```

### Update Customer [Owner/Admin]

- **Method**: `PUT`
- **URL**: `{{baseUrl}}/api/Customer/{{customerId}}`
- **Body example**:

```json
{
  "fullName": "Ahmed Ali Updated",
  "email": "ahmed.updated@gmail.com",
  "phone": "+201098765432"
}
```
- **Response example**:

```json
{
  "message": "Success"
}
```

### Delete Customer [Admin]

- **Method**: `DELETE`
- **URL**: `{{baseUrl}}/api/Customer/{{customerId}}`
- **Response example**:

```json
{
  "message": "Deleted"
}
```


## 05 - Dashboard [Owner] (Top products included in Analytics)

### Get Dashboard Summary

- **Method**: `GET`
- **URL**: `{{baseUrl}}/api/Dashboard/summary`
- **Response example**:

```json
{
  "id": "<id>",
  "message": "Details object (see Swagger for full fields)."
}
```

### Get Analytics

- **Method**: `GET`
- **URL**: `{{baseUrl}}/api/Dashboard/analytics`
- **Response example**:

```json
{
  "id": "<id>",
  "message": "Details object (see Swagger for full fields)."
}
```

### Get Full Dashboard

- **Method**: `GET`
- **URL**: `{{baseUrl}}/api/Dashboard/full`
- **Response example**:

```json
{
  "id": "<id>",
  "message": "Details object (see Swagger for full fields)."
}
```

### Get Recent Audit Logs

- **Method**: `GET`
- **URL**: `{{baseUrl}}/api/Dashboard/audit-logs/recent?count=20`
- **Response example**:

```json
{
  "id": "<id>",
  "message": "Details object (see Swagger for full fields)."
}
```

### Get Audit Log Statistics

- **Method**: `GET`
- **URL**: `{{baseUrl}}/api/Dashboard/audit-logs/statistics`
- **Response example**:

```json
{
  "id": "<id>",
  "message": "Details object (see Swagger for full fields)."
}
```

### Get Customer Audit Logs

- **Method**: `GET`
- **URL**: `{{baseUrl}}/api/Dashboard/audit-logs/customer/{{customerId}}`
- **Response example**:

```json
{
  "id": "<id>",
  "message": "Details object (see Swagger for full fields)."
}
```


## 06 - Admin Dashboard

### Get Summary

- **Method**: `GET`
- **URL**: `{{baseUrl}}/api/AdminDashboard/summary`
- **Response example**:

```json
{
  "id": "<id>",
  "message": "Details object (see Swagger for full fields)."
}
```

### Get Top Businesses

- **Method**: `GET`
- **URL**: `{{baseUrl}}/api/AdminDashboard/top-businesses?count=10`
- **Response example**:

```json
{
  "id": "<id>",
  "message": "Details object (see Swagger for full fields)."
}
```

### Get Full Dashboard

- **Method**: `GET`
- **URL**: `{{baseUrl}}/api/AdminDashboard/full`
- **Response example**:

```json
{
  "id": "<id>",
  "message": "Details object (see Swagger for full fields)."
}
```

### Get Alerts

- **Method**: `GET`
- **URL**: `{{baseUrl}}/api/AdminDashboard/alerts`
- **Response example**:

```json
{
  "id": "<id>",
  "message": "Details object (see Swagger for full fields)."
}
```

### Get Revenue Trend

- **Method**: `GET`
- **URL**: `{{baseUrl}}/api/AdminDashboard/revenue-trend?months=12`
- **Response example**:

```json
{
  "id": "<id>",
  "message": "Details object (see Swagger for full fields)."
}
```

### Get Orders by Status

- **Method**: `GET`
- **URL**: `{{baseUrl}}/api/AdminDashboard/orders-by-status`
- **Response example**:

```json
{
  "id": "<id>",
  "message": "Details object (see Swagger for full fields)."
}
```

### Get Tickets by Priority

- **Method**: `GET`
- **URL**: `{{baseUrl}}/api/AdminDashboard/tickets-by-priority`
- **Response example**:

```json
{
  "id": "<id>",
  "message": "Details object (see Swagger for full fields)."
}
```

### Get Business Health

- **Method**: `GET`
- **URL**: `{{baseUrl}}/api/AdminDashboard/business-health?top=20&sort=desc`
- **Response example**:

```json
{
  "id": "<id>",
  "message": "Details object (see Swagger for full fields)."
}
```

### Get Sentiment Trend

- **Method**: `GET`
- **URL**: `{{baseUrl}}/api/AdminDashboard/sentiment-trend?days=30`
- **Response example**:

```json
{
  "id": "<id>",
  "message": "Details object (see Swagger for full fields)."
}
```

### Suspend Business

- **Method**: `POST`
- **URL**: `{{baseUrl}}/api/AdminDashboard/business/{{businessId}}/suspend`
- **Response example**:

```json
{
  "message": "Action completed successfully."
}
```

### Activate Business

- **Method**: `POST`
- **URL**: `{{baseUrl}}/api/AdminDashboard/business/{{businessId}}/activate`
- **Response example**:

```json
{
  "message": "Action completed successfully."
}
```

### Verify Business

- **Method**: `POST`
- **URL**: `{{baseUrl}}/api/AdminDashboard/business/{{businessId}}/verify`
- **Response example**:

```json
{
  "message": "Action completed successfully."
}
```

### Unverify Business

- **Method**: `POST`
- **URL**: `{{baseUrl}}/api/AdminDashboard/business/{{businessId}}/unverify`
- **Response example**:

```json
{
  "message": "Action completed successfully."
}
```


## 07 - Menu Category

### Get All Categories

- **Method**: `GET`
- **URL**: `{{baseUrl}}/api/MenuCategory`
- **Response example**:

```json
{
  "id": "<id>",
  "message": "Details object (see Swagger for full fields)."
}
```

### Get Categories by Business

- **Method**: `GET`
- **URL**: `{{baseUrl}}/api/MenuCategory/business/{{businessId}}`
- **Response example**:

```json
{
  "id": "<id>",
  "message": "Details object (see Swagger for full fields)."
}
```

### Get Active Categories [Public]

- **Method**: `GET`
- **URL**: `{{baseUrl}}/api/MenuCategory/business/{{businessId}}/active`
- **Response example**:

```json
{
  "id": "<id>",
  "message": "Details object (see Swagger for full fields)."
}
```

### Get Category by ID

- **Method**: `GET`
- **URL**: `{{baseUrl}}/api/MenuCategory/{{menuCategoryId}}`
- **Response example**:

```json
{
  "id": "<id>",
  "message": "Details object (see Swagger for full fields)."
}
```

### Create Category [Owner/Admin]

- **Method**: `POST`
- **URL**: `{{baseUrl}}/api/MenuCategory`
- **Body example**:

```json
{
  "name": "Main Dishes",
  "description": "Our signature main courses",
  "displayOrder": 1,
  "isActive": true,
  "businessId": "{{businessId}}"
}
```
- **Response example**:

```json
{
  "message": "Success"
}
```

### Update Category [Owner/Admin]

- **Method**: `PUT`
- **URL**: `{{baseUrl}}/api/MenuCategory/{{menuCategoryId}}`
- **Body example**:

```json
{
  "name": "Starters & Salads",
  "description": "Updated",
  "displayOrder": 1,
  "isActive": true
}
```
- **Response example**:

```json
{
  "message": "Success"
}
```

### Reorder Categories [Owner/Admin]

- **Method**: `PUT`
- **URL**: `{{baseUrl}}/api/MenuCategory/business/{{businessId}}/reorder`
- **Body example**:

```json
{
  "categoryOrders": [
    {
      "menuCategoryId": "{{menuCategoryId}}",
      "displayOrder": 1
    }
  ]
}
```
- **Response example**:

```json
{
  "message": "Success"
}
```

### Delete Category [Owner/Admin]

- **Method**: `DELETE`
- **URL**: `{{baseUrl}}/api/MenuCategory/{{menuCategoryId}}`
- **Response example**:

```json
{
  "message": "Deleted"
}
```


## 08 - Menu Item

### Get All Menu Items

- **Method**: `GET`
- **URL**: `{{baseUrl}}/api/MenuItem`
- **Response example**:

```json
{
  "id": "<id>",
  "message": "Details object (see Swagger for full fields)."
}
```

### Get Menu Items by Business

- **Method**: `GET`
- **URL**: `{{baseUrl}}/api/MenuItem/business/{{businessId}}`
- **Response example**:

```json
{
  "id": "<id>",
  "message": "Details object (see Swagger for full fields)."
}
```

### Get Menu Item by ID

- **Method**: `GET`
- **URL**: `{{baseUrl}}/api/MenuItem/{{menuItemId}}`
- **Response example**:

```json
{
  "id": "<id>",
  "message": "Details object (see Swagger for full fields)."
}
```

### Create Menu Item [Owner/Admin]

- **Method**: `POST`
- **URL**: `{{baseUrl}}/api/MenuItem`
- **Body example**:

```json
{
  "name": "Margherita Pizza",
  "description": "Classic tomato sauce, mozzarella, fresh basil",
  "price": 89.99,
  "imageUrl": null,
  "isAvailable": true,
  "menuCategoryId": "{{menuCategoryId}}",
  "businessId": "{{businessId}}"
}
```
- **Response example**:

```json
{
  "message": "Success"
}
```

### Update Menu Item [Owner/Admin]

- **Method**: `PUT`
- **URL**: `{{baseUrl}}/api/MenuItem/{{menuItemId}}`
- **Body example**:

```json
{
  "name": "Margherita Pizza XL",
  "description": "Extra large classic pizza",
  "price": 109.99,
  "isAvailable": true
}
```
- **Response example**:

```json
{
  "message": "Success"
}
```

### Delete Menu Item [Owner/Admin]

- **Method**: `DELETE`
- **URL**: `{{baseUrl}}/api/MenuItem/{{menuItemId}}`
- **Response example**:

```json
{
  "message": "Deleted"
}
```


## 09 - Order

### Get All Orders

- **Method**: `GET`
- **URL**: `{{baseUrl}}/api/Order`
- **Response example**:

```json
{
  "id": "<id>",
  "message": "Details object (see Swagger for full fields)."
}
```

### Get Orders by Business

- **Method**: `GET`
- **URL**: `{{baseUrl}}/api/Order/business/{{businessId}}`
- **Response example**:

```json
{
  "id": "<id>",
  "message": "Details object (see Swagger for full fields)."
}
```

### Get Orders by Customer

- **Method**: `GET`
- **URL**: `{{baseUrl}}/api/Order/customer/{{customerId}}`
- **Response example**:

```json
{
  "id": "<id>",
  "message": "Details object (see Swagger for full fields)."
}
```

### Get Order by ID

- **Method**: `GET`
- **URL**: `{{baseUrl}}/api/Order/{{orderId}}`
- **Response example**:

```json
{
  "id": "<id>",
  "message": "Details object (see Swagger for full fields)."
}
```

### Create Order

- **Method**: `POST`
- **URL**: `{{baseUrl}}/api/Order`
- **Body example**:

```json
{
  "businessId": "{{businessId}}",
  "customerId": "{{customerId}}",
  "items": [
    {
      "menuItemId": "{{menuItemId}}",
      "quantity": 2
    }
  ]
}
```
- **Response example**:

```json
{
  "message": "Success"
}
```

### Update Order Status [Owner/Admin]

- **Method**: `PUT`
- **URL**: `{{baseUrl}}/api/Order/{{orderId}}/status`
- **Body example**:

```json
{
  "orderId": "{{orderId}}",
  "status": "Paid"
}
```
- **Response example**:

```json
{
  "message": "Success"
}
```

### Delete Order [Admin]

- **Method**: `DELETE`
- **URL**: `{{baseUrl}}/api/Order/{{orderId}}`
- **Response example**:

```json
{
  "message": "Deleted"
}
```


## 10 - Ticket

### Get All Tickets

- **Method**: `GET`
- **URL**: `{{baseUrl}}/api/Ticket`
- **Response example**:

```json
{
  "id": "<id>",
  "message": "Details object (see Swagger for full fields)."
}
```

### Get Escalation Queue

- **Method**: `GET`
- **URL**: `{{baseUrl}}/api/Ticket/queue`
- **Response example**:

```json
{
  "id": "<id>",
  "message": "Details object (see Swagger for full fields)."
}
```

### Get Tickets by Business

- **Method**: `GET`
- **URL**: `{{baseUrl}}/api/Ticket/business/{{businessId}}`
- **Response example**:

```json
{
  "id": "<id>",
  "message": "Details object (see Swagger for full fields)."
}
```

### Get Ticket by ID

- **Method**: `GET`
- **URL**: `{{baseUrl}}/api/Ticket/{{ticketId}}`
- **Response example**:

```json
{
  "id": "<id>",
  "message": "Details object (see Swagger for full fields)."
}
```

### Create Ticket

- **Method**: `POST`
- **URL**: `{{baseUrl}}/api/Ticket`
- **Body example**:

```json
{
  "businessId": "{{businessId}}",
  "customerId": "{{customerId}}",
  "subject": "Wrong order delivered",
  "description": "I ordered Margherita but received Pepperoni",
  "priority": "High",
  "type": "Complaint",
  "interactionId": null
}
```
- **Response example**:

```json
{
  "message": "Success"
}
```

### Update Ticket

- **Method**: `PUT`
- **URL**: `{{baseUrl}}/api/Ticket/{{ticketId}}`
- **Body example**:

```json
{
  "subject": "Updated subject",
  "description": "Updated description",
  "priority": "Medium",
  "status": "InProgress"
}
```
- **Response example**:

```json
{
  "message": "Success"
}
```

### Assign Ticket [Owner/Admin]

- **Method**: `POST`
- **URL**: `{{baseUrl}}/api/Ticket/{{ticketId}}/assign`
- **Body example**:

```json
{
  "assignedToUserId": "{{userId}}"
}
```
- **Response example**:

```json
{
  "message": "Success"
}
```

### Close Ticket

- **Method**: `POST`
- **URL**: `{{baseUrl}}/api/Ticket/{{ticketId}}/close`
- **Body example**:

```json
{
  "resolutionNote": "Issue resolved. Customer refunded.",
  "closedByUserId": "{{userId}}"
}
```
- **Response example**:

```json
{
  "message": "Success"
}
```

### Delete Ticket [Admin]

- **Method**: `DELETE`
- **URL**: `{{baseUrl}}/api/Ticket/{{ticketId}}`
- **Response example**:

```json
{
  "message": "Deleted"
}
```


## 11 - Interaction

### Get All Interactions

- **Method**: `GET`
- **URL**: `{{baseUrl}}/api/Interaction`
- **Response example**:

```json
{
  "id": "<id>",
  "message": "Details object (see Swagger for full fields)."
}
```

### Get Interactions by Business

- **Method**: `GET`
- **URL**: `{{baseUrl}}/api/Interaction/business/{{businessId}}`
- **Response example**:

```json
{
  "id": "<id>",
  "message": "Details object (see Swagger for full fields)."
}
```

### Get Interactions by Customer

- **Method**: `GET`
- **URL**: `{{baseUrl}}/api/Interaction/customer/{{customerId}}`
- **Response example**:

```json
{
  "id": "<id>",
  "message": "Details object (see Swagger for full fields)."
}
```

### Get Interactions by User

- **Method**: `GET`
- **URL**: `{{baseUrl}}/api/Interaction/user/{{userId}}`
- **Response example**:

```json
{
  "id": "<id>",
  "message": "Details object (see Swagger for full fields)."
}
```

### Get Interaction by ID

- **Method**: `GET`
- **URL**: `{{baseUrl}}/api/Interaction/{{interactionId}}`
- **Response example**:

```json
{
  "id": "<id>",
  "message": "Details object (see Swagger for full fields)."
}
```

### Start Interaction

- **Method**: `POST`
- **URL**: `{{baseUrl}}/api/Interaction/start`
- **Body example**:

```json
{
  "businessId": "{{businessId}}",
  "customerId": "{{customerId}}",
  "channel": "WebChat",
  "assignedUserId": null
}
```
- **Response example**:

```json
{
  "message": "Success"
}
```

### End Interaction

- **Method**: `POST`
- **URL**: `{{baseUrl}}/api/Interaction/{{interactionId}}/end`
- **Body example**:

```json
{
  "resolutionStatus": "Resolved",
  "notes": "Customer satisfied"
}
```
- **Response example**:

```json
{
  "message": "Success"
}
```

### Delete Interaction [Admin]

- **Method**: `DELETE`
- **URL**: `{{baseUrl}}/api/Interaction/{{interactionId}}`
- **Response example**:

```json
{
  "message": "Deleted"
}
```


## 12 - Message

### Get All Messages

- **Method**: `GET`
- **URL**: `{{baseUrl}}/api/Message`
- **Response example**:

```json
{
  "id": "<id>",
  "message": "Details object (see Swagger for full fields)."
}
```

### Get Messages by Interaction

- **Method**: `GET`
- **URL**: `{{baseUrl}}/api/Message/interaction/{{interactionId}}`
- **Response example**:

```json
{
  "id": "<id>",
  "message": "Details object (see Swagger for full fields)."
}
```

### Get Message by ID

- **Method**: `GET`
- **URL**: `{{baseUrl}}/api/Message/{{messageId}}`
- **Response example**:

```json
{
  "id": "<id>",
  "message": "Details object (see Swagger for full fields)."
}
```

### Create Message

- **Method**: `POST`
- **URL**: `{{baseUrl}}/api/Message`
- **Body example**:

```json
{
  "interactionId": "{{interactionId}}",
  "content": "Hello, how can I help you?",
  "senderType": "Agent",
  "senderId": "{{userId}}"
}
```
- **Response example**:

```json
{
  "message": "Success"
}
```

### Delete Message [Admin]

- **Method**: `DELETE`
- **URL**: `{{baseUrl}}/api/Message/{{messageId}}`
- **Response example**:

```json
{
  "message": "Deleted"
}
```


## 13 - Knowledge Base

### Get All KB Items

- **Method**: `GET`
- **URL**: `{{baseUrl}}/api/KnowledgeBase`
- **Response example**:

```json
{
  "id": "<id>",
  "message": "Details object (see Swagger for full fields)."
}
```

### Get KB Items by Business

- **Method**: `GET`
- **URL**: `{{baseUrl}}/api/KnowledgeBase/business/{{businessId}}`
- **Response example**:

```json
{
  "id": "<id>",
  "message": "Details object (see Swagger for full fields)."
}
```

### Get KB Item by ID

- **Method**: `GET`
- **URL**: `{{baseUrl}}/api/KnowledgeBase/{{knowledgeBaseId}}`
- **Response example**:

```json
{
  "id": "<id>",
  "message": "Details object (see Swagger for full fields)."
}
```

### Create KB Item [Owner/Admin]

- **Method**: `POST`
- **URL**: `{{baseUrl}}/api/KnowledgeBase`
- **Body example**:

```json
{
  "title": "How to place an order",
  "content": "To place an order, browse our menu and add items to your cart...",
  "businessId": "{{businessId}}",
  "isFAQ": false,
  "tags": "ordering, how-to, menu"
}
```
- **Response example**:

```json
{
  "message": "Success"
}
```

### Update KB Item [Owner/Admin]

- **Method**: `PUT`
- **URL**: `{{baseUrl}}/api/KnowledgeBase/{{knowledgeBaseId}}`
- **Body example**:

```json
{
  "title": "Updated title",
  "content": "Updated content here.",
  "businessId": "{{businessId}}",
  "isFAQ": false,
  "tags": "updated"
}
```
- **Response example**:

```json
{
  "message": "Success"
}
```

### Delete KB Item [Owner/Admin]

- **Method**: `DELETE`
- **URL**: `{{baseUrl}}/api/KnowledgeBase/{{knowledgeBaseId}}`
- **Response example**:

```json
{
  "message": "Deleted"
}
```


## 14 - FAQ

### Get FAQs by Business [Public]

- **Method**: `GET`
- **URL**: `{{baseUrl}}/api/FAQ/business/{{businessId}}`
- **Response example**:

```json
{
  "id": "<id>",
  "message": "Details object (see Swagger for full fields)."
}
```

### Manage FAQs [Owner/Admin]

- **Method**: `GET`
- **URL**: `{{baseUrl}}/api/FAQ/business/{{businessId}}/manage`
- **Response example**:

```json
{
  "id": "<id>",
  "message": "Details object (see Swagger for full fields)."
}
```

### Get FAQ by ID [Public]

- **Method**: `GET`
- **URL**: `{{baseUrl}}/api/FAQ/{{faqId}}`
- **Response example**:

```json
{
  "id": "<id>",
  "message": "Details object (see Swagger for full fields)."
}
```

### Create FAQ [Owner/Admin]

- **Method**: `POST`
- **URL**: `{{baseUrl}}/api/FAQ`
- **Body example**:

```json
{
  "title": "What are your opening hours?",
  "content": "We are open from 9 AM to 10 PM, Monday to Saturday. Closed on Sundays.",
  "businessId": "{{businessId}}",
  "isFAQ": true,
  "tags": "hours, schedule, open"
}
```
- **Response example**:

```json
{
  "message": "Success"
}
```

### Update FAQ [Owner/Admin]

- **Method**: `PUT`
- **URL**: `{{baseUrl}}/api/FAQ/{{faqId}}`
- **Body example**:

```json
{
  "title": "Updated FAQ question?",
  "content": "Updated FAQ answer.",
  "businessId": "{{businessId}}",
  "isFAQ": true
}
```
- **Response example**:

```json
{
  "message": "Success"
}
```

### Delete FAQ [Owner/Admin]

- **Method**: `DELETE`
- **URL**: `{{baseUrl}}/api/FAQ/{{faqId}}`
- **Response example**:

```json
{
  "message": "Deleted"
}
```


## 15 - Feedback

### Get All Feedback

- **Method**: `GET`
- **URL**: `{{baseUrl}}/api/Feedback`
- **Response example**:

```json
{
  "id": "<id>",
  "message": "Details object (see Swagger for full fields)."
}
```

### Get Feedback by Customer

- **Method**: `GET`
- **URL**: `{{baseUrl}}/api/Feedback/customer/{{customerId}}`
- **Response example**:

```json
{
  "id": "<id>",
  "message": "Details object (see Swagger for full fields)."
}
```

### Get Feedback by ID

- **Method**: `GET`
- **URL**: `{{baseUrl}}/api/Feedback/{{feedbackId}}`
- **Response example**:

```json
{
  "id": "<id>",
  "message": "Details object (see Swagger for full fields)."
}
```

### Submit Feedback [Public]

- **Method**: `POST`
- **URL**: `{{baseUrl}}/api/Feedback`
- **Body example**:

```json
{
  "customerId": "{{customerId}}",
  "interactionId": "{{interactionId}}",
  "rating": 5,
  "comment": "Excellent service and food!",
  "businessId": "{{businessId}}"
}
```
- **Response example**:

```json
{
  "message": "Success"
}
```

### Update Feedback [Owner/Admin]

- **Method**: `PUT`
- **URL**: `{{baseUrl}}/api/Feedback/{{feedbackId}}`
- **Body example**:

```json
{
  "rating": 4,
  "comment": "Updated comment"
}
```
- **Response example**:

```json
{
  "message": "Success"
}
```

### Delete Feedback [Admin]

- **Method**: `DELETE`
- **URL**: `{{baseUrl}}/api/Feedback/{{feedbackId}}`
- **Response example**:

```json
{
  "message": "Deleted"
}
```


## 16 - Sentiment [Read Only]

### Get All Sentiments

- **Method**: `GET`
- **URL**: `{{baseUrl}}/api/Sentiment`
- **Response example**:

```json
{
  "id": "<id>",
  "message": "Details object (see Swagger for full fields)."
}
```

### Get Sentiments by Message

- **Method**: `GET`
- **URL**: `{{baseUrl}}/api/Sentiment/message/{{messageId}}`
- **Response example**:

```json
{
  "id": "<id>",
  "message": "Details object (see Swagger for full fields)."
}
```

### Get Sentiments by Business

- **Method**: `GET`
- **URL**: `{{baseUrl}}/api/Sentiment/business/{{businessId}}`
- **Response example**:

```json
{
  "id": "<id>",
  "message": "Details object (see Swagger for full fields)."
}
```

### Get Sentiment by ID

- **Method**: `GET`
- **URL**: `{{baseUrl}}/api/Sentiment/{{sentimentId}}`
- **Response example**:

```json
{
  "id": "<id>",
  "message": "Details object (see Swagger for full fields)."
}
```


## 17 - Notification

### Get All Notifications

- **Method**: `GET`
- **URL**: `{{baseUrl}}/api/Notification`
- **Response example**:

```json
{
  "id": "<id>",
  "message": "Details object (see Swagger for full fields)."
}
```

### Get Notifications by Business

- **Method**: `GET`
- **URL**: `{{baseUrl}}/api/Notification/business/{{businessId}}`
- **Response example**:

```json
{
  "id": "<id>",
  "message": "Details object (see Swagger for full fields)."
}
```

### Get Notifications by User

- **Method**: `GET`
- **URL**: `{{baseUrl}}/api/Notification/user/{{userId}}`
- **Response example**:

```json
{
  "id": "<id>",
  "message": "Details object (see Swagger for full fields)."
}
```

### Get Notification by ID

- **Method**: `GET`
- **URL**: `{{baseUrl}}/api/Notification/{{notificationId}}`
- **Response example**:

```json
{
  "id": "<id>",
  "message": "Details object (see Swagger for full fields)."
}
```

### Create Notification [Owner/Admin]

- **Method**: `POST`
- **URL**: `{{baseUrl}}/api/Notification`
- **Body example**:

```json
{
  "businessId": "{{businessId}}",
  "userId": "{{userId}}",
  "title": "New Order Received",
  "message": "Order #1234 has been placed and is awaiting confirmation.",
  "type": "Order"
}
```
- **Response example**:

```json
{
  "message": "Success"
}
```

### Mark as Read

- **Method**: `PUT`
- **URL**: `{{baseUrl}}/api/Notification/{{notificationId}}/read`
- **Response example**:

```json
{
  "message": "Success"
}
```

### Delete Notification [Owner/Admin]

- **Method**: `DELETE`
- **URL**: `{{baseUrl}}/api/Notification/{{notificationId}}`
- **Response example**:

```json
{
  "message": "Deleted"
}
```


## 18 - Report

### Get All Reports

- **Method**: `GET`
- **URL**: `{{baseUrl}}/api/Report`
- **Response example**:

```json
{
  "id": "<id>",
  "message": "Details object (see Swagger for full fields)."
}
```

### Get Reports by Business

- **Method**: `GET`
- **URL**: `{{baseUrl}}/api/Report/business/{{businessId}}`
- **Response example**:

```json
{
  "id": "<id>",
  "message": "Details object (see Swagger for full fields)."
}
```

### Get Report by ID

- **Method**: `GET`
- **URL**: `{{baseUrl}}/api/Report/{{reportId}}`
- **Response example**:

```json
{
  "id": "<id>",
  "message": "Details object (see Swagger for full fields)."
}
```

### Create Report [Owner/Admin]

- **Method**: `POST`
- **URL**: `{{baseUrl}}/api/Report`
- **Body example**:

```json
{
  "businessId": "{{businessId}}",
  "type": "Sales",
  "period": "Monthly",
  "startDate": "2026-03-01T00:00:00Z",
  "endDate": "2026-03-31T23:59:59Z"
}
```
- **Response example**:

```json
{
  "message": "Success"
}
```

### Delete Report [Admin]

- **Method**: `DELETE`
- **URL**: `{{baseUrl}}/api/Report/{{reportId}}`
- **Response example**:

```json
{
  "message": "Deleted"
}
```


## 19 - Subscription

### Get All Subscriptions [Admin]

- **Method**: `GET`
- **URL**: `{{baseUrl}}/api/Subscription`
- **Response example**:

```json
{
  "id": "<id>",
  "message": "Details object (see Swagger for full fields)."
}
```

### Get Subscriptions by Business [Owner/Admin]

- **Method**: `GET`
- **URL**: `{{baseUrl}}/api/Subscription/business/{{businessId}}`
- **Response example**:

```json
{
  "id": "<id>",
  "message": "Details object (see Swagger for full fields)."
}
```

### Get Active Subscription [Owner/Admin]

- **Method**: `GET`
- **URL**: `{{baseUrl}}/api/Subscription/business/{{businessId}}/active`
- **Response example**:

```json
{
  "id": "<id>",
  "message": "Details object (see Swagger for full fields)."
}
```

### Get Subscription by ID [Owner/Admin]

- **Method**: `GET`
- **URL**: `{{baseUrl}}/api/Subscription/{{subscriptionId}}`
- **Response example**:

```json
{
  "id": "<id>",
  "message": "Details object (see Swagger for full fields)."
}
```

### Create Subscription [Admin]

- **Method**: `POST`
- **URL**: `{{baseUrl}}/api/Subscription`
- **Body example**:

```json
{
  "businessId": "{{businessId}}",
  "plan": "Pro",
  "startDate": "2026-03-01T00:00:00Z",
  "endDate": "2026-04-01T00:00:00Z",
  "monthlyPrice": 49.99
}
```
- **Response example**:

```json
{
  "message": "Success"
}
```

### Renew Subscription [Admin]

- **Method**: `POST`
- **URL**: `{{baseUrl}}/api/Subscription/{{subscriptionId}}/renew`
- **Body example**:

```json
{
  "newEndDate": "2026-05-01T00:00:00Z"
}
```
- **Response example**:

```json
{
  "message": "Success"
}
```

### Delete Subscription [Admin]

- **Method**: `DELETE`
- **URL**: `{{baseUrl}}/api/Subscription/{{subscriptionId}}`
- **Response example**:

```json
{
  "message": "Deleted"
}
```


## 20 - Payment Transaction

### Get All Payments [Admin]

- **Method**: `GET`
- **URL**: `{{baseUrl}}/api/PaymentTransaction`
- **Response example**:

```json
{
  "id": "<id>",
  "message": "Details object (see Swagger for full fields)."
}
```

### Get Payments by Subscription [Owner/Admin]

- **Method**: `GET`
- **URL**: `{{baseUrl}}/api/PaymentTransaction/subscription/{{subscriptionId}}`
- **Response example**:

```json
{
  "id": "<id>",
  "message": "Details object (see Swagger for full fields)."
}
```

### Get Payments by Business [Owner/Admin]

- **Method**: `GET`
- **URL**: `{{baseUrl}}/api/PaymentTransaction/business/{{businessId}}`
- **Response example**:

```json
{
  "id": "<id>",
  "message": "Details object (see Swagger for full fields)."
}
```

### Get Payment by ID [Owner/Admin]

- **Method**: `GET`
- **URL**: `{{baseUrl}}/api/PaymentTransaction/{{paymentId}}`
- **Response example**:

```json
{
  "id": "<id>",
  "message": "Details object (see Swagger for full fields)."
}
```

### Create Payment [Admin]

- **Method**: `POST`
- **URL**: `{{baseUrl}}/api/PaymentTransaction`
- **Body example**:

```json
{
  "subscriptionId": "{{subscriptionId}}",
  "amount": 49.99,
  "currency": "USD",
  "paymentMethod": "Visa",
  "transactionReference": "TXN-001",
  "status": "Success"
}
```
- **Response example**:

```json
{
  "message": "Success"
}
```

### Delete Payment [Admin]

- **Method**: `DELETE`
- **URL**: `{{baseUrl}}/api/PaymentTransaction/{{paymentId}}`
- **Response example**:

```json
{
  "message": "Deleted"
}
```


## 21 - Integration

### Get All Integrations [Admin]

- **Method**: `GET`
- **URL**: `{{baseUrl}}/api/Integration`
- **Response example**:

```json
{
  "id": "<id>",
  "message": "Details object (see Swagger for full fields)."
}
```

### Get Integrations by Business [Owner/Admin]

- **Method**: `GET`
- **URL**: `{{baseUrl}}/api/Integration/business/{{businessId}}`
- **Response example**:

```json
{
  "id": "<id>",
  "message": "Details object (see Swagger for full fields)."
}
```

### Get Integration by ID [Owner/Admin]

- **Method**: `GET`
- **URL**: `{{baseUrl}}/api/Integration/{{integrationId}}`
- **Response example**:

```json
{
  "id": "<id>",
  "message": "Details object (see Swagger for full fields)."
}
```

### Connect Integration [Owner/Admin]

- **Method**: `POST`
- **URL**: `{{baseUrl}}/api/Integration/connect`
- **Body example**:

```json
{
  "businessId": "{{businessId}}",
  "platform": "WhatsApp",
  "apiKey": "your_api_key_here",
  "webhookUrl": "https://yourapp.com/webhook"
}
```
- **Response example**:

```json
{
  "message": "Success"
}
```

### Sync Integration [Owner/Admin]

- **Method**: `POST`
- **URL**: `{{baseUrl}}/api/Integration/{{integrationId}}/sync`
- **Body example**:

```json
{
  "syncType": "Full"
}
```
- **Response example**:

```json
{
  "message": "Success"
}
```

### Delete Integration [Owner/Admin]

- **Method**: `DELETE`
- **URL**: `{{baseUrl}}/api/Integration/{{integrationId}}`
- **Response example**:

```json
{
  "message": "Deleted"
}
```


## 22 - Setting

### Get Settings by Business [Owner/Admin]

- **Method**: `GET`
- **URL**: `{{baseUrl}}/api/Setting/business/{{businessId}}`
- **Response example**:

```json
{
  "id": "<id>",
  "message": "Details object (see Swagger for full fields)."
}
```

### Update Settings [Owner/Admin]

- **Method**: `PUT`
- **URL**: `{{baseUrl}}/api/Setting/business/{{businessId}}`
- **Body example**:

```json
{
  "autoAssignTickets": true,
  "enableNotifications": true,
  "language": "en",
  "timeZone": "Africa/Cairo",
  "chatbotEnabled": true,
  "chatbotWelcomeMessage": "Hi! I'm your digital host. What can I get for you today?",
  "chatbotPersonality": "Friendly",
  "agentVoice": "default",
  "agentVoiceProvider": "azure",
  "agentVoiceSpeed": 1.0,
  "agentVoicePitch": 1.0,
  "agentVoiceLanguage": "en-US",
  "emailNotifications": true,
  "smsNotifications": false,
  "pushNotifications": true
}
```
- **Response example**:

```json
{
  "message": "Success"
}
```


## 23 - Audit Log [Read Only]

### Get All Audit Logs [Admin]

- **Method**: `GET`
- **URL**: `{{baseUrl}}/api/AuditLog`
- **Response example**:

```json
{
  "id": "<id>",
  "message": "Details object (see Swagger for full fields)."
}
```

### Get Audit Logs by Business [Owner/Admin]

- **Method**: `GET`
- **URL**: `{{baseUrl}}/api/AuditLog/business/{{businessId}}`
- **Response example**:

```json
{
  "id": "<id>",
  "message": "Details object (see Swagger for full fields)."
}
```

### Get Audit Logs by User [Admin]

- **Method**: `GET`
- **URL**: `{{baseUrl}}/api/AuditLog/user/{{userId}}`
- **Response example**:

```json
{
  "id": "<id>",
  "message": "Details object (see Swagger for full fields)."
}
```

### Get Audit Log by ID [Admin]

- **Method**: `GET`
- **URL**: `{{baseUrl}}/api/AuditLog/{{auditLogId}}`
- **Response example**:

```json
{
  "id": "<id>",
  "message": "Details object (see Swagger for full fields)."
}
```


## 24 - Chatbot [AI]

### Ask Question [Owner/Admin]

- **Method**: `POST`
- **URL**: `{{baseUrl}}/api/Chatbot/ask`
- **Body example**:

```json
{
  "question": "What is my business performance overview?",
  "conversationId": null
}
```
- **Response example**:

```json
{
  "message": "Success"
}
```

### Get Suggestions [Owner/Admin]

- **Method**: `GET`
- **URL**: `{{baseUrl}}/api/Chatbot/suggestions`
- **Response example**:

```json
{
  "id": "<id>",
  "message": "Details object (see Swagger for full fields)."
}
```


## 25 - Customer Chat [Public]

### Get Business Capabilities

- **Method**: `GET`
- **URL**: `{{baseUrl}}/api/CustomerChat/capabilities/{{businessId}}`
- **Response example**:

```json
{
  "id": "<id>",
  "message": "Details object (see Swagger for full fields)."
}
```

### Send Chat Message

- **Method**: `POST`
- **URL**: `{{baseUrl}}/api/CustomerChat/message`
- **Body example**:

```json
{
  "businessId": "{{businessId}}",
  "customerId": null,
  "message": "What are your opening hours?",
  "channel": "WebChat",
  "sessionId": "session-001"
}
```
- **Response example**:

```json
{
  "message": "Success"
}
```

### Get Order Recommendations

- **Method**: `POST`
- **URL**: `{{baseUrl}}/api/CustomerChat/recommendations`
- **Body example**:

```json
{
  "businessId": "{{businessId}}",
  "mainMenuItemId": "{{menuItemId}}",
  "customerId": null
}
```
- **Response example**:

```json
{
  "message": "Success"
}
```


## 26 - Customer Voice [Public]

### Initialize Voice Session

- **Method**: `POST`
- **URL**: `{{baseUrl}}/api/CustomerVoice/session/initialize`
- **Body example**:

```json
{
  "businessId": "{{businessId}}",
  "customerId": null,
  "callSessionId": "call-session-001"
}
```
- **Response example**:

```json
{
  "message": "Success"
}
```

### Send Voice Message

- **Method**: `POST`
- **URL**: `{{baseUrl}}/api/CustomerVoice/message`
- **Body example**:

```json
{
  "businessId": "{{businessId}}",
  "customerId": null,
  "message": "I want to place an order",
  "audioData": null,
  "channel": "Voice",
  "sessionId": "call-session-001"
}
```
- **Response example**:

```json
{
  "message": "Success"
}
```

### Mark Interaction Interrupted

- **Method**: `POST`
- **URL**: `{{baseUrl}}/api/CustomerVoice/interaction/{{interactionId}}/interrupt`
- **Response example**:

```json
{
  "message": "Success"
}
```

### Submit Voice Feedback

- **Method**: `POST`
- **URL**: `{{baseUrl}}/api/CustomerVoice/feedback`
- **Body example**:

```json
{
  "interactionId": "{{interactionId}}",
  "rating": 5,
  "comment": "Very helpful voice assistant!"
}
```
- **Response example**:

```json
{
  "message": "Success"
}
```

### Get Voice Settings

- **Method**: `GET`
- **URL**: `{{baseUrl}}/api/CustomerVoice/settings/{{businessId}}`
- **Response example**:

```json
{
  "id": "<id>",
  "message": "Details object (see Swagger for full fields)."
}
```

