CREATE DATABASE  IF NOT EXISTS `autobuskastanica` /*!40100 DEFAULT CHARACTER SET utf8 COLLATE utf8_general_ci */;
USE `autobuskastanica`;
-- MySQL dump 10.13  Distrib 8.0.36, for Win64 (x86_64)
--
-- Host: 127.0.0.1    Database: autobuskastanica
-- ------------------------------------------------------
-- Server version	5.5.5-10.4.32-MariaDB

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
-- Table structure for table `autobus`
--

DROP TABLE IF EXISTS `autobus`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `autobus` (
  `BusID` int(11) NOT NULL AUTO_INCREMENT,
  `Proizvodjac` varchar(45) NOT NULL,
  `Model` varchar(45) NOT NULL,
  `Kapacitet` int(11) NOT NULL,
  `RegTablice` varchar(15) NOT NULL,
  `NazivAutoprevoznika` varchar(100) NOT NULL,
  `RutaID` int(11) NOT NULL,
  PRIMARY KEY (`BusID`,`NazivAutoprevoznika`,`RutaID`),
  KEY `fk_autobus_autoprevoznik1_idx` (`NazivAutoprevoznika`),
  KEY `fk_autobus_ruta1_idx` (`RutaID`),
  CONSTRAINT `fk_autobus_autoprevoznik1` FOREIGN KEY (`NazivAutoprevoznika`) REFERENCES `mydb`.`autoprevoznik` (`NazivAutoprevoznika`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `fk_autobus_ruta1` FOREIGN KEY (`RutaID`) REFERENCES `mydb`.`ruta` (`RutaID`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB AUTO_INCREMENT=43 DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `autobus`
--

LOCK TABLES `autobus` WRITE;
/*!40000 ALTER TABLE `autobus` DISABLE KEYS */;
INSERT INTO `autobus` VALUES (8,'Mercedes','Sprinter',20,'123-ABC','Autoprevoz Derventa',1),(9,'MAN','Lion\'s Coach',50,'456-DEF','Autoprevoz Sarajevo',2),(10,'Setra','ComfortClass',40,'789-GHI','Autoprevoz Mostar',3),(11,'Iveco','Daily',30,'101-JKL','Autoprevoz Banja Luka',4),(12,'Neoplan','Cityliner',55,'202-MNO','Autoprevoz Zenica',5),(13,'Mercedes','Tourismo',50,'E12-K-123','Centrotrans',6),(14,'Setra','S 415 HD',52,'A34-M-456','Autoprevoz BL',7),(15,'Man','Lion’s Coach',49,'T56-Z-789','GIPS Tuzla',8),(16,'Neoplan','Cityliner',53,'B78-N-012','Zenicatrans',9),(17,'Scania','Irizar',51,'C90-P-345','Mostar Bus',10),(18,'Volvo','9700',50,'D12-Q-678','Doboj Bus',11),(19,'Iveco','Evadys',48,'E34-R-901','Semberija Transport',12),(20,'Setra','S 431 DT',60,'F56-S-234','Trebinje Bus',13),(21,'Mercedes','Sprinter',20,'G78-T-567','Bihaćprevoz',14),(22,'Man','Lions City',44,'H90-U-890','Drina Transport',15),(23,'Neoplan','Starliner',55,'J12-V-123','Prijedor Bus',16),(24,'Scania','OmniExpress',50,'K34-W-456','FočaTrans',17),(25,'Volvo','9900',54,'L56-X-789','Brčko Bus',18),(26,'Iveco','Magelys',52,'M78-Y-012','Goražde Prevoz',19),(27,'Mercedes','Travego',53,'N90-Z-345','Laktaši Bus',20),(28,'Mercedes','Sprinter',30,'BL-001-PR','Autoprevoz BL',38),(29,'Setra','S 415 HD',40,'BL-002-PR','Autoprevoz BL',39),(30,'MAN','Lion\'s Coach',50,'BL-003-PR','Autoprevoz BL',40),(31,'Volvo','B11R',55,'BL-004-PR','Autoprevoz BL',41),(32,'Neoplan','Cityliner',45,'BL-005-PR','Autoprevoz BL',42),(33,'Scania','Irizar',47,'BL-006-PR','Autoprevoz BL',43),(34,'Mercedes','Tourismo',35,'BL-007-PR','Autoprevoz BL',44),(35,'Setra','S 431 DT',60,'BL-008-PR','Autoprevoz BL',45),(36,'MAN','Lions Coach',48,'BL-009-PR','Autoprevoz BL',46),(37,'Volvo','7900 Hybrid',40,'BL-010-PR','Autoprevoz BL',47),(38,'Neoplan','Cityliner',52,'BL-011-PR','Autoprevoz BL',48),(39,'Scania','K310',50,'BL-012-PR','Autoprevoz BL',49),(40,'Mercedes','Sprinter',35,'BL-013-PR','Autoprevoz BL',50),(41,'Setra','S 416',45,'BL-014-PR','Autoprevoz BL',51),(42,'MAN','TGX',50,'BL-015-PR','Autoprevoz BL',52);
/*!40000 ALTER TABLE `autobus` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `autobuskastanica`
--

DROP TABLE IF EXISTS `autobuskastanica`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `autobuskastanica` (
  `StanicaID` int(11) NOT NULL AUTO_INCREMENT,
  `NazivStanice` varchar(45) NOT NULL,
  `Mjesto` varchar(45) NOT NULL,
  `BrojPerona` int(11) NOT NULL,
  `Adresa` varchar(45) NOT NULL,
  `Kontakt` varchar(45) NOT NULL,
  PRIMARY KEY (`StanicaID`)
) ENGINE=InnoDB AUTO_INCREMENT=8 DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `autobuskastanica`
--

LOCK TABLES `autobuskastanica` WRITE;
/*!40000 ALTER TABLE `autobuskastanica` DISABLE KEYS */;
INSERT INTO `autobuskastanica` VALUES (1,'BL Stanica','Banja Luka',20,'Adresa Stanice 1','051980000'),(2,'AS Prnjavor','Prnjavor',10,'Adresa Stanice 2','098444000');
/*!40000 ALTER TABLE `autobuskastanica` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `autoprevoznik`
--

DROP TABLE IF EXISTS `autoprevoznik`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `autoprevoznik` (
  `NazivAutoprevoznika` varchar(100) NOT NULL,
  `Mjesto` varchar(45) NOT NULL,
  `Adresa` varchar(45) NOT NULL,
  `Kontakt` varchar(45) NOT NULL,
  PRIMARY KEY (`NazivAutoprevoznika`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `autoprevoznik`
--

LOCK TABLES `autoprevoznik` WRITE;
/*!40000 ALTER TABLE `autoprevoznik` DISABLE KEYS */;
INSERT INTO `autoprevoznik` VALUES ('Autoprevoz Banja Luka','Banja Luka','Kralja Petra I 20','051-123-456'),('Autoprevoz BL','Banja Luka','Kralja Petra I 23','051/234-567'),('Autoprevoz Derventa','Derventa','Glavna 1','035-123-456'),('Autoprevoz Mostar','Mostar','Stari Most bb','036-456-789'),('Autoprevoz Sarajevo','Sarajevo','Titova 10','033-789-123'),('Autoprevoz Zenica','Zenica','Trg Alije Izetbegovića 5','032-987-654'),('Bihać Prevoz','Bihać','Unska bb','037/901-234'),('Brčko Bus','Brčko','Islahijet 2','049/345-678'),('Centrotrans','Sarajevo','Titova 12','033/123-456'),('Doboj Bus','Doboj','Nikole Tesle 5','053/678-901'),('Drina Transport','Zvornik','Karađorđeva 3','056/012-345'),('FočaTrans','Foča','Svetosavska 9','058/234-567'),('GIPS Tuzla','Tuzla','Mehmeda Spahe 8','035/345-678'),('Goražde Prevoz','Goražde','Zlatnih Ljiljana 11','038/456-789'),('Laktaši Bus','Laktaši','Bulevar RS 7','051/567-890'),('Mostar Bus','Mostar','M. Tita 17','036/567-890'),('Prijedor Bus','Prijedor','Mladena Stojanovića 6','052/123-456'),('Semberija Transport','Bijeljina','Gavrila Principa 10','055/789-012'),('Trebinje Bus','Trebinje','Vuka Karadžića 14','059/890-123'),('Zenicatrans','Zenica','Blatuša bb','032/456-789');
/*!40000 ALTER TABLE `autoprevoznik` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `karta`
--

DROP TABLE IF EXISTS `karta`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `karta` (
  `KartaID` int(11) NOT NULL AUTO_INCREMENT,
  `BusID` int(11) NOT NULL,
  `Peron` int(11) NOT NULL,
  `Tip` varchar(45) NOT NULL,
  `DatumPolaska` date NOT NULL,
  `VrijemePolaska` time NOT NULL,
  `MjestoPolaska` varchar(100) NOT NULL,
  `MjestoDolaska` varchar(100) NOT NULL,
  `VrijemeIzdavanja` time NOT NULL,
  `Cijena` double NOT NULL,
  `Sjediste` int(11) NOT NULL,
  `Ruta` varchar(45) NOT NULL,
  `DatumIzdavanja` date NOT NULL,
  `RutaID` int(11) NOT NULL,
  `PutnikID` int(11) NOT NULL,
  `RadnikID` int(11) NOT NULL,
  `StanicaID` int(11) NOT NULL,
  `NazivAutoprevoznika` varchar(100) NOT NULL,
  PRIMARY KEY (`KartaID`,`RutaID`,`PutnikID`,`RadnikID`,`StanicaID`,`NazivAutoprevoznika`),
  KEY `fk_karta_ruta_idx` (`RutaID`),
  KEY `fk_karta_putnik1_idx` (`PutnikID`),
  KEY `fk_karta_radnik1_idx` (`RadnikID`),
  KEY `fk_karta_autobuskastanica1_idx` (`StanicaID`),
  KEY `fk_karta_autoprevoznik1_idx` (`NazivAutoprevoznika`),
  CONSTRAINT `fk_karta_autobuskastanica1` FOREIGN KEY (`StanicaID`) REFERENCES `mydb`.`autobuskastanica` (`StanicaID`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `fk_karta_autoprevoznik1` FOREIGN KEY (`NazivAutoprevoznika`) REFERENCES `mydb`.`autoprevoznik` (`NazivAutoprevoznika`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `fk_karta_putnik1` FOREIGN KEY (`PutnikID`) REFERENCES `mydb`.`putnik` (`PutnikID`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `fk_karta_ruta` FOREIGN KEY (`RutaID`) REFERENCES `mydb`.`ruta` (`RutaID`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB AUTO_INCREMENT=75 DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `karta`
--

LOCK TABLES `karta` WRITE;
/*!40000 ALTER TABLE `karta` DISABLE KEYS */;
INSERT INTO `karta` VALUES (61,11,2,'Jednosmjerna','2025-01-03','07:30:00','Banja Luka','Zenica','00:08:56',22.75,1,'Banja Luka - Zenica','2025-01-29',4,82,2,2,'Autoprevoz Banja Luka'),(62,11,2,'Jednosmjerna','2025-01-17','07:30:00','Banja Luka','Zenica','00:09:27',22.75,1,'Banja Luka - Zenica','2025-01-29',4,83,2,2,'Autoprevoz Banja Luka'),(63,11,2,'Povratna','2025-01-17','07:30:00','Banja Luka','Zenica','00:09:31',40.95,1,'Banja Luka - Zenica','2025-01-29',4,84,2,2,'Autoprevoz Banja Luka'),(66,10,2,'Jednosmjerna','2025-01-10','09:00:00','Mostar','Dubica','17:43:15',24,1,'Mostar - Banja Luka','2025-01-29',3,87,2,1,'Autoprevoz Mostar'),(67,35,4,'Jednosmjerna','2025-01-02','21:00:00','Prnjavor','Trebinje','17:46:06',26.25,1,'Prnjavor - Trebinje','2025-01-29',45,88,4,2,'Autoprevoz BL'),(68,41,9,'Jednosmjerna','2025-01-02','16:00:00','Prnjavor','Sarajevo','17:46:23',26.25,1,'Prnjavor - Sarajevo','2025-01-29',51,89,4,2,'Autoprevoz BL'),(70,24,2,'Jednosmjerna','2024-12-31','13:20:00','Banja Luka','Foča','18:04:17',22.5,1,'Banja Luka - Foča','2025-01-29',17,91,2,1,'FočaTrans'),(71,22,6,'Jednosmjerna','2025-01-17','17:00:00','Banja Luka','Zvornik','18:08:36',20.25,1,'Banja Luka - Zvornik','2025-01-29',15,92,2,1,'Drina Transport');
/*!40000 ALTER TABLE `karta` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `kondukter`
--

DROP TABLE IF EXISTS `kondukter`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `kondukter` (
  `KondukterID` int(11) NOT NULL AUTO_INCREMENT,
  `Ime` varchar(45) NOT NULL,
  `Prezime` varchar(45) NOT NULL,
  `JMBG` varchar(13) NOT NULL,
  `DatumRodjenja` date NOT NULL,
  `Kontakt` varchar(45) NOT NULL,
  `Adresa` varchar(45) NOT NULL,
  `Status` tinyint(4) NOT NULL,
  `NazivAutoprevoznika` varchar(100) NOT NULL,
  `BusID` int(11) NOT NULL,
  `RutaID` int(11) NOT NULL,
  PRIMARY KEY (`KondukterID`,`NazivAutoprevoznika`,`RutaID`,`BusID`),
  KEY `fk_kondukter_autoprevoznik1_idx` (`NazivAutoprevoznika`),
  KEY `fk_kondukter_autobus1_idx` (`BusID`),
  KEY `fk_kondukter_ruta1_idx` (`RutaID`),
  CONSTRAINT `fk_kondukter_autobus1` FOREIGN KEY (`BusID`) REFERENCES `mydb`.`autobus` (`BusID`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `fk_kondukter_autoprevoznik1` FOREIGN KEY (`NazivAutoprevoznika`) REFERENCES `mydb`.`autoprevoznik` (`NazivAutoprevoznika`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `fk_kondukter_ruta1` FOREIGN KEY (`RutaID`) REFERENCES `mydb`.`ruta` (`RutaID`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB AUTO_INCREMENT=36 DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `kondukter`
--

LOCK TABLES `kondukter` WRITE;
/*!40000 ALTER TABLE `kondukter` DISABLE KEYS */;
INSERT INTO `kondukter` VALUES (1,'Miloš','Petrović','1502985123456','1980-01-15','061-123-456','Ulica 1, Derventa',1,'Autoprevoz Derventa',1,1),(2,'Ivana','Marić','2506990123456','1992-06-25','062-456-789','Ulica 2, Sarajevo',1,'Autoprevoz Sarajevo',2,2),(3,'Jovan','Kovač','1011788123456','1978-11-10','064-567-890','Ulica 3, Mostar',1,'Autoprevoz Mostar',3,3),(4,'Ana','Jovanović','0509992123456','1990-03-10','065-678-901','Ulica 4, Banja Luka',1,'Autoprevoz Banja Luka',4,4),(5,'Marko','Lukić','1204888123456','1985-07-07','063-789-012','Ulica 5, Zenica',0,'Autoprevoz Zenica',5,5),(6,'Dragan','Pavić','1603343123456','1984-04-16','067123456','Sarajevo, Ulica 16',1,'Centrotrans',1,6),(7,'Zoran','Kostić','1704233323456','1973-05-17','067234567','Banja Luka, Ulica 17',1,'Autoprevoz BL',2,5),(8,'Miroslav','Stojanović','1805123523456','1962-06-18','067345678','Tuzla, Ulica 18',1,'GIPS Tuzla',3,7),(9,'Goran','Jelić','1906013723456','1951-07-19','067456789','Zenica, Ulica 19',1,'Zenicatrans',4,9),(10,'Saša','Radovanović','2006903923456','1990-08-20','067567890','Mostar, Ulica 20',1,'Mostar Bus',5,11),(11,'Branko','Matić','2107794123456','1979-09-21','067678901','Doboj, Ulica 21',1,'Doboj Bus',6,13),(12,'Radovan','Đukić','2208684323456','1968-10-22','067789012','Bijeljina, Ulica 22',1,'Semberija Transport',7,15),(13,'Ivan','Tomić','2309574523456','1957-11-23','067890123','Trebinje, Ulica 23',1,'Trebinje Bus',8,40),(14,'Dejan','Stevanović','2408464723456','1946-12-24','067901234','Bihać, Ulica 24',1,'Bihaćprevoz',9,4),(15,'Aleksa','Petrić','2507354923456','1935-01-25','068012345','Zvornik, Ulica 25',1,'Drina Transport',10,6),(16,'Rade','Živanović','2606245123456','1924-02-26','068123456','Prijedor, Ulica 26',1,'Prijedor Bus',11,8),(17,'Dario','Marinković','2705135323456','1913-03-27','068234567','Foča, Ulica 27',1,'FočaTrans',12,10),(18,'Slavko','Babić','2804025523456','1902-04-28','068345678','Brčko, Ulica 28',1,'Brčko Bus',13,12),(19,'Jovan','Nedić','2903015723456','1981-05-29','068456789','Goražde, Ulica 29',1,'Goražde Prevoz',14,14),(20,'Mladen','Karađorđević','3002905923456','1990-06-30','068567890','Laktaši, Ulica 30',1,'Laktaši Bus',15,17),(21,'Sanja','Lukić','1378901234567','1990-10-30','0657890123','Prnjavor, Ulica 17',1,'Autoprevoz BL',1,38),(22,'Vladimir','Knežević','1389012345678','1986-09-11','0658901234','Prnjavor, Ulica 18',1,'Autoprevoz BL',1,39),(23,'Marija','Jovanović','1390123456789','1993-05-19','0659012345','Prnjavor, Ulica 19',1,'Autoprevoz BL',2,40),(24,'Marko','Nikolić','1401234567890','1988-04-10','0650123456','Prnjavor, Ulica 20',1,'Autoprevoz BL',2,41),(25,'Biljana','Kovačević','1412345678901','1991-02-15','0651234500','Prnjavor, Ulica 21',1,'Autoprevoz BL',3,42),(26,'Bogdan','Matić','1423456789012','1992-08-07','0652345600','Prnjavor, Ulica 22',1,'Autoprevoz BL',3,43),(27,'Tatjana','Pavić','1434567890123','1989-11-30','0653456700','Prnjavor, Ulica 23',1,'Autoprevoz BL',4,44),(28,'Davor','Jokić','1445678901234','1995-01-21','0654567800','Prnjavor, Ulica 24',1,'Autoprevoz BL',4,45),(29,'Teodora','Ilić','1456789012345','1990-04-25','0655678900','Prnjavor, Ulica 25',1,'Autoprevoz BL',5,46),(30,'Zoran','Pekić','1467890123456','1994-07-18','0656789000','Prnjavor, Ulica 26',1,'Autoprevoz BL',5,47),(31,'Slavica','Radović','1478901234567','1991-03-12','0657890123','Prnjavor, Ulica 27',1,'Autoprevoz BL',6,48),(32,'Maja','Marković','1489012345678','1987-02-10','0658901234','Prnjavor, Ulica 28',1,'Autoprevoz BL',6,49),(33,'Petra','Stević','1490123456789','1993-06-07','0659012345','Prnjavor, Ulica 29',1,'Autoprevoz BL',7,50),(34,'Jasmina','Tomić','1501234567890','1986-11-22','0650123456','Prnjavor, Ulica 30',1,'Autoprevoz BL',7,51),(35,'Nikolina','Čović','1512345678901','1994-09-01','0651234500','Prnjavor, Ulica 31',1,'Autoprevoz BL',8,52);
/*!40000 ALTER TABLE `kondukter` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `putnik`
--

DROP TABLE IF EXISTS `putnik`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `putnik` (
  `PutnikID` int(11) NOT NULL AUTO_INCREMENT,
  `Ime` varchar(45) NOT NULL,
  `Prezime` varchar(45) NOT NULL,
  `Kontakt` varchar(45) NOT NULL,
  PRIMARY KEY (`PutnikID`)
) ENGINE=InnoDB AUTO_INCREMENT=96 DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `putnik`
--

LOCK TABLES `putnik` WRITE;
/*!40000 ALTER TABLE `putnik` DISABLE KEYS */;
INSERT INTO `putnik` VALUES (1,'Marko','Petrović','061-123-456'),(2,'Ana','Jovanović','062-987-654'),(3,'Ivana','Marić','065-345-678'),(4,'Petar','Lukić','064-789-123'),(5,'Jelena','Kovač','061-234-567'),(6,'Marko','Petrović','061-123-456'),(7,'Ana','Jovanović','062-987-654'),(8,'Ivana','Marić','065-345-678'),(9,'Petar','Lukić','064-789-123'),(10,'Jelena','Kovač','061-234-567'),(86,'Marko','Mitric','065345123'),(88,'Jovan','Simic','051676444'),(89,'Nikola','Petrovic','051676323'),(90,'Tamara','Ivanic','066323510'),(91,'Mario','Mitrovic','965545283'),(92,'Goran','Markovic','098543432'),(94,'aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa','a','123'),(95,'aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa','a','123');
/*!40000 ALTER TABLE `putnik` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `radnik`
--

DROP TABLE IF EXISTS `radnik`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `radnik` (
  `RadnikID` int(11) NOT NULL AUTO_INCREMENT,
  `Ime` varchar(45) NOT NULL,
  `Prezime` varchar(45) NOT NULL,
  `DatumRodjenja` date NOT NULL,
  `JMBG` varchar(13) NOT NULL,
  `Kontakt` varchar(45) NOT NULL,
  `Adresa` varchar(45) NOT NULL,
  `Status` tinyint(4) NOT NULL,
  `StanicaID` int(11) NOT NULL,
  PRIMARY KEY (`RadnikID`,`StanicaID`),
  KEY `fk_radnik_autobuskastanica1_idx` (`StanicaID`),
  CONSTRAINT `fk_radnik_autobuskastanica1` FOREIGN KEY (`StanicaID`) REFERENCES `mydb`.`autobuskastanica` (`StanicaID`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB AUTO_INCREMENT=11 DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `radnik`
--

LOCK TABLES `radnik` WRITE;
/*!40000 ALTER TABLE `radnik` DISABLE KEYS */;
INSERT INTO `radnik` VALUES (1,'Ivan','Marić','1985-02-15','1502985123456','065-123-456','Kralja Petra 12, Derventa',1,1),(2,'Maja','Kovač','1990-06-25','2506990123456','061-987-654','Maršala Tita 45, Sarajevo',1,1),(3,'Petar','Petrović','1978-11-10','1011788123456','064-345-678','Musala 10, Mostar',1,1),(4,'Ana','Lukić','1992-09-05','0509992123456','062-789-123','Kralja Aleksandra 7, Banja Luka',1,2),(5,'Jelena','Jovanović','1988-04-12','1204888123456','063-234-567','Travnička 18, Zenica',1,2);
/*!40000 ALTER TABLE `radnik` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `ruta`
--

DROP TABLE IF EXISTS `ruta`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `ruta` (
  `RutaID` int(11) NOT NULL AUTO_INCREMENT,
  `Ruta` varchar(45) NOT NULL,
  `VrijemePolaska` time NOT NULL,
  `MjestoPolaska` varchar(45) NOT NULL,
  `MjestoDolaska` varchar(45) NOT NULL,
  `Dani` varchar(45) NOT NULL,
  `Cijena` float NOT NULL,
  `StanicaID` int(11) NOT NULL,
  `NazivAutoprevoznika` varchar(100) NOT NULL,
  PRIMARY KEY (`RutaID`,`NazivAutoprevoznika`),
  KEY `fk_ruta_autoprevoznik1_idx` (`NazivAutoprevoznika`),
  CONSTRAINT `fk_ruta_autoprevoznik1` FOREIGN KEY (`NazivAutoprevoznika`) REFERENCES `mydb`.`autoprevoznik` (`NazivAutoprevoznika`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB AUTO_INCREMENT=53 DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `ruta`
--

LOCK TABLES `ruta` WRITE;
/*!40000 ALTER TABLE `ruta` DISABLE KEYS */;
INSERT INTO `ruta` VALUES (1,'Derventa - Sarajevo','08:00:00','Derventa','Sarajevo','Pon,Uto,Sri',25.5,1,'Autoprevoz Derventa'),(2,'Sarajevo - Mostar','14:00:00','Sarajevo','Mostar','Cet,Pet',20,1,'Autoprevoz Sarajevo'),(3,'Mostar - Banja Luka','09:00:00','Mostar','Dubica','Sub,Ned',30,1,'Autoprevoz Mostar'),(4,'Banja Luka - Zenica','07:30:00','Banja Luka','Zenica','Pon,Pet',22.75,2,'Autoprevoz Banja Luka'),(5,'Zenica - Derventa','16:00:00','Zenica','Derventa','Uto,Sri',18.5,2,'Autoprevoz Zenica'),(6,'Bijeljina - Prijedor','15:29:00','Brcko','Prnjavor','Pon',18.5,1,'Neo Turs'),(8,'Banja Luka - Sarajevo','08:00:00','Banja Luka','Sarajevo','Ponedjeljak, Srijeda, Petak',30,1,'Centrotrans'),(9,'Banja Luka - Tuzla','10:30:00','Banja Luka','Tuzla','Utorak, Četvrtak, Subota',25,1,'GIPS Tuzla'),(10,'Banja Luka - Mostar','07:00:00','Banja Luka','Mostar','Svaki dan',40,1,'Mostar Bus'),(11,'Banja Luka - Zenica','09:15:00','Banja Luka','Zenica','Ponedjeljak - Petak',22,1,'Zenicatrans'),(12,'Banja Luka - Bihać','14:45:00','Banja Luka','Bihać','Srijeda, Subota',20,1,'Bihaćprevoz'),(13,'Banja Luka - Doboj','12:00:00','Banja Luka','Doboj','Svaki dan',15,1,'Doboj Bus'),(14,'Banja Luka - Bijeljina','06:30:00','Banja Luka','Bijeljina','Ponedjeljak, Četvrtak',28,1,'Semberija Transport'),(15,'Banja Luka - Zvornik','17:00:00','Banja Luka','Zvornik','Petak, Nedjelja',27,1,'Drina Transport'),(16,'Banja Luka - Trebinje','22:00:00','Banja Luka','Trebinje','Subota',45,1,'Trebinje Bus'),(17,'Banja Luka - Foča','13:20:00','Banja Luka','Foča','Svaki dan',30,1,'FočaTrans'),(18,'Banja Luka - Goražde','15:00:00','Banja Luka','Goražde','Utorak, Četvrtak',33,1,'Goražde Prevoz'),(19,'Banja Luka - Brčko','11:10:00','Banja Luka','Brčko','Ponedjeljak, Srijeda',26,1,'Brčko Bus'),(20,'Banja Luka - Prijedor','08:40:00','Banja Luka','Prijedor','Svaki dan',10,1,'Prijedor Bus'),(21,'Banja Luka - Laktaši','19:30:00','Banja Luka','Laktaši','Svaki dan',5,1,'Laktaši Bus'),(22,'Banja Luka - Gradiška','20:00:00','Banja Luka','Gradiška','Svaki dan',12,1,'Autoprevoz BL'),(23,'Prnjavor - Banja Luka','07:30:00','Prnjavor','Banja Luka','Svaki dan',8,2,'Autoprevoz BL'),(24,'Prnjavor - Sarajevo','05:45:00','Prnjavor','Sarajevo','Ponedjeljak, Petak',32,2,'Centrotrans'),(25,'Prnjavor - Tuzla','09:50:00','Prnjavor','Tuzla','Srijeda, Subota',26,2,'GIPS Tuzla'),(26,'Prnjavor - Mostar','14:00:00','Prnjavor','Mostar','Ponedjeljak, Četvrtak',38,2,'Mostar Bus'),(27,'Prnjavor - Zenica','11:30:00','Prnjavor','Zenica','Svaki dan',24,2,'Zenicatrans'),(28,'Prnjavor - Doboj','06:00:00','Prnjavor','Doboj','Svaki dan',12,2,'Doboj Bus'),(29,'Prnjavor - Bijeljina','08:15:00','Prnjavor','Bijeljina','Utorak, Četvrtak',29,2,'Semberija Transport'),(30,'Prnjavor - Zvornik','10:00:00','Prnjavor','Zvornik','Ponedjeljak, Petak',26,2,'Drina Transport'),(31,'Prnjavor - Trebinje','23:30:00','Prnjavor','Trebinje','Subota',46,2,'Trebinje Bus'),(32,'Prnjavor - Foča','12:45:00','Prnjavor','Foča','Srijeda, Nedjelja',31,2,'FočaTrans'),(33,'Prnjavor - Goražde','16:15:00','Prnjavor','Goražde','Utorak, Četvrtak',34,2,'Goražde Prevoz'),(34,'Prnjavor - Brčko','13:50:00','Prnjavor','Brčko','Srijeda, Petak',27,2,'Brčko Bus'),(35,'Prnjavor - Prijedor','09:10:00','Prnjavor','Prijedor','Svaki dan',11,2,'Prijedor Bus'),(36,'Prnjavor - Laktaši','17:30:00','Prnjavor','Laktaši','Svaki dan',6,2,'Laktaši Bus'),(37,'Prnjavor - Gradiška','20:45:00','Prnjavor','Gradiška','Svaki dan',14,2,'Autoprevoz BL'),(38,'Prnjavor - Banja Luka','07:30:00','Prnjavor','Banja Luka','Svaki dan',8,2,'Autoprevoz BL'),(39,'Prnjavor - Tuzla','09:00:00','Prnjavor','Tuzla','Ponedjeljak, Srijeda',20,2,'Doboj Bus'),(40,'Prnjavor - Mostar','11:00:00','Prnjavor','Mostar','Ponedjeljak, Petak',30,2,'Drina Transport'),(41,'Prnjavor - Zenica','13:00:00','Prnjavor','Zenica','Svaki dan',25,2,'Foča Trans'),(42,'Prnjavor - Doboj','15:00:00','Prnjavor','Doboj','Svaki dan',15,2,'Prijedor Bus'),(43,'Prnjavor - Bijeljina','17:00:00','Prnjavor','Bijeljina','Utorak, Četvrtak',22,2,'Mostar Bus'),(44,'Prnjavor - Zvornik','19:00:00','Prnjavor','Zvornik','Ponedjeljak, Subota',20,2,'Semberija Transport'),(45,'Prnjavor - Trebinje','21:00:00','Prnjavor','Trebinje','Nedjelja',35,2,'Autoprevoz BL'),(46,'Prnjavor - Goražde','06:00:00','Prnjavor','Goražde','Srijeda, Petak',28,2,'Autoprevoz BL'),(47,'Prnjavor - Brčko','08:00:00','Prnjavor','Brčko','Svaki dan',12,2,'Autoprevoz BL'),(48,'Prnjavor - Prijedor','10:00:00','Prnjavor','Prijedor','Svaki dan',10,2,'Brčko Bus'),(49,'Prnjavor - Laktaši','12:00:00','Prnjavor','Laktaši','Svaki dan',6,2,'Autoprevoz BL'),(50,'Prnjavor - Gradiška','14:00:00','Prnjavor','Gradiška','Ponedjeljak, Srijeda',8,2,'Centrotrans'),(51,'Prnjavor - Sarajevo','16:00:00','Prnjavor','Sarajevo','Svaki dan',35,2,'Autoprevoz BL'),(52,'Prnjavor - Tuzla','18:00:00','Prnjavor','Tuzla','Utorak, Četvrtak',23,2,'Autoprevoz BL');
/*!40000 ALTER TABLE `ruta` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `vozac`
--

DROP TABLE IF EXISTS `vozac`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `vozac` (
  `VozacID` int(11) NOT NULL AUTO_INCREMENT,
  `Ime` varchar(45) NOT NULL,
  `Prezime` varchar(45) NOT NULL,
  `JMBG` varchar(13) NOT NULL,
  `DatumRodjenja` date NOT NULL,
  `Kontakt` varchar(45) NOT NULL,
  `Adresa` varchar(45) NOT NULL,
  `Status` tinyint(4) NOT NULL,
  `NazivAutoprevoznika` varchar(100) NOT NULL,
  `BusID` int(11) NOT NULL,
  `RutaID` int(11) NOT NULL,
  PRIMARY KEY (`VozacID`,`NazivAutoprevoznika`,`BusID`,`RutaID`),
  KEY `fk_vozac_autoprevoznik1_idx` (`NazivAutoprevoznika`),
  KEY `fk_vozac_autobus1_idx` (`BusID`),
  KEY `fk_vozac_ruta1_idx` (`RutaID`),
  CONSTRAINT `fk_vozac_autobus1` FOREIGN KEY (`BusID`) REFERENCES `mydb`.`autobus` (`BusID`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `fk_vozac_autoprevoznik1` FOREIGN KEY (`NazivAutoprevoznika`) REFERENCES `mydb`.`autoprevoznik` (`NazivAutoprevoznika`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `fk_vozac_ruta1` FOREIGN KEY (`RutaID`) REFERENCES `mydb`.`ruta` (`RutaID`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB AUTO_INCREMENT=37 DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `vozac`
--

LOCK TABLES `vozac` WRITE;
/*!40000 ALTER TABLE `vozac` DISABLE KEYS */;
INSERT INTO `vozac` VALUES (1,'Marko','Lukić','0509992123456','1985-05-20','061-123-456','Derventa, Ulica 1',1,'Autoprevoz Derventa',1,1),(2,'Ana','Marić','2506990123456','1990-06-25','062-345-678','Sarajevo, Ulica 2',1,'Autoprevoz Sarajevo',2,2),(3,'Petar','Kovač','1502985123456','1985-02-15','063-789-123','Mostar, Ulica 3',1,'Autoprevoz Mostar',3,3),(4,'Ivana','Jovanović','1011788123456','1978-11-10','064-987-654','Banja Luka, Ulica 4',1,'Autoprevoz Banja Luka',4,4),(5,'Jelena','Petrović','1204888123456','1988-04-12','065-234-567','Zenica, Ulica 5',1,'Autoprevoz Zenica',5,5),(6,'Marko','Jovanović','0101990123456','1990-01-01','065123456','Sarajevo, Ulica 1',1,'Centrotrans',6,6),(7,'Petar','Petrović','0202880321456','1988-02-02','065234567','Banja Luka, Ulica 2',1,'Autoprevoz BL',7,7),(8,'Nikola','Nikolić','0303770523456','1977-03-03','065345678','Tuzla, Ulica 3',1,'GIPS Tuzla',8,8),(9,'Ivan','Stanković','0404660723456','1966-04-04','065456789','Zenica, Ulica 4',1,'Zenicatrans',9,9),(10,'Milan','Živković','0505550923456','1955-05-05','065567890','Mostar, Ulica 5',1,'Mostar Bus',10,10),(11,'Stefan','Lukić','0606441123456','1964-06-06','065678901','Doboj, Ulica 6',1,'Doboj Bus',11,11),(12,'Miloš','Milić','0707331323456','1973-07-07','065789012','Bijeljina, Ulica 7',1,'Semberija Transport',12,12),(13,'Dušan','Ilić','0808221523456','1982-08-08','065890123','Trebinje, Ulica 8',1,'Trebinje Bus',13,13),(14,'Lazar','Ristić','0909111723456','1991-09-09','065901234','Bihać, Ulica 9',1,'Bihaćprevoz',14,14),(15,'Uroš','Vasić','1010001923456','2000-10-10','066012345','Zvornik, Ulica 10',1,'Drina Transport',15,15),(16,'Aleksandar','Gavrić','1110892123456','1989-11-11','066123456','Prijedor, Ulica 11',1,'Prijedor Bus',16,16),(17,'Filip','Đorđević','1211782323456','1978-12-12','066234567','Foča, Ulica 12',1,'FočaTrans',17,17),(18,'Vladimir','Milovanović','1302672523456','1967-01-13','066345678','Brčko, Ulica 13',1,'Brčko Bus',18,18),(19,'Bogdan','Simović','1401562723456','1956-02-14','066456789','Goražde, Ulica 14',1,'Goražde Prevoz',19,19),(20,'Nenad','Pavlović','1502452923456','1945-03-15','066567890','Laktaši, Ulica 15',1,'Laktaši Bus',20,20),(21,'Marko','Jovanović','1212345678901','1985-04-15','0651234567','Prnjavor, Ulica 1',1,'Autoprevoz BL',1,38),(22,'Petar','Matić','1223456789012','1990-05-25','0652345678','Prnjavor, Ulica 2',1,'Autoprevoz BL',1,39),(23,'Ivana','Pavić','1234567890123','1988-07-30','0653456789','Prnjavor, Ulica 3',1,'Autoprevoz BL',2,40),(24,'Nikola','Jokić','1245678901234','1992-02-11','0654567890','Prnjavor, Ulica 4',1,'Autoprevoz BL',2,41),(25,'Jovana','Kovačević','1256789012345','1986-03-21','0655678901','Prnjavor, Ulica 5',1,'Autoprevoz BL',3,42),(26,'Luka','Nikolić','1267890123456','1994-08-12','0656789012','Prnjavor, Ulica 6',1,'Autoprevoz BL',3,43),(27,'Ana','Pekić','1278901234567','1991-01-15','0657890123','Prnjavor, Ulica 7',1,'Autoprevoz BL',4,44),(28,'Stefan','Ilić','1289012345678','1987-10-17','0658901234','Prnjavor, Ulica 8',1,'Autoprevoz BL',4,45),(29,'Diana','Jovanović','1290123456789','1989-06-09','0659012345','Prnjavor, Ulica 9',1,'Autoprevoz BL',5,46),(30,'Milan','Radović','1301234567890','1993-11-20','0650123456','Prnjavor, Ulica 10',1,'Autoprevoz BL',5,47),(31,'Andrej','Marković','1312345678901','1985-12-06','0651234500','Prnjavor, Ulica 11',1,'Autoprevoz BL',6,48),(32,'Sara','Kostić','1323456789012','1990-01-03','0652345600','Prnjavor, Ulica 12',1,'Autoprevoz BL',6,49),(33,'Damir','Stević','1334567890123','1983-09-25','0653456700','Prnjavor, Ulica 13',1,'Autoprevoz BL',7,50),(34,'Kristina','Tomić','1345678901234','1995-05-10','0654567800','Prnjavor, Ulica 14',1,'Autoprevoz BL',7,51),(35,'Filip','Čović','1356789012345','1992-11-14','0655678900','Prnjavor, Ulica 15',1,'Autoprevoz BL',8,52),(36,'Teodora','Brkić','1367890123456','1987-06-23','0656789000','Prnjavor, Ulica 16',1,'Autoprevoz BL',8,53);
/*!40000 ALTER TABLE `vozac` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2025-01-30 12:34:28
