# Módulo de Administradores

Este módulo gerencia os administradores da aplicação, permitindo operações de criação, leitura, atualização e exclusão.

## Endpoints

### 1. Listar Administradores

**GET** `/api/v1/administradores`

Retorna a lista de administradores cadastrados na aplicação.

#### Resposta Exemplo:

```json
[
  {
    "id": 0,
    "name": "string",
    "email": "string",
    "createdAt": "2025-03-08T12:22:02.931Z",
    "updatedAt": "2025-03-08T12:22:02.931Z"
  },
  { ... },
  { ... }
]
```

---

### 2. Criar um Administrador

**POST** `/api/v1/administradores`

Cria um novo administrador com as informações fornecidas.

#### Corpo da Requisição:

```json
{
  "name": "string",
  "email": "user@example.com",
  "password": "string"
}
```

#### Resposta Esperada:
- **201 Created** se o administrador for criado com sucesso.
- **422 Unprocessable Entity** se houver erro de validação.
- **400 Bad Request** se houver erro de validação.

---

### 3. Buscar um Administrador por ID

**GET** `/api/v1/administradores/{id}`

Obtém os detalhes de um administrador específico pelo seu ID.

#### Resposta Exemplo:

```json
{
  "id": 0,
  "name": "string",
  "email": "string",
  "createdAt": "2025-03-08T12:22:02.931Z",
  "updatedAt": "2025-03-08T12:22:02.931Z"
}
```

#### Resposta Esperada:
- **200 OK** se o administrador for encontrado.
- **404 Not Found** se o administrador não existir.

---

### 4. Atualizar um Administrador

**PUT** `/api/v1/administradores/{id}`

Atualiza as informações de um administrador existente.

#### Corpo da Requisição:

```json
{
  "name": "string",
  "email": "user@example.com",
  "password": "string"
}
```

#### Resposta Esperada:
- **200 OK** se a atualização for bem-sucedida.
- **422 Unprocessable Entity** se houver erro de validação.
- **400 Bad Request** se houver erro de validação.
- **404 Not Found** se o administrador não existir.

---

### 5. Excluir um Administrador

**DELETE** `/api/v1/administradores/{id}`

Remove um administrador do sistema.

#### Resposta Esperada:
- **200 OK** se o administrador for removido com sucesso.
- **404 Not Found** se o administrador não existir.

---

Este módulo gerencia a entidade `Administrator`, seguindo a mesma estrutura dos módulos anteriores. As operações básicas de CRUD (Create, Read, Update, Delete) estão disponíveis, permitindo o gerenciamento completo dos administradores na aplicação.