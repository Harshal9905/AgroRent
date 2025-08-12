# AgroRent .NET Core Web API

A .NET Core Web API for agricultural equipment rental platform.

## Features

- **Authentication & Authorization**: JWT-based authentication with role-based access control
- **Equipment Management**: CRUD operations for agricultural equipment with image upload via Cloudinary
- **Booking System**: Complete booking lifecycle management with status tracking
- **Payment Integration**: Razorpay payment gateway integration
- **Email Notifications**: SMTP-based email service for verification and confirmations
- **Automatic Scheduling**: Background service for automatic booking status updates

## Architecture

- **Controllers**: REST API endpoints with proper HTTP status codes
- **Services**: Business logic layer with dependency injection
- **Repositories**: Data access layer using Entity Framework Core
- **Models**: Entity models with proper relationships and validation
- **DTOs**: Data transfer objects for API requests/responses
- **Security**: JWT authentication, CORS configuration, and role-based authorization

## Prerequisites

- .NET 8.0 SDK
- MySQL Database
- Cloudinary Account
- Razorpay Account
- SMTP Email Service (Gmail recommended)

## Installation



1. **Install NuGet packages**
   ```bash
   dotnet restore
   ```

2. **Configure appsettings.json**
   - Update database connection string
   - Add JWT secret key
   - Configure email settings
   - Add Cloudinary credentials
   - Add Razorpay API keys

4. **Run database migrations**
   ```bash
   dotnet ef database update
   ```

5. **Run the application**
   ```bash
   dotnet run
   ```

## Configuration

### Database Connection
```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost;Database=agro_rent;Uid=your_username;Pwd=your_password;CharSet=utf8mb4;"
}
```

### JWT Settings
```json
"JwtSettings": {
  "SecretKey": "your-secret-key-here",
  "ExpirationMinutes": 1440
}
```

### Email Configuration
```json
"EmailSettings": {
  "SmtpHost": "smtp.gmail.com",
  "SmtpPort": 587,
  "Username": "your-email@gmail.com",
  "Password": "your-app-password",
  "EnableSsl": true
}
```

### Cloudinary Configuration
```json
"CloudinarySettings": {
  "CloudName": "your-cloud-name",
  "ApiKey": "your-api-key",
  "ApiSecret": "your-api-secret"
}
```

### Razorpay Configuration
```json
"RazorpaySettings": {
  "ApiKey": "your-api-key",
  "ApiSecret": "your-api-secret"
}
```

## API Endpoints

### Authentication
- `POST /auth/signUp` - User registration
- `POST /auth/signIn` - User login
- `GET /auth/verify?token={token}` - Email verification

### Equipment
- `GET /equipment` - Get all equipment
- `GET /equipment/{id}` - Get equipment by ID
- `GET /equipment/owner/{ownerId}` - Get equipment by owner
- `POST /equipment` - Add new equipment
- `PUT /equipment/{id}` - Update equipment
- `DELETE /equipment/{id}` - Delete equipment
- `PUT /equipment/{id}/availability` - Update availability

### Bookings
- `GET /booking` - Get all bookings
- `GET /booking/{id}` - Get booking by ID
- `GET /booking/farmer/{farmerId}` - Get bookings by farmer
- `GET /booking/owner/{ownerId}` - Get bookings by equipment owner
- `POST /booking` - Create new booking
- `PUT /booking/{id}/status` - Update booking status
- `PUT /booking/{id}/cancel` - Cancel booking
- `DELETE /booking/{id}` - Delete booking

### Farmers
- `GET /farmer` - Get all farmers
- `GET /farmer/{id}` - Get farmer by ID
- `PUT /farmer/{id}` - Update farmer
- `DELETE /farmer/{id}` - Delete farmer
- `PUT /farmer/{id}/deactivate` - Deactivate farmer
- `PUT /farmer/{id}/activate` - Activate farmer

### Payments
- `GET /payment` - Get all payments
- `GET /payment/{id}` - Get payment by ID
- `GET /payment/booking/{bookingId}` - Get payment by booking
- `POST /payment/create` - Create payment
- `POST /payment/verify` - Verify payment
- `PUT /payment/{id}/status` - Update payment status
- `DELETE /payment/{id}` - Delete payment

## Authentication

All endpoints except `/auth/*` require JWT authentication. Include the token in the Authorization header:

```
Authorization: Bearer <your-jwt-token>
```

## Database Schema

The application uses Entity Framework Core with MySQL. The database will be automatically created on first run with the following tables:

- `farmers` - User accounts and profiles
- `equipments` - Agricultural equipment listings
- `bookings` - Equipment rental bookings
- `payments` - Payment records
- `verification_tokens` - Email verification tokens

## Background Services

- **BookingStatusScheduler**: Automatically updates booking statuses every 30 minutes
  - Marks completed bookings as COMPLETED
  - Cancels expired pending bookings

## Error Handling

The API uses consistent error responses with the following format:

```json
{
  "success": false,
  "message": "Error description",
  "data": null
}
```

## Development

### Running Tests
```bash
dotnet test
```

### Building for Production
```bash
dotnet publish -c Release
```

### Docker Support
```bash
docker build -t agrorent .
docker run -p 5000:5000 agrorent
```

## Contributing

1. Fork the repository
2. Create a feature branch
3. Make your changes
4. Add tests if applicable
5. Submit a pull request

## License

This project is licensed under the MIT License.

## Support

For support and questions, please open an issue in the repository.
