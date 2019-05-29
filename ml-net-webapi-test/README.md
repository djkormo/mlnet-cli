#### Based on https://docs.microsoft.com/en-us/dotnet/machine-learning/how-to-guides/serve-model-web-api-ml-net

#### Build 

#### docker build -t ml-net-webapp:1 .

#### Run

#### docker run -d -p 5000:80 ml-net-webapp:1

#### Test: 


##### Iris-versicolor
##### curl -X POST "http://localhost:5000/api/Predict/model" -H "accept: text/plain" -H "Content-Type: application/json-patch+json" -d "{ \"sepalLength\": 4, \"sepalWidth\": 2, \"petalLength\": 3, \"petalWidth\": 3, \"label\": \"string\"}"

##### Iris-setosa
##### curl -X POST "http://localhost:5000/api/Predict/model" -H "accept: text/plain" -H "Content-Type: application/json-patch+json" -d "{ \"sepalLength\": 1.1, \"sepalWidth\": 1.1, \"petalLength\": 2.1, \"petalWidth\": 2.1, \"label\": \"string\"}"

##### Iris-virginica
##### curl -X POST "http://localhost:5000/api/Predict/model" -H "accept: text/plain" -H "Content-Type: application/json-patch+json" -d "{ \"sepalLength\": 3.1, \"sepalWidth\": 3.1, \"petalLength\": 2.1, \"petalWidth\": 2.1, \"label\": \"string\"}"