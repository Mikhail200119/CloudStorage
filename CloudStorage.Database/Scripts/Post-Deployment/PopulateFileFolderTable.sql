delete from [dbo].FileFolder

GO

insert into [dbo].FileFolder ([Name], [ParentFolderId])
values ('Root', null)

GO