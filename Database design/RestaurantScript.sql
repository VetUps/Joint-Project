-- MySQL dump 10.13  Distrib 8.0.40, for Win64 (x86_64)
--
-- Host: 127.0.0.1    Database: restaurant
-- ------------------------------------------------------
-- Server version	8.0.39

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!50503 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `allergen`
--

DROP TABLE IF EXISTS `allergen`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `allergen` (
  `allergen_id` int NOT NULL AUTO_INCREMENT,
  `allergen_name` varchar(50) DEFAULT NULL,
  PRIMARY KEY (`allergen_id`)
) ENGINE=InnoDB AUTO_INCREMENT=10 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `allergen`
--

LOCK TABLES `allergen` WRITE;
/*!40000 ALTER TABLE `allergen` DISABLE KEYS */;
INSERT INTO `allergen` VALUES (1,'Глютен'),(2,'Молочные продукты'),(3,'Орехи'),(4,'Рыба и морепродукты'),(5,'Соевые продукты'),(6,'Цитрус'),(7,'Яйцо'),(8,'Свинина'),(9,'Грибы');
/*!40000 ALTER TABLE `allergen` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `client`
--

DROP TABLE IF EXISTS `client`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `client` (
  `client_id` int NOT NULL AUTO_INCREMENT,
  `client_firstname` varchar(45) DEFAULT NULL,
  `client_name` varchar(45) DEFAULT NULL,
  `client_email` varchar(30) DEFAULT NULL,
  `client_phone` char(13) NOT NULL,
  PRIMARY KEY (`client_id`)
) ENGINE=InnoDB AUTO_INCREMENT=13 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `client`
--

LOCK TABLES `client` WRITE;
/*!40000 ALTER TABLE `client` DISABLE KEYS */;
INSERT INTO `client` VALUES (1,'Валерий','Иванов','val.ivanov@example.com','79123456789'),(2,'Марк','Петров','mark.petrov@example.com','79991234567'),(3,'Мария','Сидорова','maria.sidorov@example.ru','79001112233'),(4,'Анна','Ковалева','anna.kovalev@mail.ru','74951234567'),(5,'Сергей','Сергеев','sergey.sergeev@gmail.com','78125556677'),(6,'Ольга','Смирнова','olga.smirnova@yandex.ru','79211234567'),(7,'Дмитрий','Кузнецов','dmitry.kuznetsov@bk.ru','79515554433'),(8,'Елена','Федорова','elena.fedorova@list.ru','74997776655'),(9,'Андрей','Васильев','andrey.vasilyev@protonmail.com','79651112222'),(10,'Светлана','Николаева','svetlana.nikolaeva@mail.ru','78001234567'),(11,'Алексей','Петров','alexey.petrov@example.com','79112223344'),(12,'Татьяна','Соколова','tatiana.sokolova@gmail.com','79265554433');
/*!40000 ALTER TABLE `client` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `client_table`
--

DROP TABLE IF EXISTS `client_table`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `client_table` (
  `client_id` int NOT NULL,
  `table_id` int NOT NULL,
  KEY `table_fk_idx` (`table_id`),
  KEY `client_fk_idx` (`client_id`),
  CONSTRAINT `client_fk` FOREIGN KEY (`client_id`) REFERENCES `client` (`client_id`),
  CONSTRAINT `table_fk` FOREIGN KEY (`table_id`) REFERENCES `table` (`table_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `client_table`
--

LOCK TABLES `client_table` WRITE;
/*!40000 ALTER TABLE `client_table` DISABLE KEYS */;
INSERT INTO `client_table` VALUES (1,1),(2,2),(3,3),(4,4),(5,5),(6,6),(7,7),(8,8),(9,9),(10,10),(11,11),(12,12),(1,2),(2,2),(3,6),(4,5),(5,5),(6,5),(7,8),(8,8),(9,12),(10,12),(11,12),(12,12);
/*!40000 ALTER TABLE `client_table` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `dish`
--

DROP TABLE IF EXISTS `dish`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `dish` (
  `dish_id` int NOT NULL AUTO_INCREMENT,
  `dish_name` varchar(50) DEFAULT NULL,
  `dish_description` text,
  `dish_price` float DEFAULT NULL,
  `menu_category_id` int DEFAULT NULL,
  `dish_image` varchar(45) DEFAULT NULL,
  PRIMARY KEY (`dish_id`),
  KEY `menu_category_fk_idx` (`menu_category_id`),
  CONSTRAINT `menu_category_fk` FOREIGN KEY (`menu_category_id`) REFERENCES `menu_category` (`menu_category_id`)
) ENGINE=InnoDB AUTO_INCREMENT=57 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `dish`
--

LOCK TABLES `dish` WRITE;
/*!40000 ALTER TABLE `dish` DISABLE KEYS */;
INSERT INTO `dish` VALUES (1,'Цезарь','Салат с курицей, пармезаном и гренками.',250,1,NULL),(2,'Салат с тунцом','Салат с тунцом, яйцом и оливками.',300,1,NULL),(3,'Греческий салат','Огурцы, помидоры, оливки, фета и оливковое масло.',280,1,NULL),(4,'Салат с авокадо и креветками','Авокадо, креветки, лимонный сок и зелень.',350,1,NULL),(5,'Вегетарианская пицца','Пицца с овощами и моцареллой.',400,2,NULL),(6,'Пицца Пепперони','Пицца с пепперони и сыром.',450,2,NULL),(7,'Маргарита','Классическая пицца с томатным соусом и базиликом.',380,2,NULL),(8,'Спагетти Болоньезе','Паста с мясным соусом и пармезаном.',400,3,NULL),(9,'Пенне Альфредо','Паста с кремовым соусом и курицей.',420,3,NULL),(10,'Фетучини с грибами','Паста с грибами в сливочном соусе.',380,3,NULL),(11,'Тирамису','Десерт на основе кофе и маскарпоне.',250,4,NULL),(12,'Шоколадный фондан','Десерт с жидким шоколадом внутри.',300,4,NULL),(13,'Панна котта','Итальянский десерт на основе сливок и желатина.',220,4,NULL),(14,'Лосось на гриле','Филе лосося, запеченное с лимоном.',600,5,NULL),(15,'Креветки в чесночном соусе','Креветки, обжаренные с чесноком и зеленью.',500,5,NULL),(16,'Куриные крылышки','Запеченные с соусом барбекю.',250,6,NULL),(17,'Брускетта','Поджаренный хлеб с томатами и базиликом.',150,6,NULL),(18,'Мини-кебабы','Мясные кебабы, подаваемые с соусом.',300,6,NULL),(19,'Чипсы из овощей','Хрустящие чипсы из свеклы, моркови и картофеля.',200,6,NULL),(20,'Свежевыжатый сок','Апельсиновый или яблочный.',150,7,NULL),(21,'Чай','Чай черный или зеленый.',100,7,NULL),(22,'Кофе','Эспрессо или американо.',120,7,NULL),(23,'Фруктовый салат без сахара','Смешанные свежие фрукты.',180,8,NULL),(24,'Чиа пудинг','Пудинг из семян чиа с кокосовым молоком.',200,8,NULL),(25,'Ризотто с грибами','Кремовое ризотто с лесными грибами.',350,9,NULL),(26,'Фалафель с тахини','Жареные шарики из нута с соусом тахини.',300,9,NULL),(27,'Курица на гриле','Куриное филе, приготовленное на гриле.',500,10,NULL),(28,'Овощи на гриле','Ассорти из овощей, приготовленных на гриле.',350,10,NULL),(29,'Цезарь','Салат с курицей, пармезаном и гренками.',250,1,NULL),(30,'Салат с тунцом','Салат с тунцом, яйцом и оливками.',300,1,NULL),(31,'Греческий салат','Огурцы, помидоры, оливки, фета и оливковое масло.',280,1,NULL),(32,'Салат с авокадо и креветками','Авокадо, креветки, лимонный сок и зелень.',350,1,NULL),(33,'Вегетарианская пицца','Пицца с овощами и моцареллой.',400,2,NULL),(34,'Пицца Пепперони','Пицца с пепперони и сыром.',450,2,NULL),(35,'Маргарита','Классическая пицца с томатным соусом и базиликом.',380,2,NULL),(36,'Спагетти Болоньезе','Паста с мясным соусом и пармезаном.',400,3,NULL),(37,'Пенне Альфредо','Паста с кремовым соусом и курицей.',420,3,NULL),(38,'Фетучини с грибами','Паста с грибами в сливочном соусе.',380,3,NULL),(39,'Тирамису','Десерт на основе кофе и маскарпоне.',250,4,NULL),(40,'Шоколадный фондан','Десерт с жидким шоколадом внутри.',300,4,NULL),(41,'Панна котта','Итальянский десерт на основе сливок и желатина.',220,4,NULL),(42,'Лосось на гриле','Филе лосося, запеченное с лимоном.',600,5,NULL),(43,'Креветки в чесночном соусе','Креветки, обжаренные с чесноком и зеленью.',500,5,NULL),(44,'Куриные крылышки','Запеченные с соусом барбекю.',250,6,NULL),(45,'Брускетта','Поджаренный хлеб с томатами и базиликом.',150,6,NULL),(46,'Мини-кебабы','Мясные кебабы, подаваемые с соусом.',300,6,NULL),(47,'Чипсы из овощей','Хрустящие чипсы из свеклы, моркови и картофеля.',200,6,NULL),(48,'Свежевыжатый сок','Апельсиновый или яблочный.',150,7,NULL),(49,'Чай','Чай черный или зеленый.',100,7,NULL),(50,'Кофе','Эспрессо или американо.',120,7,NULL),(51,'Фруктовый салат без сахара','Смешанные свежие фрукты.',180,8,NULL),(52,'Чиа пудинг','Пудинг из семян чиа с кокосовым молоком.',200,8,NULL),(53,'Ризотто с грибами','Кремовое ризотто с лесными грибами.',350,9,NULL),(54,'Фалафель с тахини','Жареные шарики из нута с соусом тахини.',300,9,NULL),(55,'Курица на гриле','Куриное филе, приготовленное на гриле.',500,10,NULL),(56,'Овощи на гриле','Ассорти из овощей, приготовленных на гриле.',350,10,NULL);
/*!40000 ALTER TABLE `dish` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `dish_allergen`
--

DROP TABLE IF EXISTS `dish_allergen`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `dish_allergen` (
  `dish_id` int NOT NULL,
  `allergen_id` int NOT NULL,
  PRIMARY KEY (`dish_id`,`allergen_id`),
  KEY `allergen_fk_idx` (`allergen_id`),
  CONSTRAINT `allergen_fk` FOREIGN KEY (`allergen_id`) REFERENCES `allergen` (`allergen_id`),
  CONSTRAINT `dish_fk` FOREIGN KEY (`dish_id`) REFERENCES `dish` (`dish_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `dish_allergen`
--

LOCK TABLES `dish_allergen` WRITE;
/*!40000 ALTER TABLE `dish_allergen` DISABLE KEYS */;
INSERT INTO `dish_allergen` VALUES (1,1),(5,1),(6,1),(7,1),(8,1),(9,1),(10,1),(1,2),(3,2),(5,2),(6,2),(7,2),(8,2),(9,2),(10,2),(11,2),(12,2),(13,2),(17,2),(2,4),(4,4),(14,4),(15,4),(20,6),(1,7),(2,7),(4,7),(11,7),(12,7),(6,8),(25,9);
/*!40000 ALTER TABLE `dish_allergen` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `dish_order`
--

DROP TABLE IF EXISTS `dish_order`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `dish_order` (
  `order_id` int NOT NULL,
  `dish_id` int NOT NULL,
  KEY `dish_order_fk_idx` (`dish_id`),
  KEY `order_dish_fk_idx` (`order_id`),
  CONSTRAINT `dish_order_fk` FOREIGN KEY (`dish_id`) REFERENCES `dish` (`dish_id`),
  CONSTRAINT `order_dish_fk` FOREIGN KEY (`order_id`) REFERENCES `order` (`order_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `dish_order`
--

LOCK TABLES `dish_order` WRITE;
/*!40000 ALTER TABLE `dish_order` DISABLE KEYS */;
INSERT INTO `dish_order` VALUES (1,1),(1,2),(2,3),(3,4),(3,5),(4,1),(1,1),(1,2),(1,3),(2,5),(2,21),(3,8),(3,11),(3,20),(4,14),(4,28),(5,6),(5,22),(6,17),(6,18),(7,23),(7,24),(8,9),(9,10),(9,12),(10,15),(11,25),(11,26);
/*!40000 ALTER TABLE `dish_order` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `feedback`
--

DROP TABLE IF EXISTS `feedback`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `feedback` (
  `feedback_id` int NOT NULL AUTO_INCREMENT,
  `client_id` int DEFAULT NULL,
  `rating` int DEFAULT NULL,
  `feedback_text` text,
  `feedback_date` date DEFAULT NULL,
  PRIMARY KEY (`feedback_id`),
  KEY `client_fk_idx2` (`client_id`) /*!80000 INVISIBLE */,
  CONSTRAINT `freedback_cl` FOREIGN KEY (`client_id`) REFERENCES `client` (`client_id`)
) ENGINE=InnoDB AUTO_INCREMENT=11 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `feedback`
--

