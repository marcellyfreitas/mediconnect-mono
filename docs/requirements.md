# MediConnect — Documento de Requisitos

## 1. Especificações de Autenticação e Autorização

### 1.1 Esquemas de Autenticação

O sistema deve implementar **dois esquemas independentes de autenticação JWT**:

| Esquema | Público | Nome do Schema | Claim de Identidade |
|---|---|---|---|
| `UserSchema` | Pacientes (usuários) | `UserSchema` | `NameIdentifier` (Id do usuário) |
| `AdminScheme` | Administradores | `AdminScheme` | `NameIdentifier` (Id do admin) |

### 1.2 Chaves e Configurações

- Cada esquema deve possuir **chave secreta própria** configurada em `appsettings.json`:
  - `JWT:UserSecretKey` — chave para emissão e validação de tokens de usuários
  - `JWT:AdminSecretKey` — chave para emissão e validação de tokens de administradores
- As chaves devem ter tamanho mínimo de 32 caracteres
- Algoritmo de assinatura: **HMAC-SHA256**

### 1.3 Tokens de Acesso (Access Token)

- **Tempo de expiração:** 1 hora
- **Claims obrigatórios:**
  - `sub` (Subject) — identificador do usuário/administrador
  - `NameIdentifier` — Id do usuário/administrador
  - `Name` — nome completo
  - `GivenName` — nome (primeiro nome)
  - `Email` — e-mail do usuário/administrador
  - `jti` — identificador único do token
  - `iat` — data/hora de emissão (Unix timestamp)

### 1.4 Tokens de Atualização (Refresh Token)

- **Tempo de expiração:** 7 dias
- Deve ser um JWT válido contendo ao menos `NameIdentifier` e `jti`
- O endpoint de refresh deve:
  - Validar o refresh token recebido
  - Emitir um novo par (Access Token + Refresh Token)
  - Invalidar o refresh token anterior

### 1.5 Políticas de Autorização

| Política | Esquema Exigido | Acesso |
|---|---|---|
| `UserPolicy` | `UserSchema` | Rotas do paciente (prefixo `/api/v1/public/`) |
| `AdminPolicy` | `AdminScheme` | Rotas administrativas (prefixo `/api/v1/` sem `public/`) |

### 1.6 Fluxo de Autenticação

1. **Registro (`/register`)**
   - Usuário fornece nome, e-mail, CPF e senha
   - Senha deve ser armazenada com hash **BCrypt** (via `BCrypt.Net-Next`)
   - E-mail deve ser único no sistema
   - Retorna os dados do usuário criado

2. **Login (`/login`)**
   - Usuário fornece e-mail e senha
   - Sistema valida credenciais comparando hash BCrypt
   - Emite Access Token (1h) + Refresh Token (7d)
   - Retorna `{ accessToken, refreshToken, user }`

3. **Refresh (`/refresh-token`)**
   - Recebe refresh token válido
   - Extrai `NameIdentifier` do token
   - Emite novo par de tokens
   - Retorna `{ accessToken, refreshToken }`

4. **Logout (`/logout`)**
   - Cliente deve descartar os tokens localmente
   - (Opcional) Adicionar refresh token a uma lista de revogação no servidor

### 1.7 Proteção de Senhas

- Algoritmo: **BCrypt** (via biblioteca `BCrypt.Net-Next`)
- Fator de custo: `workFactor = 12` (recomendado)
- Nunca armazenar senhas em texto plano
- Nunca retornar hash de senha em respostas da API

---

## 2. Requisitos por Módulo

### 2.1 Módulo de Usuários (Pacientes)

| ID | Requisito | Prioridade |
|---|---|---|
| USR-01 | O sistema deve permitir que pacientes se registrem informando nome, e-mail, CPF, senha e endereço | Alta |
| USR-02 | O sistema deve validar CPF durante o cadastro (formato e dígitos verificadores) | Alta |
| USR-03 | O sistema deve validar e-mail único — não permitir duplicatas | Alta |
| USR-04 | O sistema deve permitir que o paciente atualize seus próprios dados | Alta |
| USR-05 | O sistema deve permitir que o paciente exclua sua própria conta | Média |
| USR-06 | O administrador deve poder listar, buscar, criar, atualizar e excluir usuários | Alta |
| USR-07 | A busca de usuários deve permitir pesquisa por nome, e-mail ou CPF | Média |

### 2.2 Módulo de Administradores

