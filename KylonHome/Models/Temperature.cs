using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace KylonHome.Models
{
    public class Temperature
    {
        public int ID { get; set; }

        [Display(Name ="設備名稱")]
        public string DeviceName { get; set; }

        [Display(Name ="溫度")]
        public float Temp { get; set; }

        [Display(Name = "濕度")]
        public float Humidity { get; set; }

        [Display(Name = "採集時間")]
        public DateTime AcquisitionTime { get; set; }

        [Display(Name = "備注")]
        public string Memo { get; set; }
    }
}
