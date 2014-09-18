using System;
using System.Collections.Generic;
namespace DashboardSite.Repository.Interface
{
    public interface IStatesRepository
    {
        System.Collections.Generic.List<KeyValuePair<string, string>> GetAllStates();
    }
}
