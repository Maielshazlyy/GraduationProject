## Frontend API Reference (Request + REAL response)

- **Source collection**: `DigitalEmployee_Postman_Collection.json`
- **Base URL used**: `http://localhost:9875`
- **Note**: Some endpoints may return errors if required setup data is missing. These are still real responses.

## 01 - Auth

### Register Admin [PUBLIC]

- **Method**: `POST`
- **URL**: `http://localhost:9875/api/Auth/register-admin`
- **Request body (from Postman)**:

```json
{
  "fullName": "Super Admin",
  "email": "admin+20260422122107@app.com",
  "password": "Admin@123"
}
```
- **Status**: `200`
- **Response (real)**:

```json
{
  "userId": "0620eb3e-2b1f-4c87-8ce4-372281ce7cd8",
  "email": "admin+20260422122107@app.com",
  "fullName": "Super Admin",
  "role": "Admin",
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6IjA2MjBlYjNlLTJiMWYtNGM4Ny04Y2U0LTM3MjI4MWNlN2NkOCIsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL2VtYWlsYWRkcmVzcyI6ImFkbWluKzIwMjYwNDIyMTIyMTA3QGFwcC5jb20iLCJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoiU3VwZXIgQWRtaW4iLCJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3JvbGUiOiJBZG1pbiIsImV4cCI6MTc3NzExNjA2OCwiaXNzIjoiaHR0cDovL2xvY2FsaG9zdDo1MTU3IiwiYXVkIjoiaHR0cDovL2xvY2FsaG9zdDo1MTU3In0.WljGrEhHBoTlSrgxgiWHT7Flz0_rWZcLOVXaAp4__LQ",
  "expiration": "2026-04-25T11:21:08Z",
  "businessId": null
}
```

### Register Owner [PUBLIC]

- **Method**: `POST`
- **URL**: `http://localhost:9875/api/Auth/register-owner`
- **Request body (from Postman)**:

```json
{
  "fullName": "Business Owner",
  "email": "owner+20260422122107@app.com",
  "password": "Owner@123"
}
```
- **Status**: `200`
- **Response (real)**:

```json
{
  "userId": "f233f37a-acd7-44ee-bab9-7436dae8eda2",
  "email": "owner+20260422122107@app.com",
  "fullName": "Business Owner",
  "role": "Owner",
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6ImYyMzNmMzdhLWFjZDctNDRlZS1iYWI5LTc0MzZkYWU4ZWRhMiIsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL2VtYWlsYWRkcmVzcyI6Im93bmVyKzIwMjYwNDIyMTIyMTA3QGFwcC5jb20iLCJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoiQnVzaW5lc3MgT3duZXIiLCJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3JvbGUiOiJPd25lciIsImV4cCI6MTc3NzExNjA2OCwiaXNzIjoiaHR0cDovL2xvY2FsaG9zdDo1MTU3IiwiYXVkIjoiaHR0cDovL2xvY2FsaG9zdDo1MTU3In0.uOJ64KybquDQT0Y8BG8j1-9exlMB_u3X7yCv0C1iWv0",
  "expiration": "2026-04-25T11:21:08Z",
  "businessId": null
}
```

### Register Agent [PUBLIC - requires businessId]

- **Method**: `POST`
- **URL**: `http://localhost:9875/api/Auth/register`
- **Request body (from Postman)**:

```json
{
  "fullName": "Ahmed Agent",
  "email": "agent+20260422122107@app.com",
  "password": "Agent@123",
  "businessId": ""
}
```
- **Status**: `400`
- **Response (real)**:

```json
{
  "message": "Validation Failed",
  "errors": [
    {
      "field": "BusinessId",
      "error": "BusinessId is required for agent registration."
    }
  ]
}
```

### Login

- **Method**: `POST`
- **URL**: `http://localhost:9875/api/Auth/login`
- **Request body (from Postman)**:

```json
{
  "email": "admin+20260422122107@app.com",
  "password": "Admin@123"
}
```
- **Status**: `200`
- **Response (real)**:

```json
{
  "userId": "0620eb3e-2b1f-4c87-8ce4-372281ce7cd8",
  "email": "admin+20260422122107@app.com",
  "fullName": "Super Admin",
  "role": "Admin",
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6IjA2MjBlYjNlLTJiMWYtNGM4Ny04Y2U0LTM3MjI4MWNlN2NkOCIsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL2VtYWlsYWRkcmVzcyI6ImFkbWluKzIwMjYwNDIyMTIyMTA3QGFwcC5jb20iLCJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoiU3VwZXIgQWRtaW4iLCJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3JvbGUiOiJBZG1pbiIsImV4cCI6MTc3NzExNjA2OSwiaXNzIjoiaHR0cDovL2xvY2FsaG9zdDo1MTU3IiwiYXVkIjoiaHR0cDovL2xvY2FsaG9zdDo1MTU3In0.eOvxEt08IgAG0yiF6JdWlNmwH6rIqNglWUxWQw63U0Q",
  "expiration": "2026-04-25T11:21:09Z",
  "businessId": null
}
```

### Google Login

- **Method**: `POST`
- **URL**: `http://localhost:9875/api/Auth/google-login`
- **Request body (from Postman)**:

```json
{
  "idToken": "<google_id_token_here>"
}
```
- **Status**: `400`
- **Response (real)**:

```json
{
  "message": "Invalid Google Token"
}
```

### Promote to Owner [Admin only]

- **Method**: `POST`
- **URL**: `http://localhost:9875/api/Auth/promote-to-owner`
- **Request body (from Postman)**:

```json
""
```
- **Status**: `404`
- **Response (real)**:

```json
{
  "message": "User not found."
}
```

### Promote to Admin [Admin only]

- **Method**: `POST`
- **URL**: `http://localhost:9875/api/Auth/promote-to-admin`
- **Request body (from Postman)**:

```json
""
```
- **Status**: `404`
- **Response (real)**:

```json
{
  "message": "User not found."
}
```


## 02 - Business

### Get All Businesses [Owner/Admin]

- **Method**: `GET`
- **URL**: `http://localhost:9875/api/Business`
- **Status**: `200`
- **Response (real)**:

```json
[
  {
    "id": "1",
    "businessId": "BUS-001",
    "name": "Test Company",
    "type": "Tech",
    "address": "Cairo, Egypt",
    "phone": "01000000000",
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
    "hasDelivery": false,
    "hasTakeout": false,
    "hasParking": false,
    "hasWiFi": false,
    "hasOutdoorSeating": false,
    "acceptsReservations": false,
    "paymentMethods": null,
    "isActive": false,
    "isVerified": false,
    "workingHours": [],
    "createdAt": "2025-12-14T18:09:35.48",
    "totalUsers": 0,
    "totalCustomers": 0,
    "totalTickets": 0
  },
  {
    "id": "4fbf0918-af0c-4d4c-bfcf-df7e0ba29071",
    "businessId": "8f3c307e-31e0-431f-9b50-1a6c29161922",
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
    "isActive": true,
    "isVerified": false,
    "workingHours": [],
    "createdAt": "2026-04-14T14:39:25.3117918",
    "totalUsers": 0,
    "totalCustomers": 0,
    "totalTickets": 0
  },
  {
    "id": "6b43667f-a08b-4be5-b2a6-3a782cd07769",
    "businessId": "74893538-1ca0-48c7-8cb7-f08e1d412464",
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
    "isActive": true,
    "isVerified": false,
    "workingHours": [],
    "createdAt": "2026-04-22T12:11:26.7433403",
    "totalUsers": 0,
    "totalCustomers": 0,
    "totalTickets": 0
  },
  {
    "id": "701f9138-65af-4e2d-8a59-174e0948ce82",
    "businessId": "1e0440aa-53b4-438f-bfce-05f010433cc4",
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
    "isActive": true,
    "isVerified": false,
    "workingHours": [],
    "createdAt": "2026-04-22T12:07:25.3345203",
    "totalUsers": 0,
    "totalCustomers": 0,
    "totalTickets": 0
  },
  {
    "id": "c14c1302-f710-4739-bd21-e4fb9c5d1ec0",
    "businessId": "3117ad0a-6dd2-496d-83ae-54ef74cc09d4",
    "name": "b1",
    "type": "restaurant",
    "address": "address1",
    "phone": "0123456789",
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
    "hasDelivery": false,
    "hasTakeout": false,
    "hasParking": false,
    "hasWiFi": false,
    "hasOutdoorSeating": false,
    "acceptsReservations": false,
    "paymentMethods": null,
    "isActive": false,
    "isVerified": false,
    "workingHours": [],
    "createdAt": "2026-02-03T16:47:50.9258178",
    "totalUsers": 0,
    "totalCustomers": 0,
    "totalTickets": 0
  },
  {
    "id": "e904934f-2b6b-4d71-ae27-2983d2e8e6f4",
    "businessId": "d58f05ab-fcab-43f9-bbee-4a2971ff76a1",
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
    "isActive": true,
    "isVerified": false,
    "workingHours": [],
    "createdAt": "2026-04-22T12:07:25.8773703",
    "totalUsers": 0,
    "totalCustomers": 0,
    "totalTickets": 0
  }
]
```

### Get Business by ID [Owner/Admin]

- **Method**: `GET`
- **URL**: `http://localhost:9875/api/Business/`
- **Status**: `200`
- **Response (real)**:

```json
[
  {
    "id": "1",
    "businessId": "BUS-001",
    "name": "Test Company",
    "type": "Tech",
    "address": "Cairo, Egypt",
    "phone": "01000000000",
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
    "hasDelivery": false,
    "hasTakeout": false,
    "hasParking": false,
    "hasWiFi": false,
    "hasOutdoorSeating": false,
    "acceptsReservations": false,
    "paymentMethods": null,
    "isActive": false,
    "isVerified": false,
    "workingHours": [],
    "createdAt": "2025-12-14T18:09:35.48",
    "totalUsers": 0,
    "totalCustomers": 0,
    "totalTickets": 0
  },
  {
    "id": "4fbf0918-af0c-4d4c-bfcf-df7e0ba29071",
    "businessId": "8f3c307e-31e0-431f-9b50-1a6c29161922",
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
    "isActive": true,
    "isVerified": false,
    "workingHours": [],
    "createdAt": "2026-04-14T14:39:25.3117918",
    "totalUsers": 0,
    "totalCustomers": 0,
    "totalTickets": 0
  },
  {
    "id": "6b43667f-a08b-4be5-b2a6-3a782cd07769",
    "businessId": "74893538-1ca0-48c7-8cb7-f08e1d412464",
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
    "isActive": true,
    "isVerified": false,
    "workingHours": [],
    "createdAt": "2026-04-22T12:11:26.7433403",
    "totalUsers": 0,
    "totalCustomers": 0,
    "totalTickets": 0
  },
  {
    "id": "701f9138-65af-4e2d-8a59-174e0948ce82",
    "businessId": "1e0440aa-53b4-438f-bfce-05f010433cc4",
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
    "isActive": true,
    "isVerified": false,
    "workingHours": [],
    "createdAt": "2026-04-22T12:07:25.3345203",
    "totalUsers": 0,
    "totalCustomers": 0,
    "totalTickets": 0
  },
  {
    "id": "c14c1302-f710-4739-bd21-e4fb9c5d1ec0",
    "businessId": "3117ad0a-6dd2-496d-83ae-54ef74cc09d4",
    "name": "b1",
    "type": "restaurant",
    "address": "address1",
    "phone": "0123456789",
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
    "hasDelivery": false,
    "hasTakeout": false,
    "hasParking": false,
    "hasWiFi": false,
    "hasOutdoorSeating": false,
    "acceptsReservations": false,
    "paymentMethods": null,
    "isActive": false,
    "isVerified": false,
    "workingHours": [],
    "createdAt": "2026-02-03T16:47:50.9258178",
    "totalUsers": 0,
    "totalCustomers": 0,
    "totalTickets": 0
  },
  {
    "id": "e904934f-2b6b-4d71-ae27-2983d2e8e6f4",
    "businessId": "d58f05ab-fcab-43f9-bbee-4a2971ff76a1",
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
    "isActive": true,
    "isVerified": false,
    "workingHours": [],
    "createdAt": "2026-04-22T12:07:25.8773703",
    "totalUsers": 0,
    "totalCustomers": 0,
    "totalTickets": 0
  }
]
```

### Create Business [Owner/Admin]

- **Method**: `POST`
- **URL**: `http://localhost:9875/api/Business`
- **Request body (from Postman)**:

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
- **Status**: `201`
- **Response (real)**:

```json
{
  "id": "48866c67-0fac-47c1-8acf-10f708cca0b0",
  "businessId": "15818b89-846a-42cf-aff8-bff01bb1d4a8",
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
  "isActive": true,
  "isVerified": false,
  "workingHours": [
    {
      "workingHoursId": "2774fc71-c77c-4415-b067-bbeb4a771c0a",
      "dayOfWeek": 0,
      "dayName": "Sunday",
      "openTime": null,
      "closeTime": null,
      "isClosed": true
    },
    {
      "workingHoursId": "bffd15e9-af50-4c08-9c22-e605f5e0fca5",
      "dayOfWeek": 1,
      "dayName": "Monday",
      "openTime": "09:00",
      "closeTime": "22:00",
      "isClosed": false
    },
    {
      "workingHoursId": "9c7f05b9-2a88-4d9c-94c9-2d3ed382f20f",
      "dayOfWeek": 2,
      "dayName": "Tuesday",
      "openTime": "09:00",
      "closeTime": "22:00",
      "isClosed": false
    },
    {
      "workingHoursId": "bea32b5c-6845-42da-bfa3-e065a88557a2",
      "dayOfWeek": 3,
      "dayName": "Wednesday",
      "openTime": "09:00",
      "closeTime": "22:00",
      "isClosed": false
    },
    {
      "workingHoursId": "009e83eb-e609-4ce6-8dcf-de1ce47274ed",
      "dayOfWeek": 4,
      "dayName": "Thursday",
      "openTime": "09:00",
      "closeTime": "23:00",
      "isClosed": false
    },
    {
      "workingHoursId": "387f3430-d874-40d5-8908-d287889bc5b5",
      "dayOfWeek": 5,
      "dayName": "Friday",
      "openTime": "10:00",
      "closeTime": "23:00",
      "isClosed": false
    },
    {
      "workingHoursId": "7659b53f-8de7-4951-b49b-3a46246a189f",
      "dayOfWeek": 6,
      "dayName": "Saturday",
      "openTime": "10:00",
      "closeTime": "21:00",
      "isClosed": false
    }
  ],
  "createdAt": "2026-04-22T12:21:09.7770676Z",
  "totalUsers": 1,
  "totalCustomers": 0,
  "totalTickets": 0
}
```

### Onboard Business [Public]

- **Method**: `POST`
- **URL**: `http://localhost:9875/api/Business/onboard`
- **Request body (from Postman)**:

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
- **Status**: `201`
- **Response (real)**:

