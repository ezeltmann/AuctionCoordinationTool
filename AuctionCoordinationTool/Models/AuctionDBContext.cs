using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using AuctionCoordinationTool.Models;

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
        public DbSet<AuctionCoordinationTool.Models.Bidder> Bidder { get; set; }
        public DbSet<AuctionCoordinationTool.Models.Paddle> Paddle { get; set; }
        public DbSet<AuctionCoordinationTool.Models.Bid> Bid { get; set; }
        public DbSet<AuctionCoordinationTool.Models.AuctionParameters> AuctionParameters { get; set; }
        public DbSet<AuctionCoordinationTool.Models.Ticket> Tickets { get; set; }
    }
}
