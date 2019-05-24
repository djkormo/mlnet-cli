#!/bin/bash
# root directory of mlnet tool 
DIR=$HOME/.dotnet/tools
# go 
$DIR/mlnet auto-train --task binary-classification --dataset "yelp_labelled.txt" --label-column-index 1 --has-header false --max-exploration-time 60