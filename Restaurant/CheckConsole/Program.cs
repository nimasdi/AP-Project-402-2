using DBAccess;
using System.Data.Common;

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
        string sqlStatement = "SELECT * FROM dbo.Users";
        DataAccess dataAccess = new DataAccess();

        List<Sample> rows = dataAccess.LoadData<Sample, dynamic>(sqlStatement, new { });
        foreach(Sample row in rows)
        {
            Console.WriteLine(row.UserID + " " + row.FirstName + " " + row.LastName);
        }

        //Sample for inserting into a table
        Sample new_sample = new Sample
        {
            FirstName = "Nima",
            LastName = "Saeedi",
            MobileNumber = "1234556",
            Email = "something@gmail.com",
            UserName = "nima",
            Password = "Password",
            UserType = "Gold",
            Address = "shahrak gharb",
            Gender = "Male"
        };

        string sqlS = "INSERT INTO dbo.Users (FirstName, LastName, MobileNumber, Email, UserName, Password, UserType,Address, Gender)" +
            " VALUES(@FirstName, @LastName, @MobileNumber, @Email, @UserName, @Password, @UserType,@Address, @Gender);";
        dataAccess.SaveData(sqlS, new_sample);

    }
}