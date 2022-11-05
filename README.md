## Online store ##
### Description of our site: ###
Наш сайт – это интернет-магазин, с товарами различных категорий. На сайте есть авторизация пользователей, а также удобная корзина.

#### Используемые технологии: ####
-	ASP.NET Core
-	MongoDB

#### Модуль 1: ####
- **Работа с Аккаунтом:**
    - создание аккаунта;
    - вход в аккаунт;
    - просмотр аккаунта;
    - изменение данных аккаунта;
    - удаление аккаунта;
    
#### Модуль 2: ####
 - **Работа с корзиной:**
    - просмотр товаров в корзине;
    - удаление товара из корзины;
    - добавление товара в корзину.

 - **Работа с заказами:**
    - оформление заказов;
    - просмотр заказов;

#### Модуль 3: ####
- **Работа с товарами:**
    - просмотр всех товаров;
    - просмотр товаров с фильтрацией;

____

### Обоснование выбора БД ###

Мы выбрали нереляционную базу данных MongoDB для нашего проекта. Нагляднее всего аргументировать выбор данной БД можно при сравнении её с реляционной БД (в данном случае MySQL):
- Разница в представлении данных. В MongoDB, данные представлены в виде коллекций JSON документов, а в MySQL – в виде строк и таблиц. 
    - Json формат, который чаще всего используется в http запросах, следовательно использование БД с документами Json упростит логику взаимодействия БД и веб-сайта.
- Разница в запросах к базе данных. SQL (Structured Query Language) - специальный язык запросов для взаимодействия с SQL базами, такими как MySQL, PostgreSQL и иными. В MongoDB запросы являются объектно-ориентированными.
    - Т.к. мы используем ORM подход, то использование sql запросов не является необходимым, следовательно нет смысла брать sql БД. 
- Запросы по нескольким таблицам. В MySQL есть операция JOIN, которая позволяет осуществлять запросы сразу по нескольким таблицам данных. MongoDB хоть и не имеет такой функции, зато снабжена многомерными типами данных.
    - Многомерные типы данных - основная причина выбора MongoDB в качестве БД для нашего проекта, т.к. в массиве можно хранить список всех товаров в корзине или заказе.


### Пример базы данных в MongoDB: ###
#### Пример документа Clients: ####
*{"_id": { "$oid": "635d4a5348344c8dc743332e" }, "Login": "", "Mail": "", "Name": "", "Password": ""}*

#### Пример документа Orders: ####
*{ "_id": { "$oid": "635d4b2b48344c8dc743332f" }, "Counts": { "$numberLong": "100" }, "Products": [ { "id": "635d4bc148344c8dc7433331", "Category": "", "Counts": 32, "Image": "", "Name": "", "Price": 21 } ], "idClient": 1}*

#### Пример документа Products: ####
*{"_id": { "$oid": "635d4bc148344c8dc7433331" }, "Category": "", "Counts": { "$numberLong": "100" }, "Image": "", "Price": 12}*

____

### Модули api будут использовать 3 модели для общения ###
#### Reg User ####
- ID
- Password
- Login
#### Модель User со следующими полями: ####
- ID
- Name
- Email
- Phone number
#### Модель Order со следующими полями: ####
- ID
- Products
- IdUser
- Total sum
- Date
#### Модель Basket ####
- ID
- IdUser
- Products
- Total sum
#### Модель Category ####
- ID
- Name
- Subcategories
#### Модель SubCategory ####
- ID
- Name
- Products
#### Модель Product со следующими полями: ####
-	ID
-	Name
-	Counts
-	Price
-	Image

## Список endpoints и описание к ним ##

#### Работа с аккаунтом: ####
-	Создание Аккаунта:
    - POST api/clients/registration; login, password, name, mail;
        - 201 – Created, модель User
        - 406 - notAcceptable
-	Вход в аккаунт:
    - POST api/clients/authorization; login, password
        - 200 – Ok, модель User
        - 404 – notFound
-	Просмотр аккаунта:
    - Get api/clients/{ID:int32}; need authorization with user role;
        - 200 – ok, модель User
        - 404 – notFound
        - 401 – unauthorized
-	Изменение данных аккаунта:
    - PUT api/clients/{ID:int32}; login, password, name, mail; need authorization with user role;
        - 200 – OK, модель User
        - 404 – notFound
        - 401 – unauthorized
-	Удаление Аккаунта:
    - Delete api/clients/{ID:int32}; need authorization with user role
        - 204 – noContent
        - 404 – notFound
        - 401 - unauthorized

#### Работа с заказами ####
-	Удаление товара из корзины:
    - PUT – api/clients/{ID:int32}/Basket, need authorization with User role, productId, userId, basketId, count;
        - 200 – OK
        - 404 – NotFound
        - 401 – Unauthorized
-	Просмотр товаров в корзине:
    - GET – api/clients/{ID:int32}/Basket, need authorization with User role, UserModel;
        - 200 – OK, array of ProductModel
        - 404 – NotFound
        - 401 – Unauthorized
-	Оформление заказов:
    - POST – api/clients/{ID:int32}/Basket, need authorization with User BasketModel;
        - 200 – OK
        - 404 – NotFound
        - 401 – Unauthorized
-	Просмотр заказов
    - GET – api/clients/{ID:int32}/Orders, need authorization with User role, UserModel;
        - 200 – OK, array of OrderModel
        - 404 – NotFound
        - 401 – Unauthorized

#### Работа с товарами: ####
- Просмотр всех товаров:
    - Get api/Products, don’t need authorization;
        - 200 - OK, array of product model
        - 404 - NotFound
-	Просмотр котегорий:
    - GET api/Products/Category, don’t need authorization;
        - 200 - OK, array of SubcategoryModel
        - 404 - NotFound
-	Просмотр субкатегорий:
    - GET api/Products/Category/subcategory, don’t need authorization;
        - 200 - OK, array of product model
        - 404 - NotFound
-	Просмотр Товара:
    - GET api/Products/{ID:int32}, don’t need authorization;
        - 200 - OK, product model
        - 404 - NotFound
-	Добавление товара в корзину:
    - POST api/Products/{productId:int32}, need authorization with user role;
        - 204 - noContent
        - 404 - NotFound
        - 401 – Unauthorized



