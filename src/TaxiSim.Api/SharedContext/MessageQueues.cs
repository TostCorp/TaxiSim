using RabbitMQ.Client;

using System.Text;
using System.Text.Json;

using TaxiSim.ServiceDefaults.Attributes;

namespace TaxiSim.Api.SharedContext;

[Lifetime(ServiceLifetime.Singleton)]
public sealed class MessageQueues : IMessageQueues
{
    public MessageQueues(IConnection connection)
    {
        MainChannel = connection.CreateModel();

        MainChannel.QueueDeclare(IMessageQueues.VehicleRequests);
    }

    private IModel MainChannel { get; }

    public void SendMessage<T>(string queueName, T message)
    {
        var serialized = JsonSerializer.Serialize(message);
        var byteArray = Encoding.UTF8.GetBytes(serialized);

        MainChannel.BasicPublish(string.Empty, queueName, body: byteArray);
    }

    public T? GetMessage<T>(string queueName)
    {
        var queueResult = MainChannel.BasicGet(queueName, false);
        if (queueResult is null)
        {
            return default;
        }

        return JsonSerializer.Deserialize<T>(queueResult.Body.Span);
    }
}

public interface IMessageQueues
{
    public static readonly string VehicleRequests = "vehicleRequests";

    public T? GetMessage<T>(string queueName);
    public void SendMessage<T>(string queueName, T message);
}
