# MinimalAPIDemo
This is a Minimal ASP.Net 6 API dockerized with CI-CD pipeline and Azure app kubernetes.

## generate a gitignore file
dotnet new gitignore

## kubectl commands
az aks update -n aspnetminimalapi -g minimalpaidemo --attach-acr minimalapidemo

kubectl set image deployment azure-vote-front azure-vote-front=minimalapidemo.azurecr.io/minimalapidemo:latest


kubectl logs deployment/<name-of-deployment> # logs of deployment
kubectl logs -f deployment/<name-of-deployment> # follow logs
kubectl logs deployment/coredns -n kube-system 
kubectl logs -l app=elasticsearch

kubectl run minimalapi --image=minimalapi:latest --image-pull-policy=IfNotPresent