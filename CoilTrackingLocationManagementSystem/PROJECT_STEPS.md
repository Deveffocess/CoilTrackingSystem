# Coil Tracking Project Step by Step

This starter is based on the requirement name you shared and a practical coil tracing workflow. The PDF text could not be extracted automatically in this environment, so this version uses standard coil-yard modules that we can align to the exact document after you confirm any missing points.

## Step 1: Base Project

- ASP.NET Core MVC project retained from your current solution
- Bootstrap layout redesigned for dashboard, masters, transactions, and reports
- jQuery search and demo form handling added

## Step 2: Master Data

- Create location master
- Create material grade master
- Create supplier master

## Step 3: Transaction Flow

- Inward entry for each received coil
- Assign yard or rack location
- Move coil between locations
- Mark dispatch-ready and complete dispatch

## Step 4: Reports

- Current stock report
- Location-wise stock report
- Coil movement history
- Dispatch traceability

## Step 5: MySQL Database

- Schema added in `Database/coil_tracking_mysql.sql`
- Main tables: `coils`, `storage_locations`, `coil_movements`, `inward_entries`, `dispatch_entries`

## Step 6: Next Coding Task

1. Add EF Core MySQL package
2. Create `DbContext`
3. Replace `InMemoryTrackingRepository` with database repository
4. Save forms using controller POST actions and jQuery AJAX
5. Add authentication and role-based menus

## Suggested MySQL Connection String

```json
"ConnectionStrings": {
  "DefaultConnection": "server=localhost;port=3306;database=coil_tracking_db;user=root;password=yourpassword;"
}
```
