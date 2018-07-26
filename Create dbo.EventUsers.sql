USE [H2O]
GO

/****** Object: Table [dbo].[EventUsers] Script Date: 26-07-2018 AM 06:07:21 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[EventUsers] (
    [ProjectUserId] INT            IDENTITY (1, 1) NOT NULL,
    [EventId]       INT            NOT NULL,
    [UserId]        INT            NOT NULL,
	[Joining]       VARCHAR (100)  NOT NULL,
    [Email]         VARCHAR (1000) NULL,
    [Name]          VARCHAR (1000) NULL,
    [Phone]         VARCHAR (100)  NULL,
    [UpdatedDate]   DATETIME       NOT NULL
);


DROP TABLE [dbo].[EventUsers] 