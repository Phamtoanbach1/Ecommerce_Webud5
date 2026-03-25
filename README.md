# 🛒 WebUD5 E-Commerce Backend

A modern **ASP.NET Core 8** backend system implementing **CQRS architecture** with **MediatR**, **JWT authentication**, **Google OAuth**, and **VNPay payment integration**. Built with Clean Architecture principles and Entity Framework Core.

---

## 📋 Table of Contents

- [Overview](#overview)
- [Tech Stack](#-tech-stack)
- [Features](#-features)
- [Project Structure](#-project-structure)
- [Prerequisites](#-prerequisites)
- [Getting Started](#-getting-started)
- [Configuration](#-configuration)
- [API Documentation](#-api-documentation)
- [Architecture](#-architecture)
- [Security](#-security)
- [Development](#-development)
- [Contributing](#-contributing)
- [License](#-license)

---

## 🎯 Overview

**WebUD5** is a feature-rich e-commerce backend built with **clean architecture principles** and the **CQRS (Command Query Responsibility Segregation) pattern**. It provides secure transaction processing, user authentication, payment integration, and modern REST API capabilities.

This project is production-ready and designed for scalability with:
- Async/await throughout the application
- Dependency injection for loose coupling
- Proper error handling and validation
- Comprehensive API documentation with Swagger

---

## 🛠️ Tech Stack

| Category | Technology |
|----------|-----------|
| **Framework** | ASP.NET Core 8 (.NET 8) |
| **Language** | C# 12.0 |
| **Database** | SQL Server + Entity Framework Core |
| **Architecture Pattern** | CQRS (Command Query Responsibility Segregation) |
| **Mediator** | MediatR |
| **Authentication** | JWT Bearer + Google OAuth 2.0 |
| **Payment Gateway** | VNPay |
| **API Documentation** | Swagger/OpenAPI |
| **ORM** | Entity Framework Core |

---

## ✨ Features

### 🔐 Authentication & Authorization
- ✅ **JWT Token Authentication** with configurable expiration
- ✅ **Google OAuth 2.0** for social login
- ✅ **Cookie-based authentication** support
- ✅ Token validation with issuer, audience, and signature verification
- ✅ Secure secret key management via configuration

### 💳 Transaction Management
- ✅ **Transaction status tracking** (pending, success, failed, refunded)
- ✅ **Async transaction processing** with proper error handling
- ✅ **CQRS command handling** via MediatR
- ✅ Real-time transaction status updates

### 💰 Payment Processing
- ✅ **VNPay payment gateway integration** for Vietnamese e-commerce
- ✅ **Secure payment transaction handling**
- ✅ Transaction callback processing and verification
- ✅ Multiple payment status management

### 🌐 API Features
- ✅ **RESTful API endpoints** with clean contracts
- ✅ **Swagger UI** for interactive API documentation
- ✅ **CORS support** for cross-origin requests
- ✅ Request/response validation
- ✅ Comprehensive error handling

### 📧 Service Integration
- ✅ **Email service** for notifications and confirmations
- ✅ **JWT service** for token generation and validation
- ✅ **VNPay service** for payment processing

---

## 📁 Project Structure

```
WebUD5/
├── Feature/                          # Feature-based modules
│   └── TransactionFeature/
│       ├── Command/
│       │   └── UpdateTransactionStatusCommand.cs
│       └── Query/
│
├── Models/
│   └── WebUD5DbContext.cs           # Entity Framework DbContext
│
├── Service/
│   ├── IJwtService.cs
│   ├── JwtService.cs
│   ├── IEmailService.cs
│   ├── EmailService.cs
│   └── VNPayService.cs
│
├── Controllers/                      # API Endpoints
│
├── Program.cs                        # Startup configuration
├── appsettings.json                  # Configuration (NOT in git)
├── appsettings.example.json          # Configuration template
├── README.md                         # This file
└── WebUD5.sln                        # Solution file
```

### Directory Conventions

- **Feature/**: Organized by business features using folder structure
  - Each feature contains Commands, Queries, and Handlers
- **Models/**: Database entities and DbContext
- **Service/**: Business logic and external integrations
- **Controllers/**: REST API endpoints

---

## 📦 Prerequisites

Before running the application, ensure you have:

- **[.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)** or later
- **[SQL Server 2019](https://www.microsoft.com/en-us/sql-server/sql-server-2019)** or later
- **[Visual Studio 2022](https://visualstudio.microsoft.com/)** or VS Code
- **[Git](https://git-scm.com/)** for version control
- **Node.js 16+** (for React frontend integration - optional)

---

## 🚀 Getting Started

### 1️⃣ Clone the Repository

```bash
git clone https://github.com/Phamtoanbach1/Ecommerce_Webud5.git
cd Ecommerce_Webud5
```

### 2️⃣ Restore NuGet Packages

```bash
dotnet restore
```

### 3️⃣ Configure Database Connection

Update the connection string in `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DbConnection": "Server=YOUR_SERVER;Database=WebUD5;User Id=sa;Password=YourPassword123;"
  }
}
```

**For Windows Authentication:**
```json
{
  "ConnectionStrings": {
    "DbConnection": "Server=.;Database=WebUD5;Trusted_Connection=true;"
  }
}
```

### 4️⃣ Apply Database Migrations

```bash
dotnet ef database update
```

If migrations don't exist:
```bash
dotnet ef migrations add InitialCreate
dotnet ef database update
```

### 5️⃣ Run the Application

```bash
dotnet run
```

The API will be available at:
- 🌐 **API**: `https://localhost:5001`
- 📚 **Swagger UI**: `https://localhost:5001/swagger`

---

## ⚙️ Configuration

### 1. JWT Settings

Update `appsettings.json`:

```json
{
  "JWT": {
    "SigningKey": "YourSecretKeyHere_Min32Characters_Long",
    "Issuer": "WebUD5",
    "Audience": "WebUD5-Client",
    "ExpirationMinutes": 60
  }
}
```

**Security Requirements:**
- `SigningKey` must be at least 32 characters long
- Store in environment variables in production
- Never commit to version control

### 2. Google OAuth Configuration

Update `appsettings.json`:

```json
{
  "Authentication": {
    "Google": {
      "ClientId": "YOUR_GOOGLE_CLIENT_ID.apps.googleusercontent.com",
      "ClientSecret": "YOUR_GOOGLE_CLIENT_SECRET"
    }
  }
}
```

**Setup Steps:**
1. Visit [Google Cloud Console](https://console.cloud.google.com/)
2. Create a new project
3. Enable Google+ API
4. Create OAuth 2.0 Client ID (Web Application)
5. Add authorized redirect URIs:
   - `http://localhost:5000/signin-google` (development)
   - `https://yourdomain.com/signin-google` (production)

### 3. SendGrid Configuration (Email Service)

Update `appsettings.json`:

```json
{
  "SendGridApiKey": "YOUR_SENDGRID_API_KEY"
}
```

Get API Key from [SendGrid Dashboard](https://app.sendgrid.com/settings/api_keys)

### 4. VNPay Configuration (Payment Gateway)

Update `appsettings.json`:

```json
{
  "VNPay": {
    "TmnCode": "YOUR_TMN_CODE",
    "HashSecret": "YOUR_HASH_SECRET",
    "Url": "https://sandbox.vnpayment.vn/paygate/pay.html"
  }
}
```

Get credentials from [VNPay Portal](https://merchant.vnpayment.vn/)

### 5. CORS Configuration

Backend is configured to accept requests from `http://localhost:3000` (React frontend).

To modify in production, update `Program.cs`:

```csharp
policy.WithOrigins
    ("http://localhost:3000",           // Development
     "https://yourdomain.com")          // Production
    .AllowAnyMethod()
    .AllowAnyHeader()
    .AllowCredentials();
```

---

## 📚 API Documentation

### 🎯 Swagger UI

Once the application is running, visit:
```
https://localhost:5001/swagger
```

### 📌 Key Endpoints

#### Update Transaction Status
```
POST /api/transactions/update-status
Authorization: Bearer {jwt_token}

Request Body:
{
  "transactionId": 1,
  "status": "success"
}

Response:
{
  "success": true,
  "message": "Transaction status updated"
}
```

#### Authenticate (Login)
```
POST /api/auth/login

Request Body:
{
  "email": "user@example.com",
  "password": "password123"
}

Response:
{
  "token": "eyJhbGciOiJIUzI1NiIs...",
  "expiresIn": 3600
}
```

#### Google OAuth Login
```
POST /signin-google
```

---

## 🏗️ Architecture

### CQRS Pattern

The application uses **MediatR** to implement the **CQRS pattern**:

```csharp
// Command: Write operation
public class UpdateTransactionStatusCommand : IRequest<bool>
{
    public int TransactionId { get; set; }
    public string Status { get; set; }
}

// Handler: Processes the command
public class UpdateTransactionStatusCommandHandler 
    : IRequestHandler<UpdateTransactionStatusCommand, bool>
{
    public async Task<bool> Handle(UpdateTransactionStatusCommand request, 
        CancellationToken cancellationToken)
    {
        // Business logic here
    }
}

// Usage in Controller
await _mediator.Send(new UpdateTransactionStatusCommand(1, "success"));
```

### Clean Architecture Layers

```
┌─────────────────────────────────────┐
│   Presentation Layer                │
│   Controllers & API Endpoints       │
├─────────────────────────────────────┤
│   Application Layer                 │
│   MediatR Handlers & Commands       │
├─────────────────────────────────────┤
│   Domain Layer                      │
│   Entities & Business Models        │
├─────────────────────────────────────┤
│   Infrastructure Layer              │
│   Database & External Services      │
└─────────────────────────────────────┘
```

### Dependency Injection

Services are registered in `Program.cs`:

```csharp
// Scoped: New instance per HTTP request
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<IEmailService, EmailService>();

// Transient: New instance every time
builder.Services.AddTransient<VNPayService>();

// Singleton: Single instance for app lifetime
builder.Services.AddSingleton<IHttpContextAccessor>();
```

---

## 🔐 Security

### 1. JWT Security
- ✅ HTTPS-only in production
- ✅ Token signature validation
- ✅ Issuer and audience verification
- ✅ Expiration time enforcement
- ✅ Secure secret key management

### 2. Password Security
- ✅ Bcrypt hashing (via ASP.NET Identity)
- ✅ Never stored in plain text
- ✅ Salted hashes for additional security

### 3. API Security
- ✅ CORS properly configured
- ✅ SQL injection prevention (via EF Core)
- ✅ XSS protection via Swagger
- ✅ CSRF tokens for state-changing operations

### 4. Secrets Management
- ✅ `appsettings.json` in `.gitignore`
- ✅ Configuration via environment variables
- ✅ User Secrets for local development
- ✅ Azure Key Vault for production

### 5. Best Practices
- ✅ Always validate input
- ✅ Use HTTPS in production
- ✅ Implement rate limiting
- ✅ Log security events
- ✅ Regular security audits

---

## 💻 Development

### Build the Project
```bash
dotnet build
```

### Run Tests
```bash
dotnet test
```

### Run with Hot Reload
```bash
dotnet watch run
```

### Code Style Guidelines

- Follow **C# naming conventions**
- Use **async/await** for I/O operations
- Apply **SOLID principles**
- Write **self-documenting code**
- Add comments for complex logic only

### Adding a New Feature

1. **Create feature folder structure:**
   ```
   WebUD5/Feature/YourFeature/
   ├── Command/
   │   ├── CreateYourCommand.cs
   │   └── CreateYourCommandHandler.cs
   └── Query/
       ├── GetYourQuery.cs
       └── GetYourQueryHandler.cs
   ```

2. **Create command/query:**
   ```csharp
   public class CreateYourCommand : IRequest<bool>
   {
       public string Name { get; set; }
   }
   ```

3. **Create handler:**
   ```csharp
   public class CreateYourCommandHandler 
       : IRequestHandler<CreateYourCommand, bool>
   {
       public async Task<bool> Handle(CreateYourCommand request, 
           CancellationToken cancellationToken)
       {
           // Implementation
       }
   }
   ```

4. **Register in DI (auto-registered by MediatR)**

5. **Use in controller:**
   ```csharp
   var result = await _mediator.Send(new CreateYourCommand { Name = "Test" });
   ```

---

## 🤝 Contributing

We welcome contributions! Please follow these steps:

1. **Fork the repository**
   ```bash
   git clone https://github.com/YOUR_USERNAME/Ecommerce_Webud5.git
   ```

2. **Create a feature branch**
   ```bash
   git checkout -b feature/your-feature-name
   ```

3. **Make your changes**
   - Follow code style guidelines
   - Write meaningful commit messages
   - Test your changes

4. **Commit and push**
   ```bash
   git add .
   git commit -m "Add your feature description"
   git push origin feature/your-feature-name
   ```

5. **Open a Pull Request**
   - Describe your changes clearly
   - Reference related issues
   - Wait for code review

---

## 📄 License

This project is licensed under the **MIT License** - see the [LICENSE](LICENSE) file for details.

---

## 📧 Contact & Support

**Author:** Pham Toan Bach

- **GitHub:** [@Phamtoanbach1](https://github.com/Phamtoanbach1)
- **Repository:** [Ecommerce_Webud5](https://github.com/Phamtoanbach1/Ecommerce_Webud5)
- **Issues:** [Report a bug](https://github.com/Phamtoanbach1/Ecommerce_Webud5/issues)

---

## 🙏 Acknowledgments

- [ASP.NET Core Team](https://github.com/dotnet/aspnetcore) - For the excellent framework
- [MediatR](https://github.com/jbogard/MediatR) - For CQRS implementation
- [Entity Framework Core](https://github.com/dotnet/efcore) - For ORM capabilities
- [VNPay](https://vnpayment.vn/) - For payment integration support

---

## 📈 Roadmap

- [ ] Unit tests for handlers
- [ ] Integration tests for API endpoints
- [ ] Logging with Serilog
- [ ] Caching with Redis
- [ ] Real-time notifications with SignalR
- [ ] Admin dashboard
- [ ] Advanced analytics

---

**Made with ❤️ by Pham Toan Bach**

⭐ If you find this project useful, please consider giving it a star on GitHub!

---

*Last Updated: January 2025*
