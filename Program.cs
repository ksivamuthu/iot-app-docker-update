using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Docker.DotNet;
using Docker.DotNet.Models;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Azure.Devices.Client;
using Newtonsoft.Json;

namespace iot_app_docker
{
    public class Program
    {
        private static readonly DockerClient _dockerClient = new DockerClientConfiguration(new Uri("unix:///var/run/docker.sock")).CreateClient();

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
                                     
                    var jsonStr = Encoding.ASCII.GetString(receivedMessage.GetBytes());

                    var req = JsonConvert.DeserializeObject<AppUpdateRequest>(jsonStr);

                    await DownloadDockerImageLocallyAsync(req);

                    await TagDockerImageLocallyAsync(req);

                    await deviceClient.CompleteAsync(receivedMessage);

                }catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }, null, 0, 1000);
        }

        private static async Task DownloadDockerImageLocallyAsync(AppUpdateRequest request)
        {
            await _dockerClient.Images.CreateImageAsync(
                    new ImagesCreateParameters
                    {
                        FromSrc = "ksivamuthu.azurecr.io",
                        Repo = request.Repository,
                        Tag = request.Version
                    },
                    new AuthConfig
                    {
                        Username = "ksivamuthu",
                        Password = "1+HZ3sJQDxeGw6DPouNh7MpnQL+XQAWp"
                    }, new DockerImageDownloadProgress(),  CancellationToken.None);
        }

        private static async Task TagDockerImageLocallyAsync(AppUpdateRequest request)
        {
            await _dockerClient.Images.TagImageAsync($"{request.Repository}:{request.Version}", new ImageTagParameters()
            {
                RepositoryName = request.Repository,
                Tag = "local"
            }, CancellationToken.None);
        }
    }


    public class DockerImageDownloadProgress : IProgress<JSONMessage>
    {
        public void Report(JSONMessage value)
        {
            Console.WriteLine(value.Status);
        }
    }
}

