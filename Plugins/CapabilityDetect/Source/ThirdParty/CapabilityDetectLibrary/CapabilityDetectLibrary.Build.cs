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

public class CapabilityDetectLibrary : ModuleRules
{
	public CapabilityDetectLibrary(ReadOnlyTargetRules Target) : base(Target)
	{
		Type = ModuleType.External;

		// UE 5.4 compatibility - use Latest instead of specific version
		IncludeOrderVersion = EngineIncludeOrderVersion.Latest;

		// Only include Windows-specific dependencies
		if (Target.Platform == UnrealTargetPlatform.Win64)
		{
			// Source path where DLL is located
			string SourceLibPath = Path.Combine(ModuleDirectory, "x64", "Release");
			
			// Target path in Binaries folder
			string TargetLibPath = Path.Combine(ModuleDirectory, "..", "..", "..", "..", "Binaries", "ThirdParty", "CapabilityDetectLibrary", "Win64");
			
			if (Directory.Exists(SourceLibPath))
			{
				// Add source path for compilation
				PublicIncludePaths.Add(SourceLibPath);
				PublicAdditionalLibraries.Add(Path.Combine(SourceLibPath, "CapabilityDetectLibrary.lib"));
				
				// Copy DLL to Binaries folder and use that path for runtime
				string SourceDll = Path.Combine(SourceLibPath, "CapabilityDetectLibrary.dll");
				string TargetDll = Path.Combine(TargetLibPath, "CapabilityDetectLibrary.dll");
				
				// Ensure target directory exists
				Directory.CreateDirectory(TargetLibPath);
				
				// Copy DLL if it doesn't exist or if source is newer
				if (File.Exists(SourceDll))
				{
					if (!File.Exists(TargetDll) || File.GetLastWriteTime(SourceDll) > File.GetLastWriteTime(TargetDll))
					{
						File.Copy(SourceDll, TargetDll, true);
					}
					
					// Use the target path for runtime dependencies
					PublicDelayLoadDLLs.Add("CapabilityDetectLibrary.dll");
					RuntimeDependencies.Add(TargetDll);
				}
				else
				{
					System.Console.WriteLine("Source DLL not found at: " + SourceDll);
				}
			}
			else
			{
				System.Console.WriteLine("Source library path not found: " + SourceLibPath);
			}
		}
	}
}
