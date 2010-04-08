using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Energy.Library;


namespace Energy.EnergyWatcher
{
    /// <summary>
    /// Confirmation dialog asking if the Rate should really be added
    /// </summary>
    public partial class AddRateConfirmForm : Form
    {
        Rate rate = null;
        Rate newRate = null;
        RateForm rateForm;

        public AddRateConfirmForm(Rate currentRate, RateForm parent)
        {
            InitializeComponent();
            
            rate = currentRate;
            rateForm = parent;
        }

        // cancel button
        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void butOK_Click(object sender, EventArgs e)
        {
            int newID;

            // Override the name in the object and save
            newRate = (Rate)rate.Clone();
            newRate.Name = txtName.Text;
            newRate.IsCurrentlySelectedRate = false;
            DatabaseController.InsertRate(newRate);
            newID = DatabaseController.GetLastInsertedID("Rate");
            newRate.ID = newID;

            // Call the parent Loader to refresh the Rate Dropdown and the form values
            rateForm.LoadRateFormData(newRate);

            this.Close();
        }
    }
}
