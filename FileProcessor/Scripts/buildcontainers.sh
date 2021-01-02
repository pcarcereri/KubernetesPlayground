#!/bin/bash
chmod +x ./buildcontainers.sh

echo "Building filegenerator.."
chmod +x ./../FileGenerator/buildcontainer.sh
./../FileGenerator/buildcontainer.sh

echo "Running file processor.."
chmod +x ./../FileProcessor/buildcontainer.sh
./../FileProcessor/buildcontainer.sh
