CREATE TABLE [dbo].[ShortenedUrls] (
    [Id]           UNIQUEIDENTIFIER NOT NULL,
    [LongUrl]      NVARCHAR (MAX)   NOT NULL,
    [ShortUrl]     NVARCHAR (MAX)   NOT NULL,
    [Code]         NVARCHAR (7)     NOT NULL,
    [CreatedOnUtc] DATETIME2 (7)    NOT NULL,
    CONSTRAINT [PK_ShortenedUrls] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_ShortenedUrls_Code]
    ON [dbo].[ShortenedUrls]([Code] ASC);

