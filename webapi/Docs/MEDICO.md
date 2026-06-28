# Módulo de Médicos

Este módulo gerencia os médicos da aplicação, permitindo operações de criação, leitura, atualização e exclusão.

## Endpoints

### 1. Listar Médicos

**GET** `/api/v1/medicos`

Retorna a lista de entidades cadastradas na aplicação.

#### Resposta Exemplo:

```json
[
  {
    "id": 0,
    "name": "string",
    "cpf": "string",
    "email": "string",
    "crm": "string",
    "Specialization": {...},
    "createdAt": "2025-03-03T17:50:12.726Z",
    "updatedAt": "2025-03-03T17:50:12.726Z"
  },
  { ... },
  { ... }
]
```

---

### 2. Criar uma Entidade

**POST** `/api/v1/medicos`

Cria uma nova entidade com as informações fornecidas.

#### Corpo da Requisição:

```json
{
  "name": "string",
  "cpf": "575.626.139-10",
  "email": "user@example.com",
  "crm": "288350IQ",
  "especializationId": 2147483647
}
```

#### Resposta Esperada:
- **201 Created** se a entidade for criada com sucesso.
- **422 Unprocessable Entity** se houver erro de validação.
- **400 Bad Request** se houver erro de validação.

---

### 3. Buscar uma Entidade por ID

**GET** `/api/v1/medicos/{id}`

Obtém os detalhes de uma entidade específica pelo seu ID.

#### Resposta Exemplo:

```json
{
  "id": 0,
  "name": "string",
  "cpf": "string",
  "email": "string",
  "crm": "string",
  "Specialization": {...},
  "createdAt": "2025-03-03T17:50:12.726Z",
  "updatedAt": "2025-03-03T17:50:12.726Z"
}
```

#### Resposta Esperada:
- **200 OK** se a entidade for encontrada.
- **404 Not Found** se a entidade não existir.

---

### 4. Atualizar uma Entidade

**PUT** `/api/v1/medicos/{id}`

Atualiza as informações de uma entidade existente.

#### Corpo da Requisição:

```json
{
  "name": "string",
  "cpf": "575.626.139-10",
  "email": "user@example.com",
  "crm": "288350IQ",
  "especializationId": 2147483647
}
```

#### Resposta Esperada:
- **200 OK** se a atualização for bem-sucedida.
- **422 Unprocessable Entity** se houver erro de validação.
- **400 Bad Request** se houver erro de validação.
- **404 Not Found** se a entidade não existir.

---

### 5. Excluir uma Entidade

**DELETE** `/api/v1/medicos/{id}`

Remove uma entidade do sistema.

#### Resposta Esperada:
- **200 OK** se a entidade for removida com sucesso.
- **404 Not Found** se a entidade não existir.

