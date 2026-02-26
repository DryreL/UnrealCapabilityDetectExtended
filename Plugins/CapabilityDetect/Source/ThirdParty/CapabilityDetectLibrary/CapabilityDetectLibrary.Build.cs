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
			// Source path where DLL is located - exact path as specified by user
			string SourceLibPath = Path.Combine(ModuleDirectory, "x64", "Release");
			
			// Log the exact path being checked
			System.Console.WriteLine("CapabilityDetectLibrary: Checking source path: " + SourceLibPath);
			
			// Check if the source directory exists
			if (!Directory.Exists(SourceLibPath))
			{
				System.Console.WriteLine("ERROR: CapabilityDetectLibrary: Source directory not found at: " + SourceLibPath);
				return; // Exit early if source directory doesn't exist
			}
			
			// Check if the DLL file exists
			string SourceDll = Path.Combine(SourceLibPath, "CapabilityDetectLibrary.dll");
			if (!File.Exists(SourceDll))
			{
				System.Console.WriteLine("ERROR: CapabilityDetectLibrary: DLL file not found at: " + SourceDll);
				return; // Exit early if DLL doesn't exist
			}
			
			// Check if the LIB file exists
			string SourceLib = Path.Combine(SourceLibPath, "CapabilityDetectLibrary.lib");
			if (!File.Exists(SourceLib))
			{
				System.Console.WriteLine("ERROR: CapabilityDetectLibrary: LIB file not found at: " + SourceLib);
				return; // Exit early if LIB doesn't exist
			}
			
			System.Console.WriteLine("CapabilityDetectLibrary: All source files found successfully");
			
			// Add source path for compilation (headers are in ModuleDirectory)
			PublicIncludePaths.Add(ModuleDirectory);
			PublicAdditionalLibraries.Add(SourceLib);
			
			// TARGET 1: Project's main Binaries folder (for shipping builds)
			string ProjectBinariesPath = Path.Combine(ModuleDirectory, "..", "..", "..", "..", "..", "Binaries", "Win64");
			string ProjectDllPath = Path.Combine(ProjectBinariesPath, "CapabilityDetectLibrary.dll");
			
			// TARGET 2: Plugin's own Binaries folder (for plugin loading)
			string PluginBinariesPath = Path.Combine(ModuleDirectory, "..", "..", "..", "Binaries", "Win64");
			string PluginDllPath = Path.Combine(PluginBinariesPath, "CapabilityDetectLibrary.dll");
			
			// Ensure both target directories exist
			Directory.CreateDirectory(ProjectBinariesPath);
			Directory.CreateDirectory(PluginBinariesPath);
			
			// Copy DLL to both locations
			if (!File.Exists(ProjectDllPath) || File.GetLastWriteTime(SourceDll) > File.GetLastWriteTime(ProjectDllPath))
			{
				File.Copy(SourceDll, ProjectDllPath, true);
				System.Console.WriteLine("CapabilityDetectLibrary: DLL copied to project Binaries: " + ProjectDllPath);
			}
			
			if (!File.Exists(PluginDllPath) || File.GetLastWriteTime(SourceDll) > File.GetLastWriteTime(PluginDllPath))
			{
				File.Copy(SourceDll, PluginDllPath, true);
				System.Console.WriteLine("CapabilityDetectLibrary: DLL copied to plugin Binaries: " + PluginDllPath);
			}
			
			// Use the project's main Binaries path for runtime dependencies (shipping builds)
			PublicDelayLoadDLLs.Add("CapabilityDetectLibrary.dll");
			RuntimeDependencies.Add(ProjectDllPath);
			
			System.Console.WriteLine("CapabilityDetectLibrary: Module configured successfully");
		}
		else
		{
			System.Console.WriteLine("CapabilityDetectLibrary: Skipping non-Windows platform: " + Target.Platform);
		}
	}
}
