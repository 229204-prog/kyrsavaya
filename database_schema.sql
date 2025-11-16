-- =====================================
-- Скрипт створення бази даних
-- Система управління заявками сервісного центру
-- =====================================

-- Використовуйте цей скрипт для ручного створення бази даних
-- Або база створюється автоматично при запуску програми

USE master;
GO

-- Створення бази даних (якщо не існує)
IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'ServiceCenterDB')
BEGIN
    CREATE DATABASE ServiceCenterDB;
END
GO

USE ServiceCenterDB;
GO

-- =====================================
-- Створення таблиць
-- =====================================

-- Таблиця статусів заявок
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'RepairStatuses')
BEGIN
    CREATE TABLE RepairStatuses (
        StatusID INT PRIMARY KEY IDENTITY(1,1),
        StatusName NVARCHAR(50) NOT NULL,
        Description NVARCHAR(200)
    );
END
GO

-- Таблиця клієнтів
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Clients')
BEGIN
    CREATE TABLE Clients (
        ClientID INT PRIMARY KEY IDENTITY(1,1),
        FirstName NVARCHAR(50) NOT NULL,
        LastName NVARCHAR(50) NOT NULL,
        PhoneNumber NVARCHAR(20) NOT NULL,
        Email NVARCHAR(100),
        Address NVARCHAR(200)
    );
END
GO

-- Таблиця техніків
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Technicians')
BEGIN
    CREATE TABLE Technicians (
        TechnicianID INT PRIMARY KEY IDENTITY(1,1),
        FirstName NVARCHAR(50) NOT NULL,
        LastName NVARCHAR(50) NOT NULL,
        Specialization NVARCHAR(100),
        PhoneNumber NVARCHAR(20)
    );
END
GO

-- Таблиця типів пристроїв
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'DeviceTypes')
BEGIN
    CREATE TABLE DeviceTypes (
        DeviceTypeID INT PRIMARY KEY IDENTITY(1,1),
        TypeName NVARCHAR(50) NOT NULL,
        Description NVARCHAR(200)
    );
END
GO

-- Таблиця пристроїв
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Devices')
BEGIN
    CREATE TABLE Devices (
        DeviceID INT PRIMARY KEY IDENTITY(1,1),
        DeviceTypeID INT FOREIGN KEY REFERENCES DeviceTypes(DeviceTypeID),
        Brand NVARCHAR(50),
        Model NVARCHAR(100),
        SerialNumber NVARCHAR(100)
    );
END
GO

-- Таблиця заявок на ремонт
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'RepairRequests')
BEGIN
    CREATE TABLE RepairRequests (
        RequestID INT PRIMARY KEY IDENTITY(1,1),
        ClientID INT FOREIGN KEY REFERENCES Clients(ClientID),
        DeviceID INT FOREIGN KEY REFERENCES Devices(DeviceID),
        TechnicianID INT FOREIGN KEY REFERENCES Technicians(TechnicianID),
        StatusID INT FOREIGN KEY REFERENCES RepairStatuses(StatusID),
        ProblemDescription NVARCHAR(500) NOT NULL,
        DateReceived DATETIME NOT NULL DEFAULT GETDATE(),
        DateCompleted DATETIME,
        EstimatedCost DECIMAL(10,2),
        ActualCost DECIMAL(10,2),
        Notes NVARCHAR(500)
    );
END
GO

-- =====================================
-- Вставка тестових даних
-- =====================================

-- Вставка статусів
IF NOT EXISTS (SELECT * FROM RepairStatuses)
BEGIN
    INSERT INTO RepairStatuses (StatusName, Description) VALUES
        (N'Нова', N'Заявка тільки що створена'),
        (N'В роботі', N'Технік працює над ремонтом'),
        (N'Очікування деталей', N'Очікуємо на постачання деталей'),
        (N'Завершено', N'Ремонт завершено успішно'),
        (N'Видано клієнту', N'Пристрій повернуто клієнту'),
        (N'Скасовано', N'Заявка скасована');
END
GO

-- Вставка клієнтів
IF NOT EXISTS (SELECT * FROM Clients)
BEGIN
    INSERT INTO Clients (FirstName, LastName, PhoneNumber, Email, Address) VALUES
        (N'Іван', N'Петренко', '+380501234567', 'ivan.petrenko@email.com', N'вул. Шевченка, 15, Київ'),
        (N'Марія', N'Коваленко', '+380502345678', 'maria.kovalenko@email.com', N'вул. Лесі Українки, 23, Київ'),
        (N'Олександр', N'Сидоренко', '+380503456789', 'oleksandr.sydorenko@email.com', N'пр. Перемоги, 45, Київ'),
        (N'Наталія', N'Бондаренко', '+380504567890', 'natalia.bondarenko@email.com', N'вул. Хрещатик, 10, Київ'),
        (N'Андрій', N'Мельник', '+380505678901', 'andriy.melnyk@email.com', N'вул. Саксаганського, 67, Київ');
END
GO

