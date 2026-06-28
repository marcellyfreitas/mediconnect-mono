# Módulo de Unidades

Este módulo gerencia as unidades médicas da aplicação, como clínicas, centros médicos, etc..., permitindo operações de criação, leitura, atualização e exclusão de unidades médicas.

## Endpoints

### 1. Listar Unidades

**GET** `/api/v1/unidades`

Retorna a lista de Unidades cadastrados na aplicação.

#### Resposta Exemplo:

```json
[
  {
    "id": 0,
    "name": "string",
    "phoneNumber": "string",
    "email": "string",
    "createdAt": "2025-03-03T17:20:05.515Z",
    "updatedAt": "2025-03-03T17:20:05.515Z",
    "address": {...}
  },
  { ... },
  { ... },
]
```

---

### 2. Criar uma Unidade

**POST** `/api/v1/unidades`

Cria uma nova unidade com as informações fornecidas.

#### Corpo da Requisição:

```json
{
  "name": "string",
  "phoneNumber": "string",
  "email": "unidade@example.com",
  "addressId": 2147483647
}
```

#### Resposta Esperada:
- **201 Created** se o modelo for criado com sucesso.
- **422 Unprocessable Entity** se houver erro de validação.
- **400 Bad Request** se houver erro de validação.

---

### 3. Buscar uma Unidade por ID

**GET** `/api/v1/unidades/{id}`

Obtém os detalhes de um usuário específico pelo seu ID.

#### Resposta Exemplo:

```json
{
    "id": 0,
    "name": "string",
    "phoneNumber": "string",
    "email": "string",
    "createdAt": "2025-03-03T17:20:05.515Z",
    "updatedAt": "2025-03-03T17:20:05.515Z",
    "address": {...}
}
```

#### Resposta Esperada:
- **200 OK** se o modelo for encontrado.
- **404 Not Found** se o modelo não existir.

---

### 4. Atualizar uma Unidade

**PUT** `/api/v1/unidades/{id}`

Atualiza as informações de uma unidade existente.

#### Corpo da Requisição:

```json
{
  "name": "string",
  "phoneNumber": "string",
  "email": "user@example.com",
  "addressId": 2147483647
}
```

#### Resposta Esperada:
- **200 OK** se a atualização for bem-sucedida.
- **422 Unprocessable Entity** se houver erro de validação.
- **400 Bad Request** se houver erro de validação.
- **404 Not Found** se o modelo não existir.

---

### 5. Excluir uma Unidade

**DELETE** `/api/v1/unidades/{id}`

Remove uma unidade do sistema.

#### Resposta Esperada:
- **200 OK** se o modelo for removido com sucesso.
- **404 Not Found** se o modelo não existir.

