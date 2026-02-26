Intel Capability Detect Extended - Changelog

Version 1.1

## Updates & Modernization
- **UE 5.7 Compatibility:** Updated codebase to compile seamlessly on Unreal Engine 5.7.
- **RHI Vendor Retrieval:** Updated `RHIGetVendorName` calls to use the correct `RHIVendorIdToString` API for modern UE versions.
- **Performance Index Calculation:** Unified `FSynthBenchmarkResults` usage for `ComputeGPUPerfIndex` and `ComputeCPUPerfIndex`. Correctly scoped the `GenericPlatformSurvey.h` dependency.
- **Blueprint Categorization:** Re-organized blueprint function categories into logically grouped sub-categories (`CPU`, `GPU`, `Memory`, `Benchmark`, `Core`) for easier access in graphs.
- **Module Safety:** Replaced raw handles in internal modules with in-class initializers (`= nullptr`) and added appropriate thread-safety patterns.

## ThirdParty Library Improvements
- **Memory Safety:** Mitigated memory leaks in `Intel_GetSKU` with proper C++ resource management (e.g. `std::make_unique` migration). Fixed potential `malloc` crashes and buffer overruns in `cache_l3_size` by adding null pointer checks correctly.
- **Legacy API Removal:** Removed dependencies on deprecated `PlatformSurvey.h`, dynamic runtime loaded Win32 Kernel32 endpoints (`GetProcAddress(..., "GetLogicalProcessorInformation")`), and replaced them with direct calls suitable for Win8+.
- **Cleaned Obsolete Code:** Removed unnecessary Win32 COM, DirectX, and SDK includes (`D3D11.h`, `dxdiag.h`, `iostream`, `assert.h`) to keep the library lean and prevent shipping crashes.

## Build Script Modernization
- Fixed invalid/hardcoded directory assumptions in `CapabilityDetectLibrary.Build.cs`.
- Eliminated duplicate `SynthBenchmark` constraints in `CapabilityDetect.Build.cs`.
- Replaced deprecated `"WhitelistPlatforms"` rule with `"PlatformAllowList"` in the plugin descriptor.

Version 1.0
- Initial release by Intel.