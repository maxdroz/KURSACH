CREATE TABLE IF NOT EXISTS `reader` (
	`id` int AUTO_INCREMENT NOT NULL,
	`name` varchar(255) NOT NULL,
	`surname` varchar(255) NOT NULL,
	`patronymic` varchar(255) NOT NULL,
	`birthday` DATETIME NOT NULL,
	`phone_number` varchar(255) NOT NULL,
	`address` varchar(255) NOT NULL,
	PRIMARY KEY (`id`)
);

CREATE TABLE IF NOT EXISTS `record` (
	`id` int AUTO_INCREMENT NOT NULL,
	`id_reader` int NOT NULL,
	`id_book` int NOT NULL,
	`issue_date` DATETIME NOT NULL,
	`return_date` DATETIME NOT NULL,
	PRIMARY KEY (`id`)
);

CREATE TABLE IF NOT EXISTS `book` (
	`id` int AUTO_INCREMENT NOT NULL,
	`id_author` int NOT NULL,
	`id_language` int NOT NULL,
	`id_genre` int NOT NULL,
	`id_publishing_house` int NOT NULL,
	`id_cover` int NOT NULL,
	`title` varchar(255) NOT NULL,
	PRIMARY KEY (`id`)
);

CREATE TABLE IF NOT EXISTS `author` (
	`id` int AUTO_INCREMENT NOT NULL,
	`name` varchar(255) NOT NULL,
	`surname` varchar(255) NOT NULL,
	`patronymic` varchar(255) NOT NULL,
	PRIMARY KEY (`id`)
);

CREATE TABLE IF NOT EXISTS `genre` (
	`id` int AUTO_INCREMENT NOT NULL,
	`genre` varchar(255) NOT NULL,
	PRIMARY KEY (`id`)
);

CREATE TABLE IF NOT EXISTS `language` (
	`id` int AUTO_INCREMENT NOT NULL,
	`language` varchar(255) NOT NULL,
	PRIMARY KEY (`id`)
);

CREATE TABLE IF NOT EXISTS `publishing_house` (
	`id` int AUTO_INCREMENT NOT NULL,
	`id_city` int NOT NULL,
	`title` varchar(255) NOT NULL,
	PRIMARY KEY (`id`)
);

CREATE TABLE IF NOT EXISTS `city` (
	`id` int AUTO_INCREMENT NOT NULL,
	`city_name` varchar(255) NOT NULL,
	PRIMARY KEY (`id`)
);

CREATE TABLE IF NOT EXISTS `cover` (
	`id` int AUTO_INCREMENT NOT NULL,
	`cover_description` varchar(255) NOT NULL,
	PRIMARY KEY (`id`)
);

ALTER TABLE `record` ADD CONSTRAINT FOREIGN KEY (`id_reader`) REFERENCES `reader`(`id`);

ALTER TABLE `record` ADD CONSTRAINT FOREIGN KEY (`id_book`) REFERENCES `book`(`id`);

ALTER TABLE `book` ADD CONSTRAINT FOREIGN KEY (`id_author`) REFERENCES `author`(`id`);

ALTER TABLE `book` ADD CONSTRAINT FOREIGN KEY (`id_language`) REFERENCES `language`(`id`);

ALTER TABLE `book` ADD CONSTRAINT FOREIGN KEY (`id_genre`) REFERENCES `genre`(`id`);

ALTER TABLE `book` ADD CONSTRAINT FOREIGN KEY (`id_publishing_house`) REFERENCES `publishing_house`(`id`);

ALTER TABLE `book` ADD CONSTRAINT FOREIGN KEY (`id_cover`) REFERENCES `cover`(`id`);

ALTER TABLE `publishing_house` ADD CONSTRAINT FOREIGN KEY (`id_city`) REFERENCES `city`(`id`);	

CREATE TABLE IF NOT EXISTS `log` (
	`id` int AUTO_INCREMENT NOT NULL,
	`timestamp` DATETIME NOT NULL,
	`cover_description` varchar(255) NOT NULL,
	PRIMARY KEY (`id`)
);
