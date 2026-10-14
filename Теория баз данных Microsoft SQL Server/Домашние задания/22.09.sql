USE CollegeDB;
GO
;WITH AdultStudents AS
(
    SELECT id, name, age
    FROM college.students
    WHERE age >= 18
)
SELECT * FROM AdultStudents;

;WITH Numbers AS
(
    SELECT 1 AS number
    UNION ALL
    SELECT number + 1
    FROM Numbers
    WHERE number < 20
)
SELECT number FROM Numbers
ORDER BY number
OPTION (MAXRECURSION 20);
