using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace infrastructure.Services.SeedSevices
{
    public interface ISeedRepository
    {
        Task SeedAsync();
    }
}
