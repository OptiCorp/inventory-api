using System.Text.Json;
using Azure.Messaging.ServiceBus;
using Microsoft.EntityFrameworkCore;
using Inventory.Models;
using Inventory.Models.DTO;
using Inventory.Configuration;

namespace Inventory.Services;

public class UserUpdateHandler(IServiceProvider serviceProvider)
    : BackgroundService
{
    private ServiceBusClient? _client;
    private ServiceBusProcessor? _processor;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        // Initialize the Service Bus client and processor.
        _client = new ServiceBusClient(AppSettings.QueueConnectionString);
        _processor = _client.CreateProcessor(AppSettings.TopicUserUpdated, AppSettings.SubscriptionInventory, new ServiceBusProcessorOptions());

        // Configure args handler and error handler.
        _processor.ProcessMessageAsync += MessageHandler;
        _processor.ProcessErrorAsync += ErrorHandler;

        // Start processing messages.
        await _processor.StartProcessingAsync(stoppingToken);

        Console.WriteLine($"{nameof(UserUpdateHandler)} service has started.");

        // Wait for a cancellation signal (e.g., Ctrl + C) or another stopping condition.
        await Task.Delay(Timeout.Infinite, stoppingToken);

        Console.WriteLine($"{nameof(UserUpdateHandler)} service has stopped.");

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
        var updatedUserDto = JsonSerializer.Deserialize<UserBusUpdateDto>(body);

        Console.WriteLine(updatedUserDto?.Username);

        var scopedService = scope.ServiceProvider.GetRequiredService<InventoryDbContext>();

        var user = await scopedService.User.FirstOrDefaultAsync(u => updatedUserDto != null && u.UmId == updatedUserDto.Id);
        if (user != null)
        {
            if (updatedUserDto?.Username != null)
                user.Username = updatedUserDto.Username;
            if (updatedUserDto?.FirstName != null)
                user.FirstName = updatedUserDto.FirstName;
            if (updatedUserDto?.LastName != null)
                user.LastName = updatedUserDto.LastName;
            if (updatedUserDto?.Email != null)
                user.Email = updatedUserDto.Email;
            if (updatedUserDto?.UserRole != null)
                user.UserRole = updatedUserDto.UserRole;
            if (updatedUserDto?.AzureAdUserId != null)
                user.AzureAdUserId = updatedUserDto.AzureAdUserId;
            if (updatedUserDto?.Status != null)
            {
                var status = updatedUserDto.Status.ToLower();
                user.Status = status switch
                {
                    "active" => UserStatus.Active,
                    "disabled" => UserStatus.Disabled,
                    "deleted" => UserStatus.Deleted,
                    _ => user.Status
                };
            }
        }

        if (user != null)
            user.UpdatedDate = TimeZoneInfo.ConvertTime(DateTime.Now,
                TimeZoneInfo.FindSystemTimeZoneById("Central European Standard Time"));
        await scopedService.SaveChangesAsync();
        await args.CompleteMessageAsync(args.Message);
    }

    private static Task ErrorHandler(ProcessErrorEventArgs args)
    {
        ArgumentNullException.ThrowIfNull(args);
        return Task.CompletedTask;
    }
}