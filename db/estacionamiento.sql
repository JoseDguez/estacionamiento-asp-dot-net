USE [master]
GO
/****** Object:  Database [Estacionamiento]    Script Date: 8/10/2016 6:57:06 PM ******/
CREATE DATABASE [Estacionamiento]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'Estacionamiento', FILENAME = N'c:\Program Files\Microsoft SQL Server\MSSQL11.SQLEXPRESS\MSSQL\DATA\Estacionamiento.mdf' , SIZE = 5120KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'Estacionamiento_log', FILENAME = N'c:\Program Files\Microsoft SQL Server\MSSQL11.SQLEXPRESS\MSSQL\DATA\Estacionamiento_log.ldf' , SIZE = 1024KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO
ALTER DATABASE [Estacionamiento] SET COMPATIBILITY_LEVEL = 110
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [Estacionamiento].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [Estacionamiento] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [Estacionamiento] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [Estacionamiento] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [Estacionamiento] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [Estacionamiento] SET ARITHABORT OFF 
GO
ALTER DATABASE [Estacionamiento] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [Estacionamiento] SET AUTO_CREATE_STATISTICS ON 
GO
ALTER DATABASE [Estacionamiento] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [Estacionamiento] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [Estacionamiento] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [Estacionamiento] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [Estacionamiento] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [Estacionamiento] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [Estacionamiento] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [Estacionamiento] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [Estacionamiento] SET  DISABLE_BROKER 
GO
ALTER DATABASE [Estacionamiento] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [Estacionamiento] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [Estacionamiento] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [Estacionamiento] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [Estacionamiento] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [Estacionamiento] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [Estacionamiento] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [Estacionamiento] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [Estacionamiento] SET  MULTI_USER 
GO
ALTER DATABASE [Estacionamiento] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [Estacionamiento] SET DB_CHAINING OFF 
GO
ALTER DATABASE [Estacionamiento] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [Estacionamiento] SET TARGET_RECOVERY_TIME = 0 SECONDS 
GO
USE [Estacionamiento]
GO
/****** Object:  Table [dbo].[ALUMNOS]    Script Date: 8/10/2016 6:57:06 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ALUMNOS](
	[matricula] [varchar](25) NOT NULL,
	[nombre] [nvarchar](50) NULL,
	[apellidos] [nvarchar](50) NULL,
	[carrera] [int] NULL,
	[turno] [char](1) NULL,
	[email] [varchar](50) NULL,
	[contrasena] [nvarchar](250) NULL,
	[rol] [nvarchar](25) NULL,
 CONSTRAINT [PK_ALUMNOS] PRIMARY KEY CLUSTERED 
(
	[matricula] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[AUTOS]    Script Date: 8/10/2016 6:57:07 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[AUTOS](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[alumno] [varchar](25) NULL,
	[marca] [int] NULL,
	[modelo] [nvarchar](50) NULL,
	[ano] [int] NULL,
	[color] [nvarchar](50) NULL,
 CONSTRAINT [PK_AUTOS] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[CAJONES]    Script Date: 8/10/2016 6:57:07 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[CAJONES](
	[alias] [nvarchar](25) NOT NULL,
	[turno] [char](1) NOT NULL,
	[ocupado] [bit] NULL,
	[auto] [int] NULL,
 CONSTRAINT [PK_CAJONES_1] PRIMARY KEY CLUSTERED 
(
	[alias] ASC,
	[turno] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[CARRERAS]    Script Date: 8/10/2016 6:57:07 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CARRERAS](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[descripcion] [nvarchar](50) NULL,
 CONSTRAINT [PK_CARRERAS] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[CICLOS_ESCOLARES]    Script Date: 8/10/2016 6:57:07 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[CICLOS_ESCOLARES](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[ciclo] [varchar](25) NULL,
	[estatus] [bit] NULL,
 CONSTRAINT [PK_CICLOS_ESCOLARES] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[MARCAS_AUTO]    Script Date: 8/10/2016 6:57:07 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[MARCAS_AUTO](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[marca] [varchar](50) NULL,
 CONSTRAINT [PK_MARCAS_AUTO] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[MODELOS_AUTO]    Script Date: 8/10/2016 6:57:07 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[MODELOS_AUTO](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[marca] [int] NULL,
	[modelo] [varchar](50) NULL,
 CONSTRAINT [PK_MODELOS_AUTO] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[PAGOS]    Script Date: 8/10/2016 6:57:07 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[PAGOS](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[alumno] [varchar](25) NULL,
	[cajon] [nvarchar](25) NULL,
	[turno] [char](1) NULL,
	[ciclo_escolar] [varchar](25) NULL,
	[monto] [decimal](18, 3) NULL,
	[pagado] [bit] NULL,
	[fecha_pago] [datetime] NULL,
	[estatus] [varchar](25) NULL,
 CONSTRAINT [PK_PAGOS] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
USE [master]
GO
ALTER DATABASE [Estacionamiento] SET  READ_WRITE 
GO
