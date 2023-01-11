using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;

namespace LDAP_ConsoleApp
{
    internal class Program
    {
      
        static void Main(string[] args)
        {
        
            IsAuth();
            print();

        }
       

        static bool IsAuth()
        {
           bool Valid = false;

            try
            {
                DirectoryEntry entry = new DirectoryEntry("LDAP://WIN-9FIKOFC9GQF", "GECEVIC\\matko", "Pa$$w0rd");
                object nativeObj = entry.NativeObject;
                Valid = true;
            }
            catch (DirectoryServicesCOMException comex)
            {
                //Not Authenticated. comex.Message will return the reason
            }
            return Valid;

        }
        static void print()
        {
            if(IsAuth() == true)
            {
                Console.WriteLine("User is authenticated!");
            }
            else 
            {
                Console.WriteLine("User is not authenticated!");
            }
        }

        public static List<String> getGroupUsers(String strGroup)
        {
            List<String> groupMembers = new List<String>();

            PrincipalContext contex = new PrincipalContext(ContextType.Domain, "192.168.100.64", "GECEVIC\\Administrator", "Pa$$w0rd");

            GroupPrincipal Group = GroupPrincipal.FindByIdentity(contex, strGroup);

            if (Group != null)
            {
                PrincipalSearchResult<Principal> allMembers = Group.GetMembers(true);

                foreach (Principal principal in allMembers)
                {
                    String login = principal.SamAccountName;
                    String display = principal.DisplayName;
                    String user = principal.Description;
                    groupMembers.Add("SAN: " + login + ", DISPLAY NAME: " + display + ", DESCRIPTION: " + user);
                }
            }
            else
            {
                throw new Exception("Users group not found!");
            }

            return groupMembers;
        }
    }
}
