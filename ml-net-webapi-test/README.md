#### Based on https://docs.microsoft.com/en-us/dotnet/machine-learning/how-to-guides/serve-model-web-api-ml-net

#### Test: 

##### curl -X POST -H "Content-Type: application/json" -d '{"SentimentText":"Who are you to be my enemy!"}'  http://localhost:5000/api/predict/sentimentprediction/


##### curl -X GET -H "Content-Type: application/json" -d '{"SentimentText":"Who are you to be my enemy!"}'  http://localhost:5000/api/predict/sentimentprediction/