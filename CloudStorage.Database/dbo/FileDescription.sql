CREATE TABLE [dbo].[FileDescription] (
    [FileDescriptionId] INT             IDENTITY (1, 1) NOT NULL,
    [ProvidedName]      NVARCHAR (50)   NOT NULL,
    [UniqueName]        NVARCHAR (100)  NOT NULL,
    [ContentType]       NVARCHAR (100)  NOT NULL,
    [SizeInBytes]       INT             NOT NULL,
    [CreatedDate]       DATETIME2 (7)   NOT NULL,
    [ContentHash]       NVARCHAR (150)  NOT NULL,
    [UploadedBy]        VARCHAR (100)   NOT NULL,
    [Preview]           VARBINARY (MAX) NULL,
    CONSTRAINT [PK_FileDescription] PRIMARY KEY CLUSTERED ([FileDescriptionId] ASC)
);