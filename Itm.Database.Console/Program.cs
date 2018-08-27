﻿using System;
using System.Configuration;
using System.Threading.Tasks;
using AutoMapper;
using Itm.Database.Context;
using Itm.Database.Core.Entities;
using Itm.Database.Core.Services;
using Itm.Database.Services;
using Itm.Log;
using Itm.Log.Core;
using Itm.Models;
using Itm.ObjectMap;
using Microsoft.EntityFrameworkCore;
using Unity;
using Unity.Injection;

namespace Itm.Database.Console
{
	class Program
	{
		static void Main(string[] args)
		{
			IUnityContainer container = new UnityContainer();
			//Logger _logger = new Logger ("Internal.log");

			var dbConnection = ConfigurationManager.ConnectionStrings["AppDbConnection"].ConnectionString;
			container.RegisterType<IDatabaseConnection, DefaultDatabaseConnection>(new InjectionConstructor(dbConnection));

			#region SQL Server
			//var options = new DbContextOptionsBuilder<AppDbContext>().UseSqlServer(dbConnection).Options;
			#endregion SQL Server

			#region SQLite
			var options = new DbContextOptionsBuilder<AppDbContext>().UseSqlite("Data Source=AppData.db3;").Options;
			#endregion SQLite

			container.RegisterType<ILogger, Logger> (new InjectionConstructor ());
			container.RegisterType<AppDbContext>(new InjectionConstructor(options));
			container.RegisterType<IMapper, ObjectMapper> ();
			container.RegisterType<IAppUser, AppUser>(new InjectionConstructor(1, "LoggedUser"));
			container.RegisterType<IUserService, UserService>();
			IAppUser user = container.Resolve<IAppUser>();

			MainAsync(container).Wait();
		}

		static async Task MainAsync(IUnityContainer container)
		{
			IUserService repo = container.Resolve<IUserService>();

			var newUser = new UserModel
			{
				FirstName = "User-" + DateTime.Now.ToString (),
				LastName = "Last Name",
				UserName = "Username",
				Password = "Password"
			};

			await repo.AddUserAsync (newUser);

			var updateUser = repo.GetUserByIDWithCredentialsAsync (1);
			if (updateUser.Result.Model != null && updateUser.Result.DidError == false) {
				updateUser.Result.Model.LastName = "Update-1";
				await repo.UpdateUserAsync (updateUser.Result.Model);
			}

			//var updateUser = repo.GetUsersByIDAsync (1);
			//if (updateUser.Result.Model != null && updateUser.Result.DidError == false) {
			//	updateUser.Result.Model.LastName = "Update-1";
			//	await repo.UpdateUserAsync (updateUser.Result.Model);
			//}

			//var list = repo.GetUsersAsync ().Result.Model.ToList ();
			//System.Console.WriteLine (list.Count);
		}
	}
}