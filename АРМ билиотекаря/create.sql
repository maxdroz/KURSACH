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

DROP PROCEDURE IF EXISTS `log_data`;
CREATE PROCEDURE `log_data`(data varchar(255))
BEGIN
	INSERT INTO log values(null, now(), data);
END $$

DROP TRIGGER IF EXISTS `reader1`;
CREATE TRIGGER `reader1` AFTER INSERT
ON reader FOR EACH ROW BEGIN
	call log_data(CONCAT('Inserted reader with id: ', new.id));
END $$

DROP TRIGGER IF EXISTS `record1`;
CREATE TRIGGER `record1` AFTER INSERT
ON record FOR EACH ROW BEGIN
	call log_data(CONCAT('Inserted record with id: ', new.id));
END $$

DROP TRIGGER IF EXISTS `book1`;
CREATE TRIGGER `book1` AFTER INSERT
ON book FOR EACH ROW BEGIN
	call log_data(CONCAT('Inserted book with id: ', new.id));
END $$

DROP TRIGGER IF EXISTS `author1`;
CREATE TRIGGER `author1` AFTER INSERT
ON author FOR EACH ROW BEGIN
	call log_data(CONCAT('Inserted author with id: ', new.id));
END $$

DROP TRIGGER IF EXISTS `genre1`;
CREATE TRIGGER `genre1` AFTER INSERT
ON genre FOR EACH ROW BEGIN
	call log_data(CONCAT('Inserted genre with id: ', new.id));
END $$

DROP TRIGGER IF EXISTS `language1`;
CREATE TRIGGER `language1` AFTER INSERT
ON language FOR EACH ROW BEGIN
	call log_data(CONCAT('Inserted language with id: ', new.id));
END $$

DROP TRIGGER IF EXISTS `publishing_house1`;
CREATE TRIGGER `publishing_house1` AFTER INSERT
ON publishing_house FOR EACH ROW BEGIN
	call log_data(CONCAT('Inserted publishing_house with id: ', new.id));
END $$

DROP TRIGGER IF EXISTS `city1`;
CREATE TRIGGER `city1` AFTER INSERT
ON city FOR EACH ROW BEGIN
	call log_data(CONCAT('Inserted city with id: ', new.id));
END $$

DROP TRIGGER IF EXISTS `cover1`;
CREATE TRIGGER `cover1` AFTER INSERT
ON cover FOR EACH ROW BEGIN
	call log_data(CONCAT('Inserted cover with id: ', new.id));
END $$



DROP TRIGGER IF EXISTS `reader2`;
CREATE TRIGGER `reader2` AFTER UPDATE
ON reader FOR EACH ROW BEGIN
	call log_data(CONCAT('Updated reader with id: ', new.id));
END $$

DROP TRIGGER IF EXISTS `record2`;
CREATE TRIGGER `record2` AFTER UPDATE
ON record FOR EACH ROW BEGIN
	call log_data(CONCAT('Updated record with id: ', new.id));
END $$

DROP TRIGGER IF EXISTS `book2`;
CREATE TRIGGER `book2` AFTER UPDATE
ON book FOR EACH ROW BEGIN
	call log_data(CONCAT('Updated book with id: ', new.id));
END $$

DROP TRIGGER IF EXISTS `author2`;
CREATE TRIGGER `author2` AFTER UPDATE
ON author FOR EACH ROW BEGIN
	call log_data(CONCAT('Updated author with id: ', new.id));
END $$

DROP TRIGGER IF EXISTS `genre2`;
CREATE TRIGGER `genre2` AFTER UPDATE
ON genre FOR EACH ROW BEGIN
	call log_data(CONCAT('Updated genre with id: ', new.id));
END $$

DROP TRIGGER IF EXISTS `language2`;
CREATE TRIGGER `language2` AFTER UPDATE
ON language FOR EACH ROW BEGIN
	call log_data(CONCAT('Updated language with id: ', new.id));
END $$

DROP TRIGGER IF EXISTS `publishing_house2`;
CREATE TRIGGER `publishing_house2` AFTER UPDATE
ON publishing_house FOR EACH ROW BEGIN
	call log_data(CONCAT('Updated publishing_house with id: ', new.id));
END $$

DROP TRIGGER IF EXISTS `city2`;
CREATE TRIGGER `city2` AFTER UPDATE
ON city FOR EACH ROW BEGIN
	call log_data(CONCAT('Updated city with id: ', new.id));
END $$

DROP TRIGGER IF EXISTS `cover2`;
CREATE TRIGGER `cover2` AFTER UPDATE
ON cover FOR EACH ROW BEGIN
	call log_data(CONCAT('Updated cover with id: ', new.id));
END $$



DROP TRIGGER IF EXISTS `reader3`;
CREATE TRIGGER `reader3` BEFORE DELETE
ON reader FOR EACH ROW BEGIN
	call log_data(CONCAT('Deleted reader with id: ', old.id));
END $$

DROP TRIGGER IF EXISTS `record3`;
CREATE TRIGGER `record3` BEFORE DELETE
ON record FOR EACH ROW BEGIN
	call log_data(CONCAT('Deleted record with id: ', old.id));
END $$

DROP TRIGGER IF EXISTS `book3`;
CREATE TRIGGER `book3` BEFORE DELETE
ON book FOR EACH ROW BEGIN
	call log_data(CONCAT('Deleted book with id: ', old.id));
END $$

DROP TRIGGER IF EXISTS `author3`;
CREATE TRIGGER `author3` BEFORE DELETE
ON author FOR EACH ROW BEGIN
	call log_data(CONCAT('Deleted author with id: ', old.id));
END $$

DROP TRIGGER IF EXISTS `genre3`;
CREATE TRIGGER `genre3` BEFORE DELETE
ON genre FOR EACH ROW BEGIN
	call log_data(CONCAT('Deleted genre with id: ', old.id));
END $$

DROP TRIGGER IF EXISTS `language3`;
CREATE TRIGGER `language3` BEFORE DELETE
ON language FOR EACH ROW BEGIN
	call log_data(CONCAT('Deleted language with id: ', old.id));
END $$

DROP TRIGGER IF EXISTS `publishing_house3`;
CREATE TRIGGER `publishing_house3` BEFORE DELETE
ON publishing_house FOR EACH ROW BEGIN
	call log_data(CONCAT('Deleted publishing_house with id: ', old.id));
END $$

DROP TRIGGER IF EXISTS `city3`;
CREATE TRIGGER `city3` BEFORE DELETE
ON city FOR EACH ROW BEGIN
	call log_data(CONCAT('Deleted city with id: ', old.id));
END $$

DROP TRIGGER IF EXISTS `cover3`;
CREATE TRIGGER `cover3` BEFORE DELETE
ON cover FOR EACH ROW BEGIN
	call log_data(CONCAT('Deleted cover with id: ', old.id));
END $$

