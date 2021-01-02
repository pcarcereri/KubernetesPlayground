#!/bin/bash
chmod +x ./buildcontainer.sh

echo "Building filegenerator.."
docker build -t pcarcere/filegenerator:latest .
echo "Pushing filegenerator.."
docker push pcarcere/filegenerator:latest
