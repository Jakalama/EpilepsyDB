using EpilepsieDB.Authorization;
using EpilepsieDB.Models;
using EpilepsieDB.Services;
using EpilepsieDB.Web.API.APIModels;
using EpilepsieDB.Web.API.Extensions;
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
    public class RecordingsController : ControllerBase
    {
        private readonly IRecordingService _recordingService;

        public RecordingsController(IRecordingService recordingService)
        {
            _recordingService = recordingService;
        }

        // GET: api/recordings/getall
        [HttpGet("getall")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<RecordingApiDto>>> GetRecordings()
        {
            var list = await _recordingService.Get(includeProperties: "Patient");

            return Ok(list.ToDtos());
        }

        // GET: api/recordings/ofpatient/5
        [HttpGet("ofPatient/{patientID}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<RecordingApiDto>>> GetOfPatient(int patientID)
        {
            var list = await _recordingService.Get(filter: r => r.PatientID == patientID, includeProperties: "Patient");

            return Ok(list.ToDtos());
        }

        // GET: api/recordings/get/5
        [HttpGet("get/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<RecordingApiDto>> GetRecording(int id)
        {
            var recording = (await _recordingService.Get(filter: p => p.ID == id, includeProperties: "Patient")).FirstOrDefault();

            if (recording == null)
                return NotFound();

            return recording.ToDto();
        }

        // POST: api/recordings/new
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("new")]
        [Authorize(Roles = RoleSet.AllowCreateScan)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<RecordingApiDto>> PostRecording(CreateRecordingDto dto)
        {
            var recording = new Recording()
            {
                PatientID = dto.PatientID.Value,
                RecordingNumber = dto.RecordingNumber
            };

            await _recordingService.Create(recording);

            return CreatedAtAction(nameof(GetRecording), new { id = recording.ID }, dto);
        }

        public class CreateRecordingDto
        {
            [Required]
            public int? PatientID { get; set; }

            [Required]
            public string RecordingNumber { get; set; }
        }
    }
}
