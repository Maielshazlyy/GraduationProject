## Frontend API Docs (Request JSON + SUCCESS Response JSON)

- **Postman source**: `DigitalEmployee_Postman_Collection.json`
- **Swagger source**: `http://localhost:9875/swagger/v1/swagger.json`
- **Responses**: Generated from Swagger `200/201` schemas (so they match the DTO contract).

## 01 - Auth

### Register Admin [PUBLIC]

- **Method**: `POST`
- **URL**: `{{baseUrl}}/api/Auth/register-admin`
- **Request body**:

```json
{
  "fullName": "Super Admin",
  "email": "admin@app.com",
  "password": "Admin@123"
}
```
- **Success response (200/201)**:

```json
{
  "message": "User registered successfully.",
  "userId": "<userId>"
}
```

### Register Owner [PUBLIC]

- **Method**: `POST`
- **URL**: `{{baseUrl}}/api/Auth/register-owner`
- **Request body**:

```json
{
  "fullName": "Business Owner",
  "email": "owner@app.com",
  "password": "Owner@123"
}
```
- **Success response (200/201)**:

```json
{
  "message": "User registered successfully.",
  "userId": "<userId>"
}
```

### Register Agent [PUBLIC - requires businessId]

- **Method**: `POST`
- **URL**: `{{baseUrl}}/api/Auth/register`
- **Request body**:

```json
{
  "fullName": "Ahmed Agent",
  "email": "agent@app.com",
  "password": "Agent@123",
  "businessId": "{{businessId}}"
}
```
- **Success response (200/201)**:

```json
{
  "message": "User registered successfully.",
  "userId": "<userId>"
}
```

### Login

- **Method**: `POST`
- **URL**: `{{baseUrl}}/api/Auth/login`
- **Request body**:

```json
{
  "email": "admin@app.com",
  "password": "Admin@123"
}
```
- **Success response (200/201)**:

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
- **Request body**:

```json
{
  "idToken": "<google_id_token_here>"
}
```
- **Success response (200/201)**:

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
- **Request body**:

```json
"{{userId}}"
```
- **Success response (200/201)**:

```json
{
  "message": "OK"
}
```

### Promote to Admin [Admin only]

- **Method**: `POST`
- **URL**: `{{baseUrl}}/api/Auth/promote-to-admin`
- **Request body**:

```json
"{{userId}}"
```
- **Success response (200/201)**:

```json
{
  "message": "OK"
}
```


## 02 - Business

### Get All Businesses [Owner/Admin]

- **Method**: `GET`
- **URL**: `{{baseUrl}}/api/Business`
- **Success response (200/201)**:

```json
{
  "message": "OK"
}
```

### Get Business by ID [Owner/Admin]

- **Method**: `GET`
- **URL**: `{{baseUrl}}/api/Business/{{businessId}}`
- **Success response (200/201)**:

```json
{
  "message": "OK"
}
```

### Create Business [Owner/Admin]

