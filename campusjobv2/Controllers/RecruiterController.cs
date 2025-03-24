using campusjobv2.Models;
using campusjobv2.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace campusjobv2.Controllers
{
    public class RecruiterController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RecruiterController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var model = new RecruiterViewModel();

            // Get pending shifts (offered but not accepted)
            var pendingShifts = await _context.OfferedShifts
                .Include(o => o.Employee)
                .ThenInclude(e => e.Recruiter)
                .ThenInclude(r => r.User)
                .Where(o => !o.Status)
                .ToListAsync();

            foreach (var shift in pendingShifts)
            {
                model.PendingShifts.Add(new RecruiterViewModel.ShiftRecord
                {
                    ShiftId = shift.Offer_ID,
                    Date = shift.Start_Date,
                    StudentId = shift.Student_ID,
                    StudentName = $"{shift.Employee.Recruiter.User.First_Name} {shift.Employee.Recruiter.User.Last_Name}",
                    IsVisaRestricted = shift.Employee.VisaStatuses.Any(v => v.Status && v.ExpiryDate > DateTime.Now),
                    StartTime = shift.Start_Date,
                    EndTime = shift.End_Date,
                    ApprovedHours = 0, // Will be updated when approved
                    Duration = shift.Total_Hours
                });
            }

            // Get active shifts (accepted but not completed)
            var activeShifts = await _context.OfferedShifts
                .Include(o => o.Employee)
                .ThenInclude(e => e.Recruiter)
                .ThenInclude(r => r.User)
                .Where(o => o.Status && o.End_Date > DateTime.Now)
                .ToListAsync();

            foreach (var shift in activeShifts)
            {
                model.ActiveShifts.Add(new RecruiterViewModel.ShiftRecord
                {
                    ShiftId = shift.Offer_ID,
                    Date = shift.Start_Date,
                    StudentId = shift.Student_ID,
                    StudentName = $"{shift.Employee.Recruiter.User.First_Name} {shift.Employee.Recruiter.User.Last_Name}",
                    IsVisaRestricted = shift.Employee.VisaStatuses.Any(v => v.Status && v.ExpiryDate > DateTime.Now),
                    StartTime = shift.Start_Date,
                    EndTime = shift.End_Date,
                    ApprovedHours = shift.ApprovedShifts.Sum(a => a.Hours_Worked),
                    Duration = shift.Total_Hours
                });
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> CreateShift(DateTime shiftDate, DateTime startTime, DateTime endTime, decimal duration)
        {
            var shift = new OfferedShift
            {
                Student_ID = 1, // Default student, should be handled differently in production
                Recruitment_ID = 1, // Current recruiter, should get from session
                Date_Offered = DateTime.Now,
                Status = false, // Pending
                Start_Date = shiftDate.Date.Add(startTime.TimeOfDay),
                End_Date = shiftDate.Date.Add(endTime.TimeOfDay),
                Total_Hours = duration
            };

            _context.OfferedShifts.Add(shift);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
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
