using EpilepsieDB.Authorization;
using EpilepsieDB.Services;
using EpilepsieDB.Web.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace EpilepsieDB.Web.View.Controllers
{
    [Controller]
    [Authorize(Roles = RoleSet.AllowDownload)]
    [ApiExplorerSettings(IgnoreApi = true)]
    [Route("[controller]/[action]", Order = 0)]
    public class DownloadsController : Controller
    {
        private readonly DownloadAdapter _downloadAdapter;

        public DownloadsController(
            IDownloadService downloadService)
        {
            _downloadAdapter = new DownloadAdapter(downloadService);
        }

        public async Task<IActionResult> DownloadScan(int? scanId)
        {
            return await _downloadAdapter.DownloadScan(scanId);
        }

        public async Task<IActionResult> DownloadScansByRecording(int? recordingID)
        {
            return await _downloadAdapter.DownloadScansByRecording(recordingID);
        }

        public async Task<IActionResult> DownloadPatient(int? patientID)
        {
            return await _downloadAdapter.DownloadPatient(patientID);
        }

        public async Task<IActionResult> DownloadNifti(int? patientID)
        {
            return await _downloadAdapter.DownloadNifti(patientID);
        }
    }
}
