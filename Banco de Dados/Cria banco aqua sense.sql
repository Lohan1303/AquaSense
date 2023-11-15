go
create database AquaSense

go
use AquaSense


------------------------------------------Criação da tabela Usuario---------------------------------------------------
go
create table Usuario
(
	id_usuario int primary key identity(1,1) not null,
	login_usuario varchar(100) not null,
	nome_pessoa varchar(100) not null,
	senha varchar(100) not null,
	imagem varbinary(max) null,
	adm bit default(0) null,

	UNIQUE (login_usuario),
)

------------------------------------------Criação da tabela Conj. Habitacional------------------------------------------------
go
create table Conjunto_Habitacional
(
	id_conjunto_habitacional int primary key identity(1,1) not null,
	nome varchar(100) not null,
	endereco varchar(200) not null,
	cnpj varchar(20) not null,
	id_usuario_adm int not null,
	
	foreign key (id_usuario_adm) references Usuario(id_usuario) on delete cascade,

	UNIQUE (id_usuario_adm)
)

go
create or alter procedure spConsulta_ConjuntoHabitacionalPorUsuario
(
   @id_usuario_adm int
)
as
begin
 select * from Conjunto_Habitacional where id_usuario_adm = @id_usuario_adm
end
GO

------------------------------------------Criação da tabela Apartamento--------------------------------------------------
go
create table Apartamento
(
	id_apartamento int primary key identity(1,1) not null,
	numero_apartamento varchar(100) not null,
	id_conjunto_habitacional int not null,
	id_sensor int not null,
	id_usuario int null,

	foreign key (id_conjunto_habitacional) references Conjunto_Habitacional(id_conjunto_habitacional) on delete cascade,
	foreign key (id_usuario) references Usuario(id_usuario),
)


------------------------------------------Criação da tabela Sensor--------------------------------------------------
go
create table Sensor
(
	id_sensor int primary key identity(1,1) not null,
	descricao varchar(100) not null,
	id_apartamento int not null,

	foreign key (id_apartamento) references Apartamento(id_apartamento)
)