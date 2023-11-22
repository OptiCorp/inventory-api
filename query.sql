DECLARE @RowCount INT = 1;

WHILE @RowCount <= 5000
    BEGIN

        INSERT INTO [dbo].[Units] (Id, WPId, SerialNumber, ProductNumber, DocumentationId, Location, Description, Vendor, Comment, CreatedDate)
        VALUES (
                       'UId: ' + CAST(@RowCount AS NVARCHAR),
                       'UWPId: ' + CAST(@RowCount AS NVARCHAR),
                       'USerialNumber: ' + CAST(@RowCount AS NVARCHAR),
                       'UProductNumber: ' + CAST(@RowCount AS NVARCHAR),
                       'UDocumentationId: ' + CAST(@RowCount AS NVARCHAR),
                       'ULocation: ' + CAST(@RowCount AS NVARCHAR),
                       'UDescription: ' + CAST(@RowCount AS NVARCHAR),
                       'UVendor: ' + CAST(@RowCount AS NVARCHAR),
                       'UComment: ' + CAST(@RowCount AS NVARCHAR),
                       GETDATE()
               );

        SET @RowCount = @RowCount + 1;
    END;


DECLARE @RowCount INT = 1;

WHILE @RowCount <= 5000
    BEGIN

        INSERT INTO [dbo].[Assemblies] (Id, WPId, SerialNumber, ProductNumber, DocumentationId, Location, Description, UnitId, Vendor, Comment, CreatedDate)
        VALUES (
                       'AId: ' + CAST(@RowCount AS NVARCHAR),
                       'AWPId: ' + CAST(@RowCount AS NVARCHAR),
                       'ASerialNumber: ' + CAST(@RowCount AS NVARCHAR),
                       'AProductNumber: ' + CAST(@RowCount AS NVARCHAR),
                       'ADocumentationId: ' + CAST(@RowCount AS NVARCHAR),
                       'ALocation: ' + CAST(@RowCount AS NVARCHAR),
                       'ADescription: ' + CAST(@RowCount AS NVARCHAR),
                       (SELECT TOP 1 Id from [dbo].[Units] ORDER BY NEWID()),
                       'AVendor: ' + CAST(@RowCount AS NVARCHAR),
                       'AComment: ' + CAST(@RowCount AS NVARCHAR),
                       GETDATE()
               );

        SET @RowCount = @RowCount + 1;
    END;


DECLARE @RowCount INT = 1;

WHILE @RowCount <= 5000
    BEGIN

        INSERT INTO [dbo].[Subassemblies] (Id, WPId, SerialNumber, ProductNumber, DocumentationId, Location, Description, AssemblyId, Vendor, Comment, CreatedDate)
        VALUES (
                       'SId: ' + CAST(@RowCount AS NVARCHAR),
                       'SWPId: ' + CAST(@RowCount AS NVARCHAR),
                       'SSerialNumber: ' + CAST(@RowCount AS NVARCHAR),
                       'SProductNumber: ' + CAST(@RowCount AS NVARCHAR),
                       'SDocumentationId: ' + CAST(@RowCount AS NVARCHAR),
                       'SLocation: ' + CAST(@RowCount AS NVARCHAR),
                       'SDescription: ' + CAST(@RowCount AS NVARCHAR),
                       (SELECT TOP 1 Id from [dbo].[Assemblies] ORDER BY NEWID()),
                       'SVendor: ' + CAST(@RowCount AS NVARCHAR),
                       'SComment: ' + CAST(@RowCount AS NVARCHAR),
                       GETDATE()
               );

        SET @RowCount = @RowCount + 1;
    END;


DECLARE @RowCount INT = 1;

WHILE @RowCount <= 5000
    BEGIN

        INSERT INTO [dbo].[Parts] (Id, WPId, SerialNumber, ProductNumber, DocumentationId, Location, Description, SubassemblyId, Vendor, Comment, CreatedDate)
        VALUES (
                       'IId: ' + CAST(@RowCount AS NVARCHAR),
                       'IWPId: ' + CAST(@RowCount AS NVARCHAR),
                       'ISerialNumber: ' + CAST(@RowCount AS NVARCHAR),
                       'IProductNumber: ' + CAST(@RowCount AS NVARCHAR),
                       'IDocumentationId: ' + CAST(@RowCount AS NVARCHAR),
                       'ILocation: ' + CAST(@RowCount AS NVARCHAR),
                       'IDescription: ' + CAST(@RowCount AS NVARCHAR),
                       (SELECT TOP 1 Id from [dbo].[Subassemblies] ORDER BY NEWID()),
                       'IVendor: ' + CAST(@RowCount AS NVARCHAR),
                       'IComment: ' + CAST(@RowCount AS NVARCHAR),
                       GETDATE()
               );

        SET @RowCount = @RowCount + 1;
    END;