| ID | Requisito | Prioridade |
|---|---|---|
| ADM-01 | O sistema deve permitir o cadastro de administradores com nome, e-mail e senha | Alta |
| ADM-02 | O sistema deve permitir login de administradores com geração de token JWT próprio | Alta |
| ADM-03 | O administrador master (seed) deve ser criado automaticamente ao iniciar o sistema | Alta |
| ADM-04 | A listagem de administradores não deve expor o administrador seed | Média |
| ADM-05 | O administrador deve poder criar, editar, listar e excluir outros administradores | Alta |

### 2.3 Módulo de Médicos

| ID | Requisito | Prioridade |
|---|---|---|
| DOC-01 | O sistema deve permitir cadastro de médicos com nome, CPF, e-mail, CRM e especialidade | Alta |
| DOC-02 | O CRM deve ser armazenado como campo único | Alta |
| DOC-03 | Cada médico deve estar associado a uma especialidade | Alta |
| DOC-04 | O administrador deve poder listar, buscar, criar, atualizar e excluir médicos | Alta |
| DOC-05 | O sistema deve permitir associar um médico a múltiplos centros médicos | Alta |
| DOC-06 | A busca de médicos deve ser feita por nome ou especialidade | Alta |
| DOC-07 | Os pacientes devem poder visualizar dados de um médico específico | Média |

### 2.4 Módulo de Especializações

| ID | Requisito | Prioridade |
|---|---|---|
| ESP-01 | O sistema deve permitir cadastro de especializações (nome, descrição) | Alta |
| ESP-02 | O administrador deve poder criar, editar, listar e excluir especializações | Alta |
| ESP-03 | Os pacientes devem poder listar todas as especializações disponíveis | Média |

### 2.5 Módulo de Centros Médicos (Unidades)

| ID | Requisito | Prioridade |
|---|---|---|
| CEN-01 | O sistema deve permitir cadastro de centros médicos com nome, telefone, e-mail e endereço | Alta |
| CEN-02 | Cada centro médico deve estar associado a um endereço | Alta |
| CEN-03 | O administrador deve poder criar, editar, listar e excluir centros médicos | Alta |
| CEN-04 | Os pacientes devem poder listar e visualizar detalhes dos centros médicos | Média |

### 2.6 Módulo de Endereços

| ID | Requisito | Prioridade |
|---|---|---|
| END-01 | O sistema deve armazenar endereços completos (logradouro, CEP, bairro, cidade, estado, país, número, complemento) | Alta |
| END-02 | O administrador deve poder criar, editar, listar e excluir endereços | Alta |
| END-03 | Os pacientes devem poder gerenciar seu próprio endereço | Alta |

### 2.7 Módulo de Agendamentos

| ID | Requisito | Prioridade |
|---|---|---|
| AGE-01 | O paciente deve poder criar um agendamento informando data, médico e centro médico | Alta |
| AGE-02 | O sistema deve gerar um número de protocolo único para cada agendamento | Alta |
| AGE-03 | O paciente deve poder listar seus próprios agendamentos | Alta |
| AGE-04 | O paciente deve poder cancelar seu próprio agendamento | Alta |
| AGE-05 | O administrador deve poder listar e gerenciar todos os agendamentos | Alta |
| AGE-06 | O sistema deve permitir consultar médicos disponíveis por especialidade | Alta |
| AGE-07 | O sistema deve permitir consultar horários disponíveis para uma data, médico e centro médico | Alta |
| AGE-08 | O status do agendamento deve ser controlado (agendado, confirmado, cancelado, concluído) | Média |

### 2.8 Módulo de Avaliações de Agendamentos

| ID | Requisito | Prioridade |
|---|---|---|
| AVA-01 | O paciente deve poder avaliar um agendamento após a consulta (nota de 1 a 5) | Média |
| AVA-02 | A avaliação deve conter nota, comentário opcional e referência ao agendamento | Média |
| AVA-03 | Cada agendamento só pode ter uma avaliação | Média |
| AVA-04 | O administrador deve poder listar todas as avaliações | Baixa |

### 2.9 Módulo de Exames

| ID | Requisito | Prioridade |
|---|---|---|
| EXA-01 | O sistema deve permitir cadastro de exames (nome, descrição, departamento, tipo, prazo, preparo) | Alta |
| EXA-02 | O administrador deve poder criar, editar, listar e excluir exames | Alta |
| EXA-03 | Os pacientes devem poder listar exames | Média |

### 2.10 Módulo de Convênios

| ID | Requisito | Prioridade |
|---|---|---|
| CON-01 | O sistema deve permitir cadastro de convênios médicos (nome) | Alta |
| CON-02 | O administrador deve poder criar, editar, listar e excluir convênios | Alta |
| CON-03 | Os pacientes devem poder listar convênios disponíveis | Média |

