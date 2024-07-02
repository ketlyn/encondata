using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace TechsysLog.Identity.Api.Data
{
    public class TechsysLogDBIdentityContext : IdentityDbContext
    {
        public TechsysLogDBIdentityContext(DbContextOptions<TechsysLogDBIdentityContext> options) : base(options) { }
    }
}