```json
{
  "id": "d8a4ccc9-a1ee-4302-964e-537c17119a77",
  "businessId": "ac3fd098-3bd3-4f7f-9084-21e50c605974",
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
  "isActive": true,
  "isVerified": false,
  "workingHours": [
    {
      "workingHoursId": "c9879077-2112-4b4e-a700-c41a9a838380",
      "dayOfWeek": 0,
      "dayName": "Sunday",
      "openTime": null,
      "closeTime": null,
      "isClosed": true
    },
    {
      "workingHoursId": "922461c2-8b62-41d7-af4d-fddd9f69961d",
      "dayOfWeek": 1,
      "dayName": "Monday",
      "openTime": "09:00",
      "closeTime": "22:00",
      "isClosed": false
    },
    {
      "workingHoursId": "643eb63e-c0bb-40d8-9e7e-36a6a9ce4a88",
      "dayOfWeek": 2,
      "dayName": "Tuesday",
      "openTime": "09:00",
      "closeTime": "22:00",
      "isClosed": false
    },
    {
      "workingHoursId": "0f213e50-4bb8-4ae4-bf47-fcc21804ea56",
      "dayOfWeek": 3,
      "dayName": "Wednesday",
      "openTime": "09:00",
      "closeTime": "22:00",
      "isClosed": false
    },
    {
      "workingHoursId": "728dcb71-b1b2-4f8d-94d5-808a05f6b32c",
      "dayOfWeek": 4,
      "dayName": "Thursday",
      "openTime": "09:00",
      "closeTime": "23:00",
      "isClosed": false
    },
    {
      "workingHoursId": "a0b881f6-23a9-445b-9c62-64ff3cf95a37",
      "dayOfWeek": 5,
      "dayName": "Friday",
      "openTime": "10:00",
      "closeTime": "23:00",
      "isClosed": false
    },
    {
      "workingHoursId": "821ffe92-1437-4434-9fb8-476486ffa2f7",
      "dayOfWeek": 6,
      "dayName": "Saturday",
      "openTime": "10:00",
      "closeTime": "21:00",
      "isClosed": false
    }
  ],
  "createdAt": "2026-04-22T12:21:10.0265942Z",
  "totalUsers": 0,
  "totalCustomers": 0,
  "totalTickets": 0
}
```

### Update Business [Owner/Admin]

- **Method**: `PUT`
- **URL**: `http://localhost:9875/api/Business/ac3fd098-3bd3-4f7f-9084-21e50c605974`
- **Request body (from Postman)**:

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
- **Status**: `404`
- **Response (real)**:

```json
{
  "message": "Business with id 'ac3fd098-3bd3-4f7f-9084-21e50c605974' not found."
}
```

### Delete Business [Admin]

- **Method**: `DELETE`
- **URL**: `http://localhost:9875/api/Business/ac3fd098-3bd3-4f7f-9084-21e50c605974`
- **Status**: `404`
- **Response (real)**:

```json
{
  "message": "Business with id 'ac3fd098-3bd3-4f7f-9084-21e50c605974' not found."
}
```


## 03 - User

### Get All Users [Admin]

- **Method**: `GET`
- **URL**: `http://localhost:9875/api/User`
- **Status**: `200`
- **Response (real)**:

```json
[
  {
    "userId": "04e98269-e17d-4d8f-8fff-d7189ed30226",
    "fullName": "ramez",
    "email": "ramez@gmail.com",
    "role": "Admin",
    "businessId": null,
    "businessName": "",
    "totalHandledInteractions": 0,
    "totalAssignedTickets": 0,
    "createdAt": "2026-02-03T16:30:08.5519323"
  },
  {
    "userId": "0620eb3e-2b1f-4c87-8ce4-372281ce7cd8",
    "fullName": "Super Admin",
    "email": "admin+20260422122107@app.com",
    "role": "Owner",
    "businessId": "48866c67-0fac-47c1-8acf-10f708cca0b0",
    "businessName": "",
    "totalHandledInteractions": 0,
    "totalAssignedTickets": 0,
    "createdAt": "2026-04-22T12:21:08.5817411"
  },
  {
    "userId": "0803d5af-aeb5-4eaa-8e9e-c2622eae4a51",
    "fullName": "mai",
    "email": "mai@gmail.com",
    "role": "Admin",
    "businessId": "1",
    "businessName": "",
    "totalHandledInteractions": 0,
    "totalAssignedTickets": 0,
    "createdAt": "2026-02-02T20:07:07.0004029"
  },
  {
    "userId": "18b51258-30da-4251-a609-f42966a9e9ae",
    "fullName": "Super Admin",
    "email": "admin@app.com",
    "role": "Owner",
    "businessId": "701f9138-65af-4e2d-8a59-174e0948ce82",
    "businessName": "",
    "totalHandledInteractions": 0,
    "totalAssignedTickets": 0,
    "createdAt": "2026-04-14T15:24:36.486633"
  },
  {
    "userId": "1bd04371-511e-422b-b8af-cb4d05c67314",
    "fullName": "Business Owner",
    "email": "owner@app.com",
    "role": "Owner",
    "businessId": "4fbf0918-af0c-4d4c-bfcf-df7e0ba29071",
    "businessName": "",
    "totalHandledInteractions": 0,
    "totalAssignedTickets": 0,
    "createdAt": "2026-04-14T14:36:57.0487739"
  },
  {
    "userId": "64f72a0e-ac0d-4ad0-b247-4912f6e529ea",
    "fullName": "Mai Elshazly",
    "email": "maielshazly75@gmail.com",
    "role": "Owner",
    "businessId": "1",
    "businessName": "",
    "totalHandledInteractions": 0,
    "totalAssignedTickets": 0,
    "createdAt": "2025-12-15T14:10:20.1353727"
  },
  {
    "userId": "ad1916c3-33b0-4b3a-a6d3-95a0c577f5d6",
    "fullName": "New Agent",
    "email": "newagent@app.com",
    "role": "Agent",
    "businessId": "701f9138-65af-4e2d-8a59-174e0948ce82",
    "businessName": "",
    "totalHandledInteractions": 0,
    "totalAssignedTickets": 0,
    "createdAt": "2026-04-22T12:11:27.5312793"
  },
  {
    "userId": "e4d02db5-1bb9-4608-83bc-c4bb2a17a9f5",
    "fullName": "Mai Elshazly",
    "email": "mai@test.com",
    "role": "Owner",
    "businessId": "1",
    "businessName": "",
    "totalHandledInteractions": 0,
    "totalAssignedTickets": 0,
    "createdAt": "2025-12-14T16:14:42.8719677"
  },
  {
    "userId": "f233f37a-acd7-44ee-bab9-7436dae8eda2",
    "fullName": "Business Owner",
    "email": "owner+20260422122107@app.com",
    "role": "Owner",
    "businessId": null,
    "businessName": "",
    "totalHandledInteractions": 0,
    "totalAssignedTickets": 0,
    "createdAt": "2026-04-22T12:21:08.909271"
  }
]
```

### Get Users by Business [Owner/Admin]

- **Method**: `GET`
- **URL**: `http://localhost:9875/api/User/business/ac3fd098-3bd3-4f7f-9084-21e50c605974`
- **Status**: `200`
- **Response (real)**:

```json
[]
```

### Get User by ID

- **Method**: `GET`
- **URL**: `http://localhost:9875/api/User/`
- **Status**: `200`
- **Response (real)**:

```json
[
  {
    "userId": "04e98269-e17d-4d8f-8fff-d7189ed30226",
    "fullName": "ramez",
    "email": "ramez@gmail.com",
    "role": "Admin",
    "businessId": null,
    "businessName": "",
    "totalHandledInteractions": 0,
    "totalAssignedTickets": 0,
    "createdAt": "2026-02-03T16:30:08.5519323"
  },
  {
    "userId": "0620eb3e-2b1f-4c87-8ce4-372281ce7cd8",
    "fullName": "Super Admin",
    "email": "admin+20260422122107@app.com",
    "role": "Owner",
    "businessId": "48866c67-0fac-47c1-8acf-10f708cca0b0",
    "businessName": "",
    "totalHandledInteractions": 0,
    "totalAssignedTickets": 0,
    "createdAt": "2026-04-22T12:21:08.5817411"
  },
  {
    "userId": "0803d5af-aeb5-4eaa-8e9e-c2622eae4a51",
    "fullName": "mai",
    "email": "mai@gmail.com",
    "role": "Admin",
    "businessId": "1",
    "businessName": "",
    "totalHandledInteractions": 0,
    "totalAssignedTickets": 0,
    "createdAt": "2026-02-02T20:07:07.0004029"
  },
  {
    "userId": "18b51258-30da-4251-a609-f42966a9e9ae",
    "fullName": "Super Admin",
    "email": "admin@app.com",
    "role": "Owner",
    "businessId": "701f9138-65af-4e2d-8a59-174e0948ce82",
    "businessName": "",
    "totalHandledInteractions": 0,
    "totalAssignedTickets": 0,
    "createdAt": "2026-04-14T15:24:36.486633"
  },
  {
    "userId": "1bd04371-511e-422b-b8af-cb4d05c67314",
    "fullName": "Business Owner",
    "email": "owner@app.com",
    "role": "Owner",
    "businessId": "4fbf0918-af0c-4d4c-bfcf-df7e0ba29071",
    "businessName": "",
    "totalHandledInteractions": 0,
    "totalAssignedTickets": 0,
    "createdAt": "2026-04-14T14:36:57.0487739"
  },
  {
    "userId": "64f72a0e-ac0d-4ad0-b247-4912f6e529ea",
    "fullName": "Mai Elshazly",
    "email": "maielshazly75@gmail.com",
    "role": "Owner",
    "businessId": "1",
    "businessName": "",
    "totalHandledInteractions": 0,
    "totalAssignedTickets": 0,
    "createdAt": "2025-12-15T14:10:20.1353727"
  },
  {
    "userId": "ad1916c3-33b0-4b3a-a6d3-95a0c577f5d6",
    "fullName": "New Agent",
    "email": "newagent@app.com",
    "role": "Agent",
    "businessId": "701f9138-65af-4e2d-8a59-174e0948ce82",
    "businessName": "",
    "totalHandledInteractions": 0,
    "totalAssignedTickets": 0,
    "createdAt": "2026-04-22T12:11:27.5312793"
  },
  {
    "userId": "e4d02db5-1bb9-4608-83bc-c4bb2a17a9f5",
    "fullName": "Mai Elshazly",
    "email": "mai@test.com",
    "role": "Owner",
    "businessId": "1",
    "businessName": "",
    "totalHandledInteractions": 0,
    "totalAssignedTickets": 0,
    "createdAt": "2025-12-14T16:14:42.8719677"
  },
  {
    "userId": "f233f37a-acd7-44ee-bab9-7436dae8eda2",
    "fullName": "Business Owner",
    "email": "owner+20260422122107@app.com",
    "role": "Owner",
    "businessId": null,
    "businessName": "",
    "totalHandledInteractions": 0,
    "totalAssignedTickets": 0,
    "createdAt": "2026-04-22T12:21:08.909271"
  }
]
```

### Get User by Email [Admin]

- **Method**: `GET`
- **URL**: `http://localhost:9875/api/User/email/agent@app.com`
- **Status**: `404`
- **Response (real)**:

```json
{
  "message": "User with email 'agent@app.com' not found."
}
```

### Update User [Admin]

- **Method**: `PUT`
- **URL**: `http://localhost:9875/api/User/`
- **Request body (from Postman)**:

```json
{
  "fullName": "Updated Name",
  "phone": "+201012345678"
}
```
- **Status**: `405`
- **Response (real)**:

```json
{
  "raw": ""
}
```

### Assign Role [Admin]

- **Method**: `POST`
- **URL**: `http://localhost:9875/api/User//assign-role`
- **Request body (from Postman)**:

```json
{
  "userId": "",
  "newRole": "Agent"
}
```
- **Status**: `404`
- **Response (real)**:

```json
{
  "raw": ""
}
```

### Create Human Employee / Agent [Owner/Admin]

- **Method**: `POST`
- **URL**: `http://localhost:9875/api/User/agents`
- **Request body (from Postman)**:

```json
{
  "fullName": "New Agent",
  "email": "newagent@app.com",
  "password": "Agent@123",
  "phone": "+201098765432"
}
```
- **Status**: `400`
- **Response (real)**:

```json
{
  "message": "BusinessId not found in token. Make sure the owner is linked to a business."
}
```

### Delete User [Admin]

- **Method**: `DELETE`
- **URL**: `http://localhost:9875/api/User/`
- **Status**: `405`
- **Response (real)**:

```json
{
  "raw": ""
}
```


## 04 - Customer

### Get All Customers

- **Method**: `GET`
- **URL**: `http://localhost:9875/api/Customer`
- **Status**: `200`
- **Response (real)**:

```json
[
  {
    "customerId": "37b0b04e-7203-46f1-8b00-db4ba9dbb7a9",
    "fullName": "ahmed",
    "email": "ahmed1@gmail.com",
    "phone": "0123456789",
    "createdAt": "2026-02-03T16:49:37.7404367",
    "totalOrders": 0,
    "totalTickets": 0,
    "businessId": "1",
    "businessName": ""
  },
  {
    "customerId": "75b0d7a9-c4fa-474b-ac7d-f8b817f209d8",
    "fullName": "Ahmed Ali Updated",
    "email": "ahmed.updated@gmail.com",
    "phone": "+201098765432",
    "createdAt": "2026-04-22T12:11:28.0722923",
    "totalOrders": 0,
    "totalTickets": 0,
    "businessId": "701f9138-65af-4e2d-8a59-174e0948ce82",
    "businessName": ""
  },
  {
    "customerId": "d2848455-8e76-4840-9d68-bdb4d7a4b5fe",
    "fullName": "mohand",
    "email": "mohand@gmail.com",
    "phone": "0102345678",
    "createdAt": "2026-02-07T13:25:34.3869405",
    "totalOrders": 0,
    "totalTickets": 0,
    "businessId": "1",
    "businessName": ""
  }
]
```

### Get Customers by Business

- **Method**: `GET`
- **URL**: `http://localhost:9875/api/Customer/business/ac3fd098-3bd3-4f7f-9084-21e50c605974`
- **Status**: `200`
- **Response (real)**:

```json
[]
```

### Get Customer by ID

- **Method**: `GET`
- **URL**: `http://localhost:9875/api/Customer/`
- **Status**: `200`
- **Response (real)**:

```json
[
  {
    "customerId": "37b0b04e-7203-46f1-8b00-db4ba9dbb7a9",
    "fullName": "ahmed",
    "email": "ahmed1@gmail.com",
    "phone": "0123456789",
    "createdAt": "2026-02-03T16:49:37.7404367",
    "totalOrders": 0,
    "totalTickets": 0,
    "businessId": "1",
    "businessName": ""
  },
  {
    "customerId": "75b0d7a9-c4fa-474b-ac7d-f8b817f209d8",
    "fullName": "Ahmed Ali Updated",
    "email": "ahmed.updated@gmail.com",
    "phone": "+201098765432",
    "createdAt": "2026-04-22T12:11:28.0722923",
    "totalOrders": 0,
    "totalTickets": 0,
    "businessId": "701f9138-65af-4e2d-8a59-174e0948ce82",
    "businessName": ""
  },
  {
    "customerId": "d2848455-8e76-4840-9d68-bdb4d7a4b5fe",
    "fullName": "mohand",
    "email": "mohand@gmail.com",
    "phone": "0102345678",
    "createdAt": "2026-02-07T13:25:34.3869405",
    "totalOrders": 0,
    "totalTickets": 0,
    "businessId": "1",
    "businessName": ""
  }
]
```

### Get Customer by Email

- **Method**: `GET`
- **URL**: `http://localhost:9875/api/Customer/email/customer@example.com`
- **Status**: `404`
- **Response (real)**:

```json
{
  "message": "Customer with email 'customer@example.com' not found."
}
```

