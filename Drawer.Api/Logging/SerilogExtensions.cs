using Serilog;

namespace Drawer.Api.Logging
{
    public static class SerilogExtensions
    {
        /// <summary>
        /// Serilog 로거를 생성하고 Logging Provider에 등록한다.
        /// </summary>
        /// <param name="builder"></param>
        public static void AddSerilog(this WebApplicationBuilder builder)
        {
            Log.Logger = new LoggerConfiguration()
              .ReadFrom.Configuration(builder.Configuration)
              .CreateLogger();

            builder.Host.UseSerilog();
        }
    }
}
