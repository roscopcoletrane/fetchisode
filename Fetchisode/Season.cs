using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Fetchisode
{
	/// <summary>
	/// Container to hold a list of episode names for a specific season of a show.
	/// </summary>
	public class Season
	{
		public List<string> epNameList;
		public string number;

		/// <summary>
		/// Constructor
		/// 
		/// Used when loading from website.
		/// 
		/// Creates a list to contain episode information for a specific season.
		/// </summary>
		/// <param name="num">Season number</param>
		public Season(string num)
		{
			number = num;
			epNameList = new List<string>();
		}

		/// <summary>
		/// Constructor
		/// 
		/// Used when loading from xml file.
		/// 
		/// Parses xml node to get season number and episode titles
		/// </summary>
		/// <param name="seasonNode">XmlNode that contains season data</param>
		public Season(XmlNode seasonNode)
		{
			number = seasonNode.Attributes["number"].ToString();
			epNameList = new List<string>();

			foreach (XmlNode epNode in seasonNode.ChildNodes)
			{
				epNameList.Add(epNode.InnerText.ToString());
			}
		}

		/// <summary>
		/// Gets the list of episode names for this season with episode number, properly formatted. (## - name)
		/// </summary>
		/// <returns>List<string> containing episode names</returns>
		public List<string> GetEpNameList()
		{
			List<string> list = new List<string>();
			int epNum = 1;
			foreach (string name in epNameList)
			{
				if (epNum < 10)
					list.Add("0" + epNum.ToString() + " - " + name);
				else
					list.Add(epNum.ToString() + " - " + name);
				epNum++;
			}

			return list;
		}
	}
}
