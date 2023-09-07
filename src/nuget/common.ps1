# Paths
$packFolder = (Get-Item -Path "./" -Verbose).FullName
$rootFolder = Join-Path $packFolder "../"

function Write-Info   
{
	param(
        [Parameter(Mandatory = $true)]
        [string]
        $text
    )

	Write-Host $text -ForegroundColor Black -BackgroundColor Green

	try 
	{
	   $host.UI.RawUI.WindowTitle = $text
	}		
	catch 
	{
		#Changing window title is not suppoerted!
	}
}

function Write-Error   
{
	param(
        [Parameter(Mandatory = $true)]
        [string]
        $text
    )

	Write-Host $text -ForegroundColor Red -BackgroundColor Black 
}

function Seperator   
{
	Write-Host ("_" * 100)  -ForegroundColor gray 
}



function Get-Current-Version { 
	$commonPropsFilePath = resolve-path "../common.props"
	$commonPropsXmlCurrent = [xml](Get-Content $commonPropsFilePath ) 
	$currentVersion = $commonPropsXmlCurrent.Project.PropertyGroup.Version.Trim()
	return $currentVersion
}

function Get-Current-Branch {
	return git branch --show-current
}	   

function Read-File {
	param(
        [Parameter(Mandatory = $true)]
        [string]
        $filePath
    )
		
	$pathExists = Test-Path -Path $filePath -PathType Leaf
	if ($pathExists)
	{
		return Get-Content $filePath		
	}
	else{
		Write-Error  "$filePath path does not exist!"
	}
}


# List of projects
$projects = (
	# Core
	"Core/ViazyNetCore.Core",
	"Core/ViazyNetCore.AttachmentProvider",
	"Core/ViazyNetCore.Auth",
	"Core/ViazyNetCore.Authorization",
	"Core/ViazyNetCore.Caching",
	"Core/ViazyNetCore.Data.FreeSql",
	"Core/ViazyNetCore.Identity",
	# "Core/ViazyNetCore.OpenIddict",
	
	#Infrastructure
	"Infrastructure/ViazyNetCore.Annotations",
	"Infrastructure/ViazyNetCore.AspNetCore",
	"Infrastructure/ViazyNetCore.AutoMapper",
	"Infrastructure/ViazyNetCore.DI",
	"Infrastructure/ViazyNetCore.EventBus",
	"Infrastructure/ViazyNetCore.EventBus.RabbitMQ",
	"Infrastructure/ViazyNetCore.Formatter.Excel",
	"Infrastructure/ViazyNetCore.Formatter.Response",
	"Infrastructure/ViazyNetCore.MultiTenancy",
	"Infrastructure/ViazyNetCore.MultiTenancy.AspNetCore",
	"Infrastructure/ViazyNetCore.NLog",
	"Infrastructure/ViazyNetCore.RabbitMQ",
	"Infrastructure/ViazyNetCore.Redis",
	"Infrastructure/ViazyNetCore.Swagger",
	"Infrastructure/ViazyNetCore.TaskScheduler",
	"Infrastructure/ViazyNetCore.TaskScheduler.RabbitMQ",
	"Infrastructure/ViazyNetCore.Web.DevServer",
	"Infrastructure/ViazyNetCore.OSS",
	"Infrastructure/ViazyNetCore.OSS.Minio"
)