using Confluent.Kafka;


var config = new ConsumerConfig
{
    BootstrapServers = "localhost:9092",
    GroupId = "ConsoleConsumer",
    AutoOffsetReset = AutoOffsetReset.Earliest,
    EnableAutoCommit = false
};

using (var consumer = new ConsumerBuilder<Ignore, string>(config).Build())
{
    consumer.Subscribe("Clientes");

    try
    {
        while (true)
        {
            var message = consumer.Consume();
            Console.WriteLine($"Mensaje recibido: {message.Value}");
        }
    }
    catch (OperationCanceledException)
    {
        // El consumidor se detuvo o se canceló
    }
    finally
    {
        consumer.Close();
    }
}

Console.WriteLine("Press any key to exit");
Console.ReadKey();
