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

using UnrealBuildTool;
using System.Collections.Generic;

public class CapabilityDetectDemoEditorTarget : TargetRules
{
	public CapabilityDetectDemoEditorTarget(TargetInfo Target) : base(Target)
	{
		Type = TargetType.Editor;

		ExtraModuleNames.AddRange( new string[] { "CapabilityDetectDemo" } );
		
		// ERROR: Targets with a unique build environment cannot be built with an installed engine.
		//BuildEnvironment = TargetBuildEnvironment.Unique;
		
		// Override build environment settings
		bOverrideBuildEnvironment = true;

		DefaultBuildSettings = BuildSettingsVersion.V5;
		IncludeOrderVersion = EngineIncludeOrderVersion.Latest;
		bValidateFormatStrings = true;
		WindowsPlatform.bStrictConformanceMode = true;
		CppStandard = CppStandardVersion.Default;
		bLegacyParentIncludePaths = false;
		//ShadowVariableWarningLevel = WarningLevel.Error;
	}
}
