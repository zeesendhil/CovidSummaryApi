using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CovidSummaryApi.Model
{
    
    public class Parameters
    {
        [Key]
        public Guid ID { get; set; }
        public string country { get; set; }
        public string day { get; set; }
    }

    public class Cases
    {
        [Key]
        public Guid ID { get; set; }
        public string @new { get; set; }
        public int active { get; set; }
        public int critical { get; set; }
        public int recovered { get; set; }
        public string _1M_pop { get; set; }
        public int total { get; set; }
    }

    public class Deaths
    {
        [Key]
        public Guid ID { get; set; }
        public string @new { get; set; }
        public string _1M_pop { get; set; }
        public int total { get; set; }
    }

    public class Tests
    {
        [Key]
        public Guid ID { get; set; }
        public string _1M_pop { get; set; }
        public int total { get; set; }
    }

    public class Response
    {
        [Key]
        public Guid ID { get; set; }
        public string continent { get; set; }
        public string country { get; set; }
        public int population { get; set; }
        public Cases cases { get; set; }
        public Deaths deaths { get; set; }
        public Tests tests { get; set; }
        public string day { get; set; }
        public DateTime time { get; set; }
    }
}
