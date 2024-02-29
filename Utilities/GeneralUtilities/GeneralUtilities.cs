using Azure.Identity;
using Azure.Security.KeyVault.Secrets;

namespace Inventory.Utilities;

public class GeneralUtilities : IGeneralUtilities
{
    public string? GetSecretValueFromKeyVault(string secretName)
    {
        const string keyVaultUrl = "https://kvv2-turbinsikker-prod.vault.azure.net/";
        var credential = new DefaultAzureCredential();
        var client = new SecretClient(new Uri(keyVaultUrl), credential);
        var secret = client.GetSecret(secretName);
        return secret.Value.Value;
    }
}