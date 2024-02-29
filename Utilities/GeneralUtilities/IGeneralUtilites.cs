namespace Inventory.Utilities;

public interface IGeneralUtilities
{
    string? GetSecretValueFromKeyVault(string secretName);

}