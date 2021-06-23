namespace RA.PropertyRetriever
{
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.DependencyInjection.Extensions;
    using RA.PropertyRetriever.Services;
    using RA.PropertyRetriever.Services.Interfaces;

    public static class Extensions
    {
        /// <summary>
        /// Add LocalEnvironmentService dependency injection
        /// </summary>
        /// <param name="this">Service Collection</param>
        /// <returns>Service Collection with LocalEnvironmentService dependencies injected</returns>
        public static IServiceCollection AddLocalEnvironmentService(this IServiceCollection @this)
        {
            @this.TryAddSingleton<ILocalEnvironmentService, LocalEnvironmentService>();
            return @this;
        }

        /// <summary>
        /// Add PropertyRetrieverService dependency injection
        /// </summary>
        /// <param name="this">Service Collection</param>
        /// <returns>Service Collection with PropertyRetrieverService dependencies injected</returns>
        public static IServiceCollection AddPropertyRetrieverService(this IServiceCollection @this)
        {
            @this.AddLocalEnvironmentService();
            @this.TryAddSingleton<IPropertyRetrieverService, PropertyRetrieverService>();
            return @this;
        }
    }
}
