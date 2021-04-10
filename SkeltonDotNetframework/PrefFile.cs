using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Windows.Forms;
using System.Linq;

namespace BRY
{
	public class PrefFile
	{
		private Dictionary<string, object> m_pref = new Dictionary<string, object>();
		private string m_AppName = "";
		private string m_PrefFolder = "";
		private string m_PrefFileName = "";
		public string AppName { get { return m_AppName; } }
		public string PrefFolder { get { return m_PrefFolder; } }
		public string PrefFileName { get { return m_PrefFileName; } }

		// ***************************************************************
		public PrefFile()
		{
			m_pref.Clear();

			m_AppName = GetAppName();
			m_PrefFolder = GetPrefFolder(m_AppName);
			m_PrefFileName = Path.Combine(m_PrefFolder, m_AppName + ".json");
		}
		// ***************************************************************
		static public string GetAppName()
		{
			return Path.GetFileNameWithoutExtension(Application.ExecutablePath);
		}
		// ***************************************************************
		static public string GetPrefFolder(string a = "")
		{
			string p = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
			if (a == "")
			{
				a = Path.GetFileNameWithoutExtension(Application.ExecutablePath);
			}

			string p2 = Path.Combine(p, a);
			if (Directory.Exists(p2) == false)
			{
				Directory.CreateDirectory(p2);
			}
			return p2;

		}
		// ***************************************************************
		public bool Save(string p = "")
		{
			bool ret = false;
			if (p == "") p = m_PrefFileName;
			try
			{
				if (File.Exists(p)) File.Delete(p);
				File.WriteAllText(p, ToJson(), Encoding.GetEncoding("utf-8"));
				ret = File.Exists(p);
			}
			catch
			{
				ret = false;
			}
			return ret;
		}
		// ************************************************************************
		public bool Load(string p = "")
		{
			bool ret = false;
			if (p == "") p = m_PrefFileName;

			try
			{
				if (File.Exists(p) == true)
				{
					string str = File.ReadAllText(p, Encoding.GetEncoding("utf-8"));
					if (str != "")
					{
						ret = FromJson(str);
					}
				}
			}
			catch
			{
				ret = false;
			}
			return ret;
		}
		// ************************************************************************
		public string ToJson()
		{
			var options = new JsonSerializerOptions
			{
				Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
				WriteIndented = true
			};
			return JsonSerializer.Serialize(m_pref, options);
		}
		// ************************************************************************
		public bool FromJson(string js)
		{
			bool ret = false;
			var options = new JsonSerializerOptions
			{
				Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
			};
			try
			{
				m_pref.Clear();
				m_pref = JsonSerializer.Deserialize<Dictionary<string, object>>(js, options);
				ret = (m_pref.Count > 0);
			}
			catch
			{
				ret = false;
			}


			return ret;
		}
     // ************************************************************************
		private void SetObj(string key, object obj)
		{
			if (m_pref.ContainsKey(key))
			{
				m_pref[key] = obj;
			}
			else
			{
				m_pref.Add(key, obj);
			}
		}
		// ************************************************************************
		private object GetObj(string key)
		{
			object ret = null;