### 2.11 Módulo de Planos de Saúde

| ID | Requisito | Prioridade |
|---|---|---|
| PLA-01 | O sistema deve permitir cadastro de planos de saúde (nome, cobertura) | Alta |
| PLA-02 | Cada plano de saúde pode estar associado a múltiplos convênios | Alta |
| PLA-03 | O administrador deve poder criar, editar, listar e excluir planos de saúde | Alta |

---

## 3. Requisitos da API (Rotas)

### 3.1 Padrão de Resposta da API

Todas as respostas devem seguir o formato padrão:

```json
{
  "success": true,
  "message": "Operação realizada com sucesso",
  "data": { },
  "status": 200
}
```

### 3.2 Rotas de Autenticação — Usuário

**Base:** `/api/v1/auth/user`

| Método | Rota | Autenticação | Descrição |
|---|---|---|---|
| `POST` | `/login` | Pública | Realiza login do paciente. Retorna tokens JWT |
| `POST` | `/refresh-token` | Pública | Renova o par de tokens |
| `POST` | `/logout` | `UserPolicy` | Realiza logout do paciente |
| `POST` | `/register` | Pública | Cadastra um novo paciente |
| `GET` | `/usuario` | `UserPolicy` | Retorna dados do paciente autenticado |

### 3.3 Rotas de Autenticação — Administrador

**Base:** `/api/v1/auth/admin`

| Método | Rota | Autenticação | Descrição |
|---|---|---|---|
| `POST` | `/login` | Pública | Realiza login do administrador. Retorna tokens JWT |
| `POST` | `/refresh-token` | Pública | Renova o par de tokens |
| `POST` | `/logout` | `AdminPolicy` | Realiza logout do administrador |
| `POST` | `/register` | Pública | Cadastra um novo administrador |
| `GET` | `/usuario` | `AdminPolicy` | Retorna dados do administrador autenticado |

### 3.4 Rotas de Administração — Usuários

**Base:** `/api/v1/usuarios`

| Método | Rota | Autenticação | Descrição |
|---|---|---|---|
| `GET` | `/` | `AdminPolicy` | Lista todos os pacientes |
| `GET` | `/pesquisar?search=` | `AdminPolicy` | Busca pacientes por nome, e-mail ou CPF |
| `GET` | `/{id}` | `AdminPolicy` | Obtém dados de um paciente pelo ID |
| `POST` | `/` | `AdminPolicy` | Cria um novo paciente |
| `PUT` | `/{id}` | `AdminPolicy` | Atualiza dados de um paciente |
| `DELETE` | `/{id}` | `AdminPolicy` | Exclui um paciente |

### 3.5 Rotas de Administração — Administradores

**Base:** `/api/v1/administradores`

| Método | Rota | Autenticação | Descrição |
|---|---|---|---|
| `GET` | `/` | `AdminPolicy` | Lista todos os administradores |
| `GET` | `/{id}` | `AdminPolicy` | Obtém dados de um administrador pelo ID |
| `POST` | `/` | `AdminPolicy` | Cria um novo administrador |
| `PUT` | `/{id}` | `AdminPolicy` | Atualiza dados de um administrador |
| `DELETE` | `/{id}` | `AdminPolicy` | Exclui um administrador |

### 3.6 Rotas de Administração — Médicos

**Base:** `/api/v1/medicos`

| Método | Rota | Autenticação | Descrição |
|---|---|---|---|
| `GET` | `/` | `AdminPolicy` | Lista todos os médicos |
| `GET` | `/{id}` | `AdminPolicy` | Obtém dados de um médico pelo ID |
| `POST` | `/` | `AdminPolicy` | Cria um novo médico |
| `PUT` | `/{id}` | `AdminPolicy` | Atualiza dados de um médico |
| `DELETE` | `/{id}` | `AdminPolicy` | Exclui um médico |

### 3.7 Rotas de Administração — Especializações

**Base:** `/api/v1/especializacoes`

| Método | Rota | Autenticação | Descrição |
|---|---|---|---|
| `GET` | `/` | `AdminPolicy` | Lista todas as especializações |
| `GET` | `/{id}` | `AdminPolicy` | Obtém dados de uma especialização pelo ID |
| `POST` | `/` | `AdminPolicy` | Cria uma nova especialização |
| `PUT` | `/{id}` | `AdminPolicy` | Atualiza dados de uma especialização |
| `DELETE` | `/{id}` | `AdminPolicy` | Exclui uma especialização |

