using EpilepsieDB.Authorization;
using EpilepsieDB.Models;
using EpilepsieDB.Services;
using EpilepsieDB.Source;
using EpilepsieDB.Web.API.APIModels;
using EpilepsieDB.Web.API.Extensions;
using EpilepsieDB.Web.Common;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EpilepsieDB.Web.API.Controllers
{
    [Authorize(Roles = RoleSet.AllowRead, AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class ScansController : ControllerBase
    {
        private readonly IScanService _scanService;

        public ScansController(IScanService scanService)
        {
            _scanService = scanService;
        }

        // GET: api/scans/getall
        [HttpGet("getall")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<ScanApiDto>>> GetScans()
        {
            var list = await _scanService.Get(includeProperties: "Recording,Recording.Patient");

            return Ok(list.ToDtos());
        }

        // GET: api/scans/ofpatient/5
        [HttpGet("ofpatient/{patientID}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<ScanApiDto>>> GetOfPatient(int patientID)
        {
            var list = await _scanService.Get(filter: s => s.Recording.Patient.ID == patientID, includeProperties: "Recording,Recording.Patient");

            return Ok(list.ToDtos());
        }

        // GET: api/scans/ofrecording/5
        [HttpGet("ofrecording/{recordingID}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<ScanApiDto>>> GetOfRecording(int recordingID)
        {
            var list = await _scanService.Get(filter: s => s.Recording.ID == recordingID, includeProperties: "Recording,Recording.Patient");

            return Ok(list.ToDtos());
        }

        // GET: api/scans/get/5
        [HttpGet("get/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ScanApiDto>> GetScan(int id)
        {
            var scan = (await _scanService.Get(filter: p => p.ID == id, includeProperties: "Recording,Recording.Patient")).FirstOrDefault();

            if (scan == null)
                return NotFound();

            return scan.ToDto();
        }

        // POST: api/scans/new
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("new")]
        [RequestSizeLimit(1024 * 1024 * 1024)]
        [Authorize(Roles = RoleSet.AllowCreateScan)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<ScanApiDto>> PostScan([FromForm]CreateScanDto dto)
        {
            var scan = new Scan()
            {
                RecordingID = dto.RecordingID.Value,
                ScanNumber = dto.ScanNumber,
            };

            IFileStream file = new FormFileProxy(dto.EdfFile);

            await _scanService.Create(scan, file);

            return CreatedAtAction(nameof(GetScan), new { id = scan.ID }, dto);
        }

        public class CreateScanDto
        {
            [Required]
            public int? RecordingID { get; set; }

            [Required]
            public string ScanNumber { get; set; }

            [Required]
            public IFormFile EdfFile { get; set; }
        }
    }
}
