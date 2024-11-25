using StartFMS.EF;
using StartFMS.Entity;

namespace StartFMS.Backend.API.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddScopedForInterface<TInterface, TImplementation>(this IServiceCollection services)
             where TInterface : class
             where TImplementation : class, TInterface
        {
            services.AddScoped<TInterface>(provider =>
            {
                return ActivatorUtilities.CreateInstance<TImplementation>(provider,
                    provider.GetRequiredService<StartFmsBackendContext>(),
                    provider.GetRequiredService<ILogger<TImplementation>>());
            });
        }

        public static void AddScopedForInterface<TInterface, TImplementation>(this IServiceCollection services, params object[] parameters)
            where TInterface : class
            where TImplementation : class, TInterface
        {
            services.AddScoped<TInterface>(provider =>
            {
                // 將參數合併到提供者的 GetRequiredService 方法中
                var context = provider.GetRequiredService<StartFmsBackendContext>();
                var logger = provider.GetRequiredService<ILogger<TImplementation>>();

                // 確保 parameters 中的順序是正確的
                var allParameters = new object[] { context, logger }.Concat(parameters).ToArray();

                return ActivatorUtilities.CreateInstance<TImplementation>(provider, allParameters);
            });
        }

    }
}
