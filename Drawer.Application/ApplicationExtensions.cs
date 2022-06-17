using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drawer.Application
{
    /// <summary>
    /// Application 프로젝트 확장 메소드
    /// </summary>
    public static class ApplicationExtensions
    {
        public static void AddApplicationDependency(this IServiceCollection services)
        {
            services.AddMediatR(typeof(ApplicationExtensions).Assembly);
        }
    }
}
