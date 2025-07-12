using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Serilog;
using Serilog.Sinks.File;
using System.Threading.Tasks;

namespace Hospital_Project.Helpers
{
    static class WriteLog
    {
        public static void ConfigureLogger()
        {
            string projectRoot = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory)!.Parent!.Parent!.Parent!.FullName;
            string logPath = Path.Combine(projectRoot, "logs", "log.txt");

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.File(
                    path: logPath,
                    rollingInterval: RollingInterval.Day,
                    outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}"
                )
                .CreateLogger();
        }
    }
}
