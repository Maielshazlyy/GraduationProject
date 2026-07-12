## Frontend API Reference (ALL endpoints, REAL responses)

- **Source collection**: `DigitalEmployee_Postman_Collection.json`
- **Base URL used**: `http://localhost:9875`
- **Note**: This document includes **all endpoints** (even if non-200) captured from the Postman collection.

## 01 - Auth

### Register Admin [PUBLIC]

- **Method**: `POST`
- **URL**: `http://localhost:9875/api/Auth/register-admin`
- **Request body (from Postman)**:

```json
{
  "fullName": "Super Admin",
  "email": "admin+20260425165411@app.com",
  "password": "Admin@123"
}
```
- **Status**: `200`
- **Response (real)**:

```json
{
  "userId": "0eaa5647-8e13-448a-84c3-59a0eacaca88",
  "email": "admin+20260425165411@app.com",
  "fullName": "Super Admin",
  "role": "Admin",
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6IjBlYWE1NjQ3LThlMTMtNDQ4YS04NGMzLTU5YTBlYWNhY2E4OCIsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL2VtYWlsYWRkcmVzcyI6ImFkbWluKzIwMjYwNDI1MTY1NDExQGFwcC5jb20iLCJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoiU3VwZXIgQWRtaW4iLCJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3JvbGUiOiJBZG1pbiIsImV4cCI6MTc3NzM5NTI1MSwiaXNzIjoiaHR0cDovL2xvY2FsaG9zdDo1MTU3IiwiYXVkIjoiaHR0cDovL2xvY2FsaG9zdDo1MTU3In0.KBinePrU1T4qeszwylVskVK4NfpdXp_hXPK2PAiwi3E",
  "expiration": "2026-04-28T16:54:11Z",
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
  "email": "owner+20260425165411@app.com",
  "password": "Owner@123"
}
```
- **Status**: `200`
- **Response (real)**:

```json
{
  "userId": "f95b7498-84b2-42da-a193-4508b2d13a1f",
  "email": "owner+20260425165411@app.com",
  "fullName": "Business Owner",
  "role": "Owner",
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6ImY5NWI3NDk4LTg0YjItNDJkYS1hMTkzLTQ1MDhiMmQxM2ExZiIsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL2VtYWlsYWRkcmVzcyI6Im93bmVyKzIwMjYwNDI1MTY1NDExQGFwcC5jb20iLCJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoiQnVzaW5lc3MgT3duZXIiLCJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3JvbGUiOiJPd25lciIsImV4cCI6MTc3NzM5NTI1MSwiaXNzIjoiaHR0cDovL2xvY2FsaG9zdDo1MTU3IiwiYXVkIjoiaHR0cDovL2xvY2FsaG9zdDo1MTU3In0.E6-OVVmk1eSiYhwx-267--7rof3w2R7mmS6PMIvMk6U",
  "expiration": "2026-04-28T16:54:11Z",
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
  "email": "agent+20260425165411@app.com",
  "password": "Agent@123",
  "businessId": "{{businessId}}"
}
```
- **Status**: `400`
- **Response (real)**:

```json
{
  "message": "Business with id '{{businessId}}' not found."
}
```

### Login

- **Method**: `POST`
- **URL**: `http://localhost:9875/api/Auth/login`
- **Request body (from Postman)**:

```json
{
  "email": "admin+20260425165411@app.com",
  "password": "Admin@123"
}
```
- **Status**: `200`
- **Response (real)**:

```json
{
  "userId": "0eaa5647-8e13-448a-84c3-59a0eacaca88",
  "email": "admin+20260425165411@app.com",
  "fullName": "Super Admin",
  "role": "Admin",
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6IjBlYWE1NjQ3LThlMTMtNDQ4YS04NGMzLTU5YTBlYWNhY2E4OCIsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL2VtYWlsYWRkcmVzcyI6ImFkbWluKzIwMjYwNDI1MTY1NDExQGFwcC5jb20iLCJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoiU3VwZXIgQWRtaW4iLCJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3JvbGUiOiJBZG1pbiIsImV4cCI6MTc3NzM5NTI1MSwiaXNzIjoiaHR0cDovL2xvY2FsaG9zdDo1MTU3IiwiYXVkIjoiaHR0cDovL2xvY2FsaG9zdDo1MTU3In0.KBinePrU1T4qeszwylVskVK4NfpdXp_hXPK2PAiwi3E",
  "expiration": "2026-04-28T16:54:11Z",
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
"{{userId}}"
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
"{{userId}}"
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
    "id": "079018da-a455-476d-9124-fa5558b265f4",
    "businessId": "7ccf2912-baad-496c-a757-8617931ae4ca",
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
    "createdAt": "2026-04-22T12:29:43.0477727",
    "totalUsers": 0,
    "totalCustomers": 0,
    "totalTickets": 0
  },
  {
    "id": "0af8c860-0d41-470b-940c-0f54b0855230",
    "businessId": "2f39f572-c663-478c-99ae-3b60dc36c86b",
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
    "createdAt": "2026-04-22T12:31:05.4703934",
    "totalUsers": 0,
    "totalCustomers": 0,
    "totalTickets": 0
  },
  {
    "id": "0bc8178d-0ca1-45ce-8da0-e3db10f10288",
    "businessId": "c576530a-65b0-4228-b0db-e53bf70ed97a",
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
    "createdAt": "2026-04-22T12:48:31.7823769",
    "totalUsers": 0,
    "totalCustomers": 0,
    "totalTickets": 0
  },
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
    "id": "1aeed440-454b-4780-8fc6-c1fedb9f0113",
    "businessId": "fa8077e2-ca0f-4ed3-8515-c1d5b879ac94",
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
    "createdAt": "2026-04-22T12:23:06.9761527",
    "totalUsers": 0,
    "totalCustomers": 0,
    "totalTickets": 0
  },
  {
    "id": "3344be22-4931-404a-a4d1-750fc803df36",
    "businessId": "c265bbb1-1665-40e0-84f5-66d7b6592674",
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
    "createdAt": "2026-04-22T12:31:05.5655685",
    "totalUsers": 0,
    "totalCustomers": 0,
    "totalTickets": 0
  },
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
    "workingHours": [],
    "createdAt": "2026-04-22T12:21:09.7770676",
    "totalUsers": 0,
    "totalCustomers": 0,
    "totalTickets": 0
  },
  {
    "id": "4b741c5c-589e-4c48-885d-d878d75a2918",
    "businessId": "c67efff2-0d0b-4fcc-8c7d-6934692edbbd",
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
    "createdAt": "2026-04-22T12:34:08.6051357",
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
    "id": "76c32d17-76ba-458e-9687-df9dd75b96a4",
    "businessId": "e06ea4f3-5e85-4cd8-873a-2cdbd4868f9d",
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
    "createdAt": "2026-04-22T12:33:14.2537616",
    "totalUsers": 0,
    "totalCustomers": 0,
    "totalTickets": 0
  },
  {
    "id": "9ada2312-9d87-4118-a960-7e8fbfe448d0",
    "businessId": "b533fee8-8afd-410d-92ab-cd6cda843325",
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
    "createdAt": "2026-04-22T12:33:14.1815624",
    "totalUsers": 0,
    "totalCustomers": 0,
    "totalTickets": 0
  },
  {
    "id": "a92e7b8b-9237-4cc5-8479-52762ee86b54",
    "businessId": "d5fe3312-b7ed-4fb3-ad5d-dc54c8a38411",
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
    "createdAt": "2026-04-22T12:29:43.1285201",
    "totalUsers": 0,
    "totalCustomers": 0,
    "totalTickets": 0
  },
  {
    "id": "af905b25-6947-4594-b352-dac4c05ee18b",
    "businessId": "e0d01b5e-8b77-465a-954c-51ed5679551c",
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
    "createdAt": "2026-04-22T12:23:07.2249164",
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
    "id": "c5d75e93-eddc-4be1-8ecb-9d1734dbb7de",
    "businessId": "75228016-95fa-4a12-87c2-eac2e7b3325c",
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
    "createdAt": "2026-04-22T12:34:08.5276021",
    "totalUsers": 0,
    "totalCustomers": 0,
    "totalTickets": 0
  },
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
    "workingHours": [],
    "createdAt": "2026-04-22T12:21:10.0265942",
    "totalUsers": 0,
    "totalCustomers": 0,
    "totalTickets": 0
  },
  {
    "id": "ddf2c130-9507-4435-a47d-455c3b29daa2",
    "businessId": "8f5e4238-f517-4c3e-b3bd-2cc9b87c6cb2",
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
    "createdAt": "2026-04-22T12:32:43.2541937",
    "totalUsers": 0,
    "totalCustomers": 0,
    "totalTickets": 0
  },
  {
    "id": "e62fb486-1085-46b4-9a0d-39371903e8ed",
    "businessId": "1d1e973f-2dfe-4904-b06a-e86ebea69524",
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
    "createdAt": "2026-04-25T16:51:32.1227249",
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
  },
  {
    "id": "f6c9135e-c5c0-4d1f-ac13-ae1046706114",
    "businessId": "f71afd2d-5c9f-4f9c-8993-938b63fb60d3",
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
    "createdAt": "2026-04-25T16:53:23.2279952",
    "totalUsers": 0,
    "totalCustomers": 0,
    "totalTickets": 0
  },
  {
    "id": "fdb58ac3-df2f-4374-b4b5-41ed2a08da20",
    "businessId": "0e1b947d-b855-4a00-b4f7-7d98e40c8d07",
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
    "createdAt": "2026-04-22T12:32:43.3472076",
    "totalUsers": 0,
    "totalCustomers": 0,
    "totalTickets": 0
  }
]
```

### Get Business by ID [Owner/Admin]

- **Method**: `GET`
- **URL**: `http://localhost:9875/api/Business/079018da-a455-476d-9124-fa5558b265f4`
- **Status**: `200`
- **Response (real)**:

