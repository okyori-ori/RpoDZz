USE CollegeDB;
GO
CREATE TABLE college.student_audit
(
    id INT IDENTITY(1,1) PRIMARY KEY,
    student_id INT NOT NULL,
    student_name NVARCHAR(100) NOT NULL,
    action_type NVARCHAR(20) NOT NULL,
    action_date DATETIME2 NOT NULL
);
GO
CREATE TRIGGER college.trg_Students_AuditInsert
ON college.students
AFTER INSERT
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO college.student_audit
        (student_id, student_name, action_type, action_date)
    SELECT id, name, N'INSERT', SYSDATETIME()
    FROM inserted;
END;
GO

CREATE TABLE college.student_changes
(
    id INT IDENTITY(1,1) PRIMARY KEY,
    student_id INT NOT NULL,
    old_average_grade DECIMAL(4,2),
    new_average_grade DECIMAL(4,2),
    change_date DATETIME2 NOT NULL
);
GO
CREATE TRIGGER college.trg_Students_AuditUpdate
ON college.students
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO college.student_changes
        (student_id, old_average_grade,
         new_average_grade, change_date)
    SELECT i.id, d.average_grade, i.average_grade, SYSDATETIME()
    FROM inserted i
    JOIN deleted d ON d.id = i.id
    WHERE i.average_grade <> d.average_grade
       OR (i.average_grade IS NULL AND d.average_grade IS NOT NULL)
       OR (i.average_grade IS NOT NULL AND d.average_grade IS NULL);
END;
GO

CREATE TRIGGER college.trg_Students_CheckGrade
ON college.students
AFTER INSERT, UPDATE
AS
BEGIN
    SET NOCOUNT ON;
    IF EXISTS (SELECT 1 FROM inserted WHERE average_grade > 5)
    BEGIN
        THROW 50002, N'Средний балл не может быть больше 5', 1;
    END;
END;
GO

INSERT INTO college.students (name, age, average_grade, group_id)
VALUES (N'Олег', 18, 4.0, 2), (N'Елена', 19, 4.3, 2);
SELECT * FROM college.student_audit ORDER BY id;

UPDATE college.students
SET average_grade = average_grade + 0.1
WHERE group_id = 1;
SELECT * FROM college.student_changes ORDER BY id;
GO

BEGIN TRY
    UPDATE college.students SET average_grade = 5.5 WHERE id = 1;
END TRY
BEGIN CATCH
    SELECT ERROR_NUMBER() AS error_number,
           ERROR_MESSAGE() AS error_message;
END CATCH;
SELECT id, name, average_grade FROM college.students WHERE id = 1;
