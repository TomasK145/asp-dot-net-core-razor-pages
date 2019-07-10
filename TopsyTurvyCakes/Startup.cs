using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using TopsyTurvyCakes.Models;

namespace TopsyTurvyCakes
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            //folder Pages je dolezity pretoze tam bude Razor engine vyhladavat stranky
            //MVC je zareferencovane, pretoze Razor pages je super set ASP.NET Core MVC
            services.AddMvc()
                    .AddRazorPagesOptions(options =>
                    {
                        options.Conventions.AuthorizeFolder("/Admin"); //zabezpeci nastavenie autorizacia pre vsetky pages vramci foldra Admin
                        options.Conventions.AuthorizeFolder("/Account");
                        options.Conventions.AllowAnonymousToPage("/Account/Login"); //zbezpeci ze page "Login" bude dostupna aj pre anonymnych uzivatelov hoci cely folder "Account" je pod autorizaciou
                    });

            //AddTransient --> nova instancia je vytvorena kazdy krat ked je pozadovana
            //AddSingleton --> jedna instancie triedy je vytvorena len jeden krat pre cely lifecycle aplikacie, vzdy je pri poziadavka vratena ta ista instancia triedy
            //AddScoped --> jedna instancia triedy je vytvorena pre kazdy web request, prepouziva sa pocas zivotneho cyklu requestu a potom je odstranena
            services.AddTransient<IRecipesService, RecipesService>();

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme) //pridanie cookie autentifikacie
                .AddCookie();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseAuthentication();
            app.UseStaticFiles(); //asp.net core kontroluje wwwroot folder a ziska staticke files pokial maju nazov aky je pozadovany requestom

            app.UseMvcWithDefaultRoute();
        }
    }
}