### Create Customer [Owner/Admin]

- **Method**: `POST`
- **URL**: `http://localhost:9875/api/Customer`
- **Request body (from Postman)**:

```json
{
  "fullName": "Ahmed Ali",
  "email": "ahmed@gmail.com",
  "phone": "+201098765432",
  "businessId": "ac3fd098-3bd3-4f7f-9084-21e50c605974"
}
```
- **Status**: `400`
- **Response (real)**:

```json
{
  "message": "Business with id 'ac3fd098-3bd3-4f7f-9084-21e50c605974' not found."
}
```

### Update Customer [Owner/Admin]

- **Method**: `PUT`
- **URL**: `http://localhost:9875/api/Customer/`
- **Request body (from Postman)**:

```json
{
  "fullName": "Ahmed Ali Updated",
  "email": "ahmed.updated@gmail.com",
  "phone": "+201098765432"
}
```
- **Status**: `405`
- **Response (real)**:

```json
{
  "raw": ""
}
```

### Delete Customer [Admin]

- **Method**: `DELETE`
- **URL**: `http://localhost:9875/api/Customer/`
- **Status**: `405`
- **Response (real)**:

```json
{
  "raw": ""
}
```


## 05 - Dashboard [Owner] (Top products included in Analytics)

### Get Dashboard Summary

- **Method**: `GET`
- **URL**: `http://localhost:9875/api/Dashboard/summary`
- **Status**: `400`
- **Response (real)**:

```json
{
  "message": "BusinessId not found in token. Please ensure you are linked to a business."
}
```

### Get Analytics

- **Method**: `GET`
- **URL**: `http://localhost:9875/api/Dashboard/analytics`
- **Status**: `400`
- **Response (real)**:

```json
{
  "message": "BusinessId not found in token."
}
```

### Get Full Dashboard

- **Method**: `GET`
- **URL**: `http://localhost:9875/api/Dashboard/full`
- **Status**: `400`
- **Response (real)**:

```json
{
  "message": "BusinessId not found in token."
}
```

### Get Recent Audit Logs

- **Method**: `GET`
- **URL**: `http://localhost:9875/api/Dashboard/audit-logs/recent?count=20`
- **Status**: `400`
- **Response (real)**:

```json
{
  "message": "BusinessId not found in token."
}
```

### Get Audit Log Statistics

- **Method**: `GET`
- **URL**: `http://localhost:9875/api/Dashboard/audit-logs/statistics`
- **Status**: `400`
- **Response (real)**:

```json
{
  "message": "BusinessId not found in token."
}
```

### Get Customer Audit Logs

- **Method**: `GET`
- **URL**: `http://localhost:9875/api/Dashboard/audit-logs/customer/`
- **Status**: `404`
- **Response (real)**:

```json
{
  "raw": ""
}
```


## 06 - Admin Dashboard

### Get Summary

- **Method**: `GET`
- **URL**: `http://localhost:9875/api/AdminDashboard/summary`
- **Status**: `200`
- **Response (real)**:

```json
{
  "totalBusinesses": 8,
  "activeBusinesses": 6,
  "newBusinessesLast30Days": 6,
  "totalOrders": 1,
  "totalRevenue": 0,
  "pendingOrders": 1,
  "completedOrders": 0,
  "cancelledOrders": 0,
  "totalTickets": 3,
  "openTickets": 3,
  "escalatedTickets": 0,
  "totalInteractions": 2,
  "activeInteractions": 1,
  "totalFeedbacks": 2,
  "averageRating": 4,
  "positiveSentiments": 0,
  "negativeSentiments": 0,
  "neutralSentiments": 0,
  "averageSentimentScore": 0,
  "totalAuditLogs": 8,
  "auditLogsLast24Hours": 7,
  "lastAuditLogDate": "2026-04-22T12:21:09.8946799",
  "lastOrderDate": "2026-02-07T14:05:06.2980903",
  "lastTicketDate": "2026-04-22T12:11:31.4439549",
  "lastFeedbackDate": "2026-04-22T12:11:37.2220859"
}
```

### Get Top Businesses

- **Method**: `GET`
- **URL**: `http://localhost:9875/api/AdminDashboard/top-businesses?count=10`
- **Status**: `200`
- **Response (real)**:

```json
[
  {
    "businessId": "1",
    "businessName": "Test Company",
    "isActive": false,
    "ordersCount": 1,
    "revenue": 0,
    "openTicketsCount": 2,
    "customersCount": 2
  },
  {
    "businessId": "48866c67-0fac-47c1-8acf-10f708cca0b0",
    "businessName": "The Italian Place",
    "isActive": true,
    "ordersCount": 0,
    "revenue": 0,
    "openTicketsCount": 0,
    "customersCount": 0
  },
  {
    "businessId": "4fbf0918-af0c-4d4c-bfcf-df7e0ba29071",
    "businessName": "The Italian Place",
    "isActive": true,
    "ordersCount": 0,
    "revenue": 0,
    "openTicketsCount": 0,
    "customersCount": 0
  },
  {
    "businessId": "6b43667f-a08b-4be5-b2a6-3a782cd07769",
    "businessName": "The Italian Place",
    "isActive": true,
    "ordersCount": 0,
    "revenue": 0,
    "openTicketsCount": 0,
    "customersCount": 0
  },
  {
    "businessId": "701f9138-65af-4e2d-8a59-174e0948ce82",
    "businessName": "The Italian Place",
    "isActive": true,
    "ordersCount": 0,
    "revenue": 0,
    "openTicketsCount": 1,
    "customersCount": 1
  },
  {
    "businessId": "c14c1302-f710-4739-bd21-e4fb9c5d1ec0",
    "businessName": "b1",
    "isActive": false,
    "ordersCount": 0,
    "revenue": 0,
    "openTicketsCount": 0,
    "customersCount": 0
  },
  {
    "businessId": "d8a4ccc9-a1ee-4302-964e-537c17119a77",
    "businessName": "The Italian Place",
    "isActive": true,
    "ordersCount": 0,
    "revenue": 0,
    "openTicketsCount": 0,
    "customersCount": 0
  },
  {
    "businessId": "e904934f-2b6b-4d71-ae27-2983d2e8e6f4",
    "businessName": "The Italian Place",
    "isActive": true,
    "ordersCount": 0,
    "revenue": 0,
    "openTicketsCount": 0,
    "customersCount": 0
  }
]
```

### Get Full Dashboard

- **Method**: `GET`
- **URL**: `http://localhost:9875/api/AdminDashboard/full`
- **Status**: `200`
- **Response (real)**:

```json
{
  "summary": {
    "totalBusinesses": 8,
    "activeBusinesses": 6,
    "newBusinessesLast30Days": 6,
    "totalOrders": 1,
    "totalRevenue": 0,
    "pendingOrders": 1,
    "completedOrders": 0,
    "cancelledOrders": 0,
    "totalTickets": 3,
    "openTickets": 3,
    "escalatedTickets": 0,
    "totalInteractions": 2,
    "activeInteractions": 1,
    "totalFeedbacks": 2,
    "averageRating": 4,
    "positiveSentiments": 0,
    "negativeSentiments": 0,
    "neutralSentiments": 0,
    "averageSentimentScore": 0,
    "totalAuditLogs": 8,
    "auditLogsLast24Hours": 7,
    "lastAuditLogDate": "2026-04-22T12:21:09.8946799",
    "lastOrderDate": "2026-02-07T14:05:06.2980903",
    "lastTicketDate": "2026-04-22T12:11:31.4439549",
    "lastFeedbackDate": "2026-04-22T12:11:37.2220859"
  },
  "topBusinessesByRevenue": [
    {
      "businessId": "1",
      "businessName": "Test Company",
      "isActive": false,
      "ordersCount": 1,
      "revenue": 0,
      "openTicketsCount": 2,
      "customersCount": 2
    },
    {
      "businessId": "48866c67-0fac-47c1-8acf-10f708cca0b0",
      "businessName": "The Italian Place",
      "isActive": true,
      "ordersCount": 0,
      "revenue": 0,
      "openTicketsCount": 0,
      "customersCount": 0
    },
    {
      "businessId": "4fbf0918-af0c-4d4c-bfcf-df7e0ba29071",
      "businessName": "The Italian Place",
      "isActive": true,
      "ordersCount": 0,
      "revenue": 0,
      "openTicketsCount": 0,
      "customersCount": 0
    },
    {
      "businessId": "6b43667f-a08b-4be5-b2a6-3a782cd07769",
      "businessName": "The Italian Place",
      "isActive": true,
      "ordersCount": 0,
      "revenue": 0,
      "openTicketsCount": 0,
      "customersCount": 0
    },
    {
      "businessId": "701f9138-65af-4e2d-8a59-174e0948ce82",
      "businessName": "The Italian Place",
      "isActive": true,
      "ordersCount": 0,
      "revenue": 0,
      "openTicketsCount": 1,
      "customersCount": 1
    },
    {
      "businessId": "c14c1302-f710-4739-bd21-e4fb9c5d1ec0",
      "businessName": "b1",
      "isActive": false,
      "ordersCount": 0,
      "revenue": 0,
      "openTicketsCount": 0,
      "customersCount": 0
    },
    {
      "businessId": "d8a4ccc9-a1ee-4302-964e-537c17119a77",
      "businessName": "The Italian Place",
      "isActive": true,
      "ordersCount": 0,
      "revenue": 0,
      "openTicketsCount": 0,
      "customersCount": 0
    },
    {
      "businessId": "e904934f-2b6b-4d71-ae27-2983d2e8e6f4",
      "businessName": "The Italian Place",
      "isActive": true,
      "ordersCount": 0,
      "revenue": 0,
      "openTicketsCount": 0,
      "customersCount": 0
    }
  ]
}
```

### Get Alerts

- **Method**: `GET`
- **URL**: `http://localhost:9875/api/AdminDashboard/alerts`
- **Status**: `200`
- **Response (real)**:

```json
[
  {
    "id": "8136a710-2f84-4e8c-a462-8fdff2ffe37a",
    "type": "InactiveBusiness",
    "severity": "Medium",
    "businessId": "1",
    "businessName": "Test Company",
    "message": "No orders recorded in the last 14 days.",
    "createdAt": "2026-04-22T12:21:12.112807Z"
  },
  {
    "id": "0e7b6022-8303-4d1f-8589-a2d9ac43471e",
    "type": "InactiveBusiness",
    "severity": "Medium",
    "businessId": "48866c67-0fac-47c1-8acf-10f708cca0b0",
    "businessName": "The Italian Place",
    "message": "No orders recorded in the last 14 days.",
    "createdAt": "2026-04-22T12:21:12.112807Z"
  },
  {
    "id": "084897ce-a978-4ed7-a0de-e1f1b0d03730",
    "type": "InactiveBusiness",
    "severity": "Medium",
    "businessId": "4fbf0918-af0c-4d4c-bfcf-df7e0ba29071",
    "businessName": "The Italian Place",
    "message": "No orders recorded in the last 14 days.",
    "createdAt": "2026-04-22T12:21:12.112807Z"
  },
  {
    "id": "6da20ace-c0d2-42de-be5f-b610798ac1f2",
    "type": "InactiveBusiness",
    "severity": "Medium",
    "businessId": "6b43667f-a08b-4be5-b2a6-3a782cd07769",
    "businessName": "The Italian Place",
    "message": "No orders recorded in the last 14 days.",
    "createdAt": "2026-04-22T12:21:12.112807Z"
  },
  {
    "id": "077f3fae-176e-4183-aec7-cd38689af314",
    "type": "InactiveBusiness",
    "severity": "Medium",
    "businessId": "701f9138-65af-4e2d-8a59-174e0948ce82",
    "businessName": "The Italian Place",
    "message": "No orders recorded in the last 14 days.",
    "createdAt": "2026-04-22T12:21:12.112807Z"
  },
  {
    "id": "36fd8640-f8d5-43ef-8076-359ee14c7de5",
    "type": "InactiveBusiness",
    "severity": "Medium",
    "businessId": "c14c1302-f710-4739-bd21-e4fb9c5d1ec0",
    "businessName": "b1",
    "message": "No orders recorded in the last 14 days.",
    "createdAt": "2026-04-22T12:21:12.112807Z"
  },
  {
    "id": "1a8ee629-f54d-4476-956a-3251adfba309",
    "type": "InactiveBusiness",
    "severity": "Medium",
    "businessId": "d8a4ccc9-a1ee-4302-964e-537c17119a77",
    "businessName": "The Italian Place",
    "message": "No orders recorded in the last 14 days.",
    "createdAt": "2026-04-22T12:21:12.112807Z"
  },
  {
    "id": "bdd0ba47-4df9-416d-84ff-8f707fb2e961",
    "type": "InactiveBusiness",
    "severity": "Medium",
    "businessId": "e904934f-2b6b-4d71-ae27-2983d2e8e6f4",
    "businessName": "The Italian Place",
    "message": "No orders recorded in the last 14 days.",
    "createdAt": "2026-04-22T12:21:12.112807Z"
  }
]
```

### Get Revenue Trend

- **Method**: `GET`
- **URL**: `http://localhost:9875/api/AdminDashboard/revenue-trend?months=12`
- **Status**: `200`
- **Response (real)**:

```json
[
  {
    "period": "2025-05",
    "revenue": 0,
    "ordersCount": 0
  },
  {
    "period": "2025-06",
    "revenue": 0,
    "ordersCount": 0
  },
  {
    "period": "2025-07",
    "revenue": 0,
    "ordersCount": 0
  },
  {
    "period": "2025-08",
    "revenue": 0,
    "ordersCount": 0
  },
  {
    "period": "2025-09",
    "revenue": 0,
    "ordersCount": 0
  },
  {
    "period": "2025-10",
    "revenue": 0,
    "ordersCount": 0
  },
  {
    "period": "2025-11",
    "revenue": 0,
    "ordersCount": 0
  },
  {
    "period": "2025-12",
    "revenue": 0,
    "ordersCount": 0
  },
  {
    "period": "2026-01",
    "revenue": 0,
    "ordersCount": 0
  },
  {
    "period": "2026-02",
    "revenue": 0,
    "ordersCount": 0
  },
  {
    "period": "2026-03",
    "revenue": 0,
    "ordersCount": 0
  },
  {
    "period": "2026-04",
    "revenue": 0,
    "ordersCount": 0
  }
]
```

### Get Orders by Status

- **Method**: `GET`
- **URL**: `http://localhost:9875/api/AdminDashboard/orders-by-status`
- **Status**: `200`
- **Response (real)**:

```json
[
  {
    "status": "Pending",
    "count": 1
  }
]
```

### Get Tickets by Priority

- **Method**: `GET`
- **URL**: `http://localhost:9875/api/AdminDashboard/tickets-by-priority`
- **Status**: `200`
- **Response (real)**:

```json
[
  {
    "priority": "Unknown",
    "count": 3
  }
]
```

### Get Business Health

- **Method**: `GET`
- **URL**: `http://localhost:9875/api/AdminDashboard/business-health?top=20&sort=desc`
- **Status**: `200`
- **Response (real)**:

