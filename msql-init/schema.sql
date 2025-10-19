use crud_db;
create table People
(
Id int primary key auto_increment,
Name varchar(50) not null,
BirthDate timestamp  not null,
Gender varchar(10) not null,
AcceptTerms bool not null
);