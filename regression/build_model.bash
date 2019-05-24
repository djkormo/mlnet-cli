# root directory of mlnet tool 
DIR=$HOME/.dotnet/tools
# go 
$DIR/mlnet auto-train --task regression --dataset "BostonHousing.csv" --label-column-index 13 --has-header true --max-exploration-time 60