```json
{
  "id": "079018da-a455-476d-9124-fa5558b265f4",
  "businessId": "7ccf2912-baad-496c-a757-8617931ae4ca",
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
  "createdAt": "2026-04-22T12:29:43.0477727",
  "totalUsers": 0,
  "totalCustomers": 0,
  "totalTickets": 0
}
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
  "id": "d2462718-ab23-40ad-94fd-36d7e35d240c",
  "businessId": "b0346d8a-92fa-44e5-887f-8d633bb47c09",
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
      "workingHoursId": "7567daa1-a480-49ff-9f81-96a6df0cfac3",
      "dayOfWeek": 0,
      "dayName": "Sunday",
      "openTime": null,
      "closeTime": null,
      "isClosed": true
    },
    {
      "workingHoursId": "1df14bc8-f814-45d4-84ed-ab45cc7da5fb",
      "dayOfWeek": 1,
      "dayName": "Monday",
      "openTime": "09:00",
      "closeTime": "22:00",
      "isClosed": false
    },
    {
      "workingHoursId": "701a1b15-dec6-427e-ac34-1d20a0d54aaf",
      "dayOfWeek": 2,
      "dayName": "Tuesday",
      "openTime": "09:00",
      "closeTime": "22:00",
      "isClosed": false
    },
    {
      "workingHoursId": "d2e008e3-1095-41e0-bcc3-1f5a98c464da",
      "dayOfWeek": 3,
      "dayName": "Wednesday",
      "openTime": "09:00",
      "closeTime": "22:00",
      "isClosed": false
    },
    {
      "workingHoursId": "883e083e-2f79-4e28-a960-7b9fa34d1f9c",
      "dayOfWeek": 4,
      "dayName": "Thursday",
      "openTime": "09:00",
      "closeTime": "23:00",
      "isClosed": false
    },
    {
      "workingHoursId": "16bbb184-eb1f-40af-b983-0076bcef7bb0",
      "dayOfWeek": 5,
      "dayName": "Friday",
      "openTime": "10:00",
      "closeTime": "23:00",
      "isClosed": false
    },
    {
      "workingHoursId": "fd75b78d-b4c0-405d-93cf-e6dbe6e9bfd5",
      "dayOfWeek": 6,
      "dayName": "Saturday",
      "openTime": "10:00",
      "closeTime": "21:00",
      "isClosed": false
    }
  ],
  "createdAt": "2026-04-25T16:54:12.172914Z",
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
  "id": "02d647c9-b836-428f-9dff-2269eb117d73",
  "businessId": "6158aef9-15d9-4b03-b6ec-84c9af12a3e7",
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
      "workingHoursId": "24a66f9d-e8c6-4e2c-96f3-d7572fe1768f",
      "dayOfWeek": 0,
      "dayName": "Sunday",
      "openTime": null,
      "closeTime": null,
      "isClosed": true
    },
    {
      "workingHoursId": "7422e12c-e6a7-48d7-9328-df119d3e9759",
      "dayOfWeek": 1,
      "dayName": "Monday",
      "openTime": "09:00",
      "closeTime": "22:00",
      "isClosed": false
    },
    {
      "workingHoursId": "a1f59ea1-7c11-4f6f-92fc-55131317f6e2",
      "dayOfWeek": 2,
      "dayName": "Tuesday",
      "openTime": "09:00",
      "closeTime": "22:00",
      "isClosed": false
    },
    {
      "workingHoursId": "bc2d0803-56c3-45e6-ba00-b6d79d162a92",
      "dayOfWeek": 3,
      "dayName": "Wednesday",
      "openTime": "09:00",
      "closeTime": "22:00",
      "isClosed": false
    },
    {
      "workingHoursId": "5e7563da-c48f-49d3-a913-9fe85a3c7b3d",
      "dayOfWeek": 4,
      "dayName": "Thursday",
      "openTime": "09:00",
      "closeTime": "23:00",
      "isClosed": false
    },
    {
      "workingHoursId": "9bf92a44-2380-47dc-ba87-0f149a059b1f",
      "dayOfWeek": 5,
      "dayName": "Friday",
      "openTime": "10:00",
      "closeTime": "23:00",
      "isClosed": false
    },
    {
      "workingHoursId": "3425aa8a-3286-4f71-bed4-8259f137a93d",
      "dayOfWeek": 6,
      "dayName": "Saturday",
      "openTime": "10:00",
      "closeTime": "21:00",
      "isClosed": false
    }
  ],
  "createdAt": "2026-04-25T16:54:12.2699377Z",
  "totalUsers": 0,
  "totalCustomers": 0,
  "totalTickets": 0
}
```

### Update Business [Owner/Admin]

- **Method**: `PUT`
- **URL**: `http://localhost:9875/api/Business/02d647c9-b836-428f-9dff-2269eb117d73`
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
- **Status**: `200`
- **Response (real)**:

```json
{
  "id": "02d647c9-b836-428f-9dff-2269eb117d73",
  "businessId": "6158aef9-15d9-4b03-b6ec-84c9af12a3e7",
  "name": "The Italian Place Updated",
  "type": "Restaurant",
  "address": "456 New Street, Cairo",
  "phone": "+201012345678",
  "email": "info@italianplace.com",
  "website": "https://italianplace.com",
  "facebookUrl": null,
  "instagramUrl": null,
  "city": "Cairo",
  "country": "Egypt",
  "latitude": 30.0444,
  "longitude": 31.2357,
  "description": "Updated description",
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
  "createdAt": "2026-04-25T16:54:12.2699377",
  "totalUsers": 0,
  "totalCustomers": 0,
  "totalTickets": 0
}
```

### Delete Business [Admin]

- **Method**: `DELETE`
- **URL**: `http://localhost:9875/api/Business/02d647c9-b836-428f-9dff-2269eb117d73`
- **Status**: `500`
- **Response (real)**:

