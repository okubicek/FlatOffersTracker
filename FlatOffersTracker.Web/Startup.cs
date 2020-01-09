using Castle.Facilities.AspNetCore;
using Castle.Windsor;
using FlatOffersTracker.DependencyInjection.Domain;
using FlatOffersTracker.DependencyInjection.Repository;
using FlatOffersTracker.Web.Controllers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace FlatOffersTracker.Web
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		private static readonly WindsorContainer Container = new WindsorContainer();

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public IServiceProvider ConfigureServices(IServiceCollection services)
		{
			Container.AddFacility<AspNetCoreFacility>(f => f.CrossWiresInto(services));

			services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

			Container.AddScopedDdContext(Configuration.GetConnectionString("DefaultConnection"));
			Container.Install(new EFCoreRepositoryInstaller());
			Container.Install(new FlatOffersTrackerInstaller());

			// In production, the React files will be served from this directory
			services.AddSpaStaticFiles(configuration =>
			{
				configuration.RootPath = "ClientApp/build";
			});			

			return services.AddWindsor(Container, opts =>
				opts.UseEntryAssembly(typeof(FlatOffersController).Assembly),
				() => services.BuildServiceProvider(validateScopes:true));
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IHostingEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			else
			{
				app.UseExceptionHandler("/Error");
				// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
				app.UseHsts();
			}

			app.UseHttpsRedirection();
			app.UseStaticFiles();
			app.UseSpaStaticFiles();

			app.UseMvc(routes =>
			{
				routes.MapRoute(
					name: "default",
					template: "{controller}/{action=Index}/{id?}");
			});

			app.UseSpa(spa =>
			{
				spa.Options.SourcePath = "ClientApp";

				if (env.IsDevelopment())
				{
					spa.UseReactDevelopmentServer(npmScript: "start");
				}
			});
		}
	}
}