LOCK TABLES `feedback` WRITE;
/*!40000 ALTER TABLE `feedback` DISABLE KEYS */;
INSERT INTO `feedback` VALUES (1,1,5,'Прекрасное обслуживание и вкусная еда! Обязательно вернемся.','2023-11-15'),(2,2,4,'В целом хорошо, но немного медленное обслуживание. Еда отличная.','2023-11-16'),(3,3,3,'Обычное место, ничего особенного.','2023-11-17'),(4,4,1,'Ужасное обслуживание, еда невкусная. Больше сюда не приду.','2023-11-18'),(5,1,5,'Снова посетили ресторан и остались очень довольны! Меню обновилось, все очень вкусно.','2023-12-01'),(6,5,4,'Заказали столик заранее, нас сразу проводили. Официант был внимателен, но иногда приходилось ждать. Блюда вкусные, особенно понравился десерт.  Атмосфера приятная. Цены немного выше среднего, но качество соответствует. Рекомендую попробовать.','2023-12-05'),(7,6,2,NULL,'2023-12-10'),(8,7,1,'Ужасно.','2023-12-12'),(9,NULL,5,'Лучший ресторан в городе!','2023-12-15'),(10,9,3,'Всё нормально, без нареканий.',NULL);
/*!40000 ALTER TABLE `feedback` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `menu_category`
--

DROP TABLE IF EXISTS `menu_category`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `menu_category` (
  `menu_category_id` int NOT NULL AUTO_INCREMENT,
  `menu_category_name` varchar(40) DEFAULT NULL,
  PRIMARY KEY (`menu_category_id`)
) ENGINE=InnoDB AUTO_INCREMENT=11 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `menu_category`
--

LOCK TABLES `menu_category` WRITE;
/*!40000 ALTER TABLE `menu_category` DISABLE KEYS */;
INSERT INTO `menu_category` VALUES (1,'Салаты'),(2,'Пицца'),(3,'Паста'),(4,'Десерты'),(5,'Рыба и морепродукты'),(6,'Закуски'),(7,'Напитки'),(8,'Десерты без сахара'),(9,'Вегетарианские блюда'),(10,'Блюда на гриле');
/*!40000 ALTER TABLE `menu_category` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `order`
--

DROP TABLE IF EXISTS `order`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `order` (
  `order_id` int NOT NULL AUTO_INCREMENT,
  `client_id` int DEFAULT NULL,
  `order_date` date DEFAULT NULL,
  `order_status` varchar(25) DEFAULT NULL,
  PRIMARY KEY (`order_id`),
  KEY `order_client_fk_idx` (`client_id`),
  CONSTRAINT `order_client_fk` FOREIGN KEY (`client_id`) REFERENCES `client` (`client_id`)
) ENGINE=InnoDB AUTO_INCREMENT=12 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `order`
--

LOCK TABLES `order` WRITE;
/*!40000 ALTER TABLE `order` DISABLE KEYS */;
INSERT INTO `order` VALUES (1,1,'2025-03-14','подан'),(2,1,'2025-03-14','подан'),(3,2,'2025-03-15','обработка заказа'),(4,3,'2025-03-15','новый заказ'),(5,4,'2025-03-15','готовится'),(6,1,'2025-03-10','подан'),(7,5,'2025-03-25','обработка заказа'),(8,6,'2025-03-20','новый заказ'),(9,7,'2025-03-15','готовится'),(10,8,'2025-03-13','подан'),(11,9,'2025-03-15','обработка заказа');
/*!40000 ALTER TABLE `order` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `table`
--

DROP TABLE IF EXISTS `table`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `table` (
  `table_id` int NOT NULL AUTO_INCREMENT,
  `table_capacity` int DEFAULT NULL,
  `table_location` varchar(30) DEFAULT NULL,
  `table_status` varchar(15) DEFAULT NULL,
  PRIMARY KEY (`table_id`)
) ENGINE=InnoDB AUTO_INCREMENT=13 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `table`
--

LOCK TABLES `table` WRITE;
/*!40000 ALTER TABLE `table` DISABLE KEYS */;
INSERT INTO `table` VALUES (1,4,'Главный зал','Свободен'),(2,2,'У окна','Занят'),(3,6,'На веранде','Зарезервирован'),(4,8,'На веранде','Свободен'),(5,4,'Главный зал','Занят'),(6,2,'У окна','Свободен'),(7,2,'На веранде','Свободен'),(8,2,'У бара','Занят'),(9,4,'Главный зал','Свободен'),(10,4,'На веранде','Свободен'),(11,6,'Главный зал','Свободен'),(12,6,'Главный зал','Занят');
/*!40000 ALTER TABLE `table` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2025-03-26 10:24:30
