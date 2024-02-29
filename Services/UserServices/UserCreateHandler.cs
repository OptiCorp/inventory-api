using System.Text.Json;
using Azure.Messaging.ServiceBus;
using Inventory.Models;
using Inventory.Models.DTO;
using Inventory.Configuration;
using Inventory.Utilities;

namespace Inventory.Services;

public class UserCreateHandler(IServiceProvider serviceProvider)
    : BackgroundService
{
    private ServiceBusClient? _client;
    private ServiceBusProcessor? _processor;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var scope = serviceProvider.CreateScope();
        var scopedService = scope.ServiceProvider.GetRequiredService<IGeneralUtilities>();

        // Initialize the Service Bus client and processor.
        _client = new ServiceBusClient(scopedService.GetSecretValueFromKeyVault("Sb-connection-string"));
        _processor = _client.CreateProcessor(AppSettings.TopicUserCreated, AppSettings.SubscriptionInventory, new ServiceBusProcessorOptions());

        // Configure args handler and error handler.
        _processor.ProcessMessageAsync += MessageHandler;
        _processor.ProcessErrorAsync += ErrorHandler;

        // Start processing messages.
        await _processor.StartProcessingAsync(stoppingToken);

        Console.WriteLine($"{nameof(UserCreateHandler)} service has started.");

        // Wait for a cancellation signal (e.g., Ctrl + C) or another stopping condition.
        await Task.Delay(Timeout.Infinite, stoppingToken);

        Console.WriteLine($"{nameof(UserCreateHandler)} service has stopped.");

        // Stop processing messages and clean up resources.
        await _processor.StopProcessingAsync(stoppingToken);
        await _processor.DisposeAsync();
        await _client.DisposeAsync();
    }

    private async Task MessageHandler(ProcessMessageEventArgs args)
    {
        using var scope = serviceProvider.CreateScope();
        ArgumentNullException.ThrowIfNull(args);

        var body = args.Message.Body.ToString();
        var userBody = JsonSerializer.Deserialize<UserBusCreateDto>(body);

        var scopedService = scope.ServiceProvider.GetRequiredService<InventoryDbContext>();

        var user = new User
        {
            UmId = userBody?.Id,
            AzureAdUserId = userBody?.AzureAdUserId,
            Username = userBody?.Username,
            FirstName = userBody?.FirstName,
            LastName = userBody?.LastName,
            Email = userBody?.Email,
            UserRole = userBody?.UserRole,
            CreatedDate = userBody?.CreatedDate,
            Status = Enum.Parse<UserStatus>(userBody?.Status!)
        };
        await scopedService.User.AddAsync(user);

        await scopedService.SaveChangesAsync();
        await args.CompleteMessageAsync(args.Message);
    }

    private static Task ErrorHandler(ProcessErrorEventArgs args)
    {
        ArgumentNullException.ThrowIfNull(args);
        return Task.CompletedTask;
    }
}