// using System.Text.Json;
// using Azure.Messaging.ServiceBus;
// using Microsoft.EntityFrameworkCore;
// using Inventory.Models;
// using Inventory.Models.DTO;
// using Inventory.Configuration;
// using Inventory.Utilities;

// namespace Inventory.Services;

// public class UserDeleteHandler(IServiceProvider serviceProvider)
//     : BackgroundService
// {
//     private ServiceBusClient? _client;
//     private ServiceBusProcessor? _processor;

//     protected override async Task ExecuteAsync(CancellationToken stoppingToken)
//     {
//         using var scope = serviceProvider.CreateScope();
//         var scopedService = scope.ServiceProvider.GetRequiredService<IGeneralUtilities>();

//         // Initialize the Service Bus client and processor.
//         _client = new ServiceBusClient(scopedService.GetSecretValueFromKeyVault("inventory-send-sas"));
//         _processor = _client.CreateProcessor(AppSettings.TopicUserDeleted, AppSettings.SubscriptionInventory, new ServiceBusProcessorOptions());

//         // Configure args handler and error handler.
//         _processor.ProcessMessageAsync += MessageHandler;
//         _processor.ProcessErrorAsync += ErrorHandler;

//         // Start processing messages.
//         await _processor.StartProcessingAsync(stoppingToken);

//         Console.WriteLine($"{nameof(UserDeleteHandler)} service has started.");

//         // Wait for a cancellation signal (e.g., Ctrl + C) or another stopping condition.
//         await Task.Delay(Timeout.Infinite, stoppingToken);

//         Console.WriteLine($"{nameof(UserDeleteHandler)} service has stopped.");

//         // Stop processing messages and clean up resources.
//         await _processor.StopProcessingAsync(stoppingToken);
//         await _processor.DisposeAsync();
//         await _client.DisposeAsync();
//     }

//     private async Task MessageHandler(ProcessMessageEventArgs args)
//     {
//         using var scope = serviceProvider.CreateScope();
//         ArgumentNullException.ThrowIfNull(args);

//         var body = args.Message.Body.ToString();

//         var userBody = JsonSerializer.Deserialize<UserBusDeleteDto>(body);

//         var scopedService = scope.ServiceProvider.GetRequiredService<InventoryDbContext>();

//         switch (userBody?.DeleteMode)
//         {
//             case "Soft":
//                 {
//                     var user = await scopedService.User.FirstOrDefaultAsync(u => u.UmId == userBody.Id);
//                     if (user != null)
//                     {
//                         user.Status = UserStatus.Deleted;
//                     }

//                     break;
//                 }
//             case "Hard":
//                 {
//                     var user = await scopedService.User.FirstOrDefaultAsync(u => u.UmId == userBody.Id);
//                     if (user != null)
//                     {
//                         scopedService.User.Remove(user);
//                     }

//                     break;
//                 }
//         }

//         await scopedService.SaveChangesAsync();
//         await args.CompleteMessageAsync(args.Message);
//     }

//     private static Task ErrorHandler(ProcessErrorEventArgs args)
//     {
//         ArgumentNullException.ThrowIfNull(args);
//         return Task.CompletedTask;
//     }
// }