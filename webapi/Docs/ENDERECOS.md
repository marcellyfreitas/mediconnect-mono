# Módulo de Endereços

Este módulo gerencia os endereços da aplicação, permitindo operações de criação, leitura, atualização e exclusão.

## Endpoints

### 1. Listar Endereços

**GET** `/api/v1/enderecos`

Retorna a lista de endereços cadastrados na aplicação.

#### Resposta Exemplo:

```json
[
  {
    "id": 0,
    "logradouro": "string",
    "cep": "string",
    "bairro": "string",
    "cidade": "string",
    "estado": "string",
    "pais": "string",
    "numero": "string",
    "complemento": "string",
    "createdAt": "2025-03-03T17:59:28.290Z",
    "updatedAt": "2025-03-03T17:59:28.290Z"
  },
  { ... },
  { ... }
]
```

---

### 2. Criar um Endereço

**POST** `/api/v1/enderecos`

Cria um novo endereço com as informações fornecidas.

#### Corpo da Requisição:

```json
{
  "logradouro": "string",
  "cep": "69295-431",
  "bairro": "string",
  "cidade": "string",
  "estado": "string",
  "pais": "string",
  "numero": "string",
  "complemento": "string"
}
```

#### Resposta Esperada:
- **201 Created** se o endereço for criado com sucesso.
- **422 Unprocessable Entity** se houver erro de validação.
- **400 Bad Request** se houver erro de validação.

---

### 3. Buscar um Endereço por ID

**GET** `/api/v1/enderecos/{id}`

Obtém os detalhes de um endereço específico pelo seu ID.

#### Resposta Exemplo:

```json
{
  "id": 0,
  "logradouro": "string",
  "cep": "string",
  "bairro": "string",
  "cidade": "string",
  "estado": "string",
  "pais": "string",
  "numero": "string",
  "complemento": "string",
  "createdAt": "2025-03-03T17:59:28.290Z",
  "updatedAt": "2025-03-03T17:59:28.290Z"
}
```

#### Resposta Esperada:
- **200 OK** se o endereço for encontrado.
- **404 Not Found** se o endereço não existir.

---

### 4. Atualizar um Endereço

**PUT** `/api/v1/enderecos/{id}`

Atualiza as informações de um endereço existente.

#### Corpo da Requisição:

```json
{
  "logradouro": "string",
  "cep": "69295-431",
  "bairro": "string",
  "cidade": "string",
  "estado": "string",
  "pais": "string",
  "numero": "string",
  "complemento": "string"
}
```

#### Resposta Esperada:
- **200 OK** se a atualização for bem-sucedida.
- **422 Unprocessable Entity** se houver erro de validação.
- **400 Bad Request** se houver erro de validação.
- **404 Not Found** se o endereço não existir.

---

### 5. Excluir um Endereço

**DELETE** `/api/v1/enderecos/{id}`

Remove um endereço do sistema.

#### Resposta Esperada:
- **200 OK** se o endereço for removido com sucesso.
- **404 Not Found** se o endereço não existir.

---

Este módulo gerencia a entidade `Address`, seguindo a mesma estrutura dos módulos anteriores. As operações básicas de CRUD (Create, Read, Update, Delete) estão disponíveis, permitindo o gerenciamento completo dos endereços na aplicação.