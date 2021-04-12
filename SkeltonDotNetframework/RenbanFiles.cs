using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;

namespace BRY
{
	public class RenbanName
	{
		public string Directory { get; set; }
		public string Node { get; set; }
		public string Frame { get; set; }
		public string Ext { get; set; }
		public string FullName
		{
			get { return Path.Combine(Directory, Name); }
		}
		public string Name
		{
			get { return Node + Frame + Ext; }
		}
		public bool IsFrame { get { return Frame != ""; } }
		public int FrameValue
		{
			get { return int.Parse(Frame); }
		}
		public int Frameketa
		{
			get { return Frame.Length; }
		}
		public RenbanName()
		{
			Clear();
		}
		public RenbanName(string p)
		{
			Clear();
			string[] sa = SplitFileName(p);
			Directory = sa[0];
			Node = sa[1];
			Frame = sa[2];
			Ext = sa[3];
		}
		public void Clear()
		{
			Directory = "";
			Node = "";
			Frame = "";
			Ext = "";
		}
		public new string ToString()
		{
			return FullName;
		}
		public string ToJson()
		{
			string ret = string.Format("\"Directory\":\"{0}\",\"Node\":\"{1}\",\"Frame\":\"{2}\",\"Ext\":\"{3}\"", Directory, Node, Frame, Ext);
			return "{" + ret + "}";
		}

		static public int IndexOfFrame(string s)
		{
			int ret = -1;
			int cnt = s.Length;
			if (cnt <= 0) return ret;
			for ( int i= cnt-1;i>=0;i--)
			{
				if((s[i]<'0')|| (s[i] > '9'))
				{
					ret = i;
					break;
				}
			}
			if (ret == cnt - 1)
			{
				ret = -1;
			}
			else
			{
				ret += 1;
			}
			return ret;
		}
		static public string[] SplitFrame(string s)
		{
			string [] ret = new string[2];
			ret[0] = "";
			ret[1] = "";
			if (s.Length <= 0) return ret;

			int idx = IndexOfFrame(s);
			if (idx < 0)
			{
				ret[0] = s;
			}else if (idx==0)
			{
				ret[1] = s;
			}
			else
			{
				//AAAA0001
				//01234567
				ret[0] = s.Substring(0, idx);
				ret[1] = s.Substring(idx);
			}
			return ret;
		}
		static public string[] SplitFileName(string s)
		{
			string[] ret = new string[4];
			ret[0] = "";
			ret[1] = "";
			ret[2] = "";
			ret[3] = "";
			if (s == "") return ret;
			ret[0] = Path.GetDirectoryName(s);
			string[] sa = SplitFrame(Path.GetFileNameWithoutExtension(s));
			ret[1] = sa[0];
			ret[2] = sa[1];
			ret[3] = Path.GetExtension(s);
			return ret;
		}
	}

	public class RenbanFiles
	{
		private string m_Directory = "";
		private string m_Node = "";
		private string m_Ext = "";
		private int m_StartFrame = 0;
		private int m_LastFrame = 0;
		private int m_FrameKeta = 4;
		private List<string>m_Frames = new List<string>();
		// *************************************************
		public int Count
		{
			get { return m_Frames.Count; }
		}
		public int FrameKeta
		{
			get { return m_FrameKeta; }
		}
		public string Directory { get { return m_Directory; } }
		public string Node { get { return m_Node; } }
		public string Ext { get { return m_Ext; } }
		// *************************************************
		public new string ToString()
		{
			string ret = "";
			if(m_Frames.Count<=0)
			{
				ret = m_Node +"[]"+ m_Ext;
			}else if (m_Frames.Count == 1)
			{
				ret = m_Node +  m_Frames[0] + m_Ext;
			}
			else
			{
				ret = m_Node + "[" + m_Frames[0] + "-" + m_Frames[m_Frames.Count-1] + "]" + m_Ext;
			}
			return ret;
		}
		// *************************************************
		public RenbanFiles()
		{
			Clear();
		}
		// *************************************************
		public void Clear()
		{
			m_Directory = "";
			m_Node = "";
			m_Ext = "";
			m_StartFrame = 1;
			m_LastFrame = 1;
			m_FrameKeta = 4;
			m_Frames.Clear();

		}
		// *************************************************
		public bool AddFileName(string p)
		{
			return AddFileName(new RenbanName(p));
		}
		// *************************************************
		public bool AddFileName(RenbanName rn)
		{
			if (rn.IsFrame == false) return false;

			if (m_Frames.Count <= 0)
			{
				m_Directory = rn.Directory;
				m_Node = rn.Node;
				m_Ext = rn.Ext;
			}
			else
			{
				if ((this.m_Directory != "") && (rn.Directory != ""))
				{
					if (m_Directory != rn.Directory) return false;
				}

				if (m_Node != rn.Node) return false;
				if (m_Ext != rn.Ext) return false;
			}
			int v = rn.FrameValue;
			if (m_StartFrame > v) m_StartFrame = v;
			if (m_LastFrame < v) m_LastFrame = v;
			int k = rn.Frameketa;
			if (m_FrameKeta > k) m_FrameKeta = k;
			m_Frames.Add(rn.Frame);

			return true;
		}
		// *************************************************
		static public RenbanFiles[] GetFiles(string FolderPass)
		{
			List<RenbanFiles> ret = new List<RenbanFiles>();

			var di = new DirectoryInfo(FolderPass);
			var files = di.EnumerateFiles("*", SearchOption.TopDirectoryOnly);
			if (files.Count<FileInfo>() <= 0) return new RenbanFiles[0];

			foreach (FileInfo fi in files)
			{
				RenbanName rn = new RenbanName(fi.FullName);
				if (rn.IsFrame == false) continue;
				if (ret.Count<=0)
				{
					RenbanFiles rf = new RenbanFiles();
					ret.Add(rf);
				}
				if (ret[ret.Count-1].AddFileName(rn)==false)
				{
					RenbanFiles rf = new RenbanFiles();
					rf.AddFileName(rn);
					ret.Add(rf);

				}
			}
			return ret.ToArray();
		}
		static public RenbanFiles GetFiles(RenbanName rn)
		{
			RenbanFiles ret = new RenbanFiles();
			if (rn.IsFrame == false) return ret;
			if (rn.Directory == "") rn.Directory = ".\\";
			DirectoryInfo d = new DirectoryInfo(rn.Directory);
			if (d.Exists == false) return ret;
			rn.Directory = d.FullName;

			var di = new DirectoryInfo(rn.Directory);
			var files = di.EnumerateFiles(rn.Node + "*" + rn.Ext, SearchOption.TopDirectoryOnly);
			if (files.Count<FileInfo>() <= 0) return ret;

			foreach (FileInfo fi in files)
			{
				RenbanName rn2 = new RenbanName(fi.FullName);
				ret.AddFileName(rn2);
			}
			return ret;
		}

	}
}
