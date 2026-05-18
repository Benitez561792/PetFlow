# 🐾 PetFlow API

**C# · .NET 8 · ASP.NET Core · Oracle Database · Entity Framework Core · Swagger/OpenAPI**

---

## 📋 Sumário

- [Sobre o Projeto](#sobre-o-projeto)
- [Equipe](#equipe)
- [Objetivo do Challenge](#objetivo-do-challenge)
- [Tecnologias Utilizadas](#tecnologias-utilizadas)
- [Arquitetura do Projeto](#arquitetura-do-projeto)
- [Visão de Domínio](#visão-de-domínio)
- [Documentação das Rotas](#documentação-das-rotas)
- [Banco de Dados](#banco-de-dados)
- [Instruções de Instalação e Execução](#instruções-de-instalação-e-execução)
- [Testes da API](#testes-da-api)
- [Observações Finais](#observações-finais)

---

## 📌 Sobre o Projeto

O **PetFlow** é uma API REST desenvolvida em **C# com ASP.NET Core (.NET 8)** para gerenciamento de saúde preventiva de pets. A proposta central é a **gamificação do cuidado animal**: toda vez que um tutor registra um evento de saúde concluído para seu pet (vacina, consulta, banho medicado, etc.), ele acumula pontos que podem ser trocados por cupons de desconto em clínicas parceiras.

O sistema gerencia o ciclo completo: cadastro de tutores e pets, vínculo com clínicas e planos de saúde, assinatura de planos, histórico de eventos clínicos, acúmulo de pontos de recompensa, emissão de cupons e registro de resgates.

O projeto demonstra uma aplicação backend robusta com:
- arquitetura em camadas bem definida
- persistência relacional via Oracle Database
- mapeamento objeto-relacional com Entity Framework Core
- transformação de dados com AutoMapper e DTOs
- documentação automática e interativa via Swagger
- tratamento global de exceções com mensagens amigáveis

---

## 👥 Equipe

| Nome | RM |
|------|----|
| Lucas Grillo Alcântara | 561413 |
| Pietro Ferreira Gomes Abrahamian | 561469 |
| Pedro Peres Benitez | 561792 |
| Lucca Ramos Mussumecci | 562027 |

**Turma:** 2TDSPX

---

## 🎯 Objetivo do Challenge

Desenvolver uma solução utilizando C# e ASP.NET Core capaz de:

- persistir dados em banco relacional Oracle
- gerenciar informações de saúde pet com lógica de gamificação por pontos
- aplicar Programação Orientada a Objetos com entidades bem definidas
- utilizar Entity Framework Core com relacionamentos entre entidades
- garantir validações e tratamento de exceções para erros Oracle
- respeitar os fundamentos de APIs REST (verbos HTTP, códigos de status, recursos)
- disponibilizar documentação interativa via Swagger/OpenAPI
- atender aos requisitos técnicos da disciplina

---

## 🛠️ Tecnologias Utilizadas

| Tecnologia | Versão | Uso |
|------------|--------|-----|
| .NET / ASP.NET Core | 8.0 | Framework principal da API |
| Entity Framework Core | 8.0.13 | ORM e migrações de banco |
| Oracle.EntityFrameworkCore | 8.23.60 | Provider Oracle para EF Core |
| AutoMapper | 16.1.1 | Mapeamento entre entidades e DTOs |
| Swashbuckle.AspNetCore | 7.2.0 | Geração do Swagger/OpenAPI |
| Oracle Database | — | Banco de dados relacional |
| System.Text.Json | — | Serialização JSON com suporte a enums |

---

## 🧱 Arquitetura do Projeto

O projeto segue uma arquitetura em camadas com separação clara de responsabilidades:

```
PetFlowAPI/
├── Controllers/         # Camada de apresentação — endpoints REST
│   ├── TutorController.cs
│   ├── PetController.cs
│   ├── ClinicController.cs
│   ├── PlanController.cs
│   ├── SubscriptionController.cs
│   ├── HealthEventController.cs
│   ├── CouponController.cs
│   └── RedeemController.cs
│
├── Data/
│   └── PetFlowContext.cs        # DbContext do Entity Framework Core
│
├── DTOs/
│   └── PetFlowDTOs.cs           # Objetos de entrada e saída (Request/Response)
│
├── Enums/
│   └── PetFlowEnums.cs          # Enumerações: CouponStatus, HealthEventStatus, SubscriptionStatus
│
├── Mappings/
│   └── MappingProfile.cs        # Configurações do AutoMapper
│
├── Migrations/                  # Migrações geradas pelo EF Core
│
├── Models/
│   └── PetFlowModels.cs         # Entidades mapeadas para as tabelas Oracle
│
├── appsettings.json             # Configurações da aplicação (connection string)
└── Program.cs                   # Bootstrap, middlewares e injeção de dependências
```

---

## 🧠 Visão de Domínio

| Entidade | Descrição |
|----------|-----------|
| 👤 **Tutor** | Responsável pelos pets. Possui endereços, pontos de recompensa e histórico de resgates. |
| 🐾 **Pet** | Animal vinculado a um tutor e a uma espécie. Possui eventos de saúde, assinaturas de planos e score de risco. |
| 🏥 **Clínica** | Veterinária parceira do sistema. Oferece planos e registra eventos de saúde. |
| 📄 **Plano** | Plano de saúde vinculado a uma clínica, com duração em dias e multiplicador de pontos por evento. |
| 📅 **Assinatura** | Contratação de um plano por um pet. Controla período e status. |
| ❤️ **Evento de Saúde** | Registro clínico e preventivo do pet (vacina, consulta, banho, etc.). |
| 🎟️ **Cupom** | Cupom de desconto gerado via template, com código único, status e validade. |
| 🎫 **Resgate** | Uso de um cupom pelo tutor, consumindo pontos acumulados. |
| 🔢 **Ponto de Recompensa** | Pontos acumulados pelo tutor por ações realizadas. |
| ⚠️ **Score de Risco** | Pontuação de risco calculada por pet, classificada em faixas de nível de risco. |

---

## 📦 Documentação das Rotas

A API roda em `http://localhost:5000`. Todos os recursos suportam paginação via `?page=0&size=10`.

> Acesse a documentação interativa completa em: **`http://localhost:5000/swagger`**

---

### 👤 Tutores — `/tutors`

| Método | Rota | Descrição | Parâmetros de Query |
|--------|------|-----------|---------------------|
| `GET` | `/tutors` | Lista tutores com paginação e filtro por nome | `name`, `page`, `size` |
| `GET` | `/tutors/{id}` | Busca tutor pelo ID | — |
| `POST` | `/tutors` | Cadastra novo tutor | — |
| `PUT` | `/tutors/{id}` | Atualiza dados do tutor | — |
| `DELETE` | `/tutors/{id}` | Remove tutor e todos os dependentes em cascata | — |

**Request body (POST / PUT):**
```json
{
  "name": "Maria Silva",
  "email": "maria@email.com",
  "phone": "11999999999",
  "password": "senha123"
}
```

**Response (200 / 201):**
```json
{
  "id": 1,
  "name": "Maria Silva",
  "email": "maria@email.com",
  "phone": "11999999999",
  "createdAt": "2026-05-17T10:00:00"
}
```

> O DELETE remove em cascata: resgates, pontos de recompensa, endereços, pets e todos os filhos dos pets (eventos, assinaturas, scores de risco).

---

### 🐾 Pets — `/pets`

| Método | Rota | Descrição | Parâmetros de Query |
|--------|------|-----------|---------------------|
| `GET` | `/pets` | Lista pets com paginação e filtro por nome | `name`, `page`, `size` |
| `GET` | `/pets/{id}` | Busca pet pelo ID | — |
| `POST` | `/pets` | Cadastra novo pet | — |
| `PUT` | `/pets/{id}` | Atualiza dados do pet | — |
| `DELETE` | `/pets/{id}` | Remove pet e seus eventos, assinaturas e scores de risco | — |

**Request body (POST / PUT):**
```json
{
  "name": "Rex",
  "breed": "Labrador",
  "birthDate": "2020-03-15",
  "weight": 28.5,
  "speciesId": 1,
  "tutorId": 1
}
```

**Response (200 / 201):**
```json
{
  "id": 1,
  "name": "Rex",
  "breed": "Labrador",
  "birthDate": "2020-03-15T00:00:00",
  "weight": 28.5,
  "speciesId": 1,
  "tutorId": 1,
  "createdAt": "2026-05-17T10:00:00"
}
```

---

### 🏥 Clínicas — `/clinics`

| Método | Rota | Descrição | Parâmetros de Query |
|--------|------|-----------|---------------------|
| `GET` | `/clinics` | Lista clínicas com paginação e filtro por nome | `name`, `page`, `size` |
| `GET` | `/clinics/{id}` | Busca clínica pelo ID | — |
| `POST` | `/clinics` | Cadastra nova clínica | — |
| `PUT` | `/clinics/{id}` | Atualiza dados da clínica | — |
| `DELETE` | `/clinics/{id}` | Remove clínica e todos os dependentes em cascata | — |

**Request body (POST / PUT):**
```json
{
  "name": "Clínica Vet Saúde",
  "address": "Rua das Flores, 100 - São Paulo/SP",
  "phone": "1133334444",
  "cnpj": "12.345.678/0001-99"
}
```

**Response (200 / 201):**
```json
{
  "id": 1,
  "name": "Clínica Vet Saúde",
  "address": "Rua das Flores, 100 - São Paulo/SP",
  "phone": "1133334444",
  "cnpj": "12.345.678/0001-99",
  "createdAt": "2026-05-17T10:00:00"
}
```

> O DELETE remove em cascata: planos, assinaturas, eventos de saúde, descontos de parceiros, templates de cupom, cupons e resgates.

---

### 📄 Planos — `/plans`

| Método | Rota | Descrição | Parâmetros de Query |
|--------|------|-----------|---------------------|
| `GET` | `/plans` | Lista planos com paginação e filtro por nome | `name`, `page`, `size` |
| `GET` | `/plans/{id}` | Busca plano pelo ID | — |
| `POST` | `/plans` | Cadastra novo plano | — |
| `PUT` | `/plans/{id}` | Atualiza dados do plano | — |
| `DELETE` | `/plans/{id}` | Remove plano e suas assinaturas vinculadas | — |

**Request body (POST / PUT):**
```json
{
  "name": "Plano Premium",
  "description": "Cobertura completa preventiva",
  "price": 89.90,
  "durationDays": 365,
  "pointsPerEvent": 2,
  "clinicId": 1
}
```

**Response (200 / 201):**
```json
{
  "id": 1,
  "name": "Plano Premium",
  "description": "Cobertura completa preventiva",
  "price": 89.90,
  "durationDays": 365,
  "pointsPerEvent": 2,
  "clinicId": 1
}
```

---

### 📅 Assinaturas — `/subscriptions`

| Método | Rota | Descrição | Parâmetros de Query |
|--------|------|-----------|---------------------|
| `GET` | `/subscriptions` | Lista assinaturas com paginação, filtro por pet e status | `petId`, `status`, `page`, `size`, `sortBy`, `direction` |
| `GET` | `/subscriptions/{id}` | Busca assinatura pelo ID | — |
| `POST` | `/subscriptions` | Cadastra nova assinatura | — |
| `PUT` | `/subscriptions/{id}` | Atualiza dados da assinatura | — |
| `PUT` | `/subscriptions/{id}/status` | Atualiza apenas o status da assinatura | `status` (query param) |
| `DELETE` | `/subscriptions/{id}` | Remove assinatura | — |

**Status disponíveis:** `ATIVO` · `ENCERRADO` · `CANCELADO` · `EXPIRADO`

**Request body (POST / PUT):**
```json
{
  "startDate": "2026-01-01",
  "endDate": "2026-12-31",
  "status": "ATIVO",
  "petId": 1,
  "planId": 1
}
```

**Response (200 / 201):**
```json
{
  "id": 1,
  "startDate": "2026-01-01T00:00:00",
  "endDate": "2026-12-31T00:00:00",
  "status": "ATIVO",
  "petId": 1,
  "planId": 1,
  "createdAt": "2026-05-17T10:00:00"
}
```

**Atualizar status:**
```
PUT /subscriptions/1/status?status=ENCERRADO
```

---

### ❤️ Eventos de Saúde — `/health-events`

| Método | Rota | Descrição | Parâmetros de Query |
|--------|------|-----------|---------------------|
| `GET` | `/health-events` | Lista eventos com paginação, filtro por pet e status | `petId`, `status`, `page`, `size`, `sortBy`, `direction` |
| `GET` | `/health-events/{id}` | Busca evento pelo ID | — |
| `POST` | `/health-events` | Registra novo evento de saúde | — |
| `PUT` | `/health-events/{id}` | Atualiza dados do evento | — |
| `DELETE` | `/health-events/{id}` | Remove evento de saúde | — |

**Status disponíveis:** `AGENDADO` · `REALIZADO` · `CANCELADO`

**Request body (POST / PUT):**
```json
{
  "description": "Vacinação antirrábica anual",
  "eventDate": "2026-06-10T10:00:00",
  "status": "AGENDADO",
  "eventTypeId": 1,
  "petId": 1,
  "clinicId": 1
}
```

**Response (200 / 201):**
```json
{
  "id": 1,
  "description": "Vacinação antirrábica anual",
  "eventDate": "2026-06-10T10:00:00",
  "status": "AGENDADO",
  "eventTypeId": 1,
  "petId": 1,
  "clinicId": 1,
  "createdAt": "2026-05-17T10:00:00"
}
```

---

### 🎟️ Cupons — `/coupons`

| Método | Rota | Descrição | Parâmetros de Query |
|--------|------|-----------|---------------------|
| `GET` | `/coupons` | Lista cupons com paginação e filtro por código | `code`, `page`, `size` |
| `GET` | `/coupons/{id}` | Busca cupom pelo ID | — |
| `POST` | `/coupons` | Cadastra novo cupom (valida `expirationDate`) | — |
| `PUT` | `/coupons/{id}` | Atualiza dados do cupom (valida `expirationDate`) | — |
| `PUT` | `/coupons/{id}/status` | Atualiza apenas o status do cupom | `status` (query param) |
| `DELETE` | `/coupons/{id}` | Remove cupom e seus resgates vinculados | — |

**Status disponíveis:** `DISPONIVEL` · `RESGATADO` · `UTILIZADO`

> A API valida que `expirationDate` não pode ser uma data no passado. Retorna `400` caso contrário.

**Request body (POST / PUT):**
```json
{
  "code": "PETFLOW2026",
  "status": "DISPONIVEL",
  "expirationDate": "2026-12-31",
  "templateId": 1
}
```

**Response (200 / 201):**
```json
{
  "id": 1,
  "code": "PETFLOW2026",
  "status": "DISPONIVEL",
  "expirationDate": "2026-12-31T00:00:00",
  "templateId": 1,
  "createdAt": "2026-05-17T10:00:00"
}
```

**Atualizar status:**
```
PUT /coupons/1/status?status=UTILIZADO
```

---

### 🎫 Resgates — `/redeems`

| Método | Rota | Descrição | Parâmetros de Query |
|--------|------|-----------|---------------------|
| `GET` | `/redeems` | Lista resgates com paginação, filtro por tutor e ordenação | `tutorId`, `page`, `size`, `sortBy`, `direction` |
| `GET` | `/redeems/{id}` | Busca resgate pelo ID | — |
| `POST` | `/redeems` | Registra novo resgate de cupom | — |
| `DELETE` | `/redeems/{id}` | Remove resgate | — |

**Request body (POST):**
```json
{
  "pointsUsed": 150,
  "tutorId": 1,
  "couponId": 1
}
```

**Response (200 / 201):**
```json
{
  "id": 1,
  "pointsUsed": 150,
  "tutorId": 1,
  "couponId": 1,
  "createdAt": "2026-05-17T10:00:00"
}
```

---

### ⚠️ Respostas de Erro

A API trata globalmente os erros Oracle com respostas padronizadas:

| Código HTTP | Situação |
|-------------|----------|
| `400` | Campo obrigatório ausente (`ORA-01400`) ou valor fora do limite (`ORA-01438`) |
| `400` | `expirationDate` no passado (validação de negócio nos cupons) |
| `404` | Recurso não encontrado |
| `409` | Registro duplicado — email ou código já existente (`ORA-00001`) |
| `409` | Bloqueio por dependência de FK (`ORA-02292`) |
| `500` | Erro interno inesperado |

---

## 🗃️ Banco de Dados

O projeto utiliza **Oracle Database**. O script completo está disponível em `clyvo_vet_corrigido.sql`.

### Tabelas

| Tabela | Descrição |
|--------|-----------|
| `TUTOR` | Tutores cadastrados |
| `ADDRESS` | Endereços dos tutores |
| `PET` | Pets vinculados a tutores e espécies |
| `SPECIES` | Espécies disponíveis (cão, gato, etc.) |
| `CLINIC` | Clínicas veterinárias parceiras |
| `PLAN` | Planos de saúde das clínicas |
| `SUBSCRIPTION` | Assinaturas de planos por pets |
| `HEALTH_EVENT` | Eventos de saúde registrados |
| `EVENT_TYPE` | Tipos de evento com pontuação |
| `COUPON` | Cupons de desconto emitidos |
| `COUPON_TEMPLATE` | Templates de cupom vinculados a parceiros |
| `PARTNER_DISCOUNT` | Descontos de clínicas parceiras |
| `REDEEM` | Resgates de cupons por tutores |
| `REWARD_POINT` | Pontos acumulados pelos tutores |
| `REWARD_ACTION` | Tipos de ação que geram pontos |
| `RISK_SCORE` | Score de risco calculado por pet |
| `RISK_LEVEL` | Faixas de classificação de risco |

---

## 🚀 Instruções de Instalação e Execução

### Pré-requisitos

Antes de iniciar, certifique-se de ter instalado:

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- Oracle Database (local ou remoto) com acesso configurado
- Um cliente Oracle (SQL Developer, DBeaver ou sqlplus) para executar o script SQL

---

### Passo 1 — Clonar o repositório

```bash
git clone (https://github.com/Benitez561792/PetFlow.git)
cd petflow-api/PetFlowAPI
```

---

### Passo 2 — Configurar o banco de dados

Execute o script SQL no seu banco Oracle para criar todas as tabelas e inserir os dados iniciais:

```sql
-- No SQL Developer, DBeaver ou sqlplus:
@clyvo_vet_corrigido.sql
```

---

### Passo 3 — Configurar a string de conexão

Edite o arquivo `appsettings.json` na raiz do projeto com as credenciais do seu Oracle:

```json
{
  "ConnectionStrings": {
    "OracleConnection": "User Id=SEU_USUARIO;Password=SUA_SENHA;Data Source=SEU_HOST:1521/XEPDB1;"
  }
}
```

> Para Oracle XE local, o `Data Source` geralmente é `localhost:1521/XEPDB1`.

---

### Passo 4 — Restaurar dependências

```bash
dotnet restore
```

---

### Passo 5 — Executar a aplicação

```bash
dotnet run
```

A API estará disponível em:

```
http://localhost:5000
```

A documentação Swagger estará disponível em:

```
http://localhost:5000/swagger
```

---

### Passo 6 (opcional) — Aplicar migrações EF Core

Caso prefira usar as migrações do EF Core em vez do script SQL manual:

```bash
dotnet ef database update
```

> Certifique-se de que a connection string está corretamente configurada antes de executar este comando.

---

### Resumo rápido

```bash
git clone (https://github.com/Benitez561792/PetFlow.git)
cd petflow-api/PetFlowAPI
# 1. Execute o script clyvo_vet_corrigido.sql no Oracle
# 2. Edite appsettings.json com suas credenciais Oracle
dotnet restore
dotnet run
# Acesse: http://localhost:5000/swagger
```

---

## 🧪 Testes da API

Os endpoints podem ser testados via **Swagger UI**, **Postman** ou **Insomnia**.

Fluxo sugerido respeitando as dependências de FK:

1. `POST /clinics` — cadastrar uma clínica
2. `POST /plans` — criar um plano vinculado à clínica
3. `POST /tutors` — cadastrar um tutor
4. `POST /pets` — cadastrar um pet vinculado ao tutor
5. `POST /subscriptions` — assinar um plano para o pet
6. `POST /health-events` — registrar eventos de saúde para o pet
7. `POST /coupons` — criar um cupom
8. `POST /redeems` — resgatar o cupom usando pontos do tutor
9. Testar filtros: `GET /health-events?petId=1&status=REALIZADO`
10. Testar paginação: `GET /tutors?page=0&size=5`
11. Testar atualização de status: `PUT /subscriptions/1/status?status=ENCERRADO`

---

## 🧭 Observações Finais

O PetFlow foi desenvolvido com foco em:

- **Arquitetura em camadas** clara: Controllers → DTOs → AutoMapper → Models → EF Core → Oracle
- **Tratamento de exceções Oracle** com mensagens amigáveis ao cliente (sem expor stack trace)
- **Deleção em cascata manual** respeitando as restrições de FK do Oracle
- **Documentação automática** completa via Swagger/OpenAPI
- **Padrão REST** com verbos HTTP semânticos e códigos de status corretos

Desenvolvido como parte do **Challenge 2TDSPX — FIAP 2026**
