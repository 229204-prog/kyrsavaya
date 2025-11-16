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

namespace WindowsFormsApp2.Forms
{
    public partial class AddRequestForm : Form
    {
        private DataAccess dataAccess;

        public AddRequestForm()
        {
            InitializeComponent();
            dataAccess = new DataAccess();
        }

        private void AddRequestForm_Load(object sender, EventArgs e)
        {
            try
            {
                LoadComboBoxes();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка при завантаженні даних: {ex.Message}", 
                    "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadComboBoxes()
        {
            // Load clients
            var clients = dataAccess.GetAllClients();
            cmbClient.DataSource = clients;
            cmbClient.DisplayMember = "FullName";
            cmbClient.ValueMember = "ClientID";

            // Load devices
            var devices = dataAccess.GetAllDevices();
            cmbDevice.DataSource = devices;
            cmbDevice.DisplayMember = "FullDescription";
            cmbDevice.ValueMember = "DeviceID";

            // Load technicians
            var technicians = dataAccess.GetAllTechnicians();
            cmbTechnician.DataSource = technicians;
            cmbTechnician.DisplayMember = "FullName";
            cmbTechnician.ValueMember = "TechnicianID";

            // Load statuses
            var statuses = dataAccess.GetAllRepairStatuses();
            cmbStatus.DataSource = statuses;
            cmbStatus.DisplayMember = "StatusName";
            cmbStatus.ValueMember = "StatusID";
            
            // Set default status to "Нова" (first one)
            if (statuses.Count > 0)
                cmbStatus.SelectedIndex = 0;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (ValidateInputs())
                {
                    var request = new RepairRequest
                    {
                        ClientID = (int)cmbClient.SelectedValue,
                        DeviceID = (int)cmbDevice.SelectedValue,
                        TechnicianID = (int)cmbTechnician.SelectedValue,
                        StatusID = (int)cmbStatus.SelectedValue,
                        ProblemDescription = txtProblem.Text.Trim(),
                        DateReceived = dtpDateReceived.Value,
                        EstimatedCost = string.IsNullOrWhiteSpace(txtEstimatedCost.Text) ? 
                            (decimal?)null : decimal.Parse(txtEstimatedCost.Text),
                        Notes = txtNotes.Text.Trim()
                    };

                    int newId = dataAccess.AddRepairRequest(request);
                    
                    MessageBox.Show($"Заявку №{newId} успішно створено!", 
                        "Успіх", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка при збереженні заявки: {ex.Message}", 
                    "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool ValidateInputs()
        {
            if (cmbClient.SelectedValue == null)
            {
                MessageBox.Show("Будь ласка, оберіть клієнта", "Увага", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (cmbDevice.SelectedValue == null)
            {
                MessageBox.Show("Будь ласка, оберіть пристрій", "Увага", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (cmbTechnician.SelectedValue == null)
            {
                MessageBox.Show("Будь ласка, оберіть техніка", "Увага", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtProblem.Text))
            {
                MessageBox.Show("Будь ласка, опишіть проблему", "Увага", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (!string.IsNullOrWhiteSpace(txtEstimatedCost.Text))
            {
                decimal cost;
                if (!decimal.TryParse(txtEstimatedCost.Text, out cost) || cost < 0)
                {
                    MessageBox.Show("Будь ласка, введіть коректну вартість", "Увага", 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
            }

            return true;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