- **Method**: `POST`
- **URL**: `{{baseUrl}}/api/Business`
- **Request body**:

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
    { "dayOfWeek": 0, "openTime": null, "closeTime": null, "isClosed": true },
    { "dayOfWeek": 1, "openTime": "09:00", "closeTime": "22:00", "isClosed": false },
    { "dayOfWeek": 2, "openTime": "09:00", "closeTime": "22:00", "isClosed": false },
    { "dayOfWeek": 3, "openTime": "09:00", "closeTime": "22:00", "isClosed": false },
    { "dayOfWeek": 4, "openTime": "09:00", "closeTime": "23:00", "isClosed": false },
    { "dayOfWeek": 5, "openTime": "10:00", "closeTime": "23:00", "isClosed": false },
    { "dayOfWeek": 6, "openTime": "10:00", "closeTime": "21:00", "isClosed": false }
  ]
}
```
- **Success response (200/201)**:

```json
{
  "message": "OK"
}
```

### Onboard Business [Public]

- **Method**: `POST`
- **URL**: `{{baseUrl}}/api/Business/onboard`
- **Request body**:

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
    { "dayOfWeek": 0, "openTime": null, "closeTime": null, "isClosed": true },
    { "dayOfWeek": 1, "openTime": "09:00", "closeTime": "22:00", "isClosed": false },
    { "dayOfWeek": 2, "openTime": "09:00", "closeTime": "22:00", "isClosed": false },
    { "dayOfWeek": 3, "openTime": "09:00", "closeTime": "22:00", "isClosed": false },
    { "dayOfWeek": 4, "openTime": "09:00", "closeTime": "23:00", "isClosed": false },
    { "dayOfWeek": 5, "openTime": "10:00", "closeTime": "23:00", "isClosed": false },
    { "dayOfWeek": 6, "openTime": "10:00", "closeTime": "21:00", "isClosed": false }
  ],
  "agentName": "Luigi",
  "agentTone": "Friendly",
  "welcomeMessage": "Welcome to The Italian Place! How can we help you today?",
  "knowledgeBaseItems": [],
  "menuCategories": [
    { "name": "Main Dishes", "description": "Signature plates", "displayOrder": 1 }
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
- **Success response (200/201)**:

```json
{
  "message": "OK"
}
```

### Update Business [Owner/Admin]

- **Method**: `PUT`
- **URL**: `{{baseUrl}}/api/Business/{{businessId}}`
- **Request body**:

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
- **Success response (200/201)**:

```json
{
  "message": "OK"
}
```

### Delete Business [Admin]

- **Method**: `DELETE`
- **URL**: `{{baseUrl}}/api/Business/{{businessId}}`
- **Success response (200/201)**:

```json
{
  "message": "Deleted"
}
```


## 03 - User

### Get All Users [Admin]

- **Method**: `GET`
- **URL**: `{{baseUrl}}/api/User`
- **Success response (200/201)**:

```json
{
  "message": "OK"
}
```

### Get Users by Business [Owner/Admin]

- **Method**: `GET`
- **URL**: `{{baseUrl}}/api/User/business/{{businessId}}`
- **Success response (200/201)**:

```json
{
  "message": "OK"
}
```

### Get User by ID

- **Method**: `GET`
- **URL**: `{{baseUrl}}/api/User/{{userId}}`
- **Success response (200/201)**:

```json
{
  "message": "OK"
}
```

### Get User by Email [Admin]

- **Method**: `GET`
- **URL**: `{{baseUrl}}/api/User/email/agent@app.com`
- **Success response (200/201)**:

```json
{
  "message": "OK"
}
```

### Update User [Admin]

- **Method**: `PUT`
- **URL**: `{{baseUrl}}/api/User/{{userId}}`
- **Request body**:

```json
{
  "fullName": "Updated Name",
  "phone": "+201012345678"
}
```
- **Success response (200/201)**:

```json
{
  "message": "OK"
}
```

### Assign Role [Admin]

- **Method**: `POST`
- **URL**: `{{baseUrl}}/api/User/{{userId}}/assign-role`
- **Request body**:

```json
{
  "userId": "{{userId}}",
  "newRole": "Agent"
}
```
- **Success response (200/201)**:

```json
{
  "message": "OK"
}
```

### Create Human Employee / Agent [Owner/Admin]

- **Method**: `POST`
- **URL**: `{{baseUrl}}/api/User/agents`
- **Request body**:

```json
{
  "fullName": "New Agent",
  "email": "newagent@app.com",
  "password": "Agent@123",
  "phone": "+201098765432"
}
```
- **Success response (200/201)**:

```json
{
  "message": "OK"
}
```

### Delete User [Admin]

- **Method**: `DELETE`
- **URL**: `{{baseUrl}}/api/User/{{userId}}`
- **Success response (200/201)**:

```json
{
  "message": "Deleted"
}
```


## 04 - Customer

### Get All Customers

- **Method**: `GET`
- **URL**: `{{baseUrl}}/api/Customer`
- **Success response (200/201)**:

```json
{
  "message": "OK"
}
```

### Get Customers by Business

- **Method**: `GET`
- **URL**: `{{baseUrl}}/api/Customer/business/{{businessId}}`
- **Success response (200/201)**:

```json
{
  "message": "OK"
}
```

### Get Customer by ID

- **Method**: `GET`
- **URL**: `{{baseUrl}}/api/Customer/{{customerId}}`
- **Success response (200/201)**:

```json
{
  "message": "OK"
}
```

### Get Customer by Email

- **Method**: `GET`
- **URL**: `{{baseUrl}}/api/Customer/email/customer@example.com`
- **Success response (200/201)**:

```json
{
  "message": "OK"
}
```

### Create Customer [Owner/Admin]

- **Method**: `POST`
- **URL**: `{{baseUrl}}/api/Customer`
- **Request body**:

```json
{
  "fullName": "Ahmed Ali",
  "email": "ahmed@gmail.com",
  "phone": "+201098765432",
  "businessId": "{{businessId}}"
}
```
- **Success response (200/201)**:

```json
{
  "message": "OK"
}
```

### Update Customer [Owner/Admin]

- **Method**: `PUT`
- **URL**: `{{baseUrl}}/api/Customer/{{customerId}}`
- **Request body**:

```json
{
  "fullName": "Ahmed Ali Updated",
  "email": "ahmed.updated@gmail.com",
  "phone": "+201098765432"
}
```
- **Success response (200/201)**:

```json
{
  "message": "OK"
}
```

### Delete Customer [Admin]

- **Method**: `DELETE`
- **URL**: `{{baseUrl}}/api/Customer/{{customerId}}`
- **Success response (200/201)**:

```json
{
  "message": "Deleted"
}
```


## 05 - Dashboard [Owner] (Top products included in Analytics)

### Get Dashboard Summary

- **Method**: `GET`
- **URL**: `{{baseUrl}}/api/Dashboard/summary`
- **Success response (200/201)**:

```json
{
  "message": "OK"
}
```

### Get Analytics

- **Method**: `GET`
- **URL**: `{{baseUrl}}/api/Dashboard/analytics`
- **Success response (200/201)**:

```json
{
  "message": "OK"
}
```

### Get Full Dashboard

- **Method**: `GET`
- **URL**: `{{baseUrl}}/api/Dashboard/full`
- **Success response (200/201)**:

```json
{
  "message": "OK"
}
```

### Get Recent Audit Logs

- **Method**: `GET`
- **URL**: `{{baseUrl}}/api/Dashboard/audit-logs/recent?count=20`
- **Success response (200/201)**:

```json
{
  "message": "OK"
}
```

### Get Audit Log Statistics

- **Method**: `GET`
- **URL**: `{{baseUrl}}/api/Dashboard/audit-logs/statistics`
- **Success response (200/201)**:

```json
{
  "message": "OK"
}
```

### Get Customer Audit Logs

- **Method**: `GET`
- **URL**: `{{baseUrl}}/api/Dashboard/audit-logs/customer/{{customerId}}`
- **Success response (200/201)**:

```json
{
  "message": "OK"
}
```


## 06 - Admin Dashboard

### Get Summary

- **Method**: `GET`
- **URL**: `{{baseUrl}}/api/AdminDashboard/summary`
- **Success response (200/201)**:

```json
{
  "message": "OK"
}
```

### Get Top Businesses

- **Method**: `GET`
- **URL**: `{{baseUrl}}/api/AdminDashboard/top-businesses?count=10`
- **Success response (200/201)**:

```json
{
  "message": "OK"
}
```

### Get Full Dashboard

- **Method**: `GET`
- **URL**: `{{baseUrl}}/api/AdminDashboard/full`
- **Success response (200/201)**:

```json
{
  "message": "OK"
}
```

### Get Alerts

- **Method**: `GET`
- **URL**: `{{baseUrl}}/api/AdminDashboard/alerts`
- **Success response (200/201)**:

```json
{
  "message": "OK"
}
```

### Get Revenue Trend

- **Method**: `GET`
- **URL**: `{{baseUrl}}/api/AdminDashboard/revenue-trend?months=12`
- **Success response (200/201)**:

```json
{
  "message": "OK"
}
```

### Get Orders by Status

- **Method**: `GET`
- **URL**: `{{baseUrl}}/api/AdminDashboard/orders-by-status`
- **Success response (200/201)**:

```json
{
  "message": "OK"
}
```

### Get Tickets by Priority

- **Method**: `GET`
- **URL**: `{{baseUrl}}/api/AdminDashboard/tickets-by-priority`
- **Success response (200/201)**:

```json
{
  "message": "OK"
}
```

### Get Business Health

- **Method**: `GET`
- **URL**: `{{baseUrl}}/api/AdminDashboard/business-health?top=20&sort=desc`
- **Success response (200/201)**:

```json
{
  "message": "OK"
}
```

### Get Sentiment Trend

- **Method**: `GET`
- **URL**: `{{baseUrl}}/api/AdminDashboard/sentiment-trend?days=30`
- **Success response (200/201)**:

```json
{
  "message": "OK"
}
```

### Suspend Business

- **Method**: `POST`
- **URL**: `{{baseUrl}}/api/AdminDashboard/business/{{businessId}}/suspend`
- **Success response (200/201)**:

```json
{
  "message": "OK"
}
```

### Activate Business

- **Method**: `POST`
- **URL**: `{{baseUrl}}/api/AdminDashboard/business/{{businessId}}/activate`
- **Success response (200/201)**:

```json
{
  "message": "OK"
}
```

### Verify Business

- **Method**: `POST`
- **URL**: `{{baseUrl}}/api/AdminDashboard/business/{{businessId}}/verify`
- **Success response (200/201)**:

```json
{
  "message": "OK"
}
```

### Unverify Business

- **Method**: `POST`
- **URL**: `{{baseUrl}}/api/AdminDashboard/business/{{businessId}}/unverify`
- **Success response (200/201)**:

```json
{
  "message": "OK"
}
```


## 07 - Menu Category

### Get All Categories

- **Method**: `GET`
- **URL**: `{{baseUrl}}/api/MenuCategory`
- **Success response (200/201)**:

```json
{
  "message": "OK"
}
```

### Get Categories by Business

- **Method**: `GET`
- **URL**: `{{baseUrl}}/api/MenuCategory/business/{{businessId}}`
- **Success response (200/201)**:

```json
{
  "message": "OK"
}
```

### Get Active Categories [Public]

- **Method**: `GET`
- **URL**: `{{baseUrl}}/api/MenuCategory/business/{{businessId}}/active`
- **Success response (200/201)**:

```json
{
  "message": "OK"
}
```

### Get Category by ID

- **Method**: `GET`
- **URL**: `{{baseUrl}}/api/MenuCategory/{{menuCategoryId}}`
- **Success response (200/201)**:

```json
{
  "message": "OK"
}
```

### Create Category [Owner/Admin]

- **Method**: `POST`
- **URL**: `{{baseUrl}}/api/MenuCategory`
- **Request body**:

```json
{
  "name": "Main Dishes",
  "description": "Our signature main courses",
  "displayOrder": 1,
  "isActive": true,
  "businessId": "{{businessId}}"
}
```
- **Success response (200/201)**:

```json
{
  "message": "OK"
}
```

### Update Category [Owner/Admin]

- **Method**: `PUT`
- **URL**: `{{baseUrl}}/api/MenuCategory/{{menuCategoryId}}`
- **Request body**:

```json
{
  "name": "Starters & Salads",
  "description": "Updated",
  "displayOrder": 1,
  "isActive": true
}
```
- **Success response (200/201)**:

```json
{
  "message": "OK"
}
```

### Reorder Categories [Owner/Admin]

- **Method**: `PUT`
- **URL**: `{{baseUrl}}/api/MenuCategory/business/{{businessId}}/reorder`
- **Request body**:

```json
{
  "categoryOrders": [
    { "menuCategoryId": "{{menuCategoryId}}", "displayOrder": 1 }
  ]
}
```
- **Success response (200/201)**:

```json
{
  "message": "OK"
}
```

### Delete Category [Owner/Admin]

- **Method**: `DELETE`
- **URL**: `{{baseUrl}}/api/MenuCategory/{{menuCategoryId}}`
- **Success response (200/201)**:

```json
{
  "message": "Deleted"
}
```


## 08 - Menu Item

### Get All Menu Items

- **Method**: `GET`
- **URL**: `{{baseUrl}}/api/MenuItem`
- **Success response (200/201)**:

```json
{
  "message": "OK"
}
```

### Get Menu Items by Business

- **Method**: `GET`
- **URL**: `{{baseUrl}}/api/MenuItem/business/{{businessId}}`
- **Success response (200/201)**:

```json
{
  "message": "OK"
}
```

### Get Menu Item by ID

- **Method**: `GET`
- **URL**: `{{baseUrl}}/api/MenuItem/{{menuItemId}}`
- **Success response (200/201)**:

```json
{
  "message": "OK"
}
```

### Create Menu Item [Owner/Admin]

- **Method**: `POST`
- **URL**: `{{baseUrl}}/api/MenuItem`
- **Request body**:

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
- **Success response (200/201)**:

```json
{
  "message": "OK"
}
```

### Update Menu Item [Owner/Admin]

- **Method**: `PUT`
- **URL**: `{{baseUrl}}/api/MenuItem/{{menuItemId}}`
- **Request body**:

```json
{
  "name": "Margherita Pizza XL",
  "description": "Extra large classic pizza",
  "price": 109.99,
  "isAvailable": true
}
```
- **Success response (200/201)**:

```json
{
  "message": "OK"
}
```

### Delete Menu Item [Owner/Admin]

- **Method**: `DELETE`
- **URL**: `{{baseUrl}}/api/MenuItem/{{menuItemId}}`
- **Success response (200/201)**:

```json
{
  "message": "Deleted"
}
```


## 09 - Order

### Get All Orders

- **Method**: `GET`
- **URL**: `{{baseUrl}}/api/Order`
- **Success response (200/201)**:

```json
{
  "message": "OK"
}
```

### Get Orders by Business

- **Method**: `GET`
- **URL**: `{{baseUrl}}/api/Order/business/{{businessId}}`
- **Success response (200/201)**:

```json
{
  "message": "OK"
}
```

### Get Orders by Customer

- **Method**: `GET`
- **URL**: `{{baseUrl}}/api/Order/customer/{{customerId}}`
- **Success response (200/201)**:

```json
{
  "message": "OK"
}
```

### Get Order by ID

- **Method**: `GET`
- **URL**: `{{baseUrl}}/api/Order/{{orderId}}`
- **Success response (200/201)**:

```json
{
  "message": "OK"
}
```

### Create Order

- **Method**: `POST`
- **URL**: `{{baseUrl}}/api/Order`
- **Request body**:

```json
{
  "businessId": "{{businessId}}",
  "customerId": "{{customerId}}",
  "items": [
    { "menuItemId": "{{menuItemId}}", "quantity": 2 }
  ]
}
```
- **Success response (200/201)**:

```json
{
  "message": "OK"
}
```

### Update Order Status [Owner/Admin]

- **Method**: `PUT`
- **URL**: `{{baseUrl}}/api/Order/{{orderId}}/status`
- **Request body**:

```json
{
  "orderId": "{{orderId}}",
  "status": "Paid"
}
```
- **Success response (200/201)**:

```json
{
  "message": "OK"
}
```

### Delete Order [Admin]

- **Method**: `DELETE`
- **URL**: `{{baseUrl}}/api/Order/{{orderId}}`
- **Success response (200/201)**:

```json
{
  "message": "Deleted"
}
```


## 10 - Ticket

### Get All Tickets

- **Method**: `GET`
- **URL**: `{{baseUrl}}/api/Ticket`
- **Success response (200/201)**:

```json
{
  "message": "OK"
}
```

### Get Escalation Queue

- **Method**: `GET`
- **URL**: `{{baseUrl}}/api/Ticket/queue`
- **Success response (200/201)**:

```json
{
  "message": "OK"
}
```

### Get Tickets by Business

- **Method**: `GET`
- **URL**: `{{baseUrl}}/api/Ticket/business/{{businessId}}`
- **Success response (200/201)**:

```json
{
  "message": "OK"
}
```

### Get Ticket by ID

- **Method**: `GET`
- **URL**: `{{baseUrl}}/api/Ticket/{{ticketId}}`
- **Success response (200/201)**:

```json
{
  "message": "OK"
}
```

### Create Ticket

- **Method**: `POST`
- **URL**: `{{baseUrl}}/api/Ticket`
- **Request body**:

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
- **Success response (200/201)**:

```json
{
  "message": "OK"
}
```

### Update Ticket

- **Method**: `PUT`
- **URL**: `{{baseUrl}}/api/Ticket/{{ticketId}}`
- **Request body**:

```json
{
  "subject": "Updated subject",
  "description": "Updated description",
  "priority": "Medium",
  "status": "InProgress"
}
```
- **Success response (200/201)**:

```json
{
  "message": "OK"
}
```

### Assign Ticket [Owner/Admin]

- **Method**: `POST`
- **URL**: `{{baseUrl}}/api/Ticket/{{ticketId}}/assign`
- **Request body**:

```json
{
  "assignedToUserId": "{{userId}}"
}
```
- **Success response (200/201)**:

```json
{
  "message": "OK"
}
```

### Close Ticket

- **Method**: `POST`
- **URL**: `{{baseUrl}}/api/Ticket/{{ticketId}}/close`
- **Request body**:

```json
{
  "resolutionNote": "Issue resolved. Customer refunded.",
  "closedByUserId": "{{userId}}"
}
```
- **Success response (200/201)**:

```json
{
  "message": "OK"
}
```

### Delete Ticket [Admin]

- **Method**: `DELETE`
- **URL**: `{{baseUrl}}/api/Ticket/{{ticketId}}`
- **Success response (200/201)**:

```json
{
  "message": "Deleted"
}
```


## 11 - Interaction

### Get All Interactions

- **Method**: `GET`
- **URL**: `{{baseUrl}}/api/Interaction`
- **Success response (200/201)**:

```json
{
  "message": "OK"
}
```

### Get Interactions by Business

- **Method**: `GET`
- **URL**: `{{baseUrl}}/api/Interaction/business/{{businessId}}`
- **Success response (200/201)**:

```json
{
  "message": "OK"
}
```

### Get Interactions by Customer

- **Method**: `GET`
- **URL**: `{{baseUrl}}/api/Interaction/customer/{{customerId}}`
- **Success response (200/201)**:

```json
{
  "message": "OK"
}
```

### Get Interactions by User

- **Method**: `GET`
- **URL**: `{{baseUrl}}/api/Interaction/user/{{userId}}`
- **Success response (200/201)**:

```json
{
  "message": "OK"
}
```

### Get Interaction by ID

- **Method**: `GET`
- **URL**: `{{baseUrl}}/api/Interaction/{{interactionId}}`
- **Success response (200/201)**:

```json
{
  "message": "OK"
}
```

### Start Interaction

- **Method**: `POST`
- **URL**: `{{baseUrl}}/api/Interaction/start`
- **Request body**:

```json
{
  "businessId": "{{businessId}}",
  "customerId": "{{customerId}}",
  "channel": "WebChat",
  "assignedUserId": null
}
```
- **Success response (200/201)**:

```json
{
  "message": "OK"
}
```

### End Interaction

- **Method**: `POST`
- **URL**: `{{baseUrl}}/api/Interaction/{{interactionId}}/end`
- **Request body**:

```json
{
  "resolutionStatus": "Resolved",
  "notes": "Customer satisfied"
}
```
- **Success response (200/201)**:

```json
{
  "message": "OK"
}
```

### Delete Interaction [Admin]

- **Method**: `DELETE`
- **URL**: `{{baseUrl}}/api/Interaction/{{interactionId}}`
- **Success response (200/201)**:

```json
{
  "message": "Deleted"
}
```


## 12 - Message

### Get All Messages

- **Method**: `GET`
- **URL**: `{{baseUrl}}/api/Message`
- **Success response (200/201)**:

```json
{
  "message": "OK"
}
```

### Get Messages by Interaction

- **Method**: `GET`
- **URL**: `{{baseUrl}}/api/Message/interaction/{{interactionId}}`
- **Success response (200/201)**:

```json
{
  "message": "OK"
}
```

### Get Message by ID

- **Method**: `GET`
- **URL**: `{{baseUrl}}/api/Message/{{messageId}}`
- **Success response (200/201)**:

```json
{
  "message": "OK"
}
```

### Create Message

- **Method**: `POST`
- **URL**: `{{baseUrl}}/api/Message`
- **Request body**:

```json
{
  "interactionId": "{{interactionId}}",
  "content": "Hello, how can I help you?",
  "senderType": "Agent",
  "senderId": "{{userId}}"
}
```
- **Success response (200/201)**:

```json
{
  "message": "OK"
}
```

### Delete Message [Admin]

- **Method**: `DELETE`
- **URL**: `{{baseUrl}}/api/Message/{{messageId}}`
- **Success response (200/201)**:

```json
{
  "message": "Deleted"
}
```


## 13 - Knowledge Base

### Get All KB Items

- **Method**: `GET`
- **URL**: `{{baseUrl}}/api/KnowledgeBase`
- **Success response (200/201)**:

```json
{
  "message": "OK"
}
```

### Get KB Items by Business

- **Method**: `GET`
- **URL**: `{{baseUrl}}/api/KnowledgeBase/business/{{businessId}}`
- **Success response (200/201)**:

```json
{
  "message": "OK"
}
```

### Get KB Item by ID

- **Method**: `GET`
- **URL**: `{{baseUrl}}/api/KnowledgeBase/{{knowledgeBaseId}}`
- **Success response (200/201)**:

```json
{
  "message": "OK"
}
```

### Create KB Item [Owner/Admin]

- **Method**: `POST`
- **URL**: `{{baseUrl}}/api/KnowledgeBase`
- **Request body**:

```json
{
  "title": "How to place an order",
  "content": "To place an order, browse our menu and add items to your cart...",
  "businessId": "{{businessId}}",
  "isFAQ": false,
  "tags": "ordering, how-to, menu"
}
```
- **Success response (200/201)**:

```json
{
  "message": "OK"
}
```

### Update KB Item [Owner/Admin]

- **Method**: `PUT`
- **URL**: `{{baseUrl}}/api/KnowledgeBase/{{knowledgeBaseId}}`
- **Request body**:

```json
{
  "title": "Updated title",
  "content": "Updated content here.",
  "businessId": "{{businessId}}",
  "isFAQ": false,
  "tags": "updated"
}
```
- **Success response (200/201)**:

```json
{
  "message": "OK"
}
```

### Delete KB Item [Owner/Admin]

- **Method**: `DELETE`
- **URL**: `{{baseUrl}}/api/KnowledgeBase/{{knowledgeBaseId}}`
- **Success response (200/201)**:

```json
{
  "message": "Deleted"
}
```


## 14 - FAQ

### Get FAQs by Business [Public]

- **Method**: `GET`
- **URL**: `{{baseUrl}}/api/FAQ/business/{{businessId}}`
- **Success response (200/201)**:

```json
{
  "message": "OK"
}
```

### Manage FAQs [Owner/Admin]

- **Method**: `GET`
- **URL**: `{{baseUrl}}/api/FAQ/business/{{businessId}}/manage`
- **Success response (200/201)**:

```json
{
  "message": "OK"
}
```

### Get FAQ by ID [Public]

- **Method**: `GET`
- **URL**: `{{baseUrl}}/api/FAQ/{{faqId}}`
- **Success response (200/201)**:

```json
{
  "message": "OK"
}
```

### Create FAQ [Owner/Admin]

- **Method**: `POST`
- **URL**: `{{baseUrl}}/api/FAQ`
- **Request body**:

```json
{
  "title": "What are your opening hours?",
  "content": "We are open from 9 AM to 10 PM, Monday to Saturday. Closed on Sundays.",
  "businessId": "{{businessId}}",
  "isFAQ": true,
  "tags": "hours, schedule, open"
}
```
- **Success response (200/201)**:

```json
{
  "message": "OK"
}
```

### Update FAQ [Owner/Admin]

- **Method**: `PUT`
- **URL**: `{{baseUrl}}/api/FAQ/{{faqId}}`
- **Request body**:

```json
{
  "title": "Updated FAQ question?",
  "content": "Updated FAQ answer.",
  "businessId": "{{businessId}}",
  "isFAQ": true
}
```
- **Success response (200/201)**:

```json
{
  "message": "OK"
}
```

### Delete FAQ [Owner/Admin]

- **Method**: `DELETE`
- **URL**: `{{baseUrl}}/api/FAQ/{{faqId}}`
- **Success response (200/201)**:

```json
{
  "message": "Deleted"
}
```


## 15 - Feedback

### Get All Feedback

- **Method**: `GET`
- **URL**: `{{baseUrl}}/api/Feedback`
- **Success response (200/201)**:

```json
{
  "message": "OK"
}
```

### Get Feedback by Customer

- **Method**: `GET`
- **URL**: `{{baseUrl}}/api/Feedback/customer/{{customerId}}`
- **Success response (200/201)**:

```json
{
  "message": "OK"
}
```

### Get Feedback by ID

- **Method**: `GET`
- **URL**: `{{baseUrl}}/api/Feedback/{{feedbackId}}`
- **Success response (200/201)**:

```json
{
  "message": "OK"
}
```

### Submit Feedback [Public]

- **Method**: `POST`
- **URL**: `{{baseUrl}}/api/Feedback`
- **Request body**:

```json
{
  "customerId": "{{customerId}}",
  "interactionId": "{{interactionId}}",
  "rating": 5,
  "comment": "Excellent service and food!",
  "businessId": "{{businessId}}"
}
```
- **Success response (200/201)**:

```json
{
  "message": "OK"
}
```

### Update Feedback [Owner/Admin]

- **Method**: `PUT`
- **URL**: `{{baseUrl}}/api/Feedback/{{feedbackId}}`
- **Request body**:

```json
{
  "rating": 4,
  "comment": "Updated comment"
}
```
- **Success response (200/201)**:

```json
{
  "message": "OK"
}
```

### Delete Feedback [Admin]

- **Method**: `DELETE`
- **URL**: `{{baseUrl}}/api/Feedback/{{feedbackId}}`
- **Success response (200/201)**:

```json
{
  "message": "Deleted"
}
```


## 16 - Sentiment [Read Only]

### Get All Sentiments

- **Method**: `GET`
- **URL**: `{{baseUrl}}/api/Sentiment`
- **Success response (200/201)**:

```json
{
  "message": "OK"
}
```

### Get Sentiments by Message

- **Method**: `GET`
- **URL**: `{{baseUrl}}/api/Sentiment/message/{{messageId}}`
- **Success response (200/201)**:

```json
{
  "message": "OK"
}
```

### Get Sentiments by Business

- **Method**: `GET`
- **URL**: `{{baseUrl}}/api/Sentiment/business/{{businessId}}`
- **Success response (200/201)**:

```json
{
  "message": "OK"
}
```

### Get Sentiment by ID

- **Method**: `GET`
- **URL**: `{{baseUrl}}/api/Sentiment/{{sentimentId}}`
- **Success response (200/201)**:

```json
{
  "message": "OK"
}
```


## 17 - Notification

### Get All Notifications

- **Method**: `GET`
- **URL**: `{{baseUrl}}/api/Notification`
- **Success response (200/201)**:

```json
{
  "message": "OK"
}
```

### Get Notifications by Business

- **Method**: `GET`
- **URL**: `{{baseUrl}}/api/Notification/business/{{businessId}}`
- **Success response (200/201)**:

```json
{
  "message": "OK"
}
```

### Get Notifications by User

- **Method**: `GET`
- **URL**: `{{baseUrl}}/api/Notification/user/{{userId}}`
- **Success response (200/201)**:

```json
{
  "message": "OK"
}
```

### Get Notification by ID

- **Method**: `GET`
- **URL**: `{{baseUrl}}/api/Notification/{{notificationId}}`
- **Success response (200/201)**:

```json
{
  "message": "OK"
}
```

### Create Notification [Owner/Admin]

- **Method**: `POST`
- **URL**: `{{baseUrl}}/api/Notification`
- **Request body**:

```json
{
  "businessId": "{{businessId}}",
  "userId": "{{userId}}",
  "title": "New Order Received",
  "message": "Order #1234 has been placed and is awaiting confirmation.",
  "type": "Order"
}
```
- **Success response (200/201)**:

```json
{
  "message": "OK"
}
```

### Mark as Read

- **Method**: `PUT`
- **URL**: `{{baseUrl}}/api/Notification/{{notificationId}}/read`
- **Success response (200/201)**:

```json
{
  "message": "OK"
}
```

### Delete Notification [Owner/Admin]

- **Method**: `DELETE`
- **URL**: `{{baseUrl}}/api/Notification/{{notificationId}}`
- **Success response (200/201)**:

```json
{
  "message": "Deleted"
}
```


## 18 - Report

### Get All Reports

- **Method**: `GET`
- **URL**: `{{baseUrl}}/api/Report`
- **Success response (200/201)**:

```json
{
  "message": "OK"
}
```

### Get Reports by Business

- **Method**: `GET`
- **URL**: `{{baseUrl}}/api/Report/business/{{businessId}}`
- **Success response (200/201)**:

```json
{
  "message": "OK"
}
```

### Get Report by ID

- **Method**: `GET`
- **URL**: `{{baseUrl}}/api/Report/{{reportId}}`
- **Success response (200/201)**:

```json
{
  "message": "OK"
}
```

### Create Report [Owner/Admin]

- **Method**: `POST`
- **URL**: `{{baseUrl}}/api/Report`
- **Request body**:

```json
{
  "businessId": "{{businessId}}",
  "type": "Sales",
  "period": "Monthly",
  "startDate": "2026-03-01T00:00:00Z",
  "endDate": "2026-03-31T23:59:59Z"
}
```
- **Success response (200/201)**:

```json
{
  "message": "OK"
}
```

### Delete Report [Admin]

- **Method**: `DELETE`
- **URL**: `{{baseUrl}}/api/Report/{{reportId}}`
- **Success response (200/201)**:

```json
{
  "message": "Deleted"
}
```


## 19 - Subscription

### Get All Subscriptions [Admin]

- **Method**: `GET`
- **URL**: `{{baseUrl}}/api/Subscription`
- **Success response (200/201)**:

```json
{
  "message": "OK"
}
```

### Get Subscriptions by Business [Owner/Admin]

- **Method**: `GET`
- **URL**: `{{baseUrl}}/api/Subscription/business/{{businessId}}`
- **Success response (200/201)**:

```json
{
  "message": "OK"
}
```

### Get Active Subscription [Owner/Admin]

- **Method**: `GET`
- **URL**: `{{baseUrl}}/api/Subscription/business/{{businessId}}/active`
- **Success response (200/201)**:

```json
{
  "message": "OK"
}
```

### Get Subscription by ID [Owner/Admin]

- **Method**: `GET`
- **URL**: `{{baseUrl}}/api/Subscription/{{subscriptionId}}`
- **Success response (200/201)**:

```json
{
  "message": "OK"
}
```

### Create Subscription [Admin]

- **Method**: `POST`
- **URL**: `{{baseUrl}}/api/Subscription`
- **Request body**:

```json
{
  "businessId": "{{businessId}}",
  "plan": "Pro",
  "startDate": "2026-03-01T00:00:00Z",
  "endDate": "2026-04-01T00:00:00Z",
  "monthlyPrice": 49.99
}
```
- **Success response (200/201)**:

```json
{
  "message": "OK"
}
```

### Renew Subscription [Admin]

- **Method**: `POST`
- **URL**: `{{baseUrl}}/api/Subscription/{{subscriptionId}}/renew`
- **Request body**:

```json
{
  "newEndDate": "2026-05-01T00:00:00Z"
}
```
- **Success response (200/201)**:

```json
{
  "message": "OK"
}
```

### Delete Subscription [Admin]

- **Method**: `DELETE`
- **URL**: `{{baseUrl}}/api/Subscription/{{subscriptionId}}`
- **Success response (200/201)**:

```json
{
  "message": "Deleted"
}
```


## 20 - Payment Transaction

### Get All Payments [Admin]

- **Method**: `GET`
- **URL**: `{{baseUrl}}/api/PaymentTransaction`
- **Success response (200/201)**:

```json
{
  "message": "OK"
}
```

### Get Payments by Subscription [Owner/Admin]

- **Method**: `GET`
- **URL**: `{{baseUrl}}/api/PaymentTransaction/subscription/{{subscriptionId}}`
- **Success response (200/201)**:

```json
{
  "message": "OK"
}
```

### Get Payments by Business [Owner/Admin]

- **Method**: `GET`
- **URL**: `{{baseUrl}}/api/PaymentTransaction/business/{{businessId}}`
- **Success response (200/201)**:

```json
{
  "message": "OK"
}
```

### Get Payment by ID [Owner/Admin]

- **Method**: `GET`
- **URL**: `{{baseUrl}}/api/PaymentTransaction/{{paymentId}}`
- **Success response (200/201)**:

```json
{
  "message": "OK"
}
```

### Create Payment [Admin]

- **Method**: `POST`
- **URL**: `{{baseUrl}}/api/PaymentTransaction`
- **Request body**:

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
- **Success response (200/201)**:

```json
{
  "message": "OK"
}
```

### Delete Payment [Admin]

- **Method**: `DELETE`
- **URL**: `{{baseUrl}}/api/PaymentTransaction/{{paymentId}}`
- **Success response (200/201)**:

```json
{
  "message": "Deleted"
}
```


## 21 - Integration

### Get All Integrations [Admin]

- **Method**: `GET`
- **URL**: `{{baseUrl}}/api/Integration`
- **Success response (200/201)**:

```json
{
  "message": "OK"
}
```

### Get Integrations by Business [Owner/Admin]

- **Method**: `GET`
- **URL**: `{{baseUrl}}/api/Integration/business/{{businessId}}`
- **Success response (200/201)**:

```json
{
  "message": "OK"
}
```

### Get Integration by ID [Owner/Admin]

- **Method**: `GET`
- **URL**: `{{baseUrl}}/api/Integration/{{integrationId}}`
- **Success response (200/201)**:

```json
{
  "message": "OK"
}
```

### Connect Integration [Owner/Admin]

- **Method**: `POST`
- **URL**: `{{baseUrl}}/api/Integration/connect`
- **Request body**:

```json
{
  "businessId": "{{businessId}}",
  "platform": "WhatsApp",
  "apiKey": "your_api_key_here",
  "webhookUrl": "https://yourapp.com/webhook"
}
```
- **Success response (200/201)**:

```json
{
  "message": "OK"
}
```

### Sync Integration [Owner/Admin]

- **Method**: `POST`
- **URL**: `{{baseUrl}}/api/Integration/{{integrationId}}/sync`
- **Request body**:

```json
{
  "syncType": "Full"
}
```
- **Success response (200/201)**:

```json
{
  "message": "OK"
}
```

### Delete Integration [Owner/Admin]

- **Method**: `DELETE`
- **URL**: `{{baseUrl}}/api/Integration/{{integrationId}}`
- **Success response (200/201)**:

```json
{
  "message": "Deleted"
}
```


## 22 - Setting

### Get Settings by Business [Owner/Admin]

- **Method**: `GET`
- **URL**: `{{baseUrl}}/api/Setting/business/{{businessId}}`
- **Success response (200/201)**:

```json
{
  "message": "OK"
}
```

### Update Settings [Owner/Admin]

- **Method**: `PUT`
- **URL**: `{{baseUrl}}/api/Setting/business/{{businessId}}`
- **Request body**:

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
- **Success response (200/201)**:

```json
{
  "message": "OK"
}
```


## 23 - Audit Log [Read Only]

### Get All Audit Logs [Admin]

- **Method**: `GET`
- **URL**: `{{baseUrl}}/api/AuditLog`
- **Success response (200/201)**:

```json
{
  "message": "OK"
}
```

### Get Audit Logs by Business [Owner/Admin]

- **Method**: `GET`
- **URL**: `{{baseUrl}}/api/AuditLog/business/{{businessId}}`
- **Success response (200/201)**:

```json
{
  "message": "OK"
}
```

### Get Audit Logs by User [Admin]

- **Method**: `GET`
- **URL**: `{{baseUrl}}/api/AuditLog/user/{{userId}}`
- **Success response (200/201)**:

```json
{
  "message": "OK"
}
```

### Get Audit Log by ID [Admin]

- **Method**: `GET`
- **URL**: `{{baseUrl}}/api/AuditLog/{{auditLogId}}`
- **Success response (200/201)**:

```json
{
  "message": "OK"
}
```


## 24 - Chatbot [AI]

### Ask Question [Owner/Admin]

- **Method**: `POST`
- **URL**: `{{baseUrl}}/api/Chatbot/ask`
- **Request body**:

```json
{
  "question": "What is my business performance overview?",
  "conversationId": null
}
```
- **Success response (200/201)**:

```json
{
  "message": "OK"
}
```

### Get Suggestions [Owner/Admin]

- **Method**: `GET`
- **URL**: `{{baseUrl}}/api/Chatbot/suggestions`
- **Success response (200/201)**:

```json
{
  "message": "OK"
}
```


## 25 - Customer Chat [Public]

### Get Business Capabilities

- **Method**: `GET`
- **URL**: `{{baseUrl}}/api/CustomerChat/capabilities/{{businessId}}`
- **Success response (200)**:

```json
{
  "businessId": "<string>",
  "businessName": "<string>",
  "chatEnabled": true,
  "voiceEnabled": true,
  "welcomeMessage": "<string>",
  "voiceSettings": {
    "agentVoice": "<string>",
    "agentVoiceProvider": "<string>",
    "agentVoiceSpeed": 0.0,
    "agentVoicePitch": 0.0,
    "agentVoiceLanguage": "<string>"
  }
}
```

### Send Chat Message

- **Method**: `POST`
- **URL**: `{{baseUrl}}/api/CustomerChat/message`
- **Request body**:

```json
{
  "businessId": "{{businessId}}",
  "customerId": null,
  "message": "What are your opening hours?",
  "channel": "WebChat",
  "sessionId": "session-001"
}
```
- **Success response (200)**:

```json
{
  "interactionId": "<string>",
  "replyText": "<string>",
  "replyAudio": "<string>",
  "replyAudioFormat": "<string>",
  "orderId": "<string>",
  "ticketId": "<string>",
  "cart": {
    "totalPrice": 0.0,
    "items": [
      null
    ]
  },
  "recommendations": [
    {
      "menuItemId": "<string>",
      "name": "<string>",
      "price": 0.0,
      "reason": "<string>"
    }
  ],
  "hasDeliveryDelay": true,
  "alternativeTimeSlots": [
    "<string>"
  ],
  "isInterrupted": true
}
```

### Get Order Recommendations

- **Method**: `POST`
- **URL**: `{{baseUrl}}/api/CustomerChat/recommendations`
- **Request body**:

```json
{
  "businessId": "{{businessId}}",
  "mainMenuItemId": "{{menuItemId}}",
  "customerId": null
}
```
- **Success response (200)**:

```json
[
  {
    "menuItemId": "<string>",
    "name": "<string>",
    "price": 0.0,
    "reason": "<string>"
  }
]
```


## 26 - Customer Voice [Public]

### Initialize Voice Session

- **Method**: `POST`
- **URL**: `{{baseUrl}}/api/CustomerVoice/session/initialize`
- **Request body**:

```json
{
  "businessId": "{{businessId}}",
  "customerId": null,
  "callSessionId": "call-session-001"
}
```
- **Success response (200)**:

```json
{
  "interactionId": "<string>",
  "channel": "<string>",
  "interactionType": "<string>",
  "status": "<string>",
  "isEnded": true,
  "startedAt": "2026-04-14T12:00:00Z",
  "endedAt": "2026-04-14T12:00:00Z",
  "callSessionId": "<string>",
  "businessId": "<string>",
  "business": {
    "id": "<string>",
    "businessId": "<string>",
    "name": "<string>",
    "type": "<string>",
    "address": "<string>",
    "phone": "<string>",
    "email": "<string>",
    "website": "<string>",
    "facebookUrl": "<string>",
    "instagramUrl": "<string>",
    "city": "<string>",
    "country": "<string>",
    "latitude": 0.0,
    "longitude": 0.0,
    "description": "<string>",
    "cuisineType": "<string>",
    "priceRange": "<string>",
    "logoUrl": "<string>",
    "coverImageUrl": "<string>",
    "hasDelivery": true,
    "hasTakeout": true,
    "hasParking": true,
    "hasWiFi": true,
    "hasOutdoorSeating": true,
    "acceptsReservations": true,
    "paymentMethods": "<string>",
    "isActive": true,
    "isVerified": true,
    "createdAt": "2026-04-14T12:00:00Z",
    "users": [
      null
    ],
    "customers": [
      null
    ],
    "menuItems": [
      null
    ],
    "orders": [
      null
    ],
    "tickets": [
      null
    ],
    "interactions": [
      null
    ],
    "notifications": [
      null
    ],
    "reports": [
      null
    ],
    "knowledgeBases": [
      null
    ],
    "menuCategories": [
      null
    ],
    "workingHours": [
      null
    ],
    "setting": {
      "settingId": null,
      "autoAssignTickets": null,
      "enableNotifications": null,
      "language": null,
      "timeZone": null,
      "chatbotEnabled": null,
      "chatbotWelcomeMessage": null,
      "chatbotPersonality": null,
      "agentVoice": null,
      "agentVoiceProvider": null,
      "agentVoiceSpeed": null,
      "agentVoicePitch": null,
      "agentVoiceLanguage": null,
      "customSystemPrompt": null,
      "customGreetingTemplate": null,
      "emailNotifications": null,
      "smsNotifications": null,
      "pushNotifications": null,
      "businessId": null,
      "business": null
    },
    "subscriptions": [
      null
    ],
    "integrations": [
      null
    ],
    "auditLogs": [
      null
    ]
  },
  "handledByUserId": "<string>",
  "handledByUser": {
    "id": "<string>",
    "userName": "<string>",
    "normalizedUserName": "<string>",
    "email": "<string>",
    "normalizedEmail": "<string>",
    "emailConfirmed": true,
    "passwordHash": "<string>",
    "securityStamp": "<string>",
    "concurrencyStamp": "<string>",
    "phoneNumber": "<string>",
    "phoneNumberConfirmed": true,
    "twoFactorEnabled": true,
    "lockoutEnd": "2026-04-14T12:00:00Z",
    "lockoutEnabled": true,
    "accessFailedCount": 0,
    "fullName": "<string>",
    "role": "<string>",
    "createdAt": "2026-04-14T12:00:00Z",
    "businessId": "<string>",
    "business": {
      "id": null,
      "businessId": null,
      "name": null,
      "type": null,
      "address": null,
      "phone": null,
      "email": null,
      "website": null,
      "facebookUrl": null,
      "instagramUrl": null,
      "city": null,
      "country": null,
      "latitude": null,
      "longitude": null,
      "description": null,
      "cuisineType": null,
      "priceRange": null,
      "logoUrl": null,
      "coverImageUrl": null,
      "hasDelivery": null,
      "hasTakeout": null,
      "hasParking": null,
      "hasWiFi": null,
      "hasOutdoorSeating": null,
      "acceptsReservations": null,
      "paymentMethods": null,
      "isActive": null,
      "isVerified": null,
      "createdAt": null,
      "users": null,
      "customers": null,
      "menuItems": null,
      "orders": null,
      "tickets": null,
      "interactions": null,
      "notifications": null,
      "reports": null,
      "knowledgeBases": null,
      "menuCategories": null,
      "workingHours": null,
      "setting": null,
      "subscriptions": null,
      "integrations": null,
      "auditLogs": null
    },
    "interactionsHandled": [
      null
    ],
    "ticketsAssigned": [
      null
    ],
    "auditLogs": [
      null
    ],
    "notifications": [
      null
    ],
    "messages": [
      null
    ]
  },
  "customerId": "<string>",
  "customer": {
    "customerId": "<string>",
    "fullName": "<string>",
    "email": "<string>",
    "phone": "<string>",
    "createdAt": "2026-04-14T12:00:00Z",
    "businessId": "<string>",
    "business": {
      "id": null,
      "businessId": null,
      "name": null,
      "type": null,
      "address": null,
      "phone": null,
      "email": null,
      "website": null,
      "facebookUrl": null,
      "instagramUrl": null,
      "city": null,
      "country": null,
      "latitude": null,
      "longitude": null,
      "description": null,
      "cuisineType": null,
      "priceRange": null,
      "logoUrl": null,
      "coverImageUrl": null,
      "hasDelivery": null,
      "hasTakeout": null,
      "hasParking": null,
      "hasWiFi": null,
      "hasOutdoorSeating": null,
      "acceptsReservations": null,
      "paymentMethods": null,
      "isActive": null,
      "isVerified": null,
      "createdAt": null,
      "users": null,
      "customers": null,
      "menuItems": null,
      "orders": null,
      "tickets": null,
      "interactions": null,
      "notifications": null,
      "reports": null,
      "knowledgeBases": null,
      "menuCategories": null,
      "workingHours": null,
      "setting": null,
      "subscriptions": null,
      "integrations": null,
      "auditLogs": null
    },
    "orders": [
      null
    ],
    "tickets": [
      null
    ],
    "interactions": [
      null
    ],
    "feedbacks": [
      null
    ]
  },
  "relatedOrderId": "<string>",
  "relatedTicketId": "<string>",
  "messages": [
    {
      "messageId": "<string>",
      "senderType": "<string>",
      "content": "<string>",
      "sentAt": "2026-04-14T12:00:00Z",
      "interactionId": "<string>",
      "interaction": null,
      "userId": "<string>",
      "user": null,
      "intent": "<string>",
      "aiMetadataJson": "<string>",
      "audioPath": "<string>",
      "confidenceScore": 0.0,
      "sentiment": null
    }
  ]
}
```

### Send Voice Message

- **Method**: `POST`
- **URL**: `{{baseUrl}}/api/CustomerVoice/message`
- **Request body**:

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
- **Success response (200)**:

```json
{
  "interactionId": "<string>",
  "replyText": "<string>",
  "replyAudio": "<string>",
  "replyAudioFormat": "<string>",
  "orderId": "<string>",
  "ticketId": "<string>",
  "cart": {
    "totalPrice": 0.0,
    "items": [
      null
    ]
  },
  "recommendations": [
    {
      "menuItemId": "<string>",
      "name": "<string>",
      "price": 0.0,
      "reason": "<string>"
    }
  ],
  "hasDeliveryDelay": true,
  "alternativeTimeSlots": [
    "<string>"
  ],
  "isInterrupted": true
}
```

### Mark Interaction Interrupted

- **Method**: `POST`
- **URL**: `{{baseUrl}}/api/CustomerVoice/interaction/{{interactionId}}/interrupt`
- **Success response (200/201)**:

```json
{
  "message": "OK"
}
```

### Submit Voice Feedback

- **Method**: `POST`
- **URL**: `{{baseUrl}}/api/CustomerVoice/feedback`
- **Request body**:

```json
{
  "interactionId": "{{interactionId}}",
  "rating": 5,
  "comment": "Very helpful voice assistant!"
}
```
- **Success response (200)**:

```json
{
  "feedbackId": "<string>",
  "comment": "<string>",
  "rating": 0,
  "createdAt": "2026-04-14T12:00:00Z",
  "ticketId": "<string>",
  "ticket": {
    "id": "<string>",
    "ticketId": "<string>",
    "subject": "<string>",
    "status": "<string>",
    "isEnded": true,
    "createdAt": "2026-04-14T12:00:00Z",
    "closedAt": "2026-04-14T12:00:00Z",
    "ticketType": "<string>",
    "priorityLevel": "<string>",
    "escalationConfidence": 0.0,
    "escalationReason": "<string>",
    "interactionId": "<string>",
    "relatedOrderId": "<string>",
    "feedbacks": [
      null
    ],
    "businessId": "<string>",
    "business": {
      "id": null,
      "businessId": null,
      "name": null,
      "type": null,
      "address": null,
      "phone": null,
      "email": null,
      "website": null,
      "facebookUrl": null,
      "instagramUrl": null,
      "city": null,
      "country": null,
      "latitude": null,
      "longitude": null,
      "description": null,
      "cuisineType": null,
      "priceRange": null,
      "logoUrl": null,
      "coverImageUrl": null,
      "hasDelivery": null,
      "hasTakeout": null,
      "hasParking": null,
      "hasWiFi": null,
      "hasOutdoorSeating": null,
      "acceptsReservations": null,
      "paymentMethods": null,
      "isActive": null,
      "isVerified": null,
      "createdAt": null,
      "users": null,
      "customers": null,
      "menuItems": null,
      "orders": null,
      "tickets": null,
      "interactions": null,
      "notifications": null,
      "reports": null,
      "knowledgeBases": null,
      "menuCategories": null,
      "workingHours": null,
      "setting": null,
      "subscriptions": null,
      "integrations": null,
      "auditLogs": null
    },
    "assignedToUserId": "<string>",
    "assignedToUser": {
      "id": null,
      "userName": null,
      "normalizedUserName": null,
      "email": null,
      "normalizedEmail": null,
      "emailConfirmed": null,
      "passwordHash": null,
      "securityStamp": null,
      "concurrencyStamp": null,
      "phoneNumber": null,
      "phoneNumberConfirmed": null,
      "twoFactorEnabled": null,
      "lockoutEnd": null,
      "lockoutEnabled": null,
      "accessFailedCount": null,
      "fullName": null,
      "role": null,
      "createdAt": null,
      "businessId": null,
      "business": null,
      "interactionsHandled": null,
      "ticketsAssigned": null,
      "auditLogs": null,
      "notifications": null,
      "messages": null
    },
    "customerId": "<string>",
    "customer": {
      "customerId": null,
      "fullName": null,
      "email": null,
      "phone": null,
      "createdAt": null,
      "businessId": null,
      "business": null,
      "orders": null,
      "tickets": null,
      "interactions": null,
      "feedbacks": null
    }
  },
  "interactionId": "<string>",
  "interaction": {
    "interactionId": "<string>",
    "channel": "<string>",
    "interactionType": "<string>",
    "status": "<string>",
    "isEnded": true,
    "startedAt": "2026-04-14T12:00:00Z",
    "endedAt": "2026-04-14T12:00:00Z",
    "callSessionId": "<string>",
    "businessId": "<string>",
    "business": {
      "id": null,
      "businessId": null,
      "name": null,
      "type": null,
      "address": null,
      "phone": null,
      "email": null,
      "website": null,
      "facebookUrl": null,
      "instagramUrl": null,
      "city": null,
      "country": null,
      "latitude": null,
      "longitude": null,
      "description": null,
      "cuisineType": null,
      "priceRange": null,
      "logoUrl": null,
      "coverImageUrl": null,
      "hasDelivery": null,
      "hasTakeout": null,
      "hasParking": null,
      "hasWiFi": null,
      "hasOutdoorSeating": null,
      "acceptsReservations": null,
      "paymentMethods": null,
      "isActive": null,
      "isVerified": null,
      "createdAt": null,
      "users": null,
      "customers": null,
      "menuItems": null,
      "orders": null,
      "tickets": null,
      "interactions": null,
      "notifications": null,
      "reports": null,
      "knowledgeBases": null,
      "menuCategories": null,
      "workingHours": null,
      "setting": null,
      "subscriptions": null,
      "integrations": null,
      "auditLogs": null
    },
    "handledByUserId": "<string>",
    "handledByUser": {
      "id": null,
      "userName": null,
      "normalizedUserName": null,
      "email": null,
      "normalizedEmail": null,
      "emailConfirmed": null,
      "passwordHash": null,
      "securityStamp": null,
      "concurrencyStamp": null,
      "phoneNumber": null,
      "phoneNumberConfirmed": null,
      "twoFactorEnabled": null,
      "lockoutEnd": null,
      "lockoutEnabled": null,
      "accessFailedCount": null,
      "fullName": null,
      "role": null,
      "createdAt": null,
      "businessId": null,
      "business": null,
      "interactionsHandled": null,
      "ticketsAssigned": null,
      "auditLogs": null,
      "notifications": null,
      "messages": null
    },
    "customerId": "<string>",
    "customer": {
      "customerId": null,
      "fullName": null,
      "email": null,
      "phone": null,
      "createdAt": null,
      "businessId": null,
      "business": null,
      "orders": null,
      "tickets": null,
      "interactions": null,
      "feedbacks": null
    },
    "relatedOrderId": "<string>",
    "relatedTicketId": "<string>",
    "messages": [
      null
    ]
  },
  "customerId": "<string>",
  "customer": {
    "customerId": "<string>",
    "fullName": "<string>",
    "email": "<string>",
    "phone": "<string>",
    "createdAt": "2026-04-14T12:00:00Z",
    "businessId": "<string>",
    "business": {
      "id": null,
      "businessId": null,
      "name": null,
      "type": null,
      "address": null,
      "phone": null,
      "email": null,
      "website": null,
      "facebookUrl": null,
      "instagramUrl": null,
      "city": null,
      "country": null,
      "latitude": null,
      "longitude": null,
      "description": null,
      "cuisineType": null,
      "priceRange": null,
      "logoUrl": null,
      "coverImageUrl": null,
      "hasDelivery": null,
      "hasTakeout": null,
      "hasParking": null,
      "hasWiFi": null,
      "hasOutdoorSeating": null,
      "acceptsReservations": null,
      "paymentMethods": null,
      "isActive": null,
      "isVerified": null,
      "createdAt": null,
      "users": null,
      "customers": null,
      "menuItems": null,
      "orders": null,
      "tickets": null,
      "interactions": null,
      "notifications": null,
      "reports": null,
      "knowledgeBases": null,
      "menuCategories": null,
      "workingHours": null,
      "setting": null,
      "subscriptions": null,
      "integrations": null,
      "auditLogs": null
    },
    "orders": [
      null
    ],
    "tickets": [
      null
    ],
    "interactions": [
      null
    ],
    "feedbacks": [
      null
    ]
  },
  "sentimentScore": 0.0
}
```

### Get Voice Settings

- **Method**: `GET`
- **URL**: `{{baseUrl}}/api/CustomerVoice/settings/{{businessId}}`
- **Success response (200)**:

```json
{
  "agentVoice": "<string>",
  "agentVoiceProvider": "<string>",
  "agentVoiceSpeed": 0.0,
  "agentVoicePitch": 0.0,
  "agentVoiceLanguage": "<string>"
}
```

