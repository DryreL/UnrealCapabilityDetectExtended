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

		if (Target.Platform == UnrealTargetPlatform.Win64)
		{
			// Add the import library
			//PublicLibraryPaths.Add(Path.Combine(ModuleDirectory, "x64", "Release"));
			
			//PublicAdditionalLibraries.Add("CapabilityDetectLibrary.lib");
			//PublicAdditionalLibraries.Add("$(PluginDir)/Source/ThirdParty/CapabilityDetectLibrary/x64/Release/CapabilityDetectLibrary.lib");

			/*
			string LibAdditionalPath = Path.Combine(ModuleDirectory, "x64", "Release");
			foreach (string file in Directory.GetFiles(LibAdditionalPath, "*lib"))
			{
				PublicAdditionalLibraries.Add(file);
			}
			*/

			PublicIncludePaths.Add(Path.Combine(ModuleDirectory, "x64", "Release"));
			PublicAdditionalLibraries.Add("$(ModuleDir)/x64/Release/CapabilityDetectLibrary.lib");
			PublicDelayLoadDLLs.Add("$(ModuleDir)/x64/Release/CapabilityDetectLibrary.dll");

            // Delay-load the DLL, so we can load it from the right place first
            //PublicDelayLoadDLLs.Add("$(PluginDir)/Source/ThirdParty/CapabilityDetectLibrary/x64/Release/CapabilityDetectLibrary.dll");

            /*
                        string LibDelayLoadPath = Path.Combine(ModuleDirectory, "x64", "Release");
                        foreach (string file in Directory.GetFiles(LibDelayLoadPath, "*dll"))
                        {
                            PublicDelayLoadDLLs.Add(file);
                        }
            */
        }
        else if (Target.Platform == UnrealTargetPlatform.Mac)
        {
/*
            string LibMacDelayLoadPath = Path.Combine(ModuleDirectory, "Mac", "Release");
            foreach (string file in Directory.GetFiles(LibMacDelayLoadPath, "*dylib"))
            {
	            PublicDelayLoadDLLs.Add(file);
            }
*/
			PublicIncludePaths.Add(Path.Combine(ModuleDirectory, "Mac", "Release"));
			PublicDelayLoadDLLs.Add("CapabilityDetectLibrary.dylib");

        }
	}
}
