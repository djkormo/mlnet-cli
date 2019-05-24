# root directory of mlnet tool 
DIR=$HOME/.dotnet/tools
# go 
$DIR/mlnet auto-train --task regression --dataset "USA_Housing 	.csv" --label-column-index 5 --has-header true --max-exploration-time 60