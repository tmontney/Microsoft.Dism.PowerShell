Import-Module "..\Microsoft.Dism.PowerShell\Microsoft.Dism.PowerShell.psd1"

#$IMAGE_FILE_PATH = "$PSScriptRoot\Winre.wim"
#$MOUNT_PATH = "$PSScriptRoot\mount"

$IMAGE_FILE_PATH = "C:\Dism\Winre.wim"
$MOUNT_PATH = "C:\Dism\mount"
$PACKAGE_PATH = "C:\Dism\windows10.0-kb5021040-x64_2216fe185502d04d7a420a115a53613db35c0af9.cab"

function New-DismProgressCallback {
    [Microsoft.Dism.DismProgressCallback]$progressCallback = {
        param (
            [Parameter(Mandatory = $true)]
            [Microsoft.Dism.DismProgress]
            $Progress
        )
    
        $PercentComplete = [Math]::Round(($Progress.Current / $Progress.Total * 100), 0)
        Write-Progress -Activity $Progress.UserData -PercentComplete $PercentComplete
    }

    return $progressCallback
}

if (-not (Test-Path -Path $MOUNT_PATH)) {
    [void](New-Item -Path $MOUNT_PATH -ItemType Directory -Force -ErrorAction Stop)
}

try {
    Initialize-PSDism -LogLevel "LogErrors"
    Mount-PSDismImage -ImageFilePath $IMAGE_FILE_PATH -MountPath $MOUNT_PATH -ImageIndex 1 -ProgressCallback ((New-DismProgressCallback)) -UserData "Mounting Image"
    $Session = Open-PSDismSession -ImagePath $MOUNT_PATH
    Find-PSDismPackage -Session $Session -PackagePath $PACKAGE_PATH
}
finally {
    if (-not $Session.IsClosed) { Close-PSDismSession -Session $Session }
    Dismount-PSDismImage -MountPath $MOUNT_PATH -CommitChanges:$false -ProgressCallback ((New-DismProgressCallback)) -UserData "Dismounting Image"
    Remove-PSDism
}