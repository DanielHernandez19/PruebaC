USE [db_pruebaC]
GO
/****** Object:  Table [dbo].[Empleado]    Script Date: 27/4/2024 23:29:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Empleado](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Nombre] [nvarchar](50) NULL,
	[Apellido] [nvarchar](50) NULL,
	[Telefono] [nvarchar](25) NULL,
	[Correo] [nvarchar](50) NULL,
	[FechaContratacion] [date] NULL,
	[Foto] [nvarchar](max) NULL,
	[Archivo] [nvarchar](max) NULL,
	[TipoArchivo] [nvarchar](50) NULL,
 CONSTRAINT [PK_Empleado] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  StoredProcedure [dbo].[sp_sel_empleados]    Script Date: 27/4/2024 23:29:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_sel_empleados]
(
	@Option int = 0,
	@Id int = 0	
)
AS 
BEGIN
	IF @Option = 1
		BEGIN
			SELECT [Id], Nombre, Apellido, Telefono, Correo, FechaContratacion
			FROM [dbo].[Empleado]
			ORDER BY [Id] ASC
		END
	ELSE IF @Option = 2
		BEGIN
			SELECT [Id], Nombre, Apellido, Telefono, Correo, FechaContratacion
			FROM [dbo].[Empleado]
			WHERE [Id] = @Id
		END
END
GO
/****** Object:  StoredProcedure [dbo].[sp_trn_empleado]    Script Date: 27/4/2024 23:29:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_trn_empleado]
(
	@Option int = 0,
	@Id int = 0,
	@Nombre nvarchar(50) = null,
	@Apellido nvarchar(50) = null,
	@Telefono nvarchar(50) = null,
	@Correo nvarchar(100) = null,
	@FechaContratacion date = null,
	@Foto nvarchar(max) =null,
	@Archivo nvarchar(max) = null,
	@TipoArchivo nvarchar(50) = null
)
AS 
BEGIN
	IF @Option = 1
		BEGIN
			BEGIN TRY
				INSERT INTO  Empleado(Nombre,Apellido,Telefono,Correo,FechaContratacion, Foto,Archivo,TipoArchivo)
				VALUES (@Nombre,@Apellido,@Telefono,@Correo,@FechaContratacion,@Foto, @Archivo,@TipoArchivo)
				SELECT @Id = SCOPE_IDENTITY();
			END TRY
			BEGIN CATCH
				 SELECT ERROR_NUMBER() AS ErrorNumber,
					ERROR_SEVERITY() AS ErrorSeverity,
					ERROR_STATE() AS ErrorState,
					ERROR_PROCEDURE() AS ErrorProcedure,
					ERROR_LINE() AS ErrorLine,
					ERROR_MESSAGE() AS ErrorMessage;
			END CATCH
		 SELECT @Id AS Id;
		END
	ELSE IF @Option = 2
		BEGIN
			BEGIN TRY
				UPDATE Empleado
					SET
						Nombre = COALESCE(@Nombre, Nombre),
						Apellido = COALESCE(@Apellido, Apellido),
						Telefono = COALESCE(@Telefono, Telefono),
						Correo = COALESCE(@Correo, Correo),
						Foto = COALESCE(@Foto, Foto),
						Archivo = COALESCE(@Archivo, Archivo),
						TipoArchivo = COALESCE(@TipoArchivo, TipoArchivo)
					WHERE Id = @Id
			END TRY
			BEGIN CATCH
				 SELECT ERROR_NUMBER() AS ErrorNumber,
					ERROR_SEVERITY() AS ErrorSeverity,
					ERROR_STATE() AS ErrorState,
					ERROR_PROCEDURE() AS ErrorProcedure,
					ERROR_LINE() AS ErrorLine,
					ERROR_MESSAGE() AS ErrorMessage;
			END CATCH
		 SELECT @Id AS Id;
		END
	IF @Option = 3
	BEGIN
		BEGIN TRY
			DELETE FROM Empleado WHERE Id = @Id
		END TRY
		BEGIN CATCH
				 SELECT ERROR_NUMBER() AS ErrorNumber,
					ERROR_SEVERITY() AS ErrorSeverity,
					ERROR_STATE() AS ErrorState,
					ERROR_PROCEDURE() AS ErrorProcedure,
					ERROR_LINE() AS ErrorLine,
					ERROR_MESSAGE() AS ErrorMessage;
			END CATCH
		 SELECT @Id AS Id;
	END
END
GO
