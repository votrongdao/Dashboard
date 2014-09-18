using DashboardSite.Model;
using System;

namespace DashboardSite.Repository.Interface
{
    public interface IAddressRepository
    {
        int AddNewAddress(AddressModel model);
        AddressModel GetAddressByID(int id);
        void UpdateAddress(AddressModel model);
    }
}