```json
{
  "raw": "Microsoft.EntityFrameworkCore.DbUpdateException: An error occurred while saving the entity changes. See the inner exception for details.\r\n ---> Microsoft.Data.SqlClient.SqlException (0x80131904): The INSERT statement conflicted with the FOREIGN KEY constraint \"FK_AuditLogs_Businesses_BusinessId\". The conflict occurred in database \"DigitalEmployeeDB\", table \"dbo.Businesses\", column 'Id'.\r\nThe statement has been terminated.\r\n   at System.Threading.Tasks.ContinuationResultTaskFromResultTask`2.InnerInvoke()\r\n   at System.Threading.ExecutionContext.RunInternal(ExecutionContext executionContext, ContextCallback callback, Object state)\r\n--- End of stack trace from previous location ---\r\n   at System.Threading.ExecutionContext.RunInternal(ExecutionContext executionContext, ContextCallback callback, Object state)\r\n   at System.Threading.Tasks.Task.ExecuteWithThreadLocal(Task& currentTaskSlot, Thread threadPoolThread)\r\n--- End of stack trace from previous location ---\r\n   at Microsoft.EntityFrameworkCore.Storage.RelationalCommand.ExecuteReaderAsync(RelationalCommandParameterObject parameterObject, CancellationToken cancellationToken)\r\n   at Microsoft.EntityFrameworkCore.Storage.RelationalCommand.ExecuteReaderAsync(RelationalCommandParameterObject parameterObject, CancellationToken cancellationToken)\r\n   at Microsoft.EntityFrameworkCore.Update.ReaderModificationCommandBatch.ExecuteAsync(IRelationalConnection connection, CancellationToken cancellationToken)\r\nClientConnectionId:245ff758-376b-41d3-b07c-4d2d0c325b2f\r\nError Number:547,State:0,Class:16\r\n   --- End of inner exception stack trace ---\r\n   at Microsoft.EntityFrameworkCore.Update.ReaderModificationCommandBatch.ExecuteAsync(IRelationalConnection connection, CancellationToken cancellationToken)\r\n   at Microsoft.EntityFrameworkCore.SqlServer.Update.Internal.SqlServerModificationCommandBatch.ExecuteAsync(IRelationalConnection connection, CancellationToken cancellationToken)\r\n   at Microsoft.EntityFrameworkCore.Update.Internal.BatchExecutor.ExecuteAsync(IEnumerable`1 commandBatches, IRelationalConnection connection, CancellationToken cancellationToken)\r\n   at Microsoft.EntityFrameworkCore.Update.Internal.BatchExecutor.ExecuteAsync(IEnumerable`1 commandBatches, IRelationalConnection connection, CancellationToken cancellationToken)\r\n   at Microsoft.EntityFrameworkCore.Update.Internal.BatchExecutor.ExecuteAsync(IEnumerable`1 commandBatches, IRelationalConnection connection, CancellationToken cancellationToken)\r\n   at Microsoft.EntityFrameworkCore.Storage.RelationalDatabase.SaveChangesAsync(IList`1 entries, CancellationToken cancellationToken)\r\n   at Microsoft.EntityFrameworkCore.ChangeTracking.Internal.StateManager.SaveChangesAsync(IList`1 entriesToSave, CancellationToken cancellationToken)\r\n   at Microsoft.EntityFrameworkCore.ChangeTracking.Internal.StateManager.SaveChangesAsync(StateManager stateManager, Boolean acceptAllChangesOnSuccess, CancellationToken cancellationToken)\r\n   at Microsoft.EntityFrameworkCore.SqlServer.Storage.Internal.SqlServerExecutionStrategy.ExecuteAsync[TState,TResult](TState state, Func`4 operation, Func`4 verifySucceeded, CancellationToken cancellationToken)\r\n   at Microsoft.EntityFrameworkCore.DbContext.SaveChangesAsync(Boolean acceptAllChangesOnSuccess, CancellationToken cancellationToken)\r\n   at Microsoft.EntityFrameworkCore.DbContext.SaveChangesAsync(Boolean acceptAllChangesOnSuccess, CancellationToken cancellationToken)\r\n   at DAL.UnitOfWork.UnitOfWork.CompleteAsync() in d:\\Users\\Shazly\\Desktop\\assignments\\grad-project\\DAL\\UnitOfWork\\UnitOfWork.cs:line 85\r\n   at Service_layer.Services.AuditLogService.CreateAsync(String businessId, String action, String entity, String entityId, String userId) in d:\\Users\\Shazly\\Desktop\\assignments\\grad-project\\Service layer\\Services\\AuditLogService.cs:line 53\r\n   at Service_layer.Services.AuditLogService.LogBusinessActionAsync(String businessId, String action, String userId) in d:\\Users\\Shazly\\Desktop\\assignments\\grad-project\\Service layer\\Services\\AuditLogService.cs:line 103\r\n   at digital_employee.Controllers.BusinessController.Delete(String id) in d:\\Users\\Shazly\\Desktop\\assignments\\grad-project\\digital employee\\Controllers\\BusinessController.cs:line 216\r\n   at Microsoft.AspNetCore.Mvc.Infrastructure.ActionMethodExecutor.TaskOfIActionResultExecutor.Execute(ActionContext actionContext, IActionResultTypeMapper mapper, ObjectMethodExecutor executor, Object controller, Object[] arguments)\r\n   at Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker.<InvokeActionMethodAsync>g__Awaited|12_0(ControllerActionInvoker invoker, ValueTask`1 actionResultValueTask)\r\n   at Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker.<InvokeNextActionFilterAsync>g__Awaited|10_0(ControllerActionInvoker invoker, Task lastTask, State next, Scope scope, Object state, Boolean isCompleted)\r\n   at Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker.Rethrow(ActionExecutedContextSealed context)\r\n   at Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker.Next(State& next, Scope& scope, Object& state, Boolean& isCompleted)\r\n   at Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker.<InvokeInnerFilterAsync>g__Awaited|13_0(ControllerActionInvoker invoker, Task lastTask, State next, Scope scope, Object state, Boolean isCompleted)\r\n   at Microsoft.AspNetCore.Mvc.Infrastructure.ResourceInvoker.<InvokeFilterPipelineAsync>g__Awaited|20_0(ResourceInvoker invoker, Task lastTask, State next, Scope scope, Object state, Boolean isCompleted)\r\n   at Microsoft.AspNetCore.Mvc.Infrastructure.ResourceInvoker.<InvokeAsync>g__Awaited|17_0(ResourceInvoker invoker, Task task, IDisposable scope)\r\n   at Microsoft.AspNetCore.Mvc.Infrastructure.ResourceInvoker.<InvokeAsync>g__Awaited|17_0(ResourceInvoker invoker, Task task, IDisposable scope)\r\n   at Microsoft.AspNetCore.Authorization.AuthorizationMiddleware.Invoke(HttpContext context)\r\n   at Microsoft.AspNetCore.Authentication.AuthenticationMiddleware.Invoke(HttpContext context)\r\n   at Swashbuckle.AspNetCore.SwaggerUI.SwaggerUIMiddleware.Invoke(HttpContext httpContext)\r\n   at Swashbuckle.AspNetCore.Swagger.SwaggerMiddleware.Invoke(HttpContext httpContext, ISwaggerProvider swaggerProvider)\r\n   at Microsoft.AspNetCore.Diagnostics.DeveloperExceptionPageMiddlewareImpl.Invoke(HttpContext context)\r\n\r\nHEADERS\r\n=======\r\nConnection: close\r\nHost: localhost:9875\r\nUser-Agent: Python-urllib/3.13\r\nAccept-Encoding: identity\r\nAuthorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6IjBlYWE1NjQ3LThlMTMtNDQ4YS04NGMzLTU5YTBlYWNhY2E4OCIsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL2VtYWlsYWRkcmVzcyI6ImFkbWluKzIwMjYwNDI1MTY1NDExQGFwcC5jb20iLCJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoiU3VwZXIgQWRtaW4iLCJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3JvbGUiOiJBZG1pbiIsImV4cCI6MTc3NzM5NTI1MSwiaXNzIjoiaHR0cDovL2xvY2FsaG9zdDo1MTU3IiwiYXVkIjoiaHR0cDovL2xvY2FsaG9zdDo1MTU3In0.KBinePrU1T4qeszwylVskVK4NfpdXp_hXPK2PAiwi3E\r\n"
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
    "userId": "0eaa5647-8e13-448a-84c3-59a0eacaca88",
    "fullName": "Super Admin",
    "email": "admin+20260425165411@app.com",
    "role": "Owner",
    "businessId": "d2462718-ab23-40ad-94fd-36d7e35d240c",
    "businessName": "",
    "totalHandledInteractions": 0,
    "totalAssignedTickets": 0,
    "createdAt": "2026-04-25T16:54:11.4498745"
  },
  {
    "userId": "1409b377-34d0-4d7c-823a-8b753b814111",
    "fullName": "Business Owner",
    "email": "owner+20260425165322@app.com",
    "role": "Owner",
    "businessId": null,
    "businessName": "",
    "totalHandledInteractions": 0,
    "totalAssignedTickets": 0,
    "createdAt": "2026-04-25T16:53:22.6110646"
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
    "userId": "2342c1bc-a8e7-4eb9-a14f-5db8c3242f53",
    "fullName": "Super Admin",
    "email": "admin+20260422123104@app.com",
    "role": "Owner",
    "businessId": "0af8c860-0d41-470b-940c-0f54b0855230",
    "businessName": "",
    "totalHandledInteractions": 0,
    "totalAssignedTickets": 0,
    "createdAt": "2026-04-22T12:31:04.776265"
  },
  {
    "userId": "2732d28d-a71a-4a50-afde-a0571e555c4d",
    "fullName": "Business Owner",
    "email": "owner+20260425165131@app.com",
    "role": "Owner",
    "businessId": null,
    "businessName": "",
    "totalHandledInteractions": 0,
    "totalAssignedTickets": 0,
    "createdAt": "2026-04-25T16:51:31.4846971"
  },
  {
    "userId": "2c44346a-0473-4fa3-9846-70032b6b0e88",
    "fullName": "Super Admin",
    "email": "admin+20260425165322@app.com",
    "role": "Owner",
    "businessId": "f6c9135e-c5c0-4d1f-ac13-ae1046706114",
    "businessName": "",
    "totalHandledInteractions": 0,
    "totalAssignedTickets": 0,
    "createdAt": "2026-04-25T16:53:22.4884809"
  },
  {
    "userId": "2f27efef-05b9-4a82-93f7-0d757d65c82d",
    "fullName": "Super Admin",
    "email": "admin+20260422124830@app.com",
    "role": "Owner",
    "businessId": "0bc8178d-0ca1-45ce-8da0-e3db10f10288",
    "businessName": "",
    "totalHandledInteractions": 0,
    "totalAssignedTickets": 0,
    "createdAt": "2026-04-22T12:48:31.008413"
  },
  {
    "userId": "34c53abc-ab5c-47a7-88d2-7579c8b09a7b",
    "fullName": "Business Owner",
    "email": "owner+20260422123313@app.com",
    "role": "Owner",
    "businessId": null,
    "businessName": "",
    "totalHandledInteractions": 0,
    "totalAssignedTickets": 0,
    "createdAt": "2026-04-22T12:33:13.6005495"
  },
  {
    "userId": "3befede1-14f0-49e2-aec7-4c3d1dd9c7e9",
    "fullName": "Business Owner",
    "email": "owner+20260422123104@app.com",
    "role": "Owner",
    "businessId": null,
    "businessName": "",
    "totalHandledInteractions": 0,
    "totalAssignedTickets": 0,
    "createdAt": "2026-04-22T12:31:04.8946343"
  },
  {
    "userId": "54223802-80f2-43c0-9ff7-1e15654f8010",
    "fullName": "Super Admin",
    "email": "admin+20260425165131@app.com",
    "role": "Owner",
    "businessId": "e62fb486-1085-46b4-9a0d-39371903e8ed",
    "businessName": "",
    "totalHandledInteractions": 0,
    "totalAssignedTickets": 0,
    "createdAt": "2026-04-25T16:51:31.3228185"
  },
  {
    "userId": "6216cb5b-d97e-4f40-9f02-53c352b190d0",
    "fullName": "Business Owner",
    "email": "owner+20260422122304@app.com",
    "role": "Owner",
    "businessId": null,
    "businessName": "",
    "totalHandledInteractions": 0,
    "totalAssignedTickets": 0,
    "createdAt": "2026-04-22T12:23:06.1422989"
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
    "userId": "6a5f576e-3aeb-426c-952f-b3e7eacea75b",
    "fullName": "Business Owner",
    "email": "owner+20260422122942@app.com",
    "role": "Owner",
    "businessId": null,
    "businessName": "",
    "totalHandledInteractions": 0,
    "totalAssignedTickets": 0,
    "createdAt": "2026-04-22T12:29:42.3946406"
  },
  {
    "userId": "7273e882-fada-468b-a37d-dc9a4b60e713",
    "fullName": "Business Owner",
    "email": "owner+20260422124830@app.com",
    "role": "Owner",
    "businessId": null,
    "businessName": "",
    "totalHandledInteractions": 0,
    "totalAssignedTickets": 0,
    "createdAt": "2026-04-22T12:48:31.1467897"
  },
  {
    "userId": "7482d102-c2c2-4ebf-96e4-ff5f72c90a0a",
    "fullName": "Super Admin",
    "email": "admin+20260422122304@app.com",
    "role": "Owner",
    "businessId": "1aeed440-454b-4780-8fc6-c1fedb9f0113",
    "businessName": "",
    "totalHandledInteractions": 0,
    "totalAssignedTickets": 0,
    "createdAt": "2026-04-22T12:23:05.787504"
  },
  {
    "userId": "8b5341fe-6e71-424c-abb5-90df860312df",
    "fullName": "Super Admin",
    "email": "admin+20260422123407@app.com",
    "role": "Owner",
    "businessId": "c5d75e93-eddc-4be1-8ecb-9d1734dbb7de",
    "businessName": "",
    "totalHandledInteractions": 0,
    "totalAssignedTickets": 0,
    "createdAt": "2026-04-22T12:34:07.8181727"
  },
  {
    "userId": "937e1a5c-d90d-4207-a063-9859d2538044",
    "fullName": "Super Admin",
    "email": "admin+20260422123313@app.com",
    "role": "Owner",
    "businessId": "9ada2312-9d87-4118-a960-7e8fbfe448d0",
    "businessName": "",
    "totalHandledInteractions": 0,
    "totalAssignedTickets": 0,
    "createdAt": "2026-04-22T12:33:13.4755902"
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
    "userId": "b59e4fdf-80eb-465c-8bde-7649b090b052",
    "fullName": "Business Owner",
    "email": "owner+20260422123242@app.com",
    "role": "Owner",
    "businessId": null,
    "businessName": "",
    "totalHandledInteractions": 0,
    "totalAssignedTickets": 0,
    "createdAt": "2026-04-22T12:32:42.6488217"
  },
  {
    "userId": "b71dff9b-0f2b-4b93-91cc-9e780333ef79",
    "fullName": "Super Admin",
    "email": "admin+20260422122942@app.com",
    "role": "Owner",
    "businessId": "079018da-a455-476d-9124-fa5558b265f4",
    "businessName": "",
    "totalHandledInteractions": 0,
    "totalAssignedTickets": 0,
    "createdAt": "2026-04-22T12:29:42.2672914"
  },
  {
    "userId": "b940fecc-1a1d-46c8-999e-b29d46c864dd",
    "fullName": "Super Admin",
    "email": "admin+20260422123242@app.com",
    "role": "Owner",
    "businessId": "ddf2c130-9507-4435-a47d-455c3b29daa2",
    "businessName": "",
    "totalHandledInteractions": 0,
    "totalAssignedTickets": 0,
    "createdAt": "2026-04-22T12:32:42.5165424"
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
  },
  {
    "userId": "f42c8e12-c982-476a-8c4e-cf5cbae206d9",
    "fullName": "Business Owner",
    "email": "owner+20260422123407@app.com",
    "role": "Owner",
    "businessId": null,
    "businessName": "",
    "totalHandledInteractions": 0,
    "totalAssignedTickets": 0,
    "createdAt": "2026-04-22T12:34:07.9400871"
  },
  {
    "userId": "f95b7498-84b2-42da-a193-4508b2d13a1f",
    "fullName": "Business Owner",
    "email": "owner+20260425165411@app.com",
    "role": "Owner",
    "businessId": null,
    "businessName": "",
    "totalHandledInteractions": 0,
    "totalAssignedTickets": 0,
    "createdAt": "2026-04-25T16:54:11.5717987"
  }
]
```

### Get Users by Business [Owner/Admin]

- **Method**: `GET`
- **URL**: `http://localhost:9875/api/User/business/02d647c9-b836-428f-9dff-2269eb117d73`
- **Status**: `200`
- **Response (real)**:

