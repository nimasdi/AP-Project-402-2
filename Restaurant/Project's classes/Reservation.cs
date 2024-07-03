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
        public int? ReservationID { get; set; }
        public int? UserID { get; set; }
        public int? RestaurantID { get; set; }
        public DateTime ReservationDate { get; set; }
        public TimeSpan StartTime { get; set; }
        public int Duration { get; set; }
        public string Status { get; set; }
        public string ServiceType { get; set; }
        static DataAccess dataAccess = new DataAccess();

        public Reservation(int? reservationId,int? userID, int? restaurantID, int duration, string status, string serviceType)
        {
            ReservationID = reservationId;
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
