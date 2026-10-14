USE CollegeDB;
GO
CREATE PROCEDURE college.GetStudentStatistics
    @student_id INT
AS
BEGIN
    SET NOCOUNT ON;
    IF NOT EXISTS (
        SELECT 1 FROM college.students WHERE id = @student_id
    )
    BEGIN
        THROW 50001, N'Студент не найден', 1;
    END;

    SELECT s.name AS student_name, g.name AS group_name,
        COUNT(gr.id) AS grade_count,
        AVG(CAST(gr.grade AS DECIMAL(4,2))) AS average_grade,
        MIN(gr.grade) AS min_grade,
        MAX(gr.grade) AS max_grade
    FROM college.students s
    LEFT JOIN college.groups g ON g.id = s.group_id
    LEFT JOIN college.grades gr ON gr.student_id = s.id
    WHERE s.id = @student_id
    GROUP BY s.id, s.name, g.name;
END;
GO

EXEC college.GetStudentStatistics @student_id = 1;

BEGIN TRY
    EXEC college.GetStudentStatistics @student_id = -1;
END TRY
BEGIN CATCH
    SELECT ERROR_NUMBER() AS error_number,
           ERROR_MESSAGE() AS error_message;
END CATCH;
