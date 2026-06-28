# Módulo de Autenticação de Administradores

Este módulo gerencia a autenticação de administradores na aplicação, permitindo operações de login, refresh token, logout e obtenção de dados do usuário.

## Endpoints

### 1. Login

**POST** `/api/v1/auth/admin/login`

Realiza o login de um administrador.

#### Corpo da Requisição:

```json
{
  "email": "administrador@administrador.com",
  "password": "Senh@123"
}
```

#### Resposta Exemplo:

```json
{
  "success": true,
  "message": "Login efetuado com sucesso!",
  "data": {
    "accessToken": "string",
    "refreshToken": "string"
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

**POST** `/api/v1/auth/admin/refresh-token`

Gera um novo token de acesso a partir de um refresh token.

#### Corpo da Requisição:

```json
{
  "refreshToken": "string"
}
```

#### Resposta Exemplo:

```json
{
  "success": true,
  "message": "Login efetuado com sucesso!",
  "data": {
    "accessToken": "string",
    "refreshToken": "string"
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

**POST** `/api/v1/auth/admin/logout`

Realiza o logout do administrador.

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

**GET** `/api/v1/auth/admin/usuario`

Retorna os dados do administrador autenticado.

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

### 5. Registrar Novo Administrador

**POST** `/api/v1/auth/admin/register`

Cria um novo administrador.

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
- **201 Created** se o administrador for criado com sucesso.
- **422 Unprocessable Entity** se o formulário for inválido ou o e-mail já estiver em uso.
- **500 Internal Server Error** em caso de erro no servidor.

---

### Resumo das Respostas de Erro Comuns:

- **400 Bad Request**: Requisição inválida (corpo ou parâmetros incorretos).
- **401 Unauthorized**: Token inválido ou ausente.
- **404 Not Found**: Recurso não encontrado (usuário, avaliação, etc.).
- **422 Unprocessable Entity**: Erro de validação (dados inválidos ou conflitos).
- **500 Internal Server Error**: Erro interno no servidor.

---

Esse mapa de rotas segue o mesmo formato que você utilizou anteriormente, com os DTOs e os retornos detalhados. Se precisar de mais ajustes ou de algo adicional, é só avisar! 😊