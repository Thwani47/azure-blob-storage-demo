. .\Variables
az webapp config appsettings set --name $webAppService --resourceGroup $resourceGroup -- settings WEBSITE_NODE_DEFAULT_VERSION="~16"
az webapp config set --name  $webAppService --resourceGroup $resourceGroup --startup-file="pm2 serve /home/site/wwwroot/ --spa --no-daemon"