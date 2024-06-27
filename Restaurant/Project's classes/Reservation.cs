using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DBAccess;

namespace Project_s_classes
{
    public class Reservation
    {
        int ReservationID { get; set; }
        int? UserID { get; set; }
        int? RestaurantID { get; set; }
        DateTime ReservationDate { get; set; }
        TimeSpan StartTime { get; set; }
        int Duration { get; set; }
        string Status { get; set; }
        string ServiceType { get; set; }
        static DataAccess dataAccess = new DataAccess();

        public Reservation(int? userID, int? restaurantID, int duration, string status, string serviceType)
        {
            this.UserID = userID;
            this.RestaurantID = restaurantID;
            this.ReservationDate = DateTime.Now;
            this.StartTime = DateTime.Now.TimeOfDay;
            this.Duration = duration;
            this.Status = status;   
            this.ServiceType = serviceType;

            //saving data to the data base
            string sqlStatement = "INSERT INTO dbo.Reservation(UserID, RestaurantID, ReservationDate, StartTime, Duration, Status, ServiceType)" +
                "VALUES(@UserID, @RestaurantID, @ReservationDate, @StartTime, @Duration, @Status, @ServiceType)";
            this.ReservationID = dataAccess.SaveData(sqlStatement, this, true);

        }
    }
}
