using System;
using System.Collections.Generic;
using System.Text;

namespace TimeSeries.Common.Models
{
    public class Serie
    {
        public Serie()
        {
            Observables = new HashSet<Observable>();
        }
        public int SerieId { get; set; }
        public string Comment { get; set; }

        public virtual Country Country { get; set; }
        public virtual Indicator Indicator { get; set; }

        public virtual ICollection<Observable> Observables { get; set; }
    }
}