```json
[
  {
    "businessId": "c14c1302-f710-4739-bd21-e4fb9c5d1ec0",
    "businessName": "b1",
    "healthScore": 60,
    "averageRating": 0,
    "negativeSentimentRatio": 0,
    "cancellationRate": 0,
    "openTicketsCount": 0,
    "escalatedTicketsCount": 0
  },
  {
    "businessId": "48866c67-0fac-47c1-8acf-10f708cca0b0",
    "businessName": "The Italian Place",
    "healthScore": 60,
    "averageRating": 0,
    "negativeSentimentRatio": 0,
    "cancellationRate": 0,
    "openTicketsCount": 0,
    "escalatedTicketsCount": 0
  },
  {
    "businessId": "4fbf0918-af0c-4d4c-bfcf-df7e0ba29071",
    "businessName": "The Italian Place",
    "healthScore": 60,
    "averageRating": 0,
    "negativeSentimentRatio": 0,
    "cancellationRate": 0,
    "openTicketsCount": 0,
    "escalatedTicketsCount": 0
  },
  {
    "businessId": "6b43667f-a08b-4be5-b2a6-3a782cd07769",
    "businessName": "The Italian Place",
    "healthScore": 60,
    "averageRating": 0,
    "negativeSentimentRatio": 0,
    "cancellationRate": 0,
    "openTicketsCount": 0,
    "escalatedTicketsCount": 0
  },
  {
    "businessId": "d8a4ccc9-a1ee-4302-964e-537c17119a77",
    "businessName": "The Italian Place",
    "healthScore": 60,
    "averageRating": 0,
    "negativeSentimentRatio": 0,
    "cancellationRate": 0,
    "openTicketsCount": 0,
    "escalatedTicketsCount": 0
  },
  {
    "businessId": "e904934f-2b6b-4d71-ae27-2983d2e8e6f4",
    "businessName": "The Italian Place",
    "healthScore": 60,
    "averageRating": 0,
    "negativeSentimentRatio": 0,
    "cancellationRate": 0,
    "openTicketsCount": 0,
    "escalatedTicketsCount": 0
  },
  {
    "businessId": "701f9138-65af-4e2d-8a59-174e0948ce82",
    "businessName": "The Italian Place",
    "healthScore": 59,
    "averageRating": 0,
    "negativeSentimentRatio": 0,
    "cancellationRate": 0,
    "openTicketsCount": 1,
    "escalatedTicketsCount": 0
  },
  {
    "businessId": "1",
    "businessName": "Test Company",
    "healthScore": 58,
    "averageRating": 0,
    "negativeSentimentRatio": 0,
    "cancellationRate": 0,
    "openTicketsCount": 2,
    "escalatedTicketsCount": 0
  }
]
```

### Get Sentiment Trend

- **Method**: `GET`
- **URL**: `http://localhost:9875/api/AdminDashboard/sentiment-trend?days=30`
- **Status**: `200`
- **Response (real)**:

```json
[
  {
    "date": "2026-03-24",
    "positive": 0,
    "negative": 0,
    "neutral": 0
  },
  {
    "date": "2026-03-25",
    "positive": 0,
    "negative": 0,
    "neutral": 0
  },
  {
    "date": "2026-03-26",
    "positive": 0,
    "negative": 0,
    "neutral": 0
  },
  {
    "date": "2026-03-27",
    "positive": 0,
    "negative": 0,
    "neutral": 0
  },
  {
    "date": "2026-03-28",
    "positive": 0,
    "negative": 0,
    "neutral": 0
  },
  {
    "date": "2026-03-29",
    "positive": 0,
    "negative": 0,
    "neutral": 0
  },
  {
    "date": "2026-03-30",
    "positive": 0,
    "negative": 0,
    "neutral": 0
  },
  {
    "date": "2026-03-31",
    "positive": 0,
    "negative": 0,
    "neutral": 0
  },
  {
    "date": "2026-04-01",
    "positive": 0,
    "negative": 0,
    "neutral": 0
  },
  {
    "date": "2026-04-02",
    "positive": 0,
    "negative": 0,
    "neutral": 0
  },
  {
    "date": "2026-04-03",
    "positive": 0,
    "negative": 0,
    "neutral": 0
  },
  {
    "date": "2026-04-04",
    "positive": 0,
    "negative": 0,
    "neutral": 0
  },
  {
    "date": "2026-04-05",
    "positive": 0,
    "negative": 0,
    "neutral": 0
  },
  {
    "date": "2026-04-06",
    "positive": 0,
    "negative": 0,
    "neutral": 0
  },
  {
    "date": "2026-04-07",
    "positive": 0,
    "negative": 0,
    "neutral": 0
  },
  {
    "date": "2026-04-08",
    "positive": 0,
    "negative": 0,
    "neutral": 0
  },
  {
    "date": "2026-04-09",
    "positive": 0,
    "negative": 0,
    "neutral": 0
  },
  {
    "date": "2026-04-10",
    "positive": 0,
    "negative": 0,
    "neutral": 0
  },
  {
    "date": "2026-04-11",
    "positive": 0,
    "negative": 0,
    "neutral": 0
  },
  {
    "date": "2026-04-12",
    "positive": 0,
    "negative": 0,
    "neutral": 0
  },
  {
    "date": "2026-04-13",
    "positive": 0,
    "negative": 0,
    "neutral": 0
  },
  {
    "date": "2026-04-14",
    "positive": 0,
    "negative": 0,
    "neutral": 0
  },
  {
    "date": "2026-04-15",
    "positive": 0,
    "negative": 0,
    "neutral": 0
  },
  {
    "date": "2026-04-16",
    "positive": 0,
    "negative": 0,
    "neutral": 0
  },
  {
    "date": "2026-04-17",
    "positive": 0,
    "negative": 0,
    "neutral": 0
  },
  {
    "date": "2026-04-18",
    "positive": 0,
    "negative": 0,
    "neutral": 0
  },
  {
    "date": "2026-04-19",
    "positive": 0,
    "negative": 0,
    "neutral": 0
  },
  {
    "date": "2026-04-20",
    "positive": 0,
    "negative": 0,
    "neutral": 0
  },
  {
    "date": "2026-04-21",
    "positive": 0,
    "negative": 0,
    "neutral": 0
  },
  {
    "date": "2026-04-22",
    "positive": 0,
    "negative": 0,
    "neutral": 0
  }
]
```

### Suspend Business

- **Method**: `POST`
- **URL**: `http://localhost:9875/api/AdminDashboard/business/ac3fd098-3bd3-4f7f-9084-21e50c605974/suspend`
- **Status**: `404`
- **Response (real)**:

```json
{
  "message": "Business with id 'ac3fd098-3bd3-4f7f-9084-21e50c605974' not found."
}
```

### Activate Business

- **Method**: `POST`
- **URL**: `http://localhost:9875/api/AdminDashboard/business/ac3fd098-3bd3-4f7f-9084-21e50c605974/activate`
- **Status**: `404`
- **Response (real)**:

```json
{
  "message": "Business with id 'ac3fd098-3bd3-4f7f-9084-21e50c605974' not found."
}
```

### Verify Business

- **Method**: `POST`
- **URL**: `http://localhost:9875/api/AdminDashboard/business/ac3fd098-3bd3-4f7f-9084-21e50c605974/verify`
- **Status**: `404`
- **Response (real)**:

```json
{
  "message": "Business with id 'ac3fd098-3bd3-4f7f-9084-21e50c605974' not found."
}
```

### Unverify Business

- **Method**: `POST`
- **URL**: `http://localhost:9875/api/AdminDashboard/business/ac3fd098-3bd3-4f7f-9084-21e50c605974/unverify`
- **Status**: `404`
- **Response (real)**:

```json
{
  "message": "Business with id 'ac3fd098-3bd3-4f7f-9084-21e50c605974' not found."
}
```


## 07 - Menu Category

### Get All Categories

- **Method**: `GET`
- **URL**: `http://localhost:9875/api/MenuCategory`
- **Status**: `200`
- **Response (real)**:

```json
[
  {
    "menuCategoryId": "67da000c-95d0-44fb-8766-1f430efe1e71",
    "name": "Main Dishes",
    "description": "Signature plates",
    "displayOrder": 1,
    "isActive": true,
    "createdAt": "2026-04-22T12:07:25.8789193",
    "businessId": "e904934f-2b6b-4d71-ae27-2983d2e8e6f4",
    "businessName": "",
    "menuItemsCount": 0
  },
  {
    "menuCategoryId": "ae11c382-e32b-4e80-bb05-661c7ed84f75",
    "name": "Main Dishes",
    "description": "Signature plates",
    "displayOrder": 1,
    "isActive": true,
    "createdAt": "2026-04-22T12:11:26.7878272",
    "businessId": "6b43667f-a08b-4be5-b2a6-3a782cd07769",
    "businessName": "",
    "menuItemsCount": 0
  },
  {
    "menuCategoryId": "d5a43adc-174b-46d9-a6dc-c2ec83aa4300",
    "name": "Main Dishes",
    "description": "Signature plates",
    "displayOrder": 1,
    "isActive": true,
    "createdAt": "2026-04-22T12:21:10.027588",
    "businessId": "d8a4ccc9-a1ee-4302-964e-537c17119a77",
    "businessName": "",
    "menuItemsCount": 0
  }
]
```

### Get Categories by Business

- **Method**: `GET`
- **URL**: `http://localhost:9875/api/MenuCategory/business/ac3fd098-3bd3-4f7f-9084-21e50c605974`
- **Status**: `200`
- **Response (real)**:

```json
[]
```

### Get Active Categories [Public]

- **Method**: `GET`
- **URL**: `http://localhost:9875/api/MenuCategory/business/ac3fd098-3bd3-4f7f-9084-21e50c605974/active`
- **Status**: `200`
- **Response (real)**:

```json
[]
```

### Get Category by ID

- **Method**: `GET`
- **URL**: `http://localhost:9875/api/MenuCategory/`
- **Status**: `200`
- **Response (real)**:

```json
[
  {
    "menuCategoryId": "67da000c-95d0-44fb-8766-1f430efe1e71",
    "name": "Main Dishes",
    "description": "Signature plates",
    "displayOrder": 1,
    "isActive": true,
    "createdAt": "2026-04-22T12:07:25.8789193",
    "businessId": "e904934f-2b6b-4d71-ae27-2983d2e8e6f4",
    "businessName": "",
    "menuItemsCount": 0
  },
  {
    "menuCategoryId": "ae11c382-e32b-4e80-bb05-661c7ed84f75",
    "name": "Main Dishes",
    "description": "Signature plates",
    "displayOrder": 1,
    "isActive": true,
    "createdAt": "2026-04-22T12:11:26.7878272",
    "businessId": "6b43667f-a08b-4be5-b2a6-3a782cd07769",
    "businessName": "",
    "menuItemsCount": 0
  },
  {
    "menuCategoryId": "d5a43adc-174b-46d9-a6dc-c2ec83aa4300",
    "name": "Main Dishes",
    "description": "Signature plates",
    "displayOrder": 1,
    "isActive": true,
    "createdAt": "2026-04-22T12:21:10.027588",
    "businessId": "d8a4ccc9-a1ee-4302-964e-537c17119a77",
    "businessName": "",
    "menuItemsCount": 0
  }
]
```

### Create Category [Owner/Admin]

- **Method**: `POST`
- **URL**: `http://localhost:9875/api/MenuCategory`
- **Request body (from Postman)**:

```json
{
  "name": "Main Dishes",
  "description": "Our signature main courses",
  "displayOrder": 1,
  "isActive": true,
  "businessId": "ac3fd098-3bd3-4f7f-9084-21e50c605974"
}
```
- **Status**: `400`
- **Response (real)**:

```json
{
  "message": "Business with id 'ac3fd098-3bd3-4f7f-9084-21e50c605974' not found."
}
```

### Update Category [Owner/Admin]

- **Method**: `PUT`
- **URL**: `http://localhost:9875/api/MenuCategory/`
- **Request body (from Postman)**:

```json
{
  "name": "Starters & Salads",
  "description": "Updated",
  "displayOrder": 1,
  "isActive": true
}
```
- **Status**: `405`
- **Response (real)**:

```json
{
  "raw": ""
}
```

### Reorder Categories [Owner/Admin]

- **Method**: `PUT`
- **URL**: `http://localhost:9875/api/MenuCategory/business/ac3fd098-3bd3-4f7f-9084-21e50c605974/reorder`
- **Request body (from Postman)**:

```json
{
  "categoryOrders": [
    { "menuCategoryId": "", "displayOrder": 1 }
  ]
}
```
- **Status**: `200`
- **Response (real)**:

```json
{
  "message": "Categories reordered successfully."
}
```

### Delete Category [Owner/Admin]

- **Method**: `DELETE`
- **URL**: `http://localhost:9875/api/MenuCategory/`
- **Status**: `405`
- **Response (real)**:

```json
{
  "raw": ""
}
```


## 08 - Menu Item

### Get All Menu Items

- **Method**: `GET`
- **URL**: `http://localhost:9875/api/MenuItem`
- **Status**: `200`
- **Response (real)**:

```json
[
  {
    "menuItemId": "342cafff-ce52-4647-b67c-634bd58351b3",
    "name": "Margherita Pizza",
    "description": "Tomato, mozzarella, fresh basil",
    "price": 89.99,
    "menuCategoryId": "67da000c-95d0-44fb-8766-1f430efe1e71",
    "menuCategoryName": null,
    "isAvailable": true,
    "businessId": "e904934f-2b6b-4d71-ae27-2983d2e8e6f4",
    "businessName": ""
  },
  {
    "menuItemId": "7948c9dd-2b12-473e-8d24-a345455df859",
    "name": "Margherita Pizza",
    "description": "Tomato, mozzarella, fresh basil",
    "price": 89.99,
    "menuCategoryId": "ae11c382-e32b-4e80-bb05-661c7ed84f75",
    "menuCategoryName": null,
    "isAvailable": true,
    "businessId": "6b43667f-a08b-4be5-b2a6-3a782cd07769",
    "businessName": ""
  },
  {
    "menuItemId": "80cf8cd7-c571-4316-a4d3-3602c6f2f587",
    "name": "Margherita Pizza",
    "description": "Tomato, mozzarella, fresh basil",
    "price": 89.99,
    "menuCategoryId": "d5a43adc-174b-46d9-a6dc-c2ec83aa4300",
    "menuCategoryName": null,
    "isAvailable": true,
    "businessId": "d8a4ccc9-a1ee-4302-964e-537c17119a77",
    "businessName": ""
  },
  {
    "menuItemId": "da33f03c-f632-4112-a6c6-a8dd372856fa",
    "name": "burger",
    "description": "description",
    "price": 30.0,
    "menuCategoryId": null,
    "menuCategoryName": null,
    "isAvailable": true,
    "businessId": "1",
    "businessName": ""
  }
]
```

### Get Menu Items by Business

- **Method**: `GET`
- **URL**: `http://localhost:9875/api/MenuItem/business/ac3fd098-3bd3-4f7f-9084-21e50c605974`
- **Status**: `200`
- **Response (real)**:

```json
[]
```

### Get Menu Item by ID

- **Method**: `GET`
- **URL**: `http://localhost:9875/api/MenuItem/`
- **Status**: `200`
- **Response (real)**:

