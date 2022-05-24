# TrustedPartners

DB engine version is: Microsoft SQL Server 2016 (SP1)
The DataBase name used is: TrustedExercise

If you want to use a different db name you can adjust the connection string at app.config file:
<add name="TrustedPartners" connectionString="Server=(localdb)\ProjectsV13;Initial Catalog=TrustedExercise;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False" />

APIKEY validation was added,
you can change the APIKEY at appsettings.json file
current "ApiKey": "b55dbdd0-3fc2-4476-bc3a-fde0598f0fdc"

Data should be added running the seeder script that came with the excercise
Also you must add three store procedures to the database, run the following script:

---------------------------------------------------------
USE [dbo].[TrustedExercise]
GO

CREATE PROCEDURE [dbo].[GetTopAgents]
	@number int = 0,
	@year int = 0
	
AS
SELECT a.AGENT_CODE, a.AGENT_NAME, a.PHONE_NO
FROM
	[dbo].[AGENTS] a
	JOIN [dbo].[ORDERS] o ON a.AGENT_CODE = o.AGENT_CODE
	
GROUP BY a.AGENT_CODE, a.AGENT_NAME, a.PHONE_NO, YEAR(o.ORD_DATE)
HAVING YEAR(o.ORD_DATE) = @year AND Count(*) >= @number
ORDER BY a.AGENT_CODE

FOR JSON AUTO;

-------------------------------------------------------
USE [dbo].[TrustedExercise]
GO

CREATE PROCEDURE [dbo].[GetListOfAgents]
	@agentList nvarchar(max),
	@number int = 0
	
AS
SELECT ROW_NUMBER() OVER (PARTITION BY AGENT_CODE ORDER BY AGENT_CODE) AS number,
    [ORD_NUM], [ORD_AMOUNT], [ADVANCE_AMOUNT], [ORD_DATE], [CUST_CODE], [AGENT_CODE], [ORD_DESCRIPTION] 
	into #temp
FROM
	[dbo].[ORDERS]
WHERE 
	AGENT_CODE IN (
					SELECT * 
					FROM STRING_SPLIT(@agentList,',')
					)
ORDER BY [AGENT_CODE] ASC,[ORD_DATE] DESC

SELECT [ORD_NUM], [ORD_AMOUNT], [ADVANCE_AMOUNT], [ORD_DATE], [CUST_CODE], [AGENT_CODE], [ORD_DESCRIPTION]
FROM 
	#temp
WHERE 
	number = 2
FOR JSON AUTO;

DROP TABLE #temp

------------------------------------

USE [dbo].[TrustedExercise]
GO

CREATE PROCEDURE [dbo].[GetTopAgents]
	@number int = 0,
	@year int = 0
	
AS
SELECT a.AGENT_CODE, a.AGENT_NAME, a.PHONE_NO
FROM
	[dbo].[AGENTS] a
	JOIN [dbo].[ORDERS] o ON a.AGENT_CODE = o.AGENT_CODE
	
GROUP BY a.AGENT_CODE, a.AGENT_NAME, a.PHONE_NO, YEAR(o.ORD_DATE)
HAVING YEAR(o.ORD_DATE) = @year AND Count(*) >= @number
ORDER BY a.AGENT_CODE

FOR JSON AUTO;

______________________________________
USE [dbo].[TrustedExercise]
GO

CREATE PROCEDURE [dbo].[HighestAgentFistQuarter]
	@year int = 0
	
AS
	SELECT TOP 1 a.AGENT_CODE
FROM
	[dbo].[AGENTS] a
	JOIN [dbo].[ORDERS] o ON a.AGENT_CODE = o.AGENT_CODE
GROUP BY	a.AGENT_CODE, o.ADVANCE_AMOUNT, o.ORD_DATE
HAVING YEAR(o.ORD_DATE) = @year AND MONTH(o.ORD_DATE) IN (1,2,3)
ORDER BY SUM(o.ADVANCE_AMOUNT) DESC
FOR JSON AUTO;