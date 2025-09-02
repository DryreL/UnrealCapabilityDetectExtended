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

		// Only include Windows-specific dependencies
		if (Target.Platform == UnrealTargetPlatform.Win64)
		{
			string LibPath = Path.Combine(ModuleDirectory, "x64", "Release");
			
			if (Directory.Exists(LibPath))
			{
				PublicIncludePaths.Add(LibPath);
				PublicAdditionalLibraries.Add(Path.Combine(LibPath, "CapabilityDetectLibrary.lib"));
				PublicDelayLoadDLLs.Add(Path.Combine(LibPath, "CapabilityDetectLibrary.dll"));
			}
		}
	}
}
