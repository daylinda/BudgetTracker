# Tracker App — Setup & Fix Guide

## What is this?

A two-part solution:
- **`Tracker/`** — .NET MAUI mobile app (Android/iOS/Windows) that displays push notifications
- **`TrackerApp.API/`** — ASP.NET Core 8 Web API that stores/retrieves notifications via Firebase Realtime Database

---

## Bugs Fixed

The following issues were preventing the app from working:

### 1. Deadlock in ViewModel (Critical)
**File:** `Tracker/ViewModel/NotificationViewModel.cs`

The original code called `.Result` on an async Task from the UI thread, causing a deadlock that freezes the app silently.

```csharp
// BROKEN — blocks UI thread, causes deadlock
Notifications = getNotificationsAsync().Result.ToList();

// FIXED — proper async relay command
[RelayCommand]
private async Task GetNotificationsAsync()
{
    var results = await _notificationService.GetNotificationsAsync();
    ...
}
```

### 2. Wrong POST Endpoint URL (Bug)
**File:** `Tracker/Services/NotificationService.cs`

The MAUI service was posting to `"notifications"` but the API controller's route is `/notification`.

```csharp
// BROKEN
await _httpClient.PostAsJsonAsync("notifications", notification);

// FIXED
await _httpClient.PostAsJsonAsync("/notification", notification);
```

### 3. API Crashes on Missing Firebase Key (Critical)
**File:** `TrackerApp.API/Program.cs`

`FirebaseApp.Create()` was called unconditionally. If `Config/firebase-key.json` is missing (which it always is on a fresh clone), the API throws at startup.

**Fixed:** Wrapped in a `File.Exists()` check with a clear console warning.

### 4. List vs ObservableCollection
**File:** `Tracker/ViewModel/NotificationViewModel.cs`

`List<Notification>` doesn't notify the UI when items change. Replaced with `ObservableCollection<Notification>` so the CollectionView updates automatically.

### 5. UserService Not Registered
**File:** `TrackerApp.API/Program.cs`

`UserService` was instantiated in its controller but never added to the DI container, causing a runtime injection error.

**Fixed:** Added `builder.Services.AddSingleton<UserService>();`

### 6. No CORS Policy
**File:** `TrackerApp.API/Program.cs`

The MAUI Android/iOS app would be blocked by CORS when calling the local API. Added a development CORS policy.

---

## Prerequisites

| Tool | Version | Notes |
|------|---------|-------|
| Visual Studio 2022 | 17.8+ | With MAUI and ASP.NET workloads |
| .NET SDK | 8.0 | `dotnet --version` to check |
| Android SDK | API 21+ | Via VS Android Tools |
| Firebase Project | — | Free tier is sufficient |

---

## First-Time Setup

### Step 1 — Firebase Project

1. Go to [console.firebase.google.com](https://console.firebase.google.com)
2. Create a project (or use `trackerapp-b5664` if you already have it)
3. Enable **Realtime Database** → Start in test mode
4. Go to **Project Settings → Service Accounts → Generate new private key**
5. Save the downloaded JSON as:
   ```
   TrackerApp.API/Config/firebase-key.json
   ```
   ⚠️ **Never commit this file to Git.** It is already in `.gitignore`.

### Step 2 — Configure the API

Open `TrackerApp.API/appsettings.json` and verify your Firebase details match:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(local); Database=LoginApiMauiDb; Trusted_Connection=True; Trust Server Certificate=True; MultipleActiveResultSets=True"
  },
  "Firebase": {
    "ProjectId": "YOUR_FIREBASE_PROJECT_ID",
    "DatabaseUrl": "https://YOUR_PROJECT_ID-default-rtdb.YOUR_REGION.firebasedatabase.app/"
  }
}
```

### Step 3 — Run the API

```bash
cd TrackerApp.API
dotnet run
```

The API starts at `https://localhost:7155` (or check `Properties/launchSettings.json` for your port).

Verify it's running by visiting:
- `https://localhost:7155/swagger` — Swagger UI
- `https://localhost:7155/all` — Returns `[]` if no notifications yet

### Step 4 — Configure the MAUI App

Open `Tracker/Config/settings.json` and confirm the URL matches your running API:

```json
{
  "Settings": {
    "ApiBaseUrl": "https://localhost:7155"
  }
}
```

> **Android emulator note:** `localhost` doesn't resolve to your machine from within the Android emulator. Use `10.0.2.2` instead:
> ```json
> { "Settings": { "ApiBaseUrl": "https://10.0.2.2:7155" } }
> ```

### Step 5 — Run the MAUI App

Open `Tracker.sln` in Visual Studio 2022, select your target (Android Emulator, iOS Simulator, or Windows Machine), and press **Run**.

---

## Project Structure

```
Tracker/
├── Tracker/                        ← .NET MAUI App
│   ├── Config/
│   │   ├── Settings.cs             ← Config model
│   │   └── settings.json           ← Embedded API base URL
│   ├── IServices/
│   │   └── INotificationService.cs ← Interface
│   ├── Model/
│   │   └── Notification.cs         ← Data model
│   ├── Services/
│   │   └── NotificationService.cs  ← HTTP client calling the API
│   ├── View/
│   │   └── NotificationView.xaml   ← UI: list of notifications
│   ├── ViewModel/
│   │   ├── MainViewModel.cs
│   │   └── NotificationViewModel.cs← Fetch & display logic
│   └── MauiProgram.cs              ← DI wiring
│
└── TrackerApp.API/                 ← ASP.NET Core API
    ├── Config/
    │   ├── FirebaseSettings.cs
    │   └── firebase-key.json       ← YOU MUST ADD THIS (not in repo)
    ├── Controllers/
    │   ├── NotificationController.cs  GET /all, POST /notification
    │   └── UserController.cs
    ├── Model/
    │   ├── Notification.cs
    │   └── User.cs
    ├── Services/
    │   ├── NotificationService.cs  ← Firebase Realtime DB read/write
    │   └── UserService.cs
    ├── appsettings.json            ← Firebase config + connection string
    └── Program.cs                  ← App startup & DI
```

---

## API Endpoints

| Method | Route | Description |
|--------|-------|-------------|
| GET | `/all` | Returns all notifications from Firebase |
| POST | `/notification` | Saves a new notification to Firebase |

---

## Common Errors & Fixes

| Error | Cause | Fix |
|-------|-------|-----|
| API crashes at startup | `firebase-key.json` missing | Add your service account key to `Config/` |
| `FileNotFoundException: settings.json` | Build action not set | Ensure `settings.json` is `EmbeddedResource` in `.csproj` |
| App hangs on button press | `.Result` deadlock (old code) | Use the fixed async ViewModel |
| Android can't reach API | `localhost` doesn't resolve | Use `10.0.2.2` in `settings.json` for Android emulator |
| `POST` returns 404 | Wrong endpoint path | Fixed — now posts to `/notification` |
| CORS error in browser/emulator | No CORS policy | Fixed — `AllowAll` policy added for development |

---

## What's Not Yet Implemented

- **Authentication** — `UserController` is empty; Firebase Auth is imported but not wired up
- **User-specific notifications** — `UserId` field exists on the model but is not filtered
- **SQL Server** — EF Core + SQL Server packages are present but no `DbContext` is defined
- **Push notifications** — The app receives and displays notifications but doesn't yet register a device token for real push delivery