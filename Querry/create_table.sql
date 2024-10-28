CREATE TABLE `defaultdb`.`Groups` (
    `Id` INT AUTO_INCREMENT PRIMARY KEY,
    `UserId` INT NOT NULL,
    `Title` VARCHAR(255) NOT NULL,
    `Type` VARCHAR(50) NOT NULL,
    `LucideIcon` VARCHAR(255) DEFAULT '<Archive className="h-4 w-4 text-icon-primary" />',
    `CanDelete` BOOLEAN NOT NULL,
    `IsFavorite` BOOLEAN DEFAULT FALSE,
    `CreatedAt` DATETIME DEFAULT CURRENT_TIMESTAMP,
    `UpdatedAt` DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP
);

CREATE TABLE `defaultdb`.`Accounts` (
    `Id` INT PRIMARY KEY AUTO_INCREMENT,
    `UserId` INT NOT NULL,
    `Title` VARCHAR(255) NOT NULL DEFAULT '',
    `Type` VARCHAR(255) NOT NULL,
    `GroupId` INT NOT NULL DEFAULT -1,
    `Website_URL` VARCHAR(255),
    `Username` VARCHAR(255) DEFAULT '',
    `Email` VARCHAR(255) DEFAULT '',
    `Phone` VARCHAR(50) DEFAULT '',
    `Password` VARCHAR(255) DEFAULT '',
    `TwoFactor` VARCHAR(255) DEFAULT '',
    `Notes` VARCHAR(4000) DEFAULT '',
    `IsFavorite` BOOLEAN NOT NULL DEFAULT FALSE,
    `CreatedAt` DATETIME DEFAULT CURRENT_TIMESTAMP,
    `UpdatedAt` DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP
);

CREATE TABLE `defaultdb`.`Notes` (
    `Id` INT PRIMARY KEY AUTO_INCREMENT,
    `UserId` INT NOT NULL,
    `Title` VARCHAR(255) NOT NULL,
    `Type` VARCHAR(255) NOT NULL,
    `GroupId` INT NOT NULL DEFAULT -1,
    `Notes` VARCHAR(255) DEFAULT '',
    `IsFavorite` BOOLEAN NOT NULL DEFAULT FALSE,
    `CreatedAt` DATETIME DEFAULT CURRENT_TIMESTAMP,
    `UpdatedAt` DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP
);

CREATE TABLE `defaultdb`.`Identities` (
    `Id` INT PRIMARY KEY AUTO_INCREMENT,
    `UserId` INT NOT NULL,
    `GroupId` INT NOT NULL DEFAULT -1,
    `Type` VARCHAR(255) NOT NULL,
    -- Personal
    `Firstname` VARCHAR(255) DEFAULT '',
    `Lastname` VARCHAR(255) DEFAULT '',
    `DateOfBirth` VARCHAR(255) DEFAULT '',
    `Gender` VARCHAR(50) DEFAULT '',
    `Country` VARCHAR(255) DEFAULT '',
    `City` VARCHAR(255) DEFAULT '',
    `Street` VARCHAR(255) DEFAULT '',
    `Zipcode` VARCHAR(50) DEFAULT '',
    -- Passport
    `PassportID` VARCHAR(255) DEFAULT '',
    `PassportIssuedBy` VARCHAR(255) DEFAULT '',
    `PassportIssuedDate` VARCHAR(255) DEFAULT '',
    `PassportExpiredDate` VARCHAR(255) DEFAULT '',
    -- ID Card
    `IDCardID` VARCHAR(255) DEFAULT '',
    `IDCardIssuedBy` VARCHAR(255) DEFAULT '',
    `IDCardIssuedDate` VARCHAR(255) DEFAULT '',
    `IDCardExpiredDate` VARCHAR(255) DEFAULT '',
    -- Driving License
    `DrivingLicenseID` VARCHAR(255) DEFAULT '',
    `DrivingLicenseIssuedBy` VARCHAR(255) DEFAULT '',
    `DrivingLicenseIssuedDate` VARCHAR(255) DEFAULT '',
    `DrivingLicenseExpiredDate` VARCHAR(255) DEFAULT '',
    -- Contact
    `Phone` VARCHAR(50) DEFAULT '',
    `Gmail` VARCHAR(255) DEFAULT '',
    `PasswordGmail` VARCHAR(255) DEFAULT '',
    `TwoFactorGmail` VARCHAR(255) DEFAULT '',
    -- Job
    `JobTitle` VARCHAR(255) DEFAULT '',
    `JobCompany` VARCHAR(255) DEFAULT '',
    `JobDescription` VARCHAR(255) DEFAULT '',
    `JobStartDate` VARCHAR(255) DEFAULT '',
    `JobEndDate` VARCHAR(255) DEFAULT '',
    -- Other
    `IsFavorite` BOOLEAN NOT NULL DEFAULT FALSE,
    `CreatedAt` DATETIME DEFAULT CURRENT_TIMESTAMP,
    `UpdatedAt` DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP
);

