using System;
using System.Data.SqlClient;
using System.IO;

namespace WindowsFormsApp2.Database
{
    public class DatabaseHelper
    {
        private static string dbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ServiceCenterDB.mdf");
        private static string connectionString = $@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename={dbPath};Integrated Security=True;Connect Timeout=30";

        public static string ConnectionString => connectionString;

        public static void InitializeDatabase()
        {
            if (!File.Exists(dbPath))
            {
                CreateDatabase();
                CreateTables();
                InsertTestData();
            }
        }

        private static void CreateDatabase()
        {
            string masterConnectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;Integrated Security=True;Connect Timeout=30";
            
            using (SqlConnection connection = new SqlConnection(masterConnectionString))
            {
                connection.Open();
                string createDbQuery = $@"CREATE DATABASE ServiceCenterDB ON PRIMARY 
                    (NAME = ServiceCenterDB_Data, FILENAME = '{dbPath}')";
                
                using (SqlCommand command = new SqlCommand(createDbQuery, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        private static void CreateTables()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                
                string createTablesQuery = @"
                    -- Таблиця статусів заявок
                    CREATE TABLE RepairStatuses (
                        StatusID INT PRIMARY KEY IDENTITY(1,1),
                        StatusName NVARCHAR(50) NOT NULL,
                        Description NVARCHAR(200)
                    );

                    -- Таблиця клієнтів
                    CREATE TABLE Clients (
                        ClientID INT PRIMARY KEY IDENTITY(1,1),
                        FirstName NVARCHAR(50) NOT NULL,
                        LastName NVARCHAR(50) NOT NULL,
                        PhoneNumber NVARCHAR(20) NOT NULL,
                        Email NVARCHAR(100),
                        Address NVARCHAR(200)
                    );

                    -- Таблиця техніків
                    CREATE TABLE Technicians (
                        TechnicianID INT PRIMARY KEY IDENTITY(1,1),
                        FirstName NVARCHAR(50) NOT NULL,
                        LastName NVARCHAR(50) NOT NULL,
                        Specialization NVARCHAR(100),
                        PhoneNumber NVARCHAR(20)
                    );

                    -- Таблиця типів пристроїв
                    CREATE TABLE DeviceTypes (
                        DeviceTypeID INT PRIMARY KEY IDENTITY(1,1),
                        TypeName NVARCHAR(50) NOT NULL,
                        Description NVARCHAR(200)
                    );

                    -- Таблиця пристроїв
                    CREATE TABLE Devices (
                        DeviceID INT PRIMARY KEY IDENTITY(1,1),
                        DeviceTypeID INT FOREIGN KEY REFERENCES DeviceTypes(DeviceTypeID),
                        Brand NVARCHAR(50),
                        Model NVARCHAR(100),
                        SerialNumber NVARCHAR(100)
                    );

                    -- Таблиця заявок на ремонт
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
                ";

                using (SqlCommand command = new SqlCommand(createTablesQuery, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        private static void InsertTestData()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                
                string insertDataQuery = @"
                    -- Вставка статусів
                    INSERT INTO RepairStatuses (StatusName, Description) VALUES
                        (N'Нова', N'Заявка тільки що створена'),
                        (N'В роботі', N'Технік працює над ремонтом'),
                        (N'Очікування деталей', N'Очікуємо на постачання деталей'),
                        (N'Завершено', N'Ремонт завершено успішно'),
                        (N'Видано клієнту', N'Пристрій повернуто клієнту'),
                        (N'Скасовано', N'Заявка скасована');

                    -- Вставка клієнтів
                    INSERT INTO Clients (FirstName, LastName, PhoneNumber, Email, Address) VALUES
                        (N'Іван', N'Петренко', '+380501234567', 'ivan.petrenko@email.com', N'вул. Шевченка, 15, Київ'),
                        (N'Марія', N'Коваленко', '+380502345678', 'maria.kovalenko@email.com', N'вул. Лесі Українки, 23, Київ'),
                        (N'Олександр', N'Сидоренко', '+380503456789', 'oleksandr.sydorenko@email.com', N'пр. Перемоги, 45, Київ'),
                        (N'Наталія', N'Бондаренко', '+380504567890', 'natalia.bondarenko@email.com', N'вул. Хрещатик, 10, Київ'),
                        (N'Андрій', N'Мельник', '+380505678901', 'andriy.melnyk@email.com', N'вул. Саксаганського, 67, Київ');

                    -- Вставка техніків
                    INSERT INTO Technicians (FirstName, LastName, Specialization, PhoneNumber) VALUES
                        (N'Петро', N'Іваненко', N'Ремонт смартфонів', '+380671234567'),
                        (N'Сергій', N'Ткаченко', N'Ремонт ноутбуків', '+380672345678'),
                        (N'Віктор', N'Морозов', N'Ремонт планшетів', '+380673456789'),
                        (N'Дмитро', N'Павленко', N'Ремонт побутової техніки', '+380674567890');

                    -- Вставка типів пристроїв
                    INSERT INTO DeviceTypes (TypeName, Description) VALUES
                        (N'Смартфон', N'Мобільні телефони та смартфони'),
                        (N'Ноутбук', N'Портативні комп''ютери'),
                        (N'Планшет', N'Планшетні комп''ютери'),
                        (N'Комп''ютер', N'Настільні комп''ютери'),
                        (N'Побутова техніка', N'Холодильники, пральні машини тощо');

                    -- Вставка пристроїв
                    INSERT INTO Devices (DeviceTypeID, Brand, Model, SerialNumber) VALUES
                        (1, N'Samsung', N'Galaxy S21', 'SN001234567'),
                        (1, N'Apple', N'iPhone 12', 'SN002345678'),
                        (2, N'Dell', N'Inspiron 15', 'SN003456789'),
                        (2, N'HP', N'Pavilion 14', 'SN004567890'),
                        (3, N'Apple', N'iPad Air', 'SN005678901'),
                        (4, N'Asus', N'ROG Desktop', 'SN006789012'),
                        (5, N'LG', N'Холодильник GC-B247', 'SN007890123');

                    -- Вставка заявок на ремонт
                    INSERT INTO RepairRequests (ClientID, DeviceID, TechnicianID, StatusID, ProblemDescription, DateReceived, EstimatedCost, Notes) VALUES
                        (1, 1, 1, 2, N'Розбитий екран, потрібна заміна дисплею', DATEADD(day, -5, GETDATE()), 2500.00, N'Клієнт чекає на дзвінок'),
                        (2, 3, 2, 3, N'Не включається, можлива проблема з материнською платою', DATEADD(day, -3, GETDATE()), 5000.00, N'Очікуємо постачання деталей'),
                        (3, 2, 1, 1, N'Не працює камера', DATEADD(day, -1, GETDATE()), 1500.00, N'Нова заявка'),
                        (4, 5, 3, 2, N'Тріснутий екран планшета', DATEADD(day, -7, GETDATE()), 3000.00, N'В процесі ремонту'),
                        (5, 7, 4, 4, N'Холодильник не охолоджує', DATEADD(day, -10, GETDATE()), 4500.00, N'Ремонт завершено, очікує клієнта'),
                        (1, 4, 2, 1, N'Перегрівається ноутбук', DATEADD(day, 0, GETDATE()), 1000.00, N'Діагностика');
                ";

                using (SqlCommand command = new SqlCommand(insertDataQuery, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        public static bool TestConnection()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }
    }
}
