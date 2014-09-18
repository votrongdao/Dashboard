using System;
namespace DashboardSite.Core.Interface
{
    public interface IUserPrincipal
    {
        int UserId { get; }
        string UserName { get; }
        string UserRole { get; }
    }
}
