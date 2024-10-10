using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TooGoodToGoAvans.DomainService
{
    public interface IPackageService
    {
        public Task<bool> CheckIfPackageIsReserved();
    }
}
