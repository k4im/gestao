## Icarus
## As api's pertencentes a este projeto foram separadas em repositorios separados devido a utilização do github-actions
Cada serviço estará sendo adicionado a um container docker, assim como o gateway, e sistema de autenticação.

Cada serviço se torna idependente desta forma, sendo assim cada um deles terá seu proprio banco de dados, assim como suas proprias regras de negocios, fazendo com que apenas o serviço seja responsavel por leitura ou escrita na base de dado referente a ele, desta maneira será possivel estar utilizando tecnologias diversas para compor o sistema, não se limitando apenas um tipo de banco de dado ou linguagem.

Também será mais facil estar escalando o sistema conforme a necessidade, utilizando replicas de cada serviço. 
Toda a comunicação entre os serviços estará acontecendo de forma assincrona, para isto será utilizado um broker de mensageria(Service Bus), neste caso foi optado o **RabbiMQ**, o serviço remetente de dados estará criando uma exchange e uma fila para aquele tipo de mensagem, após determinada ação no serviço rementente será disparado uma mensagem para o broker, o serviço destinatário estará assinando a fila e à escutando, cada mensagem nova chegada na fila, o serviço destinatário estará realizando uma lógica de consumo e tratativa.

![Visão geral drawio](https://github.com/k4im/gestao/assets/108486349/69773d64-e767-4ca7-bca6-2403089e20e2)

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

## Objeto de valores & entidade de pessoas

![image](https://github.com/k4im/gestao/assets/108486349/d9f77dde-57f9-489e-9267-20b5640fffcf)

A entidade de pessoa é composta por quatro objetos de valores, sendo eles os objetos expostos acima, cada objeto de valor individualmente estará se auto validando, entre os quatro objetos apenas dois objetos tem motivos para mudarem, sendo eles o endereço e o telefone de uma pessoa.

Como adicionar uma nova pessoa:

- Primeiramente cada pessoa necessita de um nome, neste caso o mesmo deve ser composto apenas por letras, não podendo conter numeros ou caracteres especiais, caso exista caracteres especiais ou numeros, no momento de criação de uma nova pessoa será levantado uma exceção, impedindo a execução da função.

* Para cada endereço é necessário preencher todos os campos descritos no objeto neste caso é necessário que no campo de cep, esteja constando apenas numeros, pois o mesmo é composto apenas de numeros, caso seja adicionado letras ou caracteres especiais o sistema estará levantando uma exeção, impedindo a execução da função.

+ Para cada telefone, o campo **codigo de pais** assim como o campo **codigo de area** devem conter apenas dois numeros, caso seja adicionado numeros a mais nestes dois campos o sistema levantará uma exeção impedindo a execução da função, também é importante que em todos os campos de telefone seja adicionado apenas numeros, caso o contrario será levantada uma exceção informando a necessidade apenas de numeros no campo do obejto.

+ Para cada cpf, é necessário estar inserindo apenas os numeros, um cpf não pode estar contendo menos ou mais de 11 caracteres numericos, caso o contrario será levantada uma exceção impedindo a função de executar.

Após todos os campos serem preenchidos corretamente o sistema irá criar uma nova pessoa, para realizar uma atualização nesta entdidade é possivel estar realizando a troca de endereço & telefone.


