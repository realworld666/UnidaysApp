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
