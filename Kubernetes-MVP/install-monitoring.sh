
helm repo add prometheus-community https://prometheus-community.github.io/helm-charts

helm search repo prometheus-community

helm install prometheus prometheus-community/prometheus 

export POD_NAME=$(kubectl get pods --namespace default -l "app=prometheus,component=server" -o jsonpath="{.items[0].metadata.name}") 

kubectl --namespace default port-forward $POD_NAME 9090

helm repo add grafana https://grafana.github.io/helm-charts

helm search repo grafana

helm install grafana grafana/grafana

echo "Grafana password:"
kubectl get secret --namespace default grafana -o jsonpath="{.data.admin-password}" | base64 --decode ; echo

export POD_NAME=$(kubectl get pods --namespace default -l "app.kubernetes.io/name=grafana,app.kubernetes.io/instance=grafana" -o jsonpath="{.items[0].metadata.name}")

kubectl --namespace default port-forward $POD_NAME 3000