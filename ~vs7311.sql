USE [University]
GO
/****** Object:  StoredProcedure [dbo].[GetApplication]    Script Date: 3/16/2019 4:23:14 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER Procedure [dbo].[GetApplication]  (@emailid nvarchar(200))
AS 
Begin

begin try
    IF (@emailid != '')
	begin	
	 select * from Application where Email=@emailid;
	end
	else
	begin
	select * from Application;
	end
end try
begin catch 
select 'Faild' as status
end catch
end

exec [GetApplication] @emailid=''