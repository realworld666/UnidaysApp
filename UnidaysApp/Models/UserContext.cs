using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace UnidaysApp.Models
{
	public class UserContext : DbContext
	{
		public virtual DbSet<User> Users { get; set; }

		public UserContext( DbContextOptions<UserContext> options )
			: base( options )
		{

		}

		public UserContext()
		{

		}

		protected override void OnModelCreating( ModelBuilder modelBuilder )
		{
			modelBuilder.Entity<User>( entity =>
			{
				entity.Property( e => e.Email ).IsRequired();
				entity.Property( e => e.Password ).IsRequired();
			} );
		}
	}
}
