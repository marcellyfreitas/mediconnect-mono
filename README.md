# Projeto de Conexão Mobile com API  

Este repositório contém dois projetos integrados para oferecer uma solução completa de conexão entre um aplicativo mobile e uma API REST:  

- **mobileapp**: Aplicativo mobile desenvolvido para conectar pacientes e médicos, permitindo agendamento de consultas, acompanhamento de exames, entre outros serviços.  
- **webapi**: API desenvolvida com ASP.NET para gerenciar o backend do aplicativo, incluindo autenticação, gerenciamento de usuários e CRUDs necessários para o funcionamento da aplicação.  

---

O aplicativo mobile consome dados de uma API RESTful, fornecida pelo projeto webapi, para garantir uma experiência rápida e segura para os usuários.  

---
 
## Estrutura do Repositório  

    .
    ├── mobileapp/     # Aplicativo mobile
    └── webapi/        # API ASP.NET

---

## Como Executar  

### Pré-requisitos  

- Node.js e npm/yarn instalados para o projeto mobile.  
- .NET 8 SDK instalado para o projeto webapi.  
- Docker (opcional) para rodar a API em container.

---

### Executando o Aplicativo Mobile  

1. Navegue até a pasta do aplicativo:  
    ```bash
    cd mobileapp
    ```

2. Instale as dependências:  
    ```bash
    npm install
    ```

3. Execute o aplicativo:  
    ```bash
    npm run start
    ```
    ou para abrir direto no navegador
    ```bash
    npm run web
    ```

---

### Executando a API ASP.NET  

1. Navegue até a pasta do projeto webapi:  
    ```bash
    cd webapi
    ```

2. Restaure as dependências:  
    ```bash
    dotnet restore
    ```

3. Compile o projeto:  
    ```bash
    dotnet clean
    dotnet build
    ```

4. Execute as migrações para preparar o banco de dados (opcional):  
    ```bash
    dotnet ef database update
    ```

5. Inicie a aplicação:  
    ```bash
    dotnet watch
    ```

6. A API estará disponível em: [http://localhost:5000](http://localhost:5000)  

---

## Tecnologias Utilizadas  

- **mobileapp**: Desenvolvido com React Native e Expo, Typescript, Nativewind.  
- **webapi**: Desenvolvido com ASP.NET Core e Entity Framework Core.  

---
