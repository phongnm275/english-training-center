-- ============================================================================
-- Database Maintenance & Backup Scripts
-- English Training Center Management System
-- Version: 1.0
-- Created: January 28, 2026
-- ============================================================================

USE [master];
GO

-- ============================================================================
-- 1. Create Database Backup (FULL BACKUP)
-- ============================================================================
-- Description: Create a full backup of the database
-- Usage: EXECUTE [sp_BackupDatabase] 'C:\Backups\', 'EnglishTrainingCenter'
-- ============================================================================

CREATE PROCEDURE [dbo].[sp_BackupDatabase]
    @BackupPath NVARCHAR(500),
    @DatabaseName NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;
    
    DECLARE @BackupFile NVARCHAR(500);
    DECLARE @BackupCommand NVARCHAR(1000);
    
    -- Generate backup filename with timestamp
    SET @BackupFile = @BackupPath + @DatabaseName + '_' + 
                      REPLACE(REPLACE(REPLACE(CONVERT(NVARCHAR(19), GETDATE(), 120), '-', ''), ' ', '_'), ':', '') + 
                      '.bak';
    
    -- Execute backup
    SET @BackupCommand = 'BACKUP DATABASE [' + @DatabaseName + '] TO DISK = N''' + @BackupFile + 
                        ''' WITH NOFORMAT, INIT, NAME = N''' + @DatabaseName + '-Full Database Backup'', 
                        SKIP, NOREWIND, NOUNLOAD, STATS = 10;';
    
    EXEC sp_executesql @BackupCommand;
    
    PRINT 'Database backup completed successfully to: ' + @BackupFile;
END
GO

-- ============================================================================
-- 2. Rebuild Indexes
-- ============================================================================
-- Description: Rebuild all fragmented indexes in the database
-- ============================================================================

CREATE PROCEDURE [dbo].[sp_RebuildIndexes]
    @DatabaseName NVARCHAR(100),
    @FragmentationThreshold INT = 10
AS
BEGIN
    SET NOCOUNT ON;
    
    DECLARE @TableName NVARCHAR(255);
    DECLARE @IndexName NVARCHAR(255);
    DECLARE @SQL NVARCHAR(500);
    
    DECLARE IndexCursor CURSOR FOR
    SELECT DISTINCT
        OBJECT_NAME(ips.object_id) AS TableName,
        i.name AS IndexName
    FROM sys.dm_db_index_physical_stats(DB_ID(@DatabaseName), NULL, NULL, NULL, 'LIMITED') ips
    JOIN sys.indexes i ON ips.object_id = i.object_id AND ips.index_id = i.index_id
    WHERE ips.avg_fragmentation_in_percent > @FragmentationThreshold
    AND ips.page_count > 1000
    AND i.name IS NOT NULL;
    
    OPEN IndexCursor;
    
    FETCH NEXT FROM IndexCursor INTO @TableName, @IndexName;
    
    WHILE @@FETCH_STATUS = 0
    BEGIN
        SET @SQL = 'ALTER INDEX [' + @IndexName + '] ON [' + @TableName + '] REBUILD;';
        EXEC sp_executesql @SQL;
        
        PRINT 'Rebuilt index: ' + @IndexName + ' on table: ' + @TableName;
        
        FETCH NEXT FROM IndexCursor INTO @TableName, @IndexName;
    END
    
    CLOSE IndexCursor;
    DEALLOCATE IndexCursor;
    
    PRINT 'Index rebuild completed.';
END
GO

-- ============================================================================
-- 3. Update Statistics
-- ============================================================================
-- Description: Update statistics for all tables
-- ============================================================================

CREATE PROCEDURE [dbo].[sp_UpdateStatistics]
    @DatabaseName NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;
    
    DECLARE @TableName NVARCHAR(255);
    DECLARE @SQL NVARCHAR(500);
    
    DECLARE StatsCursor CURSOR FOR
    SELECT TABLE_NAME
    FROM INFORMATION_SCHEMA.TABLES
    WHERE TABLE_SCHEMA = 'dbo'
    AND TABLE_TYPE = 'BASE TABLE';
    
    OPEN StatsCursor;
    
    FETCH NEXT FROM StatsCursor INTO @TableName;
    
    WHILE @@FETCH_STATUS = 0
    BEGIN
        SET @SQL = 'UPDATE STATISTICS [dbo].[' + @TableName + '] WITH FULLSCAN;';
        EXEC sp_executesql @SQL;
        
        PRINT 'Updated statistics for: ' + @TableName;
        
        FETCH NEXT FROM StatsCursor INTO @TableName;
    END
    
    CLOSE StatsCursor;
    DEALLOCATE StatsCursor;
    
    PRINT 'Statistics update completed.';
END
GO

-- ============================================================================
-- 4. Check Database Integrity
-- ============================================================================
-- Description: Run DBCC CHECKDB to check database integrity
-- ============================================================================

CREATE PROCEDURE [dbo].[sp_CheckDatabaseIntegrity]
    @DatabaseName NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;
    
    DECLARE @SQL NVARCHAR(500);
    
    -- Check database integrity
    SET @SQL = 'DBCC CHECKDB ([' + @DatabaseName + ']) WITH PHYSICAL_ONLY;';
    
    EXEC sp_executesql @SQL;
    
    PRINT 'Database integrity check completed.';
END
GO

-- ============================================================================
-- 5. Cleanup Old Backups
-- ============================================================================
-- Description: Delete backup files older than specified days
-- ============================================================================

CREATE PROCEDURE [dbo].[sp_CleanupOldBackups]
    @BackupPath NVARCHAR(500),
    @DaysToKeep INT = 30
AS
BEGIN
    SET NOCOUNT ON;
    
    DECLARE @SQL NVARCHAR(1000);
    DECLARE @FileList TABLE (FileName NVARCHAR(500));
    
    -- Get file listing (requires xp_cmdshell - use with caution)
    -- This is a simplified approach; in production, use PowerShell or maintenance plans
    
    PRINT 'Backup cleanup routine for files older than ' + CAST(@DaysToKeep AS NVARCHAR(10)) + ' days';
    PRINT 'Backup path: ' + @BackupPath;
    -- Additional cleanup logic can be implemented here
END
GO

-- ============================================================================
-- Sample Usage Scripts
-- ============================================================================

/*
-- Backup Database
EXECUTE sp_BackupDatabase 
    @BackupPath = 'C:\Backups\',
    @DatabaseName = 'EnglishTrainingCenter';

-- Rebuild Indexes (when fragmentation > 10%)
EXECUTE sp_RebuildIndexes
    @DatabaseName = 'EnglishTrainingCenter',
    @FragmentationThreshold = 10;

-- Update Statistics
EXECUTE sp_UpdateStatistics
    @DatabaseName = 'EnglishTrainingCenter';

-- Check Database Integrity
EXECUTE sp_CheckDatabaseIntegrity
    @DatabaseName = 'EnglishTrainingCenter';

-- Cleanup Old Backups (older than 30 days)
EXECUTE sp_CleanupOldBackups
    @BackupPath = 'C:\Backups\',
    @DaysToKeep = 30;
*/

PRINT '========================================';
PRINT 'Maintenance Procedures Created!';
PRINT '========================================';
