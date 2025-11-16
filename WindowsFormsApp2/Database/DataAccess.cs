using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using WindowsFormsApp2.Models;

namespace WindowsFormsApp2.Database
{
    public class DataAccess
    {
        private string connectionString;

        public DataAccess()
        {
            connectionString = DatabaseHelper.ConnectionString;
        }

        public List<RepairRequest> GetAllRepairRequests()
        {
            List<RepairRequest> requests = new List<RepairRequest>();

            string query = @"
                SELECT 
                    rr.RequestID,
                    rr.ClientID,
                    rr.DeviceID,
                    rr.TechnicianID,
                    rr.StatusID,
                    rr.ProblemDescription,
                    rr.DateReceived,
                    rr.DateCompleted,
                    rr.EstimatedCost,
                    rr.ActualCost,
                    rr.Notes,
                    c.FirstName + ' ' + c.LastName AS ClientName,
                    d.Brand + ' ' + d.Model AS DeviceName,
                    t.FirstName + ' ' + t.LastName AS TechnicianName,
                    rs.StatusName
                FROM RepairRequests rr
                INNER JOIN Clients c ON rr.ClientID = c.ClientID
                INNER JOIN Devices d ON rr.DeviceID = d.DeviceID
                INNER JOIN Technicians t ON rr.TechnicianID = t.TechnicianID
                INNER JOIN RepairStatuses rs ON rr.StatusID = rs.StatusID
                ORDER BY rr.DateReceived DESC";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    requests.Add(new RepairRequest
                    {
                        RequestID = reader.GetInt32(0),
                        ClientID = reader.GetInt32(1),
                        DeviceID = reader.GetInt32(2),
                        TechnicianID = reader.GetInt32(3),
                        StatusID = reader.GetInt32(4),
                        ProblemDescription = reader.GetString(5),
                        DateReceived = reader.GetDateTime(6),
                        DateCompleted = reader.IsDBNull(7) ? (DateTime?)null : reader.GetDateTime(7),
                        EstimatedCost = reader.IsDBNull(8) ? (decimal?)null : reader.GetDecimal(8),
                        ActualCost = reader.IsDBNull(9) ? (decimal?)null : reader.GetDecimal(9),
                        Notes = reader.IsDBNull(10) ? null : reader.GetString(10),
                        ClientName = reader.GetString(11),
                        DeviceName = reader.GetString(12),
                        TechnicianName = reader.GetString(13),
                        StatusName = reader.GetString(14)
                    });
                }
            }

            return requests;
        }

        public List<Client> GetAllClients()
        {
            List<Client> clients = new List<Client>();
            string query = "SELECT ClientID, FirstName, LastName, PhoneNumber, Email, Address FROM Clients ORDER BY LastName, FirstName";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    clients.Add(new Client
                    {
                        ClientID = reader.GetInt32(0),
                        FirstName = reader.GetString(1),
                        LastName = reader.GetString(2),
                        PhoneNumber = reader.GetString(3),
                        Email = reader.IsDBNull(4) ? null : reader.GetString(4),
                        Address = reader.IsDBNull(5) ? null : reader.GetString(5)
                    });
                }
            }

            return clients;
        }

        public List<Technician> GetAllTechnicians()
        {
            List<Technician> technicians = new List<Technician>();
            string query = "SELECT TechnicianID, FirstName, LastName, Specialization, PhoneNumber FROM Technicians ORDER BY LastName, FirstName";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    technicians.Add(new Technician
                    {
                        TechnicianID = reader.GetInt32(0),
                        FirstName = reader.GetString(1),
                        LastName = reader.GetString(2),
                        Specialization = reader.IsDBNull(3) ? null : reader.GetString(3),
                        PhoneNumber = reader.IsDBNull(4) ? null : reader.GetString(4)
                    });
                }
            }

            return technicians;
        }

        public List<Device> GetAllDevices()
        {
            List<Device> devices = new List<Device>();
            string query = @"
                SELECT d.DeviceID, d.DeviceTypeID, d.Brand, d.Model, d.SerialNumber, dt.TypeName
                FROM Devices d
                INNER JOIN DeviceTypes dt ON d.DeviceTypeID = dt.DeviceTypeID
                ORDER BY d.Brand, d.Model";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    devices.Add(new Device
                    {
                        DeviceID = reader.GetInt32(0),
                        DeviceTypeID = reader.GetInt32(1),
                        Brand = reader.IsDBNull(2) ? null : reader.GetString(2),
                        Model = reader.IsDBNull(3) ? null : reader.GetString(3),
                        SerialNumber = reader.IsDBNull(4) ? null : reader.GetString(4),
                        DeviceTypeName = reader.GetString(5)
                    });
                }
            }

            return devices;
        }

        public List<RepairStatus> GetAllRepairStatuses()
        {
            List<RepairStatus> statuses = new List<RepairStatus>();
            string query = "SELECT StatusID, StatusName, Description FROM RepairStatuses ORDER BY StatusID";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    statuses.Add(new RepairStatus
                    {
                        StatusID = reader.GetInt32(0),
                        StatusName = reader.GetString(1),
                        Description = reader.IsDBNull(2) ? null : reader.GetString(2)
                    });
                }
            }

            return statuses;
        }

        public int AddRepairRequest(RepairRequest request)
        {
            string query = @"
                INSERT INTO RepairRequests (ClientID, DeviceID, TechnicianID, StatusID, ProblemDescription, DateReceived, EstimatedCost, Notes)
                VALUES (@ClientID, @DeviceID, @TechnicianID, @StatusID, @ProblemDescription, @DateReceived, @EstimatedCost, @Notes);
                SELECT CAST(SCOPE_IDENTITY() as int)";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@ClientID", request.ClientID);
                command.Parameters.AddWithValue("@DeviceID", request.DeviceID);
                command.Parameters.AddWithValue("@TechnicianID", request.TechnicianID);
                command.Parameters.AddWithValue("@StatusID", request.StatusID);
                command.Parameters.AddWithValue("@ProblemDescription", request.ProblemDescription);
                command.Parameters.AddWithValue("@DateReceived", request.DateReceived);
                command.Parameters.AddWithValue("@EstimatedCost", (object)request.EstimatedCost ?? DBNull.Value);
                command.Parameters.AddWithValue("@Notes", (object)request.Notes ?? DBNull.Value);

                connection.Open();
                return (int)command.ExecuteScalar();
            }
        }

        public void UpdateRepairRequestStatus(int requestID, int statusID)
        {
            string query = "UPDATE RepairRequests SET StatusID = @StatusID WHERE RequestID = @RequestID";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@StatusID", statusID);
                command.Parameters.AddWithValue("@RequestID", requestID);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }
    }
}
