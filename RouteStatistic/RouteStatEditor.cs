using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RouteStatistic
{
    public partial class RouteStatEditor : Form
    {
        RouteStatistic _dr;
        public RouteStatEditor(RouteStatistic dr, bool isReadOnly) :base()
        {
            InitializeComponent();
            _dr = dr;

            if (isReadOnly)
            {
                cbSummaryType.Enabled = false;
                btnOK.Visible = false;
                btnOK.Enabled = false;
                btnCancel.Left = (btnOK.Left + btnCancel.Left) / 2;
                btnCancel.Text = "Close";
                this.AcceptButton = btnCancel;
            }

            cbSummaryType.Items.Add("Distance");
            cbSummaryType.Items.Add("MaxSpeed");
            cbSummaryType.Items.Add("MinSpeed");

            if (dr.SummaryType != null && cbSummaryType.Items.Contains(dr.SummaryType))
            {
                cbSummaryType.Text = dr.SummaryType;
            }


        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            _dr.SummaryType = cbSummaryType.Text;
            this.Close();
            this.Dispose();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
            this.Dispose();
        }


    }
}
