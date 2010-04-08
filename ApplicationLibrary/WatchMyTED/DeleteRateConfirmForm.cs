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
    /// Form to confirm the deletion of a Rate
    /// </summary>
    public partial class DeleteRateConfirmForm : Form
    {
        Rate rate = null;
        RateForm rateForm;

        public DeleteRateConfirmForm(Rate currentRate, RateForm parent)
        {
            InitializeComponent();

            rate = currentRate;
            rateForm = parent;
        }

        private void butCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void butOK_Click(object sender, EventArgs e)
        {
            if (DatabaseController.GetRateCount() > 1)
            {
                DatabaseController.DeleteRate(rate);
                rateForm.LoadRateFormData();
            }

            this.Close();
        }
    }
}
