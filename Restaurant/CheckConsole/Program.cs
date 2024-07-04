using DBAccess;
using Project_s_classes;
using System.Data.Common;
using System.Reflection.Metadata;

//all the things in here are just test to see the output of the codes

public class Sample
{
    public int UserID { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string MobileNumber { get; set; }

    public string Email { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }

    public string UserType { get; set; }

    public string Address { get; set; }

    public string Gender { get; set; }
}

class Program
{
    static void Main(string[] args)
    {
        //A test to read data
        
        DataAccess dataAccess = new DataAccess();
    }
}