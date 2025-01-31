using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AutoMapper;
using System.Text;
using System.Threading.Tasks;
using GeminiAdvancedAPI.Application.Interfaces;
using GeminiAdvancedAPI.Application.Services;

namespace GeminiAdvancedAPI.Application
{
	public static class DependencyInjection
	{
		public static IServiceCollection AddApplicationServices(this IServiceCollection services)
		{
			services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
			services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
			services.AddAutoMapper(Assembly.GetExecutingAssembly()); // AddAutoMapper satırını ekledik

            services.AddScoped<ITokenService, TokenService>();

            return services;
		}
	}
}
