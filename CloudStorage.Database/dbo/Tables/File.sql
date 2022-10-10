CREATE TABLE [dbo].[File] (
    [FileId]       INT             IDENTITY (1, 1) NOT NULL,
    [ProvidedName] NVARCHAR (MAX)  NOT NULL,
    [UniqueName]   NVARCHAR (MAX)  NOT NULL,
    [Content]      VARBINARY (MAX) NOT NULL,
    [UserName]     NVARCHAR (50)   NOT NULL,
    PRIMARY KEY CLUSTERED ([FileId] ASC)
);

