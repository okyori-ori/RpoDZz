USE CollegeDB;
GO
CREATE TABLE college.grades
(
    id INT IDENTITY(1,1) PRIMARY KEY,
    student_id INT NOT NULL,
    grade INT NOT NULL CHECK (grade BETWEEN 2 AND 5),
    FOREIGN KEY (student_id) REFERENCES college.students(id)
);
INSERT INTO college.grades (student_id, grade)
VALUES (1, 5), (1, 4), (2, 4), (2, 5), (3, 3),
       (3, 4), (4, 5), (4, 5), (5, 2), (5, 3);
GO

CREATE VIEW college.student_statistics
AS
SELECT s.id AS student_id, s.name AS student_name,
    g.name AS group_name,
    COUNT(gr.id) AS grade_count,
    AVG(CAST(gr.grade AS DECIMAL(4,2))) AS average_grade,
    MIN(gr.grade) AS min_grade,
    MAX(gr.grade) AS max_grade
FROM college.students s
LEFT JOIN college.groups g ON g.id = s.group_id
LEFT JOIN college.grades gr ON gr.student_id = s.id
GROUP BY s.id, s.name, g.name;
GO

SELECT * FROM college.student_statistics;

SELECT * FROM college.student_statistics
WHERE average_grade >= 4.5;

SELECT * FROM college.student_statistics
ORDER BY average_grade DESC;

SELECT * FROM college.student_statistics
WHERE group_name = N'ИС-31';

SELECT * FROM college.student_statistics WHERE student_id = 1;
INSERT INTO college.grades (student_id, grade) VALUES (1, 5);
SELECT * FROM college.student_statistics WHERE student_id = 1;