```json
[]
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
- **URL**: `http://localhost:9875/api/Customer/business/02d647c9-b836-428f-9dff-2269eb117d73`
- **Status**: `200`
- **Response (real)**:

```json
[]
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
  "email": "customer+20260425165411@app.com",
  "phone": "+201098765432",
  "businessId": "02d647c9-b836-428f-9dff-2269eb117d73"
}
```
- **Status**: `400`
- **Response (real)**:

```json
{
  "message": "Business with id '02d647c9-b836-428f-9dff-2269eb117d73' not found."
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


## 06 - Admin Dashboard

### Get Summary

- **Method**: `GET`
- **URL**: `http://localhost:9875/api/AdminDashboard/summary`
- **Status**: `200`
- **Response (real)**:

```json
{
  "totalBusinesses": 24,
  "activeBusinesses": 22,
  "newBusinessesLast30Days": 22,
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
  "totalAuditLogs": 18,
  "auditLogsLast24Hours": 3,
  "lastAuditLogDate": "2026-04-25T16:54:12.190245",
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
    "businessId": "079018da-a455-476d-9124-fa5558b265f4",
    "businessName": "The Italian Place",
    "isActive": true,
    "ordersCount": 0,
    "revenue": 0,
    "openTicketsCount": 0,
    "customersCount": 0
  },
  {
    "businessId": "0af8c860-0d41-470b-940c-0f54b0855230",
    "businessName": "The Italian Place",
    "isActive": true,
    "ordersCount": 0,
    "revenue": 0,
    "openTicketsCount": 0,
    "customersCount": 0
  },
  {
    "businessId": "0bc8178d-0ca1-45ce-8da0-e3db10f10288",
    "businessName": "The Italian Place",
    "isActive": true,
    "ordersCount": 0,
    "revenue": 0,
    "openTicketsCount": 0,
    "customersCount": 0
  },
  {
    "businessId": "1aeed440-454b-4780-8fc6-c1fedb9f0113",
    "businessName": "The Italian Place",
    "isActive": true,
    "ordersCount": 0,
    "revenue": 0,
    "openTicketsCount": 0,
    "customersCount": 0
  },
  {
    "businessId": "3344be22-4931-404a-a4d1-750fc803df36",
    "businessName": "The Italian Place",
    "isActive": true,
    "ordersCount": 0,
    "revenue": 0,
    "openTicketsCount": 0,
    "customersCount": 0
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
    "businessId": "4b741c5c-589e-4c48-885d-d878d75a2918",
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
    "totalBusinesses": 24,
    "activeBusinesses": 22,
    "newBusinessesLast30Days": 22,
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
    "totalAuditLogs": 18,
    "auditLogsLast24Hours": 3,
    "lastAuditLogDate": "2026-04-25T16:54:12.190245",
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
      "businessId": "079018da-a455-476d-9124-fa5558b265f4",
      "businessName": "The Italian Place",
      "isActive": true,
      "ordersCount": 0,
      "revenue": 0,
      "openTicketsCount": 0,
      "customersCount": 0
    },
    {
      "businessId": "0af8c860-0d41-470b-940c-0f54b0855230",
      "businessName": "The Italian Place",
      "isActive": true,
      "ordersCount": 0,
      "revenue": 0,
      "openTicketsCount": 0,
      "customersCount": 0
    },
    {
      "businessId": "0bc8178d-0ca1-45ce-8da0-e3db10f10288",
      "businessName": "The Italian Place",
      "isActive": true,
      "ordersCount": 0,
      "revenue": 0,
      "openTicketsCount": 0,
      "customersCount": 0
    },
    {
      "businessId": "1aeed440-454b-4780-8fc6-c1fedb9f0113",
      "businessName": "The Italian Place",
      "isActive": true,
      "ordersCount": 0,
      "revenue": 0,
      "openTicketsCount": 0,
      "customersCount": 0
    },
    {
      "businessId": "3344be22-4931-404a-a4d1-750fc803df36",
      "businessName": "The Italian Place",
      "isActive": true,
      "ordersCount": 0,
      "revenue": 0,
      "openTicketsCount": 0,
      "customersCount": 0
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
      "businessId": "4b741c5c-589e-4c48-885d-d878d75a2918",
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
    "id": "98d596be-9838-4fbe-80de-24003d55cdb7",
    "type": "InactiveBusiness",
    "severity": "Medium",
    "businessId": "079018da-a455-476d-9124-fa5558b265f4",
    "businessName": "The Italian Place",
    "message": "No orders recorded in the last 14 days.",
    "createdAt": "2026-04-25T16:54:13.4322219Z"
  },
  {
    "id": "d4b455bb-d035-4382-86e4-09dd198800fa",
    "type": "InactiveBusiness",
    "severity": "Medium",
    "businessId": "0af8c860-0d41-470b-940c-0f54b0855230",
    "businessName": "The Italian Place",
    "message": "No orders recorded in the last 14 days.",
    "createdAt": "2026-04-25T16:54:13.4322219Z"
  },
  {
    "id": "b106858a-81fb-45a8-ade9-eba8d6aaf594",
    "type": "InactiveBusiness",
    "severity": "Medium",
    "businessId": "0bc8178d-0ca1-45ce-8da0-e3db10f10288",
    "businessName": "The Italian Place",
    "message": "No orders recorded in the last 14 days.",
    "createdAt": "2026-04-25T16:54:13.4322219Z"
  },
  {
    "id": "4b8054c0-b676-4bc0-bd9a-90eeeb6c153f",
    "type": "InactiveBusiness",
    "severity": "Medium",
    "businessId": "1",
    "businessName": "Test Company",
    "message": "No orders recorded in the last 14 days.",
    "createdAt": "2026-04-25T16:54:13.4322219Z"
  },
  {
    "id": "27b27eb8-1f6a-4cfc-a9bb-c28c8ce6acfb",
    "type": "InactiveBusiness",
    "severity": "Medium",
    "businessId": "1aeed440-454b-4780-8fc6-c1fedb9f0113",
    "businessName": "The Italian Place",
    "message": "No orders recorded in the last 14 days.",
    "createdAt": "2026-04-25T16:54:13.4322219Z"
  },
  {
    "id": "df287916-6e87-42a2-9246-895d14115dee",
    "type": "InactiveBusiness",
    "severity": "Medium",
    "businessId": "3344be22-4931-404a-a4d1-750fc803df36",
    "businessName": "The Italian Place",
    "message": "No orders recorded in the last 14 days.",
    "createdAt": "2026-04-25T16:54:13.4322219Z"
  },
  {
    "id": "1ed64e35-b4b1-422d-a3c8-b4d48f25055d",
    "type": "InactiveBusiness",
    "severity": "Medium",
    "businessId": "48866c67-0fac-47c1-8acf-10f708cca0b0",
    "businessName": "The Italian Place",
    "message": "No orders recorded in the last 14 days.",
    "createdAt": "2026-04-25T16:54:13.4322219Z"
  },
  {
    "id": "fcfa0d5b-a909-4353-b054-9547854e3175",
    "type": "InactiveBusiness",
    "severity": "Medium",
    "businessId": "4b741c5c-589e-4c48-885d-d878d75a2918",
    "businessName": "The Italian Place",
    "message": "No orders recorded in the last 14 days.",
    "createdAt": "2026-04-25T16:54:13.4322219Z"
  },
  {
    "id": "f82dee5f-ebd1-4933-85ac-a2f82a069e34",
    "type": "InactiveBusiness",
    "severity": "Medium",
    "businessId": "4fbf0918-af0c-4d4c-bfcf-df7e0ba29071",
    "businessName": "The Italian Place",
    "message": "No orders recorded in the last 14 days.",
    "createdAt": "2026-04-25T16:54:13.4322219Z"
  },
  {
    "id": "f7fc61b6-a566-4509-b0fd-18dacbfbec8a",
    "type": "InactiveBusiness",
    "severity": "Medium",
    "businessId": "6b43667f-a08b-4be5-b2a6-3a782cd07769",
    "businessName": "The Italian Place",
    "message": "No orders recorded in the last 14 days.",
    "createdAt": "2026-04-25T16:54:13.4322219Z"
  },
  {
    "id": "aee669f7-810b-4436-816e-2aa1a348e8e1",
    "type": "InactiveBusiness",
    "severity": "Medium",
    "businessId": "701f9138-65af-4e2d-8a59-174e0948ce82",
    "businessName": "The Italian Place",
    "message": "No orders recorded in the last 14 days.",
    "createdAt": "2026-04-25T16:54:13.4322219Z"
  },
  {
    "id": "f34cd9d3-d76b-430a-b919-69e4c20351ad",
    "type": "InactiveBusiness",
    "severity": "Medium",
    "businessId": "76c32d17-76ba-458e-9687-df9dd75b96a4",
    "businessName": "The Italian Place",
    "message": "No orders recorded in the last 14 days.",
    "createdAt": "2026-04-25T16:54:13.4322219Z"
  },
  {
    "id": "e9bf5645-2469-4648-8af1-1d3c90e2f669",
    "type": "InactiveBusiness",
    "severity": "Medium",
    "businessId": "9ada2312-9d87-4118-a960-7e8fbfe448d0",
    "businessName": "The Italian Place",
    "message": "No orders recorded in the last 14 days.",
    "createdAt": "2026-04-25T16:54:13.4322219Z"
  },
  {
    "id": "8a81c4fb-8b36-4e4b-af61-137981041fec",
    "type": "InactiveBusiness",
    "severity": "Medium",
    "businessId": "a92e7b8b-9237-4cc5-8479-52762ee86b54",
    "businessName": "The Italian Place",
    "message": "No orders recorded in the last 14 days.",
    "createdAt": "2026-04-25T16:54:13.4322219Z"
  },
  {
    "id": "75f97907-c209-4c8e-a8c3-47feca8ae1f5",
    "type": "InactiveBusiness",
    "severity": "Medium",
    "businessId": "af905b25-6947-4594-b352-dac4c05ee18b",
    "businessName": "The Italian Place",
    "message": "No orders recorded in the last 14 days.",
    "createdAt": "2026-04-25T16:54:13.4322219Z"
  },
  {
    "id": "f03f9f3f-e51f-4596-b952-a59c0df5e83a",
    "type": "InactiveBusiness",
    "severity": "Medium",
    "businessId": "c14c1302-f710-4739-bd21-e4fb9c5d1ec0",
    "businessName": "b1",
    "message": "No orders recorded in the last 14 days.",
    "createdAt": "2026-04-25T16:54:13.4322219Z"
  },
  {
    "id": "91980fbe-d310-4a9c-a76c-0051091f68bc",
    "type": "InactiveBusiness",
    "severity": "Medium",
    "businessId": "c5d75e93-eddc-4be1-8ecb-9d1734dbb7de",
    "businessName": "The Italian Place",
    "message": "No orders recorded in the last 14 days.",
    "createdAt": "2026-04-25T16:54:13.4322219Z"
  },
  {
    "id": "c0184eb5-b4a4-45ee-b213-b9fc62801529",
    "type": "InactiveBusiness",
    "severity": "Medium",
    "businessId": "d2462718-ab23-40ad-94fd-36d7e35d240c",
    "businessName": "The Italian Place",
    "message": "No orders recorded in the last 14 days.",
    "createdAt": "2026-04-25T16:54:13.4322219Z"
  },
  {
    "id": "d5ff496a-9fd4-4c52-8ce4-8db0677d4f26",
    "type": "InactiveBusiness",
    "severity": "Medium",
    "businessId": "d8a4ccc9-a1ee-4302-964e-537c17119a77",
    "businessName": "The Italian Place",
    "message": "No orders recorded in the last 14 days.",
    "createdAt": "2026-04-25T16:54:13.4322219Z"
  },
  {
    "id": "62431150-2add-4e6c-9dec-b57dec1b5442",
    "type": "InactiveBusiness",
    "severity": "Medium",
    "businessId": "ddf2c130-9507-4435-a47d-455c3b29daa2",
    "businessName": "The Italian Place",
    "message": "No orders recorded in the last 14 days.",
    "createdAt": "2026-04-25T16:54:13.4322219Z"
  },
  {
    "id": "4dc93881-b8f0-463a-83a0-257be9b9698e",
    "type": "InactiveBusiness",
    "severity": "Medium",
    "businessId": "e62fb486-1085-46b4-9a0d-39371903e8ed",
    "businessName": "The Italian Place",
    "message": "No orders recorded in the last 14 days.",
    "createdAt": "2026-04-25T16:54:13.4322219Z"
  },
  {
    "id": "f82011bc-c0a9-4d9f-9bf9-3b64564da41e",
    "type": "InactiveBusiness",
    "severity": "Medium",
    "businessId": "e904934f-2b6b-4d71-ae27-2983d2e8e6f4",
    "businessName": "The Italian Place",
    "message": "No orders recorded in the last 14 days.",
    "createdAt": "2026-04-25T16:54:13.4322219Z"
  },
  {
    "id": "7eb81eb2-6a16-40e6-a4a2-53e8a55f46ac",
    "type": "InactiveBusiness",
    "severity": "Medium",
    "businessId": "f6c9135e-c5c0-4d1f-ac13-ae1046706114",
    "businessName": "The Italian Place",
    "message": "No orders recorded in the last 14 days.",
    "createdAt": "2026-04-25T16:54:13.4322219Z"
  },
  {
    "id": "aef78bac-a511-432a-ae2e-8e629707b381",
    "type": "InactiveBusiness",
    "severity": "Medium",
    "businessId": "fdb58ac3-df2f-4374-b4b5-41ed2a08da20",
    "businessName": "The Italian Place",
    "message": "No orders recorded in the last 14 days.",
    "createdAt": "2026-04-25T16:54:13.4322219Z"
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
    "businessId": "079018da-a455-476d-9124-fa5558b265f4",
    "businessName": "The Italian Place",
    "healthScore": 60,
    "averageRating": 0,
    "negativeSentimentRatio": 0,
    "cancellationRate": 0,
    "openTicketsCount": 0,
    "escalatedTicketsCount": 0
  },
  {
    "businessId": "0af8c860-0d41-470b-940c-0f54b0855230",
    "businessName": "The Italian Place",
    "healthScore": 60,
    "averageRating": 0,
    "negativeSentimentRatio": 0,
    "cancellationRate": 0,
    "openTicketsCount": 0,
    "escalatedTicketsCount": 0
  },
  {
    "businessId": "0bc8178d-0ca1-45ce-8da0-e3db10f10288",
    "businessName": "The Italian Place",
    "healthScore": 60,
    "averageRating": 0,
    "negativeSentimentRatio": 0,
    "cancellationRate": 0,
    "openTicketsCount": 0,
    "escalatedTicketsCount": 0
  },
  {
    "businessId": "1aeed440-454b-4780-8fc6-c1fedb9f0113",
    "businessName": "The Italian Place",
    "healthScore": 60,
    "averageRating": 0,
    "negativeSentimentRatio": 0,
    "cancellationRate": 0,
    "openTicketsCount": 0,
    "escalatedTicketsCount": 0
  },
  {
    "businessId": "3344be22-4931-404a-a4d1-750fc803df36",
    "businessName": "The Italian Place",
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
    "businessId": "4b741c5c-589e-4c48-885d-d878d75a2918",
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
    "businessId": "76c32d17-76ba-458e-9687-df9dd75b96a4",
    "businessName": "The Italian Place",
    "healthScore": 60,
    "averageRating": 0,
    "negativeSentimentRatio": 0,
    "cancellationRate": 0,
    "openTicketsCount": 0,
    "escalatedTicketsCount": 0
  },
  {
    "businessId": "9ada2312-9d87-4118-a960-7e8fbfe448d0",
    "businessName": "The Italian Place",
    "healthScore": 60,
    "averageRating": 0,
    "negativeSentimentRatio": 0,
    "cancellationRate": 0,
    "openTicketsCount": 0,
    "escalatedTicketsCount": 0
  },
  {
    "businessId": "a92e7b8b-9237-4cc5-8479-52762ee86b54",
    "businessName": "The Italian Place",
    "healthScore": 60,
    "averageRating": 0,
    "negativeSentimentRatio": 0,
    "cancellationRate": 0,
    "openTicketsCount": 0,
    "escalatedTicketsCount": 0
  },
  {
    "businessId": "af905b25-6947-4594-b352-dac4c05ee18b",
    "businessName": "The Italian Place",
    "healthScore": 60,
    "averageRating": 0,
    "negativeSentimentRatio": 0,
    "cancellationRate": 0,
    "openTicketsCount": 0,
    "escalatedTicketsCount": 0
  },
  {
    "businessId": "c5d75e93-eddc-4be1-8ecb-9d1734dbb7de",
    "businessName": "The Italian Place",
    "healthScore": 60,
    "averageRating": 0,
    "negativeSentimentRatio": 0,
    "cancellationRate": 0,
    "openTicketsCount": 0,
    "escalatedTicketsCount": 0
  },
  {
    "businessId": "d2462718-ab23-40ad-94fd-36d7e35d240c",
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
    "businessId": "ddf2c130-9507-4435-a47d-455c3b29daa2",
    "businessName": "The Italian Place",
    "healthScore": 60,
    "averageRating": 0,
    "negativeSentimentRatio": 0,
    "cancellationRate": 0,
    "openTicketsCount": 0,
    "escalatedTicketsCount": 0
  },
  {
    "businessId": "e62fb486-1085-46b4-9a0d-39371903e8ed",
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
  },
  {
    "date": "2026-04-23",
    "positive": 0,
    "negative": 0,
    "neutral": 0
  },
  {
    "date": "2026-04-24",
    "positive": 0,
    "negative": 0,
    "neutral": 0
  },
  {
    "date": "2026-04-25",
    "positive": 0,
    "negative": 0,
    "neutral": 0
  }
]
```

