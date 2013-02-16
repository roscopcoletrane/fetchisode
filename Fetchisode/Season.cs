using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
		/// Creates a list to contain episode information for a specific season.
		/// </summary>
		/// <param name="num">Season number</param>
		public Season(string num)
		{
			number = num;
			epNameList = new List<string>();
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
