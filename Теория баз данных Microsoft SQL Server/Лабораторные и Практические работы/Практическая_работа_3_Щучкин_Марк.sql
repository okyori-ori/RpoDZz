USE CollegeDB;
GO

IF NOT EXISTS (SELECT 1 FROM sys.schemas WHERE name = N'game')
    EXEC (N'CREATE SCHEMA game');
GO

DROP TABLE IF EXISTS game.locations;
GO

CREATE TABLE game.locations
(
    id            INT           PRIMARY KEY,
    name          NVARCHAR(100) NOT NULL,
    parent_id     INT           NULL,
    location_type NVARCHAR(50)  NOT NULL,
    difficulty    INT           NOT NULL,
    reward        INT           NOT NULL,
    CONSTRAINT FK_locations_parent
        FOREIGN KEY (parent_id) REFERENCES game.locations (id)
);
GO

INSERT INTO game.locations
    (id, name, parent_id, location_type, difficulty, reward)
VALUES
    (1,  N'Королевство',     NULL, N'Мир',        1,  0),

    (2,  N'Северные земли',  1,    N'Регион',     2,  0),
    (3,  N'Южные земли',     1,    N'Регион',     2,  0),

    (4,  N'Лес',             2,    N'Локация',    3,  100),
    (5,  N'Горная крепость', 2,    N'Локация',    5,  300),

    (6,  N'Древние руины',   4,    N'Подземелье', 6,  500),
    (7,  N'Логово волков',   4,    N'Подземелье', 4,  250),
    (8,  N'Пещера дракона',  5,    N'Подземелье', 10, 1000),

    (9,  N'Пустыня',         3,    N'Локация',    3,  100),
    (10, N'Город',           3,    N'Локация',    2,  50),

    (11, N'Храм песков',     9,    N'Подземелье', 7,  600),
    (12, N'Таверна',         10,   N'Здание',     1,  20),
    (13, N'Рынок',           10,   N'Здание',     1,  30);
GO

SELECT * FROM game.locations;
GO

WITH DangerousLocations AS
(
    SELECT
        id,
        name,
        difficulty,
        reward
    FROM game.locations
    WHERE difficulty >= 5
)
SELECT *
FROM DangerousLocations;
GO

WITH LocationStatistics AS
(
    SELECT
        parent_id,
        COUNT(*)                                              AS child_count,
        CAST(AVG(CAST(difficulty AS DECIMAL(5,2))) AS DECIMAL(4,2)) AS avg_difficulty,
        MAX(reward)                                           AS max_reward
    FROM game.locations
    WHERE parent_id IS NOT NULL
    GROUP BY parent_id
)
SELECT
    l.name             AS [Локация],
    s.child_count      AS [Количество дочерних],
    s.avg_difficulty   AS [Средняя сложность],
    s.max_reward       AS [Максимальная награда]
FROM game.locations AS l
JOIN LocationStatistics AS s ON s.parent_id = l.id
WHERE l.location_type = N'Локация'
ORDER BY l.id;
GO

WITH RegionTree AS
(

    SELECT
        id AS root_id,
        id,
        difficulty,
        0  AS [level]
    FROM game.locations
    WHERE location_type = N'Регион'

    UNION ALL

    SELECT
        t.root_id,
        c.id,
        c.difficulty,
        t.[level] + 1
    FROM game.locations AS c
    JOIN RegionTree     AS t ON c.parent_id = t.id
)
SELECT
    r.name              AS [Регион],
    r.difficulty        AS [Сложность региона],
    MAX(t.difficulty)   AS [Макс. сложность внутри]
FROM game.locations AS r
JOIN RegionTree     AS t ON t.root_id = r.id
WHERE t.[level] > 0
GROUP BY r.id, r.name, r.difficulty
HAVING MAX(t.difficulty) >= 7;
GO

WITH LocationTree AS
(

    SELECT id, name, parent_id
    FROM game.locations
    WHERE name = N'Северные земли'

    UNION ALL

    SELECT child.id, child.name, child.parent_id
    FROM game.locations AS child
    JOIN LocationTree   AS parent ON child.parent_id = parent.id
)
SELECT *
FROM LocationTree;
GO

WITH LocationTree AS
(
    SELECT id, name, parent_id, 0 AS [level]
    FROM game.locations
    WHERE name = N'Северные земли'

    UNION ALL

    SELECT child.id, child.name, child.parent_id, parent.[level] + 1
    FROM game.locations AS child
    JOIN LocationTree   AS parent ON child.parent_id = parent.id
)
SELECT
    name      AS [Локация],
    [level]   AS [Уровень]
FROM LocationTree
ORDER BY [level], id;
GO

WITH LocationTree AS
(
    SELECT id, name, parent_id, location_type, difficulty, reward,
           0 AS [level]
    FROM game.locations
    WHERE name = N'Северные земли'

    UNION ALL

    SELECT child.id, child.name, child.parent_id, child.location_type,
           child.difficulty, child.reward, parent.[level] + 1
    FROM game.locations AS child
    JOIN LocationTree   AS parent ON child.parent_id = parent.id
)
SELECT
    name          AS [Локация],
    [level]       AS [Уровень],
    location_type AS [Тип],
    difficulty    AS [Сложность],
    reward        AS [Награда]
FROM LocationTree
ORDER BY [level], id;
GO

