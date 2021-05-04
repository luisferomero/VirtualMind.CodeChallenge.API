CREATE DATABASE CurrencyExchange
GO

USE CurrencyExchange
GO

CREATE TABLE CurrenciesPurchases (
[Id] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY,
[UserId] [int] NOT NULL,
[ISOCode] [varchar](50) NOT NULL,
[Amount] [money] NOT NULL,
[DateTime] [datetime] NOT NULL,
)