namespace RA.PropertyRetriever
{
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.DependencyInjection.Extensions;
    using RA.PropertyRetriever.Services;
    using RA.PropertyRetriever.Services.Interfaces;

    public static class Extensions
    {
        /// <summary>
        /// Add MyEnvironment dependency injection
        /// </summary>
        /// <param name="this">Service Collection</param>
        /// <returns>Service Collection with MyEnvironment dependencies injected</returns>
        public static IServiceCollection AddLocalEnvironment(this IServiceCollection @this)
        {
            @this.TryAddSingleton<ILocalEnvironmentService, LocalEnvironmentService>();
            return @this;
        }

        /// <summary>
        /// Add PropertyRetriever dependency injection
        /// </summary>
        /// <param name="this">Service Collection</param>
        /// <returns>Service Collection with PropertyRetriever dependencies injected</returns>
        public static IServiceCollection AddPropertyRetriever(this IServiceCollection @this)
        {
            @this.AddLocalEnvironment();
            @this.TryAddSingleton<IPropertyRetrieverService, PropertyRetrieverService>();
            return @this;
        }
    }
}