### Suspend Business

- **Method**: `POST`
- **URL**: `http://localhost:9875/api/AdminDashboard/business/02d647c9-b836-428f-9dff-2269eb117d73/suspend`
- **Status**: `404`
- **Response (real)**:

```json
{
  "message": "Business with id '02d647c9-b836-428f-9dff-2269eb117d73' not found."
}
```

### Activate Business

- **Method**: `POST`
- **URL**: `http://localhost:9875/api/AdminDashboard/business/02d647c9-b836-428f-9dff-2269eb117d73/activate`
- **Status**: `404`
- **Response (real)**:

```json
{
  "message": "Business with id '02d647c9-b836-428f-9dff-2269eb117d73' not found."
}
```

### Verify Business

- **Method**: `POST`
- **URL**: `http://localhost:9875/api/AdminDashboard/business/02d647c9-b836-428f-9dff-2269eb117d73/verify`
- **Status**: `404`
- **Response (real)**:

```json
{
  "message": "Business with id '02d647c9-b836-428f-9dff-2269eb117d73' not found."
}
```

### Unverify Business

- **Method**: `POST`
- **URL**: `http://localhost:9875/api/AdminDashboard/business/02d647c9-b836-428f-9dff-2269eb117d73/unverify`
- **Status**: `404`
- **Response (real)**:

