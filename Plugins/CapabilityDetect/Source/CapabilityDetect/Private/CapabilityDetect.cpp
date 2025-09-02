// Copyright 1998-2017 Epic Games, Inc. All Rights Reserved.

#include "CapabilityDetect.h"
#include "Core.h"
#include "Modules/ModuleManager.h"
#include "Interfaces/IPluginManager.h"
#include "ThirdParty/CapabilityDetectLibrary/CapabilityDetectLibrary.h"

#define LOCTEXT_NAMESPACE "FCapabilityDetectModule"

void FCapabilityDetectModule::StartupModule()
{
	// This code will execute after your module is loaded into memory; the exact timing is specified in the .uplugin file per-module

	// Add on the relative location of the third party dll and load it
	FString LibraryPath;
#if PLATFORM_WINDOWS
	// Try multiple possible paths for DLL loading
	
	// PATH 1: Project's main Binaries folder (for shipping builds)
	FString ProjectRoot = FPaths::ProjectDir();
	FString ProjectBinariesPath = FPaths::Combine(*ProjectRoot, TEXT("Binaries"), TEXT("Win64"));
	FString ProjectDllPath = FPaths::Combine(*ProjectBinariesPath, TEXT("CapabilityDetectLibrary.dll"));
	
	// PATH 2: Plugin's own Binaries folder (for plugin loading)
	FString PluginBaseDir = IPluginManager::Get().FindPlugin("CapabilityDetect")->GetBaseDir();
	FString PluginBinariesPath = FPaths::Combine(*PluginBaseDir, TEXT("Binaries"), TEXT("Win64"));
	FString PluginDllPath = FPaths::Combine(*PluginBinariesPath, TEXT("CapabilityDetectLibrary.dll"));
	
	// Log all paths for debugging
	UE_LOG(LogTemp, Log, TEXT("CapabilityDetect: Project root: %s"), *ProjectRoot);
	UE_LOG(LogTemp, Log, TEXT("CapabilityDetect: Project Binaries path: %s"), *ProjectBinariesPath);
	UE_LOG(LogTemp, Log, TEXT("CapabilityDetect: Plugin base dir: %s"), *PluginBaseDir);
	UE_LOG(LogTemp, Log, TEXT("CapabilityDetect: Plugin Binaries path: %s"), *PluginBinariesPath);
	
	// Try to find DLL in either location
	if (FPaths::FileExists(ProjectDllPath))
	{
		LibraryPath = ProjectDllPath;
		UE_LOG(LogTemp, Log, TEXT("CapabilityDetect: DLL found in project Binaries: %s"), *LibraryPath);
	}
	else if (FPaths::FileExists(PluginDllPath))
	{
		LibraryPath = PluginDllPath;
		UE_LOG(LogTemp, Log, TEXT("CapabilityDetect: DLL found in plugin Binaries: %s"), *LibraryPath);
	}
	else
	{
		// DLL not found in either location
		FString ErrorMessage = FString::Printf(TEXT("CapabilityDetect: DLL file not found in either location:\nProject: %s\nPlugin: %s"), *ProjectDllPath, *PluginDllPath);
		UE_LOG(LogTemp, Error, TEXT("%s"), *ErrorMessage);
		
		// Show error dialog in editor builds
	#if WITH_EDITOR
		FMessageDialog::Open(EAppMsgType::Ok, FText::FromString(ErrorMessage));
	#endif
		
		return; // Exit early if DLL doesn't exist
	}
	
	UE_LOG(LogTemp, Log, TEXT("CapabilityDetect: Attempting to load DLL from: %s"), *LibraryPath);
#endif // PLATFORM_WINDOWS

	CapabilityDetectLibraryHandle = !LibraryPath.IsEmpty() ? FPlatformProcess::GetDllHandle(*LibraryPath) : nullptr;

	if (CapabilityDetectLibraryHandle)
	{
		UE_LOG(LogTemp, Log, TEXT("CapabilityDetect: DLL loaded successfully from: %s"), *LibraryPath);
		Intel_InitializeResources();
	}
	else
	{
		// Log the error but don't show dialog in shipping builds
		FString ErrorMessage = FString::Printf(TEXT("CapabilityDetect: Failed to load capability detect library from: %s"), *LibraryPath);
		UE_LOG(LogTemp, Warning, TEXT("%s"), *ErrorMessage);
		
		// Only show dialog in editor builds
	#if WITH_EDITOR
		FMessageDialog::Open(EAppMsgType::Ok, FText::FromString(ErrorMessage));
	#endif
	}
}

void FCapabilityDetectModule::ShutdownModule()
{
	// This function may be called during shutdown to clean up your module.  For modules that support dynamic reloading,
	// we call this function before unloading the module.
	
	Intel_FreeResources();

	// Free the dll handle
	FPlatformProcess::FreeDllHandle(CapabilityDetectLibraryHandle);
	CapabilityDetectLibraryHandle = nullptr;
}

#undef LOCTEXT_NAMESPACE
	
IMPLEMENT_MODULE(FCapabilityDetectModule, CapabilityDetect)