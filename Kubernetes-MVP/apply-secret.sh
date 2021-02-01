# see https://anthonychu.ca/post/aspnet-core-appsettings-secrets-kubernetes/
chmod +x ./apply-secret.sh

kubectl delete deployment api-deployment

kubectl apply -f . 