# Projeto de Conexão Mobile com API  

Este repositório contém dois projetos integrados para oferecer uma solução completa de conexão entre um aplicativo mobile e uma API REST:  

- **mobileapp**: Aplicativo mobile desenvolvido para conectar pacientes e médicos, permitindo agendamento de consultas, acompanhamento de exames, entre outros serviços.  
- **webapi**: API desenvolvida com ASP.NET para gerenciar o backend do aplicativo, incluindo autenticação, gerenciamento de usuários e CRUDs necessários para o funcionamento da aplicação.  

---

O aplicativo mobile consome dados de uma API RESTful, fornecida pelo projeto webapi, para garantir uma experiência rápida e segura para os usuários.  

---

## 📷 Prints

<p align="center">
  <img src="https://github.com/user-attachments/assets/452861e6-43d6-45be-be6a-e12987c2b7e9" width="200" />
  <img src="https://github.com/user-attachments/assets/38f9f5ec-03a0-45dc-ba91-90f1127bbb4a" width="200" />
  <img src="https://github.com/user-attachments/assets/4b84d8ee-d5a0-4177-a890-1d08a65f0c85" width="200" />
</p>

<p align="center">
  <img src="https://github.com/user-attachments/assets/899e4ff6-547f-4eb1-b4d6-fcbd6253c1ff" width="200" />
  <img src="https://github.com/user-attachments/assets/0d28aaf5-96eb-40c0-9a73-f51df7fdfa1b" width="200" />
  <img src="https://github.com/user-attachments/assets/bf112971-ee66-445c-bcce-eaf6a4732e8e" width="200" />
</p>

<p align="center">
  <img src="https://github.com/user-attachments/assets/f04adbf8-ec1c-4cb3-a89b-215e66399a6a" width="200" />
  <img src="https://github.com/user-attachments/assets/2a7d8b81-4602-4eac-b2df-035f6b869d78" width="200" />
  <img src="https://github.com/user-attachments/assets/8f3a78d6-ff35-40b5-9b83-0fce826f2bee" width="200" />
</p>

---


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
