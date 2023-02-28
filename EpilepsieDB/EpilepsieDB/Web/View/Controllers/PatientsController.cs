using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using EpilepsieDB.Models;
using EpilepsieDB.Services;
using EpilepsieDB.Web.View.Extensions;
using System.Collections.Generic;
using EpilepsieDB.Web.View.ViewModels;
using Microsoft.AspNetCore.Authorization;
using EpilepsieDB.Source;
using EpilepsieDB.Web.Common;
using Microsoft.AspNetCore.Hosting;
using EpilepsieDB.Authorization;

namespace EpilepsieDB.Web.View.Controllers
{
    [Controller]
    [Authorize(Roles = RoleSet.AllowRead)]
    [ApiExplorerSettings(IgnoreApi = true)]
    [Route("[controller]/[action]", Order = 0)]
    public class PatientsController : Controller
    {
        private readonly IPatientService _patientService;
        private readonly IRecordingService _recordingService;

        public PatientsController(
            IPatientService patientService,
            IRecordingService recordingService)
        {
            _patientService = patientService;
            _recordingService = recordingService;
        }

        // GET: Patients
        public async Task<IActionResult> Index()
        {
            return View((await _patientService.GetAll()).ToDtos());
        }

        // GET: Patients/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var patient = await _patientService.Get(id.Value);

            if (patient == null)
                return NotFound();

            IEnumerable<RecordingDto> recordings = (await _recordingService.Get(filter: r => r.PatientID == id.Value)).ToDtos();

            var dto = patient.ToDetailDto();
            dto.Recordings = recordings;

            return View(dto);
        }

        // GET: Patients/Create
        [Authorize(Roles = RoleSet.AllowCreatePatient)]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Patients/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [RequestSizeLimit(100000000)]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = RoleSet.AllowCreatePatient)]
        public async Task<IActionResult> Create([Bind("Acronym,NiftiFile,MriImage")] PatientDto dto)
        {
            if (ModelState.IsValid)
            {
                IFileStream niftiFileProxy = null;
                IFileStream mriFileProxy = null;

                // convert IFormFile to 
                if (dto.NiftiFile != null)
                    niftiFileProxy = new FormFileProxy(dto.NiftiFile);

                if (dto.MriImage != null)
                    mriFileProxy = new FormFileProxy(dto.MriImage);

                Patient patient = dto.ToModel();
                await _patientService.Create(patient, niftiFileProxy, mriFileProxy);
                return RedirectToAction(nameof(Index));
            }
            return View(dto);
        }

        [Authorize(Roles = RoleSet.AllowCreatePatient)]
        // GET: Patients/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var patient = await _patientService.Get(id.Value);
            if (patient == null)
                return NotFound();

            return View(patient.ToEditDto());
        }

        // POST: Patients/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = RoleSet.AllowCreatePatient)]
        [RequestSizeLimit(100000000)]
        public async Task<IActionResult> Edit(int patientID, [Bind("PatientID,Acronym,NiftiFile,MriImage")] PatientEditDto dto)
        {
            if (patientID != dto.PatientID)
                return NotFound();

            if (ModelState.IsValid)
            {
                IFileStream niftiFileProxy = null;
                IFileStream mriFileProxy = null;

                // convert IFormFile to 
                if (dto.NiftiFile != null)
                    niftiFileProxy = new FormFileProxy(dto.NiftiFile);

                if (dto.MriImage != null)
                    mriFileProxy = new FormFileProxy(dto.MriImage);

                bool success = await _patientService.Update(dto.ToModel(), niftiFileProxy, mriFileProxy);

                if (!success)
                    return NotFound();

                return RedirectToAction(nameof(Index));
            }

            return View(dto);
        }

        // GET: Patients/Delete/5
        [Authorize(Roles = RoleSet.AllowCreatePatient)]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var patient = await _patientService.Get(id.Value);

            if (patient == null)
                return NotFound();

            return View(patient.ToDto());
        }

        // POST: Patients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = RoleSet.AllowCreatePatient)]
        public async Task<IActionResult> DeleteConfirmed(int patientID)
        {
            bool success = await _patientService.Delete(patientID);
            if (!success)
                return NotFound();

            return RedirectToAction(nameof(Index));
        }
    }
}
