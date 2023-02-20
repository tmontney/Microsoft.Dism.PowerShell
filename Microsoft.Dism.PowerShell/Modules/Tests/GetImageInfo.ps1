Import-Module "C:\ACC\Dev\Visual Studio\Microsoft.Dism.PowerShell\Microsoft.Dism.PowerShell\bin\Debug\Microsoft.Dism.PowerShell.dll"

#$IMAGE_FILE_PATH = "$PSScriptRoot\Winre.wim"
$IMAGE_FILE_PATH = "C:\Dism\Winre.wim"

try {
    Initialize-PSDism -LogLevel "LogErrors"
    Get-PSDismImageInfo -ImagePath $IMAGE_FILE_PATH
}
finally {
    Remove-PSDism
}