-- Вставка техніків
IF NOT EXISTS (SELECT * FROM Technicians)
BEGIN
    INSERT INTO Technicians (FirstName, LastName, Specialization, PhoneNumber) VALUES
        (N'Петро', N'Іваненко', N'Ремонт смартфонів', '+380671234567'),
        (N'Сергій', N'Ткаченко', N'Ремонт ноутбуків', '+380672345678'),
        (N'Віктор', N'Морозов', N'Ремонт планшетів', '+380673456789'),
        (N'Дмитро', N'Павленко', N'Ремонт побутової техніки', '+380674567890');
END
GO

-- Вставка типів пристроїв
IF NOT EXISTS (SELECT * FROM DeviceTypes)
BEGIN
    INSERT INTO DeviceTypes (TypeName, Description) VALUES
        (N'Смартфон', N'Мобільні телефони та смартфони'),
        (N'Ноутбук', N'Портативні комп''ютери'),
        (N'Планшет', N'Планшетні комп''ютери'),
        (N'Комп''ютер', N'Настільні комп''ютери'),
        (N'Побутова техніка', N'Холодильники, пральні машини тощо');
END
GO

-- Вставка пристроїв
IF NOT EXISTS (SELECT * FROM Devices)
BEGIN
    INSERT INTO Devices (DeviceTypeID, Brand, Model, SerialNumber) VALUES
        (1, N'Samsung', N'Galaxy S21', 'SN001234567'),
        (1, N'Apple', N'iPhone 12', 'SN002345678'),
        (2, N'Dell', N'Inspiron 15', 'SN003456789'),
        (2, N'HP', N'Pavilion 14', 'SN004567890'),
        (3, N'Apple', N'iPad Air', 'SN005678901'),
        (4, N'Asus', N'ROG Desktop', 'SN006789012'),
        (5, N'LG', N'Холодильник GC-B247', 'SN007890123');
END
GO

-- Вставка заявок на ремонт
IF NOT EXISTS (SELECT * FROM RepairRequests)
BEGIN
    INSERT INTO RepairRequests (ClientID, DeviceID, TechnicianID, StatusID, ProblemDescription, DateReceived, EstimatedCost, Notes) VALUES
        (1, 1, 1, 2, N'Розбитий екран, потрібна заміна дисплею', DATEADD(day, -5, GETDATE()), 2500.00, N'Клієнт чекає на дзвінок'),
        (2, 3, 2, 3, N'Не включається, можлива проблема з материнською платою', DATEADD(day, -3, GETDATE()), 5000.00, N'Очікуємо постачання деталей'),
        (3, 2, 1, 1, N'Не працює камера', DATEADD(day, -1, GETDATE()), 1500.00, N'Нова заявка'),
        (4, 5, 3, 2, N'Тріснутий екран планшета', DATEADD(day, -7, GETDATE()), 3000.00, N'В процесі ремонту'),
        (5, 7, 4, 4, N'Холодильник не охолоджує', DATEADD(day, -10, GETDATE()), 4500.00, N'Ремонт завершено, очікує клієнта'),
        (1, 4, 2, 1, N'Перегрівається ноутбук', DATEADD(day, 0, GETDATE()), 1000.00, N'Діагностика');
END
GO

-- =====================================
-- Корисні запити
-- =====================================

-- Переглянути всі заявки з деталями
SELECT 
    rr.RequestID AS [№],
    c.FirstName + ' ' + c.LastName AS [Клієнт],
    d.Brand + ' ' + d.Model AS [Пристрій],
    rr.ProblemDescription AS [Проблема],
    t.FirstName + ' ' + t.LastName AS [Технік],
    rs.StatusName AS [Статус],
    rr.DateReceived AS [Дата прийому],
    rr.EstimatedCost AS [Вартість]
FROM RepairRequests rr
INNER JOIN Clients c ON rr.ClientID = c.ClientID
INNER JOIN Devices d ON rr.DeviceID = d.DeviceID
INNER JOIN Technicians t ON rr.TechnicianID = t.TechnicianID
INNER JOIN RepairStatuses rs ON rr.StatusID = rs.StatusID
ORDER BY rr.DateReceived DESC;
GO

-- Статистика по статусах
SELECT 
    rs.StatusName AS [Статус],
    COUNT(*) AS [Кількість заявок]
FROM RepairRequests rr
INNER JOIN RepairStatuses rs ON rr.StatusID = rs.StatusID
GROUP BY rs.StatusName
ORDER BY COUNT(*) DESC;
GO

-- Статистика по техніках
SELECT 
    t.FirstName + ' ' + t.LastName AS [Технік],
    t.Specialization AS [Спеціалізація],
    COUNT(rr.RequestID) AS [Кількість заявок]
FROM Technicians t
LEFT JOIN RepairRequests rr ON t.TechnicianID = rr.TechnicianID
GROUP BY t.FirstName, t.LastName, t.Specialization
ORDER BY COUNT(rr.RequestID) DESC;
GO

PRINT N'База даних успішно створена та заповнена тестовими даними!';
