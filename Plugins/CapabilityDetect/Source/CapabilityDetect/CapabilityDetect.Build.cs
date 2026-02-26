// Copyright 1998-2017 Epic Games, Inc. All Rights Reserved.

using UnrealBuildTool;
using System.IO;

public class CapabilityDetect : ModuleRules
{
	public CapabilityDetect(ReadOnlyTargetRules Target) : base(Target)
	{
		PCHUsage = ModuleRules.PCHUsageMode.UseExplicitOrSharedPCHs;
		
		// UE 5 compatibility - use Latest instead of specific version
		IncludeOrderVersion = EngineIncludeOrderVersion.Latest;
		
		// Set Modern capabilities
		bEnableExceptions = false;
		
		// Enable precompiled builds for all targets
		PrecompileForTargets = PrecompileTargetsType.Any;
		bPrecompile = true;
		
		PublicIncludePaths.AddRange(
			new string[] {
				//"CapabilityDetect/Public",
                "Runtime/SynthBenchmark/Public"
				// ... add public include paths required here ...
			}
		);
				
		
		PrivateIncludePaths.AddRange(
			new string[] {
				//"CapabilityDetect/Private",
                //"Runtime/SynthBenchmark/Private"
				// ... add other private include paths required here ...
			}
		);
			
		
		PublicDependencyModuleNames.AddRange(
			new string[]
			{
				"Core",
				"CoreUObject",
				"Projects"
				// ... add other public dependencies that you statically link with here ...
			}
		);
			
		
		PrivateDependencyModuleNames.AddRange(
			new string[]
			{
				// ... add private dependencies that you statically link with here ...	
                "Engine",
                "RHI",
                "ApplicationCore",
                "SynthBenchmark"
            }
		);
		
		// Only add CapabilityDetectLibrary dependency on Windows platform
		if (Target.Platform == UnrealTargetPlatform.Win64)
		{
			PublicDependencyModuleNames.Add("CapabilityDetectLibrary");
			
			// Ensure DLL is copied to packaged game
			// This is required for the plugin to work when installed in Engine
			PublicDelayLoadDLLs.Add("CapabilityDetectLibrary.dll");
			
			// Add runtime dependency for packaging
			// This ensures the DLL is included in the final game package
			RuntimeDependencies.Add("$(TargetOutputDir)/CapabilityDetectLibrary.dll");
			
			// Also add the DLL from the plugin's Binaries folder as fallback
			string PluginBinariesPath = Path.Combine(ModuleDirectory, "..", "..", "Binaries", "Win64");
			string PluginDllPath = Path.Combine(PluginBinariesPath, "CapabilityDetectLibrary.dll");
			
			if (File.Exists(PluginDllPath))
			{
				RuntimeDependencies.Add(PluginDllPath);
				System.Console.WriteLine("CapabilityDetect: Added plugin DLL to runtime dependencies: " + PluginDllPath);
			}
			else
			{
				System.Console.WriteLine("CapabilityDetect: Plugin DLL not found at: " + PluginDllPath);
			}
		}
		
		// UE 5 compatibility - ensure proper module loading
		if (Target.bBuildEditor)
		{
			PrivateDependencyModuleNames.AddRange(new string[] { "UnrealEd" });
		}
		
		DynamicallyLoadedModuleNames.AddRange(
			new string[]
			{
				// ... add any modules that your module loads dynamically here ...
			}
		);
	}
}
