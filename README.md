# PongSenaiKinect

![.NET 8.0](https://img.shields.io/badge/.NET-8.0-blue) ![C#](https://img.shields.io/badge/Language-C%23-blueviolet) ![MIT License](https://img.shields.io/badge/License-MIT-lightgrey)

> Projeto de Jogo Pong controlado por sensor Kinect com backend em ASP.NET Core para gestão de usuários e pontuação, reconhecimento facial e tracking de mãos.

> O projeto foi uma proposta de atividade do curso Senai Celso Charuri, a ideia principal era conseguir fazer 
um "kinect" com a webcam do computador, qual leria nossos movimentos e jogariamos um jogo dessa forma. Infelizmente o projeto não teve fim, porém ainda tem códigos muito interessantes

>A proposta era fazer um Pong Game, para ser 1 jogador vs 1 jogador, para isso utilizariamos trackings das mãos dos jogadores, a qual seria previamente registrado. Também teria registro e reconhecimento facial para analise de resultados e login no jogo, que seria feito com algoritimos de K-Means utilizando a paleta de cores especifica da pessoa, o que trás diversos problemas como diferença de iluminação, porém foi a forma mais facil encontrada. 

---

## 📚 Sumário

- [Sobre o Projeto](#sobre-o-projeto)
- [Tecnologias](#tecnologias)
- [Pré-requisitos](#pré-requisitos)
- [Estrutura do Repositório](#estrutura-do-repositório)
- [Backend](#backend)
  - [Inicialização do Banco de Dados](#inicialização-do-banco-de-dados)
  - [Executar API](#executar-api)
  - [Endpoints](#endpoints)
- [Frontend](#frontend)
  - [Hand Tracking & Blur](#hand-tracking--blur)
  - [Reconhecimento Facial (KMeans)](#reconhecimento-facial-kmeans)
  - [Interface Desktop (Jogo Pong)](#interface-desktop-jogo-pong)
- [Contribuindo](#contribuindo)
- [Licença](#licença)

---

## 💡 Sobre o Projeto

O **PongSenaiKinect** combina três componentes principais:

1. **Backend**: API RESTful em ASP.NET Core que persiste usuários, face data e placares no SQL Server.
2. **Reconhecimento Facial**: Módulo que usa KMeans para comparar arrays de cores representando dados faciais.
3. **Frontend**:
   - **Hand Tracking & Blur**: Aplica blur em background com base na posição da mão detectada pelo Kinect.
   - **Interface Desktop**: Jogo Pong onde as raquetes são controladas pelo movimento das mãos.

Use este projeto para aprendizado de integração entre sensores, visão computacional e aplicações full‑stack .NET.

---

## 🚀 Tecnologias

| Componente             | Tecnologia / Framework                |
|------------------------|---------------------------------------|
| Backend API            | ASP.NET Core 8.0, Entity Framework   |
| Banco de Dados         | SQL Server                            |
| Serviços               | C# (.NET 8), EF Core                  |
| Reconhecimento Facial  | .NET Console, KMeans (C#)            |
| Hand Tracking & Blur   | Kinect SDK, WinForms                  |
| Interface Desktop      | WinForms (.NET 8), C#                 |

---

## ✅ Pré-requisitos

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- SQL Server (local ou remoto)
- Sensor Kinect e [Kinect SDK](https://www.microsoft.com/en-us/download/details.aspx?id=44561)
- Visual Studio 2022 ou superior (para projetos WinForms)

---

## 🗂️ Estrutura do Repositório

```
PongSenaiKinect/
├── backend/                 # API ASP.NET Core
│   ├── Controllers/         # UserController.cs
│   ├── Model/               # EF Core DbContext e entidades
│   ├── Services/            # UserService (KMeans verify)
│   ├── script.sql           # Criação do banco e tabelas
│   └── Program.cs           # Configuração e Swagger
├── frontend/
│   ├── hand/                # Projeto Hand Tracking & Blur
│   ├── recognition/         # SenaiRecognition (KMeans app)
│   └── interface/FrontendDesktop/  # Jogo Pong WinForms
└── LICENSE                  # MIT
```

---

## Backend

### 📋 Inicialização do Banco de Dados

1. Abra o **SQL Server Management Studio** ou seu cliente preferido.
2. Execute o script `backend/script.sql` para criar **PongGameDB** e tabelas `Usuario`, `Score` e `RGB`.

```sql
USE master;
GO
-- Remove antiga base
IF EXISTS(select * from sys.databases where name = 'PongGameDB')
  DROP DATABASE PongGameDB;
GO
CREATE DATABASE PongGameDB;
GO
USE PongGameDB;
GO
-- Cria tabelas Usuario, Score, RGB...
```

### ▶️ Executar API

```bash
cd backend
dotnet build
dotnet run
```
A API estará em `https://localhost:5001`. Acesse a documentação interativa em `https://localhost:5001/swagger`.

### 📡 Endpoints

| Método | Rota                     | Descrição                           |
|--------|--------------------------|-------------------------------------|
| GET    | `/User`                  | Registra usuário: `Usuario` e `Color[]` no corpo da query. |
| GET    | `/User/login/{id}`       | Login facial: envia `Usuario` e `Color[]` para verificação.
|

---

## Frontend

### 🖐️ Hand Tracking & Blur

Projeto console/WinForms que:
- Conecta ao Kinect para detectar posição da mão.
- Aplica blur no background fora da região da mão.

```bash
cd frontend/hand
# Abra a solução HandForms.sln no VS e execute.
```

### 🧠 Reconhecimento Facial (KMeans)

Aplicativo console que:
- Lê imagens de `TestImages/`.
- Extrai arrays de cores.
- Compara com dados armazenados via algoritmo KMeans.

```bash
cd frontend/recognition
dotnet build
dotnet run
```

### 🖥️ Interface Desktop (Jogo Pong)

Formulário WinForms onde:
- As raquetes respondem ao movimento das mãos trackeadas.
- Pontuação é enviada ao backend.

1. Abra `frontend/interface/FrontendDesktop/FrontendDesktop.sln` no Visual Studio.
2. Compile e execute.

---
