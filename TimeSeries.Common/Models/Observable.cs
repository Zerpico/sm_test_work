using System;
using System.Collections.Generic;
using System.Text;

namespace TimeSeries.Common.Models
{
    public class Observable
    {
        public int ObservableId { get; set; }       
        public DateTime Time { get; set; }
        public double ObservableValue { get; set; }

        
        public virtual Serie Serie { get; set; }
    }
}
