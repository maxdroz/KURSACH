use library;

delimiter //

DROP FUNCTION IF EXISTS creare_log_insert_librarian_statement;
CREATE FUNCTION creare_log_insert_librarian_statement(id TEXT, name TEXT, surname TEXT, password_hash TEXT, id_admin TEXT)
RETURNS TEXT
DETERMINISTIC
BEGIN
	SELECT CONCAT("INSERT id = ", id, ", name = ", name, ", surname = ", surname, ", password_hash = ", password_hash, ", id_admin = ", id_admin)
	INTO @res;
	RETURN @res;
END;//

DROP TRIGGER IF EXISTS log_librarian_updates_insert;
CREATE TRIGGER log_librarian_updates_insert 
AFTER INSERT 
ON librarian
FOR EACH ROW 
BEGIN
	INSERT INTO logs(table_name, prev_value, new_value, time) VALUES (
		"librarian",
		null,
		creare_log_insert_librarian_statement(NEW.id, NEW.name, NEW.surname, NEW.password_hash, NEW.id_admin), 
		CURRENT_TIMESTAMP()
	);
END;//

DROP FUNCTION IF EXISTS creare_log_update_librarian_statement;
CREATE FUNCTION creare_log_update_librarian_statement(id TEXT, name TEXT, surname TEXT, password_hash TEXT, id_admin TEXT)
RETURNS TEXT
DETERMINISTIC
BEGIN
	SELECT CONCAT("UPDATE id = ", id, ", name = ", name, ", surname = ", surname, ", password_hash = ", password_hash, ", id_admin = ", id_admin)
	INTO @res;
	RETURN @res;
END;//

DROP TRIGGER IF EXISTS log_librarian_updates_update;
CREATE TRIGGER log_librarian_updates_update
BEFORE UPDATE
ON librarian
FOR EACH ROW 
BEGIN
	INSERT INTO logs(table_name, prev_value, new_value, time) VALUES (
		"librarian", 
		creare_log_update_librarian_statement(OLD.id, OLD.name, OLD.surname, OLD.password_hash, OLD.id_admin), 
		creare_log_update_librarian_statement(NEW.id, NEW.name, NEW.surname, NEW.password_hash, NEW.id_admin), 
		CURRENT_TIMESTAMP()
	);
END;//

DROP FUNCTION IF EXISTS creare_log_delete_librarian_statement;
CREATE FUNCTION creare_log_delete_librarian_statement(id TEXT, name TEXT, surname TEXT, password_hash TEXT, id_admin TEXT)
RETURNS TEXT
DETERMINISTIC
BEGIN
	SELECT CONCAT("DELETE id = ", id, ", name = ", name, ", surname = ", surname, ", password_hash = ", password_hash, ", id_admin = ", id_admin)
	INTO @res;
	RETURN @res;
END;//

DROP TRIGGER IF EXISTS log_librarian_updates_delete;
CREATE TRIGGER log_librarian_updates_delete
BEFORE DELETE
ON librarian
FOR EACH ROW 
BEGIN
	INSERT INTO logs(table_name, prev_value, new_value, time) VALUES (
		"librarian", 
		creare_log_delete_librarian_statement(OLD.id, OLD.name, OLD.surname, OLD.password_hash, OLD.id_admin), 
		null, 
		CURRENT_TIMESTAMP()
	);
END;//

-- ----------------------------------------

DROP FUNCTION IF EXISTS creare_log_insert_admin_statement;
CREATE FUNCTION creare_log_insert_admin_statement(id TEXT, name TEXT, surname TEXT, password_hash TEXT)
RETURNS TEXT
DETERMINISTIC
BEGIN
	SELECT CONCAT("INSERT id = ", id, ", ", "name = ", name, ", ", "surname = ", surname, ", ", "password_hash = ", password_hash)
	INTO @res;
	RETURN @res;
END;//

DROP TRIGGER IF EXISTS log_admin_updates_insert;
CREATE TRIGGER log_admin_updates_insert 
AFTER INSERT 
ON `admin`
FOR EACH ROW 
BEGIN
	INSERT INTO logs(table_name, prev_value, new_value, time) VALUES (
		"admin",
		null,
		creare_log_insert_admin_statement(NEW.id, NEW.name, NEW.surname, NEW.password_hash), 
		CURRENT_TIMESTAMP()
	);
END;//

DROP FUNCTION IF EXISTS creare_log_update_admin_statement;
CREATE FUNCTION creare_log_update_admin_statement(id TEXT, name TEXT, surname TEXT, password_hash TEXT)
RETURNS TEXT
DETERMINISTIC
BEGIN
	SELECT CONCAT("UPDATE id = ", id, ", name = ", name, ", surname = ", surname, ", password_hash = ", password_hash)
	INTO @res;
	RETURN @res;
