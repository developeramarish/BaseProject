﻿using System.Threading.Tasks;
using Itm.Models;
using Moq;
using Xunit;
using Itm.Database.Core.Services.ResponseTypes;
using Itm.Log.Core;
using System;
using System.Diagnostics;
using Itm.ObjectMap;
using Itm.Database.Core.Entities;
using AutoMapper;
using Itm.Database.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Itm.Database.Services.UnitTest
{
    public class UserServiceTest
    {
        [Fact]
        //public async Task AddUserAsync_AddUser_Success()
        public void AddUserAsync_AddUser_Success()
        {
            // arrange
            // ILogger
            var loggerMock = new Mock<ILogger>();
            loggerMock.Setup(_ => _.Info(It.IsAny<string>()))
                .Callback((string message) => Trace.WriteLine(message));

            // IMapper
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new UserProfile());
            });
            var mapper = config.CreateMapper();


            // IAppUser
            var appUser = new AppUser(1, "LoggedUser");

            //AppDbContext
            //https://stackoverflow.com/questions/47553878/mocking-entity-framework-core-context
            var options = new DbContextOptionsBuilder<AppDbContext>()
                  .UseInMemoryDatabase(Guid.NewGuid().ToString())
                  .ConfigureWarnings(w => w.Ignore(InMemoryEventId.TransactionIgnoredWarning))
                  .Options;

            var appDbContext = new AppDbContext(options);

            IUserService userService = new UserService(loggerMock.Object, mapper, appUser, appDbContext);

            var newUser = new UserModel
            {
                FirstName = "User-" + DateTime.Now.ToString(),
                LastName = "Last Name",
                UserName = "Username",
                Password = "Password"
            };

            // act
            var dbUser = userService.AddUserAsync(newUser);

            // assert
            Assert.Equal(newUser.FirstName, dbUser.Result.Model.FirstName);
        }
    }
}