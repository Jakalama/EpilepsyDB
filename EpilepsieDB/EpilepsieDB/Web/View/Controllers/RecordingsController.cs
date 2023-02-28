using EpilepsieDB.Authorization;
using EpilepsieDB.Services;
using EpilepsieDB.Web.View.Extensions;
using EpilepsieDB.Web.View.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EpilepsieDB.Web.View.Controllers
{
    [Controller]
    [Authorize(Roles = RoleSet.AllowRead)]
    [ApiExplorerSettings(IgnoreApi = true)]
    [Route("[controller]/[action]", Order = 0)]
    public class RecordingsController : Controller
    {
        private readonly IRecordingService _recordingService;
        private readonly IPatientService _patientService;
        private readonly IScanService _scanService;

        public RecordingsController(
            IRecordingService recordingService,
            IPatientService patientService,
            IScanService scanService)
        {
            _recordingService = recordingService;
            _patientService = patientService;
            _scanService = scanService;
        }

        // GET: Recordings/Create
        [Authorize(Roles = RoleSet.AllowCreateScan)]
        public async Task<IActionResult> Create(int patientID)
        {
            RecordingDto dto = new RecordingDto();
            dto.PatientID = patientID;

            return View(dto);
        }

        // POST: Recordings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = RoleSet.AllowCreateScan)]
        public async Task<IActionResult> Create([Bind("PatientID,RecordingNumber")] RecordingDto recordingDto)
        {
            if (ModelState.IsValid)
            {
                var recording = recordingDto.ToModel(await _patientService.Get(recordingDto.PatientID));

                await _recordingService.Create(recording);

                return RedirectToAction(nameof(PatientsController.Details), "Patients", new { id = recordingDto.PatientID });
            }

            return View(recordingDto);
        }

        // GET: Scans/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var recording = (await _recordingService.Get(filter: r => r.ID == id.Value, includeProperties: "Patient"))
                .FirstOrDefault();

            if (recording == null)
                return NotFound();

            IEnumerable<ScanDto> scans = (await _scanService.Get(filter: r => r.RecordingID == id.Value)).ToDto();

            var dto = recording.ToDetailDto(scans);

            return View(dto);
        }

        // GET: Recordings/Edit/5
        [Authorize(Roles = RoleSet.AllowCreateScan)]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var recording = await _recordingService.Get(id.Value);
            if (recording == null)
                return NotFound();

            var dto = recording.ToDto();

            return View(dto);
        }

        // POST: Recordings/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = RoleSet.AllowCreateScan)]
        public async Task<IActionResult> Edit(int recordingID, [Bind("RecordingNumber,RecordingID,PatientID")] RecordingDto dto)
        {
            if (recordingID != dto.RecordingID)
                return NotFound();

            if (ModelState.IsValid)
            {
                bool success = await _recordingService.Update(dto.ToModel());

                if (!success)
                    return NotFound();

                return RedirectToAction(nameof(PatientsController.Details), "Patients", new { id = dto.PatientID });
            }

            return View(dto);
        }

        // GET: Recordings/Delete/5
        [Authorize(Roles = RoleSet.AllowCreateScan)]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var recording = await _recordingService.Get(id.Value);

            if (recording == null)
                return NotFound();

            return View(recording.ToDto());
        }

        // POST: Recordings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = RoleSet.AllowCreateScan)]
        public async Task<IActionResult> DeleteConfirmed(int? recordingID)
        {
            var recording = await _recordingService.Get(recordingID.Value);
            bool success = await _recordingService.Delete(recordingID.Value);
            if (!success)
                return NotFound();

            return RedirectToAction(nameof(PatientsController.Details), "Patients", new { id = recording.PatientID });
        }
    }
}
