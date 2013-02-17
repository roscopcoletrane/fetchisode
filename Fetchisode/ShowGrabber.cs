using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;

using System.Net;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using Microsoft.Win32;

namespace Fetchisode
{
	public class ShowGrabber
	{
		public List<string> letterList = new List<string>() { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };
		public List<List<Show>> fullList;
		

		public ShowGrabber()
		{
			if (File.Exists("showdata.xml"))
			{
				LoadFromXML();
			}
			else
			{
				DownloadShowData();
				OutputToXML();
			}
		}

		public void DownloadShowData()
		{
			WebClient web = new WebClient();

			fullList = new List<List<Show>>();

			string html;
			List<string> htmlList;
			List<Show> showList;
			foreach (string letter in letterList)
			{
				html = web.DownloadString(@"http://www.epguides.com/menu" + letter + "/");
				htmlList = html.Split('\n').ToList();

				showList = new List<Show>();

				Match regexMatch;
				foreach (string line in htmlList)
				{
					//<li><b><a href="../AforAndromeda/">A for Andromeda</a></b></li>
					regexMatch = Regex.Match(line, @"<li><b><a href=""../(?<url>.+)/"">(?<name>.+)</a></b>.*</li>");

					if (regexMatch.Success)
					{
						showList.Add(new Show(regexMatch.Groups["name"].Value, regexMatch.Groups["url"].Value));
					}
				}

				fullList.Add(showList);
			}
		}

		public void OutputToXML()
		{
			
			XmlWriterSettings settings = new XmlWriterSettings();
			settings.Indent = true;
			XmlWriter writer = XmlWriter.Create("showdata.xml", settings);
			
			writer.WriteStartElement("ShowList");

			int letterIndex = 0;
			foreach (List<Show> showList in fullList)
			{
				writer.WriteStartElement("List");
				writer.WriteAttributeString("letter", letterList[letterIndex]);
				letterIndex++;

				foreach (Show show in showList)
				{
					writer.WriteStartElement("Show");
					writer.WriteElementString("Name", show.showName);
					writer.WriteElementString("URL", show.url);
					writer.WriteEndElement(); //Show
				}

				writer.WriteEndElement(); //List
			}

			writer.WriteEndElement(); //ShowList
			writer.Close();
		}

		public void LoadFromXML()
		{
			XmlDocument AppXMLDocument = new XmlDocument();
			AppXMLDocument.Load("showdata.xml");

			fullList = new List<List<Show>>();

			List<Show> showList;
			foreach (XmlNode letterNode in AppXMLDocument.SelectSingleNode("ShowList").ChildNodes)
			{
				showList = new List<Show>();
				foreach (XmlNode showNode in letterNode.ChildNodes)
				{
					showList.Add(new Show(showNode));
				}
				fullList.Add(showList);
			}
		}

		public List<Show> GetShowList(string letter)
		{
			int index = letterList.FindIndex(
				delegate(string s)
				{
					return s.Equals(letter);
				}
			);
			return fullList[index];
		}
	}
}
