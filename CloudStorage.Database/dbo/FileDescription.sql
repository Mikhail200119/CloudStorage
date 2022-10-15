CREATE TABLE [dbo].[FileDescription] (
    [FileDescriptionId] INT            IDENTITY (1, 1) NOT NULL,
    [ProvidedName]      NVARCHAR (50)  NOT NULL,
    [UniqueName]        NVARCHAR (200) NOT NULL,
    [UserName]          NVARCHAR (50)  NOT NULL,
    [ContentType]       NVARCHAR (20)  NOT NULL,
    [ContentHash]       NVARCHAR (50)  NOT NULL,
    CONSTRAINT [PK_FileDescription] PRIMARY KEY CLUSTERED ([FileDescriptionId] ASC)
);

