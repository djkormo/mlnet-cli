RND=$RANDOM

# based on 
# https://github.com/MicrosoftDocs/azure-docs/blob/master/articles/storage/files/storage-how-to-use-files-cli.md

# https://microsoft.github.io/AzureTipsAndTricks/blog/tip174.html

ML_LOCATION=northeurope
ML_GROUP=rg-ml-net
ML_STACCOUNT=staccmlnet$RND
ML_FUNCAPP=mlnetfuncapp$RND

ML_MODEL_FILE=$1

ML_MODEL_BLOB=$2

if [[ $# -eq 0 ]] ; then
    echo 'Nie podano nazwy pliku z modelem jako parametru wejsciowego pierwszego !'
    exit 1
fi


if [[ $# -eq 1 ]] ; then
    echo 'Nie podano nazwy blobu jako parametru wejsciowego drugiego !'
    exit 1
fi

# domyslna nazwa grupy 
az configure --defaults group=$ML_GROUP

# domyslna lokalizacja rejestru z obrazami 
az configure --defaults location=$ML_LOCATION

# odswiezamy zawartosc repozytorium w ramach galęzi master 
git checkout master

# pobieramy zawartość repozytorium 
git pull

# weryfikujemy niespójności zawartości lokalnego i zdalnego repozytorium 
git status

#if [[ az group exists -n $ML_GROUP ]]; 
if [ $(az group exists -n $ML_GROUP) == 'true' ];
then
  echo "$ML_GROUP grupa zasobow istnieje"
  
else
   az group create --name $ML_GROUP --location $ML_LOCATION
fi 

# tu sprawdzenie czy musimy tworzyć konto w ramach grupy $ML_GROUP


ML_STACCOUNT_CURR=$(az storage account list -g $ML_GROUP --query "[0].name" | tr -d '"'|grep "staccmlnet")
# sprawdzamy czy konto nie zalostalo zalozone wczesniej ]
if [ -z "$ML_STACCOUNT_CURR" ]

then

  echo "$ML_STACCOUNT_CURR konto nie istnieje"
  
  az storage account create --name $ML_STACCOUNT --location $ML_LOCATION --resource-group $ML_GROUP --sku Standard_LRS

  az functionapp create --name $ML_FUNCAPP --storage-account $ML_STACCOUNT --consumption-plan-location $ML_LOCATION --resource-group $ML_GROUP

  az functionapp config appsettings set --name $ML_FUNCAPP --resource-group $ML_GROUP --settings FUNCTIONS_EXTENSION_VERSION=beta

  az storage account create --name $ML_STACCOUNT --location $ML_LOCATION --resource-group $ML_GROUP --sku Standard_LRS

  az functionapp create --name $ML_FUNCAPP --storage-account $ML_STACCOUNT --consumption-plan-location $ML_LOCATION --r#esource-group $ML_GROUP

  az functionapp config appsettings set --name $ML_FUNCAPP --resource-group $ML_GROUP --settings FUNCTIONS_EXTENSION_VERSION=beta

  az storage account keys list --account-name $ML_STACCOUNT --resource-group $ML_GROUP

else
  
   echo "$ML_STACCOUNT_CURR konto istnieje"
   
   
fi

# list w naszej grupie zasobow 
az storage account list -g $ML_GROUP -o table 

# pobieramy glowny klucz 
ACC_KEY=$(az storage account keys list --resource-group $ML_GROUP --account-name $ML_STACCOUNT_CURR --query "[0].value" | tr -d '"')

# tworzymy kontener  
az storage container create --name models --account-key $ACC_KEY --account-name $ML_STACCOUNT_CURR

az storage blob upload --account-name $ML_STACCOUNT_CURR  --account-key $ACC_KEY -c "models" -f $ML_MODEL_FILE -n $ML_MODEL_BLOB

# lista kontenerow 
az storage container list --account-name $ML_STACCOUNT_CURR --output table

# lista blobow w kontenerze
az storage blob list --container-name models --account-name $ML_STACCOUNT_CURR --output table
