# Data Import Utility

This directory contains a Python script to import data from `Address.csv` and `SalesOrderHeader.csv` into a SQL Server database (`BellBI`).

## Prerequisites

1. **Python 3.8+**
2. **SQL Server instance** running (e.g., via Docker on port `1433`).

---

## Step 1: Database Setup

Before importing the data, drop the existing tables (if any) and create the tables with the correct column schemas (using `DATE` instead of `DATETIME` to prevent unwanted time suffixes).

Run the following SQL commands in your database:

```sql
USE BellBI;
GO

-- 1. Drop existing tables (if they exist, child table first due to foreign keys)
DROP TABLE IF EXISTS SalesOrderHeader;
DROP TABLE IF EXISTS Address;
GO

-- 2. Create Address table
CREATE TABLE Address (
    AddressID INT PRIMARY KEY,
    AddressLine1 VARCHAR(255) NOT NULL,
    City VARCHAR(100) NOT NULL
);
GO

-- 3. Create SalesOrderHeader table (using DATE to avoid default '00:00:00.000' time suffix)
CREATE TABLE SalesOrderHeader (
    SalesOrderID INT PRIMARY KEY,
    OrderDate DATE NOT NULL,
    ActualShipDate DATE NULL,
    CustomerID INT NOT NULL,
    ShipToAddressID INT NOT NULL,
    TotalDue DECIMAL(18, 4) NOT NULL,
    FOREIGN KEY (ShipToAddressID) REFERENCES Address(AddressID)
);
GO
```

---

## Step 2: Environment Configuration

The script reads database configuration from a `.env` file in the project root directory. Use the provided `.env.example` template to set up your environment:

1. Copy `.env.example` to `.env` in the project root directory:
   ```bash
   cp ../.env.example ../.env
   ```

2. Open the newly created `../.env` file and update the variables (especially `MSSQL_SA_PASSWORD`) with your actual SQL Server credentials.


---

## Step 3: Run the Import Script

1. Navigate to the `SQL data` directory:
   ```bash
   cd "SQL data"
   ```

2. Create and activate a virtual environment:
   ```bash
   python3 -m venv .venv
   source .venv/bin/activate
   ```

3. Install the dependencies:
   ```bash
   pip install -r requirements.txt
   ```

4. Place your CSV data files (`Address.csv` and `SalesOrderHeader.csv`) in the `SQL data` directory.

5. Execute the import script:
   ```bash
   python import_data.py
   ```

Upon success, you should see:
```text
importing Address.csv...
Address imported successfully!
importing SalesOrderHeader.csv...
SalesOrderHeader imported successfully!
```
