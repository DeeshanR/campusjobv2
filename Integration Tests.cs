using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Web.Helpers;



using campusjobv2;
using campusjobv2.Controllers;
using campusjobv2.Migrations;
using campusjobv2.Models;
using campusjobv2.Models.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Logging;
using Moq;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Tests_For_Campus_Jobs_Project
{
    public class BaseFunctions
    {
        public int? InjectRecruitersIntoDatabase(User recruiter, ApplicationDbContext context)
        {
            //model binding is the process in which URLs are turned into useable data for the computer
            //for the model state to return true, we are going to have all parts of the query be addressed in the model state dictionary
            int? recruiterID = null;
            using (context)
            {
                // no need for:
                //      context.Database.EnsureCreated();
                // as the database has already been made

                if (context.Database.CanConnect())
                {
                    //add recruiter into user
                    using (var transaction = context.Database.BeginTransaction())
                    {
                        try
                        {
                            //add recruiter to user table
                            context.Users.Add(recruiter);
                            context.SaveChanges();

                            //add recruiter to recruiter table
                            var newRecruiter = new Recruiter
                            {
                                User_ID = recruiter.User_ID,
                                User = recruiter
                            };
                            context.Recruiters.Add(newRecruiter);
                            context.SaveChanges();
                            recruiterID = newRecruiter.Recruitment_ID;

                            //add reference to recruiter instance in user instance
                            recruiter.Recruiter = newRecruiter;
                            //set recruiter in database
                            context.Users.Update(recruiter);

                            context.SaveChanges(true);

                            transaction.Commit();
                        }
                        catch
                        {
                            transaction.Rollback();
                        }

                    }

                }

                return recruiterID;
            }
        }

        public Recruiter? InjectRecruitersIntoDatabase_Recruiter(User recruiter, ApplicationDbContext context)
        {
            //model binding is the process in which URLs are turned into useable data for the computer
            //for the model state to return true, we are going to have all parts of the query be addressed in the model state dictionary
            Recruiter basic = null;
            using (context)
            {
                // no need for:
                //      context.Database.EnsureCreated();
                // as the database has already been made

                if (context.Database.CanConnect())
                {
                    //add recruiter into user
                    using (var transaction = context.Database.BeginTransaction())
                    {
                        try
                        {
                            //add recruiter to user table
                            context.Users.Add(recruiter);
                            context.SaveChanges();

                            //add recruiter to recruiter table
                            var newRecruiter = new Recruiter
                            {
                                User_ID = recruiter.User_ID,
                                User = recruiter
                            };
                            context.Recruiters.Add(newRecruiter);
                            context.SaveChanges();
                            basic = newRecruiter;

                            //add reference to recruiter instance in user instance
                            recruiter.Recruiter = newRecruiter;
                            //set recruiter in database
                            context.Users.Update(recruiter);

                            context.SaveChanges(true);

                            transaction.Commit();
                        }
                        catch
                        {
                            transaction.Rollback();
                        }

                    }

                }

                return basic;
            }
        }

        public OfferedShift? InjectShiftIntoDatabase(OfferedShift shift, ApplicationDbContext context)
        {
            OfferedShift offeredShift = null;
            if (context.Database.CanConnect())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        context.OfferedShifts.Add(shift);
                        context.SaveChanges();
                        offeredShift = shift;

                        transaction.Commit();
                    }
                    catch { transaction.Rollback(); }
                }
            }
            return offeredShift;
        }

        public Employee? InjectEmployeesIntoDatabase(StudentAccountModel student, int recruiterID, ApplicationDbContext context)
        {
            //model binding is the process in which URLs are turned into useable data for the computer
            //for the model state to return true, we are going to have all parts of the query be addressed in the model state dictionary
            Employee employee = null;
            using (context)
            {
                if (context.Database.CanConnect())
                {
                    //add recruiter into user
                    using (var transaction = context.Database.BeginTransaction())
                    {
                        try
                        {
                            User user = new User
                            {
                                //User_ID = 666,
                                First_Name = student.FirstName,
                                Last_Name = student.LastName,
                                Email = student.Email,
                                Password = "student.Password,",
                                Role = 3

                            };
                            //add recruiter to user table
                            context.Users.Add(user);
                            context.SaveChanges();
                            int usering = user.User_ID;

                            var userable = context.Users.Where(u => u.User_ID == 677);

                            //add recruiter to recruiter table
                            var newStudent = new Employee
                            {
                                Student_ID = user.User_ID+3,
                                Recruitment_ID = recruiterID,
                                User = user,
                            };
                            context.Employees.Add(newStudent);
                            context.SaveChanges();

                            context.RightToWorkDocuments.Add(new RightToWorkDocument
                            {
                                Student_ID = user.User_ID,
                                Document_URL = "N/A",
                                Upload_Date = DateTime.Now,
                                Employee = newStudent
                            });
                            context.VisaStatuses.Add(new VisaStatus
                            {
                                Student_ID = user.User_ID,
                                Status = false,
                                ExpiryDate = new DateTime(2026, 05, 03),
                                Employee = newStudent
                            });

                            context.SaveChanges();
                            employee = newStudent;

                            //add reference to recruiter instance in user instance
                            //recruiter.Recruiter = newRecruiter;
                            //set recruiter in database
                            //context.Users.Update(recruiter);

                            context.SaveChanges(true);

                            transaction.Commit();
                        }
                        catch
                        {
                            transaction.Rollback();
                        }

                    }

                }

                return employee;
            }
        }

        public Admin? InjectAdminIntoDatabase(User newAdmin, ApplicationDbContext context)
        {
            Admin admin = null;
            if (context.Database.CanConnect())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        context.Users.Add(newAdmin);
                        context.SaveChanges();

                        Admin admini = new Admin
                        {
                            User = newAdmin,
                            User_ID = newAdmin.User_ID
                        };
                        context.Admins.Add(admini);
                        context.SaveChanges();
                        admin = admini;

                        transaction.Commit();
                    }
                    catch
                    {
                        transaction.Rollback();
                    }
                }
            }
            return admin;
        }

        public Notification? InjectNotifIntoDatabase(Notification notif, ApplicationDbContext context)
        {
            Notification notification = null;
            if (context.Database.CanConnect())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        Notification newNotif = notif;
                        context.Notifications.Add(newNotif);
                        context.SaveChanges();
                        notification = newNotif;
                        transaction.Commit();
                    }
                    catch
                    {
                        transaction.Rollback();
                    }
                }
            }
            return notification;
        }

        public bool? ReadNotifStatus(int notifId, ApplicationDbContext context)
        {
            if (context.Database.CanConnect())
            {
                try
                {
                    List<Notification> returnNotif = context.Notifications.Where(n => n.Notification_ID == notifId).ToList<Notification>();
                    if (returnNotif == null)
                    {
                        return false;
                    }
                    return returnNotif[0].Read;
                }
                catch
                {
                    return null;
                }
            }
            return null;

        }

    }
    internal class AdminControllerTests : BaseFunctions
    {
        // testing the controllers and database as one unit
        // testing entering values into the database through calling the controller functions directly

        // admin controller functions to test:
        // create student account
        // load pending shifts
        // approve shift

        


        //[TestType]_[ClassName]_[MethodName]_[IntendedOutcome]
        //IntergrationTest_AdminController_CreateStudentAccount_SuccessfulAccountCreation
        [Test]
        public async Task IT_AC_CreateStudentAccount_Success()
        {
            //ARRANGE 

            // access vital details
            // - database context

            // new ApplicationDbContext
            DbContextOptionsBuilder<ApplicationDbContext> optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsBuilder.UseMySql("Server=localhost;Database=CampusJobV2;User=root;Password=P@ssword123;", new MySqlServerVersion(new Version(8, 0, 34)));
            DbContextOptions<ApplicationDbContext> contextOptions = optionsBuilder.Options;
            ApplicationDbContext context = new ApplicationDbContext(contextOptions);

            // mock Logger
            var mockLogger = new Mock<ILogger<AdminController>>();
            ILogger<AdminController> adminLogger = mockLogger.Object;

            //new admincontroller
            AdminController adminController = new AdminController(context, adminLogger);
            adminController.ModelState.Clear();

            var randomInt = new Random();
            string recruiterEmail = $"testEmail{randomInt.Next(0, 10000)}@email.com";

            User recruiter = new User
            {
                First_Name = "Recruiter",
                Last_Name = "McRecruiter",
                Email = recruiterEmail,
                Password = "password",
                Role = 2
            };

            int? recruitID = InjectRecruitersIntoDatabase(recruiter, context);

            string studentEmail = $"StudentName{randomInt.Next(0, 10000)}@email.com";
            StudentAccountModel studentAccountModel = new StudentAccountModel
            {
                FirstName = "StudentName",
                LastName = "Surname",
                Email = studentEmail,
                Department = "Software Engineering",
                RecruiterId = recruitID.Value,
                IsVisaRestricted = false,
                VisaExpiryDate = null
            };


            //after InjectRecruiters, dbcontext gets disposed of, so we make another one
            context = new ApplicationDbContext(contextOptions);
            adminController = new AdminController(context, adminLogger);

            Mock<ITempDataProvider> mockProvider = new Mock<ITempDataProvider> { CallBase = true };
            var provider = mockProvider.Object;

            adminController.TempData = new TempDataDictionary(new DefaultHttpContext(), provider);



            //ACT

            var result = await adminController.CreateStudentAccount(studentAccountModel);


            string success = adminController.TempData["Success"] as string;


            //ASSERT
            Assert.That(success.Contains("Student account created successfully"));
        }

        [Test]
        public async Task IT_AC_ApproveShift_Success()
        {
            //Arrange
            DbContextOptionsBuilder<ApplicationDbContext> optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsBuilder.UseMySql("Server=localhost;Database=CampusJobV2;User=root;Password=P@ssword123;", new MySqlServerVersion(new Version(8, 0, 34)));
            DbContextOptions<ApplicationDbContext> contextOptions = optionsBuilder.Options;
            ApplicationDbContext context = new ApplicationDbContext(contextOptions);

            // mock Logger
            var mockLogger = new Mock<ILogger<AdminController>>();
            ILogger<AdminController> adminLogger = mockLogger.Object;

            //new admincontroller
            AdminController adminController = new AdminController(context, adminLogger);
            adminController.ModelState.Clear();


            //create shift
            //inject into offeredshift table 
            //
            //possibly create student or recruiter too?

            //make recruiter
            var randomInt = new Random();
            string recruiterEmail = $"testEmail{randomInt.Next(0, 10000)}@email.com";

            User recruiter = new User
            {
                First_Name = "Recruiter",
                Last_Name = "McRecruiter",
                Email = recruiterEmail,
                Password = "password",
                Role = 2
            };

            Recruiter returnRecruiter = InjectRecruitersIntoDatabase_Recruiter(recruiter, context);

            //make student

            string studentEmail = $"StudentName{randomInt.Next(0, 10000)}@email.com";
            StudentAccountModel studentAccountModel = new StudentAccountModel
            {
                FirstName = "StudentName",
                LastName = "Surname",
                Email = studentEmail,
                Department = "Software Engineering",
                RecruiterId = returnRecruiter.Recruitment_ID,
                IsVisaRestricted = false,
                VisaExpiryDate = null
            };


            //after InjectRecruiters, dbcontext gets disposed of, so we make another one
            context = new ApplicationDbContext(contextOptions);
            adminController = new AdminController(context, adminLogger);

            Mock<ITempDataProvider> mockProvider = new Mock<ITempDataProvider> { CallBase = true };
            var provider = mockProvider.Object;

            adminController.TempData = new TempDataDictionary(new DefaultHttpContext(), provider);

            var result = await adminController.CreateStudentAccount(studentAccountModel);




            context = new ApplicationDbContext(contextOptions);
            adminController = new AdminController(context, adminLogger);



            //this is the issue
            //Employee employee = FindEmployeeData_Employee(, context);




            context = new ApplicationDbContext(contextOptions);
            adminController = new AdminController(context, adminLogger);

            OfferedShift offeredShift = new OfferedShift
            {
                Student_ID = 2,
                Recruitment_ID = 11,
                //Date Offered is already set
                Status = 1,
                Start_Date = DateTime.Now,
                End_Date = DateTime.Now,
                Total_Hours = 1m,
                //Employee = employee,
                Recruiter = returnRecruiter
            };

            OfferedShift returnShift = InjectShiftIntoDatabase(offeredShift, context);



            //ACT
            //after InjectRecruiters, dbcontext gets disposed of, so we make another one
            context = new ApplicationDbContext(contextOptions);
            adminController = new AdminController(context, adminLogger);


            provider = mockProvider.Object;

            adminController.TempData = new TempDataDictionary(new DefaultHttpContext(), provider);

            var testResult = await adminController.ApproveShift(returnShift.Offer_ID);


            string success = adminController.TempData["Success"] as string;



            //ASSERT
            Assert.That(success.Contains("Shift approved successfully"));

            
        }

        [Test]
        public async Task IT_AC_Index_Success()
        {
            DbContextOptionsBuilder<ApplicationDbContext> optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsBuilder.UseMySql("Server=localhost;Database=CampusJobV2;User=root;Password=P@ssword123;", new MySqlServerVersion(new Version(8, 0, 34)));
            DbContextOptions<ApplicationDbContext> contextOptions = optionsBuilder.Options;
            ApplicationDbContext context = new ApplicationDbContext(contextOptions);

            var randomInt = new Random();
            string adminEmail = $"testAdminEmail{randomInt.Next(0, 10000)}@email.com";

            Admin returnAdmin = InjectAdminIntoDatabase(new User
            {
                First_Name = "Test",
                Last_Name = "Test",
                Email = adminEmail,
                Password = "Test",
                Role = 1
            }, context);

            context = new ApplicationDbContext(contextOptions);

            Mock<ISession> mockSession = new Mock<ISession>();
            ISession session = mockSession.Object;
            session.SetInt32("AdminId", returnAdmin.Admin_ID);
            

            var mockLogger = new Mock<ILogger<AdminController>>();
            ILogger<AdminController> adminLogger = mockLogger.Object;

            ControllerContext controllerContext = new ControllerContext { HttpContext = new DefaultHttpContext { Session = session } };

            AdminController adminController = new AdminController(context, adminLogger)
            {
                ControllerContext = controllerContext
            };

            Mock<ITempDataProvider> mockProvider = new Mock<ITempDataProvider> { CallBase = true };
            var provider = mockProvider.Object;

            

            adminController.TempData = new TempDataDictionary(new DefaultHttpContext(), provider);
            //make an admin!
            

            ViewResult indexResult = await adminController.Index("Student") as ViewResult;

            Assert.That(indexResult != null);
        }

        [Test]
        public async Task IT_AC_Index_RedirectToAction()
        {
            DbContextOptionsBuilder<ApplicationDbContext> optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsBuilder.UseMySql("Server=localhost;Database=CampusJobV2;User=root;Password=P@ssword123;", new MySqlServerVersion(new Version(8, 0, 34)));
            DbContextOptions<ApplicationDbContext> contextOptions = optionsBuilder.Options;
            ApplicationDbContext context = new ApplicationDbContext(contextOptions);

            var mockLogger = new Mock<ILogger<AdminController>>();
            ILogger<AdminController> adminLogger = mockLogger.Object;

            AdminController adminController = new AdminController(context, adminLogger);

            Mock<ITempDataProvider> mockProvider = new Mock<ITempDataProvider> { CallBase = true };
            var provider = mockProvider.Object;

            adminController.TempData = new TempDataDictionary(new DefaultHttpContext(), provider);

            //indexResult = await adminController.Index("Student) as Re;
        }
    }

    internal class TimesheetControllerTests : BaseFunctions
    {
        [Test]
        public async Task AcceptShift_Test()
        {
            // access vital details
            // - database context

            // new ApplicationDbContext
            DbContextOptionsBuilder<ApplicationDbContext> optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsBuilder.UseMySql("Server=localhost;Database=CampusJobV2;User=root;Password=P@ssword123;", new MySqlServerVersion(new Version(8, 0, 34)));
            DbContextOptions<ApplicationDbContext> contextOptions = optionsBuilder.Options;
            ApplicationDbContext context = new ApplicationDbContext(contextOptions);

            // mock Logger
            var mockLogger = new Mock<ILogger<AdminController>>();
            ILogger<AdminController> logger = mockLogger.Object;


            //new admincontroller
            TimeSheetsController controller = new TimeSheetsController(context);
            AdminController adminController = new AdminController(context, logger);
            controller.ModelState.Clear();

            Mock<ITempDataProvider> mockProvider = new Mock<ITempDataProvider> { CallBase = true };
            var provider = mockProvider.Object;

            adminController.TempData = new TempDataDictionary(new DefaultHttpContext(), provider);

            /* create recruiter

            var randomInt = new Random();
            string recruiterEmail = $"testEmail{randomInt.Next(0, 10000)}@email.com";

            User recruiter = new User
            {
                First_Name = "Recruiter",
                Last_Name = "McRecruiter",
                Email = recruiterEmail,
                Password = "password",
                Role = 2
            };

            Recruiter returnRecruiter = InjectRecruitersIntoDatabase_Recruiter(recruiter, context);

            //create student, return student ID

            context = new ApplicationDbContext(contextOptions);
            adminController = new AdminController(context, logger);

            string studentEmail = $"StudentName{randomInt.Next(0, 10000)}@email.com";
            StudentAccountModel studentAccountModel = new StudentAccountModel
            {
                FirstName = "StudentName",
                LastName = "Surname",
                Email = studentEmail,
                Department = "Software Engineering",
                RecruiterId = returnRecruiter.Recruitment_ID,
                IsVisaRestricted = false,
                VisaExpiryDate = null
            };

            adminController.CreateStudentAccount(studentAccountModel);
            //Employee returnEmployee = InjectEmployeesIntoDatabase(studentAccountModel, returnRecruiter.Recruitment_ID, context);
            */
            Mock<ISession> mockSession = new Mock<ISession>();
            ISession session = mockSession.Object;
            session.SetInt32("StudentId", 667);
            

            //create a shift and inject it into the database
            context = new ApplicationDbContext(contextOptions);
            adminController = new AdminController(context, logger);

            OfferedShift offeredShift = new OfferedShift
            {
                
                //Date Offered is already set
                Status = 1,
                Start_Date = DateTime.Now,
                End_Date = DateTime.Now,
                Total_Hours = 1m,
                Employee = new Employee(),
                Recruiter = new Recruiter(),
                //Student_ID
                //Recruitment_ID,
            };

            OfferedShift returnShift = InjectShiftIntoDatabase(offeredShift, context);



            context = new ApplicationDbContext(contextOptions);
            ControllerContext controllerContext = new ControllerContext { HttpContext = new DefaultHttpContext { Session = session } };

            controller = new TimeSheetsController(context)
            {
                ControllerContext = controllerContext
            };

            //call acccept shift
            var result = await controller.AcceptShift(returnShift.Offer_ID);



            
        }

        //decline shift

        [Test]
        public async Task DeclineShift()
        {

        }

        [Test]
        public async Task MarkAsRead()
        {
            DbContextOptionsBuilder<ApplicationDbContext> optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsBuilder.UseMySql("Server=localhost;Database=CampusJobV2;User=root;Password=P@ssword123;", new MySqlServerVersion(new Version(8, 0, 34)));
            DbContextOptions<ApplicationDbContext> contextOptions = optionsBuilder.Options;
            ApplicationDbContext context = new ApplicationDbContext(contextOptions);

            // mock Logger
            var mockLogger = new Mock<ILogger<AdminController>>();
            ILogger<AdminController> logger = mockLogger.Object;


            //new admincontroller
            TimeSheetsController controller = new TimeSheetsController(context);
            AdminController adminController = new AdminController(context, logger);
            controller.ModelState.Clear();

            Mock<ITempDataProvider> mockProvider = new Mock<ITempDataProvider> { CallBase = true };
            var provider = mockProvider.Object;

            adminController.TempData = new TempDataDictionary(new DefaultHttpContext(), provider);

            //create recruiter
            var randomInt = new Random();
            string recruiterEmail = $"testEmail{randomInt.Next(0, 10000)}@email.com";

            User recruiter = new User
            {
                First_Name = "Recruiter",
                Last_Name = "McRecruiter",
                Email = recruiterEmail,
                Password = "password",
                Role = 2
            };
            Recruiter returnRecruiter = InjectRecruitersIntoDatabase_Recruiter(recruiter, context);

            Notification notif = new Notification
            {
                User_ID = returnRecruiter.User_ID,
                Message = "Hello",
            };

            context = new ApplicationDbContext(contextOptions);
            Notification returnNotif = InjectNotifIntoDatabase(notif, context);

            context = new ApplicationDbContext(contextOptions);
            controller = new TimeSheetsController(context);

            var result = await controller.MarkAsRead(returnNotif.Notification_ID);
            context = new ApplicationDbContext(contextOptions);

            Assert.That(ReadNotifStatus(returnNotif.Notification_ID, context), Is.True);


        }
    }
}
