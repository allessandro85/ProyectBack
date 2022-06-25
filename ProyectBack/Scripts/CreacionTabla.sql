Create table Persona(
	ID int IDENTITY(1,1) PRIMARY KEY,
	Nombre nvarchar(100),
	Apellido nvarchar(250),
	Provincia nvarchar(50),
	Dni nvarchar(8) NOT NULL,
	Telefono nvarchar(10),  
	Activo int,
	Email nvarchar(50),
	Profiles nvarchar(50),
	Skills nvarchar(50)
)