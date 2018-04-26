using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace AuctionReportGenerator.Models
{
    public class AuctionDBContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=Auction.db");
        }

        public DbSet<AuctionReportGenerator.Models.Donor> Donor { get; set; }
        public DbSet<AuctionReportGenerator.Models.Donation> Donation { get; set; }
        public DbSet<AuctionReportGenerator.Models.Bidder> Bidder { get; set; }
        public DbSet<AuctionReportGenerator.Models.Paddle> Paddle { get; set; }
        public DbSet<AuctionReportGenerator.Models.Bid> Bid { get; set; }
    }
}
