// Copyright 1998-2017 Epic Games, Inc. All Rights Reserved.

using UnrealBuildTool;

public class CapabilityDetect : ModuleRules
{
	public CapabilityDetect(ReadOnlyTargetRules Target) : base(Target)
	{
		PCHUsage = ModuleRules.PCHUsageMode.UseExplicitOrSharedPCHs;
		
		// UE 5.4 compatibility - use Latest instead of specific version
		IncludeOrderVersion = EngineIncludeOrderVersion.Latest;
		
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
				"Projects",
                "SynthBenchmark"
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
		}
		
		// UE 5.4 compatibility - ensure proper module loading
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
