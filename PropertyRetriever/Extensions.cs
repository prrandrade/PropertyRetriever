namespace PropertyRetriever
{
    using Interfaces;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.DependencyInjection.Extensions;
    using Services;

    public static class Extensions
    {
        /// <summary>
        /// Add MyEnvironment dependency injection
        /// </summary>
        /// <param name="this">Service Collection</param>
        /// <returns>Service Collection with MyEnvironment dependencies injected</returns>
        public static IServiceCollection AddLocalEnvironment(this IServiceCollection @this)
        {
            @this.TryAddSingleton<ILocalEnvironment, LocalEnvironment>();
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
            @this.TryAddSingleton<IPropertyRetriever, PropertyRetriever>();
            return @this;
        }
    }
}
