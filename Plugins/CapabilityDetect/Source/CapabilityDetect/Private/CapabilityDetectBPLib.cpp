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

#include "CapabilityDetectBPLib.h"
#include "ThirdParty/CapabilityDetectLibrary/CapabilityDetectLibrary.h"
#include "RHI.h"
#include "RHIGlobals.h"
#include "RHIStrings.h"
#include "SynthBenchmark.h"
#include "GenericPlatform/GenericPlatformSurvey.h"
#include "HAL/PlatformMemory.h"
#include "GenericPlatform/GenericPlatformMisc.h"
#include "Misc/App.h"

void UCapabilityDetectBPLib::InitializeResources()
{
#if PLATFORM_WINDOWS
	Intel_InitializeResources();
#endif
}

void UCapabilityDetectBPLib::FreeResources()
{
#if PLATFORM_WINDOWS
	Intel_FreeResources();
#endif
}

bool UCapabilityDetectBPLib::IsIntelCPU()
{
#if PLATFORM_WINDOWS
	return Intel_IsIntelCPU();
#else
	return FPlatformMisc::GetCPUVendor().Contains("Intel");
#endif
}

float UCapabilityDetectBPLib::ComputeGPUPerfIndex()
{
	static bool bRunOnce = false;
	static FSynthBenchmarkResults synthBenchResults;

	if (!bRunOnce) {
		ISynthBenchmark::Get().Run(synthBenchResults, true);
		bRunOnce = true;
	}

	return synthBenchResults.ComputeGPUPerfIndex();
}

float UCapabilityDetectBPLib::ComputeCPUPerfIndex()
{
	static bool bRunOnce = false;
	static FSynthBenchmarkResults synthBenchResults;

	if (!bRunOnce) {
		ISynthBenchmark::Get().Run(synthBenchResults, false);
		bRunOnce = true;
	}

	return synthBenchResults.ComputeCPUPerfIndex();
}

int32 UCapabilityDetectBPLib::GetNumLogicalCores()
{
#if PLATFORM_WINDOWS
	return Intel_GetNumLogicalCores();
#else
	return FPlatformMisc::NumberOfCoresIncludingHyperthreads();
#endif
}

int32 UCapabilityDetectBPLib::GetNumPhysicalCores()
{
#if PLATFORM_WINDOWS
	return Intel_GetNumPhysicalCores();
#else
	return FPlatformMisc::NumberOfCores();
#endif
}

bool UCapabilityDetectBPLib::IsHyperThreadingEnabled()
{
	return GetNumLogicalCores() > GetNumPhysicalCores();
}

float UCapabilityDetectBPLib::GetUsablePhysMemoryGB()
{
#if PLATFORM_WINDOWS
	float IntelMem = Intel_GetUsablePhysMemoryGB();
	if (IntelMem > 0.f) return IntelMem;
#endif
	return FPlatformMemory::GetStats().TotalPhysicalGB;
}

float UCapabilityDetectBPLib::GetComittedMemoryMB()
{
#if PLATFORM_WINDOWS
	float IntelMem = Intel_GetComittedMemoryMB();
	if (IntelMem > 0.f) return IntelMem;
#endif
	return static_cast<float>(FPlatformMemory::GetStats().UsedPhysical / (1024ULL * 1024ULL));
}

float UCapabilityDetectBPLib::GetAvailableMemoryMB()
{
#if PLATFORM_WINDOWS
	float IntelMem = Intel_GetAvailableMemoryMB();
	if (IntelMem > 0.f) return IntelMem;
#endif
	return static_cast<float>(FPlatformMemory::GetStats().AvailablePhysical / (1024ULL * 1024ULL));
}

float UCapabilityDetectBPLib::GetCacheSizeMB()
{
#if PLATFORM_WINDOWS
	return Intel_GetCacheSizeMB();
#else
	return 0.0f;
#endif
}

float UCapabilityDetectBPLib::GetMaxBaseFrequency()
{
#if PLATFORM_WINDOWS
	return Intel_GetMaxBaseFrequency();
#else
	return 0.0f;
#endif
}

float UCapabilityDetectBPLib::GetCoreFrequency()
{
#if PLATFORM_WINDOWS
	return Intel_GetCoreFrequency();
#else
	return 0.0f;
#endif
}

float UCapabilityDetectBPLib::GetCorePercMaxFrequency()
{
#if PLATFORM_WINDOWS
	return Intel_GetCorePercMaxFrequency();
#else
	return 0.0f;
#endif
}

FString UCapabilityDetectBPLib::GetFullProcessorName()
{
#if PLATFORM_WINDOWS
	const int bufferSize = 512;
	char buffer[bufferSize];
	int size = bufferSize;
	Intel_GetFullProcessorName(buffer, &size);
	return FString(ANSI_TO_TCHAR(buffer));
#else
	return FPlatformMisc::GetCPUBrand();
#endif
}

FString UCapabilityDetectBPLib::GetProcessorName()
{
#if PLATFORM_WINDOWS
	const int bufferSize = 512;
	char buffer[bufferSize];
	int size = bufferSize;
	Intel_GetProcessorName(buffer, &size);
	return FString(ANSI_TO_TCHAR(buffer));
#else
	return FPlatformMisc::GetCPUBrand();
#endif
}

FString UCapabilityDetectBPLib::GetCPUVendorName()
{
	return FPlatformMisc::GetCPUVendor();
}

bool UCapabilityDetectBPLib::IsRHIIntel()
{
	return IsRHIDeviceIntel();
}

bool UCapabilityDetectBPLib::IsRHIAMD()
{
	return IsRHIDeviceAMD();
}

bool UCapabilityDetectBPLib::IsRHINVIDIA()
{
	return IsRHIDeviceNVIDIA();
}

FName UCapabilityDetectBPLib::RHIVendorName()
{
	return FName(RHIVendorIdToString());
}

FString UCapabilityDetectBPLib::GetRHIAdapterDescription()
{
	return GRHIAdapterName;
}

FString UCapabilityDetectBPLib::GetRHIShaderPlatformName()
{
	return LegacyShaderPlatformToShaderFormat(GMaxRHIShaderPlatform).ToString();
}
