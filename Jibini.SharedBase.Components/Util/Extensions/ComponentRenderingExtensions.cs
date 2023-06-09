using BlazorTemplater;
using Microsoft.AspNetCore.Components;
using System.Linq.Expressions;

namespace Jibini.SharedBase.Util.Extensions;

/// <summary>
/// Any class or component implementing this interface can be rendered to HTML
/// through the component renderer. Allows limiting rendering to components
/// which take into account the possibility of being cloned, or are intended to
/// be used out of context as templated content.
/// </summary>
public interface IRenderable<TSelf>
{ }

public static class ComponentRenderingExtensions
{
    /// <summary>
    /// Creates a duplicate component to render it to HTML, returning the outer
    /// HTML content. Components do not need to implement the method.
    /// </summary>
    public static string ToHtml<T>(this IRenderable<T> it, IServiceProvider? serviceProvider = null)
        where T : IComponent
    {
        var renderer = new ComponentRenderer<T>();
        if (serviceProvider is not null)
        {
            renderer.AddServiceProvider(serviceProvider);
        }
        var fields = typeof(T).GetProperties()
            .ToList()
            .FindAll((it) => it.GetCustomAttributes(typeof(ParameterAttribute), true).Any());
        var paramInfo = Expression.Parameter(typeof(T));

        // Copy every field which can be written via renderer setter call
        foreach (var field in fields)
        {
            var value = field.GetValue(it);
            // Blazor Templater does not support setting parameters to null
            if (value is not null)
            {
                var access = Expression.MakeMemberAccess(paramInfo, field);
                var expr = Expression.Lambda<Func<T, object?>>(access, paramInfo);

                renderer.Set(expr, value);
            }
        }

        return renderer.Render();
    }
}
