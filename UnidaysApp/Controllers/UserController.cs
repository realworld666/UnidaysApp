using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using UnidaysApp.Models;
using UnidaysApp.Util;

namespace UnidaysApp.Controllers
{
	[Route( "api/[controller]" )]
	public class UserController : Controller
	{
		private readonly UserContext _context;

		private const int MinPasswordLength = 8;
		public const string PasswordTooShort = "Password is not long enough";
		public const string InvalidEmail = "Email is not in a recognised format";
		public const string DuplicateEmail = "An account already exists with email {0}";

		public UserController( UserContext context )
		{
			_context = context;

			/*if( _context.Users.Count() == 0 )
			{
				_context.Users.Add( new User { Email = "test@test.com", Password = "Test" } );
				_context.SaveChanges();
			}*/
		}

#if false
		[HttpGet]
		public IEnumerable<User> GetAll()
		{
			return _context.Users.ToList();
		}

		[HttpGet( "{id}", Name = "GetUser" )]
		public IActionResult GetById( long id )
		{
			var item = _context.Users.FirstOrDefault( t => t.Id == id );
			if( item == null )
			{
				return NotFound();
			}
			return new ObjectResult( item );
		}
#endif

		private User GetByEmail( string email )
		{
			return _context.Users.FirstOrDefault( t => t.Email == email );
		}

		[HttpPost]
		public IActionResult Create( [FromBody] User item )
		{
			if( item == null )
			{
				return BadRequest();
			}

			IActionResult result = ValidateInput( item );
			if( result.GetType() != typeof( OkResult ) )
			{
				return result;
			}

			// Encrypt password
			var salt = Salt.Create();
			var hash = Hash.Create( item.Password, salt );

			item.Password = hash;
			item.Salt = salt;

			_context.Users.Add( item );
			_context.SaveChanges();

			return CreatedAtRoute( "GetUser", new { id = item.Id }, item );
		}

		private IActionResult ValidateInput( User item )
		{
			var validator = new RegexUtilities();

			// Is the email valid?
			if( !validator.IsValidEmail( item.Email ) )
			{
				return BadRequest( InvalidEmail );
			}

			// Is the password long enough
			if( item.Password.Length < MinPasswordLength )
			{
				return BadRequest( PasswordTooShort );
			}

			// Have we already created an account with this email address?
			if( GetByEmail( item.Email ) != null )
			{
				return BadRequest( string.Format( DuplicateEmail, item.Email ) );
			}

			return Ok();
		}
	}
}
