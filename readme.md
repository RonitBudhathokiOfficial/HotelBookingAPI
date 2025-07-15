# Hotel Booking API

This is a RESTful Hotel Booking API built with **ASP.NET Core** and **Entity Framework Core**, designed to handle hotel room bookings. It supports operations like listing hotels, checking room availability, booking rooms, and retrieving booking details.

## 🚀 Features

- ✅ Search hotels by name
- ✅ Check available rooms based on date and guest count
- ✅ Book rooms with automatic reference generation
- ✅ Retrieve booking by reference
- ✅ Seed and reset test data
- ✅ Fully documented with Swagger (OpenAPI)
- ✅ Unit tested with xUnit and EF Core In-Memory

---

## 🔧 Technologies Used

- ASP.NET Core 7
- Entity Framework Core (In-Memory)
- AutoMapper
- Swagger / Swashbuckle
- xUnit, Moq, FluentAssertions

---

## 📦 Project Structure

```
HotelBookingAPI/         --> Main API project
HotelBookingAPI.Tests/   --> Unit test project (xUnit)
```

---

## ⚙️ How to Run

1. **Clone the repo**

```bash
git clone <your-repo-url>
cd HotelBookingAPI
```

2. **Run the API**

```bash
dotnet run --project HotelBookingAPI
```

3. **Access Swagger UI**

```
https://localhost:<port>/swagger
```

4. **Seed the database**

```http
POST /seed
```

5. **Reset the database**

```http
POST /reset
```

6. **Usage Walkthrough**

To test the flow using Swagger UI or Postman:

✅ Seed the hotel data using POST /seed

✅ Get all hotels using GET /all or search by name using GET /search?name=... to find a hotel ID

✅ Get available rooms using GET /{hotelId}/available-rooms?startDate=2025-08-01&endDate=2025-08-05&numberOfGuests=2

✅ Book a room using POST /book — hotelId and roomId must be valid; bookingReference can be left empty; use yyyy-MM-dd date format

✅ Retrieve the booking using GET /{bookingReference}

✅ Verify room is booked — repeat step 3 to confirm the same room no longer appears in availability

---

## 🧪 Run Tests

```bash
dotnet test HotelBookingAPI.Tests
```

This will run all unit tests using an in-memory EF Core database.

---

## 📘 API Endpoints

| Endpoint                          | Description                            |
| --------------------------------- | -------------------------------------- |
| `GET /search?name={hotelName}`    | Find a hotel by name                   |
| `GET /{hotelId}/available-rooms`  | Get available rooms by date and guests |
| `GET /api/hotels/{hotelId}/rooms` | Get all rooms in a hotel               |
| `POST /book`                      | Book a room with full Booking model    |
| `GET /{reference}`                | Get booking by reference               |
| `POST /seed`                      | Seed the database                      |
| `POST /reset`                     | Clear all data                         |

---

## 📝 Notes

- The API does **not** require authentication.
- Booking references are auto-generated and unique.
- Swagger UI provides a convenient way to test the endpoints.

---

📤 Deployment 

The solution has been deployed to AZURE : https://hotelbookingserviceapi-cnfaa0huhpgtb6gz.uksouth-01.azurewebsites.net/swagger/index.html


## 📫 Contact

Created by **Ronit Budhathoki**

---

