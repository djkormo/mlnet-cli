#!/bin/bash

# based on https://docs.microsoft.com/en-us/azure/container-instances/container-instances-using-azure-container-registry

# zmienne konfiguracyjne

RND=$RANDOM

ACR_LOCATION=northeurope
ACR_GROUP=rg-ml-webapp
ACR_NAME=acrml$RND
ACI_NAME=aciml$RND
AKV_NAME=keyvault$RND

# domyslna nazwa grupy 
az configure --defaults group=$ACR_GROUP

# domyslna lokalizacja rejestru z obrazami 
az configure --defaults location=$ACR_LOCATION

# odswiezamy zawartosc repozytorium w ramach galęzi master 
git checkout master

# pobieramy zawartość repozytorium 
git pull

# weryfikujemy niespójności zawartości lokalnego i zdalnego repozytorium 
git status

# zawartosc pliku DockerFile
cat Dockerfile 


# nowa grupa zasobów
az group create --name $ACR_GROUP

# tworzymy rejestr dla kontenerów 
az acr create  --name $ACR_NAME --sku Basic

# włączenie konta administratorskiego
az acr update -n  $ACR_NAME --admin-enabled true

# pobranie wygenerowanego użytkownika i hasła, przeniesione na poziom SP 
#ACR_USERNAME=$ACR_NAME
#ACR_PASSWORD=$(az acr credential show --name $ACR_NAME --query "passwords[0].value")

#echo "$ACR_USERNAME"
#echo "$ACR_PASSWORD"


# key vault 
az keyvault create -g $ACR_GROUP -n $AKV_NAME

# service principal 
az keyvault secret set \
  --vault-name $AKV_NAME \
  --name $ACR_NAME-pull-pwd \
  --value $(az ad sp create-for-rbac \
                --name http://$ACR_NAME-pull \
                --scopes $(az acr show --name $ACR_NAME --query id --output tsv) \
                --role acrpull \
                --query password \
                --output tsv)

				
az keyvault secret set \
    --vault-name $AKV_NAME \
    --name $ACR_NAME-pull-usr \
    --value $(az ad sp show --id http://$ACR_NAME-pull --query appId --output tsv)				




	

ACR_LOGIN_SERVER=$(az acr show --name $ACR_NAME --resource-group $ACR_GROUP --query "loginServer" --output tsv)				
		

# budujemy obraz kontenerowy  na podstawie zawartości pliku Dockerfile

az acr build --registry $ACR_NAME --image ml-net-webapp:v1 .

# lista zbudowanych obrazów
az acr repository list --name $ACR_NAME --output table
	
		
# szczegoly 
az acr repository show -n $ACR_NAME -t ml-net-webapp:v1


# utworzenie instancji obrazu ->utworzenie aplikacji 
az container create \
    --name aci-demo \
    --resource-group $ACR_GROUP \
    --image $ACR_LOGIN_SERVER/ml-net-webapp:v1 \
    --registry-login-server $ACR_LOGIN_SERVER \
    --registry-username $(az keyvault secret show --vault-name $AKV_NAME -n $ACR_NAME-pull-usr --query value -o tsv) \
    --registry-password $(az keyvault secret show --vault-name $AKV_NAME -n $ACR_NAME-pull-pwd --query value -o tsv) \
    --dns-name-label ml-net-webapp$RND \
	--ports 80 \
    --ip-address=Public \
    --query ipAddress.fqdn
		
			

# lista uruchomionych kontenerow

 az container show --resource-group  $ACR_GROUP \
 --name $ACI_NAME \
  --output table


# aplikacja dostępna pod adresem 
# http://ai-customvision$RND.northeurope.azurecontainer.io:80

# testowanie
# clear
# koty
#curl -X POST http://ml-net-webapp$RND.northeurope.azurecontainer.io/image -F imageData=@images/cat1.jpg
curl -X POST http://ml-net-webapp$RND.northeurope.azurecontainer.io:5000/api/Predict/model  -d "{  \"sepalLength\": 0,  \"sepalWidth\": 0,  \"petalLength\": 0,  \"petalWidth\": 0, \"label\": \"string\"}"


#curl -X POST http://ml-net-webapp$RND.northeurope.azurecontainer.io/image -F imageData=@images/dog1.jpg
#curl -X POST http://ml-net-webapp$RND.northeurope.azurecontainer.io/image -F imageData=@images/dog2.jpg

# konie
#curl -X POST http://ml-net-webapp$RND.northeurope.azurecontainer.io/image -F imageData=@images/horse1.jpg
#curl -X POST http://ml-net-webapp$RND.northeurope.azurecontainer.io/image -F imageData=@images/horse2.jpg


# usuwamy zasoby w ramach calej grupy 
#az group delete --name $ACR_GROUP --no-wait -y
