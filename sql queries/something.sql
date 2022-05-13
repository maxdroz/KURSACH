use testDB;

delimiter //

DROP TRIGGER IF EXISTS log_librarian_updates_insert;

CREATE TRIGGER log_librarian_updates_insert 
AFTER INSERT 
ON librarian
FOR EACH ROW 
BEGIN
	INSERT INTO logs(table_name, prev_value, new_value, time) VALUES (
		"librarian",
		null,
		CONCAT("id = ", NEW.id, ", name = ", NEW.name, ", surname = ", NEW.surname, ", password_hash = ", NEW.password_hash, ", id_admin = ", NEW.id_admin), 
		CURRENT_TIMESTAMP()
	);
END;//

DROP TRIGGER IF EXISTS log_librarian_updates_update;

CREATE TRIGGER log_librarian_updates_update
BEFORE UPDATE
ON librarian
FOR EACH ROW 
BEGIN
	INSERT INTO logs(table_name, prev_value, new_value, time) VALUES (
		"librarian", 
		CONCAT("id = ", OLD.id, ", name = ", OLD.name, ", surname = ", OLD.surname, ", password_hash = ", OLD.password_hash, ", id_admin = ", OLD.id_admin), 
		CONCAT("id = ", NEW.id, ", name = ", NEW.name, ", surname = ", NEW.surname, ", password_hash = ", NEW.password_hash, ", id_admin = ", NEW.id_admin), 
		CURRENT_TIMESTAMP()
	);
END;//

DROP TRIGGER IF EXISTS log_librarian_updates_delete;

CREATE TRIGGER log_librarian_updates_delete
BEFORE DELETE
ON librarian
FOR EACH ROW 
BEGIN
	INSERT INTO logs(table_name, prev_value, new_value, time) VALUES (
		"librarian", 
		CONCAT("id = ", OLD.id, ", ", "name = ", OLD.name, ", ", "surname = ", OLD.surname, ", ", "password_hash = ", OLD.password_hash, ", ", "id_admin = ", OLD.id_admin), 
		null, 
		CURRENT_TIMESTAMP()
	);
END;//

-- Used to generate "CONCAT" expressions for triggers
DROP FUNCTION IF EXISTS get_table_statement;
CREATE FUNCTION get_table_statement(tableName TEXT)
RETURNS TEXT
READS SQL DATA
DETERMINISTIC
BEGIN
	SELECT CONCAT("CONCAT(", GROUP_CONCAT(CONCAT('"', column_name, ' = ",' ,' OLD.', column_name) SEPARATOR ', ", ", '), ")")
    INTO @table_fields
	FROM   `information_schema`.`columns` 
	WHERE  `table_schema`=DATABASE() 
       AND `table_name`= tableName;

	RETURN @table_fields;
END;//    
