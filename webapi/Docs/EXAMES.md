# Módulo de Exames

Este módulo gerencia os exames da aplicação, permitindo operações de criação, leitura, atualização e exclusão.

## Endpoints

### 1. Listar Exames

**GET** `/api/v1/exames`

Retorna a lista de exames cadastrados na aplicação.

#### Resposta Exemplo:

```json
[
  {
    "id": 0,
    "name": "string",
    "description": "string",
    "active": 0,
    "createdAt": "2025-03-12T12:01:21.402Z",
    "updatedAt": "2025-03-12T12:01:21.402Z"
  },
  { ... },
  { ... }
]
```

---

### 2. Criar um Exame

**POST** `/api/v1/exames`

Cria um novo exame com as informações fornecidas.

#### Corpo da Requisição:

```json
{
  "name": "string",
  "description": "string"
}
```

#### Resposta Esperada:
- **201 Created** se o exame for criado com sucesso.
- **422 Unprocessable Entity** se houver erro de validação.
- **400 Bad Request** se houver erro de validação.

---

### 3. Buscar um Exame por ID

**GET** `/api/v1/exames/{id}`

Obtém os detalhes de um exame específico pelo seu ID.

#### Resposta Exemplo:

```json
{
  "id": 0,
  "name": "string",
  "description": "string",
  "active": 0,
  "createdAt": "2025-03-12T12:01:21.402Z",
  "updatedAt": "2025-03-12T12:01:21.402Z"
}
```

#### Resposta Esperada:
- **200 OK** se o exame for encontrado.
- **404 Not Found** se o exame não existir.

---

### 4. Atualizar um Exame

**PUT** `/api/v1/exames/{id}`

Atualiza as informações de um exame existente.

#### Corpo da Requisição:

```json
{
  "name": "string",
  "description": "string"
}
```

#### Resposta Esperada:
- **200 OK** se a atualização for bem-sucedida.
- **422 Unprocessable Entity** se houver erro de validação.
- **400 Bad Request** se houver erro de validação.
- **404 Not Found** se o exame não existir.

---

### 5. Excluir um Exame

**DELETE** `/api/v1/exames/{id}`

Remove um exame do sistema.

#### Resposta Esperada:
- **200 OK** se o exame for removido com sucesso.
- **404 Not Found** se o exame não existir.

---
