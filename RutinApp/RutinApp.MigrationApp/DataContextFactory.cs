using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using RutinApp.Shared.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace RutinApp.MigrationApp
{
    public class DataContextFactory : IDesignTimeDbContextFactory<DataContext>
    {
        public DataContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<DataContext>();
            optionsBuilder.UseSqlite("Data Source=app.db");
            return new DataContext(optionsBuilder.Options);
        }
    }
}
