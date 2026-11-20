drop database if exists inscricoes;
CREATE DATABASE IF NOT EXISTS inscricoes
CHARACTER SET utf8mb4;
USE inscricoes;
drop table if exists estudante;
CREATE TABLE IF NOT EXISTS estudante (
id INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
nome VARCHAR(120) NOT NULL,
email VARCHAR(180) NOT NULL UNIQUE,
curso varchar(120) not null,
data_ingresso datetime,
telefone varchar(11),
cidade varchar(150)
);
/*
CREATE USER 'app_inscricoes'@'localhost'
IDENTIFIED BY 'localroot';
GRANT SELECT, INSERT, UPDATE, DELETE
ON inscricoes.* TO 'app_inscricoes'@'localhost';
*/

