﻿ -- Wait for 8 second
WAITFOR DELAY '00:00:08'
BEGIN TRANSACTION TRAN1
BEGIN TRY  
	--CODES BEGIN
	
	

	--CODES END
COMMIT TRANSACTION TRAN1
PRINT 'COMMIT TRANSACTION TRAN1';
END TRY  
BEGIN CATCH  
ROLLBACK TRANSACTION TRAN1
PRINT 'ROLLBACK TRANSACTION TRAN1';
     THROW;  
END CATCH 