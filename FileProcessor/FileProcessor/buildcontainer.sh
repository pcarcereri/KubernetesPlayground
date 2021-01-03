#!/bin/bash
chmod +x ./buildcontainer.sh

echo "Building file processor.."
docker build -t pcarcere/fileprocessor:latest .
echo "Pushing file processor.."
docker push pcarcere/fileprocessor:latest