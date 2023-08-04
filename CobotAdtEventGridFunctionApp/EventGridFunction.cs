using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.EventGrid;
using Microsoft.Extensions.Logging;
using Azure.Messaging.EventGrid;
using Azure.DigitalTwins.Core;
using Azure.Identity;
using Newtonsoft.Json;
using System.Threading.Tasks;
using CobotADTEventGridFunctionApp.Model;

namespace CobotADTEventGridFunctionApp
{
    public static class EventGridFunctionApp
    {
        private static readonly string adtInstanceUrl = Environment.GetEnvironmentVariable("ADT_SERVICE_URL");

        [FunctionName("ProcessADTRoutedDataToTwinOfTwin")]
        public static async Task ProcessADTRoutedDataToTwinOfTwin([EventGridTrigger] EventGridEvent eventGridEvent, ILogger log)
        {
            DefaultAzureCredential cred = new DefaultAzureCredential();
            DigitalTwinsClient client = new DigitalTwinsClient(new Uri(adtInstanceUrl), cred);
            log.LogInformation(eventGridEvent.Data.ToString());
            if (eventGridEvent != null && eventGridEvent.Data != null)
            {
                RootObject rootObject = JsonConvert.DeserializeObject<RootObject>(eventGridEvent.Data.ToString());
                log.LogInformation(JsonConvert.SerializeObject(rootObject, Formatting.Indented));
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
                    case "dtmi:com:Cobot:JointLoad:Elbow;1":
                        jsonPatchDocument.AppendReplace("/Position", rootObject.Data.Patch.Find(patch => patch.Path.Contains("/Position")).Value);
                        jsonPatchDocument.AppendReplace("/Temperature", rootObject.Data.Patch.Find(patch => patch.Path.Contains("/Temperature")).Value);
                        jsonPatchDocument.AppendReplace("/Voltage", rootObject.Data.Patch.Find(patch => patch.Path.Contains("/Voltage")).Value);
                        jsonPatchDocument.AppendReplace("/X", rootObject.Data.Patch.Find(patch => patch.Path.Contains("/X")).Value);
                        jsonPatchDocument.AppendReplace("/Y", rootObject.Data.Patch.Find(patch => patch.Path.Contains("/Y")).Value);
                        jsonPatchDocument.AppendReplace("/Z", rootObject.Data.Patch.Find(patch => patch.Path.Contains("/Z")).Value);
                        await client.UpdateDigitalTwinAsync("TElbow", jsonPatchDocument);
                        break;
                    case "dtmi:com:Cobot:JointLoad:Base;1":
                        jsonPatchDocument.AppendReplace("/Position", rootObject.Data.Patch.Find(patch => patch.Path.Contains("/Position")).Value);
                        jsonPatchDocument.AppendReplace("/Temperature", rootObject.Data.Patch.Find(patch => patch.Path.Contains("/Temperature")).Value);
                        jsonPatchDocument.AppendReplace("/Voltage", rootObject.Data.Patch.Find(patch => patch.Path.Contains("/Voltage")).Value);
                        await client.UpdateDigitalTwinAsync("TBase", jsonPatchDocument);
                        break;
                    case "dtmi:com:Cobot:JointLoad:Shoulder;1":
                        jsonPatchDocument.AppendReplace("/Position", rootObject.Data.Patch.Find(patch => patch.Path.Contains("/Position")).Value);
                        jsonPatchDocument.AppendReplace("/Temperature", rootObject.Data.Patch.Find(patch => patch.Path.Contains("/Temperature")).Value);
                        jsonPatchDocument.AppendReplace("/Voltage", rootObject.Data.Patch.Find(patch => patch.Path.Contains("/Voltage")).Value);
                        await client.UpdateDigitalTwinAsync("TShoulder", jsonPatchDocument);
                        break;
                    case "dtmi:com:Cobot:JointLoad:Wrist1;1":
                        jsonPatchDocument.AppendReplace("/Position", rootObject.Data.Patch.Find(patch => patch.Path.Contains("/Position")).Value);
                        jsonPatchDocument.AppendReplace("/Temperature", rootObject.Data.Patch.Find(patch => patch.Path.Contains("/Temperature")).Value);
                        jsonPatchDocument.AppendReplace("/Voltage", rootObject.Data.Patch.Find(patch => patch.Path.Contains("/Voltage")).Value);
                        await client.UpdateDigitalTwinAsync("TWrist1", jsonPatchDocument);
                        break;
                    case "dtmi:com:Cobot:JointLoad:Wrist2;1":
                        jsonPatchDocument.AppendReplace("/Position", rootObject.Data.Patch.Find(patch => patch.Path.Contains("/Position")).Value);
                        jsonPatchDocument.AppendReplace("/Temperature", rootObject.Data.Patch.Find(patch => patch.Path.Contains("/Temperature")).Value);
                        jsonPatchDocument.AppendReplace("/Voltage", rootObject.Data.Patch.Find(patch => patch.Path.Contains("/Voltage")).Value);
                        await client.UpdateDigitalTwinAsync("TWrist2", jsonPatchDocument);
                        break;
                    case "dtmi:com:Cobot:JointLoad:Wrist3;1":
                        jsonPatchDocument.AppendReplace("/Position", rootObject.Data.Patch.Find(patch => patch.Path.Contains("/Position")).Value);
                        jsonPatchDocument.AppendReplace("/Temperature", rootObject.Data.Patch.Find(patch => patch.Path.Contains("/Temperature")).Value);
                        jsonPatchDocument.AppendReplace("/Voltage", rootObject.Data.Patch.Find(patch => patch.Path.Contains("/Voltage")).Value);
                        await client.UpdateDigitalTwinAsync("TWrist3", jsonPatchDocument);
                        break;
                    case "dtmi:com:Cobot:JointLoad:Tool;1":
                        jsonPatchDocument.AppendReplace("/Temperature", rootObject.Data.Patch.Find(patch => patch.Path.Contains("/Temperature")).Value);
                        jsonPatchDocument.AppendReplace("/Voltage", rootObject.Data.Patch.Find(patch => patch.Path.Contains("/Voltage")).Value);
                        jsonPatchDocument.AppendReplace("/X", rootObject.Data.Patch.Find(patch => patch.Path.Contains("/X")).Value);
                        jsonPatchDocument.AppendReplace("/Y", rootObject.Data.Patch.Find(patch => patch.Path.Contains("/Y")).Value);
                        jsonPatchDocument.AppendReplace("/Z", rootObject.Data.Patch.Find(patch => patch.Path.Contains("/Z")).Value);
                        jsonPatchDocument.AppendReplace("/Rx", rootObject.Data.Patch.Find(patch => patch.Path.Contains("/Rx")).Value);
                        jsonPatchDocument.AppendReplace("/Ry", rootObject.Data.Patch.Find(patch => patch.Path.Contains("/Ry")).Value);
                        jsonPatchDocument.AppendReplace("/Rz", rootObject.Data.Patch.Find(patch => patch.Path.Contains("/Rz")).Value);
                        await client.UpdateDigitalTwinAsync("TTool", jsonPatchDocument);
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
