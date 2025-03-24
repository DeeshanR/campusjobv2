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
    // 1. First find or create a default student employee
    var defaultStudent = await _context.Employees
        .Include(e => e.Recruiter)
        .FirstOrDefaultAsync(e => e.Recruiter.User.Email == "default@student.com");
    
    if (defaultStudent == null)
    {
        // Create default student user first
        var studentUser = new User
        {
            First_Name = "Default",
            Last_Name = "Student",
            Email = "default@student.com",
            Password = "TempPassword123", // Should be hashed in production
            Role = 3 // Student role
        };
        _context.Users.Add(studentUser);
        await _context.SaveChangesAsync();

        // Get any recruiter (or use the same default recruiter logic from before)
        var recruiter = await _context.Recruiters.FirstOrDefaultAsync();
        if (recruiter == null)
        {
            ModelState.AddModelError("", "No recruiters available to assign students");
            return View(); // Return to form with error
        }

        // Then create employee record
        defaultStudent = new Employee
        {
            Student_ID = studentUser.User_ID, // Assuming Student_ID matches User_ID
            Recruitment_ID = recruiter.Recruitment_ID
        };
        _context.Employees.Add(defaultStudent);
        await _context.SaveChangesAsync();
    }

    // 2. Create the shift with valid student ID
    var shift = new OfferedShift
    {
        Student_ID = defaultStudent.Student_ID, // Use the valid student ID
        Recruitment_ID = defaultStudent.Recruitment_ID, // Or get from current user if logged in
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
    var shift = await _context.OfferedShifts
        .Include(o => o.ApprovedShifts)
        .FirstOrDefaultAsync(o => o.Offer_ID == shiftId);

    if (shift == null)
    {
        return NotFound();
    }

    // Mark as approved
    shift.Status = true;

    // Create an approved shift record
    var approvedShift = new ApprovedShift
    {
        Offer_ID = shift.Offer_ID,
        Hours_Worked = shift.Total_Hours,
        Status = true,
        Date_Uploaded = DateTime.Now
    };

    _context.ApprovedShifts.Add(approvedShift);
    
    try
    {
        await _context.SaveChangesAsync();
        return RedirectToAction("Index");
    }
    catch (Exception ex)
    {
        // Log the error
        Console.WriteLine($"Error approving shift: {ex.Message}");
        return StatusCode(500, "Error approving shift");
    }
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
