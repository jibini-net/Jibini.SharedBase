using Jibini.SharedBase.Data.Models;
using Microsoft.Extensions.Options;
using System.Reflection;

namespace Jibini.SharedBase.Middleware;

public static class ServiceApiRoutingDelegate
{
    public static readonly Func<HttpContext, Task<object?>> Handler = async (HttpContext e) =>
    {
        // Namespaces must be configured in order to enable
        var _config = e.RequestServices.GetService<IOptions<ServiceApiConfiguration>>();
        if (_config?.Value is null)
        {
            throw new Exception("Configure API access to the service collection at startup");
        }
        var config = _config.Value;

        /*
         * Searches the service collection according to the configuration,
         * finding any service candidates by interface name.
         */
        var resolveService = (IServiceProvider sp, string name) =>
        {
            foreach (var serviceNamespace in config.SearchNamespaces)
            {
                var serviceInterface = typeof(Program).Assembly.GetType($"{serviceNamespace}.{name}");
                if (serviceInterface is null)
                {
                    continue;
                }
                var service = sp.GetService(serviceInterface);
                if (service is not null)
                {
                    return (serviceInterface, service);
                }
            }
            return (null, null);
        };

        /*
         * Attempts to find a function which handles the named method, applying
         * rules defined within the service interface's attributes.
         */
        var resolveAction = (Type serviceInterface, object? service, string name, string method) =>
        {
            var interfaceAction = serviceInterface.GetMethod(name);
            var typeAction = service!.GetType().GetMethod(name);
            if (interfaceAction is null || typeAction is null)
            {
                return (null, null);
            }
            var attr = interfaceAction.GetCustomAttributes<ApiActionAttribute>()
                    .Cast<ApiActionAttribute>()
                    .Where((it) => it.Method.Equals(method, StringComparison.InvariantCultureIgnoreCase))
                    .FirstOrDefault();
            if (attr is null)
            {
                return (null, null);
            }
            return (interfaceAction, typeAction);
        };

        /*
         * Creates a trivial listing of exposed actions under a given service.
         */
        var generateListing = (Type serviceInterface, object service, string basePath) =>
        {
            var allAttrs = serviceInterface.GetMethods()
                    .SelectMany((func) => func.GetCustomAttributes<ApiActionAttribute>()
                        .Select((action) => (func, action)))
                    .ToList();
            return (from it in allAttrs
                    select new
                    {
                        Action = it.func.Name,
                        it.action.Method,
                        FullPath = $"{basePath.TrimEnd('/')}/{it.func.Name}"
                    }).ToList();
        };

        {
            // Path format: "/{IService}/{Action?}/{Subpath**}"
            var path = e.Request.Path.ToString().TrimEnd('/').Split("/", 5);
            if (path.Length < 3)
            {
                e.Response.StatusCode = StatusCodes.Status400BadRequest;
                return null;
            }

            var (serviceInterface, service) = resolveService(e.RequestServices, path[2]);
            if (serviceInterface is null || service is null)
            {
                e.Response.StatusCode = StatusCodes.Status404NotFound;
                return null;
            }
            // Exit early with action listing if no action name is provided
            if (path.Length == 3)
            {
                var basePath = $"{e.Request.Scheme}://{e.Request.Host}/api/{path[2]}/{e.Request.PathBase}";
                return generateListing(serviceInterface, service, basePath);
            }

            var (interfaceAction, typeAction) = resolveAction(serviceInterface, service, path[3], e.Request.Method);
            if (interfaceAction is null || typeAction is null)
            {
                e.Response.StatusCode = StatusCodes.Status404NotFound;
                return null;
            }

            // Execute action and unwrap result if the result is a Task
            var resultData = typeAction!.Invoke(service, Array.Empty<object>());
            if (resultData is Task)
            {
                // The Task result type is unknown, but there should be one
                await ((Task)resultData);
                resultData = (resultData as dynamic).Result;
            }
            return resultData;
        }
    };
}