WITH LocationTree AS
(
    SELECT id, name, parent_id, difficulty, reward
    FROM game.locations
    WHERE name = N'Северные земли'

    UNION ALL

    SELECT child.id, child.name, child.parent_id, child.difficulty, child.reward
    FROM game.locations AS child
    JOIN LocationTree   AS parent ON child.parent_id = parent.id
),
MaxReward AS
(
    SELECT MAX(reward) AS max_reward
    FROM LocationTree
)
SELECT
    t.name       AS [Название локации],
    t.reward     AS [Награда],
    t.difficulty AS [Сложность]
FROM LocationTree AS t
JOIN MaxReward    AS m ON t.reward = m.max_reward;
GO

WITH LocationTree AS
(
    SELECT
        id, name, parent_id,
        0 AS [level],
        CAST(RIGHT(N'0000' + CAST(id AS NVARCHAR(10)), 4) AS NVARCHAR(900)) AS sort_path
    FROM game.locations
    WHERE name = N'Северные земли'

    UNION ALL

    SELECT
        child.id, child.name, child.parent_id,
        parent.[level] + 1,
        CAST(parent.sort_path + N'/' + RIGHT(N'0000' + CAST(child.id AS NVARCHAR(10)), 4) AS NVARCHAR(900))
    FROM game.locations AS child
    JOIN LocationTree   AS parent ON child.parent_id = parent.id
)
SELECT
    REPLICATE(N'    ', [level]) + name AS display_name
FROM LocationTree
ORDER BY sort_path;
GO

WITH LocationTree AS
(

    SELECT
        id, name, parent_id, location_type, difficulty, reward,
        0 AS [level],
        CAST(RIGHT(N'0000' + CAST(id AS NVARCHAR(10)), 4) AS NVARCHAR(900)) AS sort_path
    FROM game.locations
    WHERE name = N'Королевство'

    UNION ALL

    SELECT
        child.id, child.name, child.parent_id, child.location_type,
        child.difficulty, child.reward,
        parent.[level] + 1,
        CAST(parent.sort_path + N'/' + RIGHT(N'0000' + CAST(child.id AS NVARCHAR(10)), 4) AS NVARCHAR(900))
    FROM game.locations AS child
    JOIN LocationTree   AS parent ON child.parent_id = parent.id
)
SELECT
    REPLICATE(N'    ', [level]) + name AS display_name,
    location_type                      AS [Тип],
    [level]                            AS [Уровень],
    difficulty                         AS [Сложность],
    reward                             AS [Награда]
FROM LocationTree
ORDER BY sort_path
OPTION (MAXRECURSION 100);
GO

WITH LocationTree AS
(
    SELECT
        id, name, parent_id, location_type, difficulty, reward,
        0 AS [level],
        CAST(RIGHT(N'0000' + CAST(id AS NVARCHAR(10)), 4) AS NVARCHAR(900)) AS sort_path
    FROM game.locations
    WHERE name = N'Королевство'

    UNION ALL

    SELECT
        child.id, child.name, child.parent_id, child.location_type,
        child.difficulty, child.reward,
        parent.[level] + 1,
        CAST(parent.sort_path + N'/' + RIGHT(N'0000' + CAST(child.id AS NVARCHAR(10)), 4) AS NVARCHAR(900))
    FROM game.locations AS child
    JOIN LocationTree   AS parent ON child.parent_id = parent.id
)
SELECT
    REPLICATE(N'    ', [level]) + name AS display_name,
    location_type                      AS [Тип],
    [level]                            AS [Уровень],
    difficulty                         AS [Сложность],
    reward                             AS [Награда],
    CASE
        WHEN difficulty <= 3 THEN N'Безопасная'
        WHEN difficulty <= 6 THEN N'Опасная'
        WHEN difficulty <= 9 THEN N'Очень опасная'
        ELSE                      N'Босс'
    END                                AS danger_level
FROM LocationTree
ORDER BY sort_path
OPTION (MAXRECURSION 100);
GO

WITH BranchTree AS
(

    SELECT
        id AS root_id,
        id,
        reward,
        0  AS [level]
    FROM game.locations

    UNION ALL

    SELECT
        b.root_id,
        c.id,
        c.reward,
        b.[level] + 1
    FROM game.locations AS c
    JOIN BranchTree     AS b ON c.parent_id = b.id
)
SELECT
    l.name AS [Локация],
    SUM(CASE WHEN b.[level] > 0 THEN b.reward ELSE 0 END) AS [Суммарная награда потомков]
FROM game.locations AS l
JOIN BranchTree     AS b ON b.root_id = l.id
GROUP BY l.id, l.name
ORDER BY l.id
OPTION (MAXRECURSION 100);
GO

WITH LocationTree AS
(
    SELECT id, name, reward, 0 AS [level]
    FROM game.locations
    WHERE name = N'Лес'

    UNION ALL

    SELECT child.id, child.name, child.reward, parent.[level] + 1
    FROM game.locations AS child
    JOIN LocationTree   AS parent ON child.parent_id = parent.id
)
SELECT name AS [Локация], reward AS [Награда]
FROM LocationTree
WHERE [level] > 0
UNION ALL
SELECT N'Итого:', SUM(reward)
FROM LocationTree
WHERE [level] > 0;
GO