CREATE TABLE `defaultdb`.`Clones` (
    `Id` INT PRIMARY KEY AUTO_INCREMENT,
    `UserId` INT NOT NULL,
    `Type` VARCHAR(255) NOT NULL,
    `GroupId` INT NOT NULL DEFAULT -1,
    -- Account credentials
    `Email` VARCHAR(255) DEFAULT '',
    `Password` VARCHAR(255) DEFAULT '',
    `TwoFactor` VARCHAR(255) DEFAULT '',
    -- Personal information
    `Phone` VARCHAR(50) DEFAULT '',
    `DisplayName` VARCHAR(255) DEFAULT '',
    `DateOfBirth` VARCHAR(255) DEFAULT '',
    -- Regional settings
    `Country` VARCHAR(255) DEFAULT '',
    `Language` VARCHAR(255) DEFAULT '',
    -- Technical settings
    `Agent` VARCHAR(255) DEFAULT '',
    `Proxy` VARCHAR(255) DEFAULT '',
    -- Account status and metadata
    `Status` VARCHAR(255) DEFAULT '',
    `Notes` VARCHAR(4000) DEFAULT '',
    `IsFavorite` BOOLEAN NOT NULL DEFAULT FALSE,
    `CreatedAt` DATETIME DEFAULT CURRENT_TIMESTAMP,
    `UpdatedAt` DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP
);

CREATE TABLE `defaultdb`.`GoogleAccounts` (
    `Id` INT PRIMARY KEY AUTO_INCREMENT,
    `UserId` INT NOT NULL,
    `Type` VARCHAR(255) NOT NULL,
    `GroupId` INT NOT NULL DEFAULT -1,
    -- Account credentials
    `Email` VARCHAR(255) DEFAULT '',
    `Password` VARCHAR(255) DEFAULT '',
    `RecoveryEmail` VARCHAR(255) DEFAULT '',
    `TwoFactor` VARCHAR(255) DEFAULT '',
    -- Personal information
    `Phone` VARCHAR(50) DEFAULT '',
    `DisplayName` VARCHAR(255) DEFAULT '',
    `DateOfBirth` VARCHAR(255) DEFAULT '',
    -- Regional settings
    `Country` VARCHAR(255) DEFAULT '',
    `Language` VARCHAR(255) DEFAULT '',
    -- Technical settings
    `Agent` VARCHAR(255) DEFAULT '',
    `Proxy` VARCHAR(255) DEFAULT '',
    -- Account status and metadata
    `Status` VARCHAR(255) DEFAULT '',
    `Notes` VARCHAR(4000) DEFAULT '',
    `IsFavorite` BOOLEAN NOT NULL DEFAULT FALSE,
    `CreatedAt` DATETIME DEFAULT CURRENT_TIMESTAMP,
    `UpdatedAt` DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP
);

CREATE TABLE `defaultdb`.`Cards` (
    `Id` INT PRIMARY KEY AUTO_INCREMENT,
    `UserId` INT NOT NULL,
    `Title` VARCHAR(255) DEFAULT '',
    `Type` VARCHAR(255) NOT NULL,
    `GroupId` INT NOT NULL DEFAULT -1,
    `FullName` VARCHAR(255) DEFAULT '',
    `CardNumber` VARCHAR(255) DEFAULT '',
    `ExpirationDate` VARCHAR(10) DEFAULT '',
    `Pin` VARCHAR(50) DEFAULT '',
    `Notes` VARCHAR(4000) DEFAULT '',
    `IsFavorite` BOOLEAN NOT NULL DEFAULT FALSE,
    `CreatedAt` DATETIME DEFAULT CURRENT_TIMESTAMP,
    `UpdatedAt` DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP
);


