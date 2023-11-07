
# Arquivos de gestão do projeto Icarus.

Através deste repositório podera ser encontrado os arquivos **.yml** para realizar o deploy por completo do sistema icarus.


![Visão geral drawio](https://github.com/k4im/gestao/assets/108486349/7bfb3a83-3920-4f28-ba4f-9d086d4cf028)
## Tecnologias utilizadas

![Docker](https://img.shields.io/badge/docker-%230db7ed.svg?style=for-the-badge&logo=docker&logoColor=white) ![RabbitMQ](https://img.shields.io/badge/Rabbitmq-FF6600?style=for-the-badge&logo=rabbitmq&logoColor=white) ![C#](https://img.shields.io/badge/c%23-%23239120.svg?style=for-the-badge&logo=c-sharp&logoColor=white)
## Variaveis de ambientes

Realizei uma verificação referente as variaveis de ambiente configuraveis.


`ASPNETCORE_ENVIRONMENT`

`DB_CONNECTION`

`RABBIT_MQ_USER`

`RABBIT_MQ_PWD`

#### DB_CONNECTION
* Variavel responsavel por estar realizando a configuração de conexão com o banco de dados. A mesma pode ser configuravel através dos arquivos de configurações assim como repassando por argumentos na execução docker.

#### RABBIT_MQ_USER
* Variavel responsavel por estar repassando a configuração o usuario de acesso ao RabbitMQ.

#### RABBIT_MQ_PWD 
* Variavel responsavel por estar realizando a autenticação do usuario.

## Comunicação

A comunicação no sistema funciona de forma assincrona, como citado acima.

Neste caso a cada projeto novo adicionado ao serviço de projetos dispara uma mensagem que será enviada para o broker.

Após está mensagem estar no broker o serviço de estoque estará assinando a fila e consumindo as mensagens que existem nesta fila.

Para cada mensagem consumida o serviço de estoque realiza a lógica de subtração da quantidade de chapas utilizadas por cada projeto existente na fila.
## Serviços disponiveis

Atualmente o sistema consta com 4 serviços rodando, sendo eles:

- **Serviço de estoque:** estará realizando toda a administração relacionadas a produtos, sendo a adição, atualização e removoção de novos produtos, assim como tratativas relacionadas a quantidades de produtos que estão sendo utilizados por projetos.

* **Serviço de projetos:** estará realizando toda a parte administrativa relacionada a projetos, realizando os metodos de **CRUD,** também estará criando uma exchange e fila para enviar as novas mensagens ao broker assim que um projeto for criado, cada mensagem relacionado a este serviço conterá apenas o nome do projeto, nome do produto e a quantidade de produtos que foram utilizadas.

+ **Serviço de autenticacao:** este serviço estará realizando a parte de autenticação de cada novo usuario, assim como a criação de novos funcionarios e administradores para o sistema, para cada usuario criado será necessário estar tendo uma pessoa, seja ela funcionario ou administrador referenciando a este usuario. Após ter esta pessoa criada, será possivel estar realizando a criação de um novo usuario, para isso será apenas necessário estar informando o papel e senha que será utilizada para criar tal usuario, após realizado o procedimento será retornado a chave de acesso para login.
## Workers

O projeto contará com dois worker que serão utilizados para comunicação entre as api de projeto e estoque.

O papel destes workers será estar realizando periodicamente consulta ao rabbitmq para verificação de novas mensagens em suas determinas exchanges/filas.

* atualmente o projeto conta com o worker da api de projetos em funcionamento.


## Deploy Backend

Para fazer o deploy desse projeto rode

```bash
  cd app/backend/
```

```bash
  docker-compose up -d
```

