--Drops for resetting
DROP TABLE Pizza;
DROP TABLE PizzaOrder;
DROP TABLE PizzaUser;
DROP TABLE Store;

--Creations
Create TABLE Store(
ID int Primary key check(ID>99 AND ID<1000),
Income DECIMAL(10,2) DEFAULT 0,
NextOrder INT DEFAULT 1,
Dough DECIMAL(7,3) DEFAULT 0,
Sauce DECIMAL(7,3) DEFAULT 0, 
Cheese DECIMAL(7,3) DEFAULT 0, 
Pepperoni DECIMAL(7,3) DEFAULT 0, 
Sausage DECIMAL(7,3) DEFAULT 0,
Ham DECIMAL(7,3) DEFAULT 0,
Bacon DECIMAL(7,3) DEFAULT 0, 
Beef DECIMAL(7,3) DEFAULT 0, 
Onion DECIMAL(7,3) DEFAULT 0,
Green_Pepper DECIMAL(7,3) DEFAULT 0,
Mushroom DECIMAL(7,3) DEFAULT 0,
Black_Olive DECIMAL(7,3) DEFAULT 0
);

CREATE TABLE PizzaUser (
Username NVARCHAR(255) primary key,
StoreID int not null
);

CREATE TABLE PizzaOrder(
ID DECIMAL(10,3) primary key,
StoreID int not null,
Username NVARCHAR(255) not null,
OrderTime DateTime2 not null,
Total Decimal(10,2) not null,
Foreign Key (StoreID) References  Store(ID),
Foreign Key (Username) References PizzaUser(Username)
);

CREATE TABLE Pizza (
OrderID DECIMAL(10,3) not null,
Code int not null,
Quantity int default 1,
Primary key (OrderID, Code),
Foreign key (OrderID) references PizzaOrder(ID)
);

Select* from Store;
Select* from PizzaOrder;
Select* from Pizza;
Select* from PizzaUser;

--Some stores to populate store with
Insert Into Store Values(101,0,1,2500,2500,2500,2500,2500,2500,2500,2500,2500,2500,2500,2500);
Insert Into Store Values(723,0,1,2500,2500,2500,2500,2500,2500,2500,2500,2500,2500,2500,2500);
Insert Into Store Values(988,0,1,2500,2500,2500,2500,2500,2500,2500,2500,2500,2500,2500,2500);