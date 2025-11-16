using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsApp2.Database;
using WindowsFormsApp2.Models;

namespace WindowsFormsApp2
{
    public partial class Form1 : Form
    {
        private DataAccess dataAccess;

        public Form1()
        {
            InitializeComponent();
            dataAccess = new DataAccess();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                // Initialize database
                DatabaseHelper.InitializeDatabase();
                
                // Load repair requests
                LoadRepairRequests();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка при завантаженні даних: {ex.Message}", 
                    "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadRepairRequests()
        {
            try
            {
                var requests = dataAccess.GetAllRepairRequests();
                
                // Create a DataTable for display
                DataTable dt = new DataTable();
                dt.Columns.Add("№", typeof(int));
                dt.Columns.Add("Клієнт", typeof(string));
                dt.Columns.Add("Пристрій", typeof(string));
                dt.Columns.Add("Проблема", typeof(string));
                dt.Columns.Add("Технік", typeof(string));
                dt.Columns.Add("Статус", typeof(string));
                dt.Columns.Add("Дата прийому", typeof(string));
                dt.Columns.Add("Вартість", typeof(string));

                foreach (var request in requests)
                {
                    dt.Rows.Add(
                        request.RequestID,
                        request.ClientName,
                        request.DeviceName,
                        request.ProblemDescription,
                        request.TechnicianName,
                        request.StatusName,
                        request.DateReceived.ToString("dd.MM.yyyy HH:mm"),
                        request.EstimatedCost.HasValue ? $"{request.EstimatedCost.Value:N2} грн" : "-"
                    );
                }

                dataGridViewRequests.DataSource = dt;
                
                // Update statistics
                lblStats.Text = $"Всього заявок: {requests.Count}";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка при завантаженні заявок: {ex.Message}", 
                    "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadRepairRequests();
        }

        private void btnAddRequest_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Форма додавання заявки буде реалізована", 
                "Інформація", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
