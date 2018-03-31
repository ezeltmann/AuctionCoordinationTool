using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace AuctionCoordinationTool.Models
{
    public class AuctionDBContext : DbContext
    {
        public AuctionDBContext (DbContextOptions<AuctionDBContext> options)
            : base(options)
        {
        }

        public DbSet<AuctionCoordinationTool.Models.Donor> Donor { get; set; }
        public DbSet<AuctionCoordinationTool.Models.Donation> Donation { get; set; }
    }
}