### 3.8 Rotas de Administração — Centros Médicos (Unidades)

**Base:** `/api/v1/unidades`

| Método | Rota | Autenticação | Descrição |
|---|---|---|---|
| `GET` | `/` | `AdminPolicy` | Lista todos os centros médicos |
| `GET` | `/{id}` | `AdminPolicy` | Obtém dados de um centro médico pelo ID |
| `POST` | `/` | `AdminPolicy` | Cria um novo centro médico |
| `PUT` | `/{id}` | `AdminPolicy` | Atualiza dados de um centro médico |
| `DELETE` | `/{id}` | `AdminPolicy` | Exclui um centro médico |

### 3.9 Rotas de Administração — Endereços

**Base:** `/api/v1/enderecos`

| Método | Rota | Autenticação | Descrição |
|---|---|---|---|
| `GET` | `/` | `AdminPolicy` | Lista todos os endereços |
| `GET` | `/{id}` | `AdminPolicy` | Obtém dados de um endereço pelo ID |
| `POST` | `/` | `AdminPolicy` | Cria um novo endereço |
| `PUT` | `/{id}` | `AdminPolicy` | Atualiza dados de um endereço |
| `DELETE` | `/{id}` | `AdminPolicy` | Exclui um endereço |

### 3.10 Rotas de Administração — Agendamentos

**Base:** `/api/v1/agendamentos`

| Método | Rota | Autenticação | Descrição |
|---|---|---|---|
| `GET` | `/` | `AdminPolicy` | Lista todos os agendamentos |
| `GET` | `/medicos?search=&especialidade=` | `AdminPolicy` | Busca médicos disponíveis |
| `GET` | `/horarios?date=&doctorId=&medicalCenterId=` | `AdminPolicy` | Consulta horários disponíveis |
| `GET` | `/{id}` | `AdminPolicy` | Obtém dados de um agendamento pelo ID |
| `POST` | `/` | `AdminPolicy` | Cria um novo agendamento |
| `PUT` | `/{id}` | `AdminPolicy` | Atualiza dados de um agendamento |
| `PUT` | `/{id}/cancelar` | `AdminPolicy` | Cancela um agendamento |
| `DELETE` | `/{id}` | `AdminPolicy` | Exclui um agendamento |

### 3.11 Rotas de Administração — Exames

**Base:** `/api/v1/exames`

| Método | Rota | Autenticação | Descrição |
|---|---|---|---|
| `GET` | `/` | `AdminPolicy` | Lista todos os exames |
| `GET` | `/{id}` | `AdminPolicy` | Obtém dados de um exame pelo ID |
| `POST` | `/` | `AdminPolicy` | Cria um novo exame |
| `PUT` | `/{id}` | `AdminPolicy` | Atualiza dados de um exame |
| `DELETE` | `/{id}` | `AdminPolicy` | Exclui um exame |

### 3.12 Rotas de Administração — Planos de Saúde

**Base:** `/api/v1/planos`

| Método | Rota | Autenticação | Descrição |
|---|---|---|---|
| `GET` | `/` | `AdminPolicy` | Lista todos os planos de saúde |
| `GET` | `/{id}` | `AdminPolicy` | Obtém dados de um plano pelo ID |
| `POST` | `/` | `AdminPolicy` | Cria um novo plano de saúde |
| `PUT` | `/{id}` | `AdminPolicy` | Atualiza dados de um plano |
| `DELETE` | `/{id}` | `AdminPolicy` | Exclui um plano de saúde |

### 3.13 Rotas de Administração — Convênios

**Base:** `/api/v1/convenios`

| Método | Rota | Autenticação | Descrição |
|---|---|---|---|
| `GET` | `/` | `AdminPolicy` | Lista todos os convênios |
| `GET` | `/{id}` | `AdminPolicy` | Obtém dados de um convênio pelo ID |
| `POST` | `/` | `AdminPolicy` | Cria um novo convênio |
| `PUT` | `/{id}` | `AdminPolicy` | Atualiza dados de um convênio |
| `DELETE` | `/{id}` | `AdminPolicy` | Exclui um convênio |

### 3.14 Rotas Públicas (Paciente) — Agendamentos

**Base:** `/api/v1/public/agendamentos`

