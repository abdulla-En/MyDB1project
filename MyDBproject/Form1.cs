using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MyDBproject.BLL; 
using MyDBproject.Models;

namespace MyDBproject
{
    public partial class Form1 : Form
    {
        
        private AppService _service = new AppService();

        public Form1()
        {
            InitializeComponent();
        }

        # region              __ INSERT SECTION  __
        // Insert Student
        private void btnAddStudent_Click(object sender, EventArgs e)
        {
            try
            {
                Student st = new Student
                {
                    FirstName = textFirstname.Text,
                    LastName = textLastname.Text,
                    Email = textEmail.Text,     
                };
                string message = _service.AddStudent(st);
                ShowMsg(message);
            }
            catch (Exception ex) { ShowMsg("Error: " + ex.Message); }
        }

        // Insert Program
        private void btnAddProgram_Click(object sender, EventArgs e)
        {
            try
            {
                ProgramModel pm = new ProgramModel
                {
                    Title = textProgTittle.Text,
                    CategoryId = int.Parse(textCatId.Text),
                    InstructorId = int.Parse(textInstId.Text),
                    Difficulty = textdifficulty.Text,
                    Fee = decimal.Parse(textProgFee.Text),
                
                };
                string message = _service.AddProgram(pm);
                ShowMsg(message);
            }
            catch (Exception ex) { ShowMsg("Error: " + ex.Message); }
        }
        #endregion


        #region              __ UPDATE SECTION  __

        // Update Program fee
        private void btnUpdateFee_Click(object sender, EventArgs e)
        {
            try
            {
                int pId = int.Parse(textUpFeePId.Text);
                decimal fee = decimal.Parse(textNewFee.Text);
                string message = _service.UpdateFee(pId, fee);
                ShowMsg(message);
            }
            catch (Exception ex) { ShowMsg("Error: " + ex.Message); }
        }

        // Update Enrollment progress
        private void btnUpdateProgress_Click(object sender, EventArgs e)
        {
            try
            {
                int id = int.Parse(textUpEnrolId.Text);
                int units = int.Parse(textUnitsComp.Text);
                string message = _service.UpdateProgress(id, units);
                ShowMsg(message);
            }
            catch (Exception ex) { ShowMsg("Error: " + ex.Message); }
        }
        #endregion


        # region              __ DELETE SECTION  __

        // Delete Enrollment
        private void btnDeleteEnrollment_Click(object sender, EventArgs e)
        {
            try
            {
                int id = int.Parse(textDelEnrolId.Text);
                string message = _service.DeleteEnrollment(id);
                ShowMsg(message);
            }
            catch (Exception ex) { ShowMsg("Error: " + ex.Message); }
        }

        // Delete Unit
        private void btnDeleteUnit_Click(object sender, EventArgs e)
        {
            try
            {
                int pid = int.Parse(textDelUnitPID.Text);
                int sn = int.Parse(textDelUnitSn.Text);
                string message = _service.DeleteUnit(pid, sn);
                ShowMsg(message);
            }
            catch (Exception ex) { ShowMsg("Error: " + ex.Message); }
        }
        #endregion



        # region              __ SELECT SECTION  __

        // display all instruvtors
        private void btnShowInstructors_Click(object sender, EventArgs e)
        {
            dgvSimple.DataSource = _service.GetInstructors();
        }

        // display students with their programs and date of enrollment
        private void btnShowEnrollments_Click(object sender, EventArgs e)
        {
            dgvJoin.DataSource = _service.GetEnrollmentsJoin();
        }

        #endregion

        private void ShowMsg(string msg)
        {
            lblStatus.Text = msg;
            lblStatus.ForeColor = msg.StartsWith("Error") ? System.Drawing.Color.Red : System.Drawing.Color.Green;
        }
    }
}