```json
{
  "message": "Business with id '02d647c9-b836-428f-9dff-2269eb117d73' not found."
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
    "menuCategoryId": "07766ff0-ec21-45d0-8000-a454f68eaddf",
    "name": "Main Dishes",
    "description": "Signature plates",
    "displayOrder": 1,
    "isActive": true,
    "createdAt": "2026-04-22T12:29:43.1287318",
    "businessId": "a92e7b8b-9237-4cc5-8479-52762ee86b54",
    "businessName": "",
    "menuItemsCount": 0
  },
  {
    "menuCategoryId": "13da09d4-3bcc-40a4-805b-c15167ed49a2",
    "name": "Main Dishes",
    "description": "Signature plates",
    "displayOrder": 1,
    "isActive": true,
    "createdAt": "2026-04-22T12:33:14.253898",
    "businessId": "76c32d17-76ba-458e-9687-df9dd75b96a4",
    "businessName": "",
    "menuItemsCount": 0
  },
  {
    "menuCategoryId": "57da9321-44d3-4107-9601-7cdd3957c4f8",
    "name": "Main Dishes",
    "description": "Signature plates",
    "displayOrder": 1,
    "isActive": true,
    "createdAt": "2026-04-22T12:31:05.5657408",
    "businessId": "3344be22-4931-404a-a4d1-750fc803df36",
    "businessName": "",
    "menuItemsCount": 0
  },
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
    "menuCategoryId": "8d22997f-aa08-4ca2-9e95-9d6735a17cae",
    "name": "Main Dishes",
    "description": "Signature plates",
    "displayOrder": 1,
    "isActive": true,
    "createdAt": "2026-04-22T12:34:08.6053071",
    "businessId": "4b741c5c-589e-4c48-885d-d878d75a2918",
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
    "menuCategoryId": "b5ed7b8f-f4e9-40b3-b28e-06a72b751560",
    "name": "Main Dishes",
    "description": "Signature plates",
    "displayOrder": 1,
    "isActive": true,
    "createdAt": "2026-04-22T12:23:07.225926",
    "businessId": "af905b25-6947-4594-b352-dac4c05ee18b",
    "businessName": "",
    "menuItemsCount": 0
  },
  {
    "menuCategoryId": "c0df722e-5151-4b10-9b95-bd0685858c4c",
    "name": "Main Dishes",
    "description": "Signature plates",
    "displayOrder": 1,
    "isActive": true,
    "createdAt": "2026-04-22T12:32:43.3473413",
    "businessId": "fdb58ac3-df2f-4374-b4b5-41ed2a08da20",
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
- **URL**: `http://localhost:9875/api/MenuCategory/business/02d647c9-b836-428f-9dff-2269eb117d73`
- **Status**: `200`
- **Response (real)**:

```json
[]
```

### Get Active Categories [Public]

- **Method**: `GET`
- **URL**: `http://localhost:9875/api/MenuCategory/business/02d647c9-b836-428f-9dff-2269eb117d73/active`
- **Status**: `200`
- **Response (real)**:

```json
[]
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
  "businessId": "02d647c9-b836-428f-9dff-2269eb117d73"
}
```
- **Status**: `400`
- **Response (real)**:

```json
{
  "message": "Business with id '02d647c9-b836-428f-9dff-2269eb117d73' not found."
}
```

### Reorder Categories [Owner/Admin]

- **Method**: `PUT`
- **URL**: `http://localhost:9875/api/MenuCategory/business/02d647c9-b836-428f-9dff-2269eb117d73/reorder`
- **Request body (from Postman)**:

```json
{
  "categoryOrders": [
    { "menuCategoryId": "{{menuCategoryId}}", "displayOrder": 1 }
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


## 08 - Menu Item

### Get All Menu Items

- **Method**: `GET`
- **URL**: `http://localhost:9875/api/MenuItem`
- **Status**: `200`
- **Response (real)**:

```json
[
  {
    "menuItemId": "008af2f4-53c1-4401-9604-bed9629f952f",
    "name": "Margherita Pizza",
    "description": "Tomato, mozzarella, fresh basil",
    "price": 89.99,
    "menuCategoryId": "57da9321-44d3-4107-9601-7cdd3957c4f8",
    "menuCategoryName": null,
    "isAvailable": true,
    "businessId": "3344be22-4931-404a-a4d1-750fc803df36",
    "businessName": ""
  },
  {
    "menuItemId": "17f836f9-7868-49f6-8c58-558e50a1e766",
    "name": "Margherita Pizza",
    "description": "Tomato, mozzarella, fresh basil",
    "price": 89.99,
    "menuCategoryId": "07766ff0-ec21-45d0-8000-a454f68eaddf",
    "menuCategoryName": null,
    "isAvailable": true,
    "businessId": "a92e7b8b-9237-4cc5-8479-52762ee86b54",
    "businessName": ""
  },
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
    "menuItemId": "5eb81a4f-def3-46f6-b632-ab55f0b83c77",
    "name": "Margherita Pizza",
    "description": "Tomato, mozzarella, fresh basil",
    "price": 89.99,
    "menuCategoryId": "13da09d4-3bcc-40a4-805b-c15167ed49a2",
    "menuCategoryName": null,
    "isAvailable": true,
    "businessId": "76c32d17-76ba-458e-9687-df9dd75b96a4",
    "businessName": ""
  },
  {
    "menuItemId": "753a7a19-edc7-44e3-9047-91ad82b0f948",
    "name": "Margherita Pizza",
    "description": "Tomato, mozzarella, fresh basil",
    "price": 89.99,
    "menuCategoryId": "c0df722e-5151-4b10-9b95-bd0685858c4c",
    "menuCategoryName": null,
    "isAvailable": true,
    "businessId": "fdb58ac3-df2f-4374-b4b5-41ed2a08da20",
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
    "menuItemId": "ad22743a-b0c9-4e72-8cd8-a4fa04b4dcd0",
    "name": "Margherita Pizza",
    "description": "Tomato, mozzarella, fresh basil",
    "price": 89.99,
    "menuCategoryId": "8d22997f-aa08-4ca2-9e95-9d6735a17cae",
    "menuCategoryName": null,
    "isAvailable": true,
    "businessId": "4b741c5c-589e-4c48-885d-d878d75a2918",
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
  },
  {
    "menuItemId": "e87e82c6-a9d9-471b-ab31-8e059a5bac25",
    "name": "Margherita Pizza",
    "description": "Tomato, mozzarella, fresh basil",
    "price": 89.99,
    "menuCategoryId": "b5ed7b8f-f4e9-40b3-b28e-06a72b751560",
    "menuCategoryName": null,
    "isAvailable": true,
    "businessId": "af905b25-6947-4594-b352-dac4c05ee18b",
    "businessName": ""
  }
]
```

### Get Menu Items by Business

- **Method**: `GET`
- **URL**: `http://localhost:9875/api/MenuItem/business/02d647c9-b836-428f-9dff-2269eb117d73`
- **Status**: `200`
- **Response (real)**:

```json
[]
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
  "menuCategoryId": "{{menuCategoryId}}",
  "businessId": "02d647c9-b836-428f-9dff-2269eb117d73"
}
```
- **Status**: `400`
- **Response (real)**:

```json
{
  "message": "Business with id '02d647c9-b836-428f-9dff-2269eb117d73' not found."
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
- **URL**: `http://localhost:9875/api/Order/business/02d647c9-b836-428f-9dff-2269eb117d73`
- **Status**: `200`
- **Response (real)**:

```json
[]
```

### Create Order

- **Method**: `POST`
- **URL**: `http://localhost:9875/api/Order`
- **Request body (from Postman)**:

```json
{
  "businessId": "02d647c9-b836-428f-9dff-2269eb117d73",
  "customerId": "{{customerId}}",
  "items": [
    { "menuItemId": "{{menuItemId}}", "quantity": 2 }
  ]
}
```
- **Status**: `400`
- **Response (real)**:

```json
{
  "message": "Business with id '02d647c9-b836-428f-9dff-2269eb117d73' not found."
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
- **URL**: `http://localhost:9875/api/Ticket/business/02d647c9-b836-428f-9dff-2269eb117d73`
- **Status**: `200`
- **Response (real)**:

```json
[]
```

### Create Ticket

- **Method**: `POST`
- **URL**: `http://localhost:9875/api/Ticket`
- **Request body (from Postman)**:

```json
{
  "businessId": "02d647c9-b836-428f-9dff-2269eb117d73",
  "customerId": "{{customerId}}",
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
  "message": "Business with id '02d647c9-b836-428f-9dff-2269eb117d73' not found."
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
- **URL**: `http://localhost:9875/api/Interaction/business/02d647c9-b836-428f-9dff-2269eb117d73`
- **Status**: `200`
- **Response (real)**:

