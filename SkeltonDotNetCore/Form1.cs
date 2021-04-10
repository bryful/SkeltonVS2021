using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.IO;
using BRY;

namespace SkeltonDotNetCore
{
	public partial class Form1 : Form
	{
		public Form1()
		{
			InitializeComponent();

			QuitMenuItem.Click += QuitMenuItem_Click1;
			AboutMenuItem.Click += AboutMenuItem_Click;
		}


		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);
			PrefFile pref = new PrefFile();
			if (pref.Load())
			{
				Size sz;
				if (pref.GetSize("Size", out sz)) this.Size = sz;
				Point p;
				if (pref.GetPoint("Point", out p)) this.Location = p;
			}
			this.Text = Path.GetFileNameWithoutExtension(Application.ExecutablePath);
		}
		protected override void OnFormClosed(FormClosedEventArgs e)
		{
			base.OnFormClosed(e);
			PrefFile pref = new PrefFile();
			pref.SetSize("Size", this.Size);
			pref.SetPoint("Point", this.Location);

			pref.Save();
		}
		private void AboutMenuItem_Click(object sender, EventArgs e)
		{
			AppInfoDialog.ShowAppInfoDialog();
		}

		private void QuitMenuItem_Click1(object sender, EventArgs e)
		{
			Application.Exit();
		}
	}
}
