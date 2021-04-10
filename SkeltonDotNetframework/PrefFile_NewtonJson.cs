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

// Newton.Jsonをインストールすること
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BRY
{
	/// <summary>
	/// 環境設定ファイルをjsonで保存するclass
	/// アセンブリ情報をしっかり設定しないと変になる
	/// </summary>
	public class PrefFile
	{
		private JObject _pref = new JObject();
		private string _filePath = "";
		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="appName">アプリケーション名を指定。省略すると実行ファイル名になる。</param>
		public PrefFile(string appName="")
		{
			if (appName == "") appName = Path.GetFileNameWithoutExtension(Application.ExecutablePath);
			_filePath = Path.Combine(Application.UserAppDataPath, appName + ".json");

		}
		public void SetInt(string key,int value)
		{
			_pref.Add(key, value);
		}
		public int GetInt(string key ,ref bool ok)
		{
			int ret = 0;
			ok = false;
			if (_pref.ContainsKey(key)==true)
			{
				if (_pref[key].Type == JTokenType.Integer)
				{
					ret = (int)_pref[key];
					ok = true;
				}
			}
			return ret;
		}
		public void SetDouble(string key, double value)
		{
			_pref.Add(key, value);
		}
		public double GetDouble(string key, ref bool ok)
		{
			double ret = 0;
			ok = false;
			if (_pref.ContainsKey(key) == true)
			{
				if (_pref[key].Type == JTokenType.Float)
				{
					ret = (double)_pref[key];
					ok = true;
				}
			}
			return ret;
		}
		public void SetString(string key, string value)
		{
			_pref.Add(key, value);
		}
		public string GetString(string key, ref bool ok)
		{
			string ret = "";
			if (_pref.ContainsKey(key) == true)
			{
				if (_pref[key].Type == JTokenType.String)
				{
					ret = (string)_pref[key];
					ok = true;
				}
			}
			return ret;
		}
		public void SetValue(string key, JToken value)
		{
			_pref.Add(key, (JToken)value);
		}
		public JToken GetValue(string key, out bool ok)
		{
			JToken ret = null;
			ok = false;
			if (_pref.ContainsKey(key) == true)
			{
				ret = _pref[key];
				ok = true;
			}


			return ret;
		}
		public void SetStringArray(string key, string[] values)
		{
			JArray ja = new JArray();
			if (values.Length > 0)
			{
				foreach (string i in values)
				{
					ja.Add(i);
				}
			}
			_pref.Add(key, ja);
		}
		public string[] GetStringArray(string key, ref bool ok)
		{
			string[] ret = new string[0];
			ok = false;
			if (_pref.ContainsKey(key) == true)
			{
				JToken o = _pref[key];
				if (o is JArray)
				{
					JArray ary = (JArray)o;
					if (ary.Count > 0)
					{
						Array.Resize(ref ret, ary.Count);
						for (int i = 0; i < ary.Count; i++)
						{
							if (ary[i].Type == JTokenType.String)
								ret[i] = (string)ary[i];
						}
						ok = true;
					}
				}
			}
			return ret;
		}
		public void SetIntArray(string key, int[] values)
		{
			JArray ja = new JArray();
			if (values.Length > 0)
			{
				foreach (int i in values)
				{
					ja.Add(i);
				}
			}
			_pref.Add(key, ja);
		}
		public int[] GetIntArray(string key, ref bool ok)
		{
			int[] ret = new int[0];
			ok = false;
			if (_pref.ContainsKey(key) == true)
			{
				JToken o = _pref[key];
				if (o is JArray)
				{
					JArray ary = (JArray)o;
					if (ary.Count > 0)
					{
						Array.Resize(ref ret, ary.Count);
						for (int i = 0; i < ary.Count; i++)
						{
							if (ary[i].Type == JTokenType.Integer)
								ret[i] = (int)ary[i];
						}
						ok = true;
					}
				}
			}
			return ret;
		}
		public void SetDoubleArray(string key, double[] values)
		{
			JArray ja = new JArray();
			if (values.Length > 0)
			{
				foreach (int i in values)
				{
					ja.Add(i);
				}
			}
			_pref.Add(key, ja);
		}
		public double[] GetDoubleArray(string key, ref bool ok)
		{
			double[] ret = new double[0];
			ok = false;
			if (_pref.ContainsKey(key) == true)
			{
				JToken o = _pref[key];
				if (o is JArray)
				{
					JArray ary = (JArray)o;
					if (ary.Count > 0)
					{
						Array.Resize(ref ret, ary.Count);
						for (int i = 0; i < ary.Count; i++)
						{
							if (ary[i].Type == JTokenType.Float)
								ret[i] = (double)ary[i];
						}
						ok = true;
					}
				}
			}
			return ret;
		}
		public void SetArray(string key, JToken[]values)
		{
			JArray ja = new JArray();
			if (values.Length>0)
			{
				foreach(JToken i in values)
				{
					ja.Add(i);
				}
			}
			_pref.Add(key, ja);
		}
		public JToken[] GetArray(string key, out bool ok)
		{
			JToken[] ret = new JToken[0];
			ok = false;
			if (_pref.ContainsKey(key) == true)
			{
				JToken o = _pref[key];
				if (o is JArray)
				{
					JArray ary = (JArray)o;
					if (ary.Count>0)
					{
						Array.Resize(ref ret, ary.Count);
						for (int i=0; i < ary.Count; i++)
						{
							ret[i] = ary[i];
						}
						ok = true;
					}
				}
			}
			return ret;
		}
		public void SetSize(string key, Size sz)
		{
			JObject jo = new JObject();
			jo.Add("Width", sz.Width);
			jo.Add("Height", sz.Height);
			_pref.Add(key, jo);
		}
		public Size GetSize(string key, ref bool ok)
		{
			Size ret = new Size(0, 0);
			ok = false;
			if (_pref.ContainsKey(key) == true)
			{
				if (_pref[key].Type == JTokenType.Object)
				{
					ok = true;
					JObject jo = (JObject)_pref[key];
					if (jo.ContainsKey("Width") == true)
					{
						ret.Width = (int)jo["Width"];
					}
					else
					{
						ok = false;
					}
					if (jo.ContainsKey("Height") == true)
					{
						ret.Height = (int)jo["Height"];
					}
					else
					{
						ok = false;
					}

				}
			}
			return ret;
		}
		public void SetPoint(string key, Point sz)
		{
			JObject jo = new JObject();
			jo.Add("X", sz.X);
			jo.Add("Y", sz.Y);
			_pref.Add(key, jo);
		}
		public Point GetPoint(string key, ref bool ok)
		{
			Point ret = new Point(0, 0);
			ok = false;
			if (_pref.ContainsKey(key) == true)
			{
				if (_pref[key].Type == JTokenType.Object)
				{
					ok = true;
					JObject jo = (JObject)_pref[key];
					if (jo.ContainsKey("X") == true)
					{
						ret.X = (int)jo["X"];
					}
					else
					{
						ok = false;
					}
					if (jo.ContainsKey("Y") == true)
					{
						ret.Y = (int)jo["Y"];
					}
					else
					{
						ok = false;
					}

				}
			}
			return ret;
		}
		/// <summary>
		/// 保存する
		/// </summary>
		/// <param name="p">保存パス</param>
		/// <returns></returns>
		public bool Save(string p)
		{
			bool ret = false;

			try
			{
				string js = _pref.ToString();
				File.WriteAllText(p, js,Encoding.GetEncoding("utf-8"));
				ret = true;
			}
			catch
			{
				ret = false;
			}
			return ret;
		}
		/// <summary>
		/// デフォルトのパスに保存
		/// </summary>
		/// <returns></returns>
		public bool Save()
		{
			return Save(_filePath);
		}
		/// <summary>
		/// 指定したパスを読み込む
		/// </summary>
		/// <param name="p">読み込むパス</param>
		/// <returns></returns>
		public bool Load(string p)
		{
			bool ret = false;

			try
			{
				if (File.Exists(p) == true)
				{
					string str = File.ReadAllText(p, Encoding.GetEncoding("utf-8"));
					if (str != "")
					{
						_pref = JObject.Parse(str);
						ret = true;
					}
				}
			}
			catch
			{
				_pref = new JObject();
				ret = false;
			}
			return ret;
		}
		/// <summary>
		/// デフォルトのパスを読み込む
		/// </summary>
		/// <returns></returns>
		public bool Load()
		{
			return Load(_filePath);
		}

	}
}
