using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EpilepsieDB.Web.View.ViewModels
{
    public class ScanDetailDto
    {
        public int ScanID { get; set; }

        public string ScanNumber { get; set; }

        public string PatientAcronym { get; set; }

        public string RecordingNumber { get; set; }

        public string Version { get; set; }

        public string PatientInfo { get; set; }

        public string RecordInfo { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Start Date")]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        public DateTime StartDate { get; set; }

        public int NumberOfRecords { get; set; }

        public float DurationOfDataRecord { get; set; }

        public int NumberOfSignals { get; set; }

        public int RecordingID { get; set; }

        public string Labels { get; set; }

        public string TransducerTypes { get; set; }

        public string PhysicalDimensions { get; set; }

        public IEnumerable<ElectrodeDetail> Electrodes{ get; set; }

        public IEnumerable<BlockDetail> Blocks { get; set; }
    }

    public class ElectrodeDetail
    {
        public int ElectrodeNumber { get; set; }
        public string Label { get; set; }
        public string Type { get; set; }
        public float Gain { get; set; }
        public int Samples { get; set; }
        public float SamplingRate { get; set; }
    }

    public class BlockDetail
    {
        public int BlockNumber { get; set; }
        
        public float Duration { get; set; }
        public int NumChannels { get; set; }

        [DataType(DataType.DateTime)]
        [Display(Name = "Start Time")]
        [DisplayFormat(DataFormatString = "{hh:mm:ss:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        public DateTime Starttime { get; set; }

        [DataType(DataType.DateTime)]
        [Display(Name = "End Time")]
        [DisplayFormat(DataFormatString = "{hh:mm:ss:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        public DateTime Endtime { get; set; }
        public int Gap { get; set; }
    }
}
