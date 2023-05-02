. .\Variables.ps1

if (!(Test-Path "..\deploy")){
    New-Item -ItemType Directory -Path ..\deploy
}

$currentLocation = Get-Location
Set-Location ..\src\client

# install packages
npm i

# build project
npm run build

# Create build artifacts archive
Compress-Archive -Path .\dist\* -DestinationPath ..\..\deploy\$webAppService.zip -Force

# CD to root directory
Set-Location $currentLocation

# Deploy
az webapp deploy --resource-group $resourceGroup --name $webAppService --src-path ..\deploy\$webAppService.zip --type zip --async true