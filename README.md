Cada serviço estará sendo adicionado a um container docker, assim como o gateway, e sistema de autenticação.

Cada serviço se torna idependente desta forma, sendo assim cada um deles terá seu proprio banco de dados, assim como suas proprias regras de negocios, fazendo com que apenas o serviço seja responsavel por leitura ou escrita na base de dado referente a ele, desta maneira será possivel estar utilizando tecnologias diversas para compor o sistema, não se limitando apenas um tipo de banco de dado ou linguagem.

Também será mais facil estar escalando o sistema conforme a necessidade, utilizando replicas de cada serviço. Toda a comunicação entre os serviços estará acontecendo de forma assincrona, para isto será utilizado um broker de mensageria(Service Bus), neste caso foi optado o **RabbiMQ**, o serviço que necessita de um dado de outo estará se comunicando previamente com o service bus, enviando assim uma mensagem para o mesmo, após isto o serviço que necessita estar assinando a fila referente a aquele serviço, fazendo a escuta da mesma, conforme a chegada de novos dados na fila, o serviço estará realizando a lógica para consumo e tratativa destes dados!

![image](https://github.com/k4im/gestao/assets/108486349/7020b8b2-d9af-491b-8cc8-f5dd42967b51)
