#!/bin/bash
chmod +x ./runcontainers.sh

echo "Building fileparser.."
docker build -t pcarcere/fileparser:latest .
echo "Pushing fileparser.."
docker push pcarcere/fileparser:latest