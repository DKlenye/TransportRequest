﻿using System;
using System.Linq;
using System.DirectoryServices.AccountManagement;
using Intranet.Models;

namespace Intranet.Utils
{
    public class AccountManager
    {
        public static string GetUserDisplayName(string identity)
        {
            string displayName = String.Empty;
            PrincipalContext context;

            if (String.IsNullOrEmpty(identity)) return "";

            if (identity.Substring(0, 7).ToLower() == "polymir")            
                context = new PrincipalContext(ContextType.Domain, "POLYMIR.NET");            
            else            
                context = new PrincipalContext(ContextType.Domain, "lan.naftan.by");
            
            using (context)
            {
                var principal = UserPrincipal.FindByIdentity(context, identity);
                displayName = principal.DisplayName;
            }
            return displayName;
        }

        /// <summary>
        /// Is current user has permissions to approve transport requests
        /// </summary>
        /// <param name="identity"></param>
        /// <returns>Tuple where Item1 - EmloyeeId, Item2 - is this user approver </returns>
        public static Tuple<int, bool> IsApprover(string identity)
        {             
            var _db = new transportEntities();

            if (String.IsNullOrEmpty(identity))
            {
                return new Tuple<int, bool>(10,true);
            }


            //bool result = false;

            int employeeId = 0;
            var context = new PrincipalContext(ContextType.Domain, "lan.naftan.by");

            if (identity.Substring(0, 7).ToLower() == "polymir") //If user from Polymir than MAGIC
            {
                //парсить кусок логина на предмет табельного номера
                string tabN = identity.Substring(9, identity.Length - 9);
                int n = 0;
                for (int i = 0; i < tabN.Length; i++)
                {
                    if (tabN.Substring(i, 1) != "0")
                    {
                        n = i;
                        break;
                    }
                }
                tabN = tabN.Substring(n, tabN.Length - n); //предполагаемый табельный номер

                var empl = _db.Employees.Where(e => e.EmployeeNumber == tabN).SingleOrDefault();
                if (empl != null)
                    employeeId = empl.EmployeeId;
            }
            else //User from Naftan, do search in AD
            {
                string sIdMan = String.Empty;
                int idMan = 0;
                using (context)
                {
                    var principal = UserPrincipal.FindByIdentity(context, identity);
                    sIdMan = principal.EmployeeId;
                }
                Int32.TryParse(sIdMan, out idMan);
                var empl = _db.Employees.Where(e => e.id_men == idMan).SingleOrDefault();
                if (empl != null)
                    employeeId = empl.EmployeeId;
                else //Если полимировец с логином из домена LAN
                {
                    //парсить кусок логина на предмет табельного номера
                    string tabN = identity.Substring(5, identity.Length - 5);
                    int n = 0;
                    for (int i = 0; i < tabN.Length; i++)
                    {
                        if (tabN.Substring(i, 1) != "0")
                        {
                            n = i;
                            break;
                        }
                    }
                    tabN = tabN.Substring(n, tabN.Length - n); //предполагаемый табельный номер 
                    var empl1 = _db.Employees.Where(e => e.EmployeeNumber == tabN).SingleOrDefault();
                    if (empl1 != null)
                        employeeId = empl1.EmployeeId;
                }
            }

            if (employeeId == 0)
                return Tuple.Create(employeeId, false);
            
            bool isApprover = _db.RequestApprovers.Any(ap => ap.EmployeeId == employeeId);            

            return Tuple.Create(employeeId, isApprover);
        }       
    }
}