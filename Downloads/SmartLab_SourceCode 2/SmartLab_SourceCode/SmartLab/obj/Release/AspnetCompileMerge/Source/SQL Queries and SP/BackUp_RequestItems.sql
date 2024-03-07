Automation of Archival creation and Deletion of SQL Table data on Lab Infra Planning and Ordering Requests

1. Request details need to be retained for 10 years and backup to be maintained for 2 years after retention period.
  i.e.,  If older data greater than 10 years retention period is available, 
	1. Archive data older than 10 years, but not older than 12 years
	2. Remove data older than 10 years from production db after  archival

--Context: 
--1. 10 years Request data need to be retained
--2. After 10 years, data needs to be archived for 2 years and removed from the production db  

--Check for data < 10 years
USE BudgetingToolDB_UAT
IF EXISTS(Select * from RequestItems_Table where VKM_Year < (YEAR(GETDATE()) - 10))
BEGIN
	-- If older data greater than 10 years retention period is available, 
	
	-- 1. Archive data < 10 years
	-- 2. Remove data < 10 years from production db after archival
                     -- 3. Remove Archival data < 12 years, if present
	Insert into BudgetingToolDB_Archive_RequestItems_Table.dbo.RequestItems_Table  Select * from RequestItems_Table where VKM_Year < (YEAR(GETDATE()) - 10) 
	Delete from RequestItems_Table where VKM_Year < (YEAR(GETDATE()) - 10) 
	Delete from BudgetingToolDB_Archive_RequestItems_Table.dbo.RequestItems_Table where VKM_Year < (YEAR(GETDATE()) - 12)
END
