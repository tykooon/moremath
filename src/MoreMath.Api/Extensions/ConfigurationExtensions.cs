using Microsoft.AspNetCore.Mvc;

namespace MoreMath.Api.Extensions;

public static class ConfigurationExtensions
{
    public static ConfigurationManager AddSecrets(this ConfigurationManager configBuilder)
    {
        configBuilder.AddEnvironmentVariables(prefix: "Moremath_");
        var path = configBuilder["Vault:Path"];
        if (path is null)
        {
            Console.WriteLine("Vault path is not specified");
        }
        else
        {
            configBuilder.AddJsonFile(path, optional: true, reloadOnChange: true);
        }

        return configBuilder;
    }




}
