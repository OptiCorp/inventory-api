DECLARE @RowCount INT = 1;

WHILE @RowCount <= 5000
BEGIN

INSERT INTO [dbo].[Items] (Id, WpId, SerialNumber, ProductNumber, Type, DocumentationId, Location, Description, Vendor, Comment, CreatedDate)
VALUES (
    'UId: ' + CAST(@RowCount AS NVARCHAR),
    'UWpId: ' + CAST(@RowCount AS NVARCHAR),
    'USerialNumber: ' + CAST(@RowCount AS NVARCHAR),
    'UProductNumber: ' + CAST(@RowCount AS NVARCHAR),
    'Unit',
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

INSERT INTO [dbo].[Items] (Id, WpId, SerialNumber, ProductNumber, Type, DocumentationId, Location, Description, ParentId, Vendor, Comment, CreatedDate)
VALUES (
    'AId: ' + CAST(@RowCount AS NVARCHAR),
    'AWpId: ' + CAST(@RowCount AS NVARCHAR),
    'ASerialNumber: ' + CAST(@RowCount AS NVARCHAR),
    'AProductNumber: ' + CAST(@RowCount AS NVARCHAR),
    'Assembly',
    'ADocumentationId: ' + CAST(@RowCount AS NVARCHAR),
    'ALocation: ' + CAST(@RowCount AS NVARCHAR),
    'ADescription: ' + CAST(@RowCount AS NVARCHAR),
    (SELECT TOP 1 Id from [dbo].[Items] WHERE Type = 'Unit' ORDER BY NEWID()),
    'AVendor: ' + CAST(@RowCount AS NVARCHAR),
    'AComment: ' + CAST(@RowCount AS NVARCHAR),
    GETDATE()
    );

SET @RowCount = @RowCount + 1;
END;


DECLARE @RowCount INT = 1;

WHILE @RowCount <= 5000
BEGIN

INSERT INTO [dbo].[Items] (Id, WpId, SerialNumber, ProductNumber, Type, DocumentationId, Location, Description, ParentId, Vendor, Comment, CreatedDate)
VALUES (
    'SId: ' + CAST(@RowCount AS NVARCHAR),
    'SWpId: ' + CAST(@RowCount AS NVARCHAR),
    'SSerialNumber: ' + CAST(@RowCount AS NVARCHAR),
    'SProductNumber: ' + CAST(@RowCount AS NVARCHAR),
    'Subassembly',
    'SDocumentationId: ' + CAST(@RowCount AS NVARCHAR),
    'SLocation: ' + CAST(@RowCount AS NVARCHAR),
    'SDescription: ' + CAST(@RowCount AS NVARCHAR),
    (SELECT TOP 1 Id from [dbo].[Items] WHERE Type = 'Assembly' ORDER BY NEWID()),
    'SVendor: ' + CAST(@RowCount AS NVARCHAR),
    'SComment: ' + CAST(@RowCount AS NVARCHAR),
    GETDATE()
    );

SET @RowCount = @RowCount + 1;
END;


DECLARE @RowCount INT = 1;

WHILE @RowCount <= 5000
BEGIN

INSERT INTO [dbo].[Items] (Id, WpId, SerialNumber, ProductNumber, Type, DocumentationId, Location, Description, ParentId, Vendor, Comment, CreatedDate)
VALUES (
    'IId: ' + CAST(@RowCount AS NVARCHAR),
    'IWpId: ' + CAST(@RowCount AS NVARCHAR),
    'ISerialNumber: ' + CAST(@RowCount AS NVARCHAR),
    'IProductNumber: ' + CAST(@RowCount AS NVARCHAR),
    'Part',
    'IDocumentationId: ' + CAST(@RowCount AS NVARCHAR),
    'ILocation: ' + CAST(@RowCount AS NVARCHAR),
    'IDescription: ' + CAST(@RowCount AS NVARCHAR),
    (SELECT TOP 1 Id from [dbo].[Items] WHERE Type = 'Subassembly' ORDER BY NEWID()),
    'IVendor: ' + CAST(@RowCount AS NVARCHAR),
    'IComment: ' + CAST(@RowCount AS NVARCHAR),
    GETDATE()
    );

SET @RowCount = @RowCount + 1;
END;