END;//

DROP TRIGGER IF EXISTS log_admin_updates_update;
CREATE TRIGGER log_admin_updates_update
BEFORE UPDATE
ON `admin`
FOR EACH ROW 
BEGIN
	INSERT INTO logs(table_name, prev_value, new_value, time) VALUES (
		"admin", 
		creare_log_update_admin_statement(OLD.id, OLD.name, OLD.surname, OLD.password_hash), 
		creare_log_update_admin_statement(NEW.id, NEW.name, NEW.surname, NEW.password_hash), 
		CURRENT_TIMESTAMP()
	);
END;//

DROP FUNCTION IF EXISTS creare_log_delete_admin_statement;
CREATE FUNCTION creare_log_delete_admin_statement(id TEXT, name TEXT, surname TEXT, password_hash TEXT)
RETURNS TEXT
DETERMINISTIC
BEGIN
	SELECT CONCAT("DELETE id = ", id, ", name = ", name, ", surname = ", surname, ", password_hash = ", password_hash)
	INTO @res;
	RETURN @res;
END;//

DROP TRIGGER IF EXISTS log_admin_updates_delete;
CREATE TRIGGER log_admin_updates_delete
BEFORE DELETE
ON `admin`
FOR EACH ROW 
BEGIN
	INSERT INTO logs(table_name, prev_value, new_value, time) VALUES (
		"admin", 
		creare_log_delete_admin_statement(OLD.id, OLD.name, OLD.surname, OLD.password_hash), 
		null, 
		CURRENT_TIMESTAMP()
	);
END;//

-- ---------------------------------------

DROP FUNCTION IF EXISTS creare_log_insert_record_statement;
CREATE FUNCTION creare_log_insert_record_statement(id TEXT, id_librarian TEXT, id_reader TEXT, id_book TEXT, issue_date DATE, return_date DATE)
RETURNS TEXT
DETERMINISTIC
BEGIN
	SELECT CONCAT("INSERT id = ", id, ", ", "id_librarian = ", id_librarian, ", ", "id_reader = ", id_reader, ", ", "id_book = ", id_book, ", ", "issue_date = ", issue_date, ", ", "return_date = ", return_date)
	INTO @res;
	RETURN @res;
END;//

DROP TRIGGER IF EXISTS log_record_updates_insert;
CREATE TRIGGER log_record_updates_insert 
AFTER INSERT 
ON `record`
FOR EACH ROW 
BEGIN
	INSERT INTO logs(table_name, prev_value, new_value, time) VALUES (
		"record",
		null,
		creare_log_insert_record_statement(NEW.id, NEW.id_librarian, NEW.id_reader, NEW.id_book, NEW.issue_date, NEW.return_date), 
		CURRENT_TIMESTAMP()
	);
END;//

DROP FUNCTION IF EXISTS creare_log_update_record_statement;
CREATE FUNCTION creare_log_update_record_statement(id TEXT, id_librarian TEXT, id_reader TEXT, id_book TEXT, issue_date DATE, return_date DATE)
RETURNS TEXT
DETERMINISTIC
BEGIN
	SELECT CONCAT("UPDATE id = ", id, ", ", "id_librarian = ", id_librarian, ", ", "id_reader = ", id_reader, ", ", "id_book = ", id_book, ", ", "issue_date = ", issue_date, ", ", "return_date = ", return_date)
	INTO @res;
	RETURN @res;
END;//

DROP TRIGGER IF EXISTS log_record_updates_update;
CREATE TRIGGER log_record_updates_update
BEFORE UPDATE
ON `record`
FOR EACH ROW 
BEGIN
	INSERT INTO logs(table_name, prev_value, new_value, time) VALUES (
		"record", 
		creare_log_update_record_statement(OLD.id, OLD.id_librarian, OLD.id_reader, OLD.id_book, OLD.issue_date, OLD.return_date), 
		creare_log_update_record_statement(NEW.id, NEW.id_librarian, NEW.id_reader, NEW.id_book, NEW.issue_date, NEW.return_date), 
		CURRENT_TIMESTAMP()
	);
END;//

DROP PROCEDURE IF EXISTS creare_log_delete_record_statement;
CREATE PROCEDURE creare_log_delete_record_statement(id TEXT, id_librarian TEXT, id_reader TEXT, id_book TEXT, issue_date DATE, return_date DATE, out res TEXT)
BEGIN
	SELECT CONCAT("DELETE id = ", id, ", ", "id_librarian = ", id_librarian, ", ", "id_reader = ", id_reader, ", ", "id_book = ", id_book, ", ", "issue_date = ", issue_date, ", ", "return_date = ", return_date)
	INTO res;
