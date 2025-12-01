-- יצירת מסד נתונים ToDoDB
CREATE DATABASE IF NOT EXISTS ToDoDB;

-- שימוש במסד הנתונים
USE ToDoDB;

-- יצירת טבלת Items
CREATE TABLE IF NOT EXISTS Items (
    Id INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
    Name VARCHAR(100),
    IsComplete TINYINT(1)
);

-- הוספת נתונים לדוגמה (אופציונלי)
INSERT INTO Items (Name, IsComplete) VALUES 
('משימה לדוגמה 1', 0),
('מכשימה לדוגמה 2', 1);
