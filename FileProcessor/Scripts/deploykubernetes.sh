#!/bin/bash
chmod +x ./deploykubernetes.sh

echo "Deleting previous deployments.."
kubectl delete --all deployments

echo "Deploying kubernetes yaml files.."
kubectl apply -f ./../KubernetesFiles/