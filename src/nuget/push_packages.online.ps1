$apiKey = $args[0]
$nugethost = $args[1]
. ".\common.ps1"

# Get the version
[xml]$commonPropsXml = Get-Content (Join-Path $rootFolder "common.props")
$version = $commonPropsXml.Project.PropertyGroup.Version
$nugetUrl = "https://api.nuget.org/v3/index.json"

Set-Location $packFolder

if ($nugethost -eq "1"){
  $nugetUrl = "http://120.46.73.168:5001/api/v2/package"
}

foreach($project in $projects) {
	$i += 1
	$projectFolder = Join-Path $rootFolder $project
	$projectName = ($project -split '/')[-1]
	$nugetPackageName = $projectName + "." + $version + ".nupkg"
	$nugetPackageExists = Test-Path $nugetPackageName -PathType leaf
 
	Write-Info "[$i / $totalProjectsCount] - Pushing: $nugetPackageName"
	
	if ($nugetPackageExists)
	{
		dotnet nuget push $nugetPackageName --skip-duplicate -s $nugetUrl --api-key "$apiKey"		
		#dotnet nuget push $nugetPackageName -s $nugetUrl	
		#Write-Host ("Deleting package from local: " + $nugetPackageName)
		Remove-Item $nugetPackageName -Force
	}
	else
	{
		Write-Host ("********** ERROR PACKAGE NOT FOUND: " + $nugetPackageName) -ForegroundColor red
		$errorCount += 1
		#Exit
	}
	
	Write-Host "--------------------------------------------------------------`r`n"
}
if ($errorCount > 0)
{
	Write-Host ("******* $errorCount error(s) occured *******") -ForegroundColor red
}
