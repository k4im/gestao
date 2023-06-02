## Icarus
Cada serviço estará sendo adicionado a um container docker, assim como o gateway, e sistema de autenticação.

Cada serviço se torna idependente desta forma, sendo assim cada um deles terá seu proprio banco de dados, assim como suas proprias regras de negocios, fazendo com que apenas o serviço seja responsavel por leitura ou escrita na base de dado referente a ele, desta maneira será possivel estar utilizando tecnologias diversas para compor o sistema, não se limitando apenas um tipo de banco de dado ou linguagem.

Também será mais facil estar escalando o sistema conforme a necessidade, utilizando replicas de cada serviço. 
Toda a comunicação entre os serviços estará acontecendo de forma assincrona, para isto será utilizado um broker de mensageria(Service Bus), neste caso foi optado o **RabbiMQ**, o serviço remetente de dados estará criando uma exchange e uma fila para aquele tipo de mensagem, após determinada ação no serviço rementente será disparado uma mensagem para o broker, o serviço destinatário estará assinando a fila e à escutando, cada mensagem nova chegada na fila, o serviço destinatário estará realizando uma lógica de consumo e tratativa.

![image](https://github.com/k4im/gestao/assets/108486349/7020b8b2-d9af-491b-8cc8-f5dd42967b51)

## Comunicação entre os serviços
A comunicação no sistema funciona de forma assincrona, como citado acima. 

Neste caso a cada projeto novo adicionado ao serviço de projetos dispara uma mensagem que será enviada para o broker.

Após está mensagem estar no broker o serviço de estoque estará assinando a fila e consumindo as mensagens que existem nesta fila.

Para cada mensagem consumida o serviço de estoque realiza a lógica de subtração da quantidade de chapas utilizadas por cada projeto existente na fila. 

## Serviços

Atualmente o sistema consta com 4 serviços rodando, sendo eles:

- **Serviço de estoque:** estará realizando toda a administração relacionadas a produtos, sendo a adição, atualização e removoção de novos produtos, assim como tratativas relacionadas a quantidades de produtos que estão sendo utilizados por projetos.

* **Serviço de projetos:** estará realizando toda a parte administrativa relacionada a projetos, realizando os metodos de **CRUD,** também estará criando uma exchange e fila para enviar as novas mensagens ao broker assim que um projeto for criado, cada mensagem relacionado a este serviço conterá apenas o nome do projeto, nome do produto e a quantidade de produtos que foram utilizadas.

+ **Serviço de autenticacao:** este serviço estará realizando a parte de autenticação de cada novo usuario, assim como a criação de novos funcionarios e administradores para o sistema, para cada usuario criado será necessário estar tendo uma pessoa, seja ela funcionario ou administrador referenciando a este usuario. Após ter esta pessoa criada, será possivel estar realizando a criação de um novo usuario, para isso será apenas necessário estar informando o papel e senha que será utilizada para criar tal usuario, após realizado o procedimento será retornado a chave de acesso para login.

- **Serviço de fornecedores:** este serviço será responsavel por toda parte relacionadas a fornecedores, sendo os metodos **CRUD** e suas principais regras de negocio.

## Gateway
Para consumir os serviços será necessário estar eviando as requisições para o gateway, neste caso foi optado pelo **Kong** para realizar este papel.

O motivo de ser utilizado um gateway, é que por ser tratarem de projetos distintos os mesmos não serão expostos para rede externa, todos os serviços estarão tendo a comunicação apenas funcionando no ambiente onde foi realizado o deploy, então para se estar conseguindo consumir os mesmo será necessário estar enviando as requisições para o **Kong.**

Kong estará rodando em:
>**localhost:8000/**
## Autenticação

Toda a parte de autenticação está sendo feita por **JWT**, para estar consumindo os serviços será primariamente necessário estar realizando a autenticação com o usuario e recebendo o Access token e refresh token, o mesmo é valido por apenas 1h, 
após realizado a tratativa necessário pela parte que consome, basta estar adicionando este token ao header no momento da requisição ao **gateway**.

É necessário que a parte que consuma tenha um controle de quanto tempo se passou, para então poder reenviar uma requisição com o token de refresh solicitando um novo Access token.

