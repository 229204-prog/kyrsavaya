# Структура проекту

## Курсовий проект: Система управління заявками сервісного центру

---

## Огляд проекту

**Назва**: Система управління заявками у сервісному центрі з ремонту техніки  
**Тип**: Windows Forms Application  
**Платформа**: .NET Framework 4.7.2  
**Мова**: C#  
**База даних**: SQL Server LocalDB

---

## Структура файлів та папок

```
kyrsavaya/
│
├── .gitignore                          # Git ignore файл для виключення build artifacts
├── README.md                           # Основна документація проекту
├── USER_GUIDE.md                       # Посібник користувача (українською)
├── FORMS_DOCUMENTATION.md              # Документація форм та UI
├── PROJECT_STRUCTURE.md                # Цей файл - структура проекту
├── database_schema.sql                 # SQL скрипт для створення БД вручну
├── WindowsFormsApp2.sln                # Solution файл Visual Studio
│
└── WindowsFormsApp2/                   # Головна папка проекту
    │
    ├── App.config                      # Конфігураційний файл програми
    ├── Program.cs                      # Точка входу програми
    ├── WindowsFormsApp2.csproj         # Файл проекту
    │
    ├── Form1.cs                        # Головна форма (код)
    ├── Form1.Designer.cs               # Головна форма (designer)
    ├── Form1.resx                      # Головна форма (resources)
    │
    ├── Database/                       # Папка роботи з базою даних
    │   ├── DatabaseHelper.cs           # Ініціалізація та створення БД
    │   └── DataAccess.cs               # Методи доступу до даних (CRUD)
    │
    ├── Models/                         # Папка моделей даних
    │   └── Models.cs                   # Класи моделей (entities)
    │
    ├── Forms/                          # Папка додаткових форм
    │   ├── AddRequestForm.cs           # Форма додавання заявки (код)
    │   ├── AddRequestForm.Designer.cs  # Форма додавання заявки (designer)
    │   └── AddRequestForm.resx         # Форма додавання заявки (resources)
    │
    └── Properties/                     # Папка властивостей проекту
        ├── AssemblyInfo.cs             # Інформація про збірку
        ├── Resources.resx              # Ресурси проекту
        ├── Resources.Designer.cs       # Designer для ресурсів
        ├── Settings.settings           # Налаштування проекту
        └── Settings.Designer.cs        # Designer для налаштувань
```

---

## Опис основних компонентів

### 1. Точка входу

**Program.cs**
- Головний метод `Main()`
- Ініціалізація Windows Forms
- Запуск головної форми

### 2. Головна форма (Form1)

**Файли**: `Form1.cs`, `Form1.Designer.cs`, `Form1.resx`

**Функції:**
- Відображення списку заявок у DataGridView
- Статистика по заявках
- Кнопка додавання нової заявки
- Кнопка оновлення даних
- Ініціалізація бази даних при першому запуску

**Елементи UI:**
- `dataGridViewRequests` - таблиця для відображення заявок
- `btnRefresh` - кнопка оновлення
- `btnAddRequest` - кнопка додавання заявки
- `lblTitle` - заголовок програми
- `lblStats` - статистика
- `panel1` - верхня панель з кольоровим фоном

### 3. Форма додавання заявки (AddRequestForm)

**Файли**: `AddRequestForm.cs`, `AddRequestForm.Designer.cs`, `AddRequestForm.resx`

**Функції:**
- Вибір клієнта, пристрою, техніка
- Встановлення статусу
- Введення опису проблеми
- Встановлення дати та вартості
- Валідація введених даних
- Збереження нової заявки в БД

**Елементи UI:**
- `cmbClient` - ComboBox для вибору клієнта
- `cmbDevice` - ComboBox для вибору пристрою
- `cmbTechnician` - ComboBox для вибору техніка
- `cmbStatus` - ComboBox для вибору статусу
- `txtProblem` - TextBox для опису проблеми
- `dtpDateReceived` - DateTimePicker для дати
- `txtEstimatedCost` - TextBox для вартості
- `txtNotes` - TextBox для приміток
- `btnSave` - кнопка збереження
- `btnCancel` - кнопка скасування

### 4. Шар бази даних (Database/)

#### DatabaseHelper.cs

**Функції:**
- Створення бази даних при першому запуску
- Створення таблиць з правильними зв'язками
- Вставка тестових даних
- Надання connection string для інших класів

**Методи:**
- `InitializeDatabase()` - головний метод ініціалізації
- `CreateDatabase()` - створення файлу БД
- `CreateTables()` - створення структури таблиць
- `InsertTestData()` - вставка тестових даних
- `TestConnection()` - перевірка підключення

#### DataAccess.cs

**Функції:**
- CRUD операції з базою даних
- Отримання даних для форм
- Збереження нових записів

**Методи:**
- `GetAllRepairRequests()` - отримання всіх заявок
- `GetAllClients()` - отримання всіх клієнтів
- `GetAllTechnicians()` - отримання всіх техніків
- `GetAllDevices()` - отримання всіх пристроїв
- `GetAllRepairStatuses()` - отримання всіх статусів
- `AddRepairRequest()` - додавання нової заявки
- `UpdateRepairRequestStatus()` - оновлення статусу заявки

### 5. Моделі даних (Models/)

#### Models.cs

**Класи:**
- `RepairRequest` - заявка на ремонт
- `Client` - клієнт
- `Technician` - технік
- `Device` - пристрій
- `DeviceType` - тип пристрою
- `RepairStatus` - статус заявки

