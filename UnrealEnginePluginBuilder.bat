REM ========================================
REM EDITABLE PATHS - Edit these sections
REM ========================================

REM Plugin name (.uplugin file name without extension)
set "PLUGIN_NAME=CapabilityDetect"

REM Engine directory
set "ENGINE_DIR=D:\Program Files\Epic Games\UE_5.7"

REM Plugin directory path
set "PLUGIN_PATH=D:\GithubRepos\UnrealCapabilityDetectExtended\Plugins\%PLUGIN_NAME%"

REM Build output directory
set "BUILD_OUTPUT=D:\GithubRepos\UnrealCapabilityDetectExtended\UnrealEnginePluginBuilder\%PLUGIN_NAME%"

REM Build.cs file path for auto-update (in build output, not original)
set "BUILD_CS_PATH=%BUILD_OUTPUT%\Source\%PLUGIN_NAME%\%PLUGIN_NAME%.Build.cs"

@echo off
echo ========================================
echo    %PLUGIN_NAME% Plugin Builder
echo ========================================
echo.

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

start "Plugin Build" cmd /k ""%ENGINE_DIR%\Engine\Build\BatchFiles\RunUAT.bat" BuildPlugin -Plugin="%PLUGIN_PATH%\%PLUGIN_NAME%.uplugin" -Package="%BUILD_OUTPUT%" -TargetPlatforms=Win64 -Rocket -precompile"

REM Wait for build to complete (check if output files exist)
echo Waiting for build to complete...
:WAIT_LOOP
timeout /t 10 /nobreak >nul
if exist "%BUILD_OUTPUT%\Source\%PLUGIN_NAME%\%PLUGIN_NAME%.Build.cs" (
    echo Build completed! Starting post-build operations...
    goto POST_BUILD
) else (
    echo Still waiting for build to complete...
    goto WAIT_LOOP
)

:POST_BUILD
REM ========================================
REM Result check and auto-update
REM ========================================
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
    
    REM Update bPrecompile to bUsePrecompiled
    echo Replacing bPrecompile=true with bUsePrecompiled=true...
    
    REM Use PowerShell to perform the replacement (fixed command)
    powershell -Command "$content = Get-Content '%BUILD_CS_PATH%'; $content = $content -replace 'bPrecompile = true;', 'bUsePrecompiled = true;'; Set-Content '%BUILD_CS_PATH%' -Value $content"
    
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
    echo.
    echo Plugin is ready for use!
    echo.
    echo Press any key to close this window...
    pause >nul
    exit /b 0
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
    echo.
    echo Press any key to close this window...
    pause >nul
    exit /b 1
)
