IF DB_ID(N'CollegeDB') IS NULL
    EXEC(N'CREATE DATABASE CollegeDB');
GO
USE CollegeDB;
GO
IF SCHEMA_ID(N'college') IS NULL
    EXEC(N'CREATE SCHEMA college');
GO
CREATE TABLE college.groups
(
    id INT IDENTITY(1,1) PRIMARY KEY,
    name NVARCHAR(100) NOT NULL,
    course INT NOT NULL
);

INSERT INTO college.groups (name, course)
VALUES (N'ИС-31', 3), (N'ИС-32', 3), (N'ПР-31', 3),
       (N'ПР-32', 3), (N'ДИЗ-31', 3);

SELECT * FROM college.groups;
SELECT * FROM college.groups WHERE course = 3;

UPDATE college.groups SET name = N'ИС-33' WHERE id = 2;
DELETE FROM college.groups WHERE id = 5;

ALTER TABLE college.groups
ADD CONSTRAINT CK_groups_course CHECK (course >= 1);

SELECT * FROM college.groups;
