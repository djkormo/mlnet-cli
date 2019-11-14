#!/bin/bash
# build 
docker build -t ml-net-webapp:v1 .
# run 
docker run -d -p 5000:80 ml-net-webapp:v1 
