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

        [FunctionName("ProcessADTRoutedData")]
        public static async Task RunAsync([EventGridTrigger] EventGridEvent eventGridEvent, ILogger log)
        {
            DefaultAzureCredential cred = new DefaultAzureCredential();
            DigitalTwinsClient client = new DigitalTwinsClient(new Uri(adtInstanceUrl), cred);
            log.LogInformation($"ADT service client connection created.");
            log.LogInformation(eventGridEvent.Data.ToString());
            if (eventGridEvent != null && eventGridEvent.Data != null)
            {
                RootObject rootObject = JsonConvert.DeserializeObject<RootObject>(eventGridEvent.Data.ToString());
                log.LogInformation("rootObject" + JsonConvert.SerializeObject(rootObject, Formatting.Indented));
                Azure.JsonPatchDocument jsonPatchDocument = new Azure.JsonPatchDocument();
                switch (rootObject.Data.ModelId)
                {
                    case "dtmi:com:Cobot:Cobot;1":
                        jsonPatchDocument.AppendReplace("/ElapsedTime", rootObject.Data.Patch.Find(patch => patch.Path.Contains("/ElapsedTime")).Value);
                        await client.UpdateDigitalTwinAsync("TCobot", jsonPatchDocument);
                        break;
                    case "dtmi:com:Cobot:Payload;1":
                        jsonPatchDocument.AppendReplace("/Mass", rootObject.Data.Patch.Find(patch => patch.Path.Contains("/Mass")).Value);
                        jsonPatchDocument.AppendReplace("/CogX", rootObject.Data.Patch.Find(patch => patch.Path.Contains("/CogX")).Value);
                        jsonPatchDocument.AppendReplace("/CogY", rootObject.Data.Patch.Find(patch => patch.Path.Contains("/CogY")).Value);
                        jsonPatchDocument.AppendReplace("/CogZ", rootObject.Data.Patch.Find(patch => patch.Path.Contains("/CogZ")).Value);
                        await client.UpdateDigitalTwinAsync("TPayload", jsonPatchDocument);
                        break;
                    case "dtmi:com:Cobot:ControlBox;1":
                        jsonPatchDocument.AppendReplace("/Voltage", rootObject.Data.Patch.Find(patch => patch.Path.Contains("/Voltage")).Value);
                        await client.UpdateDigitalTwinAsync("TControlBox", jsonPatchDocument);
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
