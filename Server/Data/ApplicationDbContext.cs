using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Data {
    public class ApplicationDbContext : DbContext {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): base(options) { }
        public DbSet<Faculty> Faculties { get; set; } = default!;
        public DbSet<School> Schools { get; set; } = default!;
        public DbSet<Career> Careers { get; set; } = default!;
    }
}