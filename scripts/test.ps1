# Test Script for English Training Center LMS
# Purpose: Run unit, integration, and API tests
# Usage: .\test.ps1 -TestType All -Verbose -Coverage

param(
    [ValidateSet("Unit", "Integration", "All")]
    [string]$TestType = "Unit",
    [switch]$Coverage,
    [switch]$Verbose,
    [switch]$OpenReport
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

function Write-Test {
    Write-Host "🧪 TEST: " -ForegroundColor Magenta -NoNewline
    Write-Host $args -ForegroundColor White
}

# Get project root
$projectRoot = Split-Path -Parent $MyInvocation.MyCommand.Definition
$projectRoot = Split-Path -Parent $projectRoot

Write-Info "Project root: $projectRoot"
Write-Test "Running $TestType tests..."

# Prepare test command arguments
$testArgs = @("test")

# Add coverage if requested
if ($Coverage) {
    Write-Test "Code coverage enabled"
    $testArgs += @(
        "/p:CollectCoverage=true",
        "/p:CoverageFormat=opencover",
        "/p:ExcludeByFile=**/Migrations/*"
    )
}

# Add verbosity
if ($Verbose) {
    $testArgs += "--logger", "console;verbosity=detailed"
} else {
    $testArgs += "--logger", "console;verbosity=normal"
}

# Add test type filters
switch ($TestType) {
    "Unit" {
        Write-Test "Running Unit Tests"
        $testArgs += "tests/EnglishTrainingCenter.Tests.Unit/EnglishTrainingCenter.Tests.Unit.csproj"
    }
    "Integration" {
        Write-Test "Running Integration Tests"
        if (Test-Path "tests/EnglishTrainingCenter.Tests.Integration") {
            $testArgs += "tests/EnglishTrainingCenter.Tests.Integration/EnglishTrainingCenter.Tests.Integration.csproj"
        } else {
            Write-Error "Integration test project not found"
            exit 1
        }
    }
    "All" {
        Write-Test "Running All Tests (Unit + Integration)"
        # dotnet test will discover all test projects
    }
}

Write-Host "`n"

# Run tests
Push-Location $projectRoot
try {
    dotnet @testArgs
    $exitCode = $LASTEXITCODE
} finally {
    Pop-Location
}

# Handle results
if ($exitCode -eq 0) {
    Write-Success "✅ All tests passed!"
} else {
    Write-Error "Some tests failed (Exit code: $exitCode)"
}

# Open coverage report if requested
if ($Coverage -and $OpenReport) {
    $coverageFile = Get-ChildItem -Path $projectRoot -Filter "coverage.opencover.xml" -Recurse | Select-Object -First 1
    if ($coverageFile) {
        Write-Info "Opening coverage report: $($coverageFile.FullName)"
        # Note: Requires ReportGenerator to be installed
        # reportgenerator -reports:$($coverageFile.FullName) -targetdir:coverage-report -reporttypes:HtmlInline
    }
}

exit $exitCode
