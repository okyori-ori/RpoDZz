USE CollegeDB;
GO
ALTER TABLE college.students ADD group_id INT;
UPDATE college.students
SET group_id = CASE WHEN id IN (1, 2, 3) THEN 1 ELSE 2 END
WHERE id BETWEEN 1 AND 5;

ALTER TABLE college.students
ADD CONSTRAINT FK_students_groups
FOREIGN KEY (group_id) REFERENCES college.groups(id);
GO

SELECT s.name, g.name AS group_name, s.average_grade,
    RANK() OVER (
        PARTITION BY s.group_id
        ORDER BY s.average_grade DESC
    ) AS student_rank,
    AVG(s.average_grade) OVER (
        PARTITION BY s.group_id
    ) AS group_average,
    s.average_grade - AVG(s.average_grade) OVER (
        PARTITION BY s.group_id
    ) AS deviation
FROM college.students s
JOIN college.groups g ON g.id = s.group_id
ORDER BY s.group_id, student_rank, s.id;

;WITH StudentRating AS
(
    SELECT s.id, s.name, g.name AS group_name,
        s.group_id, s.average_grade,
        ROW_NUMBER() OVER (
            PARTITION BY s.group_id
            ORDER BY s.average_grade DESC, s.id
        ) AS row_num
    FROM college.students s
    JOIN college.groups g ON g.id = s.group_id
)
SELECT name, group_name, average_grade, row_num
FROM StudentRating
WHERE row_num <= 3
ORDER BY group_id, row_num;

;WITH TestGrades AS
(
    SELECT * FROM (VALUES
        (1, CAST(5.0 AS DECIMAL(4,2))),
        (2, CAST(4.5 AS DECIMAL(4,2))),
        (3, CAST(4.5 AS DECIMAL(4,2))),
        (4, CAST(4.0 AS DECIMAL(4,2)))
    ) t(id, grade)
)
SELECT id, grade,
    ROW_NUMBER() OVER (ORDER BY grade DESC, id) AS row_num,
    RANK() OVER (ORDER BY grade DESC) AS rank_num,
    DENSE_RANK() OVER (ORDER BY grade DESC) AS dense_rank_num
FROM TestGrades
ORDER BY grade DESC, id;
