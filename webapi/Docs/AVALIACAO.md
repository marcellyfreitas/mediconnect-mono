# Módulo de Avaliações de Agendamentos

Este módulo gerencia as avaliações de agendamentos da aplicação, permitindo operações de criação, leitura, atualização e exclusão.

## Endpoints

### 1. Listar Avaliações de Agendamentos

**GET** `/api/v1/agendamento-avaliacoes`

Retorna a lista de avaliações de agendamentos cadastradas na aplicação.

#### Resposta Exemplo:

```json
[
  {
    "id": 0,
    "rating": 0,
    "comment": "string",
    "appointmentId": 0,
    "appointment": {...},
    "user": {...},
    "createdAt": "2025-03-03T18:09:14.162Z",
    "updatedAt": "2025-03-03T18:09:14.162Z",
  },
  { ... },
  { ... }
]
```

---

### 2. Criar uma Avaliação de Agendamento

**POST** `/api/v1/agendamento-avaliacoes`

Cria uma nova avaliação de agendamento com as informações fornecidas.

#### Corpo da Requisição:

```json
{
  "rating": 5,
  "comment": "string",
  "appointmentId": 2147483647,
  "userId": 2147483647
}
```

#### Resposta Esperada:
- **201 Created** se a avaliação for criada com sucesso.
- **422 Unprocessable Entity** se houver erro de validação.
- **400 Bad Request** se houver erro de validação.

---

### 3. Buscar uma Avaliação de Agendamento por ID

**GET** `/api/v1/agendamento-avaliacoes/{id}`

Obtém os detalhes de uma avaliação de agendamento específica pelo seu ID.

#### Resposta Exemplo:

```json
{
  "id": 0,
  "rating": 0,
  "comment": "string",
  "appointmentId": 0,
  "userId": 0,
  "createdAt": "2025-03-03T18:09:14.162Z",
  "updatedAt": "2025-03-03T18:09:14.162Z",
  "appointment": {...},
  "user": {...}
}
```

#### Resposta Esperada:
- **200 OK** se a avaliação for encontrada.
- **404 Not Found** se a avaliação não existir.

---

### 4. Atualizar uma Avaliação de Agendamento

**PUT** `/api/v1/agendamento-avaliacoes/{id}`

Atualiza as informações de uma avaliação de agendamento existente.

#### Corpo da Requisição:

```json
{
  "rating": 5,
  "comment": "string",
  "appointmentId": 2147483647,
  "userId": 2147483647
}
```

#### Resposta Esperada:
- **200 OK** se a atualização for bem-sucedida.
- **422 Unprocessable Entity** se houver erro de validação.
- **400 Bad Request** se houver erro de validação.
- **404 Not Found** se a avaliação não existir.

---

### 5. Excluir uma Avaliação de Agendamento

**DELETE** `/api/v1/agendamento-avaliacoes/{id}`

Remove uma avaliação de agendamento do sistema.

#### Resposta Esperada:
- **200 OK** se a avaliação for removida com sucesso.
- **404 Not Found** se a avaliação não existir.

---

Este módulo gerencia a entidade `AppointmentRating`, seguindo a mesma estrutura dos módulos anteriores. As operações básicas de CRUD (Create, Read, Update, Delete) estão disponíveis, permitindo o gerenciamento completo das avaliações de agendamentos na aplicação.