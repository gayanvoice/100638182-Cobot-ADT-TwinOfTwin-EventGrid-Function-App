using System.Collections.Generic;

namespace CobotADTEventGridFunctionApp.Model
{
    public class Data
    {
        public string ModelId { get; set; }
        public List<Patch> Patch { get; set; }
    }
}