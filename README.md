## Icarus
Cada serviço estará sendo adicionado a um container docker, assim como o gateway, e sistema de autenticação.

Cada serviço se torna idependente desta forma, sendo assim cada um deles terá seu proprio banco de dados, assim como suas proprias regras de negocios, fazendo com que apenas o serviço seja responsavel por leitura ou escrita na base de dado referente a ele, desta maneira será possivel estar utilizando tecnologias diversas para compor o sistema, não se limitando apenas um tipo de banco de dado ou linguagem.

Também será mais facil estar escalando o sistema conforme a necessidade, utilizando replicas de cada serviço. 
Toda a comunicação entre os serviços estará acontecendo de forma assincrona, para isto será utilizado um broker de mensageria(Service Bus), neste caso foi optado o **RabbiMQ**, o serviço remetente de dados estará criando uma exchange e uma fila para aquele tipo de mensagem, após determinada ação no serviço rementente será disparado uma mensagem para o broker, o serviço destinatário estará assinando a fila e à escutando, cada mensagem nova chegada na fila, o serviço destinatário estará realizando uma lógica de consumo e tratativa.

![image](https://github.com/k4im/gestao/assets/108486349/7020b8b2-d9af-491b-8cc8-f5dd42967b51)

## Comunicação entre os sistemas
A comunicação no sistema funciona de forma assincrona, como citado acima. 

Neste caso a cada projeto novo adicionado ao serviço de projetos dispara uma mensagem que será enviada para o broker.

Após está mensagem estar no broker o serviço de estoque estará assinando a fila e consumindo as mensagens que existem nesta fila.

Para cada mensagem consumida o serviço de estoque realiza a lógica de subtração da quantidade de chapas utilizadas por cada projeto existente na fila. 