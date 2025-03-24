using campusjobv2.Models;
using campusjobv2.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace campusjobv2.Controllers
{
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var model = new AdminViewModel();

     
            var approvedShifts = await _context.ApprovedShifts
                .Include(a => a.OfferedShift)
                .ThenInclude(o => o.Employee)
                .ThenInclude(e => e.Recruiter)
                .ThenInclude(r => r.User)
                .Include(a => a.OfferedShift)
                .ThenInclude(o => o.Recruiter)
                .ThenInclude(r => r.User)
                .ToListAsync();

            foreach (var shift in approvedShifts)
            {
                model.AuditBoard.Add(new AdminViewModel.AuditRecord
                {
                    ShiftId = shift.Timesheet_ID,
                    StudentId = shift.OfferedShift.Student_ID,
                    StudentName = $"{shift.OfferedShift.Employee.Recruiter.User.First_Name} {shift.OfferedShift.Employee.Recruiter.User.Last_Name}",
                    Recruiter = $"{shift.OfferedShift.Recruiter.User.First_Name} {shift.OfferedShift.Recruiter.User.Last_Name}",
                    Date = shift.Date_Uploaded,
                    Duration = shift.Hours_Worked,
                    Status = shift.Status ? "Approved" : "Pending"
                });
            }

            return View(model);
        }

[HttpPost]
public async Task<IActionResult> CreateStudentAccount(string studentId, string name, string email, string department, string visaRestricted)
{
    // 1. First find or create a default recruiter
    var defaultRecruiter = await _context.Recruiters
        .FirstOrDefaultAsync(r => r.User.Email == "default@recruiter.com");
    
    if (defaultRecruiter == null)
    {
        // Create default recruiter user first
        var recruiterUser = new User
        {
            First_Name = "Default",
            Last_Name = "Recruiter",
            Email = "default@recruiter.com",
            Password = "TempPassword123", // Should be hashed in production
            Role = 2 // Recruiter role
        };
        _context.Users.Add(recruiterUser);
        await _context.SaveChangesAsync();

        // Then create recruiter record
        defaultRecruiter = new Recruiter
        {
            User_ID = recruiterUser.User_ID
        };
        _context.Recruiters.Add(defaultRecruiter);
        await _context.SaveChangesAsync();
    }

    // 2. Create student user
    var user = new User
    {
        First_Name = name.Split(' ')[0],
        Last_Name = name.Split(' ').Length > 1 ? name.Split(' ')[1] : "",
        Email = email,
        Password = "TempPassword123", // Should be hashed in production
        Role = 3 // Student role
    };
    _context.Users.Add(user);
    await _context.SaveChangesAsync();

    // 3. Create employee record with valid recruiter
    var employee = new Employee
    {
        Student_ID = int.Parse(studentId),
        Recruitment_ID = defaultRecruiter.Recruitment_ID // Use the valid recruiter ID
    };
    _context.Employees.Add(employee);
    await _context.SaveChangesAsync();

    return RedirectToAction("Index");
}
    }
}
