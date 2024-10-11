using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace TooGoodToGoAvans.Infrastructure
{
    public class TooGoodToGoAvansDBContext_IF : IdentityDbContext
    {
        public TooGoodToGoAvansDBContext_IF(DbContextOptions options) : base(options)
        {
        }
    }
}
