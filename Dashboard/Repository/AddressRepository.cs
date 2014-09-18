using DashboardSite.Core;
using DashboardSite.Model;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DashboardSite.Repository.Interface;
using EntityObject.Entities.EntityFromDB;

namespace DashboardSite.Repository
{
    public class AddressRepository : BaseRepository<Address>, IAddressRepository
    {
        public AddressRepository()
            : base(new UnitOfWork())
        {
        }
        public AddressRepository(UnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }

        public int AddNewAddress(AddressModel model)
        {
            var address = Mapper.Map<Address>(model);
            this.InsertNew(address);
            return address.AddressID;
        }

        public AddressModel GetAddressByID(int id)
        {
            var address = this.GetByIDFast(id);
            return Mapper.Map<AddressModel>(address);
        }

        public void UpdateAddress(AddressModel model)
        {
            var address = this.GetByIDFast(model.AddressID);
            address.AddressLine1 = model.AddressLine1;
            address.AddressLine2 = model.AddressLine2;
            address.City = model.City;
            address.Email = model.Email;
            address.Fax = model.Fax;
            address.Organization = model.Organization;
            address.PhoneAlternate = model.PhoneAlternate;
            address.PhonePrimary = model.PhonePrimary;
            address.StateCode = model.StateCode;
            address.Title = model.Title;
            address.ZIP = model.ZIP;
            address.ZIP4_Code = model.ZIP4_Code;
            //Mapper.Map<Address>(model);
            this.Update(address);
        }
    }
}
