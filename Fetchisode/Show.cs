using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml;

namespace Fetchisode
{
	/// <summary>
	/// Container to hold all of the season and episode data for a specific show
	/// </summary>
	public class Show
	{
		public List<Season> seasonList;
		public string showName;
		public string url;

		/// <summary>
		/// Constructor
		/// 
		/// Used when loading from website.
		/// 
		/// Parses season and episode information from the html, loaded from page url.
		/// </summary>
		/// <param name="name">Title of the show.</param>
		/// <param name="web">URL of the show page.</param>
		public Show(string name, string web)
		{
			showName = name;
			url = web;
		}

		/// <summary>
		/// Constructor
		/// 
		/// Used when loading from xml.
		/// 
		/// Takes in the shows xml node, gets show and url info.
		/// </summary>
		/// <param name="showNode">XmlNode that contains show data</param>
		public Show(XmlNode showNode)
		{
			showName = showNode["Name"].InnerText.ToString();
			url = showNode["URL"].InnerText.ToString();
		}

		/// <summary>
		/// Downloads the html for the show page, then parses the season and episode info.
		/// </summary>
		public void Populate()
		{
			//Connects to Internet, grabs html of page of a particular show
			WebClient web = new WebClient();
			string html = web.DownloadString(@"http://epguides.com/" + url + @"/");

			//Turns html into a list of strings, all the better to parse
			List<string> htmlList = new List<string>();
			htmlList = html.Split('\n').ToList();

			seasonList = new List<Season>();

			Match regexMatch;
			string season_temp;
			string ep_temp;
			string title_temp;

			//Every show has at least one season, so create the first one manually
			seasonList.Add(new Season("1"));
			
			foreach (string line in htmlList)
			{
				//Examples:
				//1      1-01      ARR-101   02/Nov/03   <a href='http://www.tvrage.com/Arrested_Development/episodes/14360' title='Arrested Development season 1 episode 1'>Pilot</a> 
				//3      1-03      1AGE05    04/Oct/02   <a href='http://www.tvrage.com/Firefly/episodes/62715' title='Firefly season 1 episode 3'>Our Mrs. Reynolds</a>  <span class='recap'>[<a href='http://www.tvrage.com/Firefly/episodes/62715/recap'>Recap</a>]</span>

				regexMatch = Regex.Match(line, @"^[0-9]+ +(?<season>[0-9]+)-(?<episode>[0-9][0-9]).+<a href.+>(?<title>.+)</a>($| )");

				season_temp = regexMatch.Groups["season"].Value;
				ep_temp = regexMatch.Groups["episode"].Value;
				title_temp = regexMatch.Groups["title"].Value;

				if (regexMatch.Success)
				{
					//if we've moved on to the next season...
					if (!seasonList.Count.ToString().Equals(season_temp))
					{
						//...create a new season
						seasonList.Add(new Season(season_temp));
					}

					//Add the name to the last season in the list (the highest numbered season)
					//TODO: function to add episodes to the list, rather than accessing the list directly.
					seasonList[seasonList.Count - 1].epNameList.Add(title_temp);
				}
			}
		}

		public override string ToString()
		{
			return showName;
		}
	}
}
