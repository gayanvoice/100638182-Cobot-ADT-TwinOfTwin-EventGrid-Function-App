// Default URL for triggering event grid function in the local environment.
// http://localhost:7071/runtime/webhooks/EventGrid?functionName={functionname}
using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.EventGrid;
using Microsoft.Extensions.Logging;
using Azure.Messaging.EventGrid;
using Azure.DigitalTwins.Core;
using Azure.Identity;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace CobotAdtEventGridFunctionApp
{
    public static class ADTRoutedDataFunction
    {
        private static readonly string adtInstanceUrl = Environment.GetEnvironmentVariable("ADT_SERVICE_URL");

        [FunctionName("ProcessADTRoutedData")]
        public static void Run([EventGridTrigger] EventGridEvent eventGridEvent, ILogger log)
        {
            DefaultAzureCredential cred = new DefaultAzureCredential();
            DigitalTwinsClient client = new DigitalTwinsClient(new Uri(adtInstanceUrl), cred);
            log.LogInformation($"ADT service client connection created.");
            log.LogInformation(eventGridEvent.Data.ToString());
            if (eventGridEvent != null && eventGridEvent.Data != null)
            {
                JObject message = (JObject) JsonConvert.DeserializeObject(eventGridEvent.Data.ToString());

                log.LogInformation($"Updating Floor...");

                string twinId = eventGridEvent.Subject.ToString();
                log.LogInformation($"TwinId: {twinId}");

                string modelId = message["data"]["modelId"].ToString();
                log.LogInformation($"ModelId: {modelId}");
            }
        }
    }
}
