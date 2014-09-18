using DashboardSite.Model;
using System;

namespace DashboardSite.Repository.Interface
{
    public interface IEmailRepository
    {
        void SaveEmail(EmailModel email);
    }
}
