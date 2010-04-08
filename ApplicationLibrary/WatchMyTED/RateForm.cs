using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using Energy.Library;
using System.Windows.Forms;

namespace Energy.EnergyWatcher
{
    /// <summary>
    /// Form to add and edit rate information
    /// </summary>
    public partial class RateForm : Form
    {
        private Configuration config = null;
        private Rate currentRate = null;
        private int rateid = -1;
        DataTable peakStartHourList;
        DataTable peakEndHourList;
        DataTable peakWeekendStartHourList;
        DataTable peakWeekendEndHourList;
        DataTable peakSummerStartHourList;
        DataTable peakSummerEndHourList;
        DataTable peakWinterStartHourList;
        DataTable peakWinterEndHourList;
        DataTable peakSummerWeekendStartHourList;
        DataTable peakSummerWeekendEndHourList;
        DataTable peakWinterWeekendStartHourList;
        DataTable peakWinterWeekendEndHourList;
        DataTable rateList;
        bool initialRateDropdownLoad = false;

        public RateForm()
        {
            InitializeComponent();

            initialRateDropdownLoad = true;
            InitializeCurrentRate();
            LoadRateFormData(currentRate);
            initialRateDropdownLoad = false;
        }

        public void InitializeCurrentRate()
        {
            config = DatabaseController.GetConfiguration();
            currentRate = DatabaseController.GetRate(config.RateID);

            if (currentRate == null)
            {
                // could not find rate that was referenced as currentRate from config
                // take first rateID available and assign to config, and use that here
                rateid = DatabaseController.GetTopRateID();
                if (rateid != -1)
                {
                    config.RateID = rateid;
                    DatabaseController.UpdateSelectedRateConfiguration("default", config);
                    currentRate = DatabaseController.GetRate(config.RateID);
                }
                else
                {
                    // Add a new default rate, since there are no current rates available
                    DatabaseController.InsertRate();
                    rateid = DatabaseController.GetTopRateID();
                    config.RateID = rateid;
                    DatabaseController.UpdateSelectedRateConfiguration("default", config);
                    currentRate = DatabaseController.GetRate(config.RateID);
                }
            }
        }

        public void LoadRateFormData()
        {
            InitializeCurrentRate();
            LoadRateFormData(currentRate);
        }

