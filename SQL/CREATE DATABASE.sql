IF NOT EXISTS ( SELECT [name] FROM sys.databases WHERE [name] = 'Pharmacy' )
CREATE DATABASE Pharmacy
ON
(	
	NAME = N'Pharmacy', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL12.SQLEXPRESS\MSSQL\DATA\Pharmacy.mdf' , 
	SIZE = 5120KB , 
	MAXSIZE = UNLIMITED, 
	FILEGROWTH = 1024MB )
 LOG ON 
(   
	NAME = N'Pharmacy_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL12.SQLEXPRESS\MSSQL\DATA\Pharmacy_log.ldf' , 
	SIZE = 2048KB , 
	MAXSIZE = 2048GB , 
	FILEGROWTH = 10% )
	