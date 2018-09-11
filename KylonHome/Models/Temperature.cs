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

        [Display(Name ="设备名称")]
        public string DeviceName { get; set; }

        [Display(Name ="温度")]
        public float Temp { get; set; }

        [Display(Name = "湿度")]
        public float Humidity { get; set; }

        [Display(Name = "采集时间")]
        public DateTime AcquisitionTime { get; set; }

        [Display(Name = "备注")]
        public string Memo { get; set; }
    }
}
