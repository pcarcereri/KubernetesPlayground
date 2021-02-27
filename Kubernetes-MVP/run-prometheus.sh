chmod +x ./run-prometheus.sh

helm repo add prometheus-community https://prometheus-community.github.io/helm-charts

helm search repo prometheus-community

helm install prometheus prometheus-community/prometheus 

export POD_NAME=$(kubectl get pods --namespace default -l "app=prometheus,component=server" -o jsonpath="{.items[0].metadata.name}") 

kubectl --namespace default port-forward $POD_NAME 9090