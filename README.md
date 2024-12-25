# Test WCF Client Server App

This is a test repo for simple client server WCF app to reproduce issue about slow wcf over tcp in aws eks.

## Build Docker Images

Use this command to build docker images usin docker compose:

```bash
docker compose build
```

## Run Docker Containers in Docker Compose

Use this command to run docker containers using docker compose:

```bash
docker compose up
```

## Run Docker Containers in Kubernetes

Use this command to run docker containers in kubernetes:

```bash
kubectl apply -f ./k8s.yaml
```
