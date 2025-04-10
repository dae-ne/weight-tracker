param webAppServiceName string
param webAppServicePlanName string
param webAppSkuName string = 'F1'
param storageAccountName string
param keyVaultName string
param location string = resourceGroup().location

var storageConnectionStringSecretName   = 'storage-connection-string'
var storageConnectionStringReference   = '@Microsoft.KeyVault(VaultName=${keyVaultName};SecretName=${storageConnectionStringSecretName})'

module webAppPlan 'modules/appServicePlan.bicep' = {
  name: 'appServicePlanDeployment'
  params: {
    appServicePlanName: webAppServicePlanName
    skuName: webAppSkuName
    location: location
  }
}

module storageAccount 'modules/storageAccount.bicep' = {
  name: 'storageAccountDeployment'
  params: {
    storageAccountName: storageAccountName
    location: location
  }
}

module webApp 'modules/appService.bicep' = {
  name: 'appServiceDeployment'
  params: {
    appName: webAppServiceName
    appServicePlanId: webAppPlan.outputs.appServicePlanId
    location: location
    customAppSettings: [
      {
        name: 'AzureWebJobsStorage'
        value: storageConnectionStringReference
      }
    ]
  }
}

module keyVault 'modules/keyVault.bicep' = {
  name: 'keyVaultDeployment'
  params: {
    keyVaultName: keyVaultName
    webAppIdentityPrincipalId: webApp.outputs.identityPrincipalId
    storageAccountName: storageAccountName
  }
  dependsOn: [
    storageAccount
  ]
}
