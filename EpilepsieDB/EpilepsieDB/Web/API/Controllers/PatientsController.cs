using EpilepsieDB.Authorization;
using EpilepsieDB.Services;
using EpilepsieDB.Web.API.APIModels;
using EpilepsieDB.Web.API.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EpilepsieDB.Web.API.Controllers
{
    [Authorize(Roles = RoleSet.AllowRead, AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class PatientsController : ControllerBase
    {
        private readonly IPatientService _patientService;

        public PatientsController(
            IPatientService patientService)
        {
            _patientService = patientService;
        }

        // GET: api/patients/getall
        [HttpGet("getall")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<PatientApiDto>>> GetPatients()
        {
            var list = await _patientService.GetAll();

            return Ok(list.ToDtos());
        }


        // GET: api/patients/get/5
        [HttpGet("get/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<PatientApiDto>> GetPatient(int id)
        {
            var patient = (await _patientService.Get(filter: p => p.ID == id)).FirstOrDefault();

            if (patient == null)
                return NotFound();

            return patient.ToDto();
        }

    }
}
