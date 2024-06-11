namespace Inventory.Configuration;

public abstract class AppSettings
{
    public static string TopicUserCreated => "user-created";
    public static string TopicUserUpdated => "user-updated";
    public static string TopicUserDeleted => "user-deleted";

    public static string TopicItemEvent => "item-created";
    // public static string TopicItemUpdated => "item-updated";
    // public static string TopicItemDeleted => "item-deleted";

    public static string SubscriptionInventory => "inventory";
    public static string TopicChecklistTemplateEvent => "checklisttemplate-event";
}
