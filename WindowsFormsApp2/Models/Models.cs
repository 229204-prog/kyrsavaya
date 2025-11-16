using System;

namespace WindowsFormsApp2.Models
{
    public class RepairRequest
    {
        public int RequestID { get; set; }
        public int ClientID { get; set; }
        public int DeviceID { get; set; }
        public int TechnicianID { get; set; }
        public int StatusID { get; set; }
        public string ProblemDescription { get; set; }
        public DateTime DateReceived { get; set; }
        public DateTime? DateCompleted { get; set; }
        public decimal? EstimatedCost { get; set; }
        public decimal? ActualCost { get; set; }
        public string Notes { get; set; }

        // Navigation properties
        public string ClientName { get; set; }
        public string DeviceName { get; set; }
        public string TechnicianName { get; set; }
        public string StatusName { get; set; }
    }

    public class Client
    {
        public int ClientID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }

        public string FullName => $"{FirstName} {LastName}";
    }

    public class Technician
    {
        public int TechnicianID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Specialization { get; set; }
        public string PhoneNumber { get; set; }

        public string FullName => $"{FirstName} {LastName}";
    }

    public class Device
    {
        public int DeviceID { get; set; }
        public int DeviceTypeID { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public string SerialNumber { get; set; }

        public string DeviceTypeName { get; set; }
        public string FullDescription => $"{Brand} {Model}";
    }

    public class DeviceType
    {
        public int DeviceTypeID { get; set; }
        public string TypeName { get; set; }
        public string Description { get; set; }
    }

    public class RepairStatus
    {
        public int StatusID { get; set; }
        public string StatusName { get; set; }
        public string Description { get; set; }
    }
}
