﻿using System;
using System.Text;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace UnidaysApp.Util
{
	public class Hash
	{
		public static string Create( string value, string salt )
		{
			var valueBytes = KeyDerivation.Pbkdf2(
				password: value,
				salt: Encoding.UTF8.GetBytes( salt ),
				prf: KeyDerivationPrf.HMACSHA256,
				iterationCount: 5000,
				numBytesRequested: 256 / 8 );

			return Convert.ToBase64String( valueBytes );
		}

		public static bool Validate( string value, string salt, string hash )
		{
			return Create( value, salt ) == hash;
		}
	}
}
