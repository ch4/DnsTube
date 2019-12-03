using System;
using System.Linq;
using System.Windows.Forms;

namespace DnsTube
{
	public partial class frmSettings : Form
	{
		Settings settings;

		public frmSettings(Settings settings2)
		{
			settings = settings2;
			InitializeComponent();
            textBox1.Text = "";

            Utility.GetLocalIPAddresses().ToList().ForEach(x => textBox1.Text += (x+"\r\n"));
        }

		void btnCancel_Click(object sender, EventArgs e)
		{
			Close();
		}

		void btnSave_Click(object sender, EventArgs e)
		{
			settings.EmailAddress = txtEmail.Text;
			settings.ApiKey = txtApiKey.Text;
			settings.UpdateIntervalMinutes = int.Parse(txtUpdateInterval.Text);
			settings.StartMinimized = chkStartMinimized.Checked;
			settings.Save();
			Close();
		}

		void frmSettings_Load(object sender, EventArgs e)
		{
			txtEmail.Text = settings.EmailAddress;
			txtApiKey.Text = settings.ApiKey;
			txtUpdateInterval.Text = settings.UpdateIntervalMinutes.ToString();
			chkStartMinimized.Checked = settings.StartMinimized;
		}

		void txtUpdateInterval_KeyPress(object sender, KeyPressEventArgs e)
		{
			if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
			{
				e.Handled = true;
			}
		}
    }
}
