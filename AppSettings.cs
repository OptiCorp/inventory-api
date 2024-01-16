public class AppSettings
{
    public string QueueConnectionString { get; set; } = "Endpoint=sb://servicebus-turbinsikker-prod.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=jsxc2wM5vV4rhtevLn921gUZCcs7eLEsg+ASbHwJEng=";
    
    public string TopicUserCreated { get; set; } = "user-created";
    public string TopicUserUpdated { get; set; } = "user-updated";
    public string TopicUserDeleted { get; set; } = "user-deleted";
    
    public string SubscriptionInventory { get; set; } = "inventory";
}