```json
[]
```

### Start Interaction

- **Method**: `POST`
- **URL**: `http://localhost:9875/api/Interaction/start`
- **Request body (from Postman)**:

```json
{
  "businessId": "02d647c9-b836-428f-9dff-2269eb117d73",
  "customerId": "{{customerId}}",
  "channel": "WebChat",
  "assignedUserId": null
}
```
- **Status**: `400`
- **Response (real)**:

```json
{
  "message": "Business with id '02d647c9-b836-428f-9dff-2269eb117d73' not found."
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

### Create Message

- **Method**: `POST`
- **URL**: `http://localhost:9875/api/Message`
- **Request body (from Postman)**:

```json
{
  "interactionId": "{{interactionId}}",
  "content": "Hello, how can I help you?",
  "senderType": "Agent",
  "senderId": "{{userId}}"
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
- **URL**: `http://localhost:9875/api/KnowledgeBase/business/02d647c9-b836-428f-9dff-2269eb117d73`
- **Status**: `200`
- **Response (real)**:

```json
[]
```

### Create KB Item [Owner/Admin]

- **Method**: `POST`
- **URL**: `http://localhost:9875/api/KnowledgeBase`
- **Request body (from Postman)**:

```json
{
  "title": "How to place an order",
  "content": "To place an order, browse our menu and add items to your cart...",
  "businessId": "02d647c9-b836-428f-9dff-2269eb117d73",
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


## 14 - FAQ

### Get FAQs by Business [Public]

- **Method**: `GET`
- **URL**: `http://localhost:9875/api/FAQ/business/02d647c9-b836-428f-9dff-2269eb117d73`
- **Status**: `200`
- **Response (real)**:

```json
[]
```

### Manage FAQs [Owner/Admin]

- **Method**: `GET`
- **URL**: `http://localhost:9875/api/FAQ/business/02d647c9-b836-428f-9dff-2269eb117d73/manage`
- **Status**: `200`
- **Response (real)**:

```json
[]
```

### Create FAQ [Owner/Admin]

- **Method**: `POST`
- **URL**: `http://localhost:9875/api/FAQ`
- **Request body (from Postman)**:

```json
{
  "title": "What are your opening hours?",
  "content": "We are open from 9 AM to 10 PM, Monday to Saturday. Closed on Sundays.",
  "businessId": "02d647c9-b836-428f-9dff-2269eb117d73",
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

### Submit Feedback [Public]

- **Method**: `POST`
- **URL**: `http://localhost:9875/api/Feedback`
- **Request body (from Postman)**:

```json
{
  "customerId": "{{customerId}}",
  "interactionId": "{{interactionId}}",
  "rating": 5,
  "comment": "Excellent service and food!",
  "businessId": "02d647c9-b836-428f-9dff-2269eb117d73"
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
    }
  ]
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

### Get Sentiments by Business

- **Method**: `GET`
- **URL**: `http://localhost:9875/api/Sentiment/business/02d647c9-b836-428f-9dff-2269eb117d73`
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
- **URL**: `http://localhost:9875/api/Notification/business/02d647c9-b836-428f-9dff-2269eb117d73`
- **Status**: `200`
- **Response (real)**:

```json
[]
```

### Create Notification [Owner/Admin]

- **Method**: `POST`
- **URL**: `http://localhost:9875/api/Notification`
- **Request body (from Postman)**:

```json
{
  "businessId": "02d647c9-b836-428f-9dff-2269eb117d73",
  "userId": "{{userId}}",
  "title": "New Order Received",
  "message": "Order #1234 has been placed and is awaiting confirmation.",
  "type": "Order"
}
```
- **Status**: `400`
- **Response (real)**:

```json
{
  "message": "Business with id '02d647c9-b836-428f-9dff-2269eb117d73' not found."
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
- **URL**: `http://localhost:9875/api/Report/business/02d647c9-b836-428f-9dff-2269eb117d73`
- **Status**: `200`
- **Response (real)**:

```json
[]
```

### Create Report [Owner/Admin]

- **Method**: `POST`
- **URL**: `http://localhost:9875/api/Report`
- **Request body (from Postman)**:

```json
{
  "businessId": "02d647c9-b836-428f-9dff-2269eb117d73",
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


## 19 - Subscription

### Get All Subscriptions [Admin]

- **Method**: `GET`
- **URL**: `http://localhost:9875/api/Subscription`
- **Status**: `200`
- **Response (real)**:

```json
[
  {
    "id": "013ef596-c853-446e-90ad-98b0452348c7",
    "subscriptionId": "c67356f3-6f3e-4e36-a99a-45ebdc7a96d7",
    "planName": "Monthly",
    "price": 49.99,
    "isActive": true,
    "businessId": "4b741c5c-589e-4c48-885d-d878d75a2918",
    "businessName": "",
    "startDate": "2026-04-22T12:34:08.6053975",
    "endDate": "2026-05-22T12:34:08.6053975"
  },
  {
    "id": "091d1040-5693-4ecf-bf17-4e74b9a8230a",
    "subscriptionId": "b95e39e6-1635-4537-9fc6-2ff70f66e61c",
    "planName": "Monthly",
    "price": 49.99,
    "isActive": true,
    "businessId": "af905b25-6947-4594-b352-dac4c05ee18b",
    "businessName": "",
    "startDate": "2026-04-22T12:23:07.2472446",
    "endDate": "2026-05-22T12:23:07.2472446"
  },
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
    "id": "5a133803-b7b2-4cf3-92b6-3e0c138abfad",
    "subscriptionId": "4bae33f2-1a85-4816-b6d7-f214cff5cc2c",
    "planName": "Monthly",
    "price": 49.99,
    "isActive": true,
    "businessId": "76c32d17-76ba-458e-9687-df9dd75b96a4",
    "businessName": "",
    "startDate": "2026-04-22T12:33:14.253989",
    "endDate": "2026-05-22T12:33:14.253989"
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
    "id": "8e45fac0-e69e-4e99-9d1b-5fe6a2139a1a",
    "subscriptionId": "249f080b-412a-4c53-8d3d-0d630f98766f",
    "planName": "Monthly",
    "price": 49.99,
    "isActive": true,
    "businessId": "fdb58ac3-df2f-4374-b4b5-41ed2a08da20",
    "businessName": "",
    "startDate": "2026-04-22T12:32:43.3474636",
    "endDate": "2026-05-22T12:32:43.3474636"
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
  },
  {
    "id": "bec71888-c6fd-42cd-97cb-a5d46ab4a47f",
    "subscriptionId": "1321a80a-7e3d-4540-af75-7e5c13c7d6a2",
    "planName": "Monthly",
    "price": 49.99,
    "isActive": true,
    "businessId": "3344be22-4931-404a-a4d1-750fc803df36",
    "businessName": "",
    "startDate": "2026-04-22T12:31:05.5658846",
    "endDate": "2026-05-22T12:31:05.5658846"
  },
  {
    "id": "f69d4994-9ea3-4080-bfe1-28ff2c9fd51b",
    "subscriptionId": "2942a9f3-40c7-4253-b841-c43b1b726005",
    "planName": "Monthly",
    "price": 49.99,
    "isActive": true,
    "businessId": "a92e7b8b-9237-4cc5-8479-52762ee86b54",
    "businessName": "",
    "startDate": "2026-04-22T12:29:43.129014",
    "endDate": "2026-05-22T12:29:43.129014"
  }
]
```

### Get Subscriptions by Business [Owner/Admin]

- **Method**: `GET`
- **URL**: `http://localhost:9875/api/Subscription/business/02d647c9-b836-428f-9dff-2269eb117d73`
- **Status**: `200`
- **Response (real)**:

```json
[]
```

### Get Active Subscription [Owner/Admin]

- **Method**: `GET`
- **URL**: `http://localhost:9875/api/Subscription/business/02d647c9-b836-428f-9dff-2269eb117d73/active`
- **Status**: `404`
- **Response (real)**:

```json
{
  "message": "No active subscription found for business '02d647c9-b836-428f-9dff-2269eb117d73'."
}
```

### Create Subscription [Admin]

- **Method**: `POST`
- **URL**: `http://localhost:9875/api/Subscription`
- **Request body (from Postman)**:

```json
{
  "businessId": "02d647c9-b836-428f-9dff-2269eb117d73",
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
    "id": "132ba2e8-4cf0-4854-a7db-09bc62560ec7",
    "paymentId": "c177aeb7-04a7-4aae-bea8-50578f1fa7c3",
    "subscriptionId": "013ef596-c853-446e-90ad-98b0452348c7",
    "subscriptionPlanName": "",
    "amount": 49.99,
    "paymentMethod": "Card",
    "transactionDate": "2026-04-22T12:34:08.6054189",
    "status": "Success"
  },
  {
    "id": "7461538e-2c44-481b-babd-9d6c4ae86032",
    "paymentId": "d8c23170-5efc-457b-9eae-8c127fda65bc",
    "subscriptionId": "bec71888-c6fd-42cd-97cb-a5d46ab4a47f",
    "subscriptionPlanName": "",
    "amount": 49.99,
    "paymentMethod": "Card",
    "transactionDate": "2026-04-22T12:31:05.5659152",
    "status": "Success"
  },
  {
    "id": "8c543185-1319-4f37-9200-b987f0d9e899",
    "paymentId": "1989448a-cfa1-4e11-9038-1b9043077537",
    "subscriptionId": "091d1040-5693-4ecf-bf17-4e74b9a8230a",
    "subscriptionPlanName": "",
    "amount": 49.99,
    "paymentMethod": "Card",
    "transactionDate": "2026-04-22T12:23:07.2595428",
    "status": "Success"
  },
  {
    "id": "b5f0c71f-45b3-444b-b098-af0817721c53",
    "paymentId": "731ba3f9-7c0b-48a9-84e9-f6a1fb72371c",
    "subscriptionId": "5a133803-b7b2-4cf3-92b6-3e0c138abfad",
    "subscriptionPlanName": "",
    "amount": 49.99,
    "paymentMethod": "Card",
    "transactionDate": "2026-04-22T12:33:14.254008",
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
    "id": "ca71295b-03c0-4e7e-8866-fdb12db5a754",
    "paymentId": "49ffe72e-bcec-41a3-8f84-d5eee5a85122",
    "subscriptionId": "8e45fac0-e69e-4e99-9d1b-5fe6a2139a1a",
    "subscriptionPlanName": "",
    "amount": 49.99,
    "paymentMethod": "Card",
    "transactionDate": "2026-04-22T12:32:43.3474896",
    "status": "Success"
  },
  {
    "id": "f5aebf74-08cf-4712-a5c4-51e23d1535aa",
    "paymentId": "e167fb28-a854-4437-b50f-26a5a842dbcf",
    "subscriptionId": "f69d4994-9ea3-4080-bfe1-28ff2c9fd51b",
    "subscriptionPlanName": "",
    "amount": 49.99,
    "paymentMethod": "Card",
    "transactionDate": "2026-04-22T12:29:43.1290598",
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

### Get Payments by Business [Owner/Admin]

- **Method**: `GET`
- **URL**: `http://localhost:9875/api/PaymentTransaction/business/02d647c9-b836-428f-9dff-2269eb117d73`
- **Status**: `200`
- **Response (real)**:

