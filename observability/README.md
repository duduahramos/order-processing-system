# Stack de Observabilidade

Este repositório contém uma configuração de stack de observabilidade utilizando várias ferramentas populares para monitoramento e rastreamento. A stack inclui Jaeger, Tempo, Loki, Prometheus, Grafana, e um proxy reverso configurado com NGINX e CORS, para permitir que o OpenTelemetry (OTEL) Collector receba dados de telemetria de toda a aplicação, incluindo o client side (browser), via HTTP.

## Tecnologias Utilizadas

- **Jaeger**: Sistema de rastreamento distribuído para monitoramento de performance e diagnóstico de falhas.
- **Tempo**: Solução de rastreamento observável da Grafana Labs, integrando dados de rastreamento com o Grafana.
- **Loki**: Sistema de logs agregados que facilita a análise de logs, integrado com o Grafana.
- **Prometheus**: Sistema de monitoramento e alertas, coletando métricas e dados de performance de aplicações.
- **Grafana**: Plataforma de análise e visualização de métricas, permitindo criar dashboards dinâmicos para os dados coletados.
- **NGINX**: Proxy reverso configurado para gerenciar o tráfego e a configuração de CORS, permitindo que o OTEL Collector receba dados de telemetria de toda a aplicação, incluindo os dados do cliente (browser).

## Funcionalidade

A stack de ```OpenTelemetry``` configurada pode receber dados na porta ```4317(grpc)``` e na porta ```4318(http)```, sendo o protocolo grpc mais perfomático.
Aplicações frontend como ```angular e react```, que possuem ```server side e client side```, podem utilizar ambas as portas.
As ```requisições do browser``` devem ser feitas para a porta ```4318```, pois nela está configurada o ```proxy reverso``` com ```nginx```, que internamete transfere o fluxo para o container ```collector```

## Como Usar

### 1. Clonar o Repositório

```bash
git clone https://github.com/duduahramos/stack-observabilidade.git
```

### 2. Subir os Containers

```bash
docker compose -f .\docker-stack-observabilidade.yml up -d
```

### 3. Acessar o Grafana

[http://localhost:3333](http://localhost:3333)
