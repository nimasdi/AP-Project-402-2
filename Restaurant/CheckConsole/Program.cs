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

public enum ServiceType
{
    delivery = 1,
    dine_in = 2,
}
class Program
{
    static void Main(string[] args)
    {
        //A test to read data
        
        DataAccess dataAccess = new DataAccess();

        //Complaint complaint = new Complaint(null, 13, 1, "Test title", "Complaint");
        //Complaint complaint2 = new Complaint(null, 13, 1, "Test title2", "Complaint2");

        Restaurants restaurant1 = new Restaurants(
                restaurantId: null,
                name: "Gourmet Delight",
                city: "New York",
                averageRating: 4.5f,
                isReservationEnabled: true,
                serviceType: ServiceType.dine_in.ToString(),
                adminID: 1,
                password: 1234,
                userName: "gourmet_delight"
            );

        Restaurants restaurant2 = new Restaurants(
            restaurantId: null,
            name: "Pizza Palace",
            city: "Chicago",
            averageRating: 4.0f,
            isReservationEnabled: false,
            serviceType: ServiceType.delivery.ToString(),
            adminID: 2,
            password: 5678,
            userName: "pizza_palace"
        );

        Restaurants restaurant3 = new Restaurants(
            restaurantId: null,
            name: "Sushi World",
            city: "San Francisco",
            averageRating: 4.8f,
            isReservationEnabled: true,
            serviceType: ServiceType.dine_in.ToString(),
            adminID: 3,
            password: 91011,
            userName: "sushi_world"
        );

        Restaurants restaurant4 = new Restaurants(
            restaurantId: null,
            name: "Burger Hub",
            city: "Los Angeles",
            averageRating: 4.2f,
            isReservationEnabled: false,
            serviceType: ServiceType.delivery.ToString(),
            adminID: 4,
            password: 1213,
            userName: "burger_hub"
        );
    }
}