using SkyApm.Utilities.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;

namespace SkyApm.Diagnostics.SSRedis
{
    public static class SkyWalkingBuilderExtensions
    {
        public static SkyApmExtensions AddSSRedis(this SkyApmExtensions extensions)
        {
            if (extensions == null)
            {
                throw new ArgumentNullException(nameof(extensions));
            }
            extensions.Services.AddSingleton<ITracingDiagnosticProcessor, SSRedisTracingDiagnosticProcessor>();
            return extensions;
        }
    }
}
