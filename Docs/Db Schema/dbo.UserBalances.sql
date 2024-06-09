CREATE TABLE [dbo].[UserBalances] (
    [Id]      INT IDENTITY (1, 1) NOT NULL,
    [UserId]  INT NOT NULL,
    [Balance] INT NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