			if(m_pref.ContainsKey(key))
			{
				ret = m_pref[key];
			}
			return ret;
		}
		// ************************************************************************
		private bool IsNum(object o)
		{
			return ((o is int)|| (o is float)||(o is double)||(o is uint) || (o is char) || (o is byte));
		}
		// ************************************************************************
		public void SetNum(string key, double v)
		{
			SetObj(key, (object)v);
		}
		public bool GetNum(string key, out double v)
		{
			bool ret = false;
			v = 0;

			object obj = GetObj(key);
			if(obj!=null)
			{
				try
				{
					if (IsNum(obj) == true)
					{
						v = (double)obj;
						ret = true;
					}
					else
					{
						JsonElement je = (JsonElement)obj;
						if (je.ValueKind == JsonValueKind.Number)
						{
							ret = (je.TryGetDouble(out v));
						}
					}
				}
				catch
				{
					ret = false;
				}
			}

			return ret;
		}
		// ************************************************************************
		public void SetStr(string key, string v)
		{
			SetObj(key, (object)v);

		}
		public bool GetStr(string key, out string v)
		{
			bool ret = false;
			v = "";
			object obj = GetObj(key);
			if (obj != null)
			{
				try
				{
					if (obj is string)
					{
						v = (string)obj;
						ret = true;
					}
					else
					{
						JsonElement je = (JsonElement)obj;
						if (je.ValueKind == JsonValueKind.String)
						{
							v = je.GetString();
							ret = true;
						}
					}
				}
				catch
				{
					ret = false;
				}
			}

			return ret;
		}
		// ************************************************************************
		public void SetBool(string key, bool v)
		{
			SetObj(key, (object)v);
		}
		public bool GetBool(string key, out bool v)
		{
			bool ret = false;
			v = true;
			object obj = GetObj(key);
			if (obj != null)
			{
				try
				{

					if (obj is bool)
					{
						v = (bool)obj;
						ret = true;
					}
					else
					{
						JsonElement je = (JsonElement)obj;
						if (je.ValueKind == JsonValueKind.True)
						{
							v = true;
							ret = true;
						}
						else if (je.ValueKind == JsonValueKind.False)
						{
							v = false;
							ret = true;
						}
					}
				}
				catch
				{
					ret = false;
				}
			}

			return ret;
		}
		// ************************************************************************
		public void SetBoolArray(string key, bool[] v)
		{
			SetObj(key, (object)v);
		}
		public bool GetBoolArray(string key, out bool[] v)
		{
			bool ret = false;
			v = new bool[0];
			object obj = GetObj(key);
			if (obj != null)
			{
				try
				{
					if (obj is bool[])
					{
						v = (bool[])obj;
						ret = true;
					}
					else
					{
						JsonElement je = (JsonElement)obj;
						if (je.ValueKind == JsonValueKind.Array)
						{
							List<bool> da = new List<bool>();
							foreach (var vv in je.EnumerateArray())
							{
								if (vv.ValueKind == JsonValueKind.True)
								{
									da.Add(true);
								}else if (vv.ValueKind == JsonValueKind.False)
								{
									da.Add(false);
								}
								else
								{
									da.Add(false);
								}
							}
							v = da.ToArray();
							ret = true;
						}
					}
				}
				catch
				{
					ret = false;
				}
			}

			return ret;
		}
		// ************************************************************************
		public void SetNumArray(string key, double [] v)
		{
			SetObj(key, (object)v);
		}
		public bool GetNumArray(string key, out double[] v)
		{
			bool ret = false;
			v = new double[0];
			object obj = GetObj(key);
			if (obj != null)
			{
				try
				{
					if (obj is double[])
					{
						v = (double[])obj;
						ret = true;
					}
					else
					{
						JsonElement je = (JsonElement)obj;
						if (je.ValueKind == JsonValueKind.Array)
						{
							List<double> da = new List<double>();
							foreach (var vv in je.EnumerateArray())
							{
								if (vv.ValueKind == JsonValueKind.Number)
								{
									da.Add(vv.GetDouble());
								}
							}
							v = da.ToArray();
							ret = true;
						}
					}
				}
				catch
				{
					ret = false;
				}
			}

			return ret;
		}       
		// ************************************************************************
		public void SetStrArray(string key, string[] v)
		{
			SetObj(key, (object)v);
		}
		public bool GetStrArray(string key, out string[] v)
		{
			bool ret = false;
			v = new string[0];
			object obj = GetObj(key);
			if (obj != null)
			{
				try
				{

					if (obj is string[])
					{
						v = (string[])obj;
						ret = true;
					}
					else
					{
						JsonElement je = (JsonElement)obj;
						if (je.ValueKind == JsonValueKind.Array)
						{
							List<string> da = new List<string>();
							foreach (var vv in je.EnumerateArray())
							{
								if (vv.ValueKind == JsonValueKind.String)
								{
									da.Add(vv.GetString());
								}
							}
							v = da.ToArray();
							ret = true;
						}
					}
				}
				catch
				{
					ret = false;
				}
			}

			return ret;
		}
		// ************************************************************************
		public void SetPoint(string key, Point v)
		{
			SetObj(key, (object)v);
		}
		public bool GetPoint(string key, out Point v)
		{
			bool ret = false;
			v = new Point(0,0);
			object obj = GetObj(key);
			if (obj != null)
			{
				try
				{
					if (obj is Point)
					{
						v = (Point)obj;
						ret = true;
					}
					else
					{
						JsonElement je = (JsonElement)obj;
						if (je.ValueKind == JsonValueKind.Object)
						{
							JsonElement je2;
							bool bx = false;
							bool by = false;
							if (je.TryGetProperty("X", out je2))
							{
								v.X = (int)je2.GetDouble();
								bx = true;
							}
							if (je.TryGetProperty("Y", out je2))
							{
								v.Y = (int)je2.GetDouble();
								by = true;
							}
							ret = ((bx) && (by));
						}
					}
				}
				catch
				{
					ret = false;
				}
			}

			return ret;
		}
		// ************************************************************************
		public void SetSize(string key, Size v)
		{
			SetObj(key, (object)v);
		}
		public bool GetSize(string key, out Size v)
		{
			bool ret = false;
			v = new Size(0, 0);
			object obj = GetObj(key);
			if (obj != null)
			{
				try
				{
					if (obj is Size)
					{
						v = (Size)obj;
						ret = true;
					}
					else
					{
						JsonElement je = (JsonElement)obj;
						if (je.ValueKind == JsonValueKind.Object)
						{
							JsonElement je2;
							bool bw = false;
							bool bh = false;
							if (je.TryGetProperty("Width", out je2))
							{
								v.Width = (int)je2.GetDouble();
								bw = true;
							}
							if (je.TryGetProperty("Height", out je2))
							{
								v.Height = (int)je2.GetDouble();
								bh = true;
							}
							ret = ((bw) && (bh));
						}
					}
				}
				catch
				{
					ret = false;
				}
			}
			return ret;
		}
		// ************************************************************************
		public void SetColor(string key, Color v)
		{
			SetObj(key, (object)v);
		}
		private bool JeToColor(JsonElement je,out Color col)
		{
			bool ret = false;
			col = Color.Black;
			if (je.ValueKind == JsonValueKind.Object)
			{
				JsonElement je2;
				byte r = 255;
				byte g = 255;
				byte b = 255;
				byte a = 255;
				bool br = false;
				bool bg = false;
				bool bb = false;
				bool ba = false;
				if (je.TryGetProperty("R", out je2))
				{
					r = (byte)je2.GetByte();
					br = true;
				}
				if (je.TryGetProperty("G", out je2))
				{
					g = (byte)je2.GetByte();
					bg = true;
				}
				if (je.TryGetProperty("B", out je2))
				{
					b = (byte)je2.GetByte();
					bb = true;
				}
				if (je.TryGetProperty("A", out je2))
				{
					a = (byte)je2.GetByte();
					ba = true;
				}
				if ((br) && (bg) && (bb))
				{
					if (ba == false)
					{
						col = Color.FromArgb(r, g, b);
					}
					else
					{
						col = Color.FromArgb(a, r, g, b);
					}
					ret = true;
				}
			}
			return ret;
		}
		public bool GetColor(string key, out Color v)
		{
			bool ret = false;
			v = Color.Black;
			object obj = GetObj(key);
			if (obj != null)
			{
				try
				{
					if (obj is Color)
					{
						v = (Color)obj;
						ret = true;
					}
					else
					{
						Color c = Color.Black;
						if( JeToColor((JsonElement)obj, out c))
						{
							v = c; 
						}
					}
				}
				catch
				{
					ret = false;
				}
			}
			return ret;
		}
		// ************************************************************************
		public string WAdd(string s)
		{
			return "\"" + s + "\"";
		}

		// ************************************************************************
		

	}
}
