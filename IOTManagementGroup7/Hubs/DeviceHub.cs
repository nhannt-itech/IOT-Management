using IOTManagementGroup7.DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IOTManagementGroup7.Hubs
{
    public class DeviceHub : Hub
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeviceHub (IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task SendSignalDevice(string deviceId)
        {
            var obj = _unitOfWork.Device.GetFirstOrDefault(x => x.Id == deviceId,
                                            includeProperties: "Sensor,DeviceType");
            await Clients.All.SendAsync("ReceiveSignalDevice", obj);
        }
    }
}
