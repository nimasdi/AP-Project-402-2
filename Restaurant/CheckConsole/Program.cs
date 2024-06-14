using DBAccess;
using System.Data.Common;
class Program
{
    static void Main(string[] args)
    {
        DB instance = new DB();
        instance.Main(args);
    }
}