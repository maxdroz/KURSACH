CREATE TABLE IF NOT EXISTS `reader` (
  `id` int PRIMARY KEY NOT NULL AUTO_INCREMENT,
  `name` varchar(255) NOT NULL,
  `surname` varchar(255) NOT NULL,
  `patronymic` varchar(255) NOT NULL,
  `birthday` DATETIME NOT NULL,
  `phone_number` varchar(255) NOT NULL,
  `address` varchar(255) NOT NULL
)
COLLATE utf8_general_ci;

CREATE TABLE IF NOT EXISTS `record` (
  `id` int PRIMARY KEY NOT NULL AUTO_INCREMENT,
  `id_librarian` int NOT NULL,
  `id_reader` int NOT NULL,
  `id_book` int NOT NULL,
  `issue_date` DATETIME NOT NULL,
  `return_date` DATETIME NOT NULL
)
COLLATE utf8_general_ci;

CREATE TABLE IF NOT EXISTS `book` (
  `id` int PRIMARY KEY NOT NULL AUTO_INCREMENT,
  `id_author` int NOT NULL,
  `id_language` int NOT NULL,
  `id_genre` int NOT NULL,
  `id_publishing_house` int NOT NULL,
  `id_cover` int NOT NULL,
  `id_era` int NOT NULL,
  `id_type_of_literature` int NOT NULL,
  `id_book_size` int NOT NULL,
  `id_font_size` int NOT NULL,
  `title` varchar(255) NOT NULL
)
COLLATE utf8_general_ci;

CREATE TABLE IF NOT EXISTS `author` (
  `id` int PRIMARY KEY NOT NULL AUTO_INCREMENT,
  `name` varchar(255) NOT NULL,
  `surname` varchar(255) NOT NULL,
  `patronymic` varchar(255) NOT NULL
)
COLLATE utf8_general_ci;

CREATE TABLE IF NOT EXISTS `genre` (
  `id` int PRIMARY KEY NOT NULL AUTO_INCREMENT,
  `genre` varchar(255) NOT NULL
)
COLLATE utf8_general_ci;

CREATE TABLE IF NOT EXISTS `language` (
  `id` int PRIMARY KEY NOT NULL AUTO_INCREMENT,
  `language` varchar(255) NOT NULL
)
COLLATE utf8_general_ci;

CREATE TABLE IF NOT EXISTS `publishing_house` (
  `id` int PRIMARY KEY NOT NULL AUTO_INCREMENT,
  `id_city` int NOT NULL,
  `title` varchar(255) NOT NULL
)
COLLATE utf8_general_ci;

CREATE TABLE IF NOT EXISTS `city` (
  `id` int PRIMARY KEY NOT NULL AUTO_INCREMENT,
  `city_name` varchar(255) NOT NULL
)
COLLATE utf8_general_ci;

CREATE TABLE IF NOT EXISTS `cover` (
  `id` int PRIMARY KEY NOT NULL AUTO_INCREMENT,
  `cover_description` varchar(255) NOT NULL
)
COLLATE utf8_general_ci;

CREATE TABLE IF NOT EXISTS `era` (
  `id` int PRIMARY KEY NOT NULL AUTO_INCREMENT,
  `era` varchar(255) NOT NULL
)
COLLATE utf8_general_ci;

CREATE TABLE IF NOT EXISTS `type_of_literature` (
  `id` int PRIMARY KEY NOT NULL AUTO_INCREMENT,
  `type_of_literature` varchar(255) NOT NULL
)
COLLATE utf8_general_ci;

CREATE TABLE IF NOT EXISTS `librarian` (
  `id` int PRIMARY KEY NOT NULL AUTO_INCREMENT,
  `name` varchar(255) NOT NULL,
  `surname` varchar(255) NOT NULL,
  `password_hash` text NOT NULL,
  `id_admin` int NOT NULL
)
COLLATE utf8_general_ci;

CREATE TABLE IF NOT EXISTS `admin` (
  `id` int PRIMARY KEY NOT NULL AUTO_INCREMENT,
  `name` varchar(255) NOT NULL,
  `surname` varchar(255) NOT NULL,
  `password_hash` text NOT NULL
)
COLLATE utf8_general_ci;

CREATE TABLE IF NOT EXISTS `book_size` (
  `id` int PRIMARY KEY NOT NULL AUTO_INCREMENT,
  `name` varchar(255) NOT NULL,
  `dimension_x_mm` int NOT NULL,
  `dimension_y_mm` int NOT NULL
)
COLLATE utf8_general_ci;

CREATE TABLE IF NOT EXISTS `font_size` (
  `id` int PRIMARY KEY NOT NULL AUTO_INCREMENT,
  `name` varchar(255) NOT NULL,
  `value` int NOT NULL
)
COLLATE utf8_general_ci;

CREATE TABLE IF NOT EXISTS `logs` (
  `id` int PRIMARY KEY NOT NULL AUTO_INCREMENT,
  `table_name` varchar(255) NOT NULL,
  `prev_value` varchar(255),
  `new_value` varchar(255),
  `time` DATETIME NOT NULL
)
COLLATE utf8_general_ci;

ALTER TABLE `book` ADD FOREIGN KEY (`id_language`) REFERENCES `language` (`id`);

ALTER TABLE `book` ADD FOREIGN KEY (`id_author`) REFERENCES `author` (`id`);

ALTER TABLE `publishing_house` ADD FOREIGN KEY (`id_city`) REFERENCES `city` (`id`);

ALTER TABLE `book` ADD FOREIGN KEY (`id_publishing_house`) REFERENCES `publishing_house` (`id`);

ALTER TABLE `record` ADD FOREIGN KEY (`id_book`) REFERENCES `book` (`id`);

ALTER TABLE `record` ADD FOREIGN KEY (`id_reader`) REFERENCES `reader` (`id`);

ALTER TABLE `book` ADD FOREIGN KEY (`id_genre`) REFERENCES `genre` (`id`);

ALTER TABLE `book` ADD FOREIGN KEY (`id_cover`) REFERENCES `cover` (`id`);

ALTER TABLE `book` ADD FOREIGN KEY (`id_era`) REFERENCES `era` (`id`);

ALTER TABLE `record` ADD FOREIGN KEY (`id_librarian`) REFERENCES `librarian` (`id`);

ALTER TABLE `book` ADD FOREIGN KEY (`id_type_of_literature`) REFERENCES `type_of_literature` (`id`);

ALTER TABLE `librarian` ADD FOREIGN KEY (`id_admin`) REFERENCES `admin` (`id`);

ALTER TABLE `book` ADD FOREIGN KEY (`id_book_size`) REFERENCES `book_size` (`id`);

ALTER TABLE `book` ADD FOREIGN KEY (`id_font_size`) REFERENCES `font_size` (`id`);

CREATE TABLE IF NOT EXISTS `log` (
	`id` int AUTO_INCREMENT NOT NULL,
	`timestamp` DATETIME NOT NULL,
	`cover_description` varchar(255) NOT NULL,
	PRIMARY KEY (`id`)
)
COLLATE utf8_general_ci;
