using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using AdamURL.Models;

namespace AdamURL.Data
{
    public class ShortUrlContext : DbContext 
    {
        public ShortUrlContext(DbContextOptions<ShortUrlContext> options)
            : base(options)
        {
        }

        public DbSet<ShortUrl> ShortUrl { get; set; }
    }
}
