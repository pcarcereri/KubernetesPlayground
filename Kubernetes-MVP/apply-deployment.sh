chmod +x ./apply-deployment.sh

kubectl delete deployment frontend-deployment

kubectl apply -f frontend-deployment.yml