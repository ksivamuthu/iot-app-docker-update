using System;
namespace iot_app_docker
{
    public class AppUpdateRequest
    {
        public string Repository { get; set; }
        public string Version { get; set; }
    }
}
