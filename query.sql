
DECLARE @RowCount INT = 1;

WHILE @RowCount <= 5000
BEGIN
    INSERT INTO [dbo].[Assemblies] (Id, WPId, SerialNumber, ProductNumber, Description, Vendor, CreatedDate)
    VALUES (
		'Id: ' + CAST(@RowCount AS NVARCHAR),
        'WPId: ' + CAST(@RowCount AS NVARCHAR),
        'SerialNumber: ' + CAST(@RowCount AS NVARCHAR),
		'ProductNumber: ' + CAST(@RowCount AS NVARCHAR),
		'Description: ' + CAST(@RowCount AS NVARCHAR),
		'Vendor: ' + CAST(@RowCount AS NVARCHAR),
		GETDATE()
    );

	INSERT INTO [dbo].[Items] (Id, WPId, SerialNumber, ProductNumber, Description, Vendor, CreatedDate)
    VALUES (
		'Id: ' + CAST(@RowCount AS NVARCHAR),
        'WPId: ' + CAST(@RowCount AS NVARCHAR),
        'SerialNumber: ' + CAST(@RowCount AS NVARCHAR),
		'ProductNumber: ' + CAST(@RowCount AS NVARCHAR),
		'Description: ' + CAST(@RowCount AS NVARCHAR),
		'Vendor: ' + CAST(@RowCount AS NVARCHAR),
		GETDATE()
    );

	INSERT INTO [dbo].[Subassemblies] (Id, WPId, SerialNumber, ProductNumber, Description, Vendor, CreatedDate)
    VALUES (
		'Id: ' + CAST(@RowCount AS NVARCHAR),
        'WPId: ' + CAST(@RowCount AS NVARCHAR),
        'SerialNumber: ' + CAST(@RowCount AS NVARCHAR),
		'ProductNumber: ' + CAST(@RowCount AS NVARCHAR),
		'Description: ' + CAST(@RowCount AS NVARCHAR),
		'Vendor: ' + CAST(@RowCount AS NVARCHAR),
		GETDATE()
    );

	INSERT INTO [dbo].[Units] (Id, WPId, SerialNumber, ProductNumber, Description, Vendor, CreatedDate)
    VALUES (
		'Id: ' + CAST(@RowCount AS NVARCHAR),
        'WPId: ' + CAST(@RowCount AS NVARCHAR),
        'SerialNumber: ' + CAST(@RowCount AS NVARCHAR),
		'ProductNumber: ' + CAST(@RowCount AS NVARCHAR),
		'Description: ' + CAST(@RowCount AS NVARCHAR),
		'Vendor: ' + CAST(@RowCount AS NVARCHAR),
		GETDATE()
    );

    SET @RowCount = @RowCount + 1;
END;
