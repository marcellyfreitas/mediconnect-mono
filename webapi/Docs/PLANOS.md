# Módulo de Planos de Saúde

Este módulo gerencia os planos de saúde da aplicação, permitindo operações de criação, leitura, atualização e exclusão.

## Endpoints

### 1. Listar Planos de Saúde

**GET** `/api/v1/planos`

Retorna a lista de planos de saúde cadastrados na aplicação.

#### Resposta Exemplo:

```json
[
  {
    "id": 0,
    "name": "string",
    "coverage": "string",
    "medicalAgreements": [...],
    "createdAt": "2025-03-03T18:01:46.635Z",
    "updatedAt": "2025-03-03T18:01:46.635Z"
  },
  { ... },
  { ... }
]
```

---

### 2. Criar um Plano de Saúde

**POST** `/api/v1/planos`

Cria um novo plano de saúde com as informações fornecidas.

#### Corpo da Requisição:

```json
{
  "name": "string",
  "coverage": "string"
}
```

#### Resposta Esperada:
- **201 Created** se o plano de saúde for criado com sucesso.
- **422 Unprocessable Entity** se houver erro de validação.
- **400 Bad Request** se houver erro de validação.

---

### 3. Buscar um Plano de Saúde por ID

**GET** `/api/v1/planos/{id}`

Obtém os detalhes de um plano de saúde específico pelo seu ID.

#### Resposta Exemplo:

```json
{
  "id": 0,
  "name": "string",
  "coverage": "string",
  "medicalAgreements": [...],
  "createdAt": "2025-03-03T18:01:46.635Z",
  "updatedAt": "2025-03-03T18:01:46.635Z"
}
```

#### Resposta Esperada:
- **200 OK** se o plano de saúde for encontrado.
- **404 Not Found** se o plano de saúde não existir.

---

### 4. Atualizar um Plano de Saúde

**PUT** `/api/v1/planos/{id}`

Atualiza as informações de um plano de saúde existente.

#### Corpo da Requisição:

```json
{
  "name": "string",
  "coverage": "string"
}
```

#### Resposta Esperada:
- **200 OK** se a atualização for bem-sucedida.
- **422 Unprocessable Entity** se houver erro de validação.
- **400 Bad Request** se houver erro de validação.
- **404 Not Found** se o plano de saúde não existir.

---

### 5. Excluir um Plano de Saúde

**DELETE** `/api/v1/planos/{id}`

Remove um plano de saúde do sistema.

#### Resposta Esperada:
- **200 OK** se o plano de saúde for removido com sucesso.
- **404 Not Found** se o plano de saúde não existir.

---

Este módulo gerencia a entidade `HealthPlan`, seguindo a mesma estrutura dos módulos anteriores. As operações básicas de CRUD (Create, Read, Update, Delete) estão disponíveis, permitindo o gerenciamento completo dos planos de saúde na aplicação.