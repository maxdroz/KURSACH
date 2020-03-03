CREATE TABLE IF NOT EXISTS `Читатель` (
	`id` INT NOT NULL AUTO_INCREMENT,
	`Имя` VARCHAR(255) NOT NULL,
	`Фамилия` VARCHAR(255) NOT NULL,
	`Отчество` VARCHAR(255) NOT NULL,
	`Дата_рождения` VARCHAR(255) NOT NULL,
	`Номер_телефона` VARCHAR(255) NOT NULL,
	`Адрес` VARCHAR(255) NOT NULL,
	PRIMARY KEY (`id`)
);

CREATE TABLE IF NOT EXISTS `Запись` (
	`id` INT NOT NULL AUTO_INCREMENT,
	`Номер_читателя` INT NOT NULL,
	`Номер_книги` INT NOT NULL,
	`Дата_выдачи` DATETIME NOT NULL,
	`Дата_возврата` DATETIME NOT NULL,
	PRIMARY KEY (`id`)
);

CREATE TABLE IF NOT EXISTS `Книга` (
	`id` INT NOT NULL AUTO_INCREMENT,
	`Номер_автора` INT NOT NULL,
	`Номер_языка` INT NOT NULL,
	`Номер_жанра` INT NOT NULL,
	`Номер_издательства` INT NOT NULL,
	`Название` VARCHAR(255) NOT NULL,
	`Номер_описания_обложки` BINARY NOT NULL,
	PRIMARY KEY (`id`)
);

CREATE TABLE IF NOT EXISTS `Автор` (
	`id` INT NOT NULL AUTO_INCREMENT,
	`Имя` VARCHAR(255) NOT NULL,
	`Фамилия` VARCHAR(255) NOT NULL,
	`Отчество` VARCHAR(255) NOT NULL,
	PRIMARY KEY (`id`)
);

CREATE TABLE IF NOT EXISTS `Жанр` (
	`id` INT NOT NULL AUTO_INCREMENT,
	`Жанр` VARCHAR(255) NOT NULL AUTO_INCREMENT,
	PRIMARY KEY (`id`)
);

CREATE TABLE IF NOT EXISTS `Язык` (
	`id` INT NOT NULL AUTO_INCREMENT,
	`Язык` VARCHAR(255) NOT NULL AUTO_INCREMENT,
	PRIMARY KEY (`id`)
);

CREATE TABLE IF NOT EXISTS `Издательство` (
	`id` INT NOT NULL AUTO_INCREMENT,
	`Номер_города_издательства` INT NOT NULL,
	`Название` VARCHAR(255) NOT NULL,
	PRIMARY KEY (`id`)
);

CREATE TABLE IF NOT EXISTS `Город издательства` (
	`id` INT NOT NULL AUTO_INCREMENT,
	`Название_города` VARCHAR(255) NOT NULL,
	PRIMARY KEY (`id`)
);

CREATE TABLE IF NOT EXISTS `Тип_обложки` (
	`id` INT NOT NULL AUTO_INCREMENT,
	`Описание обложки` INT NOT NULL AUTO_INCREMENT,
	PRIMARY KEY (`id`)
);

ALTER TABLE `Запись` ADD CONSTRAINT `Запись_fk0` FOREIGN KEY (`Номер_читателя`) REFERENCES `Читатель`(`id`);

ALTER TABLE `Запись` ADD CONSTRAINT `Запись_fk1` FOREIGN KEY (`Номер_книги`) REFERENCES `Книга`(`id`);

ALTER TABLE `Книга` ADD CONSTRAINT `Книга_fk0` FOREIGN KEY (`Номер_автора`) REFERENCES `Автор`(`id`);

ALTER TABLE `Книга` ADD CONSTRAINT `Книга_fk1` FOREIGN KEY (`Номер_языка`) REFERENCES `Язык`(`id`);

ALTER TABLE `Книга` ADD CONSTRAINT `Книга_fk2` FOREIGN KEY (`Номер_жанра`) REFERENCES `Жанр`(`id`);

ALTER TABLE `Книга` ADD CONSTRAINT `Книга_fk3` FOREIGN KEY (`Номер_издательства`) REFERENCES `Издательство`(`id`);

ALTER TABLE `Книга` ADD CONSTRAINT `Книга_fk4` FOREIGN KEY (`Номер_описания_обложки`) REFERENCES `Тип_обложки`(`id`);

ALTER TABLE `Издательство` ADD CONSTRAINT `Издательство_fk0` FOREIGN KEY (`Номер_города_издательства`) REFERENCES `Город издательства`(`id`);
