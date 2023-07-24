. ".\common.ps1"
# Delete existing nupkg files
del *.nupkg


# Create all packages
$i = 0
$projectsCount = $projects.length
Write-Info "Running dotnet pack on $projectsCount projects..."

foreach($project in $projects) {
    $i += 1
    $projectFolder = Join-Path $rootFolder $project
	$projectName = ($project -split '/')[-1]
		
	# Create nuget pack
    Write-Info "[$i / $projectsCount] - Packing project: $projectName"
	Set-Location $projectFolder

    #dotnet clean
   dotnet pack -c Debug --no-build -- /maxcpucount

    if (-Not $?) {
        Write-Error "Packaging failed for the project: $projectName" 
        exit $LASTEXITCODE
    }
    
    # Move nuget package
    $projectName = $project.Substring($project.LastIndexOf("/") + 1)
    $projectPackPath = Join-Path $projectFolder ("/bin/Debug/" + $projectName + ".*.nupkg")
    Move-Item -Force $projectPackPath $packFolder
	
	Seperator
}

# Go back to the pack folder
Set-Location $packFolder