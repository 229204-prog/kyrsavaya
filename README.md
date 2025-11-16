# Система управління заявками сервісного центру

Курсовий проект Windows Forms App (.NET Framework 4.7.2) на мові C# за темою: Проектування системи управління заявками у сервісному центрі з ремонту техніки.

## Опис проекту

Ця система призначена для управління заявками на ремонт техніки в сервісному центрі. Програма дозволяє:
- Переглядати всі заявки на ремонт
- Додавати нові заявки
- Відстежувати статус виконання ремонтів
- Керувати інформацією про клієнтів, техніків та пристрої

## Структура бази даних

Система використовує SQL Server LocalDB з наступними таблицями:

### RepairStatuses (Статуси заявок)
- StatusID (INT, Primary Key)
- StatusName (NVARCHAR(50))
- Description (NVARCHAR(200))

### Clients (Клієнти)
- ClientID (INT, Primary Key)
- FirstName (NVARCHAR(50))
- LastName (NVARCHAR(50))
- PhoneNumber (NVARCHAR(20))
- Email (NVARCHAR(100))
- Address (NVARCHAR(200))

### Technicians (Техніки)
- TechnicianID (INT, Primary Key)
- FirstName (NVARCHAR(50))
- LastName (NVARCHAR(50))
- Specialization (NVARCHAR(100))
- PhoneNumber (NVARCHAR(20))

### DeviceTypes (Типи пристроїв)
- DeviceTypeID (INT, Primary Key)
- TypeName (NVARCHAR(50))
- Description (NVARCHAR(200))

### Devices (Пристрої)
- DeviceID (INT, Primary Key)
- DeviceTypeID (INT, Foreign Key)
- Brand (NVARCHAR(50))
- Model (NVARCHAR(100))
- SerialNumber (NVARCHAR(100))

### RepairRequests (Заявки на ремонт)
- RequestID (INT, Primary Key)
- ClientID (INT, Foreign Key)
- DeviceID (INT, Foreign Key)
- TechnicianID (INT, Foreign Key)
- StatusID (INT, Foreign Key)
- ProblemDescription (NVARCHAR(500))
- DateReceived (DATETIME)
- DateCompleted (DATETIME)
- EstimatedCost (DECIMAL(10,2))
- ActualCost (DECIMAL(10,2))
- Notes (NVARCHAR(500))

## Функціональність

### Головна форма (Form1)
- Відображення всіх заявок на ремонт у вигляді таблиці
- Статистика по заявках
- Кнопка для оновлення даних
- Кнопка для додавання нової заявки

### Форма додавання заявки (AddRequestForm)
- Вибір клієнта зі списку
- Вибір пристрою зі списку
- Призначення техніка
- Встановлення статусу
- Введення опису проблеми
- Встановлення дати прийому
- Вказання орієнтовної вартості
- Додавання приміток

## Технології

- **Платформа**: .NET Framework 4.7.2
- **Мова програмування**: C#
- **UI Framework**: Windows Forms
- **База даних**: SQL Server LocalDB
- **Data Access**: ADO.NET

## Встановлення та запуск

1. Відкрийте `WindowsFormsApp2.sln` у Visual Studio 2017 або новішій версії
2. Переконайтеся, що встановлено SQL Server LocalDB
3. Скомпілюйте та запустіть проект (F5)
4. При першому запуску автоматично створюється база даних з тестовими даними

## Тестові дані

При першому запуску в базу даних автоматично додаються тестові дані:
- 6 статусів заявок
- 5 клієнтів
- 4 техніки
- 5 типів пристроїв
- 7 пристроїв
- 6 заявок на ремонт

## Структура проекту

```
WindowsFormsApp2/
├── Database/
│   ├── DatabaseHelper.cs      # Ініціалізація БД та створення таблиць
│   └── DataAccess.cs          # Методи доступу до даних
├── Models/
│   └── Models.cs              # Класи моделей даних
├── Forms/
│   ├── AddRequestForm.cs      # Форма додавання заявки
│   └── AddRequestForm.Designer.cs
├── Form1.cs                   # Головна форма
├── Form1.Designer.cs
└── Program.cs                 # Точка входу програми
```

## Автор

Курсовий проект
