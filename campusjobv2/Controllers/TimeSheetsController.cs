using campusjobv2.Models;
using campusjobv2.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace campusjobv2.Controllers
{
    public class TimeSheetsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TimeSheetsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var model = new TimeSheetsViewModel();

            // Get confirmed shifts (accepted by student)
            var confirmedShifts = await _context.OfferedShifts
                .Include(o => o.Recruiter)
                .ThenInclude(r => r.User)
                .Where(o => o.Status && o.End_Date > DateTime.Now)
                .ToListAsync();

            foreach (var shift in confirmedShifts)
            {
                model.ConfirmedShifts.Add(new TimeSheetsViewModel.ShiftInfo
                {
                    Date = shift.Start_Date,
                    ShiftId = shift.Offer_ID,
                    AvailableShifts = "Confirmed",
                    StartTime = shift.Start_Date,
                    EndTime = shift.End_Date,
                    TotalHours = shift.Total_Hours,
                    Recruiter = $"{shift.Recruiter.User.First_Name} {shift.Recruiter.User.Last_Name}"
                });
            }

            // Get available shifts (not yet accepted)
            var availableShifts = await _context.OfferedShifts
                .Include(o => o.Recruiter)
                .ThenInclude(r => r.User)
                .Where(o => !o.Status)
                .ToListAsync();

            foreach (var shift in availableShifts)
            {
                model.AvailableShifts.Add(new TimeSheetsViewModel.ShiftInfo
                {
                    Date = shift.Start_Date,
                    ShiftId = shift.Offer_ID,
                    AvailableShifts = "Available",
                    StartTime = shift.Start_Date,
                    EndTime = shift.End_Date,
                    TotalHours = shift.Total_Hours,
                    Recruiter = $"{shift.Recruiter.User.First_Name} {shift.Recruiter.User.Last_Name}"
                });
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AcceptShift(int shiftId)
        {
            var shift = await _context.OfferedShifts.FindAsync(shiftId);
            if (shift != null)
            {
                shift.Status = true;
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> DeclineShift(int shiftId)
        {
            var shift = await _context.OfferedShifts.FindAsync(shiftId);
            if (shift != null)
            {
                _context.OfferedShifts.Remove(shift);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Index");
        }
    }
}
