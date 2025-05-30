
# Kitchen - Sistema de Pedidos

Projeto para registrar pedidos em diferentes áreas da cozinha.
## Funcionalidades

- Criação de Pedidos.
- Pedidos com itens separados por Áreas da cozinha.
- Listagem dos pedidos com respectivos itens.

## Tecnologias Utilizadas

- .NET 9 API
- REST
- SQL Server
- Entity Framework
- Migrations
- Docker
- Clean Code
- Unity Tests (XUniy, Moq)
- Injeção de Dependência
- RabbitMQ

## Rodando localmente e Testando

#### **Pré requisitos**

Ter o **Docker** instalado em sua máquina.

Para instalar acesse: https://www.docker.com/products/docker-desktop/

Ter o SDK do .NET instalado em sua máquina.

Para instalar acesse: https://dotnet.microsoft.com/pt-br/download/dotnet/9.0

Acesse o diretório do projeto

```bash
  cd KitchenApi
```

Dentro da pasta raiz do projeto, rode o comando do Docker Compose

```bash
  docker-compose up --build
```

Após alguns minutos e a finalização, acesse a aplicaçao front end **Kitchen** através do navegador pelo endereço

[http://localhost:3000/](http://localhost:3000/)

Acesse **o Swagger da API** através do navegador pelo endereço

[http://localhost:8080/swagger/index.html](http://localhost:8080/swagger/index.html)

OBS: Caso ocorra problemas e a API não rode de primeira, rode atraves da interface do Docker Desktop.

Acesse a Interface do **RabbitMQ** para acompanhar as filas de pedidos através do navegador pelo endereço

[http://localhost:15672/#/](http://localhost:15672/#/)

**Login: guest**

**Senha: guest**

## Rodando Testes

Acesse a pasta do projeto KitchenApi, abra outro terminal e rode o comando

```bash
  dotnet test
```
