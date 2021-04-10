using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;

using System.IO;


namespace BRY
{
	public partial class AppInfoDialog : Form
	{
		public AppInfoDialog()
		{
			InitializeComponent();

			this.StartPosition = FormStartPosition.CenterParent;

			Campany = "bry-ful";
			AppName = Path.GetFileNameWithoutExtension(Application.ExecutablePath);
			AppVersion = "1.00";
			Copyright = "Copyright (c) bry-ful 2021";
			Description = "";
		}
		static public void ShowAppInfoDialog()
		{
			using (AppInfoDialog dlg = new AppInfoDialog())
			{
				dlg.ShowDialog();
			}
		}
		public string AppName
		{
			get { return lbProduct.Text; }
			set
			{
				lbProduct.Text = value;
				this.Text = "Information - " +lbProduct.Text ;
			}

		}
		public string Campany
		{
			get { return lbCampany.Text; }
			set
			{
				lbCampany.Text = value;
			}

		}
		public string AppVersion
		{
			get { return lbVersion.Text; }
			set
			{
				lbVersion.Text = value;
			}

		}
		public string Copyright
		{
			get { return lbCopyright.Text; }
			set
			{
				lbCopyright.Text = value;
			}

		}
		public string Description
		{
			get { return lbDescription.Text; }
			set
			{
				lbDescription.Text = value;
			}

		}
		private void AppInfoDialog_Load(object sender, EventArgs e)
		{

		}
	}

	
}
