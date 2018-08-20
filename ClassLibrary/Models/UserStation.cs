using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ClassLibrary
{
    public class UserStation
    {
        public int Id { get; set; }
        public int StationId { get; set; }
        [JsonIgnore]
        public Station Station { get; set; }
        public string Description { get; set; }
    }
}
