#!/bin/bash
chmod +x ./runcontainers.sh

echo "Running filegenerator.."
docker run --name filegenerator --mount source=files,target=/app/files pcarcere/filegenerator:latest 
docker container prune -f

echo "Running fileparser.."
docker run --name fileparser --mount source=files,target=/app/files pcarcere/fileparser:latest 
docker container prune -f