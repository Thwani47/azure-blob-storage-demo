. .\Variables.ps1

az group create --name $resourceGroup --location $location

az appservice plan create --name $appServicePlan --resource-group $resourceGroup --sku free
az webapp create --name $webAppService --plan $appServicePlan --resource-group $resourceGroup
az webapp create --name $apiAppService --plan $appServicePlan --resource-group $resourceGroup

az storage account create --name $azureStorageAccount --resource-group $resourceGroup --sku Standard_LRS