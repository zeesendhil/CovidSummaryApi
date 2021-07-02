using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CovidSummaryApi.Model;

namespace CovidSummaryApi
{
    public class ApplicationDbContext: DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext>options) : base(options)
        { 

        }
        public DbSet<Summary> Summary { get; set; }
        public DbSet<Response> Response { get; set; }
        
    }
}
