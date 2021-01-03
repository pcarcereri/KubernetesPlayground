#!/bin/bash
chmod +x ./deploykubernetes.sh

echo "Deploying kubernetes yaml files.."
kubectl apply -f ./../KubernetesFiles/