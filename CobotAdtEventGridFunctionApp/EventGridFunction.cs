using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.EventGrid;
using Microsoft.Extensions.Logging;
using Azure.Messaging.EventGrid;
using Azure.DigitalTwins.Core;
using Azure.Identity;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Threading.Tasks;
using CobotADTEventGridFunctionApp.Model;
using System.Collections.Generic;

namespace CobotADTEventGridFunctionApp
{
    public static class EventGridFunctionApp
    {
        private static readonly string adtInstanceUrl = Environment.GetEnvironmentVariable("ADT_SERVICE_URL");

        [FunctionName("RunEventGridTrigger")]
        public static void Run([EventGridTrigger] EventGridEvent eventGridEvent, ILogger log)
        {
            DefaultAzureCredential cred = new DefaultAzureCredential();
            DigitalTwinsClient client = new DigitalTwinsClient(new Uri(adtInstanceUrl), cred);
            log.LogInformation($"ADT service client connection created.");
            log.LogInformation(eventGridEvent.Data.ToString());
            if (eventGridEvent != null && eventGridEvent.Data != null)
            {
                JObject eventGridMessage = (JObject)JsonConvert.DeserializeObject(eventGridEvent.Data.ToString());
                RootObject rootObject = JsonConvert.DeserializeObject<RootObject>(eventGridEvent.Data.ToString());
                log.LogInformation("rootObject" + JsonConvert.SerializeObject(rootObject, Formatting.Indented));

                Azure.JsonPatchDocument jsonPatchDocument = new Azure.JsonPatchDocument();
                string deviceId = (string) eventGridMessage["systemProperties"]["iothub-connection-device-id"];
                string twinDeviceId = deviceId;
                switch (deviceId)
                {
                    case "Cobot":
                        twinDeviceId = "TCobot";
                        Patch patch = rootObject.Data.Patch.Find(patch => patch.Path.Contains("/ElapsedTime"));
                        jsonPatchDocument.AppendReplace("/ElapsedTime", patch.Value);
                        log.LogInformation("executed cobot" + deviceId + patch.Value);
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
