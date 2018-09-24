﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using System.IO;
using System.Net;
using Dapper;
using Identity;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using SiproModelCore.Models;
using Utilities;


namespace SSubproducto
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            var mapper = (SqlMapper.ITypeMap)Activator
                .CreateInstance(typeof(ColumnAttributeTypeMapper<>)
                .MakeGenericType(typeof(Subproducto)));
            SqlMapper.SetTypeMap(typeof(Subproducto), mapper);
            var mapper2 = (SqlMapper.ITypeMap)Activator
               .CreateInstance(typeof(ColumnAttributeTypeMapper<>)
               .MakeGenericType(typeof(AcumulacionCosto)));
            SqlMapper.SetTypeMap(typeof(AcumulacionCosto), mapper2);
            var mapper3 = (SqlMapper.ITypeMap)Activator
               .CreateInstance(typeof(ColumnAttributeTypeMapper<>)
               .MakeGenericType(typeof(SubproductoTipo)));
            SqlMapper.SetTypeMap(typeof(SubproductoTipo), mapper3);
            var mapper4 = (SqlMapper.ITypeMap)Activator
               .CreateInstance(typeof(ColumnAttributeTypeMapper<>)
               .MakeGenericType(typeof(UnidadEjecutora)));
            SqlMapper.SetTypeMap(typeof(UnidadEjecutora), mapper4);
            var mapper5 = (SqlMapper.ITypeMap)Activator
               .CreateInstance(typeof(ColumnAttributeTypeMapper<>)
               .MakeGenericType(typeof(Producto)));
            SqlMapper.SetTypeMap(typeof(Producto), mapper5);
            var mapper6 = (SqlMapper.ITypeMap)Activator
               .CreateInstance(typeof(ColumnAttributeTypeMapper<>)
               .MakeGenericType(typeof(Entidad)));
            SqlMapper.SetTypeMap(typeof(Entidad), mapper6);
            /*var mapper7 = (SqlMapper.ITypeMap)Activator
               .CreateInstance(typeof(ColumnAttributeTypeMapper<>)
               .MakeGenericType(typeof(Objeto)));
            SqlMapper.SetTypeMap(typeof(Objeto), mapper7);*/
            var mapper8 = (SqlMapper.ITypeMap)Activator
               .CreateInstance(typeof(ColumnAttributeTypeMapper<>)
               .MakeGenericType(typeof(PagoPlanificado)));
            SqlMapper.SetTypeMap(typeof(PagoPlanificado), mapper8);
            var mapper9 = (SqlMapper.ITypeMap)Activator
               .CreateInstance(typeof(ColumnAttributeTypeMapper<>)
               .MakeGenericType(typeof(SubproductoPropiedad)));
            SqlMapper.SetTypeMap(typeof(SubproductoPropiedad), mapper9);
            var mapper10 = (SqlMapper.ITypeMap)Activator
               .CreateInstance(typeof(ColumnAttributeTypeMapper<>)
               .MakeGenericType(typeof(SubproductoPropiedadValor)));
            SqlMapper.SetTypeMap(typeof(SubproductoPropiedadValor), mapper10);
            var mapper11 = (SqlMapper.ITypeMap)Activator
               .CreateInstance(typeof(ColumnAttributeTypeMapper<>)
               .MakeGenericType(typeof(Proyecto)));
            SqlMapper.SetTypeMap(typeof(Proyecto), mapper11);

        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddIdentity<User, Rol>()
                .AddRoleStore<RoleStore>()
                .AddUserStore<UserPasswordStore>()
                .AddUserManager<CustomUserManager>()
                .AddDefaultTokenProviders();

            services.AddScoped<IUserClaimsPrincipalFactory<User>, ApplicationClaimsIdentityFactory>();

            services.AddAuthentication(sharedOptions =>
            {
                sharedOptions.DefaultAuthenticateScheme = "Identity.Application";
                sharedOptions.DefaultSignInScheme = "Identity.Application";
                // sharedOptions.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
            });

            services.AddDataProtection()
                    .PersistKeysToFileSystem(new DirectoryInfo(@"/SIPRO"))
                    .SetApplicationName("SiproApp")
                    .DisableAutomaticKeyGeneration();

            services.ConfigureApplicationCookie(options => {
                options.Cookie.Name = ".AspNet.Identity.Application";
                options.Cookie.HttpOnly = true;
                options.Cookie.SameSite = SameSiteMode.None;
                options.Cookie.Path = "/";
                options.Events.OnRedirectToLogin = context =>
                {
                    if (context.Response.StatusCode == (int)HttpStatusCode.OK)
                    {
                        context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    }
                    else
                    {
                        context.Response.Redirect(context.RedirectUri);
                    }
                    return Task.CompletedTask;
                };

                options.Events.OnRedirectToAccessDenied = context =>
                {
                    if (context.Response.StatusCode == (int)HttpStatusCode.OK)
                    {
                        context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    }
                    else
                    {
                        context.Response.Redirect(context.RedirectUri);
                    }
                    return Task.CompletedTask;
                };
            });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("Subproductos - Visualizar",
                                  policy => policy.RequireClaim("sipro/permission", "Subproductos - Visualizar"));
                options.AddPolicy("Subproductos - Editar",
                                  policy => policy.RequireClaim("sipro/permission", "Subproductos - Editar"));
                options.AddPolicy("Subproductos - Eliminar",
                                  policy => policy.RequireClaim("sipro/permission", "Subproductos - Eliminar"));
                options.AddPolicy("Subproductos - Crear",
                                  policy => policy.RequireClaim("sipro/permission", "Subproductos - Crear"));
            });

            services.AddCors(options =>
            {
                options.AddPolicy("AllowAllHeaders",
                      builder =>
                      {
                          builder.AllowAnyOrigin()
                                 .AllowAnyHeader()
                                 .AllowCredentials()
                                 .AllowAnyMethod();
                      });
            });

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseAuthentication();
            app.UseCors("AllowAllHeaders");
            app.UseMvc();
        }
    }
}