        public void LoadRateFormData(Rate rate)
        {
            chkUseThisAsCurrent.Checked = rate.IsCurrentlySelectedRate;
            chkSeasonal.Checked = rate.IsSeasonal;
            chkBasic.Checked = rate.HasBasicCharges;
            chkTOU.Checked = rate.HasTimeOfUseCharges;

            txtSummerStart.Text = rate.SummerStartMonth;
            txtSummerEnd.Text = rate.SummerEndMonth;

            txtBasicRate.Text = rate.BasicCharge.ToString();
            txtBasicRateSummer.Text = rate.SummerBasicCharge.ToString();
            txtBasicRateWinter.Text = rate.WinterBasicCharge.ToString();

            txtWeekdayPeakCharge.Text = rate.PeakCharge.ToString();
            txtWeekdayOffPeakCharge.Text = rate.OffPeakCharge.ToString();
            txtWeekendPeakCharge.Text = rate.PeakWeekendCharge.ToString();
            txtWeekendOffPeakCharge.Text = rate.OffPeakWeekendCharge.ToString();

            txtSummerWeekdayPeakCharge.Text = rate.SummerPeakCharge.ToString();
            txtSummerWeekdayOffPeakCharge.Text = rate.SummerOffPeakCharge.ToString();
            txtWinterWeekdayPeakCharge.Text = rate.WinterPeakCharge.ToString();
            txtWinterWeekdayOffPeakCharge.Text = rate.WinterOffPeakCharge.ToString();

            txtSummerWeekendPeakCharge.Text = rate.SummerPeakWeekendCharge.ToString();
            txtSummerWeekendOffPeakCharge.Text = rate.SummerOffPeakWeekendCharge.ToString();
            txtWinterWeekendPeakCharge.Text = rate.WinterPeakWeekendCharge.ToString();
            txtWinterWeekendOffPeakCharge.Text = rate.WinterOffPeakWeekendCharge.ToString();

            // load all the dropdowns
            peakStartHourList = DatabaseController.GetSelectTime();
            LoadComboBox(ddWeekdayPeakStart, peakStartHourList);
            ddWeekdayPeakStart.SelectedValue = rate.PeakStartHour;

            peakEndHourList = DatabaseController.GetSelectTime();
            LoadComboBox(ddWeekdayPeakEnd, peakEndHourList);
            ddWeekdayPeakEnd.SelectedValue = rate.PeakEndHour;

            peakWeekendStartHourList = DatabaseController.GetSelectTime();
            LoadComboBox(ddWeekendPeakStart, peakWeekendStartHourList);
            ddWeekendPeakStart.SelectedValue = rate.PeakWeekendStartHour;

            peakWeekendEndHourList = DatabaseController.GetSelectTime();
            LoadComboBox(ddWeekendPeakEnd, peakWeekendEndHourList);
            ddWeekendPeakEnd.SelectedValue = rate.PeakWeekendEndHour;

            // peakSummerStartHourList;
            peakSummerStartHourList = DatabaseController.GetSelectTime();
            LoadComboBox(ddSummerWeekdayPeakStart, peakSummerStartHourList);
            ddSummerWeekdayPeakStart.SelectedValue = rate.SummerPeakStartHour;

            // peakSummerEndHourList;
            peakSummerEndHourList = DatabaseController.GetSelectTime();
            LoadComboBox(ddSummerWeekdayPeakEnd, peakSummerEndHourList);
            ddSummerWeekdayPeakEnd.SelectedValue = rate.SummerPeakEndHour;

            // peakWinterStartHourList;
            peakWinterStartHourList = DatabaseController.GetSelectTime();
            LoadComboBox(ddWinterWeekdayPeakStart, peakWinterStartHourList);
            ddWinterWeekdayPeakStart.SelectedValue = rate.WinterPeakStartHour;

            // peakWinterEndHourList;
            peakWinterEndHourList = DatabaseController.GetSelectTime();
            LoadComboBox(ddWinterWeekdayPeakEnd, peakWinterEndHourList);
            ddWinterWeekdayPeakEnd.SelectedValue = rate.WinterPeakEndHour;

            // peakSummerWeekendStartHourList;
            peakSummerWeekendStartHourList = DatabaseController.GetSelectTime();
            LoadComboBox(ddSummerWeekendPeakStart, peakSummerWeekendStartHourList);
            ddSummerWeekendPeakStart.SelectedValue = rate.SummerPeakWeekendStartHour;

            // peakSummerWeekendEndHourList;
            peakSummerWeekendEndHourList = DatabaseController.GetSelectTime();
            LoadComboBox(ddSummerWeekendPeakEnd, peakSummerWeekendEndHourList);
            ddSummerWeekendPeakEnd.SelectedValue = rate.SummerPeakWeekendEndHour;

            // peakWinterWeekendStartHourList;
            peakWinterWeekendStartHourList = DatabaseController.GetSelectTime();
            LoadComboBox(ddWinterWeekendPeakStart, peakWinterWeekendStartHourList);
            ddWinterWeekendPeakStart.SelectedValue = rate.WinterPeakWeekendStartHour;

            // peakWinterWeekendEndHourList;
            peakWinterWeekendEndHourList = DatabaseController.GetSelectTime();
            LoadComboBox(ddWinterWeekendPeakEnd, peakWinterWeekendEndHourList);
            ddWinterWeekendPeakEnd.SelectedValue = rate.WinterPeakWeekendEndHour;

            // Toggle Seasonal group boxes
            SeasonalToggle();

            // Rate Dropdown
            rateList = DatabaseController.GetRateList();
            LoadRateDropDown();
            ddSelectRate.SelectedValue = rate.ID;

        }

        private void LoadRateDropDown()
        {
            ddSelectRate.DataSource = null;
            ddSelectRate.Items.Clear();
            ddSelectRate.DisplayMember = "Display";
            ddSelectRate.ValueMember = "Id";
            ddSelectRate.DataSource = rateList;
        }

        private void LoadComboBox(ComboBox box, DataTable data)
        {
            box.DataSource = null;
            box.Items.Clear();
            box.DisplayMember = "Display";
            box.ValueMember = "Id";
            box.DataSource = data;
        }

