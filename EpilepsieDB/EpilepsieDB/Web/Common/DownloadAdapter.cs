using EpilepsieDB.Services;
using EpilepsieDB.Source;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace EpilepsieDB.Web.Common
{
    public class DownloadAdapter
    {
        private readonly IDownloadService _downloadService;

        public DownloadAdapter(IDownloadService downloadService)
        {
            _downloadService = downloadService;
        }

        public async Task<IActionResult> DownloadScan(int? scanId)
        {
            if (scanId == null)
                return new JsonResult(new { success = false, message = "Scan not found!" });

            FileRequest file = await _downloadService.GetScanFile(scanId.Value);

            if (file == null)
                return new JsonResult(new { success = false, message = "Scan not found!" });

            return new FileContentResult(file.Data, "application/edf") { FileDownloadName = file.Name };
        }

        public async Task<IActionResult> DownloadScansByRecording(int? recordingID)
        {
            if (recordingID == null)
                return new JsonResult(new { success = false, message = "Recording not found!" });

            FileRequest file = await _downloadService.GetScanFilesByRecording(recordingID.Value);

            if (file == null)
                return new JsonResult(new { success = false, message = "Recording not found!" });

            return new FileContentResult(file.Data, "application/edf") { FileDownloadName = file.Name };
        }

        public async Task<IActionResult> DownloadPatient(int? patientID)
        {
            if (patientID == null)
                return new JsonResult(new { success = false, message = "Patient not found!" });

            FileRequest file = await _downloadService.GetFilesFromPatient(patientID.Value);

            if (file == null)
                return new JsonResult(new { success = false, message = "Patient not found!" });

            return new FileContentResult(file.Data, "application/edf") { FileDownloadName = file.Name };
        }

        public async Task<IActionResult> DownloadNifti(int? patientID)
        {
            if (patientID == null)
                return new JsonResult(new { success = false, message = "Patient not found!" });

            FileRequest file = await _downloadService.GetNiftiFile(patientID.Value);

            if (file == null)
                return new JsonResult(new { success = false, message = "Patient not found!" });

            return new FileContentResult(file.Data, "application/nii") { FileDownloadName = file.Name };
        }

    }
}
