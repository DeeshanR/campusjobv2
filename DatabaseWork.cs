using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using campusjobv2;
using campusjobv2.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Options;
using static campusjobv2.ApplicationDbContext;


namespace Tests_For_Campus_Jobs_Project
{
    public class DatabaseWork
    {
        //find database
        //check it exists
        //access database

        public DbContextOptions<ApplicationDbContext> currentOptions = new DbContextOptions<ApplicationDbContext>();

        public bool AccessingDb()
        {
            bool result = false;
            using (var context = new ApplicationDbContext(currentOptions))
            {
                result = context.Database.EnsureCreated();
                if (result)
                {
                    result = context.Database.CanConnect();
                }
            }
            return result;
        }

        public int InsertIntoDb(string query)
        {
            int columnsChanged = 0;
            bool result = AccessingDb();
            //query database with specified information
            //get result
            if (result)
            {
                using (var context = new ApplicationDbContext(currentOptions))
                {
                    DatabaseFacade newdbFacade = new DatabaseFacade(context);
                    
                    newdbFacade.BeginTransaction();
                    
                    //cant use interpolated string
                    columnsChanged = context.Database.ExecuteSqlRaw(query);
                    //context.Users.Add(new User());

                    newdbFacade.CommitTransaction();

                }
            }
            return columnsChanged;
        }

        public void RetrieveDbInfo(string query)
        {
            //retrieve information from the database

            if (AccessingDb())
            {
                using (var context = new ApplicationDbContext(currentOptions))
                {
                    DatabaseFacade newdbFacade = new DatabaseFacade(context);
                    newdbFacade.BeginTransaction();

                    context.Database.ExecuteSqlRaw(query);
                }
            }
        }


        public async void InsertIntoDb(User user)
        {
        }
    }
}
