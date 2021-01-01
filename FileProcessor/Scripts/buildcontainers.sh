#!/bin/bash
hmod +x ./buildcontainers.sh

echo "Building filegenerator.."
./FileGenerator/buildcontainer.sh

echo "Running fileparser.."
./FileParser/buildcontainer.sh
