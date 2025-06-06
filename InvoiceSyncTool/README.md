# InvoiceSyncTool

This is a simplified version of a backend tool I built for a small business. It was used to automate invoice processing and integration with an external accounting platform. The software ran in production for over 2 years until the company closed operations in 2025, after which I obtained permission to share a sanitized version for demonstration purposes.

### Features

- Loads invoice and vendor data via EF Core
- Converts invoices to the accounting platform's expected format
- Validates invoice duplication via remote API
- Posts validated invoices and tracks status
- Config-driven with support for multiple environments

### Technologies Used

- .NET Core
- Entity Framework Core
- Configuration via `appsettings.json`
- IntacctSDK integration

### Notes

- No production data is included.
- Credentials and environment-specific data have been stripped for safety.
