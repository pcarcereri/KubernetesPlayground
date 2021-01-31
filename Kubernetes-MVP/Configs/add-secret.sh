# see https://anthonychu.ca/post/aspnet-core-appsettings-secrets-kubernetes/
chmod +x ./add-secret.sh

# https://stackoverflow.com/questions/45879498/how-can-i-update-a-secret-on-kubernetes-when-it-is-generated-from-a-file
kubectl create secret generic secret-appsettings \
    --save-config --dry-run=client \
    --from-file=./appsettings.secrets.json \
    -o yaml | 
  kubectl apply -f -