```json
[
  {
    "menuItemId": "342cafff-ce52-4647-b67c-634bd58351b3",
    "name": "Margherita Pizza",
    "description": "Tomato, mozzarella, fresh basil",
    "price": 89.99,
    "menuCategoryId": "67da000c-95d0-44fb-8766-1f430efe1e71",
    "menuCategoryName": null,
    "isAvailable": true,
    "businessId": "e904934f-2b6b-4d71-ae27-2983d2e8e6f4",
    "businessName": ""
  },
  {
    "menuItemId": "7948c9dd-2b12-473e-8d24-a345455df859",
    "name": "Margherita Pizza",
    "description": "Tomato, mozzarella, fresh basil",
    "price": 89.99,
    "menuCategoryId": "ae11c382-e32b-4e80-bb05-661c7ed84f75",
    "menuCategoryName": null,
    "isAvailable": true,
    "businessId": "6b43667f-a08b-4be5-b2a6-3a782cd07769",
    "businessName": ""
  },
  {
    "menuItemId": "80cf8cd7-c571-4316-a4d3-3602c6f2f587",
    "name": "Margherita Pizza",
    "description": "Tomato, mozzarella, fresh basil",
    "price": 89.99,
    "menuCategoryId": "d5a43adc-174b-46d9-a6dc-c2ec83aa4300",
    "menuCategoryName": null,
    "isAvailable": true,
    "businessId": "d8a4ccc9-a1ee-4302-964e-537c17119a77",
    "businessName": ""
  },
  {
    "menuItemId": "da33f03c-f632-4112-a6c6-a8dd372856fa",
    "name": "burger",
    "description": "description",
    "price": 30.0,
    "menuCategoryId": null,
    "menuCategoryName": null,
    "isAvailable": true,
    "businessId": "1",
    "businessName": ""
  }
]
```

### Create Menu Item [Owner/Admin]

- **Method**: `POST`
- **URL**: `http://localhost:9875/api/MenuItem`
- **Request body (from Postman)**:

```json
{
  "name": "Margherita Pizza",
  "description": "Classic tomato sauce, mozzarella, fresh basil",
  "price": 89.99,
  "imageUrl": null,
  "isAvailable": true,
  "menuCategoryId": "",
  "businessId": "ac3fd098-3bd3-4f7f-9084-21e50c605974"
}
```
- **Status**: `400`
- **Response (real)**:

```json
{
  "message": "Business with id 'ac3fd098-3bd3-4f7f-9084-21e50c605974' not found."
}
```

### Update Menu Item [Owner/Admin]

- **Method**: `PUT`
- **URL**: `http://localhost:9875/api/MenuItem/`
- **Request body (from Postman)**:

```json
{
  "name": "Margherita Pizza XL",
  "description": "Extra large classic pizza",
  "price": 109.99,
  "isAvailable": true
}
```
- **Status**: `405`
- **Response (real)**:

```json
{
  "raw": ""
}
```

### Delete Menu Item [Owner/Admin]

- **Method**: `DELETE`
- **URL**: `http://localhost:9875/api/MenuItem/`
- **Status**: `405`
- **Response (real)**:

```json
{
  "raw": ""
}
```


## 09 - Order

### Get All Orders

- **Method**: `GET`
- **URL**: `http://localhost:9875/api/Order`
- **Status**: `200`
- **Response (real)**:

```json
[
  {
    "orderId": "91e76c92-2902-4521-b490-7be29ad0cc0e",
    "totalPrice": 60.0,
    "status": "Pending",
    "createdAt": "2026-02-07T14:05:06.2980903",
    "businessId": "1",
    "businessName": "",
    "customerId": "37b0b04e-7203-46f1-8b00-db4ba9dbb7a9",
    "customerName": "",
    "items": []
  }
]
```

### Get Orders by Business

- **Method**: `GET`
- **URL**: `http://localhost:9875/api/Order/business/ac3fd098-3bd3-4f7f-9084-21e50c605974`
- **Status**: `200`
- **Response (real)**:

```json
[]
```

### Get Orders by Customer

- **Method**: `GET`
- **URL**: `http://localhost:9875/api/Order/customer/`
- **Status**: `404`
- **Response (real)**:

```json
{
  "message": "Order with id 'customer' not found."
}
```

### Get Order by ID

- **Method**: `GET`
- **URL**: `http://localhost:9875/api/Order/`
- **Status**: `200`
- **Response (real)**:

```json
[
  {
    "orderId": "91e76c92-2902-4521-b490-7be29ad0cc0e",
    "totalPrice": 60.0,
    "status": "Pending",
    "createdAt": "2026-02-07T14:05:06.2980903",
    "businessId": "1",
    "businessName": "",
    "customerId": "37b0b04e-7203-46f1-8b00-db4ba9dbb7a9",
    "customerName": "",
    "items": []
  }
]
```

### Create Order

- **Method**: `POST`
- **URL**: `http://localhost:9875/api/Order`
- **Request body (from Postman)**:

```json
{
  "businessId": "ac3fd098-3bd3-4f7f-9084-21e50c605974",
  "customerId": "",
  "items": [
    { "menuItemId": "", "quantity": 2 }
  ]
}
```
- **Status**: `400`
- **Response (real)**:

```json
{
  "message": "Validation Failed",
  "errors": [
    {
      "field": "CustomerId",
      "error": "'Customer Id' must not be empty."
    }
  ]
}
```

### Update Order Status [Owner/Admin]

- **Method**: `PUT`
- **URL**: `http://localhost:9875/api/Order//status`
- **Request body (from Postman)**:

```json
{
  "orderId": "",
  "status": "Paid"
}
```
- **Status**: `404`
- **Response (real)**:

```json
{
  "raw": ""
}
```

### Delete Order [Admin]

- **Method**: `DELETE`
- **URL**: `http://localhost:9875/api/Order/`
- **Status**: `405`
- **Response (real)**:

```json
{
  "raw": ""
}
```


## 10 - Ticket

### Get All Tickets

- **Method**: `GET`
- **URL**: `http://localhost:9875/api/Ticket`
- **Status**: `200`
- **Response (real)**:

```json
[
  {
    "id": "2a9c6e7b-7e58-4c23-ab4d-6dccd9929230",
    "ticketId": "7086d92d-9da3-4b7f-8044-0e91840796fc",
    "businessId": "1",
    "businessName": "",
    "subject": "1",
    "status": "Open",
    "isEnded": false,
    "createdAt": "2026-02-07T13:29:58.6089956",
    "closedAt": null,
    "customerId": "37b0b04e-7203-46f1-8b00-db4ba9dbb7a9",
    "customerName": "",
    "totalFeedback": 0,
    "assignedToUserId": null,
    "assignedToUserName": "",
    "ticketType": null,
    "priorityLevel": null,
    "escalationConfidence": null,
    "escalationReason": null,
    "interactionId": null,
    "relatedOrderId": null
  },
  {
    "id": "4a9fdcbd-ce89-4221-be22-c502116d6e71",
    "ticketId": "65621454-1101-4f71-a15c-768047a5932b",
    "businessId": "701f9138-65af-4e2d-8a59-174e0948ce82",
    "businessName": "",
    "subject": "Wrong order delivered",
    "status": "Open",
    "isEnded": false,
    "createdAt": "2026-04-22T12:11:31.4439549",
    "closedAt": null,
    "customerId": "75b0d7a9-c4fa-474b-ac7d-f8b817f209d8",
    "customerName": "",
    "totalFeedback": 0,
    "assignedToUserId": null,
    "assignedToUserName": "",
    "ticketType": null,
    "priorityLevel": null,
    "escalationConfidence": null,
    "escalationReason": null,
    "interactionId": null,
    "relatedOrderId": null
  },
  {
    "id": "a7edcac8-3c09-4be6-8d39-ab639281ce6f",
    "ticketId": "36a102b4-c6d6-42aa-bd57-c205a5227d1a",
    "businessId": "1",
    "businessName": "",
    "subject": "1",
    "status": "Open",
    "isEnded": false,
    "createdAt": "2026-02-07T13:31:15.301326",
    "closedAt": null,
    "customerId": "37b0b04e-7203-46f1-8b00-db4ba9dbb7a9",
    "customerName": "",
    "totalFeedback": 0,
    "assignedToUserId": null,
    "assignedToUserName": "",
    "ticketType": null,
    "priorityLevel": null,
    "escalationConfidence": null,
    "escalationReason": null,
    "interactionId": null,
    "relatedOrderId": null
  }
]
```

### Get Escalation Queue

- **Method**: `GET`
- **URL**: `http://localhost:9875/api/Ticket/queue`
- **Status**: `400`
- **Response (real)**:

```json
{
  "message": "BusinessId not found in token."
}
```

### Get Tickets by Business

- **Method**: `GET`
- **URL**: `http://localhost:9875/api/Ticket/business/ac3fd098-3bd3-4f7f-9084-21e50c605974`
- **Status**: `200`
- **Response (real)**:

```json
[]
```

### Get Ticket by ID

- **Method**: `GET`
- **URL**: `http://localhost:9875/api/Ticket/`
- **Status**: `200`
- **Response (real)**:

```json
[
  {
    "id": "2a9c6e7b-7e58-4c23-ab4d-6dccd9929230",
    "ticketId": "7086d92d-9da3-4b7f-8044-0e91840796fc",
    "businessId": "1",
    "businessName": "",
    "subject": "1",
    "status": "Open",
    "isEnded": false,
    "createdAt": "2026-02-07T13:29:58.6089956",
    "closedAt": null,
    "customerId": "37b0b04e-7203-46f1-8b00-db4ba9dbb7a9",
    "customerName": "",
    "totalFeedback": 0,
    "assignedToUserId": null,
    "assignedToUserName": "",
    "ticketType": null,
    "priorityLevel": null,
    "escalationConfidence": null,
    "escalationReason": null,
    "interactionId": null,
    "relatedOrderId": null
  },
  {
    "id": "4a9fdcbd-ce89-4221-be22-c502116d6e71",
    "ticketId": "65621454-1101-4f71-a15c-768047a5932b",
    "businessId": "701f9138-65af-4e2d-8a59-174e0948ce82",
    "businessName": "",
    "subject": "Wrong order delivered",
    "status": "Open",
    "isEnded": false,
    "createdAt": "2026-04-22T12:11:31.4439549",
    "closedAt": null,
    "customerId": "75b0d7a9-c4fa-474b-ac7d-f8b817f209d8",
    "customerName": "",
    "totalFeedback": 0,
    "assignedToUserId": null,
    "assignedToUserName": "",
    "ticketType": null,
    "priorityLevel": null,
    "escalationConfidence": null,
    "escalationReason": null,
    "interactionId": null,
    "relatedOrderId": null
  },
  {
    "id": "a7edcac8-3c09-4be6-8d39-ab639281ce6f",
    "ticketId": "36a102b4-c6d6-42aa-bd57-c205a5227d1a",
    "businessId": "1",
    "businessName": "",
    "subject": "1",
    "status": "Open",
    "isEnded": false,
    "createdAt": "2026-02-07T13:31:15.301326",
    "closedAt": null,
    "customerId": "37b0b04e-7203-46f1-8b00-db4ba9dbb7a9",
    "customerName": "",
    "totalFeedback": 0,
    "assignedToUserId": null,
    "assignedToUserName": "",
    "ticketType": null,
    "priorityLevel": null,
    "escalationConfidence": null,
    "escalationReason": null,
    "interactionId": null,
    "relatedOrderId": null
  }
]
```

### Create Ticket

- **Method**: `POST`
- **URL**: `http://localhost:9875/api/Ticket`
- **Request body (from Postman)**:

```json
{
  "businessId": "ac3fd098-3bd3-4f7f-9084-21e50c605974",
  "customerId": "",
  "subject": "Wrong order delivered",
  "description": "I ordered Margherita but received Pepperoni",
  "priority": "High",
  "type": "Complaint",
  "interactionId": null
}
```
- **Status**: `400`
- **Response (real)**:

```json
{
  "message": "Validation Failed",
  "errors": [
    {
      "field": "CustomerId",
      "error": "CustomerId is required"
    }
  ]
}
```

### Update Ticket

- **Method**: `PUT`
- **URL**: `http://localhost:9875/api/Ticket/`
- **Request body (from Postman)**:

```json
{
  "subject": "Updated subject",
  "description": "Updated description",
  "priority": "Medium",
  "status": "InProgress"
}
```
- **Status**: `405`
- **Response (real)**:

```json
{
  "raw": ""
}
```

### Assign Ticket [Owner/Admin]

- **Method**: `POST`
- **URL**: `http://localhost:9875/api/Ticket//assign`
- **Request body (from Postman)**:

```json
{
  "assignedToUserId": ""
}
```
- **Status**: `404`
- **Response (real)**:

```json
{
  "raw": ""
}
```

### Close Ticket

- **Method**: `POST`
- **URL**: `http://localhost:9875/api/Ticket//close`
- **Request body (from Postman)**:

```json
{
  "resolutionNote": "Issue resolved. Customer refunded.",
  "closedByUserId": ""
}
```
- **Status**: `404`
- **Response (real)**:

```json
{
  "raw": ""
}
```

### Delete Ticket [Admin]

- **Method**: `DELETE`
- **URL**: `http://localhost:9875/api/Ticket/`
- **Status**: `405`
- **Response (real)**:

```json
{
  "raw": ""
}
```


## 11 - Interaction

### Get All Interactions

- **Method**: `GET`
- **URL**: `http://localhost:9875/api/Interaction`
- **Status**: `200`
- **Response (real)**:

```json
[
  {
    "interactionId": "b2b44d22-c916-459c-aa2f-a4d07399f4b4",
    "businessId": "1",
    "businessName": "",
    "handledByAgentName": "",
    "channel": "WhatsApp",
    "status": "Open",
    "isEnded": false,
    "startedAt": "2026-02-07T13:42:50.5118923",
    "endedAt": null,
    "customerId": "37b0b04e-7203-46f1-8b00-db4ba9dbb7a9",
    "customerName": "",
    "messageCount": 0,
    "handledByAgentId": null,
    "messages": null
  },
  {
    "interactionId": "be2cf649-e62c-4cdb-88c5-a4535c37ffa4",
    "businessId": "701f9138-65af-4e2d-8a59-174e0948ce82",
    "businessName": "",
    "handledByAgentName": "",
    "channel": "WebChat",
    "status": "Interrupted",
    "isEnded": true,
    "startedAt": "2026-04-22T12:11:32.1663035",
    "endedAt": "2026-04-22T12:11:32.2410069",
    "customerId": "75b0d7a9-c4fa-474b-ac7d-f8b817f209d8",
    "customerName": "",
    "messageCount": 0,
    "handledByAgentId": null,
    "messages": null
  }
]
```

### Get Interactions by Business

- **Method**: `GET`
- **URL**: `http://localhost:9875/api/Interaction/business/ac3fd098-3bd3-4f7f-9084-21e50c605974`
- **Status**: `200`
- **Response (real)**:

```json
[]
```

### Get Interactions by Customer

- **Method**: `GET`
- **URL**: `http://localhost:9875/api/Interaction/customer/`
- **Status**: `404`
- **Response (real)**:

```json
{
  "message": "Interaction with id 'customer' not found."
}
```

### Get Interactions by User

- **Method**: `GET`
- **URL**: `http://localhost:9875/api/Interaction/user/`
- **Status**: `404`
- **Response (real)**:

```json
{
  "message": "Interaction with id 'user' not found."
}
```

### Get Interaction by ID

- **Method**: `GET`
- **URL**: `http://localhost:9875/api/Interaction/`
- **Status**: `200`
- **Response (real)**:

