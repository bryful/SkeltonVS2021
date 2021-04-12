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

using System.Text.Json;
using BRY;

/// <summary>
/// 基本となるアプリのスケルトン
/// </summary>
namespace SkeltonDotNetframework
{
	public partial class Form1 : Form
	{
		//-------------------------------------------------------------
		/// <summary>
		/// コンストラクタ
		/// </summary>
		public Form1()
		{
			InitializeComponent();
		}
		/// <summary>
		/// コントロールの初期化はこっちでやる
		/// </summary>
		protected override void InitLayout()
		{
			base.InitLayout();
		}
		//-------------------------------------------------------------
		/// <summary>
		/// フォーム作成時に呼ばれる
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Form1_Load(object sender, EventArgs e)
		{
			//設定ファイルの読み込み
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
		//-------------------------------------------------------------
		/// <summary>
		/// フォームが閉じられた時
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Form1_FormClosed(object sender, FormClosedEventArgs e)
		{
			//設定ファイルの保存
			PrefFile pref = new PrefFile();
			pref.SetSize("Size", this.Size);
			pref.SetPoint("Point", this.Location);

			pref.Save();

		}
		//-------------------------------------------------------------
		/// <summary>
		/// ドラッグ＆ドロップの準備
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Form1_DragEnter(object sender, DragEventArgs e)
		{
			if (e.Data.GetDataPresent(DataFormats.FileDrop))
			{
				e.Effect = DragDropEffects.All;
			}
			else
			{
				e.Effect = DragDropEffects.None;
			}
		}
		/// <summary>
		/// ドラッグ＆ドロップの本体
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Form1_DragDrop(object sender, DragEventArgs e)
		{
			string[] files = (string[])e.Data.GetData(DataFormats.FileDrop, false);
			//ここでは単純にファイルをリストアップするだけ
			GetCommand(files);
		}
		//-------------------------------------------------------------
		/// <summary>
		/// ダミー関数
		/// </summary>
		/// <param name="cmd"></param>
		public void GetCommand(string[] cmd)
		{
			if (cmd.Length>0)
			{
				foreach (string s in cmd)
				{
					listBox1.Items.Add(s);
				}
			}
		}
		/// <summary>
		/// メニューの終了
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		//-------------------------------------------------------------
		private void quitToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Application.Exit();
		}

		private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
		{
			AppInfoDialog.ShowAppInfoDialog();
		}

		private void button1_Click_1(object sender, EventArgs e)
		{
			RenbanName rn = new RenbanName("AAA0001.tga");

			MessageBox.Show(rn.ToJson());


		}


		/*
private void button1_Click(object sender, EventArgs e)
{
	dynamic a = new DynamicJson();
	a.fff = new string[] { "a", "B" };
	a.fff = "12";
	//a.fff = new { aaa=12, ccc="www" };

	MessageBox.Show(a.fff.GetType().ToString());

	JsonPref s = new JsonPref();
	s.AddInt("aaa", 99);
	string ss = s.ToJson();
	MessageBox.Show(ss);
	s.Parse(ss);
	string sss = s.ToJson();
	MessageBox.Show(sss);

	int i = s.GetInt("aaa");
	MessageBox.Show(String.Format("{0}", i));
}
*/
	}
}
