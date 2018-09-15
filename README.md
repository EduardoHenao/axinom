--------------------------------------------------------------------------------------------------------------------------------------------
Project

wanna instance directly from github?
	url -> https://github.com/EduardoHenao/axinom
	 	clone -> https://github.com/EduardoHenao/axinom.git

there u can check the commits and see how i work! 
thanks guys for this exercise, twas fun!

All the points of the exercise were finished. Please dig inside the code to search for it!!!

--------------------------------------------------------------------------------------------------------------------------------------------
Zip file 
	- inside the zip file is the git repo as-is
	- you will find a test zip the root -> test.zip
	- and the rest is the solution itself
--------------------------------------------------------------------------------------------------------------------------------------------
Needed tools

My tools of trade where:

		Microsoft Visual Studio Community 2017 Version 15.8.3
		Microsoft .NET Framework Version 4.7.02556
		Microsoft .NET Core 2.1.401 SDK
		MS SQL Server 14.0.1000.169
		NuGet Package Manager   4.6.0
		NuGet Package Manager in Visual Studio. For more information about NuGet, visit http://docs.nuget.org/.

--------------------------------------------------------------------------------------------------------------------------------------------
Database

Please setup your database and tables before publishing and runnin the project.
For this project I used a MS SQL database.
The connection string is inside the ./DataManagementSystem/appsettings.json file "ConnectionString" : "Data Source=.;Initial Catalog=AxinomDB;Integrated Security=True;",
feel free to change it, my string uses integrated security to log to the local mssql instance

there are 2 ways of seeding:

				With MSSQL use the following script

				use master

				create database AxinomDB

				USE [AxinomDB]
				GO

				/****** Object:  Table [dbo].[DbFileNodes]    Script Date: 9/13/2018 9:02:50 PM ******/
				SET ANSI_NULLS ON
				GO

				SET QUOTED_IDENTIFIER ON
				GO

				CREATE TABLE [dbo].[DbFileNodes](
					[Id] [bigint] IDENTITY(1,1) NOT NULL,
					[RelativePath] [nvarchar](max) NOT NULL,
					[FileName] [nvarchar](max) NOT NULL,
					[FileBytes] [varbinary](max) NOT NULL,
					[DateTime] [datetime2](7) NOT NULL,
				 CONSTRAINT [PK_DbFileNodes] PRIMARY KEY CLUSTERED 
				(
					[Id] ASC
				)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
				) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
				GO

or
from visual studio 2017 -> Packager Manager Console
execute this "update-database"
--------------------------------------------------------------------------------------------------------------------------------------------
config files

	Control Panel
		- appsettings.json: contains the basic settings for the program to operate
 			"EncryptionKey": the encryption key for this instance. Encryption in AES 128 bit = 16 characters (or bytes),
  			"FileSeparator": i tought of this if u wanted to run the thing on linux
  			"StorageFolder": the folder to store the uploaded zip file. The folder is sub path of the instance, and will be created inside the publish directory.
  			"UnzipFolder": " the folder used to unzip stuff. is a temporal folder. The folder is sub path of the instance, and will be created inside the publish directory.
  			"FileManagementServiceUrl": VERY important!!! this tells the ControlPanel where to call the FileManagementService
  		- hostsettings.json: contains the port to host the app

	Data Management System
		- appsettings.json: same as control panel's appsettings.json
			"EncryptionKey": the encryption key for this instance (used to decrypt!). Encryption in AES 128 bit = 16 characters (or bytes),
  			"FileSeparator": i tought of this if u wanted to run the thing on linux
  			"DestinyFolder": " the folder where all the sent files will rest. The folder is sub path of the instance, and will be created inside the publish directory.
  			"ConnectionString": The connection string to database
			"User": the user for HTTP Basic authentication
			"Password": the password for HTTP Basic authentication
			"PersistFiles":  If you wanna use persistance and store the files in a db, otherwise they will be stored in "DestinyFolder"
		- hostsettings.json: contains the port to host the app		
--------------------------------------------------------------------------------------------------------------------------------------------

Deploy

With Visual Studio 2017 + IIS express
		please go to "Solution Explorer" and right click on the solution -> properties
		in "Common Properties" -> "Startup Project" set "multiple startup projects"
		then set the actions for "DataManagementSystem" and "ControlPanel" to "Start" 
		and for "AxinomCommon" to "None"
		finally click the Start green icon in the tool bar of Visual Studio 2017

With dotnet CLI + Kestrel
		verify in Visual Studio that the solution compiles
		open a console and go to the root of your project, then type "cd ./DataManagementSystem"
		then execute "dotnet publish --output <DataManagementSystem publishing directory> --configuration Release"
		open a console and go to the root of your project, then type "cd ./ControlPanel"
		then execute "dotnet publish --output <Control publishing directory> --configuration Release"
		then go to each published directory and execute the dll
			for the DataManagementSystem -> dotnet DataMagementSystem.dll
			for the ControlPanel -> dotnet ControlPanel.dll
		If your project crashes due to an used port pls modify the hostsettings.json port -> "urls": "http://*:5000"
		this json file is dedicated to specify the port for kestrel

--------------------------------------------------------------------------------------------------------------------------------------------