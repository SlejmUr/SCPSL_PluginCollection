using Exiled.API.Interfaces;
using System;

namespace SimpleCustomRoles
{
    public class Config : IConfig
    {
        public bool IsEnabled { get; set; }
        public bool Debug { get; set; }
    }
}
