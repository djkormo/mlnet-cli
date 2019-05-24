# root directory of mlnet tool 
DIR=$HOME/.dotnet/tools
# go 
$DIR/mlnet auto-train --task regression --dataset "AgeRangeData03_AgeGenderLabelEncodedMoreData.csv" --label-column-index 2 --has-header true --max-exploration-time 60