using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace UserApp.Services
{
    public interface INotificationService
    {
        Task<ClientData> GetHandleAsync(string group);
    }

    public class ClientData
    {
        public string Handle { get; set; }

        public string Group { get; set; }
    }
}
