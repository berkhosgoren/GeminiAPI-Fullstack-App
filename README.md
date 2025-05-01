# 🔮 Gemini Testing Fullstack App

This is a small yet functional **full-stack application** that integrates with the **Gemini API** (Google AI) to generate responses to user queries. The app also **saves every user query and response to a database**, allowing logged-in users to view their previous interactions.

The project demonstrates:
- 🔐 JWT-based authentication
- 📦 Clean API structure with .NET 8 Web API
- 🌐 Angular frontend with route guards
- 📄 Real-time Gemini API interaction
- 💾 Persistent logging of user queries to a SQL Server database

---

## 🚀 Technologies Used

| Layer      | Stack |
|------------|-------|
| Backend    | .NET 8 Web API, Entity Framework Core, SQL Server |
| Frontend   | Angular 16, TypeScript, PrimeNG |
| Auth       | JWT Token (Bearer) |
| AI API     | Gemini (Google AI API) |

---

## 🧠 Features

### ✅ Authentication
- Users can **register and log in**
- Authentication is handled using **JWT tokens**
- User ID is saved to `localStorage` to associate requests

### ✅ Chat Functionality
- Send a question to Gemini
- Response is generated and displayed in the UI
- Each interaction is saved to the database along with:
  - Question text
  - AI-generated response
  - Timestamp
  - User ID

### ✅ View History
- Authenticated users can navigate to a **Previous Questions** page
- It loads the **question + answer history** from the database (not from localStorage)
- Uses `GET /api/Content/GetHistory/{userId}`

---

## 🛠️ How to Run

### Backend (.NET API)

```bash
cd GeminiApp
dotnet restore
dotnet ef database update
dotnet run
```

### Frontend (Angular)

```bash
cd GeminiFront
npm install
ng serve
```

---
## 📝 Notes

- `appsettings.json` and `appsettings.Development.json` are ignored via `.gitignore` for security.
- **CORS** is configured for `http://localhost:4200` in the backend.
- JWT tokens are stored in localStorage and attached using an HTTP Interceptor.
- Admin/seeding is not implemented in this project since it's meant for testing AI integration only.

---


## 📌 Example Users

> You can register a new user from the UI. No seed admin included in this minimal demo.

---

