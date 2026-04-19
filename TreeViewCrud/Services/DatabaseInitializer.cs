using Microsoft.Data.SqlClient;

namespace TreeViewCrud.Services;
public static class DatabaseInitializer
{
    private static readonly string TargetConnectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=\"F:\\Вшэ\\2 курс\\КПО\\TreeViewCrud\\TreeViewCrud\\PharmacyDB.mdf\";Integrated Security=True";
    public static void EnsureDatabaseCreated()
    {
        CreateTables();
    }

    private static void CreateTables()
    {
        string sqlScript = @"
                        -- 1. Таблица категорий
                        IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Category')
                        BEGIN
                            CREATE TABLE Category (
                                Id INT IDENTITY(1,1) PRIMARY KEY,
                                Name NVARCHAR(255) NOT NULL UNIQUE,
                                Description NVARCHAR(MAX) NULL
                            );
                        END

                        -- 2. Таблица товаров
                        IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Item')
                        BEGIN
                            CREATE TABLE Item (
                                Id INT IDENTITY(1,1) PRIMARY KEY,
                                Name NVARCHAR(255) NOT NULL,
                                MnfName NVARCHAR(255) NULL,
                                Dosage NVARCHAR(50) NULL,
                                Form NVARCHAR(50) NULL,
                                PrescriptionRequired BIT NOT NULL DEFAULT 0,
                                CategoryId INT NOT NULL,
                                CONSTRAINT FK_Item_Category FOREIGN KEY (CategoryId) REFERENCES Category(Id) ON DELETE CASCADE
                            );
                        END

                        -- 3. Таблица партий
                        IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Batch')
                        BEGIN
                            CREATE TABLE Batch (
                                Id INT IDENTITY(1,1) PRIMARY KEY,
                                SerialNumber NVARCHAR(100) NOT NULL,
                                ProductionDate DATE NOT NULL,
                                ExpiryDate DATE NULL,
                                PurchasePrice DECIMAL(10,2) NOT NULL,
                                SellingPrice DECIMAL(10,2) NOT NULL,
                                Quantity INT NOT NULL CHECK (Quantity >= 0),
                                ItemId INT NOT NULL,
                                CONSTRAINT FK_Batch_Item FOREIGN KEY (ItemId) REFERENCES Item(Id) ON DELETE CASCADE,
                                CONSTRAINT CHK_Batch_Prices CHECK (SellingPrice >= PurchasePrice)
                            );
                        END

                        -- 4. Таблица пользователей (теперь AppUser)
                        IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'AppUser')
                        BEGIN
                            CREATE TABLE AppUser (
                                Id INT IDENTITY(1,1) PRIMARY KEY,
                                Login NVARCHAR(100) NOT NULL UNIQUE,
                                PasswordHash NVARCHAR(255) NOT NULL,
                                Salt NVARCHAR(255) NOT NULL,
                                IsActive BIT NOT NULL DEFAULT 1,
                                RegistrationDate DATETIME NOT NULL DEFAULT GETDATE()
                            );
                        END

                        -- 5. Таблица аудита
                        IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'AuditLog')
                        BEGIN
                            CREATE TABLE AuditLog (
                                Id INT IDENTITY(1,1) PRIMARY KEY,
                                Action NVARCHAR(50) NOT NULL,
                                TableName NVARCHAR(50) NOT NULL,
                                RecordId INT NULL,
                                OldValue NVARCHAR(MAX) NULL,
                                NewValue NVARCHAR(MAX) NULL,
                                Timestamp DATETIME NOT NULL DEFAULT GETDATE(),
                                UserId INT NULL,
                                CONSTRAINT FK_AuditLog_AppUser FOREIGN KEY (UserId) REFERENCES AppUser(Id)
                            );
                        END

                        -- Индексы
                        IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Item_CategoryId' AND object_id = OBJECT_ID('Item'))
                            CREATE INDEX IX_Item_CategoryId ON Item(CategoryId);

                        IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Batch_ItemId' AND object_id = OBJECT_ID('Batch'))
                            CREATE INDEX IX_Batch_ItemId ON Batch(ItemId);

                        IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_AppUser_Login' AND object_id = OBJECT_ID('AppUser'))
                            CREATE INDEX IX_AppUser_Login ON AppUser(Login);

                        IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_AuditLog_Timestamp' AND object_id = OBJECT_ID('AuditLog'))
                            CREATE INDEX IX_AuditLog_Timestamp ON AuditLog(Timestamp);
                ";
        using (var connection = new SqlConnection(TargetConnectionString))
        {
            connection.Open();
            // Разделяем скрипт на отдельные команды (по GO или по точке с запятой)
            // Проще выполнить весь скрипт целиком, но SqlCommand не поддерживает GO.
            // Поэтому либо убираем GO, либо разбиваем.
            // В нашем скрипте нет GO, поэтому выполним всё как одну команду:
            using (var cmd = new SqlCommand(sqlScript, connection))
            {
                cmd.ExecuteNonQuery();
            }
        }
    }
}