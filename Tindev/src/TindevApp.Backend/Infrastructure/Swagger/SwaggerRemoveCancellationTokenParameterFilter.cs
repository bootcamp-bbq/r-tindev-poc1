using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Win32.SafeHandles;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace TindevApp.Backend.Infrastructure.Swagger
{
    public class SwaggerRemoveCancellationTokenParameterFilter : IOperationFilter
    {
        public void Apply(Operation operation, OperationFilterContext context)
        {
            context.ApiDescription.ParameterDescriptions
                .Where(pd =>
                    pd.ModelMetadata.ContainerType == typeof(CancellationToken) ||
                    pd.ModelMetadata.ContainerType == typeof(WaitHandle) ||
                    pd.ModelMetadata.ContainerType == typeof(SafeWaitHandle))
                .ToList()
                .ForEach(
                    pd =>
                    {
                        if (operation.Parameters != null)
                        {
                            var cancellationTokenParameter = operation.Parameters.Single(p => p.Name == pd.Name);
                            operation.Parameters.Remove(cancellationTokenParameter);
                        }
                    });
        }
    }
}
