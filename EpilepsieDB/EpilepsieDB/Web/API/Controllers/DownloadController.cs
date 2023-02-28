using EpilepsieDB.Authorization;
using EpilepsieDB.Services;
using EpilepsieDB.Web.Common;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace EpilepsieDB.Web.API.Controllers
{
    [Authorize(Roles = RoleSet.AllowDownload, AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class DownloadController : ControllerBase
    {
        private readonly DownloadAdapter _downloadAdapter;

        public DownloadController(IDownloadService downloadService)
        {
            _downloadAdapter = new DownloadAdapter(downloadService);
        }

        // GET: api/download/scan/5
        [HttpGet("scan/{scanID}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DownloadScan(int? scanID)
        {
            return await _downloadAdapter.DownloadScan(scanID);
        }

        // GET: api/download/recording/5
        [HttpGet("recording/{recordingID}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DownloadScansByRecording(int? recordingID)
        {
            return await _downloadAdapter.DownloadScansByRecording(recordingID);
        }

        // GET: api/download/patient/5
        [HttpGet("patient/{patientID}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DownloadScansByPatient(int? patientID)
        {
            return await _downloadAdapter.DownloadPatient(patientID);
        }

        // GET: api/download/patient/nifti
        [HttpGet("patient/nifti/{patientID}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DownloadNifti(int? patientID)
        {
            return await _downloadAdapter.DownloadNifti(patientID);
        }
    }
}

