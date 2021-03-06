USE [master]
GO
/****** Object:  Database [FocusOA_Dev]    Script Date: 2022/6/30 11:31:04 ******/
CREATE DATABASE [FocusOA_Dev]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'FocusOA_Dev', FILENAME = N'D:\Program Files\Microsoft SQL Server\MSSQL14.MSSQLSERVER\MSSQL\DATA\FocusOA_Dev.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'FocusOA_Dev_log', FILENAME = N'D:\Program Files\Microsoft SQL Server\MSSQL14.MSSQLSERVER\MSSQL\DATA\FocusOA_Dev_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
GO
ALTER DATABASE [FocusOA_Dev] SET COMPATIBILITY_LEVEL = 140
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [FocusOA_Dev].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [FocusOA_Dev] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [FocusOA_Dev] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [FocusOA_Dev] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [FocusOA_Dev] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [FocusOA_Dev] SET ARITHABORT OFF 
GO
ALTER DATABASE [FocusOA_Dev] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [FocusOA_Dev] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [FocusOA_Dev] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [FocusOA_Dev] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [FocusOA_Dev] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [FocusOA_Dev] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [FocusOA_Dev] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [FocusOA_Dev] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [FocusOA_Dev] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [FocusOA_Dev] SET  DISABLE_BROKER 
GO
ALTER DATABASE [FocusOA_Dev] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [FocusOA_Dev] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [FocusOA_Dev] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [FocusOA_Dev] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [FocusOA_Dev] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [FocusOA_Dev] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [FocusOA_Dev] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [FocusOA_Dev] SET RECOVERY FULL 
GO
ALTER DATABASE [FocusOA_Dev] SET  MULTI_USER 
GO
ALTER DATABASE [FocusOA_Dev] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [FocusOA_Dev] SET DB_CHAINING OFF 
GO
ALTER DATABASE [FocusOA_Dev] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [FocusOA_Dev] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [FocusOA_Dev] SET DELAYED_DURABILITY = DISABLED 
GO
EXEC sys.sp_db_vardecimal_storage_format N'FocusOA_Dev', N'ON'
GO
ALTER DATABASE [FocusOA_Dev] SET QUERY_STORE = OFF
GO