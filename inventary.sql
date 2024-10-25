Create Database InventorySystem;

use InventorySystem;

CREATE TABLE Users (
    UserID INT PRIMARY KEY IDENTITY(1,1),
    Username VARCHAR(50) NOT NULL,
    Password VARCHAR(50) NOT NULL
);

CREATE TABLE Products (
    ProductID INT PRIMARY KEY,
    ProductName VARCHAR(100),
    Category VARCHAR(50),
    Supplier VARCHAR(100),
    UnitPrice FLOAT,
    Quantity INT
);

CREATE TABLE Orders (
    OrderID INT PRIMARY KEY,
    ProductID INT,
    ProductName VARCHAR(100),
    UnitPrice DECIMAL(10, 2),
    Quantity INT,
    Total AS (UnitPrice * Quantity),
    FOREIGN KEY (ProductID) REFERENCES Products(ProductID)
);


INSERT INTO Products (ProductID, ProductName, Category, Supplier, UnitPrice, Quantity)
VALUES 
(1, 'Tea Pack', 'Beverages', 'Dilmah', 350.00, 50),
(2, 'Rice Bag', 'Grains', 'CIC', 1000.00, 30),
(3, 'Coconut Oil', 'Groceries', 'Prima', 700.00, 20),
(4, 'Milk Powder', 'Dairy', 'Anchor', 850.00, 40),
(5, 'Sugar Pack', 'Groceries', 'Pelwatte', 200.00, 100),
(6, 'Cheese Block', 'Dairy', 'Newdale', 1200.00, 15),
(7, 'Green Tea', 'Beverages', 'Mlesna', 500.00, 25),
(8, 'Wheat Flour', 'Grains', 'Prima', 900.00, 60),
(9, 'Yoghurt Cup', 'Dairy', 'Ambewela', 120.00, 200),
(10, 'Soft Drink', 'Beverages', 'Elephant House', 100.00, 150);

