# BudgetTracker

Tracks spending across your apps via notifications — aggregate all your transactions in one place.  
Uses app/bank notifications (incoming & outgoing) to record money flows, so you can see your base spending/earning, weekly trends, and what categories you spend most/least on monthly.

**This is still in the initial stages of development**

---

## ✨ Features

- 📲 Automatic capture of spend/earn notifications  
- 📊 Weekly and monthly summaries of income vs expenses  
- 🗂️ Highlight highest/lowest spending categories per month  
- 💰 Simple budget overviews  
- 🔌 Extendable support for multiple apps/accounts  

---

## 🛠️ Tech Stack

*(Adjust based on what the repo actually uses)*  

- **C# / .NET**  
- **.NET MAUI** (cross-platform UI)  
- **REST API** backend  
- **Firebase Realtime Database**  
- **Docker** (if applicable)  

---

## 🚀 Getting Started

These instructions will help you run a local copy for development and testing.

### ✅ Prerequisites

- .NET SDK (version X or higher)  
- Node.js (if backend used)  
- Firebase credentials (or other DB)  
- (Optional) Docker  

### ⚙️ Installation

1. Clone the repository  
   ```bash
   git clone https://github.com/daylinda/BudgetTracker.git
   cd BudgetTracker
   ```
2. Run the application
   ```bash
   dotnet run
   ```
   Or open in Visual Studio / VS Code and run directly.
   
3. (If applicable) Start backend services
   ```bash
   npm run dev
   ```
   or
   ```bash
   dotnet run
   ```

## ⚙️ Setup Environment / Configuration

- Place your Firebase/database connection keys in the appropriate config/secrets file  
- Update `.env` or settings file if provided  

### Restore dependencies & build

```bash
dotnet restore
dotnet build
```

## 📖 Usage

Once running, you can:

- Receive notifications capturing transaction details  
- View dashboards summarising spending/income  
- Filter by date ranges  
- See category breakdowns  
- Compare actual spending vs budget  
- Identify highest and lowest spending categories monthly  

---

## 📂 Project Structure

```plaintext
BudgetTracker/
├── .gitignore
├── README.md
├── Tracker/           ← MAUI / client app
│   ├── src/
│   ├── Resources/
│   └── ...
├── Backend/           ← REST API / server logic (if separate)
│   ├── Controllers/
│   ├── Models/
│   ├── Services/
│   └── ...
├── docker-compose.yml (if used)
└── ...
```

---

## 🤝 Contributing

Contributions are welcome!

1. Fork the repository  
2. Create a feature branch  
   ```bash
   git checkout -b feature-name
   ```
3. Make your changes  
4. Commit with descriptive messages  
5. Push to your fork and open a Pull Request  

Please follow the existing code style, add tests where possible, and document your changes.

---

## 🗺️ Roadmap & Future Enhancements

- Support for multiple currencies  
- Richer analytics and trend visualisations  
- Dark mode / theming  
- Recurring transaction detection  
- Alerts for exceeding budgets  
- Export data (CSV, JSON)  
- Syncing/backup across devices  
- Mobile/wearable integration  

---

## 📜 License

This project is licensed under the **MIT License**.  
See the [LICENSE](LICENSE) file for details.
