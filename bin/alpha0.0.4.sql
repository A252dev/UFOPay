-- MariaDB dump 10.19-11.1.2-MariaDB, for Linux (x86_64)
--
-- Host: localhost    Database: ufopay
-- ------------------------------------------------------
-- Server version	11.1.2-MariaDB

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
-- Table structure for table `AddBalanceRequest`
--

DROP TABLE IF EXISTS `AddBalanceRequest`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `AddBalanceRequest` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `userId` int(11) NOT NULL,
  `summa` bigint(20) NOT NULL,
  `currency` longtext NOT NULL,
  `status` longtext NOT NULL,
  `comment` longtext NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=17 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `AddBalanceRequest`
--

LOCK TABLES `AddBalanceRequest` WRITE;
/*!40000 ALTER TABLE `AddBalanceRequest` DISABLE KEYS */;
INSERT INTO `AddBalanceRequest` VALUES
(1,534363,4355,'EUR','PAYED','X3YISM9R5L'),
(2,534363,8,'USD','CANCELLED','none'),
(3,534363,100,'USD','CANCELLED','none'),
(4,534363,200,'USD','CANCELLED','none'),
(5,534363,10,'EUR','SUCCESS','1UZ60YB3QP'),
(6,534363,10,'EUR','SUCCESS','TSSG0J8ERR'),
(7,534363,12,'USD','SUCCESS','6WL40JCAU7'),
(8,534363,11,'USD','SUCCESS','F5B3XBTQKV'),
(9,534363,123,'USD','SUCCESS','WT9KFG0RT5'),
(10,534363,12,'USD','SUCCESS','6WL40JCAU7'),
(11,534363,12,'USD','SUCCESS','6WL40JCAU7'),
(12,534363,22,'USD','CANCELLED','IONH9DFDJ6'),
(13,534363,1212,'USD','SUCCESS','N3MKEJUYO1'),
(14,534363,124,'RUB','SUCCESS','G9JILGJYTA'),
(15,534363,435,'RUB','SUCCESS','HKGS0T0M9Q'),
(16,534363,4355,'EUR','SUCCESS','X3YISM9R5L');
/*!40000 ALTER TABLE `AddBalanceRequest` ENABLE KEYS */;
UNLOCK TABLES;

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
  `currency` longtext NOT NULL,
  `comment` longtext NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=7 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `Transactions`
--

LOCK TABLES `Transactions` WRITE;
/*!40000 ALTER TABLE `Transactions` DISABLE KEYS */;
INSERT INTO `Transactions` VALUES
(1,'adam@mail.pl','vasya@mail.pl',123,'RUB','nahui idi'),
(2,'adam@mail.pl','vasya@mail.pl',332,'RUB','hi man yeah'),
(3,'adam@mail.pl','vasya@mail.pl',12,'EUR','hi bro;)'),
(4,'adam@mail.pl','vasya@mail.pl',21,'USD','sdlfksdjflsdhjlsfdkjhskfhsdkfhjsdfjsfdfs'),
(5,'adam@mail.pl','vasya@mail.pl',11,'USD','hui te vrot'),
(6,'adam@mail.pl','vasya@mail.pl',12,'USD','12');
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
  `balance_usd` bigint(20) NOT NULL,
  `balance_eur` bigint(20) NOT NULL,
  `balance_pln` bigint(20) NOT NULL,
  `balance_rub` bigint(20) NOT NULL,
  `KeepLoggedIn` tinyint(1) NOT NULL,
  `AgreeWithDocs` tinyint(1) NOT NULL,
  `admin` tinyint(1) NOT NULL DEFAULT 0,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `UserData`
--

LOCK TABLES `UserData` WRITE;
/*!40000 ALTER TABLE `UserData` DISABLE KEYS */;
INSERT INTO `UserData` VALUES
(1,534363,'Adam','Grochowski','adam@mail.pl','12345','12345','2023-08-25 21:06:27.487373','2023-08-10','NONE',1680,4423,250,4104,0,1,1),
(2,464575,'Vasya','Grochowski','vasya@mail.pl','12345','12345','2023-08-28 22:02:44.040212','2023-08-16','NULL',44,12,0,455,0,1,0),
(3,685195704,'ewa','Telnyashla','ewa@mail.pl','12345','12345','2023-08-30 10:42:46.741830','2023-08-12','NULL',0,0,0,0,0,1,0),
(4,1848620381,'Sergey','Telnyashla','sergey@mail.pl','12345','12345','2023-08-30 11:40:07.644838','2023-08-02','NULL',0,0,0,0,0,1,0),
(5,1254681643,'Sasha','Eay','sasha@mail.pl','12345','12345','2023-08-30 11:45:48.187783','2023-08-18','NULL',0,0,0,0,0,1,0);
/*!40000 ALTER TABLE `UserData` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `Wallets`
--

DROP TABLE IF EXISTS `Wallets`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `Wallets` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `PKOBankPolski` longtext NOT NULL,
  `TinkoffBank` longtext NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `Wallets`
--

LOCK TABLES `Wallets` WRITE;
/*!40000 ALTER TABLE `Wallets` DISABLE KEYS */;
INSERT INTO `Wallets` VALUES
(0,'000000','111111');
/*!40000 ALTER TABLE `Wallets` ENABLE KEYS */;
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
('20230825184003_init0','7.0.10'),
('20230825184337_init1','7.0.10'),
('20230829192744_init2','7.0.10'),
('20230830083357_init3','7.0.10'),
('20230901112207_init4','7.0.10'),
('20230903142005_init5','7.0.10');
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

-- Dump completed on 2023-09-03 18:34:04
