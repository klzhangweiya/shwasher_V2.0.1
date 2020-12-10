using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Dependency;
using Microsoft.AspNet.SignalR;

namespace ShwasherSys.Hubs
{
    public class IwbHub : Hub, ITransientDependency
    {
        /*public void GetShortMsg(string message)
        {
            this.LogDebug($"【SendShortMsg】：{message}");
            Clients.All.getShortMsg(message);
            this.LogDebug("SendShortMsg-Success");
        }*/
    }
}
