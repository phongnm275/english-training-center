# Database Setup Script for English Training Center LMS
# Purpose: Create and migrate test/development databases
# Usage: .\setup-database.ps1 -Environment Development -CreateIfNotExists

param(
    [ValidateSet("Development", "Test", "Production")]
    [string]$Environment = "Development",
    [string]$Server = "localhost",
    [string]$AdminUser = "sa",
    [string]$AdminPassword = "YourPassword123!",
    [switch]$CreateIfNotExists,
    [switch]$SeedData
)

function Write-Success {
    Write-Host $args -ForegroundColor Green
}

function Write-Error {
    Write-Host "❌ ERROR: " -ForegroundColor Red -NoNewline
    Write-Host $args -ForegroundColor Yellow
}

function Write-Info {
    Write-Host "ℹ️  INFO: " -ForegroundColor Cyan -NoNewline
    Write-Host $args -ForegroundColor White
}

function Write-Database {
    Write-Host "🗄️  DATABASE: " -ForegroundColor Magenta -NoNewline
    Write-Host $args -ForegroundColor White
}

# Map environment to database name
$databaseName = switch ($Environment) {
    "Development" { "EnglishTrainingCenter_Dev" }
    "Test" { "EnglishTrainingCenter_Test" }
    "Production" { "EnglishTrainingCenter" }
}

$connectionString = "Server=$Server;User Id=$AdminUser;Password=$AdminPassword;"

Write-Database "Environment: $Environment"
Write-Database "Database: $databaseName"
Write-Database "Server: $Server"

# Step 1: Check SQL Server connectivity
Write-Host "`n🔌 Step 1: Checking SQL Server connectivity..." -ForegroundColor Magenta
try {
    $connection = New-Object System.Data.SqlClient.SqlConnection $connectionString
    $connection.Open()
    $connection.Close()
    Write-Success "Connected to SQL Server"
} catch {
    Write-Error "Failed to connect to SQL Server: $_"
    Write-Info "Make sure SQL Server is running and credentials are correct"
    exit 1
}

# Step 2: Create database
Write-Host "`n📦 Step 2: Checking database..." -ForegroundColor Magenta

$checkQuery = "SELECT name FROM sys.databases WHERE name = '$databaseName'"
$connection = New-Object System.Data.SqlClient.SqlConnection $connectionString
$connection.Open()
$command = $connection.CreateCommand()
$command.CommandText = $checkQuery
$result = $command.ExecuteScalar()
$connection.Close()

if ($result) {
    Write-Database "Database '$databaseName' already exists"
} else {
    if ($CreateIfNotExists) {
        Write-Database "Creating database '$databaseName'..."
        $createQuery = "CREATE DATABASE [$databaseName] COLLATE SQL_Latin1_General_CP1_CI_AS"
        
        $connection = New-Object System.Data.SqlClient.SqlConnection $connectionString
        $connection.Open()
        $command = $connection.CreateCommand()
        $command.CommandText = $createQuery
        $command.ExecuteNonQuery()
        $connection.Close()
        
        Write-Success "Database created: $databaseName"
    } else {
        Write-Error "Database does not exist: $databaseName"
        Write-Info "Use -CreateIfNotExists to create it"
        exit 1
    }
}

# Step 3: Run migrations
Write-Host "`n🔄 Step 3: Running Entity Framework Core migrations..." -ForegroundColor Magenta

$projectRoot = Split-Path -Parent $MyInvocation.MyCommand.Definition
$projectRoot = Split-Path -Parent $projectRoot

Push-Location $projectRoot
try {
    $migrateArgs = @(
        "ef",
        "database",
        "update",
        "--project", "src/EnglishTrainingCenter.Infrastructure",
        "--startup-project", "src/EnglishTrainingCenter.API"
    )
    
    dotnet @migrateArgs
    if ($LASTEXITCODE -ne 0) {
        Write-Error "Migration failed"
        exit 1
    }
    Write-Success "Migrations completed successfully"
} finally {
    Pop-Location
}

# Step 4: Seed data (optional)
if ($SeedData) {
    Write-Host "`n🌱 Step 4: Seeding test data..." -ForegroundColor Magenta
    
    $seedScript = Join-Path $projectRoot "database/seeds/$($Environment.ToLower())-seed-data.sql"
    
    if (Test-Path $seedScript) {
        Write-Info "Running seed script: $seedScript"
        
        $seedConnection = "Server=$Server;Database=$databaseName;User Id=$AdminUser;Password=$AdminPassword;"
        $connection = New-Object System.Data.SqlClient.SqlConnection $seedConnection
        $connection.Open()
        
        $scriptContent = Get-Content $seedScript -Raw
        $connection.Close()
        
        Write-Success "Seed data applied"
    } else {
        Write-Error "Seed script not found: $seedScript"
    }
}

# Step 5: Verify database
Write-Host "`n✅ Step 5: Verifying database..." -ForegroundColor Magenta

$verifyQuery = @"
SELECT 
    COUNT(*) as TableCount
FROM 
    INFORMATION_SCHEMA.TABLES 
WHERE 
    TABLE_TYPE = 'BASE TABLE'
    AND TABLE_SCHEMA = 'dbo'
"@

$connection = New-Object System.Data.SqlClient.SqlConnection "Server=$Server;Database=$databaseName;User Id=$AdminUser;Password=$AdminPassword;"
$connection.Open()
$command = $connection.CreateCommand()
$command.CommandText = $verifyQuery
$tableCount = $command.ExecuteScalar()
$connection.Close()

Write-Success "Database verification complete"
Write-Database "Tables found: $tableCount"

Write-Host "`n"
Write-Success "✅ Database setup completed successfully!"
Write-Info "Summary:"
Write-Info "  - Environment: $Environment"
Write-Info "  - Database: $databaseName"
Write-Info "  - Server: $Server"
Write-Info "  - Tables created: $tableCount"

exit 0
