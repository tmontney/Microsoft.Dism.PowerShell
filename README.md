# Overview
This PowerShell module is a wrapper for [ManagedDism](https://github.com/jeffkl/ManagedDism). Since creating comment-based help for C# PS cmdlets is a considerable pain, please see the [DISM API Reference](https://learn.microsoft.com/en-us/windows-hardware/manufacture/desktop/dism/dism-api-reference?view=windows-11). Most of the parameters line up almost exactly with `ManagedDism` and the Win32 API signatures.

# Samples
Please refer to the [Tests directory](Microsoft.Dism.PowerShell/Modules/Tests).

# Exported Commands
Not all of the DISM commands have been wrapped in this PowerShell module. The list below attempts to map commands to their more widely-known command-line counterparts:

- [Add-PSDismPackage](https://learn.microsoft.com/en-us/windows-hardware/manufacture/desktop/dism-operating-system-package-servicing-command-line-options?view=windows-11#add-package)
- [Clear-PSDismMountPoints](https://learn.microsoft.com/en-us/windows-hardware/manufacture/desktop/dism-image-management-command-line-options-s14?view=windows-11#cleanup-mountpoints)
- Close-PSDismSession
- [Dismount-PSDismImage](https://learn.microsoft.com/en-us/windows-hardware/manufacture/desktop/dism-image-management-command-line-options-s14?view=windows-11#unmount-image)
- [Get-PSDismImageInfo](https://learn.microsoft.com/en-us/windows-hardware/manufacture/desktop/dism-image-management-command-line-options-s14?view=windows-11#get-mountedimageinfo)
- [Get-PSDismMountedImages](https://learn.microsoft.com/en-us/windows-hardware/manufacture/desktop/dism-image-management-command-line-options-s14?view=windows-11#get-mountedimageinfo)
- [Get-PSDismPackageInfo](https://learn.microsoft.com/en-us/windows-hardware/manufacture/desktop/dism-operating-system-package-servicing-command-line-options?view=windows-11#get-packageinfo)
- [Get-PSDismPackages](https://learn.microsoft.com/en-us/windows-hardware/manufacture/desktop/dism-operating-system-package-servicing-command-line-options?view=windows-11#get-packages)
- Initialize-PSDism
- [Mount-PSDismImage](https://learn.microsoft.com/en-us/windows-hardware/manufacture/desktop/dism-image-management-command-line-options-s14?view=windows-11#mount-image)
- Open-PSDismSession
- Remove-PSDism