**Властивості:**
- Основні поля з БД
- Navigation properties для зв'язаних даних
- Обчислювані властивості (FullName, FullDescription)

---

## База даних

### Таблиці (6 штук)

1. **RepairStatuses** - статуси заявок
   - StatusID (PK, INT, IDENTITY)
   - StatusName (NVARCHAR(50))
   - Description (NVARCHAR(200))

2. **Clients** - клієнти
   - ClientID (PK, INT, IDENTITY)
   - FirstName, LastName (NVARCHAR(50))
   - PhoneNumber (NVARCHAR(20))
   - Email (NVARCHAR(100))
   - Address (NVARCHAR(200))

3. **Technicians** - техніки
   - TechnicianID (PK, INT, IDENTITY)
   - FirstName, LastName (NVARCHAR(50))
   - Specialization (NVARCHAR(100))
   - PhoneNumber (NVARCHAR(20))

4. **DeviceTypes** - типи пристроїв
   - DeviceTypeID (PK, INT, IDENTITY)
   - TypeName (NVARCHAR(50))
   - Description (NVARCHAR(200))

5. **Devices** - пристрої
   - DeviceID (PK, INT, IDENTITY)
   - DeviceTypeID (FK)
   - Brand (NVARCHAR(50))
   - Model (NVARCHAR(100))
   - SerialNumber (NVARCHAR(100))

6. **RepairRequests** - заявки на ремонт
   - RequestID (PK, INT, IDENTITY)
   - ClientID (FK)
   - DeviceID (FK)
   - TechnicianID (FK)
   - StatusID (FK)
   - ProblemDescription (NVARCHAR(500))
   - DateReceived (DATETIME)
   - DateCompleted (DATETIME)
   - EstimatedCost (DECIMAL(10,2))
   - ActualCost (DECIMAL(10,2))
   - Notes (NVARCHAR(500))

### Зв'язки

```
RepairStatuses (1) ──< (M) RepairRequests
Clients (1)        ──< (M) RepairRequests
Devices (1)        ──< (M) RepairRequests
Technicians (1)    ──< (M) RepairRequests
DeviceTypes (1)    ──< (M) Devices
```

---

## Тестові дані

### Статистика тестових даних:
- **Статуси**: 6 записів
- **Клієнти**: 5 записів
- **Техніки**: 4 записи
- **Типи пристроїв**: 5 записів
- **Пристрої**: 7 записів
- **Заявки**: 6 записів

---

## Технології та підходи

### Використані технології:
- **C# 7.0+**
- **Windows Forms**
- **ADO.NET** для роботи з БД
- **SQL Server LocalDB**

### Паттерни та практики:
- **Separation of Concerns** - розділення логіки на шари
- **Data Access Layer** - окремий шар для роботи з БД
- **Model-View** - використання моделей даних
- **Validation** - перевірка введених даних

### Кольорова схема (Flat UI):
- Синій фон панелі: `RGB(41, 128, 185)`
- Зелена кнопка: `RGB(46, 204, 113)`
- Синя кнопка: `RGB(52, 152, 219)`
- Червона кнопка: `RGB(231, 76, 60)`
- Світлий фон: `RGB(236, 240, 241)`

---

## Як працює програма

### Послідовність запуску:

1. **Program.cs** → запускає `Form1`
2. **Form1_Load** → викликає `DatabaseHelper.InitializeDatabase()`
3. **DatabaseHelper** → створює БД (якщо не існує) та таблиці
4. **DatabaseHelper** → вставляє тестові дані
5. **Form1** → завантажує дані через `DataAccess`
6. **Form1** → відображає дані в DataGridView

### Додавання заявки:

1. Користувач натискає "Додати заявку"
2. Відкривається `AddRequestForm`
3. Форма завантажує списки через `DataAccess`
4. Користувач заповнює дані
5. При збереженні викликається `DataAccess.AddRepairRequest()`
6. Дані зберігаються в БД
7. Форма закривається
8. `Form1` оновлює список заявок

---

## Можливості розширення

Програму можна розширити додаванням:

1. **Нові форми:**
   - Редагування заявки
   - Управління клієнтами
   - Управління техніками
   - Управління пристроями
   - Звіти та статистика

2. **Нові функції:**
   - Пошук та фільтрація заявок
   - Експорт даних в Excel/PDF
   - Друк заявок
   - SMS/Email повідомлення
   - Облік запчастин

3. **Покращення:**
   - Авторизація користувачів
   - Різні ролі (адмін, оператор, технік)
   - Історія змін
   - Резервне копіювання
   - Багатомовний інтерфейс

---

## Вимоги до розробки

### Для компіляції потрібно:
- Visual Studio 2017 або новіша
- .NET Framework 4.7.2 SDK
- SQL Server Data Tools

### Для запуску потрібно:
- Windows 7 SP1 або новіша
- .NET Framework 4.7.2 Runtime
- SQL Server LocalDB

---

## Ліцензія та автори

**Тип проекту**: Курсовий проект  
**Навчальний заклад**: [Назва закладу]  
**Рік**: 2024

---

## Додаткові ресурси

- **README.md** - загальна інформація про проект
- **USER_GUIDE.md** - детальний посібник користувача
- **FORMS_DOCUMENTATION.md** - опис форм та елементів UI
- **database_schema.sql** - SQL скрипт для створення БД

---

**Версія документа**: 1.0  
**Останнє оновлення**: Листопад 2024
