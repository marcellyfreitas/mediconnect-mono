# Módulo de Especializações

Este módulo gerencia as especializações da aplicação, permitindo operações de criação, leitura, atualização e exclusão.

## Endpoints

### 1. Listar Especializações

**GET** `/api/v1/especializacoes`

Retorna a lista de especializações cadastradas na aplicação.

#### Resposta Exemplo:

```json
[
  {
    "id": 0,
    "name": "string",
    "description": "string",
    "createdAt": "2025-03-03T17:56:35.577Z",
    "updatedAt": "2025-03-03T17:56:35.577Z"
  },
  { ... },
  { ... }
]
```

---

### 2. Criar uma Especialização

**POST** `/api/v1/especializacoes`

Cria uma nova especialização com as informações fornecidas.

#### Corpo da Requisição:

```json
{
  "name": "string",
  "description": "string"
}
```

#### Resposta Esperada:
- **201 Created** se a especialização for criada com sucesso.
- **422 Unprocessable Entity** se houver erro de validação.
- **400 Bad Request** se houver erro de validação.

---

### 3. Buscar uma Especialização por ID

**GET** `/api/v1/especializacoes/{id}`

Obtém os detalhes de uma especialização específica pelo seu ID.

#### Resposta Exemplo:

```json
{
  "id": 0,
  "name": "string",
  "description": "string",
  "createdAt": "2025-03-03T17:56:35.577Z",
  "updatedAt": "2025-03-03T17:56:35.577Z"
}
```

#### Resposta Esperada:
- **200 OK** se a especialização for encontrada.
- **404 Not Found** se a especialização não existir.

---

### 4. Atualizar uma Especialização

**PUT** `/api/v1/especializacoes/{id}`

Atualiza as informações de uma especialização existente.

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
- **404 Not Found** se a especialização não existir.

---

### 5. Excluir uma Especialização

**DELETE** `/api/v1/especializacoes/{id}`

Remove uma especialização do sistema.

#### Resposta Esperada:
- **200 OK** se a especialização for removida com sucesso.
- **404 Not Found** se a especialização não existir.

---

Este módulo segue a mesma estrutura do módulo de médicos, mas é adaptado para gerenciar a entidade `Specialization`. As operações básicas de CRUD (Create, Read, Update, Delete) estão disponíveis, permitindo o gerenciamento completo das especializações na aplicação.