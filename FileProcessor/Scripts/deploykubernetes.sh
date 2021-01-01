#!/bin/bash
chmod +x ./deploykubernetes.sh

echo "Deploying kubernetes-deployment.yml.."
kubectl apply -f ./../KubernetesFiles/kubernetes-deployment.yml