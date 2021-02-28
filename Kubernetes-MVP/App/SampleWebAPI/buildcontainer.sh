#!/bin/bash
chmod +x ./buildcontainer.sh

echo "Building samplewebapi.."
docker build -t pcarcere/samplewebapi:latest .
echo "Pushing file samplewebapi.."
docker push pcarcere/samplewebapi:latest