```json
[
  {
    "interactionId": "b2b44d22-c916-459c-aa2f-a4d07399f4b4",
    "businessId": "1",
    "businessName": "",
    "handledByAgentName": "",
    "channel": "WhatsApp",
    "status": "Open",
    "isEnded": false,
    "startedAt": "2026-02-07T13:42:50.5118923",
    "endedAt": null,
    "customerId": "37b0b04e-7203-46f1-8b00-db4ba9dbb7a9",
    "customerName": "",
    "messageCount": 0,
    "handledByAgentId": null,
    "messages": null
  },
  {
    "interactionId": "be2cf649-e62c-4cdb-88c5-a4535c37ffa4",
    "businessId": "701f9138-65af-4e2d-8a59-174e0948ce82",
    "businessName": "",
    "handledByAgentName": "",
    "channel": "WebChat",
    "status": "Interrupted",
    "isEnded": true,
    "startedAt": "2026-04-22T12:11:32.1663035",
    "endedAt": "2026-04-22T12:11:32.2410069",
    "customerId": "75b0d7a9-c4fa-474b-ac7d-f8b817f209d8",
    "customerName": "",
    "messageCount": 0,
    "handledByAgentId": null,
    "messages": null
  }
]
```

### Start Interaction

- **Method**: `POST`
- **URL**: `http://localhost:9875/api/Interaction/start`
- **Request body (from Postman)**:

```json
{
  "businessId": "ac3fd098-3bd3-4f7f-9084-21e50c605974",
  "customerId": "",
  "channel": "WebChat",
  "assignedUserId": null
}
```
- **Status**: `400`
- **Response (real)**:

```json
{
  "message": "Validation Failed",
  "errors": [
    {
      "field": "CustomerId",
      "error": "'Customer Id' must not be empty."
    }
  ]
}
```

### End Interaction

- **Method**: `POST`
- **URL**: `http://localhost:9875/api/Interaction//end`
- **Request body (from Postman)**:

```json
{
  "resolutionStatus": "Resolved",
  "notes": "Customer satisfied"
}
```
- **Status**: `404`
- **Response (real)**:

```json
{
  "raw": ""
}
```

### Delete Interaction [Admin]

- **Method**: `DELETE`
- **URL**: `http://localhost:9875/api/Interaction/`
- **Status**: `405`
- **Response (real)**:

```json
{
  "raw": ""
}
```


## 12 - Message

### Get All Messages

- **Method**: `GET`
- **URL**: `http://localhost:9875/api/Message`
- **Status**: `200`
- **Response (real)**:

```json
[]
```

### Get Messages by Interaction

- **Method**: `GET`
- **URL**: `http://localhost:9875/api/Message/interaction/`
- **Status**: `404`
- **Response (real)**:

```json
{
  "message": "Message with id 'interaction' not found."
}
```

### Get Message by ID

- **Method**: `GET`
- **URL**: `http://localhost:9875/api/Message/`
- **Status**: `200`
- **Response (real)**:

```json
[]
```

### Create Message

- **Method**: `POST`
- **URL**: `http://localhost:9875/api/Message`
- **Request body (from Postman)**:

```json
{
  "interactionId": "",
  "content": "Hello, how can I help you?",
  "senderType": "Agent",
  "senderId": ""
}
```
- **Status**: `400`
- **Response (real)**:

```json
{
  "message": "Validation Failed",
  "errors": [
    {
      "field": "UserId",
      "error": "'User Id' must not be empty."
    },
    {
      "field": "InteractionId",
      "error": "'Interaction Id' must not be empty."
    }
  ]
}
```

### Delete Message [Admin]

- **Method**: `DELETE`
- **URL**: `http://localhost:9875/api/Message/`
- **Status**: `405`
- **Response (real)**:

```json
{
  "raw": ""
}
```


## 13 - Knowledge Base

### Get All KB Items

- **Method**: `GET`
- **URL**: `http://localhost:9875/api/KnowledgeBase`
- **Status**: `200`
- **Response (real)**:

```json
[
  {
    "knowledgeBaseId": "7353b7f2-ee2b-4b6a-a3d5-6f2bc5a6d493",
    "question": "question",
    "answer": "answer",
    "createdAt": "2026-02-07T13:44:26.4452501",
    "businessId": "1",
    "businessName": "",
    "isFAQ": false,
    "displayOrder": 0,
    "isActive": false
  }
]
```

### Get KB Items by Business

- **Method**: `GET`
- **URL**: `http://localhost:9875/api/KnowledgeBase/business/ac3fd098-3bd3-4f7f-9084-21e50c605974`
- **Status**: `200`
- **Response (real)**:

```json
[]
```

### Get KB Item by ID

- **Method**: `GET`
- **URL**: `http://localhost:9875/api/KnowledgeBase/`
- **Status**: `200`
- **Response (real)**:

```json
[
  {
    "knowledgeBaseId": "7353b7f2-ee2b-4b6a-a3d5-6f2bc5a6d493",
    "question": "question",
    "answer": "answer",
    "createdAt": "2026-02-07T13:44:26.4452501",
    "businessId": "1",
    "businessName": "",
    "isFAQ": false,
    "displayOrder": 0,
    "isActive": false
  }
]
```

### Create KB Item [Owner/Admin]

- **Method**: `POST`
- **URL**: `http://localhost:9875/api/KnowledgeBase`
- **Request body (from Postman)**:

```json
{
  "title": "How to place an order",
  "content": "To place an order, browse our menu and add items to your cart...",
  "businessId": "ac3fd098-3bd3-4f7f-9084-21e50c605974",
  "isFAQ": false,
  "tags": "ordering, how-to, menu"
}
```
- **Status**: `400`
- **Response (real)**:

```json
{
  "message": "Validation Failed",
  "errors": [
    {
      "field": "Answer",
      "error": "The Answer field is required."
    },
    {
      "field": "Question",
      "error": "The Question field is required."
    }
  ]
}
```

### Update KB Item [Owner/Admin]

- **Method**: `PUT`
- **URL**: `http://localhost:9875/api/KnowledgeBase/`
- **Request body (from Postman)**:

```json
{
  "title": "Updated title",
  "content": "Updated content here.",
  "businessId": "ac3fd098-3bd3-4f7f-9084-21e50c605974",
  "isFAQ": false,
  "tags": "updated"
}
```
- **Status**: `405`
- **Response (real)**:

```json
{
  "raw": ""
}
```

### Delete KB Item [Owner/Admin]

- **Method**: `DELETE`
- **URL**: `http://localhost:9875/api/KnowledgeBase/`
- **Status**: `405`
- **Response (real)**:

```json
{
  "raw": ""
}
```


## 14 - FAQ

### Get FAQs by Business [Public]

- **Method**: `GET`
- **URL**: `http://localhost:9875/api/FAQ/business/ac3fd098-3bd3-4f7f-9084-21e50c605974`
- **Status**: `200`
- **Response (real)**:

```json
[]
```

### Manage FAQs [Owner/Admin]

- **Method**: `GET`
- **URL**: `http://localhost:9875/api/FAQ/business/ac3fd098-3bd3-4f7f-9084-21e50c605974/manage`
- **Status**: `200`
- **Response (real)**:

```json
[]
```

### Get FAQ by ID [Public]

- **Method**: `GET`
- **URL**: `http://localhost:9875/api/FAQ/`
- **Status**: `405`
- **Response (real)**:

```json
{
  "raw": ""
}
```

### Create FAQ [Owner/Admin]

- **Method**: `POST`
- **URL**: `http://localhost:9875/api/FAQ`
- **Request body (from Postman)**:

```json
{
  "title": "What are your opening hours?",
  "content": "We are open from 9 AM to 10 PM, Monday to Saturday. Closed on Sundays.",
  "businessId": "ac3fd098-3bd3-4f7f-9084-21e50c605974",
  "isFAQ": true,
  "tags": "hours, schedule, open"
}
```
- **Status**: `400`
- **Response (real)**:

```json
{
  "message": "Validation Failed",
  "errors": [
    {
      "field": "Answer",
      "error": "The Answer field is required."
    },
    {
      "field": "Question",
      "error": "The Question field is required."
    }
  ]
}
```

### Update FAQ [Owner/Admin]

- **Method**: `PUT`
- **URL**: `http://localhost:9875/api/FAQ/`
- **Request body (from Postman)**:

```json
{
  "title": "Updated FAQ question?",
  "content": "Updated FAQ answer.",
  "businessId": "ac3fd098-3bd3-4f7f-9084-21e50c605974",
  "isFAQ": true
}
```
- **Status**: `405`
- **Response (real)**:

```json
{
  "raw": ""
}
```

### Delete FAQ [Owner/Admin]

- **Method**: `DELETE`
- **URL**: `http://localhost:9875/api/FAQ/`
- **Status**: `405`
- **Response (real)**:

```json
{
  "raw": ""
}
```


## 15 - Feedback

### Get All Feedback

- **Method**: `GET`
- **URL**: `http://localhost:9875/api/Feedback`
- **Status**: `200`
- **Response (real)**:

```json
[
  {
    "feedbackId": "469bffe4-c4c8-4cbf-80c6-2c3594df0a3f",
    "ticketId": "a7edcac8-3c09-4be6-8d39-ab639281ce6f",
    "tickerSubject": "",
    "customerId": "37b0b04e-7203-46f1-8b00-db4ba9dbb7a9",
    "customerName": "",
    "rating": 3,
    "comment": "this is a comment",
    "createdAt": "2026-02-07T13:31:37.7474347"
  },
  {
    "feedbackId": "c957d440-08fa-423c-8f0f-82d8f3797fc7",
    "ticketId": null,
    "tickerSubject": "",
    "customerId": "75b0d7a9-c4fa-474b-ac7d-f8b817f209d8",
    "customerName": "",
    "rating": 5,
    "comment": "Very helpful voice assistant!",
    "createdAt": "2026-04-22T12:11:37.2220859"
  }
]
```

### Get Feedback by Customer

- **Method**: `GET`
- **URL**: `http://localhost:9875/api/Feedback/customer/`
- **Status**: `404`
- **Response (real)**:

```json
{
  "message": "Feedback with id 'customer' not found."
}
```

### Get Feedback by ID

- **Method**: `GET`
- **URL**: `http://localhost:9875/api/Feedback/`
- **Status**: `200`
- **Response (real)**:

```json
[
  {
    "feedbackId": "469bffe4-c4c8-4cbf-80c6-2c3594df0a3f",
    "ticketId": "a7edcac8-3c09-4be6-8d39-ab639281ce6f",
    "tickerSubject": "",
    "customerId": "37b0b04e-7203-46f1-8b00-db4ba9dbb7a9",
    "customerName": "",
    "rating": 3,
    "comment": "this is a comment",
    "createdAt": "2026-02-07T13:31:37.7474347"
  },
  {
    "feedbackId": "c957d440-08fa-423c-8f0f-82d8f3797fc7",
    "ticketId": null,
    "tickerSubject": "",
    "customerId": "75b0d7a9-c4fa-474b-ac7d-f8b817f209d8",
    "customerName": "",
    "rating": 5,
    "comment": "Very helpful voice assistant!",
    "createdAt": "2026-04-22T12:11:37.2220859"
  }
]
```

### Submit Feedback [Public]

- **Method**: `POST`
- **URL**: `http://localhost:9875/api/Feedback`
- **Request body (from Postman)**:

```json
{
  "customerId": "",
  "interactionId": "",
  "rating": 5,
  "comment": "Excellent service and food!",
  "businessId": "ac3fd098-3bd3-4f7f-9084-21e50c605974"
}
```
- **Status**: `400`
- **Response (real)**:

```json
{
  "message": "Validation Failed",
  "errors": [
    {
      "field": "TicketId",
      "error": "The TicketId field is required."
    },
    {
      "field": "CustomerId",
      "error": "CustomerId is required"
    }
  ]
}
```

### Update Feedback [Owner/Admin]

- **Method**: `PUT`
- **URL**: `http://localhost:9875/api/Feedback/`
- **Request body (from Postman)**:

```json
{
  "rating": 4,
  "comment": "Updated comment"
}
```
- **Status**: `405`
- **Response (real)**:

```json
{
  "raw": ""
}
```

### Delete Feedback [Admin]

- **Method**: `DELETE`
- **URL**: `http://localhost:9875/api/Feedback/`
- **Status**: `405`
- **Response (real)**:

```json
{
  "raw": ""
}
```


## 16 - Sentiment [Read Only]

### Get All Sentiments

- **Method**: `GET`
- **URL**: `http://localhost:9875/api/Sentiment`
- **Status**: `200`
- **Response (real)**:

```json
[]
```

### Get Sentiments by Message

- **Method**: `GET`
- **URL**: `http://localhost:9875/api/Sentiment/message/`
- **Status**: `404`
- **Response (real)**:

```json
{
  "message": "Sentiment with id 'message' not found."
}
```

### Get Sentiments by Business

- **Method**: `GET`
- **URL**: `http://localhost:9875/api/Sentiment/business/ac3fd098-3bd3-4f7f-9084-21e50c605974`
- **Status**: `200`
- **Response (real)**:

```json
[]
```

### Get Sentiment by ID

- **Method**: `GET`
- **URL**: `http://localhost:9875/api/Sentiment/`
- **Status**: `200`
- **Response (real)**:

```json
[]
```


## 17 - Notification

### Get All Notifications

- **Method**: `GET`
- **URL**: `http://localhost:9875/api/Notification`
- **Status**: `200`
- **Response (real)**:

```json
[
  {
    "notificationId": "faadbc7c-408e-48a9-95a1-0ea4f499c53b",
    "title": "notifaction from ................",
    "message": "hi",
    "isRead": false,
    "createdAt": "2026-02-07T15:21:02.4306696",
    "userId": "0803d5af-aeb5-4eaa-8e9e-c2622eae4a51",
    "userName": "",
    "businessId": "1",
    "businessName": "",
    "type": null
  }
]
```

### Get Notifications by Business

- **Method**: `GET`
- **URL**: `http://localhost:9875/api/Notification/business/ac3fd098-3bd3-4f7f-9084-21e50c605974`
- **Status**: `200`
- **Response (real)**:

```json
[]
```

### Get Notifications by User

- **Method**: `GET`
- **URL**: `http://localhost:9875/api/Notification/user/`
- **Status**: `404`
- **Response (real)**:

```json
{
  "message": "Notification with id 'user' not found."
}
```

### Get Notification by ID

- **Method**: `GET`
- **URL**: `http://localhost:9875/api/Notification/`
- **Status**: `200`
- **Response (real)**:

```json
[
  {
    "notificationId": "faadbc7c-408e-48a9-95a1-0ea4f499c53b",
    "title": "notifaction from ................",
    "message": "hi",
    "isRead": false,
    "createdAt": "2026-02-07T15:21:02.4306696",
    "userId": "0803d5af-aeb5-4eaa-8e9e-c2622eae4a51",
    "userName": "",
    "businessId": "1",
    "businessName": "",
    "type": null
  }
]
```

### Create Notification [Owner/Admin]

- **Method**: `POST`
- **URL**: `http://localhost:9875/api/Notification`
- **Request body (from Postman)**:

```json
{
  "businessId": "ac3fd098-3bd3-4f7f-9084-21e50c605974",
  "userId": "",
  "title": "New Order Received",
  "message": "Order #1234 has been placed and is awaiting confirmation.",
  "type": "Order"
}
```
- **Status**: `400`
- **Response (real)**:

```json
{
  "message": "Validation Failed",
  "errors": [
    {
      "field": "UserId",
      "error": "'User Id' must not be empty."
    }
  ]
}
```

### Mark as Read

