using System.Collections.Generic;

namespace FuelTracker.ApiModels.ManufacturerApiModels
{
    public class PostManufacturer
    {
        public string Name { get; set; }
        public IEnumerable<string> ModelsNames{ get; set; }
    }
}
