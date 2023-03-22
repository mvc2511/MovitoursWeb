use BDMovitours;
go

create table Cliente(
IdCliente int primary key identity(1,1),
Nombre varchar(100),
Apellido varchar(100),
Correo varchar(100),
Contrasena varchar(50)
)
go

create proc sp_RegistrarUsuario(
@Nombre varchar(100),
@Apellido varchar(100),
@Correo varchar(100),
@Contrasena varchar(50),
@Registrado bit output,
@Mensaje varchar(100) output
)
as 
begin 
	if (not exists(select * from Cliente where Correo = @Correo))
	begin
		insert into Cliente(Nombre, Apellido, Correo, Contrasena) values(@Nombre, @Apellido, @Correo, @Contrasena)
		set @Registrado = 1
		set @Mensaje = 'Usuario registrado correctamente'
	end 
	else
	begin
		set @Registrado = 0
		set @Mensaje = 'Ya existe un usuario con ese correo' 
		end
end
go


create proc sp_ValidarUsuario(
@Correo varchar(100),
@Contrasena varchar(50)
)
as 
begin
	if(exists(select * from Cliente where Correo = @Correo and Contrasena = @Contrasena))
		select IdCliente from Cliente where Correo = @Correo and Contrasena = @Contrasena
	else 
		select '0'
end
go



declare @registrado bit, @mensaje varchar(100)
exec sp_RegistrarUsuario 'Mariano','Ventura', 'mariano@gmail.com', 'test123', @registrado output, @mensaje output
select @registrado
select @mensaje
go

select * from Cliente;
go


exec sp_ValidarUsuario 'mariano@gmail.com', 'test123'
go

delete Cliente;
go

DBCC CHECKIDENT ('Usuario', RESEED, 0);
GO