| Método | Rota | Autenticação | Descrição |
|---|---|---|---|
| `GET` | `/` | `UserPolicy` | Lista agendamentos do paciente autenticado |
| `GET` | `/medicos?search=&especialidade=` | `UserPolicy` | Busca médicos disponíveis para agendamento |
| `GET` | `/horarios?date=&doctorId=&medicalCenterId=` | `UserPolicy` | Consulta horários disponíveis |
| `GET` | `/{id}` | `UserPolicy` | Obtém dados de um agendamento pelo ID |
| `POST` | `/` | `UserPolicy` | Cria um novo agendamento |
| `PUT` | `/{id}` | `UserPolicy` | Atualiza dados de um agendamento |
| `PUT` | `/{id}/cancelar` | `UserPolicy` | Cancela um agendamento |
| `POST` | `/avaliacao` | `UserPolicy` | Adiciona avaliação a um agendamento |
| `PUT` | `/avaliacao/{id}` | `UserPolicy` | Atualiza avaliação de um agendamento |

### 3.15 Rotas Públicas (Paciente) — Centros Médicos

**Base:** `/api/v1/public/unidades`

| Método | Rota | Autenticação | Descrição |
|---|---|---|---|
| `GET` | `/` | `UserPolicy` | Lista todos os centros médicos |
| `GET` | `/{id}` | `UserPolicy` | Obtém dados de um centro médico |

### 3.16 Rotas Públicas (Paciente) — Médicos

**Base:** `/api/v1/public/medicos`

| Método | Rota | Autenticação | Descrição |
|---|---|---|---|
| `GET` | `/{id}` | `UserPolicy` | Obtém dados de um médico |

### 3.17 Rotas Públicas (Paciente) — Especializações

**Base:** `/api/v1/public/especializacoes`

| Método | Rota | Autenticação | Descrição |
|---|---|---|---|
| `GET` | `/` | `UserPolicy` | Lista todas as especializações |

### 3.18 Rotas Públicas (Paciente) — Convênios

**Base:** `/api/v1/public/convenios`

| Método | Rota | Autenticação | Descrição |
|---|---|---|---|
| `GET` | `/` | `UserPolicy` | Lista todos os convênios |

### 3.19 Rotas Públicas (Paciente) — Exames

**Base:** `/api/v1/public/exames`

| Método | Rota | Autenticação | Descrição |
|---|---|---|---|
| `GET` | `/` | `UserPolicy` | Lista todos os exames |
| `GET` | `/{id}` | `UserPolicy` | Obtém dados de um exame pelo ID |

### 3.20 Rotas Públicas (Paciente) — Usuário

**Base:** `/api/v1/public/usuario`

| Método | Rota | Autenticação | Descrição |
|---|---|---|---|
| `PUT` | `/` | `UserPolicy` | Atualiza dados do paciente autenticado |
| `DELETE` | `/` | `UserPolicy` | Exclui a conta do paciente autenticado |

### 3.21 Rotas Públicas (Paciente) — Endereços

**Base:** `/api/v1/public/enderecos`

| Método | Rota | Autenticação | Descrição |
|---|---|---|---|
| `GET` | `/` | `UserPolicy` | Lista endereços do paciente autenticado |
| `GET` | `/{id}` | `UserPolicy` | Obtém dados de um endereço pelo ID |
| `POST` | `/` | `UserPolicy` | Cria um novo endereço para o paciente |
| `PUT` | `/{id}` | `UserPolicy` | Atualiza dados de um endereço |
| `DELETE` | `/{id}` | `UserPolicy` | Exclui um endereço |

### 3.22 Rotas Gerais

| Método | Rota | Autenticação | Descrição |
|---|---|---|---|
| `GET` | `/` | Pública | Retorna nome e versão da API |

---

## 4. Requisitos de Testes

### 4.1 Stack de Testes

| Tecnologia | Versão | Finalidade |
|---|---|---|
| **xUnit** | 2.5.3 | Framework de testes unitários |
| **Moq** | 4.20.72 | Criação de mocks para dependências |
| **FluentAssertions** | 7.0 | Asserções legíveis e expressivas |
| **EntityFrameworkCore.InMemory** | 8.x | Banco de dados em memória para testes |
| **Coverlet** | — | Medição de cobertura de código |

### 4.2 Critérios de Cobertura

- **Cobertura mínima:** 80% das linhas de código da camada de serviços (`WebApi.Services`)
- **Cobertura obrigatória:**
  - 100% dos métodos públicos das interfaces de serviço
  - Todos os fluxos de exceção (cenários de erro)
  - Todos os fluxos de sucesso (cenários felizes)

### 4.3 Estrutura dos Testes

- Cada `Service` deve ter seu próprio arquivo de teste em `webapi.tests/Services/`
- Nomenclatura: `<NomeService>Tests.cs` (ex.: `UserAuthServiceTests.cs`)
- Cada método de teste deve seguir o padrão: `<Metodo>_<Cenario>_<ResultadoEsperado>`

