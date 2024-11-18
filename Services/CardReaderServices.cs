using ArduinoWebAPwithUSB.Context;
using ArduinoWebAPwithUSB.Entities;
using System.IO.Ports;

namespace ArduinoWebAPwithUSB.Services
{
    public class CardReaderService : IHostedService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IConfiguration _configuration;
        private SerialPort _serialPort;
        private CancellationTokenSource _cancellationTokenSource;

        public CardReaderService(IServiceProvider serviceProvider, IConfiguration configuration)
        {
            _serviceProvider = serviceProvider;
            _configuration = configuration;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            Task.Run(() => StartCardReading(_cancellationTokenSource.Token));
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _serialPort?.Close();
            _cancellationTokenSource?.Cancel();
            return Task.CompletedTask;
        }

        private void StartCardReading(CancellationToken cancellationToken)
        {
            var portName = _configuration["SerialPort:PortName"];
            var baudRate = int.Parse(_configuration["SerialPort:BaudRate"]!);

            _serialPort = new SerialPort(portName, baudRate);
            _serialPort.Open();

            while (!cancellationToken.IsCancellationRequested)
            {
                if (_serialPort.BytesToRead > 0)
                {
                    string cardId = _serialPort.ReadLine().Trim();

                    if (!string.IsNullOrEmpty(cardId))
                    {
                        SaveCardId(cardId);
                        Console.WriteLine($"Card ID: {cardId} has been saved.");
                    }
                }

                Thread.Sleep(100); // Kis várakozási idő, hogy ne pörögjön túl gyorsan
            }
        }

        private void SaveCardId(string cardId)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                var card = new Card { CardId = cardId, CreatedAt = DateTime.UtcNow };
                dbContext.Cards.Add(card);
                dbContext.SaveChanges();
            }
        }
    }
}
