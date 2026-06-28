
# Módulo de Autenticação de Usuários

Este módulo gerencia a autenticação de usuários na aplicação, permitindo operações de login, refresh token, logout e obtenção de dados do usuário.

## Endpoints

### 1. Login

**POST** `/api/v1/auth/user/login`

Realiza o login de um usuário.

#### Corpo da Requisição:

```json
{
  "email": "usuario@usuario.com",
  "password": "Senh@123"
}
```

#### Resposta Exemplo:

```json
{
  "success": true,
  "message": "Login efetuado com sucesso!",
  "data": {
    "AccessToken": "string",
    "RefreshToken": "string"
  }
}
```

#### Resposta Esperada:
- **200 OK** se o login for bem-sucedido.
- **400 Bad Request** se o corpo da requisição for inválido.
- **401 Unauthorized** se as credenciais forem inválidas.
- **500 Internal Server Error** em caso de erro no servidor.

---

### 2. Refresh Token

**POST** `/api/v1/auth/user/refresh-token`

Gera um novo token de acesso a partir de um refresh token.

#### Corpo da Requisição:

```json
{
  "RefreshToken": "string"
}
```

#### Resposta Exemplo:

```json
{
  "success": true,
  "message": "Login efetuado com sucesso!",
  "data": {
    "AccessToken": "string",
    "RefreshToken": "string"
  }
}
```

#### Resposta Esperada:
- **200 OK** se o refresh token for válido.
- **400 Bad Request** se o refresh token não for fornecido.
- **401 Unauthorized** se o refresh token for inválido ou expirado.
- **404 Not Found** se o usuário associado ao token não for encontrado.
- **500 Internal Server Error** em caso de erro no servidor.

---

### 3. Logout

**POST** `/api/v1/auth/user/logout`

Realiza o logout do usuário.

#### Resposta Exemplo:

```json
{
  "Message": "Logout realizado com sucesso"
}
```

#### Resposta Esperada:
- **200 OK** se o logout for bem-sucedido.

---

### 4. Obter Dados do Usuário

**GET** `/api/v1/auth/user/usuario`

Retorna os dados do usuário autenticado.

#### Resposta Exemplo:

```json
{
  "success": true,
  "message": "Dados retornados com sucesso",
  "data": {
    "id": 0,
    "name": "string",
    "email": "string",
    "cpf": "string",
    "createdAt": "2025-03-03T17:12:32.881Z",
    "updatedAt": "2025-03-03T17:12:32.881Z"
  }
}
```

#### Resposta Esperada:
- **200 OK** se os dados forem retornados com sucesso.
- **401 Unauthorized** se o token for inválido.
- **404 Not Found** se o usuário não for encontrado.
- **500 Internal Server Error** em caso de erro no servidor.

---

### 5. Registrar Novo Usuário

**POST** `/api/v1/auth/user/register`

Cria um novo usuário.

#### Corpo da Requisição:

```json
{
  "name": "string",
  "email": "user@example.com",
  "password": "string"
}
```

#### Resposta Exemplo:

```json
{
  "success": true,
  "message": "Dados retornados com sucesso",
  "data": {
    "id": 0,
    "name": "string",
    "email": "string",
    "cpf": "string",
    "createdAt": "2025-03-03T17:12:32.881Z",
    "updatedAt": "2025-03-03T17:12:32.881Z"
  }
}
```

#### Resposta Esperada:
- **201 Created** se o usuário for criado com sucesso.
- **422 Unprocessable Entity** se o formulário for inválido ou o e-mail já estiver em uso.
- **500 Internal Server Error** em caso de erro no servidor.

---

### Resumo das Respostas de Erro Comuns:

- **400 Bad Request**: Requisição inválida (corpo ou parâmetros incorretos).
- **401 Unauthorized**: Token inválido ou ausente.
- **404 Not Found**: Recurso não encontrado (usuário, avaliação, etc.).
- **422 Unprocessable Entity**: Erro de validação (dados inválidos ou conflitos).
- **500 Internal Server Error**: Erro interno no servidor.