        private void SeasonalToggle()
        {
            if (chkSeasonal.Checked)
            {
                groupBoxSeasonal.Enabled = true;
                groupBoxSeasonalBasic.Enabled = true;
                groupBoxSeasonalTOU.Enabled = true;
                ToggleNonSeasonalTOUControls(false);
            }
            else
            {
                groupBoxSeasonal.Enabled = false;
                groupBoxSeasonalBasic.Enabled = false;
                groupBoxSeasonalTOU.Enabled = false;
                ToggleNonSeasonalTOUControls(true);
            }
        }

        private void ToggleNonSeasonalTOUControls(bool trueOrFalse)
        {
            ddWeekdayPeakStart.Enabled = trueOrFalse;
            ddWeekdayPeakEnd.Enabled = trueOrFalse;
            ddWeekendPeakStart.Enabled = trueOrFalse;
            ddWeekendPeakEnd.Enabled = trueOrFalse;
            txtWeekdayPeakCharge.Enabled = trueOrFalse;
            txtWeekdayOffPeakCharge.Enabled = trueOrFalse;
            txtWeekendPeakCharge.Enabled = trueOrFalse;
            txtWeekendOffPeakCharge.Enabled = trueOrFalse;

            label6.Enabled = trueOrFalse;
            label7.Enabled = trueOrFalse;
            labStart.Enabled = trueOrFalse;
            labEnd.Enabled = trueOrFalse;
            label13.Enabled = trueOrFalse;
            label23.Enabled = trueOrFalse;
        }

        private void SaveRateFormData()
        {
            currentRate.IsCurrentlySelectedRate = chkUseThisAsCurrent.Checked;
            currentRate.IsSeasonal = chkSeasonal.Checked;
            currentRate.HasBasicCharges = chkBasic.Checked;
            currentRate.HasTimeOfUseCharges = chkTOU.Checked;

            //Seasonal start/end only the month; no day or year;
            currentRate.SummerStartMonth = VerifyValidMonth(txtSummerStart.Text, 0);
            currentRate.SummerEndMonth = VerifyValidMonth(txtSummerEnd.Text, 1);

            currentRate.BasicCharge = ConvertToDouble(txtBasicRate.Text);
            currentRate.SummerBasicCharge = ConvertToDouble(txtBasicRateSummer.Text);
            currentRate.WinterBasicCharge = ConvertToDouble(txtBasicRateWinter.Text);

            currentRate.PeakCharge = ConvertToDouble(txtWeekdayPeakCharge.Text);
            currentRate.OffPeakCharge = ConvertToDouble(txtWeekdayOffPeakCharge.Text);
            currentRate.PeakWeekendCharge = ConvertToDouble(txtWeekendPeakCharge.Text);
            currentRate.OffPeakWeekendCharge = ConvertToDouble(txtWeekendOffPeakCharge.Text);

            currentRate.SummerPeakCharge = ConvertToDouble(txtSummerWeekdayPeakCharge.Text);
            currentRate.SummerOffPeakCharge = ConvertToDouble(txtSummerWeekdayOffPeakCharge.Text);
            currentRate.WinterPeakCharge = ConvertToDouble(txtWinterWeekdayPeakCharge.Text);
            currentRate.WinterOffPeakCharge = ConvertToDouble(txtWinterWeekdayOffPeakCharge.Text);

            currentRate.SummerPeakWeekendCharge = ConvertToDouble(txtSummerWeekendPeakCharge.Text);
            currentRate.SummerOffPeakWeekendCharge = ConvertToDouble(txtSummerWeekendOffPeakCharge.Text);
            currentRate.WinterPeakWeekendCharge = ConvertToDouble(txtWinterWeekendPeakCharge.Text);
            currentRate.WinterOffPeakWeekendCharge = ConvertToDouble(txtWinterWeekendOffPeakCharge.Text);

            //dropdown values
            currentRate.PeakStartHour = (int)(((System.Data.DataRowView)(ddWeekdayPeakStart.SelectedItem)).Row.ItemArray[1]);
            currentRate.PeakEndHour = (int)(((System.Data.DataRowView)(ddWeekdayPeakEnd.SelectedItem)).Row.ItemArray[1]);
            currentRate.PeakWeekendStartHour = (int)(((System.Data.DataRowView)(ddWeekendPeakStart.SelectedItem)).Row.ItemArray[1]);
            currentRate.PeakWeekendEndHour = (int)(((System.Data.DataRowView)(ddWeekendPeakEnd.SelectedItem)).Row.ItemArray[1]);
            currentRate.SummerPeakStartHour = (int)(((System.Data.DataRowView)(ddSummerWeekdayPeakStart.SelectedItem)).Row.ItemArray[1]);
            currentRate.SummerPeakEndHour = (int)(((System.Data.DataRowView)(ddSummerWeekdayPeakEnd.SelectedItem)).Row.ItemArray[1]);
            currentRate.WinterPeakStartHour = (int)(((System.Data.DataRowView)(ddWinterWeekdayPeakStart.SelectedItem)).Row.ItemArray[1]);
            currentRate.WinterPeakEndHour = (int)(((System.Data.DataRowView)(ddWinterWeekdayPeakEnd.SelectedItem)).Row.ItemArray[1]);
            currentRate.SummerPeakWeekendStartHour = (int)(((System.Data.DataRowView)(ddSummerWeekendPeakStart.SelectedItem)).Row.ItemArray[1]);
            currentRate.SummerPeakWeekendEndHour = (int)(((System.Data.DataRowView)(ddSummerWeekendPeakEnd.SelectedItem)).Row.ItemArray[1]);
            currentRate.WinterPeakWeekendStartHour = (int)(((System.Data.DataRowView)(ddWinterWeekendPeakStart.SelectedItem)).Row.ItemArray[1]);
            currentRate.WinterPeakWeekendEndHour = (int)(((System.Data.DataRowView)(ddWinterWeekendPeakEnd.SelectedItem)).Row.ItemArray[1]);

            DatabaseController.UpdateRate(currentRate);

            // If this rate that is being saved is the active rate then update the config
            // And run an update that updates all previous rates that may have that indicator
            if (currentRate.IsCurrentlySelectedRate)
            {
                DatabaseController.UpdateRateAsOnlyCurrentRate(currentRate.ID);
                DatabaseController.UpdateSelectedRateConfiguration("default", currentRate.ID);
            }

        }

