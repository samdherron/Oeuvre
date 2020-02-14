using Microsoft.EntityFrameworkCore;
using Oeuvre.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Oeuvre.Tests
{
    class DbContextMock
    {
        public static dbo_OeuvreContext context(string dbName)
        {

            string connectionString = "Server = (localdb)\\mssqllocaldb; Database = dbo_Oeuvre; Trusted_Connection = True";
            // Create options for DbContext instance
            var options = new DbContextOptionsBuilder<dbo_OeuvreContext>()
                .UseSqlServer(connectionString)
                .Options;

            // Create instance of DbContext
            var dbContext = new dbo_OeuvreContext(options);

            // Add entities in memory
            dbContext.Seed();

            return dbContext;
        }

    }

}

