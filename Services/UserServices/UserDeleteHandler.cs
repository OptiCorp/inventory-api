using System.Text.Json;
using Azure.Messaging.ServiceBus;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Inventory.Models;
using Inventory.Models.DTO;

namespace Inventory.Services;

public class UserDeleteHandler(IOptions<AppSettings> appSettings, IServiceProvider serviceProvider)
    : BackgroundService
{
    private readonly AppSettings _appSettings = appSettings.Value ?? throw new ArgumentNullException(nameof(appSettings));
    private ServiceBusClient? _client;
    private ServiceBusProcessor? _processor;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        // Initialize the Service Bus client and processor.
        _client = new ServiceBusClient(_appSettings.QueueConnectionString);
        _processor = _client.CreateProcessor(_appSettings.TopicUserDeleted, _appSettings.SubscriptionInventory, new ServiceBusProcessorOptions());

        // Configure args handler and error handler.
        _processor.ProcessMessageAsync += MessageHandler;
        _processor.ProcessErrorAsync += ErrorHandler;

        // Start processing messages.
        await _processor.StartProcessingAsync(stoppingToken);

        Console.WriteLine($"{nameof(UserDeleteHandler)} service has started.");

        // Wait for a cancellation signal (e.g., Ctrl + C) or another stopping condition.
        await Task.Delay(Timeout.Infinite, stoppingToken);

        Console.WriteLine($"{nameof(UserDeleteHandler)} service has stopped.");

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

        var userBody = JsonSerializer.Deserialize<UserBusDeleteDto>(body);

        var scopedService = scope.ServiceProvider.GetRequiredService<InventoryDbContext>();

        switch (userBody?.DeleteMode)
        {
            case "Soft":
            {
                var user = await scopedService.User.FirstOrDefaultAsync(u => u.UmId == userBody.Id);
                if (user != null)
                {
                    user.Status = UserStatus.Deleted;
                }

                break;
            }
            case "Hard":
            {
                var user = await scopedService.User.FirstOrDefaultAsync(u => u.UmId == userBody.Id);
                if (user != null)
                {
                    scopedService.User.Remove(user);
                }

                break;
            }
        }

        await scopedService.SaveChangesAsync();
        await args.CompleteMessageAsync(args.Message);
    }

    private static Task ErrorHandler(ProcessErrorEventArgs args)
    {
        ArgumentNullException.ThrowIfNull(args);
        return Task.CompletedTask;
    }
}