. .\Variables.ps1
if (!(Test-Path "..\deploy")){
    New-Item -ItemType Directory -Path ..\deploy
}

# build application
dotnet build ..\src\AzureblobStorageDemoApi\AzureblobStorageDemoApi.csproj

# publish api 
dotnet publish ..\src\AzureblobStorageDemoApi\AzureblobStorageDemoApi.csproj --no-restore -o ..\deploy\$apiAppService

Compress-Archive -Path ..\deploy\$apiAppService\* -DestinationPath ..\deploy\$apiAppService.zip -Force

# ZIP deploy the web app
az webapp deploy --resource-group $resourceGroup --name $apiAppService --src-path ..\deploy\$apiAppService.zip --type zip --async true