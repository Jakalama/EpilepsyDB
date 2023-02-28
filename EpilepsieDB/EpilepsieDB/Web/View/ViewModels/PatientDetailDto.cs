using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace EpilepsieDB.Web.View.ViewModels
{
    public class PatientDetailDto
    {
        public int PatientID { get; set; }

        public string Acronym { get; set; }

        public string MriImagePath { get; set; }

        public IEnumerable<RecordingDto> Recordings { get; set; }
    }
}
