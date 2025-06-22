# FIREBOUND DOCS DEVELOPMENT

## Sumário

- [FIREBOUND DOCS DEVELOPMENT](#firebound-docs-development)
  - [Sumário](#sumário)
    - [Tecnologias da Documentação](#tecnologias-da-documentação)
    - [Estrutura da documentação](#estrutura-da-documentação)
  - [Documentação](#documentação)
    - [Gerar a Documentação da API](#gerar-a-documentação-da-api)
    - [Executar Scripts NPM](#executar-scripts-npm)

### Tecnologias da Documentação

- **DocFX**: Utilizado para gerar as referências de API do projeto .NET em Markdown (md).
- **Docusaurus**: Utilizado para construir o site estático a partir de arquivos Markdown, incluindo a API.

### Estrutura da documentação

```powershell
docs/
├── api/                    # Arquivos gerados pelo DocFX a partir do projeto C#
├── content/                # Conteúdo da documentação em Markdown
│ ├── api/                  # Arquivos do DocFX processados para funcionar no DocFX
│ ├── architecture/         # Arquivos relacionados à arquitetura do projeto
│ ├── design/          # Arquivos relacionados ao design do jogo
│ ├── tutorials/            # Tutoriais e guias da Framework
├── public/                 # Assets estáticos (imagens, vídeos, etc.)
│ ├── img/                  # Imagens
│ └── design/          # Arquivos relacionados ao design do jogo
└── src/                    # Projeto do Docusaurus
  ├── components/           # Componentes React
  ├── css/                  # Estilos CSS
  └── pages/                # Páginas do site

```

## Documentação

### Gerar a Documentação da API

Para gerar a documentação da API utilizando o DocFX, você precisará do projeto .NET em `src/`, ter os requisitos para criar uma build (siga os passos para o setup do ambiente de desenvolvimento no arquivo [`src/README`](../src/README.md)) e seguir os passos abaixo:

1. Certifique-se de ter o DocFX instalado. Você pode instalar o DocFX globalmente usando o comando:

   ```sh
   dotnet tool install -g docfx
   ```

2. Execute o comando abaixo para gerar a documentação da API:

   ```sh
   docfx
   ```

   A documentação será gerada na pasta `docs/content/api`.

### Executar Scripts NPM

Para executar os scripts definidos no arquivo [`package.json`](package.json):

- Para processar os arquivos da API:

  ```sh
  npm --prefix docs run process-api
  ```

  O comando irá executar o código `node processApiFiles.js`, que trata os arquivos gerados pelo DocFX de acordo com [`processApiFiles.js`](processApiFiles.js)

- Para iniciar o servidor de desenvolvimento do Docusaurus:

  ```sh
  npm --prefix docs run start
  ```

Agora você está pronto para gerar a documentação da API e executar os scripts necessários para o desenvolvimento e manutenção da documentação do projeto.
