. .\Variables
az webapp config appsettings set --name $webAppService --resource-group $resourceGroup --settings WEBSITE_NODE_DEFAULT_VERSION="~16"
az webapp config set --name  $webAppService --resource-group $resourceGroup --startup-file="pm2 serve /home/site/wwwroot/ --spa --no-daemon"

$storageAccountEndpoint=$(az storage account show-connection-string --name $azureStorageAccount --resource-group $resourceGroup --query connectionString --output tsv)
az webapp config appsettings set --resource-group $resourceGroup --name $apiAppService --settings AzureBlobStorage:Endpoint=$storageAccountEndpoint
az webapp cors add --resource-group $resourceGroup --name $apiAppService --allowed-origins "https://$webAppService.azurewebsites.net"