CREATE TABLE [dbo].[RechargeTransactions] (
    [Id]                INT            IDENTITY (1, 1) NOT NULL,
    [UserId]            INT            NOT NULL,
    [BeneficiaryId]     INT            NOT NULL,
    [Date]              DATETIME2 (7)  NOT NULL,
    [RechargeAmount]    INT            NOT NULL,
    [ServiceCharge]     INT            NOT NULL,
    [TotalAmount]       INT            NOT NULL,
    [BankTransactionId] NVARCHAR (50)  NULL,
    [IsSuccess]         BIT            NOT NULL,
    [ErrorDetails]      NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_TopUpTransactions] PRIMARY KEY CLUSTERED ([Id] ASC)
);