### 4.4 Casos de Teste por Módulo

#### AddressService (10 casos: CT-001 a CT-010)
- `GetAllAsync()`: Retornar todos ordenados por Id decrescente; lista vazia
- `GetByIdAsync(int id)`: Retornar endereço quando Id existe; null quando não existe
- `AddAsync(Address)`: Adicionar endereço válido; lançar exceção para inválido
- `UpdateAsync(Address)`: Atualizar endereço e definir UpdatedAt; exceção se não existir
- `DeleteAsync(Address)`: Remover endereço; exceção se não existir

#### AdminAuthService (15 casos: CT-011 a CT-025)
- `ValidateUserAsync(email, senha)`: Válido; senha inválida; email inexistente
- `CreateToken(Administrator)`: Token JWT válido; exceção se chave vazia; expira em 1h
- `CreateRefreshToken(Administrator)`: Token válido; expira em 7d
- `ValidateRefreshToken(refreshToken)`: Válido retorna ClaimsPrincipal; inválido retorna null
- `GetUserAsync(int id)`: Admin existe; null se não existe
- `CreateUserAsync(Administrator)`: Criar admin com sucesso
- `FindUserByEmailAsync(email)`: Email existe retorna true; não existe retorna false

#### AdministratorService (8 casos: CT-026 a CT-033)
- `GetAllAsync()`: Retornar todos exceto seed admin; lista vazia
- `GetByIdAsync(int id)`: Admin existe; null se não existe
- `AddAsync(Administrator)`: Criar com senha hasheada; exceção se erro no banco
- `UpdateAsync(Administrator)`: Atualizar com sucesso; exceção se não existe
- `DeleteAsync(Administrator)`: Remover admin comum; exceção ao remover seed admin

#### AppointmentRatingService (10 casos: CT-034 a CT-043)
- `GetAllAsync()`: Retornar todos ordenados; lista vazia
- `GetByIdAsync(int id)`: Avaliação existe; null se não existe
- `AddAsync(AppointmentRating)`: Adicionar avaliação válida; exceção se inválida
- `UpdateAsync(AppointmentRating)`: Atualizar avaliação; exceção se não existe
- `DeleteAsync(AppointmentRating)`: Remover avaliação; exceção se não existe

#### AppointmentService (16 casos: CT-044 a CT-059)
- `GetAllAsync()`: Retornar todos agendamentos; lista vazia
- `GetByIdAsync(int id)`: Agendamento existe; null se não existe
- `AddAsync(Appointment)`: Adicionar com protocolo gerado; exceção se inválido
- `UpdateAsync(Appointment)`: Atualizar com sucesso; exceção se não existe
- `DeleteAsync(Appointment)`: Remover agendamento; exceção se não existe
- `CancelAppointmentAsync(int id)`: Cancelar com sucesso; exceção se não existe
- `GetDoctorsAsync(string search, int especialidade)`: Retornar médicos filtrados; sem filtro
- `GetAvailableSlotsAsync(...)`: Retornar horários DTO; exceção se dados inválidos

#### DoctorService (8 casos: CT-060 a CT-067)
- `GetAllAsync()`: Retornar todos médicos; lista vazia
- `GetByIdAsync(int id)`: Médico existe; null se não existe
- `AddAsync(Doctor)`: Adicionar médico válido; exceção se inválido
- `UpdateAsync(Doctor)`: Atualizar médico; exceção se não existe
- `DeleteAsync(Doctor)`: Remover médico; exceção se não existe

#### HealthPlanService (8 casos: CT-068 a CT-075)
- `GetAllAsync()`: Retornar todos planos; lista vazia
- `GetByIdAsync(int id)`: Plano existe; null se não existe
- `AddAsync(HealthPlan)`: Adicionar plano válido; exceção se inválido
- `UpdateAsync(HealthPlan)`: Atualizar plano; exceção se não existe
- `DeleteAsync(HealthPlan)`: Remover plano; exceção se não existe

#### MedicalAgreementService (8 casos: CT-076 a CT-083)
- `GetAllAsync()`: Retornar todos convênios; lista vazia
- `GetByIdAsync(int id)`: Convênio existe; null se não existe
- `AddAsync(MedicalAgreement)`: Adicionar convênio; exceção se inválido
- `UpdateAsync(MedicalAgreement)`: Atualizar; exceção se não existe
- `DeleteAsync(MedicalAgreement)`: Remover; exceção se não existe

