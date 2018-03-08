﻿using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.DependencyInjection;

namespace NedlastingKlient
{
    public class ProtectionService
    {
        private static readonly IDataProtector _protector = AddDataProtectionService();

        public static string CreateProtectedPassword(string unprotectedPassword)
        {
            //IDataProtector protector = AddDataProtectionService();
            return _protector.Protect(unprotectedPassword);
        }

        private static IDataProtector AddDataProtectionService()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddDataProtection();
            var services = serviceCollection.BuildServiceProvider();
            IDataProtector protector = services.GetDataProtector("Auth.Download");
            return protector;
        }

        public static string GetUnprotectedPassword(string protectedPassword)
        {
            //IDataProtector protector = AddDataProtectionService();
            return _protector.Unprotect(protectedPassword);
        }
    }
}