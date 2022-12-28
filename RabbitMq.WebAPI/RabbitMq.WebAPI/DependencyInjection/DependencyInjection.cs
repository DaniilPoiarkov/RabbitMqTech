using System.Reflection;

namespace RabbitMq.WebAPI.DependencyInjection
{
    public static class DependencyInjection
    {
        public static IServiceCollection ApplyServiceInstallers(
            this IServiceCollection services,
            IConfiguration configuration,
            params Assembly[] assemblies)
        {
            var installers = assemblies
                .SelectMany(a => a.DefinedTypes)
                .Where(IsAssignableToType<IServiceInstaller>)
                .Select(Activator.CreateInstance)
                .Cast<IServiceInstaller>();

            foreach(var installer in installers)
                installer.InstallService(services, configuration);

            return services;
        }

        private static bool IsAssignableToType<T>(TypeInfo typeInfo) =>
                typeof(T).IsAssignableFrom(typeInfo) &&
                !typeInfo.IsAbstract &&
                !typeInfo.IsInterface;
    }
}
