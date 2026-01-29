# Build Script for English Training Center LMS
# Purpose: Automated build process for .NET Core 8.0 project
# Usage: .\build.ps1 -Configuration Release -Verbose

param(
    [ValidateSet("Debug", "Release")]
    [string]$Configuration = "Release",
    [switch]$NoRestore,
    [switch]$NoBuild,
    [switch]$Publish,
    [switch]$Verbose
)

# Color output functions
function Write-Success {
    Write-Host $args -ForegroundColor Green
}

function Write-Error {
    Write-Host "❌ ERROR: " -ForegroundColor Red -NoNewline
    Write-Host $args -ForegroundColor Yellow
}

function Write-Warning {
    Write-Host "⚠️  WARNING: " -ForegroundColor Yellow -NoNewline
    Write-Host $args -ForegroundColor White
}

function Write-Info {
    Write-Host "ℹ️  INFO: " -ForegroundColor Cyan -NoNewline
    Write-Host $args -ForegroundColor White
}

# Check .NET SDK
Write-Info "Checking .NET 8.0 SDK..."
$dotnetVersion = dotnet --version
if (-not $dotnetVersion.StartsWith("8.")) {
    Write-Error ".NET 8.0 SDK is required. Current version: $dotnetVersion"
    exit 1
}
Write-Success ".NET SDK version: $dotnetVersion"

# Navigate to project root
$projectRoot = Split-Path -Parent $MyInvocation.MyCommand.Definition
$projectRoot = Split-Path -Parent $projectRoot
$solutionFile = Join-Path $projectRoot "EnglishTrainingCenter.sln"

if (-not (Test-Path $solutionFile)) {
    Write-Error "Solution file not found at: $solutionFile"
    exit 1
}

Write-Info "Project root: $projectRoot"
Write-Info "Configuration: $Configuration"

# Step 1: Clean
Write-Host "`n🔧 Step 1: Cleaning previous builds..." -ForegroundColor Magenta
dotnet clean $solutionFile -c $Configuration
if ($LASTEXITCODE -ne 0) {
    Write-Error "Clean failed"
    exit 1
}
Write-Success "Clean completed"

# Step 2: Restore
if (-not $NoRestore) {
    Write-Host "`n📦 Step 2: Restoring NuGet packages..." -ForegroundColor Magenta
    dotnet restore $solutionFile
    if ($LASTEXITCODE -ne 0) {
        Write-Error "Restore failed"
        exit 1
    }
    Write-Success "Restore completed"
} else {
    Write-Warning "Restore skipped"
}

# Step 3: Build
if (-not $NoBuild) {
    Write-Host "`n🔨 Step 3: Building solution..." -ForegroundColor Magenta
    $buildArgs = @(
        "build",
        $solutionFile,
        "-c", $Configuration,
        "--no-restore"
    )
    
    if ($Verbose) {
        $buildArgs += "--verbosity", "detailed"
    }
    
    dotnet @buildArgs
    if ($LASTEXITCODE -ne 0) {
        Write-Error "Build failed"
        exit 1
    }
    Write-Success "Build completed"
} else {
    Write-Warning "Build skipped"
}

# Step 4: Publish (optional)
if ($Publish) {
    Write-Host "`n📤 Step 4: Publishing application..." -ForegroundColor Magenta
    $publishDir = Join-Path $projectRoot "publish"
    
    dotnet publish $solutionFile `
        -c $Configuration `
        -o $publishDir `
        --no-build `
        --no-restore
    
    if ($LASTEXITCODE -ne 0) {
        Write-Error "Publish failed"
        exit 1
    }
    Write-Success "Publish completed to: $publishDir"
}

Write-Host "`n" 
Write-Success "✅ Build process completed successfully!"
Write-Info "Summary:"
Write-Info "  - Configuration: $Configuration"
Write-Info "  - Solution: $solutionFile"
if ($Publish) {
    Write-Info "  - Published to: $(Join-Path $projectRoot "publish")"
}

exit 0
