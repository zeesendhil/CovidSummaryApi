using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CovidSummaryApi.Model
{
    public class errors
    {
        [Key]
        public Guid ID { get; set; }
        public string details { get; set; }
    }

    public class Summary
    {
        [Key]
        public Guid ID { get; set; }
        public string get { get; set; }
        public Parameters parameters { get; set; }
        public List<errors> errors { get; set; }
        public int results { get; set; }
        public List<Response> response { get; set; }

        internal object ToList()
        {
            throw new NotImplementedException();
        }
    }
}