```json
[]
```

### Create Payment [Admin]

- **Method**: `POST`
- **URL**: `http://localhost:9875/api/PaymentTransaction`
- **Request body (from Postman)**:

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
- **Status**: `400`
- **Response (real)**:

```json
{
  "message": "Subscription with id '{{subscriptionId}}' not found."
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
- **URL**: `http://localhost:9875/api/Integration/business/02d647c9-b836-428f-9dff-2269eb117d73`
- **Status**: `200`
- **Response (real)**:

```json
[]
```

### Connect Integration [Owner/Admin]

- **Method**: `POST`
- **URL**: `http://localhost:9875/api/Integration/connect`
- **Request body (from Postman)**:

```json
{
  "businessId": "02d647c9-b836-428f-9dff-2269eb117d73",
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


## 22 - Setting

### Get Settings by Business [Owner/Admin]

- **Method**: `GET`
- **URL**: `http://localhost:9875/api/Setting/business/02d647c9-b836-428f-9dff-2269eb117d73`
- **Status**: `404`
- **Response (real)**:

```json
{
  "message": "Settings not found for business '02d647c9-b836-428f-9dff-2269eb117d73'."
}
```

### Update Settings [Owner/Admin]

- **Method**: `PUT`
- **URL**: `http://localhost:9875/api/Setting/business/02d647c9-b836-428f-9dff-2269eb117d73`
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
  "message": "Business with id '02d647c9-b836-428f-9dff-2269eb117d73' not found."
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
    "auditLogId": "03759a4b-8231-471f-bc0d-1010aab44b51",
    "businessId": "0bc8178d-0ca1-45ce-8da0-e3db10f10288",
    "businessName": "",
    "action": "CreateBusiness",
    "entity": "Business",
    "entityId": "0bc8178d-0ca1-45ce-8da0-e3db10f10288",
    "createdAt": "2026-04-22T12:48:31.8018462",
    "userId": "2f27efef-05b9-4a82-93f7-0d757d65c82d",
    "userName": ""
  },
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
    "auditLogId": "17bb6442-c0de-41db-b509-725bfbb3705f",
    "businessId": "f6c9135e-c5c0-4d1f-ac13-ae1046706114",
    "businessName": "",
    "action": "CreateBusiness",
    "entity": "Business",
    "entityId": "f6c9135e-c5c0-4d1f-ac13-ae1046706114",
    "createdAt": "2026-04-25T16:53:23.2495474",
    "userId": "2c44346a-0473-4fa3-9846-70032b6b0e88",
    "userName": ""
  },
  {
    "auditLogId": "197b8c28-f0b5-472d-bf99-98dbadca68f9",
    "businessId": "c5d75e93-eddc-4be1-8ecb-9d1734dbb7de",
    "businessName": "",
    "action": "CreateBusiness",
    "entity": "Business",
    "entityId": "c5d75e93-eddc-4be1-8ecb-9d1734dbb7de",
    "createdAt": "2026-04-22T12:34:08.5476397",
    "userId": "8b5341fe-6e71-424c-abb5-90df860312df",
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
    "auditLogId": "489a4474-2f02-4871-9f8a-aca720cfd5a2",
    "businessId": "1aeed440-454b-4780-8fc6-c1fedb9f0113",
    "businessName": "",
    "action": "CreateBusiness",
    "entity": "Business",
    "entityId": "1aeed440-454b-4780-8fc6-c1fedb9f0113",
    "createdAt": "2026-04-22T12:23:07.0947645",
    "userId": "7482d102-c2c2-4ebf-96e4-ff5f72c90a0a",
    "userName": ""
  },
  {
    "auditLogId": "5dbd13cc-7d2e-437e-99ab-b2132f17a01c",
    "businessId": "0af8c860-0d41-470b-940c-0f54b0855230",
    "businessName": "",
    "action": "CreateBusiness",
    "entity": "Business",
    "entityId": "0af8c860-0d41-470b-940c-0f54b0855230",
    "createdAt": "2026-04-22T12:31:05.4917981",
    "userId": "2342c1bc-a8e7-4eb9-a14f-5db8c3242f53",
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
    "auditLogId": "7847862f-eac3-40de-bee4-b967570943bc",
    "businessId": "d2462718-ab23-40ad-94fd-36d7e35d240c",
    "businessName": "",
    "action": "CreateBusiness",
    "entity": "Business",
    "entityId": "d2462718-ab23-40ad-94fd-36d7e35d240c",
    "createdAt": "2026-04-25T16:54:12.190245",
    "userId": "0eaa5647-8e13-448a-84c3-59a0eacaca88",
    "userName": ""
  },
  {
    "auditLogId": "81e903f9-d5b3-4268-a94d-fa64b5b2f52a",
    "businessId": "e62fb486-1085-46b4-9a0d-39371903e8ed",
    "businessName": "",
    "action": "CreateBusiness",
    "entity": "Business",
    "entityId": "e62fb486-1085-46b4-9a0d-39371903e8ed",
    "createdAt": "2026-04-25T16:51:32.1695617",
    "userId": "54223802-80f2-43c0-9ff7-1e15654f8010",
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
    "auditLogId": "c13086a9-af00-46e3-b90e-c9f48148ddb9",
    "businessId": "ddf2c130-9507-4435-a47d-455c3b29daa2",
    "businessName": "",
    "action": "CreateBusiness",
    "entity": "Business",
    "entityId": "ddf2c130-9507-4435-a47d-455c3b29daa2",
    "createdAt": "2026-04-22T12:32:43.2750692",
    "userId": "b940fecc-1a1d-46c8-999e-b29d46c864dd",
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
    "auditLogId": "d0c45c55-b683-49a7-bc9a-fe6ae09bafbe",
    "businessId": "079018da-a455-476d-9124-fa5558b265f4",
    "businessName": "",
    "action": "CreateBusiness",
    "entity": "Business",
    "entityId": "079018da-a455-476d-9124-fa5558b265f4",
    "createdAt": "2026-04-22T12:29:43.0698601",
    "userId": "b71dff9b-0f2b-4b93-91cc-9e780333ef79",
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
  },
  {
    "auditLogId": "ee51b1e9-6964-4eb0-ad7b-1ab4455dff88",
    "businessId": "9ada2312-9d87-4118-a960-7e8fbfe448d0",
    "businessName": "",
    "action": "CreateBusiness",
    "entity": "Business",
    "entityId": "9ada2312-9d87-4118-a960-7e8fbfe448d0",
    "createdAt": "2026-04-22T12:33:14.1970038",
    "userId": "937e1a5c-d90d-4207-a063-9859d2538044",
    "userName": ""
  }
]
```

### Get Audit Logs by Business [Owner/Admin]

- **Method**: `GET`
- **URL**: `http://localhost:9875/api/AuditLog/business/02d647c9-b836-428f-9dff-2269eb117d73`
- **Status**: `200`
- **Response (real)**:

```json
[]
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
  "conversationId": "4a208f0a-184a-47e2-85dc-5d8524e465a4",
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
- **URL**: `http://localhost:9875/api/CustomerChat/capabilities/02d647c9-b836-428f-9dff-2269eb117d73`
- **Status**: `404`
- **Response (real)**:

```json
{
  "message": "Business with id '02d647c9-b836-428f-9dff-2269eb117d73' not found."
}
```

### Send Chat Message

- **Method**: `POST`
- **URL**: `http://localhost:9875/api/CustomerChat/message`
- **Request body (from Postman)**:

```json
{
  "businessId": "02d647c9-b836-428f-9dff-2269eb117d73",
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
  "businessId": "02d647c9-b836-428f-9dff-2269eb117d73",
  "mainMenuItemId": "{{menuItemId}}",
  "customerId": null
}
```
- **Status**: `200`
- **Response (real)**:

```json
[]
```


## 26 - Customer Voice [Public]

### Initialize Voice Session

- **Method**: `POST`
- **URL**: `http://localhost:9875/api/CustomerVoice/session/initialize`
- **Request body (from Postman)**:

```json
{
  "businessId": "02d647c9-b836-428f-9dff-2269eb117d73",
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
  "businessId": "02d647c9-b836-428f-9dff-2269eb117d73",
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

### Submit Voice Feedback

- **Method**: `POST`
- **URL**: `http://localhost:9875/api/CustomerVoice/feedback`
- **Request body (from Postman)**:

```json
{
  "interactionId": "{{interactionId}}",
  "rating": 5,
  "comment": "Very helpful voice assistant!"
}
```
- **Status**: `404`
- **Response (real)**:

```json
{
  "message": "Interaction with id '{{interactionId}}' not found."
}
```

### Get Voice Settings

- **Method**: `GET`
- **URL**: `http://localhost:9875/api/CustomerVoice/settings/02d647c9-b836-428f-9dff-2269eb117d73`
- **Status**: `404`
- **Response (real)**:

```json
{
  "message": "Voice settings not found for this business."
}
```

