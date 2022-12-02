use master
go
if exists(select * from sys.databases where name = 'PongGameDB')
		drop database PongGameDB
go

create database PongGameDB
go

use PongGameDB
go

create table Usuario(
	Id int primary key identity,
	Nickname varchar(255),
);
go

create table Score(
	id int primary key identity,
	Player1Nickname int references Usuario(id),
	Player2Nickname int references Usuario(id),
	Player1Score int,
	Player2Score int,
);
go

create table RGB(
	id int primary key identity,
	arrayIndex int,
	UserId int references Usuario(Id),
	R int,
	G int, 
	B int,
);

select*from Usuario
select*from Score
select*from RGB