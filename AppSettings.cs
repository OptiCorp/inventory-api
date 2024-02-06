namespace Inventory.Configuration;

public abstract class AppSettings
{
    public static string QueueConnectionString => "Endpoint=sb://servicebus-turbinsikker-prod.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=jsxc2wM5vV4rhtevLn921gUZCcs7eLEsg+ASbHwJEng=";

    public static string TopicUserCreated => "user-created";
    public static string TopicUserUpdated => "user-updated";
    public static string TopicUserDeleted => "user-deleted";

    public static string SubscriptionInventory => "inventory";
}
