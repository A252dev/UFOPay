-- MariaDB dump 10.19-11.0.3-MariaDB, for Linux (x86_64)
--
-- Host: localhost    Database: ufopay
-- ------------------------------------------------------
-- Server version	11.0.3-MariaDB

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `Transactions`
--

DROP TABLE IF EXISTS `Transactions`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `Transactions` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `sender` longtext NOT NULL,
  `reciever` longtext NOT NULL,
  `summa` bigint(20) NOT NULL,
  `comment` longtext NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=14 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `Transactions`
--

LOCK TABLES `Transactions` WRITE;
/*!40000 ALTER TABLE `Transactions` DISABLE KEYS */;
INSERT INTO `Transactions` VALUES
(1,'adam@mail.pl','vasya@mail.pl',100,'hi man'),
(2,'adam@mail.pl','vasya@mail.pl',100,'hi man'),
(3,'adam@mail.pl','vasya@mail.pl',45,'hi man'),
(4,'adam@mail.pl','vasya@mail.pl',100,'hi man'),
(5,'adam@mail.pl','vasya@mail.pl',100,'hi man'),
(6,'adam@mail.pl','vasya@mail.pl',140,'hi man yeah'),
(7,'adam@mail.pl','vasya@mail.pl',50,'hi man'),
(8,'adam@mail.pl','vasya@mail.pl',140,'hi man'),
(9,'adam@mail.pl','vasya@mail.pl',1231,'fjghdkfghdfkj'),
(10,'adam@mail.pl','vasya@mail.pl',100,'hi man'),
(11,'adam@mail.pl','vasya@mail.pl',-1000,'hi man'),
(12,'adam@mail.pl','vasya@mail.pl',-10000,'hi man'),
(13,'adam@mail.pl','vasya@mail.pl',100,'hi man');
/*!40000 ALTER TABLE `Transactions` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `UserData`
--

DROP TABLE IF EXISTS `UserData`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `UserData` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `userId` int(11) NOT NULL,
  `firstName` longtext NOT NULL,
  `secondName` longtext NOT NULL,
  `email` longtext NOT NULL,
  `password` longtext NOT NULL,
  `repeatPassword` longtext NOT NULL,
  `registrationData` datetime(6) NOT NULL,
  `birthday` longtext NOT NULL,
  `passport` longtext NOT NULL,
  `balance` bigint(20) NOT NULL,
  `KeepLoggedIn` tinyint(1) NOT NULL,
  `AgreeWithDocs` tinyint(1) NOT NULL DEFAULT 0,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `UserData`
--

LOCK TABLES `UserData` WRITE;
/*!40000 ALTER TABLE `UserData` DISABLE KEYS */;
INSERT INTO `UserData` VALUES
(1,0,'Adam','Kowalski','adam@mail.pl','12345','12345','2023-08-15 12:45:03.967693','1900/08/28','PL32068',16794,1,0),
(2,0,'Vasya','Telnyashla','vasya@mail.pl','12345','12345','2023-08-16 10:05:13.784662','1900/08/28','PL32068',-794,1,0);
/*!40000 ALTER TABLE `UserData` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `__EFMigrationsHistory`
--

DROP TABLE IF EXISTS `__EFMigrationsHistory`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `__EFMigrationsHistory` (
  `MigrationId` varchar(150) NOT NULL,
  `ProductVersion` varchar(32) NOT NULL,
  PRIMARY KEY (`MigrationId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `__EFMigrationsHistory`
--

LOCK TABLES `__EFMigrationsHistory` WRITE;
/*!40000 ALTER TABLE `__EFMigrationsHistory` DISABLE KEYS */;
INSERT INTO `__EFMigrationsHistory` VALUES
('20230815095605_init0','7.0.10'),
('20230821103646_init4','7.0.10');
/*!40000 ALTER TABLE `__EFMigrationsHistory` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2023-08-21 20:35:27