- **Method**: `PUT`
- **URL**: `http://localhost:9875/api/Notification//read`
- **Status**: `404`
- **Response (real)**:

```json
{
  "raw": ""
}
```

### Delete Notification [Owner/Admin]

- **Method**: `DELETE`
- **URL**: `http://localhost:9875/api/Notification/`
- **Status**: `405`
- **Response (real)**:

```json
{
  "raw": ""
}
```


## 18 - Report

### Get All Reports

- **Method**: `GET`
- **URL**: `http://localhost:9875/api/Report`
- **Status**: `200`
- **Response (real)**:

```json
[
  {
    "id": "668af9bb-8692-4266-8974-2a25cb61ba65",
    "reportId": "98d354cc-8c50-4a21-95bd-410964a57ef1",
    "title": "there is a report",
    "reportType": "Performance",
    "generatedAt": "2026-02-07T14:09:05.7775053",
    "filePath": "",
    "fileUrl": null,
    "businessId": "1",
    "businessName": "",
    "summary": null
  }
]
```

### Get Reports by Business

- **Method**: `GET`
- **URL**: `http://localhost:9875/api/Report/business/ac3fd098-3bd3-4f7f-9084-21e50c605974`
- **Status**: `200`
- **Response (real)**:

```json
[]
```

### Get Report by ID

- **Method**: `GET`
- **URL**: `http://localhost:9875/api/Report/`
- **Status**: `200`
- **Response (real)**:

```json
[
  {
    "id": "668af9bb-8692-4266-8974-2a25cb61ba65",
    "reportId": "98d354cc-8c50-4a21-95bd-410964a57ef1",
    "title": "there is a report",
    "reportType": "Performance",
    "generatedAt": "2026-02-07T14:09:05.7775053",
    "filePath": "",
    "fileUrl": null,
    "businessId": "1",
    "businessName": "",
    "summary": null
  }
]
```

### Create Report [Owner/Admin]

- **Method**: `POST`
- **URL**: `http://localhost:9875/api/Report`
- **Request body (from Postman)**:

```json
{
  "businessId": "ac3fd098-3bd3-4f7f-9084-21e50c605974",
  "type": "Sales",
  "period": "Monthly",
  "startDate": "2026-03-01T00:00:00Z",
  "endDate": "2026-03-31T23:59:59Z"
}
```
- **Status**: `400`
- **Response (real)**:

```json
{
  "message": "Validation Failed",
  "errors": [
    {
      "field": "Title",
      "error": "The Title field is required."
    }
  ]
}
```

### Delete Report [Admin]

- **Method**: `DELETE`
- **URL**: `http://localhost:9875/api/Report/`
- **Status**: `405`
- **Response (real)**:

```json
{
  "raw": ""
}
```


## 19 - Subscription

### Get All Subscriptions [Admin]

- **Method**: `GET`
- **URL**: `http://localhost:9875/api/Subscription`
- **Status**: `200`
- **Response (real)**:

```json
[
  {
    "id": "1c78dba3-5cbb-4c1c-add9-e53d45429bcf",
    "subscriptionId": "e3a443e8-58d9-4d73-869f-a2ac334d0db8",
    "planName": "Monthly",
    "price": 49.99,
    "isActive": true,
    "businessId": "6b43667f-a08b-4be5-b2a6-3a782cd07769",
    "businessName": "",
    "startDate": "2026-04-22T12:11:26.825164",
    "endDate": "2026-05-22T12:11:26.825164"
  },
  {
    "id": "75334fa4-bf09-4ad2-b8f6-53c123e80e62",
    "subscriptionId": "e9e0415b-0646-4bd9-a3ac-f3fd3f95269f",
    "planName": "Monthly",
    "price": 49.99,
    "isActive": true,
    "businessId": "e904934f-2b6b-4d71-ae27-2983d2e8e6f4",
    "businessName": "",
    "startDate": "2026-04-22T12:07:25.9141717",
    "endDate": "2026-05-22T12:07:25.9141717"
  },
  {
    "id": "abe0c9e6-d2e9-4458-aba0-c53726e2f92f",
    "subscriptionId": "35c23624-79ea-4e32-920a-cf9d05336325",
    "planName": "Monthly",
    "price": 49.99,
    "isActive": true,
    "businessId": "d8a4ccc9-a1ee-4302-964e-537c17119a77",
    "businessName": "",
    "startDate": "2026-04-22T12:21:10.0489804",
    "endDate": "2026-05-22T12:21:10.0489804"
  }
]
```

### Get Subscriptions by Business [Owner/Admin]

- **Method**: `GET`
- **URL**: `http://localhost:9875/api/Subscription/business/ac3fd098-3bd3-4f7f-9084-21e50c605974`
- **Status**: `200`
- **Response (real)**:

```json
[]
```

### Get Active Subscription [Owner/Admin]

- **Method**: `GET`
- **URL**: `http://localhost:9875/api/Subscription/business/ac3fd098-3bd3-4f7f-9084-21e50c605974/active`
- **Status**: `404`
- **Response (real)**:

```json
{
  "message": "No active subscription found for business 'ac3fd098-3bd3-4f7f-9084-21e50c605974'."
}
```

### Get Subscription by ID [Owner/Admin]

- **Method**: `GET`
- **URL**: `http://localhost:9875/api/Subscription/`
- **Status**: `200`
- **Response (real)**:

```json
[
  {
    "id": "1c78dba3-5cbb-4c1c-add9-e53d45429bcf",
    "subscriptionId": "e3a443e8-58d9-4d73-869f-a2ac334d0db8",
    "planName": "Monthly",
    "price": 49.99,
    "isActive": true,
    "businessId": "6b43667f-a08b-4be5-b2a6-3a782cd07769",
    "businessName": "",
    "startDate": "2026-04-22T12:11:26.825164",
    "endDate": "2026-05-22T12:11:26.825164"
  },
  {
    "id": "75334fa4-bf09-4ad2-b8f6-53c123e80e62",
    "subscriptionId": "e9e0415b-0646-4bd9-a3ac-f3fd3f95269f",
    "planName": "Monthly",
    "price": 49.99,
    "isActive": true,
    "businessId": "e904934f-2b6b-4d71-ae27-2983d2e8e6f4",
    "businessName": "",
    "startDate": "2026-04-22T12:07:25.9141717",
    "endDate": "2026-05-22T12:07:25.9141717"
  },
  {
    "id": "abe0c9e6-d2e9-4458-aba0-c53726e2f92f",
    "subscriptionId": "35c23624-79ea-4e32-920a-cf9d05336325",
    "planName": "Monthly",
    "price": 49.99,
    "isActive": true,
    "businessId": "d8a4ccc9-a1ee-4302-964e-537c17119a77",
    "businessName": "",
    "startDate": "2026-04-22T12:21:10.0489804",
    "endDate": "2026-05-22T12:21:10.0489804"
  }
]
```

### Create Subscription [Admin]

- **Method**: `POST`
- **URL**: `http://localhost:9875/api/Subscription`
- **Request body (from Postman)**:

```json
{
  "businessId": "ac3fd098-3bd3-4f7f-9084-21e50c605974",
  "plan": "Pro",
  "startDate": "2026-03-01T00:00:00Z",
  "endDate": "2026-04-01T00:00:00Z",
  "monthlyPrice": 49.99
}
```
- **Status**: `400`
- **Response (real)**:

```json
{
  "message": "Validation Failed",
  "errors": [
    {
      "field": "Price",
      "error": "'Price' must be greater than '0'."
    },
    {
      "field": "PlanName",
      "error": "The PlanName field is required."
    }
  ]
}
```

### Renew Subscription [Admin]

- **Method**: `POST`
- **URL**: `http://localhost:9875/api/Subscription//renew`
- **Request body (from Postman)**:

```json
{
  "newEndDate": "2026-05-01T00:00:00Z"
}
```
- **Status**: `404`
- **Response (real)**:

```json
{
  "raw": ""
}
```

### Delete Subscription [Admin]

- **Method**: `DELETE`
- **URL**: `http://localhost:9875/api/Subscription/`
- **Status**: `405`
- **Response (real)**:

```json
{
  "raw": ""
}
```


## 20 - Payment Transaction

### Get All Payments [Admin]

- **Method**: `GET`
- **URL**: `http://localhost:9875/api/PaymentTransaction`
- **Status**: `200`
- **Response (real)**:

```json
[
  {
    "id": "0946c2e2-4918-49da-beef-8fdfaa54af15",
    "paymentId": "1894ba0b-97cb-40a5-af50-10a5d45ffa21",
    "subscriptionId": "abe0c9e6-d2e9-4458-aba0-c53726e2f92f",
    "subscriptionPlanName": "",
    "amount": 49.99,
    "paymentMethod": "Card",
    "transactionDate": "2026-04-22T12:21:10.060839",
    "status": "Success"
  },
  {
    "id": "c85ecf55-02a1-4d43-8ad9-cf9c26b74eee",
    "paymentId": "cf1d0769-175f-4709-b007-a85b290e7d67",
    "subscriptionId": "1c78dba3-5cbb-4c1c-add9-e53d45429bcf",
    "subscriptionPlanName": "",
    "amount": 49.99,
    "paymentMethod": "Card",
    "transactionDate": "2026-04-22T12:11:26.8376154",
    "status": "Success"
  },
  {
    "id": "fcbbb82c-42c2-456c-988c-cb7666b30122",
    "paymentId": "85f60757-1244-4522-af3b-5460674c355c",
    "subscriptionId": "75334fa4-bf09-4ad2-b8f6-53c123e80e62",
    "subscriptionPlanName": "",
    "amount": 49.99,
    "paymentMethod": "Card",
    "transactionDate": "2026-04-22T12:07:25.9326197",
    "status": "Success"
  }
]
```

### Get Payments by Subscription [Owner/Admin]

- **Method**: `GET`
- **URL**: `http://localhost:9875/api/PaymentTransaction/subscription/`
- **Status**: `404`
- **Response (real)**:

```json
{
  "message": "PaymentTransaction with id 'subscription' not found."
}
```

### Get Payments by Business [Owner/Admin]

- **Method**: `GET`
- **URL**: `http://localhost:9875/api/PaymentTransaction/business/ac3fd098-3bd3-4f7f-9084-21e50c605974`
- **Status**: `200`
- **Response (real)**:

```json
[]
```

### Get Payment by ID [Owner/Admin]

- **Method**: `GET`
- **URL**: `http://localhost:9875/api/PaymentTransaction/`
- **Status**: `200`
- **Response (real)**:

```json
[
  {
    "id": "0946c2e2-4918-49da-beef-8fdfaa54af15",
    "paymentId": "1894ba0b-97cb-40a5-af50-10a5d45ffa21",
    "subscriptionId": "abe0c9e6-d2e9-4458-aba0-c53726e2f92f",
    "subscriptionPlanName": "",
    "amount": 49.99,
    "paymentMethod": "Card",
    "transactionDate": "2026-04-22T12:21:10.060839",
    "status": "Success"
  },
  {
    "id": "c85ecf55-02a1-4d43-8ad9-cf9c26b74eee",
    "paymentId": "cf1d0769-175f-4709-b007-a85b290e7d67",
    "subscriptionId": "1c78dba3-5cbb-4c1c-add9-e53d45429bcf",
    "subscriptionPlanName": "",
    "amount": 49.99,
    "paymentMethod": "Card",
    "transactionDate": "2026-04-22T12:11:26.8376154",
    "status": "Success"
  },
  {
    "id": "fcbbb82c-42c2-456c-988c-cb7666b30122",
    "paymentId": "85f60757-1244-4522-af3b-5460674c355c",
    "subscriptionId": "75334fa4-bf09-4ad2-b8f6-53c123e80e62",
    "subscriptionPlanName": "",
    "amount": 49.99,
    "paymentMethod": "Card",
    "transactionDate": "2026-04-22T12:07:25.9326197",
    "status": "Success"
  }
]
```

### Create Payment [Admin]

- **Method**: `POST`
- **URL**: `http://localhost:9875/api/PaymentTransaction`
- **Request body (from Postman)**:

```json
{
  "subscriptionId": "",
  "amount": 49.99,
  "currency": "USD",
  "paymentMethod": "Visa",
  "transactionReference": "TXN-001",
  "status": "Success"
}
```
- **Status**: `400`
- **Response (real)**:

```json
{
  "message": "Validation Failed",
  "errors": [
    {
      "field": "SubscriptionId",
      "error": "'Subscription Id' must not be empty."
    }
  ]
}
```

### Delete Payment [Admin]

- **Method**: `DELETE`
- **URL**: `http://localhost:9875/api/PaymentTransaction/`
- **Status**: `405`
- **Response (real)**:

```json
{
  "raw": ""
}
```


## 21 - Integration

### Get All Integrations [Admin]

- **Method**: `GET`
- **URL**: `http://localhost:9875/api/Integration`
- **Status**: `200`
- **Response (real)**:

```json
[
  {
    "id": "dc686c44-83d8-446a-8347-0135911f99cf",
    "integrationId": "254652bc-89bb-4c86-bb1e-b3b32ec4378e",
    "platformName": "whatsapp",
    "status": "Active",
    "lastSyncDate": "2026-02-07T13:39:42.0212091",
    "businessId": "1",
    "businessName": ""
  }
]
```

### Get Integrations by Business [Owner/Admin]

- **Method**: `GET`
- **URL**: `http://localhost:9875/api/Integration/business/ac3fd098-3bd3-4f7f-9084-21e50c605974`
- **Status**: `200`
- **Response (real)**:

```json
[]
```

### Get Integration by ID [Owner/Admin]

- **Method**: `GET`
- **URL**: `http://localhost:9875/api/Integration/`
- **Status**: `200`
- **Response (real)**:

```json
[
  {
    "id": "dc686c44-83d8-446a-8347-0135911f99cf",
    "integrationId": "254652bc-89bb-4c86-bb1e-b3b32ec4378e",
    "platformName": "whatsapp",
    "status": "Active",
    "lastSyncDate": "2026-02-07T13:39:42.0212091",
    "businessId": "1",
    "businessName": ""
  }
]
```

### Connect Integration [Owner/Admin]

- **Method**: `POST`
- **URL**: `http://localhost:9875/api/Integration/connect`
- **Request body (from Postman)**:

```json
{
  "businessId": "ac3fd098-3bd3-4f7f-9084-21e50c605974",
  "platform": "WhatsApp",
  "apiKey": "your_api_key_here",
  "webhookUrl": "https://yourapp.com/webhook"
}
```
- **Status**: `400`
- **Response (real)**:

```json
{
  "message": "Validation Failed",
  "errors": [
    {
      "field": "PlatformName",
      "error": "The PlatformName field is required."
    },
    {
      "field": "ApiKeyOrConfig",
      "error": "The ApiKeyOrConfig field is required."
    }
  ]
}
```

### Sync Integration [Owner/Admin]

- **Method**: `POST`
- **URL**: `http://localhost:9875/api/Integration//sync`
- **Request body (from Postman)**:

```json
{
  "syncType": "Full"
}
```
- **Status**: `404`
- **Response (real)**:

```json
{
  "raw": ""
}
```

### Delete Integration [Owner/Admin]

- **Method**: `DELETE`
- **URL**: `http://localhost:9875/api/Integration/`
- **Status**: `405`
- **Response (real)**:

```json
{
  "raw": ""
}
```


## 22 - Setting

### Get Settings by Business [Owner/Admin]

