# Módulo de Usuários

Este módulo gerencia os usuários da aplicação, permitindo operações de criação, leitura, atualização e exclusão de usuários.

## Endpoints

### 1. Listar Usuários

**GET** `/api/v1/usuarios`

Retorna a lista de usuários cadastrados na aplicação.

#### Resposta Exemplo:

```json
[
  {
    "id": 0,
    "name": "string",
    "email": "string",
    "cpf": "string",
    "address": {...},
    "createdAt": "2025-03-03T17:12:32.881Z",
    "updatedAt": "2025-03-03T17:12:32.881Z"
  }
]
```

---

### 2. Criar um Usuário

**POST** `/api/v1/usuarios`

Cria um novo usuário com as informações fornecidas.

#### Corpo da Requisição:

```json
{
  "name": "string",
  "email": "user@example.com",
  "password": "string",
  "cpf": "145.652.999-19",
  "addressId": 0
}
```

#### Resposta Esperada:
- **201 Created** se o usuário for criado com sucesso.
- **422 Unprocessable Entity** se houver erro de validação.
- **400 Bad Request** se houver erro de validação.

---

### 3. Buscar um Usuário por ID

**GET** `/api/v1/usuarios/{id}`

Obtém os detalhes de um usuário específico pelo seu ID.

#### Resposta Exemplo:

```json
{
  "id": 0,
  "name": "string",
  "email": "string",
  "cpf": "string",
  "address": {...},
  "createdAt": "2025-03-03T17:12:32.881Z",
  "updatedAt": "2025-03-03T17:12:32.881Z"
}
```

#### Resposta Esperada:
- **200 OK** se o usuário for encontrado.
- **404 Not Found** se o usuário não existir.

---

### 4. Atualizar um Usuário

**PUT** `/api/v1/usuarios/{id}`

Atualiza as informações de um usuário existente.

#### Corpo da Requisição:

```json
{
  "name": "string",
  "email": "user@example.com",
  "password": "string",
  "cpf": "145.652.999-19",
  "addressId": 0
}
```

#### Resposta Esperada:
- **200 OK** se a atualização for bem-sucedida.
- **422 Unprocessable Entity** se houver erro de validação.
- **400 Bad Request** se houver erro de validação.
- **404 Not Found** se o usuário não existir.

---

### 5. Excluir um Usuário

**DELETE** `/api/v1/usuarios/{id}`

Remove um usuário do sistema.

#### Resposta Esperada:
- **200 OK** se o usuário for removido com sucesso.
- **404 Not Found** se o usuário não existir.

