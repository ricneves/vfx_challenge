# ExchangeRates API

## Overview
The **ExchangeRates API** is a .NET 8 Web API that provides CRUD operations for managing foreign exchange rates. Each currency pair (e.g., USD/EUR) contains two prices: **bid** and **ask**.

If a requested exchange rate is not available in the database, the API retrieves the data from a third-party service ([Alpha Vantage](https://www.alphavantage.co)) and stores it for future requests.

## Features
- **CRUD operations** for managing exchange rates.
- **Real-time data fetching** from Alpha Vantage when data is not available in the database.
- **Efficient caching** of retrieved exchange rates to optimize performance.
- **RESTful API design** following Clean Architecture principles.
- **SQL Server with Entity Framework Core** for data persistence.

## Technologies Used
- **.NET 8** (ASP.NET Core Web API)
- **Entity Framework Core** (for database interaction)
- **SQL Server** (for data storage)
- **Alpha Vantage API** (for real-time exchange rates)
- **Dependency Injection** (for maintainability and testability)

## Installation
### Prerequisites
- [.NET SDK 8](https://dotnet.microsoft.com/en-us/download)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)
- Alpha Vantage API Key (Sign up [here](https://www.alphavantage.co/support/#api-key))

### Steps
1. Clone the repository:
   ```sh
   git clone https://github.com/seu-usuario/exchangerates-api.git
   cd exchangerates-api
   ```
2. Configure the **appsettings.json**:
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Server=your_server;Database=ExchangeRatesDB;User Id=your_user;Password=your_password;"
     },
     "AlphaVantage": {
       "ApiKey": "your_alpha_vantage_api_key"
     }
   }
   ```
3. Apply database migrations:
   ```sh
   dotnet ef database update
   ```
4. Run the application:
   ```sh
   dotnet run
   ```

## API Endpoints
### 1. Get Exchange Rate
   - **GET** `/api/exchangerates/{baseCurrency}/{targetCurrency}`
   - **Response:**
     ```json
     {
       "baseCurrency": "USD",
       "targetCurrency": "EUR",
       "bid": 1.085,
       "ask": 1.090,
       "lastUpdated": "2025-03-24T12:00:00Z"
     }
     ```
   - If not available in the database, it fetches from Alpha Vantage and stores it.

### 2. Add Exchange Rate
   - **POST** `/api/exchangerates`
   - **Request Body:**
     ```json
     {
       "baseCurrency": "USD",
       "targetCurrency": "GBP",
       "bid": 1.320,
       "ask": 1.325
     }
     ```

### 3. Update Exchange Rate
   - **PUT** `/api/exchangerates/{id}`
   - **Request Body:** Similar to POST request

### 4. Delete Exchange Rate
   - **DELETE** `/api/exchangerates/{id}`


