chmod +x ./apply-deployment.sh

kubectl delete deployment api-deployment

kubectl apply -f . 