using Newtonsoft.Json;
using System.Collections.Generic;

namespace SparkDotNet.Models
{
    public class PaginationResult<T>
    {
        public List<T> Items { get; set; }

        public Links Links { get; set; }

        public override string ToString() => JsonConvert.SerializeObject(this);
    }
}