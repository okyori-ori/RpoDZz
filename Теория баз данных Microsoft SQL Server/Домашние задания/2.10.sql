USE CollegeDB;
GO
CREATE FUNCTION college.GetScholarship
(
    @average_grade DECIMAL(4,2)
)
RETURNS INT
AS
BEGIN
    RETURN CASE
        WHEN @average_grade BETWEEN 4.8 AND 5.0 THEN 5000
        WHEN @average_grade >= 4.5 AND @average_grade < 4.8 THEN 4000
        WHEN @average_grade >= 4.0 AND @average_grade < 4.5 THEN 3000
        ELSE 0
    END;
END;
GO

SELECT name, average_grade,
       college.GetScholarship(average_grade) AS scholarship
FROM college.students;

CREATE FUNCTION college.GetStudentsByMinGrade
(
    @min_grade DECIMAL(4,2)
)
RETURNS TABLE
AS
RETURN
(
    SELECT id, name, age, average_grade, group_id
    FROM college.students
    WHERE average_grade >= @min_grade
);
GO

SELECT * FROM college.GetStudentsByMinGrade(4.5);

SELECT grade, college.GetScholarship(grade) AS scholarship
FROM (VALUES
    (CAST(3.99 AS DECIMAL(4,2))), (4.00), (4.49),
    (4.50), (4.79), (4.80), (5.00)
) t(grade);
