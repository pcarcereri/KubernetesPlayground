#!/bin/bash
chmod +x ./buildcontainers.sh

echo "Building filegenerator.."
chmod +x ./../FileGenerator/buildcontainer.sh
./../FileGenerator/buildcontainer.sh

echo "Running fileparser.."
chmod +x ./../FileParser/buildcontainer.sh
./../FileParser/buildcontainer.sh
