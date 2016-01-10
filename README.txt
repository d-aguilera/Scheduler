CROSS|OVER Scheduler assignment README file
===========================================

1) DEVELOPMENT ENVIRONMENT SETUP

	Prerequisites:

		* Microsoft Windows 7/8/10
		* Microsoft .NET framework 4.5.2
		* Microsoft Visual Studio 2015 Community Edition (or any other edition) with Update 1.
			Required features:
				- Microsoft Web Developer Tools
				- SQL Server database client (or other SQL Server-compatible provider)
		* A Microsoft SQL Server 2008 R2 (or later) running instance

2) DATABASE INITIALIZATION

	Prerequisites:

		* SQLCMD.EXE utility

	Instructions:

		2.1) Extract the contents of the zip file to a location in the hard drive (i.e. C:\daguilera\Scheduler)
		2.2) Open a command prompt and change to the \Source\Scripts directory (i.e. C:\daguilera\Scheduler\Source\Scritps)
		2.3) Determine the path to the DATA folder of SQL Server where the database files will be created (i.e C:\Program Files\Microsoft SQL Server\MSSQL10_50.MSSQLSERVER\MSSQL\DATA)
		2.4) Type the following command, replacing the argument with the path of 2.3:

			CreateDatabase.cmd "path_to_sql_data_folder"

			for instance,

			CreateDatabase.cmd "C:\Program Files\Microsoft SQL Server\MSSQL10_50.MSSQLSERVER\MSSQL\DATA"
		
			IMPORTANT: The script will use trusted connection (SQLCMD -E) to connect to a default local SQL Server instance.
					   Other login options or remote servers would require modifications to the script.

		2.5) Confirm execution by pressing ENTER. The database should be created along with all required schema objects.

3) EVENT LOG INITIALIZATION

	The application writes to de Event Log under two sources:

		* Scheduler.Web
		* Scheduler.SchedulerService

	These sources need to be created as follows:
	
	3.1) Open a command prompt as administrator.
	3.2) Change to the \Source\Scripts directory.
	3.3) Execute:

		CreateEventSources.cmd

	3.4) Verify both sources were created successfully.

4) BUILDING THE SOLUTION

	4.1) Start Visual Studio
	4.2) Open \Source\Scheduler.sln solution file. 10 projects should load:

		* Scheduler.AgentService
		* Scheduler.AgentService.Client
		* Scheduler.Common
		* Scheduler.CronService
		* Scheduler.DataAccess
		* Scheduler.DataContracts
		* Scheduler.SchedulerService
		* Scheduler.SchedulerService.Client
		* Scheduler.ServiceContracts
		* Scheduler.Web

	4.3) Build the solution and verify 0 errors, 0 warnings.

5) SOLUTION CONFIGURATION

	5.1) CONNECTION STRINGS

		Connection strings are stored in config files. There are 3 projects that require access to the database:

			* Scheduler.CronService
			* Scheduler.SchedulerService
			* Scheduler.Web

		5.1.1) Open each config file (Web.config or App.config) and locate the following section:

		  <connectionStrings>
			<add name="WebContext" connectionString="Data Source=(local); Initial Catalog=Scheduler; Integrated Security=True; MultipleActiveResultSets=True"
			  providerName="System.Data.SqlClient" />
		  </connectionStrings>

		Connection string name is expected to be "WebContext".

		5.1.2) If necessary, change the data source component of the 'connectionString' value to point to your SQL Server instance.
		5.1.3) Also, in case the native .NET SQL Server provider was not available, another compatible provider can be specified in the 'providerName' attribute.
		5.1.4) Save the files.

	5.2) USER NAMES

		This solution is based on WCF, and as such, can be extensively configured in terms of security.
		For a development/testing scenario, a basic username/password scheme is provided (Basic authentication).
		These credentials are stored in config files, in Base64 format.
		For simplicity, we'll configure services to all use the same credentials for authentication.
		In a real world scenario, however, security would be a critical concern and a robust, certificate-based security mechanism should be configured instead.

		There are 4 projects that require configuration:

		* Scheduler.AgentService
		* Scheduler.CronService
		* Scheduler.SchedulerService
		* Scheduler.Web
		
		5.2.1) Select a Windows user account for the services.
		5.2.2) Use an online service (like http://www.freeformatter.com/base64-encoder.html) to base64-encode domain, username and password.

			For example:
			
			CONTOSO\scheduler    -->   Q09OVE9TT1xzY2hlZHVsZXI=
			MySup3rZqrPaswrd!    -->   TXlTdXAzclpxclBhc3dyZCE=

		5.2.3) Using the encoded tokens above, open all 4 configuration files and add/modify the following entries accordingly:

		  <appSettings>
		    ...
		    ...
			<add key="Scheduler.Basic.UserName" value="Q09OVE9TT1xzY2hlZHVsZXI=" />
			<add key="Scheduler.Basic.Password" value="TXlTdXAzclpxclBhc3dyZCE=" />
		    ...
		    ...
		  </appSettings>

	5.3) WCF SERVICES AND CLIENTS
	
		WCF Services and clients can be extensively configured to suit the most various needs.
		For a development/testing scenario, however, no changes are immediately necessary.
		
		By default, services are setup to listen at the following endpoints:

		* Scheduler service:	http://localhost:8732/Design_Time_Addresses/Scheduler/Scheduler.svc
		
		* Agent service:    	http://localhost:8733/Design_Time_Addresses/Scheduler/Agent.svc

		Ports 8732 and 8733 are specially setup by Visual Studio so that the application doesn't require elevated users.

---- o ----
