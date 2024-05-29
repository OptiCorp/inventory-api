using System.Text.Json;
using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using Inventory.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.ServiceBus
{
    public class ServiceBusChecklistTemplateProcessor : BackgroundService
    {
        private readonly ILogger<ServiceBusChecklistTemplateProcessor> _logger;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly ServiceBusProcessor _processor;
        private readonly InventoryDbContext _inventoryDbContext;

        public ServiceBusChecklistTemplateProcessor(ServiceBusClient serviceBusClient, IServiceScopeFactory serviceScopeFactory, ILogger<ServiceBusChecklistTemplateProcessor> logger, IConfiguration configuration, InventoryDbContext inventoryDbContext)
        {
            _inventoryDbContext = inventoryDbContext;
            var topicName = configuration["ServiceBus:TopicChecklistEvent"] ?? throw new Exception("Missing topic name in configuration");
            var subscriptionName = configuration["ServiceBus:SubscriptionName"] ?? throw new Exception("Missing subscription name in configuration");

            _processor = serviceBusClient.CreateProcessor(topicName, subscriptionName, new ServiceBusProcessorOptions());
            _processor.ProcessMessageAsync += MessageHandler;
            _processor.ProcessErrorAsync += ErrorHandler;
            _logger = logger;
            _serviceScopeFactory = serviceScopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await _processor.StartProcessingAsync(stoppingToken);
        }

        private async Task MessageHandler(ProcessMessageEventArgs args)
        {
            try
            {
                var checklistTemplateEvent = JsonSerializer.Deserialize<ChecklistTemplateEvent>(args.Message.Body);
                if (checklistTemplateEvent == null)
                {
                    throw new Exception("Failed to deserialize checklistTemplateEvent");
                }

                _logger.LogInformation($"Received ChecklistTemplateEvent with ChecklistTemplateId: {checklistTemplateEvent.ChecklistTemplateId}, ItemTemplateId: {checklistTemplateEvent.ItemTemplateId}");


                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var itemTemplate = await _inventoryDbContext.ItemTemplates.FirstOrDefaultAsync(it => it.Id == checklistTemplateEvent.ItemTemplateId);

                    if (itemTemplate != null)
                    {
                        // TODO: fix
                        itemTemplate.ChecklistTemplateId = checklistTemplateEvent.ChecklistTemplateId;
                        await _inventoryDbContext.SaveChangesAsync();
                    }
                    else
                    {
                        _logger.LogWarning($"ID {checklistTemplateEvent.ItemTemplateId} not found");
                    }
                }

                await args.CompleteMessageAsync(args.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError($"failed to update {ex.Message}");
                await args.AbandonMessageAsync(args.Message);
            }
        }


        private Task ErrorHandler(ProcessErrorEventArgs args)
        {
            _logger.LogError($"messagehandler error: {args.Exception}.");
            return Task.CompletedTask;
        }

        public class ChecklistTemplateEvent
        {
            public string? ItemTemplateId { get; set; }
            public Guid ChecklistTemplateId { get; set; }
        }
    }
}
