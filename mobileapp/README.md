# Bem-vindo ao Seu Aplicativo Expo 🚀

Este é um projeto [Expo](https://expo.dev) criado com [`create-expo-app`](https://www.npmjs.com/package/create-expo-app).

## 📦 Primeiros Passos

### 1. Instalar Dependências
Certifique-se de ter o Node.js instalado e execute:

```bash
npm install
```
## 2. Criar arquivo de variaveis
No linux
```bash
cp .env.example .env
```
No windows
```bash
copy .env.example .env
```

### 3. Iniciar o Aplicativo
É possível utilizar,
- Emulador android - `a 
- Emulador IOS
- Preview na Web
- Abrir no dispositivo com o Expo Go
Execute o seguinte comando para iniciar o projeto:

```bash
npx expo start
```

Isso iniciará o servidor de desenvolvimento do Expo, permitindo abrir o app em:

- [Build de Desenvolvimento](https://docs.expo.dev/develop/development-builds/introduction/)
- [Emulador Android](https://docs.expo.dev/workflow/android-studio-emulator/)
- [Simulador iOS](https://docs.expo.dev/workflow/ios-simulator/)
- [Expo Go](https://expo.dev/go) (um sandbox para testar aplicativos Expo)

### 📱 Opções de Execução no Terminal
Após rodar `npx expo start`, você pode interagir com o terminal para acessar diferentes modos:

- **Abrir no Expo Go** → Escaneie o QR Code com o Expo Go (Android) ou a câmera do iOS.
- **Abrir em um dispositivo/emulador**:
  - Pressione **`a`** → Abrir no Emulador Android.
  - Pressione **`w`** → Abrir no navegador Web.
- **Trocar para Build de Desenvolvimento**:
  - Pressione **`s`** → Alternar para um build de desenvolvimento.
- **Depuração e ferramentas**:
  - Pressione **`j`** → Abrir o debugger.
  - Pressione **`r`** → Recarregar o app.
  - Pressione **`m`** → Alternar o menu do Expo.
  - Pressione **`Shift + M`** → Acessar mais ferramentas.
- **Abrir código no editor padrão**:
  - Pressione **`o`** → Abrir o projeto no editor configurado.
- **Mostrar todos os comandos disponíveis**:
  - Pressione **`?`** → Exibir todas as opções interativas.

## 🛠 Estrutura do Projeto
O projeto segue um sistema de roteamento baseado em arquivos dentro do diretório **app**. Você pode começar a desenvolver modificando estes arquivos:

```
.
├── app/
├── assets/
├── omponents/
├── contexts/
├── hooks/
├── services/
├── utils/
├── package.json
└── app.json
```

## 📖 Saiba Mais
Explore estes recursos para aprofundar seu conhecimento:

- [📘 Documentação do Expo](https://docs.expo.dev/): Fundamentos e guias avançados.
- [📚 Tutoriais do Expo](https://docs.expo.dev/tutorial/introduction/): Aprendizado prático com instruções passo a passo.

## 🤝 Junte-se à Comunidade
Conecte-se com outros desenvolvedores que criam aplicativos universais com Expo:

- [💻 Expo no GitHub](https://github.com/expo/expo): Contribuições para o código aberto.
- [💬 Comunidade no Discord](https://chat.expo.dev): Tire dúvidas e compartilhe experiências.

Feliz codificação! 🎉