using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using UnidaysApp.Controllers;
using UnidaysApp.Models;
using Xunit;

namespace UnitTestProject
{
	public class UserControllerUnitTests
	{
		List<User> mockUsers = new List<User>
		{
			new User { Email = "test1@grr.la" },
			new User { Email = "test2@grr.la" },
			new User { Email = "test3@grr.la" },
		};

		[Fact]
		public void CreateValidUser()
		{
			// Arrange
			UserController controller = GetController();

			// Act
			var result = controller.Create( new User()
			{
				Email = "unittest@grr.la",
				Password = "12345678"
			} );

			// Assert
			var createResult = Assert.IsType<CreatedAtRouteResult>( result );

		}

		[Fact]
		public void TestPasswordTooShort()
		{
			// Arrange
			UserController controller = GetController();

			// Act
			var result = controller.Create( new User()
			{
				Email = "unittest@grr.la",
				Password = "1234567"
			} );

			// Assert
			var createResult = Assert.IsType<BadRequestObjectResult>( result );
			Assert.Equal( createResult.Value, UserController.PasswordTooShort );
		}


		[Fact]
		public void TestInvalidEmail()
		{
			// Arrange
			UserController controller = GetController();

			// Act
			var result = controller.Create( new User()
			{
				Email = "bobbins",
				Password = "12345678"
			} );

			// Assert
			var createResult = Assert.IsType<BadRequestObjectResult>( result );
			Assert.Equal( createResult.Value, UserController.InvalidEmail );

		}

		private UserController GetController()
		{
			var data = mockUsers.AsQueryable();


			var mockSet = new Mock<DbSet<User>>();
			mockSet.As<IQueryable<User>>().Setup( u => u.Provider ).Returns( data.Provider );
			mockSet.As<IQueryable<User>>().Setup( u => u.Expression ).Returns( data.Expression );
			mockSet.As<IQueryable<User>>().Setup( u => u.ElementType ).Returns( data.ElementType );
			mockSet.As<IQueryable<User>>().Setup( u => u.GetEnumerator() ).Returns( data.GetEnumerator() );

			var mockContext = new Mock<UserContext>();
			mockContext.Setup( c => c.Users ).Returns( mockSet.Object );

			var controller = new UserController( mockContext.Object );
			return controller;
		}
	}
}
