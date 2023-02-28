using EpilepsieDB.Models;

namespace EpilepsieDB.Web.API.APIModels
{
    public class RecordingApiDto
    {
        public int RecordingID { get; set; }

        public string RecordingNumber { get; set; }

        public PatientApiDto Patient { get; set; }
    }
}