END;//

DROP TRIGGER IF EXISTS log_record_updates_delete;
CREATE TRIGGER log_record_updates_delete
BEFORE DELETE
ON `record`
FOR EACH ROW 
BEGIN
	CALL creare_log_delete_record_statement(OLD.id, OLD.id_librarian, OLD.id_reader, OLD.id_book, OLD.issue_date, OLD.return_date, @res);
	INSERT INTO logs(table_name, prev_value, new_value, time) VALUES (
		"record", 
		@res, 
		null, 
		CURRENT_TIMESTAMP()
	);
END;//

-- ---------------------------------------

DROP PROCEDURE IF EXISTS creare_log_insert_cover_statement;
CREATE PROCEDURE creare_log_insert_cover_statement(id TEXT, cover_description TEXT, OUT result TEXT)
BEGIN
	SELECT CONCAT("INSERT id = ", id, ", ", "cover_description = ", cover_description)
	INTO result;
END;//

DROP TRIGGER IF EXISTS log_cover_updates_insert;
CREATE TRIGGER log_cover_updates_insert 
AFTER INSERT 
ON `cover`
FOR EACH ROW 
BEGIN
	CALL creare_log_insert_cover_statement(NEW.id, NEW.cover_description, @res);
	INSERT INTO logs(table_name, prev_value, new_value, time) VALUES (
		"cover",
		null,
		@res, 
		CURRENT_TIMESTAMP()
	);
END;//

DROP PROCEDURE IF EXISTS creare_log_update_cover_statement;
CREATE PROCEDURE creare_log_update_cover_statement(id TEXT, cover_description TEXT, OUT result TEXT)
BEGIN
	SELECT CONCAT("UPDATE id = ", id, ", ", "cover_description = ", cover_description)
	INTO result;
END;//

DROP TRIGGER IF EXISTS log_cover_updates_update;
CREATE TRIGGER log_cover_updates_update
BEFORE UPDATE
ON `cover`
FOR EACH ROW 
BEGIN
	CALL creare_log_update_cover_statement(OLD.id, OLD.cover_description, @res1);
	CALL creare_log_update_cover_statement(NEW.id, NEW.cover_description, @res2);
	INSERT INTO logs(table_name, prev_value, new_value, time) VALUES (
		"cover", 
		@res1, 
		@res2, 
		CURRENT_TIMESTAMP()
	);
END;//

DROP PROCEDURE IF EXISTS creare_log_delete_cover_statement;
CREATE PROCEDURE creare_log_delete_cover_statement(id TEXT, cover_description TEXT, OUT result TEXT)
BEGIN
	SELECT CONCAT("DELETE id = ", id, ", ", "cover_description = ", cover_description)
	INTO result;
END;//

DROP TRIGGER IF EXISTS log_cover_updates_delete;
CREATE TRIGGER log_cover_updates_delete
BEFORE DELETE
ON `cover`
FOR EACH ROW 
BEGIN
	CALL creare_log_delete_cover_statement(OLD.id, OLD.cover_description, @res);
	INSERT INTO logs(table_name, prev_value, new_value, time) VALUES (
		"cover", 
		@res, 
		null, 
		CURRENT_TIMESTAMP()
	);
END;//

-- ---------------------------------------

DROP PROCEDURE IF EXISTS creare_log_insert_era_statement;
CREATE PROCEDURE creare_log_insert_era_statement(id TEXT, era TEXT, OUT result TEXT)
BEGIN
	SELECT CONCAT("INSERT id = ", id, ", ", "era = ", era)
	INTO result;
END;//

DROP TRIGGER IF EXISTS log_era_updates_insert;
CREATE TRIGGER log_era_updates_insert 
AFTER INSERT 
ON `era`
FOR EACH ROW 
BEGIN
	CALL creare_log_insert_era_statement(NEW.id, NEW.era, @res);
	INSERT INTO logs(table_name, prev_value, new_value, time) VALUES (
		"era",
		null,
		@res, 
		CURRENT_TIMESTAMP()
	);
END;//

DROP PROCEDURE IF EXISTS creare_log_update_era_statement;
CREATE PROCEDURE creare_log_update_era_statement(id TEXT, era TEXT, OUT result TEXT)
BEGIN
	SELECT CONCAT("UPDATE id = ", id, ", ", "era = ", era)
	INTO result;
