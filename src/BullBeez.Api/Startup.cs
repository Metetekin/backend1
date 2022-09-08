using AutoMapper;

using BullBeez.Api.Batch;
using BullBeez.Api.Classed;
using BullBeez.Api.Helper;
using BullBeez.Core.Helper;
using BullBeez.Core.Services;
using BullBeez.Core.UOW;
using BullBeez.Data.Context;
using BullBeez.Data.UOW;
using BullBeez.Service;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BullBeez.Api
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{

			services.AddControllers();
			services.AddScoped<IUnitOfWork, UnitOfWork>();
			services.AddScoped<ICompanyAndPersonService, CompanyAndPersonService>();
			services.AddScoped<ICommonService, CommonService>();
			services.AddScoped<IServiceService, ServiceService>();
			services.AddScoped<INotificationHelper, NotificationHelper>();
			services.AddScoped<ISmsService, SmsService>();
#if !DEBUG
		services.AddSingleton<TwitterBatch>();
		services.AddHostedService<TwitterBatch>(provider => provider.GetService<TwitterBatch>());
		string mySqlConnectionStr = Configuration.GetConnectionString("DevConnection");
		//services.AddDbContext<BullBeezDBContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DevConnection2")), ServiceLifetime.Transient);
		services.AddDbContext<BullBeezDBContext>(options => options.UseMySql(mySqlConnectionStr, ServerVersion.AutoDetect(mySqlConnectionStr)));
#endif

#if DEBUG
			services.AddDbContext<BullBeezDBContext>(opt => opt.UseSqlServer("Server=.;Initial Catalog=BullBezz;uid =sa;pwd=123;MultipleActiveResultSets=True;"));
#endif





			services.AddControllersWithViews().SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
			services.AddSwaggerGen(c =>
			{
				c.SwaggerDoc("v1", new OpenApiInfo { Title = "BullBeez.Api", Version = "v1" });
			});

			//JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
			services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
			{
				options.TokenValidationParameters = new TokenValidationParameters
				{
					ValidateIssuer = true,
					ValidateAudience = true,
					ValidateIssuerSigningKey = true,
					ValidIssuer = "bullbeez.com",
					ValidAudience = "bullbeez.com",
					IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("ahbasshfbsahjfbshajbfhjasbfashjbfsajhfvashjfashfbsahfbsahfksdjf"))

				};
			});

			services.ConfigureApplicationCookie(options => options.LoginPath = "/api/Token");

			services.AddAutoMapper(typeof(Startup));
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env,BullBeezDBContext context)
		{
#if !DEBUG
		UpdateDatabase(app);
#endif


			if (env.IsDevelopment())
			{
				//context.Database.EnsureCreated();
				app.UseDeveloperExceptionPage();
				app.UseSwagger();
				app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "BullBeez.Api v1"));
				//app.UseMiddleware<RequestLoggingMiddleware>();
			}

			app.UseHttpsRedirection();

			app.UseRouting();

			app.UseAuthorization();


			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
				endpoints.MapControllerRoute(
						name: "default",
						pattern: "{controller=Home}/{action=Index}/{id?}");
			});
		}

		private static void UpdateDatabase(IApplicationBuilder app)
		{
			using (var serviceScope = app.ApplicationServices
					.GetRequiredService<IServiceScopeFactory>()
					.CreateScope())
			{
				using (var context = serviceScope.ServiceProvider.GetService<BullBeezDBContext>())
				{
					context.Database.Migrate();
				}
			}
		}
	}
}
