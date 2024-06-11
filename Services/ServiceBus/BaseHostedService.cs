using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Persistence.ServiceBus;

public abstract class BaseHostedService : IHostedService
{
    public ServiceBusProcessor _processor;

    public ServiceBusClient _serviceBusClient;
    protected BaseHostedService(ServiceBusClient serviceBusClient, IServiceScopeFactory serviceScopFactory, ILogger logger, IConfiguration configuration)
    {
        _serviceBusClient = serviceBusClient;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _processor.ProcessMessageAsync += MessageHandler;
        _processor.ProcessErrorAsync += ErrorHandler;
        return _processor.StartProcessingAsync(cancellationToken);
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        if (_processor != null)
        {
            await _processor.StopProcessingAsync(cancellationToken);
            await _processor.DisposeAsync();
        }

        if (_serviceBusClient != null)
        {
            await _serviceBusClient.DisposeAsync();
        }

    }

    protected abstract Task MessageHandler(ProcessMessageEventArgs args);

    protected abstract Task ErrorHandler(ProcessErrorEventArgs args);



 

}