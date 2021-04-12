using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Reflection;

using System.Drawing;
using System.Windows.Forms;

namespace BRY
{
	public class FsJ
	{
		static public string WAdd(string s) { return "\"" + s + "\""; }
		static public string WDel(string s) 
		{
			s = s.Trim();
			if(s.Length>=2)
			{
				if ( (s[0]=='\"')&& (s[s.Length-1] == '\"')) 
				{
					s = s.Substring(1, s.Length - 1);
				}
			}
			return s;
		}
		static public string Serialize(object obj)
		{
			var options = new JsonSerializerOptions
			{
				Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
				WriteIndented = true
			};
			return JsonSerializer.Serialize(obj, options);
		}

		static public bool IsNum(object o)
		{
			decimal v = 0;
			return decimal.TryParse(o.ToString(), out v);
		}

	}

}
