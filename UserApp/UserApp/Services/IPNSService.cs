using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace UserApp.Services
{
    public interface IPNSService
    {
        Task<string> GetHandleAsync();
    }
}
