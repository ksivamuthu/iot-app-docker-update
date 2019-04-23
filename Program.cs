using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Azure.Devices.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace iot_app_docker
{
    public class Program
    {
        public static void Main(string[] args)
        {
            RegisterDeviceClient();
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseUrls("http://0.0.0.0:5000")
                .UseStartup<Startup>();

        private static string connectionString = "HostName=siva-iot-hub.azure-devices.net;DeviceId=smart-device-1;SharedAccessKey=PIwuifCs+4mU1yJT26yf31iXN1A1ktoOEHKjswoPqmM=";
        private static DeviceClient deviceClient;

        private static void RegisterDeviceClient()
        {
            deviceClient = DeviceClient.CreateFromConnectionString(connectionString, TransportType.Mqtt);
            deviceClient.SendEventAsync(new Message(Encoding.UTF8.GetBytes("IoTAppStarted")));

            StartReceiveMessages();

            StartTemperatureTelemetry();
        }

        private static Timer temperatureSensorTimer;
        private static void StartTemperatureTelemetry()
        {
            temperatureSensorTimer = new Timer((state) => { SendTemperatureTelemetry(); }, null, 2000, 5 * 1000);
        }

        private static void SendTemperatureTelemetry()
        {
            var data = new { type = "temperature", value = 70.5 };
            var message = new Message(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(data)));
            deviceClient.SendEventAsync(message);
        }

        private static Timer receiveMessageTimer;

        private static void StartReceiveMessages()
        {
            receiveMessageTimer = new Timer(async (state) => {
                try
                {
                    Message receivedMessage = await deviceClient.ReceiveAsync();
                    if (receivedMessage == null) return;

                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("Received message: {0}",
                    Encoding.ASCII.GetString(receivedMessage.GetBytes()));
                    Console.ResetColor();

                    await deviceClient.CompleteAsync(receivedMessage);
                }catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }, null, 0, 1000);
        }
    }
}
