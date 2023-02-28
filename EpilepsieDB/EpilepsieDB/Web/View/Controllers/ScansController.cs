using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using EpilepsieDB.Services;
using System.Linq;
using EpilepsieDB.Source;
using EpilepsieDB.Web.Common;
using EpilepsieDB.Web.View.ViewModels;
using EpilepsieDB.Web.View.Extensions;
using Microsoft.AspNetCore.Authorization;
using EpilepsieDB.Authorization;

namespace EpilepsieDB.Web.View.Controllers
{
    [Controller]
    [Authorize(Roles = RoleSet.AllowRead)]
    [ApiExplorerSettings(IgnoreApi = true)]
    [Route("[controller]/[action]")]
    public class ScansController : Controller
    {
        private readonly IScanService _scanService;
        private readonly IBlockService _blockService;
        private readonly ISignalService _signalService;
        private readonly IRecordingService _recordingService;

        public ScansController(
            IScanService scanService,
            IBlockService blockService,
            ISignalService signalService,
            IRecordingService recordingService)
        {
            _scanService = scanService;
            _blockService = blockService;
            _signalService = signalService;
            _recordingService = recordingService;
        }

        // GET: Scans/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var scan = (await _scanService.Get(filter: s => s.ID == id.Value, includeProperties: "Recording,Recording.Patient"))
                .FirstOrDefault();

            if (scan == null)
                return NotFound();

            var signals = await _signalService.Get(filter: s => s.ScanID == scan.ID);
            var blocks = await _blockService.Get(filter: b => b.ScanID == scan.ID);

            return View(scan.ToDetailDto(signals, blocks));
        }

        // GET: Scans/Create
        [Authorize(Roles = RoleSet.AllowCreateScan)]
        public async Task<IActionResult> Create(int? recordingID)
        {
            ScanDto scanDto = new ScanDto();
            scanDto.RecordingID = recordingID.Value;

            return View(scanDto);
        }

        // POST: Scans/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = RoleSet.AllowCreateScan)]
        public async Task<IActionResult> Create([Bind("RecordingID,ScanNumber,EdfFile")] ScanDto scanDto)
        {
            if (ModelState.IsValid)
            {
                var scan = scanDto.ToModel(await _recordingService.Get(scanDto.RecordingID));

                IFileStream edfFileProxy = null;

                // convert IFormFile to 
                if (scanDto.EdfFile != null)
                    edfFileProxy = new FormFileProxy(scanDto.EdfFile);

                await _scanService.Create(scan, edfFileProxy);

                return RedirectToAction(nameof(RecordingsController.Details), "Recordings", new { id = scanDto.RecordingID });
            }

            return View(scanDto);
        }

        // GET: Scans/Edit/5
        [Authorize(Roles = RoleSet.AllowCreateScan)]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var scan = await _scanService.Get(id.Value);
            if (scan == null)
                return NotFound();

            var dto = scan.ToDto();

            return View(dto);
        }

        // POST: Scans/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = RoleSet.AllowCreateScan)]
        public async Task<IActionResult> Edit(int scanID, [Bind("ScanNumber,ScanID,RecordingID")] ScanDto dto)
        {
            if (scanID != dto.ScanID)
                return NotFound();

            if (ModelState.IsValid)
            {
                bool success = await _scanService.Update(dto.ToModel());

                if (!success)
                    return NotFound();

                return RedirectToAction(nameof(RecordingsController.Details), "Recordings", new { id = dto.RecordingID });
            }

            return View(dto);
        }

        // GET: Scans/Delete/5
        [Authorize(Roles = RoleSet.AllowCreateScan)]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var scan = await _scanService.Get(id.Value);

            if (scan == null)
                return NotFound();

            return View(scan.ToDto());
        }

        // POST: Scans/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = RoleSet.AllowCreateScan)]
        public async Task<IActionResult> DeleteConfirmed(int? scanID)
        {
            var scan = await _scanService.Get(scanID.Value);
            bool success = await _scanService.Delete(scanID.Value);
            if (!success)
                return NotFound();

            return RedirectToAction(nameof(RecordingsController.Details), "Recordings", new { id = scan.RecordingID });
        }
    }
}
