Import-Module "C:\ACC\Dev\Visual Studio\Microsoft.Dism.PowerShell\Microsoft.Dism.PowerShell\bin\Debug\Microsoft.Dism.PowerShell.dll"

#$IMAGE_FILE_PATH = "$PSScriptRoot\Winre.wim"
#$MOUNT_PATH = "$PSScriptRoot\mount"

$IMAGE_FILE_PATH = "C:\Dism\Winre.wim"
$MOUNT_PATH = "C:\Dism\mount"

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

    Get-PSDismMountedImages
}
finally {
    if ($Session) { Close-PSDismSession -Session $Session }
    Dismount-PSDismImage -MountPath $MOUNT_PATH -CommitChanges:$false -ProgressCallback ((New-DismProgressCallback)) -UserData "Dismounting Image"
    Remove-PSDism
}