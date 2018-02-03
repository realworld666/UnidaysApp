using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using UnidaysApp.Models;

namespace UnidaysApp
{
	public class Startup
	{
		public IConfigurationRoot Configuration { get; }
		private IHostingEnvironment _hostingEnvironment;

		public Startup( IHostingEnvironment env )
		{
			var builder = new ConfigurationBuilder()
				.SetBasePath( env.ContentRootPath )
				.AddJsonFile( "appsettings.json", optional: true, reloadOnChange: true )
				.AddJsonFile( $"appsettings.{env.EnvironmentName}.json", optional: true )
				.AddEnvironmentVariables();
			Configuration = builder.Build();

			_hostingEnvironment = env;
		}

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices( IServiceCollection services )
		{
			// Add service and create Policy with options
			services.AddCors( options =>
			{
				options.AddPolicy( "CorsPolicy",
					builder => builder.AllowAnyOrigin()
						.AllowAnyMethod()
						.AllowAnyHeader()
						.AllowCredentials() );
			} );

			// TODO: Ideally the SQL connection string should be in a settings file for security
			services.AddDbContext<UserContext>( options => options.UseSqlServer( Resource.SqlConnection ) );
			services.AddMvc();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure( IApplicationBuilder app, IHostingEnvironment env )
		{
			// global policy - assign here or on each controller
			app.UseCors( "CorsPolicy" );

			app.UseMvc();
		}
	}
}
