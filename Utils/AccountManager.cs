using System;
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

        public static string GetUserPhoneNumber(string identity)
        {
            string phone = String.Empty;
            PrincipalContext context;

            if (String.IsNullOrEmpty(identity)) return "";

            if (identity.Substring(0, 7).ToLower() == "polymir")
                context = new PrincipalContext(ContextType.Domain, "POLYMIR.NET");
            else
                context = new PrincipalContext(ContextType.Domain, "lan.naftan.by");

            using (context)
            {
                var principal = UserPrincipal.FindByIdentity(context, identity);
                phone = principal.VoiceTelephoneNumber;
            }
            return phone;
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

            var login = Login(identity);
            Employee employee = _db.Employees.SingleOrDefault(e => e.Login == login);
            if (employee == null)
            {
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
                    employee = _db.Employees.SingleOrDefault(e => e.EmployeeNumber == tabN);

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
                    employee = _db.Employees.SingleOrDefault(e => e.id_men == idMan);

                    if (employee == null)
                        //Если полимировец с логином из домена LAN
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
                        employee = _db.Employees.SingleOrDefault(e => e.EmployeeNumber == tabN);
                    }
                }

                if (employee != null)
                {
                    employee.Login = login;
                    _db.SaveChanges();
                }

            }

            if (employee == null)
            {
                return Tuple.Create(0, false);
            }
            
            bool isApprover = _db.RequestApprovers.Any(ap => ap.EmployeeId == employee.EmployeeId);
            return Tuple.Create(employee.EmployeeId, isApprover);
        }

        private const char DomainSplitter = '\\';
        private static string Login(string identity)
        {
              return identity.Split(DomainSplitter)[1]; 
        }


    }
}