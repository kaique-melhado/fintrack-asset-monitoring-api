# FinTrack – Asset Monitoring API

**FinTrack** é uma API RESTful de alta performance para o cadastro, monitoramento e análise de ativos financeiros. Desenvolvida com foco em Clean Architecture, mensageria e processamento assíncrono, simula cenários reais do setor financeiro e bancário, integrando fontes externas como a API da Alpha Vantage.

## 🔧 Tecnologias Principais
- ASP.NET Core 8 (API REST)
- RabbitMQ + MassTransit (Mensageria)
- Hangfire (Background Jobs)
- SQL Server + MongoDB + Redis
- Python (Análise com Pandas)
- Arquitetura Limpa + SOLID + Clean Code

## 📦 Funcionalidades
- Cadastro e consulta de ativos financeiros (ações, cripto, renda fixa etc.)
- Histórico de preços com dados reais via Alpha Vantage
- Alertas de variação de preços com eventos assíncronos
- Análise de tendência e volatilidade com scripts Python (futuro)
- Pronto para migração futura em AWS (SQS, Lambda, DynamoDB)

---

> Projeto desenvolvido por Kaique Melhado como parte de seu portfólio técnico para o setor financeiro.
