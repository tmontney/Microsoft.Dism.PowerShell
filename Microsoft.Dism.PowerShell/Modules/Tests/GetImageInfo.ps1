Import-Module "..\Microsoft.Dism.PowerShell\Microsoft.Dism.PowerShell.psd1"
#$IMAGE_FILE_PATH = "$PSScriptRoot\Winre.wim"
$IMAGE_FILE_PATH = "C:\Dism\Winre.wim"

try {
    Initialize-PSDism -LogLevel "LogErrors"
    Get-PSDismImageInfo -ImagePath $IMAGE_FILE_PATH
}
finally {
    Remove-PSDism
}