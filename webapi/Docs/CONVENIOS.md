# Módulo de Convênios

Este módulo gerencia os convênios médicas da aplicação, permitindo operações de criação, leitura, atualização e exclusão de unidades médicas.

## Endpoints

### 1. Listar Convênios

**GET** `/api/v1/convenios`

Retorna a lista de Unidades cadastrados na aplicação.

#### Resposta Exemplo:

```json
[
  {
    "id": 0,
    "name": "string",
    "provider": "string",
    "startDate": "2025-03-03T17:46:15.276Z",
    "endDate": "2025-03-03T17:46:15.276Z",
    "healthPlanId": 0,
    "healthPlan": {...},
    "createdAt": "2025-03-03T17:46:15.276Z",
    "updatedAt": "2025-03-03T17:46:15.276Z"
  },
  { ... },
  { ... },
]
```

---

### 2. Criar um Convênio

**POST** `/api/v1/convenios`

Cria um novo convênio com as informações fornecidas.

#### Corpo da Requisição:

```json
{
  "name": "string",
  "provider": "string",
  "healthPlanId": 0,
  "startDate": "2025-03-03T17:47:05.199Z",
  "endDate": "2025-03-03T17:47:05.199Z"
}
```

#### Resposta Esperada:
- **201 Created** se o modelo for criado com sucesso.
- **422 Unprocessable Entity** se houver erro de validação.
- **400 Bad Request** se houver erro de validação.

---

### 3. Buscar um Convênio por ID

**GET** `/api/v1/convenios/{id}`

Obtém os detalhes de um usuário específico pelo seu ID.

#### Resposta Exemplo:

```json
{
    "id": 0,
    "name": "string",
    "provider": "string",
    "startDate": "2025-03-03T17:46:15.276Z",
    "endDate": "2025-03-03T17:46:15.276Z",
    "healthPlanId": 0,
    "healthPlan": {...},
    "createdAt": "2025-03-03T17:46:15.276Z",
    "updatedAt": "2025-03-03T17:46:15.276Z"
  }
```

#### Resposta Esperada:
- **200 OK** se o modelo for encontrado.
- **404 Not Found** se o modelo não existir.

---

### 4. Atualizar um Convênio

**PUT** `/api/v1/convenios/{id}`

Atualiza as informações de um Convênio existente.

#### Corpo da Requisição:

```json
{
  "name": "string",
  "provider": "string",
  "healthPlanId": 0,
  "startDate": "2025-03-03T17:47:05.199Z",
  "endDate": "2025-03-03T17:47:05.199Z"
}
```

#### Resposta Esperada:
- **200 OK** se a atualização for bem-sucedida.
- **422 Unprocessable Entity** se houver erro de validação.
- **400 Bad Request** se houver erro de validação.
- **404 Not Found** se o modelo não existir.

---

### 5. Excluir um Convênio

**DELETE** `/api/v1/convenios/{id}`

Remove um Convênio do sistema.

#### Resposta Esperada:
- **200 OK** se o modelo for removido com sucesso.
- **404 Not Found** se o modelo não existir.

