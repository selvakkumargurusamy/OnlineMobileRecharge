CREATE TABLE [dbo].[Beneficiaries] (
    [Id]          INT           IDENTITY (1, 1) NOT NULL,
    [NickName]    NVARCHAR (20) NOT NULL,
    [PhoneNumber] NVARCHAR (50) NOT NULL,
    [UserId]      INT           NOT NULL,
    [IsActive]    BIT           DEFAULT ((1)) NOT NULL
);

