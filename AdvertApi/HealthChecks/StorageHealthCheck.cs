using AdvertApi.Services;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AdvertApi.HealthChecks
{
    public class StorageHealthCheck : IHealthCheck
    {
        private readonly IAdvertStorage _storageService;

        public StorageHealthCheck(IAdvertStorage storageService)
        {
            _storageService = storageService;
        }
        //public async ValueTask<IHealthCheckResult> CheckAsync(CancellationToken cancellationToken = default)
        //{
        //    var isHealthy = await _storageService.CheckDBHealth();
        //    return HealthCheckResult.FromStatus(isHealthy ? CheckStatus.Healthy : CheckStatus.Unhealthy,"");
        //}

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            var isHealthy = await _storageService.CheckDBHealth();
            if (isHealthy)
                return HealthCheckResult.Healthy();

            return HealthCheckResult.Unhealthy();
        }
    }
}