#### MedicalCenterService (8 casos: CT-084 a CT-091)
- `GetAllAsync()`: Retornar todos centros médicos; lista vazia
- `GetByIdAsync(int id)`: Centro existe; null se não existe
- `AddAsync(MedicalCenter)`: Adicionar centro; exceção se inválido
- `UpdateAsync(MedicalCenter)`: Atualizar; exceção se não existe
- `DeleteAsync(MedicalCenter)`: Remover; exceção se não existe

#### MedicalExamService (8 casos: CT-092 a CT-099)
- `GetAllAsync()`: Retornar todos exames; lista vazia
- `GetByIdAsync(int id)`: Exame existe; null se não existe
- `AddAsync(MedicalExam)`: Adicionar exame; exceção se inválido
- `UpdateAsync(MedicalExam)`: Atualizar; exceção se não existe
- `DeleteAsync(MedicalExam)`: Remover; exceção se não existe

#### SpecializationService (8 casos: CT-100 a CT-107)
- `GetAllAsync()`: Retornar todas especializações; lista vazia
- `GetByIdAsync(int id)`: Especialização existe; null se não existe
- `AddAsync(Specialization)`: Adicionar; exceção se inválido
- `UpdateAsync(Specialization)`: Atualizar; exceção se não existe
- `DeleteAsync(Specialization)`: Remover; exceção se não existe

#### UserAuthService (15 casos: CT-108 a CT-122)
- `ValidateUserAsync(email, senha)`: Válido; senha inválida; email inexistente
- `CreateToken(User)`: Token JWT válido; exceção se chave vazia; expira em 1h
- `CreateRefreshToken(User)`: Token válido; expira em 7d
- `ValidateRefreshToken(refreshToken)`: Válido retorna ClaimsPrincipal; inválido retorna null
- `GetUserAsync(int id)`: Usuário existe; null se não existe
- `CreateUserAsync(User)`: Criar usuário; exceção se email duplicado
- `FindUserByEmailAsync(email)`: Email existe retorna true; não existe retorna false

#### UserService (8 casos)
- `GetAllAsync()`: Retornar todos usuários; lista vazia
- `GetByIdAsync(int id)`: Usuário existe; null se não existe
- `AddAsync(User)`: Adicionar usuário; exceção se inválido
- `UpdateAsync(User)`: Atualizar; exceção se não existe
- `DeleteAsync(User)`: Remover; exceção se não existe

---

## 5. Requisitos Não Funcionais

### 5.1 Arquitetura

| ID | Requisito |
|---|---|
| RNF-01 | O backend deve seguir o padrão **MVC** adaptado para APIs RESTful |
| RNF-02 | A lógica de negócio deve estar isolada em serviços, separada dos controllers |
| RNF-03 | O acesso a dados deve ser feito exclusivamente via **Entity Framework Core** |
| RNF-04 | As entradas e saídas da API devem utilizar **DTOs (Data Transfer Objects)**, nunca expor entidades do domínio diretamente |
| RNF-05 | As configurações de injeção de dependência, middleware e roteamento devem ser organizadas em classes de extensão estáticas |
| RNF-06 | O mobile app deve seguir arquitetura modular com separação entre camadas de serviço, contexto, hooks e componentes |

### 5.2 Banco de Dados

| ID | Requisito |
|---|---|
| RNF-07 | O banco de dados principal deve ser **MySQL 8** |
| RNF-08 | O desenvolvimento pode utilizar **SQLite** como alternativa local |
| RNF-09 | O mapeamento objeto-relacional deve ser feito com **Pomelo.EntityFrameworkCore.MySql** |
| RNF-10 | As tabelas devem utilizar nomes em português no plural (ex.: `usuarios`, `medicos`, `agendamentos`) |
| RNF-11 | Índices únicos devem ser aplicados em campos de e-mail (usuários, administradores, médicos) e protocolo de agendamento |

### 5.3 Desempenho

| ID | Requisito |
|---|---|
| RNF-12 | O tempo de resposta da API para operações CRUD simples não deve exceder **500ms** (em ambiente de produção) |
| RNF-13 | O tempo de resposta para consultas com filtro (médicos, horários) não deve exceder **1s** |
| RNF-14 | A aplicação mobile deve suportar **cache local** de dados via AsyncStorage |

### 5.4 Segurança

| ID | Requisito |
|---|---|
| RNF-15 | Todas as senhas devem ser armazenadas com hash **BCrypt** |
| RNF-16 | Todas as rotas administrativas devem exigir token JWT com política `AdminPolicy` |
| RNF-17 | Todas as rotas de paciente devem exigir token JWT com política `UserPolicy` |
| RNF-18 | As chaves secretas JWT devem ser configuradas por ambiente e nunca comitadas no repositório |
| RNF-19 | O sistema deve usar **dois esquemas JWT separados**, cada um com sua própria chave secreta |
| RNF-20 | A validação de CPF deve ser feita no backend antes do cadastro |