        // If the input value could not be converted because it contained invalid characters
        // then return 0.0; the value will be saved as a zero;
        private double ConvertToDouble(string inputString)
        {
            double outputDouble = 0.0;

            try
            {
                outputDouble = System.Convert.ToDouble(inputString);
            }
            catch
            {
                outputDouble = 0.0;
            }

            return (outputDouble);
        }

        // Concatenate a year to input and verify the specified date
        // index=0 is start of summer, index=1 is end of summer;
        private string VerifyValidMonth(string month, int index)
        {
            string result = string.Empty;
            bool dateIsValid = false;
            DateTime dt;
            string date;

            date = month + " 1 " + DateTime.Now.Year.ToString();
            dateIsValid = DateTime.TryParse(date, out dt);

            if (dateIsValid)
            {
                result = month;
            }
            else
            {
                if (index == 0)
                {
                    result = "May";
                }
                else if (index == 1)
                {
                    result = "September";
                }
            }

            return (result);
        }


        private void butOK_Click(object sender, EventArgs e)
        {
            SaveRateFormData();
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void chkSeasonal_CheckedChanged(object sender, EventArgs e)
        {
            SeasonalToggle();
        }

        private void butApply_Click(object sender, EventArgs e)
        {
            SaveRateFormData();
            // reload for display in case some illegal values were changed to legal values;
            LoadRateFormData(currentRate);
        }

        private void butAddNew_Click(object sender, EventArgs e)
        {
            Form frmRateConfirmForm = new AddRateConfirmForm(currentRate, this);

            frmRateConfirmForm.ShowDialog(this);
        }

        private void butDelete_Click(object sender, EventArgs e)
        {
            Form frmDeleteRateForm = new DeleteRateConfirmForm(currentRate, this);

            frmDeleteRateForm.ShowDialog(this);
        }

        private void ddSelectRate_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Do nothing here; Use SelectedIndexCommitted event instead
        }

        private void ddSelectRate_SelectedIndexCommitted(object sender, EventArgs e)
        {
            int rateID;

            if (!initialRateDropdownLoad)
            {
                rateID = (int)ddSelectRate.SelectedValue;
                currentRate = DatabaseController.GetRate(rateID);
                LoadRateFormData(currentRate);
            }

        }

    }
}
