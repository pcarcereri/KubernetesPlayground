chmod +x ./apply-deployment.sh

kubectl delete deployment api-deployment
kubectl delete pod sqlserver

kubectl apply -f . 