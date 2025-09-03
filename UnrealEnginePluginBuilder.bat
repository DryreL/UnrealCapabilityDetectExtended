@echo off
echo ========================================
echo    %PLUGIN_NAME% Plugin Builder
echo ========================================
echo.

REM ========================================
REM EDITABLE PATHS - Edit these sections
REM ========================================

REM Engine directory (for UE 5.4)
set "ENGINE_DIR=D:\Program Files\Epic Games\UE_5.4"

REM Plugin directory path
set "PLUGIN_PATH=D:\GithubRepos\UnrealCapabilityDetectExtended\Plugins\%PLUGIN_NAME%"

REM Plugin name (.uplugin file name without extension)
set "PLUGIN_NAME=CapabilityDetect"

REM Build output directory
set "BUILD_OUTPUT=D:\GithubRepos\UnrealCapabilityDetectExtended\UnrealEnginePluginBuilder\%PLUGIN_NAME%"

REM Build.cs file path for auto-update
set "BUILD_CS_PATH=%PLUGIN_PATH%\Source\%PLUGIN_NAME%\%PLUGIN_NAME%.Build.cs"

REM ========================================
REM Automatic checks
REM ========================================

echo Performing checks...
echo.

REM Engine directory check
if not exist "%ENGINE_DIR%\Engine\Build\BatchFiles\RunUAT.bat" (
    echo ERROR: Engine directory not found!
    echo Please check ENGINE_DIR path: %ENGINE_DIR%
    echo.
    pause
    exit /b 1
)

REM Plugin file check
if not exist "%PLUGIN_PATH%\%PLUGIN_NAME%.uplugin" (
    echo ERROR: Plugin file not found!
    echo Please check PLUGIN_PATH: %PLUGIN_PATH%
    echo.
    pause
    exit /b 1
)

REM Build.cs file check
if not exist "%BUILD_CS_PATH%" (
    echo ERROR: Build.cs file not found!
    echo Please check BUILD_CS_PATH: %BUILD_CS_PATH%
    echo.
    pause
    exit /b 1
)

REM Create build output directory
if not exist "%BUILD_OUTPUT%" (
    echo Creating build output directory: %BUILD_OUTPUT%
    mkdir "%BUILD_OUTPUT%"
)

echo All checks passed successfully!
echo.

REM ========================================
REM Build command
REM ========================================

echo Building plugin...
echo.
echo Engine: %ENGINE_DIR%
echo Plugin: %PLUGIN_PATH%\%PLUGIN_NAME%.uplugin
echo Output: %BUILD_OUTPUT%
echo Build.cs: %BUILD_CS_PATH%
echo.

echo Executing build command...
echo.

"%ENGINE_DIR%\Engine\Build\BatchFiles\RunUAT.bat" BuildPlugin -Plugin="%PLUGIN_PATH%\%PLUGIN_NAME%.uplugin" -Package="%BUILD_OUTPUT%" -TargetPlatforms=Win64 -Rocket -precompile

REM ========================================
REM Result check and auto-update
REM ========================================

if %ERRORLEVEL% EQU 0 (
    echo.
    echo ========================================
    echo    BUILD SUCCESSFUL!
    echo ========================================
    echo.
    echo Plugin built successfully!
    echo Output directory: %BUILD_OUTPUT%
    echo.
    
    REM ========================================
    REM Auto-update Build.cs file
    REM ========================================
    echo Updating Build.cs file...
    echo.
    
    REM Create backup of original file
    set "BACKUP_FILE=%BUILD_CS_PATH%.backup"
    copy "%BUILD_CS_PATH%" "%BACKUP_FILE%" >nul
    echo Backup created: %BACKUP_FILE%
    
    REM Update bPrecompile to bUsePrecompiled
    echo Replacing bPrecompile=true with bUsePrecompiled=true...
    
    REM Use PowerShell to perform the replacement
    powershell -Command "(Get-Content '%BUILD_CS_PATH%') -replace 'bPrecompile = true;', 'bUsePrecompiled = true;' | Set-Content '%BUILD_CS_PATH%'"
    
    if %ERRORLEVEL% EQU 0 (
        echo Build.cs file updated successfully!
        echo Changed: bPrecompile = true; -> bUsePrecompiled = true;
    ) else (
        echo WARNING: Failed to update Build.cs file automatically.
        echo Please manually change bPrecompile = true; to bUsePrecompiled = true;
    )
    
    echo.
    echo ========================================
    echo    AUTO-UPDATE COMPLETED!
    echo ========================================
    echo.
    echo Next steps:
    echo 1. Build.cs file has been automatically updated
    echo 2. Plugin is now ready for precompiled use
    echo 3. Copy plugin to Engine directory
    echo 4. Remove plugin from project directory
    echo.
    echo Build and update completed successfully!
    echo CMD window will remain open for you to review.
    echo.
    echo Press any key to close this window...
    pause >nul
) else (
    echo.
    echo ========================================
    echo    BUILD FAILED!
    echo ========================================
    echo.
    echo Error code: %ERRORLEVEL%
    echo Please check error messages above.
    echo.
    echo Build failed! No changes made to Build.cs file.
    echo CMD window will remain open for you to review errors.
    echo.
    echo Press any key to close this window...
    pause >nul
)
