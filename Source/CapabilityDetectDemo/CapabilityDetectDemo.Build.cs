/////////////////////////////////////////////////////////////////////////////////////////////
// Copyright 2017 Intel Corporation
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or imlied.
// See the License for the specific language governing permissions and
// limitations under the License.
/////////////////////////////////////////////////////////////////////////////////////////////

using System.IO;
using UnrealBuildTool;

public class CapabilityDetectDemo : ModuleRules
{
	public CapabilityDetectDemo(ReadOnlyTargetRules Target) : base(Target)
	{
		PCHUsage = PCHUsageMode.UseExplicitOrSharedPCHs;
	
		PublicDependencyModuleNames.AddRange(new string[] { "Core", "CoreUObject", "Engine", "InputCore" });

        string BaseDirectory = Path.GetFullPath(Path.Combine(ModuleDirectory, "..", ".."));
        string PluginDirectory = Path.Combine(BaseDirectory, "Plugins", "CapabilityDetect", "Binaries", "ThirdParty", "CapabilityDetectLibrary", "Win64");
        
        if (Target.Platform == UnrealTargetPlatform.Win64)
        {
	        // Add the required DLL as a runtime dependency
	        string dllPath = Path.Combine(PluginDirectory, "CapabilityDetectLibrary.dll");

	        if (File.Exists(dllPath))
	        {
		        RuntimeDependencies.Add(dllPath);
		        PublicDelayLoadDLLs.Add("CapabilityDetectLibrary.dll");
		        //PublicAdditionalLibraries.Add(DllPath); // If you need the static library as well
	        }
	        else
	        {
		        System.Console.WriteLine("DLL not found at path: " + dllPath);
	        }
        }

        // Uncomment if you are using Slate UI
        // PrivateDependencyModuleNames.AddRange(new string[] { "Slate", "SlateCore" });

        // Uncomment if you are using online features
        // PrivateDependencyModuleNames.Add("OnlineSubsystem");

        // To include OnlineSubsystemSteam, add it to the plugins section in your uproject file with the Enabled attribute set to true
    }
}
