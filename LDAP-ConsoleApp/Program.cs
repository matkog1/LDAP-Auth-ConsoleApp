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
            //varijable za login
            string user;
            string password;
            
            //upit za user
            Console.WriteLine("User:");
            user = Console.ReadLine();

            //upit za password
            Console.WriteLine("Password: ");
            password = Console.ReadLine();

            //autentikacija usera
            bool auth = AuthenticateUser( user, password);

            //print uspjesne/neuspjesne autentikacije
            print(auth);
            getGroupUsers("Studenti");
            
        }

        // funkcija provjere autentikacije koja prima uneseni user i password
         static bool AuthenticateUser(string user, string password)
        {
            bool isValid = false;
            try
            {
                // provjera usera i passworda

                DirectoryEntry de = new DirectoryEntry("LDAP://WIN-0R2TTEN8F02","GECEVIC\\"+user, password);
                object nativeObj = de.NativeObject;
                isValid = true;
            }
            catch
            {
                isValid = false;
            }

            return isValid;
        }

        //print funkcija koja prima bool auth(user logiran ili ne) te prema toma ispisuje odgovarajuću poruku
        static void print(bool auth)
        {
            if (auth == true)
            {
                Console.WriteLine("User authenticated!");
            }
            else
            {
                Console.WriteLine("User or password is wrong!");
            }
        }

        // funkcija provjere svih uneseni usera u LDAP
        public static List<String> getGroupUsers(String strGroup)
        {
            List<String> groupMembers = new List<String>();

            PrincipalContext contex = new PrincipalContext(ContextType.Domain, "192.168.100.69", "GECEVIC\\Administrator", "Pa$$w0rd");

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
