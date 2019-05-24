# root directory of mlnet tool 
DIR=$HOME/.dotnet/tools
go 
$DIR/mlnet auto-train --task multiclass-classification --dataset "iris.csv" --label-column-index 4 --has-header true --max-exploration-time 60