- **Method**: `GET`
- **URL**: `http://localhost:9875/api/Setting/business/ac3fd098-3bd3-4f7f-9084-21e50c605974`
- **Status**: `404`
- **Response (real)**:

```json
{
  "message": "Settings not found for business 'ac3fd098-3bd3-4f7f-9084-21e50c605974'."
}
```

### Update Settings [Owner/Admin]

- **Method**: `PUT`
- **URL**: `http://localhost:9875/api/Setting/business/ac3fd098-3bd3-4f7f-9084-21e50c605974`
- **Request body (from Postman)**:

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
- **Status**: `400`
- **Response (real)**:

```json
{
  "message": "Business with id 'ac3fd098-3bd3-4f7f-9084-21e50c605974' not found."
}
```


## 23 - Audit Log [Read Only]

### Get All Audit Logs [Admin]

- **Method**: `GET`
- **URL**: `http://localhost:9875/api/AuditLog`
- **Status**: `200`
- **Response (real)**:

```json
[
  {
    "auditLogId": "10a01f05-12ad-4acb-bb79-5e3dca11941b",
    "businessId": "4fbf0918-af0c-4d4c-bfcf-df7e0ba29071",
    "businessName": "",
    "action": "CreateBusiness",
    "entity": "Business",
    "entityId": "4fbf0918-af0c-4d4c-bfcf-df7e0ba29071",
    "createdAt": "2026-04-14T14:39:25.5791524",
    "userId": "1bd04371-511e-422b-b8af-cb4d05c67314",
    "userName": ""
  },
  {
    "auditLogId": "3a8acd7c-6fd3-475b-a167-e4e17be72a1c",
    "businessId": "701f9138-65af-4e2d-8a59-174e0948ce82",
    "businessName": "",
    "action": "CreateMenuCategory",
    "entity": "MenuCategory",
    "entityId": "d218dd89-cc3a-4664-9042-ab6b075b9515",
    "createdAt": "2026-04-22T12:11:29.8996613",
    "userId": "18b51258-30da-4251-a609-f42966a9e9ae",
    "userName": ""
  },
  {
    "auditLogId": "70df73f7-fcb2-4202-9aaa-b6fa9fc1fcc5",
    "businessId": "701f9138-65af-4e2d-8a59-174e0948ce82",
    "businessName": "",
    "action": "UpdateSettings",
    "entity": "Settings",
    "entityId": "701f9138-65af-4e2d-8a59-174e0948ce82",
    "createdAt": "2026-04-22T12:11:36.359612",
    "userId": "18b51258-30da-4251-a609-f42966a9e9ae",
    "userName": ""
  },
  {
    "auditLogId": "973a8b5f-b13e-47e7-9958-2e0f742bd8d4",
    "businessId": "701f9138-65af-4e2d-8a59-174e0948ce82",
    "businessName": "",
    "action": "CreateBusiness",
    "entity": "Business",
    "entityId": "701f9138-65af-4e2d-8a59-174e0948ce82",
    "createdAt": "2026-04-22T12:07:25.7061454",
    "userId": "18b51258-30da-4251-a609-f42966a9e9ae",
    "userName": ""
  },
  {
    "auditLogId": "9f38de01-7260-4964-889b-771a96ff8bd2",
    "businessId": "701f9138-65af-4e2d-8a59-174e0948ce82",
    "businessName": "",
    "action": "CreateHumanEmployee",
    "entity": "User",
    "entityId": "ad1916c3-33b0-4b3a-a6d3-95a0c577f5d6",
    "createdAt": "2026-04-22T12:11:27.6374777",
    "userId": "18b51258-30da-4251-a609-f42966a9e9ae",
    "userName": ""
  },
  {
    "auditLogId": "c601e02e-9a23-4978-88fd-a76aceb3d40d",
    "businessId": "701f9138-65af-4e2d-8a59-174e0948ce82",
    "businessName": "",
    "action": "DeleteMenuCategory",
    "entity": "MenuCategory",
    "entityId": "d218dd89-cc3a-4664-9042-ab6b075b9515",
    "createdAt": "2026-04-22T12:11:30.1117476",
    "userId": "18b51258-30da-4251-a609-f42966a9e9ae",
    "userName": ""
  },
  {
    "auditLogId": "c72cc009-2b3c-4fa9-ad96-e8b77326de50",
    "businessId": "48866c67-0fac-47c1-8acf-10f708cca0b0",
    "businessName": "",
    "action": "CreateBusiness",
    "entity": "Business",
    "entityId": "48866c67-0fac-47c1-8acf-10f708cca0b0",
    "createdAt": "2026-04-22T12:21:09.8946799",
    "userId": "0620eb3e-2b1f-4c87-8ce4-372281ce7cd8",
    "userName": ""
  },
  {
    "auditLogId": "d24cccf0-2abe-4947-9e3b-dda92fa57b56",
    "businessId": "701f9138-65af-4e2d-8a59-174e0948ce82",
    "businessName": "",
    "action": "UpdateMenuCategory",
    "entity": "MenuCategory",
    "entityId": "d218dd89-cc3a-4664-9042-ab6b075b9515",
    "createdAt": "2026-04-22T12:11:29.9776973",
    "userId": "18b51258-30da-4251-a609-f42966a9e9ae",
    "userName": ""
  }
]
```

### Get Audit Logs by Business [Owner/Admin]

- **Method**: `GET`
- **URL**: `http://localhost:9875/api/AuditLog/business/ac3fd098-3bd3-4f7f-9084-21e50c605974`
- **Status**: `200`
- **Response (real)**:

```json
[]
```

### Get Audit Logs by User [Admin]

- **Method**: `GET`
- **URL**: `http://localhost:9875/api/AuditLog/user/`
- **Status**: `404`
- **Response (real)**:

```json
{
  "message": "AuditLog with id 'user' not found."
}
```

### Get Audit Log by ID [Admin]

- **Method**: `GET`
- **URL**: `http://localhost:9875/api/AuditLog/`
- **Status**: `200`
- **Response (real)**:

```json
[
  {
    "auditLogId": "10a01f05-12ad-4acb-bb79-5e3dca11941b",
    "businessId": "4fbf0918-af0c-4d4c-bfcf-df7e0ba29071",
    "businessName": "",
    "action": "CreateBusiness",
    "entity": "Business",
    "entityId": "4fbf0918-af0c-4d4c-bfcf-df7e0ba29071",
    "createdAt": "2026-04-14T14:39:25.5791524",
    "userId": "1bd04371-511e-422b-b8af-cb4d05c67314",
    "userName": ""
  },
  {
    "auditLogId": "3a8acd7c-6fd3-475b-a167-e4e17be72a1c",
    "businessId": "701f9138-65af-4e2d-8a59-174e0948ce82",
    "businessName": "",
    "action": "CreateMenuCategory",
    "entity": "MenuCategory",
    "entityId": "d218dd89-cc3a-4664-9042-ab6b075b9515",
    "createdAt": "2026-04-22T12:11:29.8996613",
    "userId": "18b51258-30da-4251-a609-f42966a9e9ae",
    "userName": ""
  },
  {
    "auditLogId": "70df73f7-fcb2-4202-9aaa-b6fa9fc1fcc5",
    "businessId": "701f9138-65af-4e2d-8a59-174e0948ce82",
    "businessName": "",
    "action": "UpdateSettings",
    "entity": "Settings",
    "entityId": "701f9138-65af-4e2d-8a59-174e0948ce82",
    "createdAt": "2026-04-22T12:11:36.359612",
    "userId": "18b51258-30da-4251-a609-f42966a9e9ae",
    "userName": ""
  },
  {
    "auditLogId": "973a8b5f-b13e-47e7-9958-2e0f742bd8d4",
    "businessId": "701f9138-65af-4e2d-8a59-174e0948ce82",
    "businessName": "",
    "action": "CreateBusiness",
    "entity": "Business",
    "entityId": "701f9138-65af-4e2d-8a59-174e0948ce82",
    "createdAt": "2026-04-22T12:07:25.7061454",
    "userId": "18b51258-30da-4251-a609-f42966a9e9ae",
    "userName": ""
  },
  {
    "auditLogId": "9f38de01-7260-4964-889b-771a96ff8bd2",
    "businessId": "701f9138-65af-4e2d-8a59-174e0948ce82",
    "businessName": "",
    "action": "CreateHumanEmployee",
    "entity": "User",
    "entityId": "ad1916c3-33b0-4b3a-a6d3-95a0c577f5d6",
    "createdAt": "2026-04-22T12:11:27.6374777",
    "userId": "18b51258-30da-4251-a609-f42966a9e9ae",
    "userName": ""
  },
  {
    "auditLogId": "c601e02e-9a23-4978-88fd-a76aceb3d40d",
    "businessId": "701f9138-65af-4e2d-8a59-174e0948ce82",
    "businessName": "",
    "action": "DeleteMenuCategory",
    "entity": "MenuCategory",
    "entityId": "d218dd89-cc3a-4664-9042-ab6b075b9515",
    "createdAt": "2026-04-22T12:11:30.1117476",
    "userId": "18b51258-30da-4251-a609-f42966a9e9ae",
    "userName": ""
  },
  {
    "auditLogId": "c72cc009-2b3c-4fa9-ad96-e8b77326de50",
    "businessId": "48866c67-0fac-47c1-8acf-10f708cca0b0",
    "businessName": "",
    "action": "CreateBusiness",
    "entity": "Business",
    "entityId": "48866c67-0fac-47c1-8acf-10f708cca0b0",
    "createdAt": "2026-04-22T12:21:09.8946799",
    "userId": "0620eb3e-2b1f-4c87-8ce4-372281ce7cd8",
    "userName": ""
  },
  {
    "auditLogId": "d24cccf0-2abe-4947-9e3b-dda92fa57b56",
    "businessId": "701f9138-65af-4e2d-8a59-174e0948ce82",
    "businessName": "",
    "action": "UpdateMenuCategory",
    "entity": "MenuCategory",
    "entityId": "d218dd89-cc3a-4664-9042-ab6b075b9515",
    "createdAt": "2026-04-22T12:11:29.9776973",
    "userId": "18b51258-30da-4251-a609-f42966a9e9ae",
    "userName": ""
  }
]
```


## 24 - Chatbot [AI]

### Ask Question [Owner/Admin]

- **Method**: `POST`
- **URL**: `http://localhost:9875/api/Chatbot/ask`
- **Request body (from Postman)**:

```json
{
  "question": "What is my business performance overview?",
  "conversationId": null
}
```
- **Status**: `200`
- **Response (real)**:

```json
{
  "answer": "📈 **Performance Summary for The Italian Place:**\n\r\n**Business Overview:**\r\n• Type: Restaurant\r\n• Total Revenue: $0.00\r\n• Total Orders: 0\r\n• Total Customers: 0\n\r\n**Customer Satisfaction:**\r\n• Average Rating: 0.0/5.0\r\n• Positive Sentiment Rate: 0.0%\n\r\n**Support Performance:**\r\n• Total Tickets: 0\r\n• Resolution Rate: 0.0%\n\r\n**Recent Activity:**\r\n",
  "conversationId": "144b20c0-1eec-4b5a-b658-cffa760752b9",
  "suggestions": [
    "What is my business performance overview?",
    "What are recommendations to improve sales?",
    "What is my customer sentiment analysis?",
    "How can I improve customer satisfaction?"
  ]
}
```

### Get Suggestions [Owner/Admin]

- **Method**: `GET`
- **URL**: `http://localhost:9875/api/Chatbot/suggestions`
- **Status**: `200`
- **Response (real)**:

```json
{
  "suggestions": [
    "What is my business performance overview?",
    "What is my total revenue and sales?",
    "What is my customer satisfaction rate?",
    "What is my sentiment analysis?",
    "How many tickets do I have?",
    "What are recommendations to improve sales?",
    "How can I improve customer satisfaction?",
    "What is my average order value?",
    "How many new customers did I get?",
    "What is my ticket resolution time?"
  ]
}
```


## 25 - Customer Chat [Public]

### Get Business Capabilities

- **Method**: `GET`
- **URL**: `http://localhost:9875/api/CustomerChat/capabilities/ac3fd098-3bd3-4f7f-9084-21e50c605974`
- **Status**: `404`
- **Response (real)**:

```json
{
  "message": "Business with id 'ac3fd098-3bd3-4f7f-9084-21e50c605974' not found."
}
```

### Send Chat Message

- **Method**: `POST`
- **URL**: `http://localhost:9875/api/CustomerChat/message`
- **Request body (from Postman)**:

```json
{
  "businessId": "ac3fd098-3bd3-4f7f-9084-21e50c605974",
  "customerId": null,
  "message": "What are your opening hours?",
  "channel": "WebChat",
  "sessionId": "session-001"
}
```
- **Status**: `500`
- **Response (real)**:

```json
{
  "message": "Chat handling failed.",
  "error": "An error occurred while saving the entity changes. See the inner exception for details."
}
```

### Get Order Recommendations

- **Method**: `POST`
- **URL**: `http://localhost:9875/api/CustomerChat/recommendations`
- **Request body (from Postman)**:

```json
{
  "businessId": "ac3fd098-3bd3-4f7f-9084-21e50c605974",
  "mainMenuItemId": "",
  "customerId": null
}
```
- **Status**: `400`
- **Response (real)**:

```json
{
  "message": "MainMenuItemId is required."
}
```


## 26 - Customer Voice [Public]

### Initialize Voice Session

- **Method**: `POST`
- **URL**: `http://localhost:9875/api/CustomerVoice/session/initialize`
- **Request body (from Postman)**:

```json
{
  "businessId": "ac3fd098-3bd3-4f7f-9084-21e50c605974",
  "customerId": null,
  "callSessionId": "call-session-001"
}
```
- **Status**: `500`
- **Response (real)**:

```json
{
  "message": "Failed to initialize voice session.",
  "error": "An error occurred while saving the entity changes. See the inner exception for details."
}
```

### Send Voice Message

- **Method**: `POST`
- **URL**: `http://localhost:9875/api/CustomerVoice/message`
- **Request body (from Postman)**:

```json
{
  "businessId": "ac3fd098-3bd3-4f7f-9084-21e50c605974",
  "customerId": null,
  "message": "I want to place an order",
  "audioData": null,
  "channel": "Voice",
  "sessionId": "call-session-001"
}
```
- **Status**: `500`
- **Response (real)**:

```json
{
  "message": "Voice handling failed.",
  "error": "An error occurred while saving the entity changes. See the inner exception for details."
}
```

### Mark Interaction Interrupted

- **Method**: `POST`
- **URL**: `http://localhost:9875/api/CustomerVoice/interaction//interrupt`
- **Status**: `404`
- **Response (real)**:

```json
{
  "raw": ""
}
```

### Submit Voice Feedback

- **Method**: `POST`
- **URL**: `http://localhost:9875/api/CustomerVoice/feedback`
- **Request body (from Postman)**:

```json
{
  "interactionId": "",
  "rating": 5,
  "comment": "Very helpful voice assistant!"
}
```
- **Status**: `400`
- **Response (real)**:

```json
{
  "message": "InteractionId is required."
}
```

### Get Voice Settings

- **Method**: `GET`
- **URL**: `http://localhost:9875/api/CustomerVoice/settings/ac3fd098-3bd3-4f7f-9084-21e50c605974`
- **Status**: `404`
- **Response (real)**:

```json
{
  "message": "Voice settings not found for this business."
}
```