### 5.5 Manutenibilidade

| ID | Requisito |
|---|---|
| RNF-21 | O código deve seguir os princípios **SOLID** |
| RNF-22 | Interfaces devem ser utilizadas para abstrair implementações de serviços |
| RNF-23 | A injeção de dependência deve ser feita via construtor |
| RNF-24 | Os seeders devem implementar a interface `ISeeder` e ser executados automaticamente na inicialização |

### 5.6 Documentação

| ID | Requisito |
|---|---|
| RNF-25 | A API deve expor documentação interativa via **Swagger** na rota `/swagger` |
| RNF-26 | O projeto deve conter um `README.md` na raiz com instruções de setup, arquitetura e funcionalidades |
| RNF-27 | Os casos de teste unitários devem ser documentados em `/docs/testes_unitarios.md` |
| RNF-28 | Este documento de requisitos deve ser mantido em `/docs/requirements.md` |

### 5.7 Frontend Mobile

| ID | Requisito |
|---|---|
| RNF-29 | O aplicativo mobile deve ser desenvolvido com **React Native 0.76+** e **Expo** |
| RNF-30 | A estilização deve utilizar **NativeWind** (Tailwind CSS para React Native) |
| RNF-31 | A navegação deve ser baseada em arquivos com **Expo Router** |
| RNF-32 | O estado global de autenticação deve ser gerenciado via **Context API** |
| RNF-33 | A comunicação com o backend deve ser feita via **Axios** |

---

## 6. Requisitos do Banco de Dados

### 6.1 Entidades e Mapeamento

| Entidade | Tabela | Descrição |
|---|---|---|
| `User` | `usuarios` | Pacientes do sistema |
| `Administrator` | `administradores` | Administradores do sistema |
| `Doctor` | `medicos` | Médicos |
| `Specialization` | `especializacoes` | Especializações médicas |
| `MedicalCenter` | `centros_medicos` | Clínicas, hospitais e unidades |
| `Appointment` | `agendamentos` | Agendamentos de consultas |
| `AppointmentRating` | `agendamento_avaliacoes` | Avaliações pós-consulta |
| `Address` | `enderecos` | Endereços |
| `MedicalExam` | `exames` | Exames médicos |
| `MedicalAgreement` | `convenios` | Convênios médicos |
| `HealthPlan` | `planos_saude` | Planos de saúde |
| `DoctorMedicalCenter` | `medico_centro_medico` | Relação N:N médico ↔ centro médico |

### 6.2 Relacionamentos

- `Doctor` N:1 `Specialization` — um médico tem uma especialização
- `Doctor` N:N `MedicalCenter` — médico pode atender em múltiplos centros (tabela intermediária `medico_centro_medico`)
- `MedicalCenter` 1:1 `Address` — centro médico possui um endereço
- `Appointment` N:1 `User` — agendamento pertence a um paciente
- `Appointment` N:1 `Doctor` — agendamento é com um médico
- `Appointment` N:1 `MedicalCenter` — agendamento ocorre em um centro
- `AppointmentRating` N:1 `Appointment` — avaliação refere-se a um agendamento
- `AppointmentRating` N:1 `User` — avaliação feita por um paciente
- `HealthPlan` N:N `MedicalAgreement` — plano pode conter múltiplos convênios (tabela intermediária)

### 6.3 Índices Únicos

- `usuarios.Email`
- `administradores.Email`
- `medicos.Email`
- `agendamentos.Protocol`

---

## 7. Regras de Negócio

| ID | Regra |
|---|---|
| RN-01 | Um paciente não pode agendar uma consulta no passado |
| RN-02 | Um agendamento só pode ser cancelado pelo próprio paciente que o criou |
| RN-03 | Um paciente não pode avaliar um agendamento que não seja seu |
| RN-04 | Cada agendamento só pode receber uma única avaliação |
| RN-05 | A nota da avaliação deve ser um valor inteiro entre 1 e 5 |
| RN-06 | O e-mail deve ser único para cada tipo de usuário (paciente, administrador, médico) |
| RN-07 | O protocolo do agendamento deve ser único e gerado automaticamente |
| RN-08 | O administrador seed (`administrador@administrador.com`) não pode ser excluído |
| RN-09 | O administrador seed não deve aparecer na listagem de administradores |