END;//

DROP TRIGGER IF EXISTS log_era_updates_update;
CREATE TRIGGER log_era_updates_update
BEFORE UPDATE
ON `era`
FOR EACH ROW 
BEGIN
	CALL creare_log_update_era_statement(OLD.id, OLD.era, @res1);
	CALL creare_log_update_era_statement(NEW.id, NEW.era, @res2);
	INSERT INTO logs(table_name, prev_value, new_value, time) VALUES (
		"era", 
		@res1, 
		@res2, 
		CURRENT_TIMESTAMP()
	);
END;//

DROP PROCEDURE IF EXISTS creare_log_delete_era_statement;
CREATE PROCEDURE creare_log_delete_era_statement(id TEXT, era TEXT, OUT result TEXT)
BEGIN
	SELECT CONCAT("DELETE id = ", id, ", ", "era = ", era)
	INTO result;
END;//

DROP TRIGGER IF EXISTS log_era_updates_delete;
CREATE TRIGGER log_era_updates_delete
BEFORE DELETE
ON `era`
FOR EACH ROW 
BEGIN
	CALL creare_log_delete_era_statement(OLD.id, OLD.era, @res);
	INSERT INTO logs(table_name, prev_value, new_value, time) VALUES (
		"era", 
		@res, 
		null, 
		CURRENT_TIMESTAMP()
	);
END;//
-- ---------------------------------------

DROP PROCEDURE IF EXISTS creare_log_insert_language_statement;
CREATE PROCEDURE creare_log_insert_language_statement(id TEXT, language TEXT, OUT result TEXT)
BEGIN
	SELECT CONCAT("INSERT id = ", id, ", ", "language = ", language)
	INTO result;
END;//

DROP TRIGGER IF EXISTS log_language_updates_insert;
CREATE TRIGGER log_language_updates_insert 
AFTER INSERT 
ON `language`
FOR EACH ROW 
BEGIN
	CALL creare_log_insert_language_statement(NEW.id, NEW.language, @res);
	INSERT INTO logs(table_name, prev_value, new_value, time) VALUES (
		"language",
		null,
		@res, 
		CURRENT_TIMESTAMP()
	);
END;//

DROP PROCEDURE IF EXISTS creare_log_update_language_statement;
CREATE PROCEDURE creare_log_update_language_statement(id TEXT, language TEXT, OUT result TEXT)
BEGIN
	SELECT CONCAT("UPDATE id = ", id, ", ", "language = ", language)
	INTO result;
END;//

DROP TRIGGER IF EXISTS log_language_updates_update;
CREATE TRIGGER log_language_updates_update
BEFORE UPDATE
ON `language`
FOR EACH ROW 
BEGIN
	CALL creare_log_update_language_statement(OLD.id, OLD.language, @res1);
	CALL creare_log_update_language_statement(NEW.id, NEW.language, @res2);
	INSERT INTO logs(table_name, prev_value, new_value, time) VALUES (
		"language", 
		@res1, 
		@res2, 
		CURRENT_TIMESTAMP()
	);
END;//

DROP PROCEDURE IF EXISTS creare_log_delete_language_statement;
CREATE PROCEDURE creare_log_delete_language_statement(id TEXT, language TEXT, OUT result TEXT)
BEGIN
	SELECT CONCAT("DELETE id = ", id, ", ", "language = ", language)
	INTO result;
END;//

DROP TRIGGER IF EXISTS log_language_updates_delete;
CREATE TRIGGER log_language_updates_delete
BEFORE DELETE
ON `language`
FOR EACH ROW 
BEGIN
	CALL creare_log_delete_language_statement(OLD.id, OLD.language, @res);
	INSERT INTO logs(table_name, prev_value, new_value, time) VALUES (
		"language", 
		@res, 
		null, 
		CURRENT_TIMESTAMP()
	);
END;//
-- ---------------------------------------

-- Used to generate "CONCAT" expressions for triggers
DROP FUNCTION IF EXISTS get_table_statement;

CREATE FUNCTION get_table_statement(tableName TEXT)
RETURNS TEXT
READS SQL DATA
DETERMINISTIC
BEGIN
	SELECT CONCAT("CONCAT(", GROUP_CONCAT(CONCAT('"', column_name, ' = ",' ,' ', column_name) SEPARATOR ', ", ", '), ")")
    INTO @table_fields
	FROM   `information_schema`.`columns` 
	WHERE  `table_schema`=DATABASE() 
       AND `table_name`= tableName;

	RETURN @table_fields;
END;//    

SELECT get_table_statement('era');