# Módulo de Agendamentos

Este módulo gerencia os agendamentos da aplicação, permitindo operações de criação, leitura, atualização e exclusão.

## Endpoints

### 1. Listar Agendamentos

**GET** `/api/v1/agendamentos`

Retorna a lista de agendamentos cadastrados na aplicação.

#### Resposta Exemplo:

```json
[
  {
    "id": 0,
    "date": "2025-03-03T18:06:14.080Z",
    "notes": "string",
    "status": "string",
    "doctor": {...},
    "user": {...},
    "appointmentRating": {...},
    "createdAt": "2025-03-03T18:06:14.080Z",
    "updatedAt": "2025-03-03T18:06:14.080Z"
  },
  { ... },
  { ... }
]
```

---

### 2. Criar um Agendamento

**POST** `/api/v1/agendamentos`

Cria um novo agendamento com as informações fornecidas.

#### Corpo da Requisição:

```json
{
  "date": "2025-03-03T18:07:25.548Z",
  "notes": "string",
  "status": "string",
  "userId": 2147483647,
  "doctorId": 2147483647,
  "appointmentRatingId": 2147483647
}
```

#### Resposta Esperada:
- **201 Created** se o agendamento for criado com sucesso.
- **422 Unprocessable Entity** se houver erro de validação.
- **400 Bad Request** se houver erro de validação.

---

### 3. Buscar um Agendamento por ID

**GET** `/api/v1/agendamentos/{id}`

Obtém os detalhes de um agendamento específico pelo seu ID.

#### Resposta Exemplo:

```json
{
  "id": 0,
  "date": "2025-03-03T18:06:14.080Z",
  "notes": "string",
  "status": "string",
  "doctor": {...},
  "user": {...},
  "appointmentRating": {...},
  "createdAt": "2025-03-03T18:06:14.080Z",
  "updatedAt": "2025-03-03T18:06:14.080Z"
}
```

#### Resposta Esperada:
- **200 OK** se o agendamento for encontrado.
- **404 Not Found** se o agendamento não existir.

---

### 4. Atualizar um Agendamento

**PUT** `/api/v1/agendamentos/{id}`

Atualiza as informações de um agendamento existente.

#### Corpo da Requisição:

```json
{
  "date": "2025-03-03T18:07:25.548Z",
  "notes": "string",
  "status": "string",
  "userId": 2147483647,
  "doctorId": 2147483647,
  "appointmentRatingId": 2147483647
}
```

#### Resposta Esperada:
- **200 OK** se a atualização for bem-sucedida.
- **422 Unprocessable Entity** se houver erro de validação.
- **400 Bad Request** se houver erro de validação.
- **404 Not Found** se o agendamento não existir.

---

### 5. Excluir um Agendamento

**DELETE** `/api/v1/agendamentos/{id}`

Remove um agendamento do sistema.

#### Resposta Esperada:
- **200 OK** se o agendamento for removido com sucesso.
- **404 Not Found** se o agendamento não existir.

---

Este módulo gerencia a entidade `Appointment`, seguindo a mesma estrutura dos módulos anteriores. As operações básicas de CRUD (Create, Read, Update, Delete) estão disponíveis, permitindo o gerenciamento completo dos agendamentos na aplicação.