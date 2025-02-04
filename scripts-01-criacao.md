# 1. Cria o Resource Group
az group create --name myResourceGroup --location eastus

# 2. Cria a Storage Account (o nome deve ser único globalmente)
az storage account create \
  --name mystorageaccount123 \
  --location eastus \
  --resource-group myResourceGroup \
  --sku Standard_LRS

# 3. Cria a conta do Cosmos DB
az cosmosdb create \
  --name mycosmosaccount123 \
  --resource-group myResourceGroup \
  --kind GlobalDocumentDB

# 4. Cria a Function App usando o plano de consumo
az functionapp create \
  --resource-group myResourceGroup \
  --consumption-plan-location eastus \
  --name myfunctionapp123 \
  --storage-account mystorageaccount123 \
  --runtime dotnet
  