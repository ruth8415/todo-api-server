@echo off
"C:\Program Files\MySQL\MySQL Server 8.0\bin\mysql.exe" -u ruth -pr1u2t3i4w -e "CREATE DATABASE IF NOT EXISTS ToDoDB;"
"C:\Program Files\MySQL\MySQL Server 8.0\bin\mysql.exe" -u ruth -pr1u2t3i4w ToDoDB -e "CREATE TABLE IF NOT EXISTS Items (Id INT NOT NULL AUTO_INCREMENT PRIMARY KEY, Name VARCHAR(100), IsComplete TINYINT(1));"
"C:\Program Files\MySQL\MySQL Server 8.0\bin\mysql.exe" -u ruth -pr1u2t3i4w ToDoDB -e "INSERT INTO Items (Name, IsComplete) VALUES ('משימה לדוגמה 1', 0), ('משימה לדוגמה 2', 1);"
echo Database created successfully!
