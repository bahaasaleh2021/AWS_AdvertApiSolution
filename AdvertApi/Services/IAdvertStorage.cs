using AdvertApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdvertApi.Services
{
    public interface IAdvertStorage
    {
        Task<string> Add(AdvertModel advert);

        Task Confirm(ConfirmAdvertModel confirm);

        Task<bool> CheckDBHealth();
        Task<AdvertModel> GetById(string id);
    }
}
