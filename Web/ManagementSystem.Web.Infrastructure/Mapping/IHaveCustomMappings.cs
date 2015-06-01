using AutoMapper;
using System;
using System.Linq;

namespace ManagementSystem.Web.Infrastructure.Mapping
{
    public interface IHaveCustomMappings
    {
        void CreateMappings(IConfiguration configuration);
    }
}
