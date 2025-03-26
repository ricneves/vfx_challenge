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
   git clone https://github.com/ricneves/vfx_challenge.git
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
   - **GET** `/api/exchangerates/{FromCurrency}/{ToCurrency}?date={YYYY-MM-DD}`
   - **Parameters:**
     - `FromCurrency` (string) - Source currency code (e.g., USD)
     - `ToCurrency` (string) - Target currency code (e.g., EUR)
     - `date` (optional, format: YYYY-MM-DD) - Exchange rate date. If omitted, returns the latest available rate.

   - **Response:**
     ```json
     {
       "fromCurrency": "USD",
       "toCurrency": "EUR",
       "bidPrice": 1.085,
       "askPrice": 1.090,
       "date": "2025-03-26T00:00:00"
     }
     ```

   - **Behavior:**
     - If the exchange rate is not available in the database, the API will attempt to fetch it from Alpha Vantage and store it.

   - **Possible HTTP Status Codes:**
     - `200 OK` - Success
     - `400 Bad Request` - If the parameters are invalid
     - `404 Not Found` - If the exchange rate is not found
     - `500 Internal Server Error` - If there is a failure when fetching the rate externally

   - **Example Requests:**
     ```
     GET /api/exchangerates/USD/EUR
     GET /api/exchangerates/USD/EUR?date=2025-03-26
     ```

### 2. Add Exchange Rate
   - **POST** `/api/exchangerates`
   - **Request Body:**
     ```json
     {
       "fromCurrency": "USD",
       "toCurrency": "GBP",
       "bidPrice": 1.320,
       "askPrice": 1.325,
	   "date": "2025-03-26T00:00:00"
     }
     ```

### 3. Update Exchange Rate
   - **PATCH** `/api/exchangerates/{id}`
   - **Request Body:** 
	 ```json
     {
       "id": 1,
       "bidPrice": 1.320,
       "askPrice": 1.325
     }
     ```

### 4. Delete Exchange Rate
   - **DELETE** `/api/exchangerates/{id}`


