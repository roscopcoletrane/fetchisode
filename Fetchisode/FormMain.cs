using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.Net;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using Microsoft.Win32;

namespace Fetchisode
{
	public partial class FormMain : Form
	{
		FileInfo selectedFile;
		List<string> showUrlList;
		List<string> showNameList;
		Show selectedShow;
		DirectoryInfo videoFolder;
		RegistryKey videoFolderKey;
		

		public FormMain()
		{
			InitializeComponent();
		}

		private void FormMain_Load(object sender, EventArgs e)
		{
			//Populate letter combo box
			List<string> letterList = new List<string>() { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };
			comboBoxLetter.DataSource = letterList;
			comboBoxLetter.Text = "";

			//Populate file list from text box
			textBoxVideoDir.Text = ReadRegistry();
			PopulateFileList();
		}

		private void PopulateFileList()
		{
			videoFolder = new DirectoryInfo(textBoxVideoDir.Text);
			if (videoFolder.Exists)
				listBoxFileList.DataSource = videoFolder.GetFiles();
		}

		private string ReadRegistry()
		{
			string videoFolderPath = null;
			string videoFolderKeyPath = @"SOFTWARE\Rosco\FetchisodeCopy";

			try
			{
				videoFolderKey = Registry.CurrentUser.OpenSubKey(videoFolderKeyPath, true);
				videoFolderPath = videoFolderKey.GetValue("Video Folder Path").ToString();
			}
			catch (Exception)
			{
				//Key does not exist
				Registry.CurrentUser.CreateSubKey(videoFolderKeyPath);
				videoFolderKey = Registry.CurrentUser.OpenSubKey(videoFolderKeyPath, true);
				videoFolderKey.SetValue("Video Folder Path", @"H:\TV\ZZZzzz");
			}
			
			videoFolderPath = videoFolderKey.GetValue("Video Folder Path").ToString();

			return videoFolderPath;
		}

		private void listBoxFileList_SelectedIndexChanged(object sender, EventArgs e)
		{
			selectedFile = new FileInfo(textBoxVideoDir.Text + "\\" + listBoxFileList.SelectedItem.ToString());

			//Extrapolate first letter

			//If file starts with 'The'
			if (selectedFile.Name.StartsWith("The"))
			{
				//...assuming the name starts with "The." or "The "...
				comboBoxLetter.Text = selectedFile.Name[4].ToString().ToUpper();
			}
			else
			{
				comboBoxLetter.Text = selectedFile.Name[0].ToString().ToUpper();
			}

			//Populate show list (call comboBoxLetter_SelectedIndexChanged)
			//PopulateShows(comboBoxLetter.Text);

			//Cleanup
			//listBoxEpisode.DataSource = null;
		}


		private void PopulateShows(string letter)
		{
			//Connects to Internet, grabs html of page of a particular letter
			WebClient web = new WebClient();
			string html = web.DownloadString(@"http://www.epguides.com/menu" + letter + @"/");

			//Turns html into a list of strings, the better to parse
			List<string> htmlList = new List<string>();
			htmlList = html.Split('\n').ToList();

			showUrlList = new List<string>();
			showNameList = new List<string>();

			Match regexMatch;
			foreach (string line in htmlList)
			{
				//<li><b><a href="../AforAndromeda/">A for Andromeda</a></b></li>
				regexMatch = Regex.Match(line, @"<li><b><a href=""../(?<url>.+)/"">(?<name>.+)</a></b>.*</li>");

				if (regexMatch.Success)
				{
					showUrlList.Add(regexMatch.Groups["url"].Value);
					showNameList.Add(regexMatch.Groups["name"].Value);
				}
			}

			comboBoxShow.DataSource = showNameList;
		}

		private void listBoxSeason_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (listBoxSeason.SelectedIndex < 0)
				return;
			//Populate episode list
			listBoxEpisode.DataSource = null;
			listBoxEpisode.DataSource = selectedShow.seasonList[listBoxSeason.SelectedIndex].GetEpNameList();
			//listBoxEpisode.Refresh();
		}


		private void listBoxEpisode_DoubleClick(object sender, EventArgs e)
		{
			string source = selectedFile.FullName;
			string destination = BuildFileName();

			File.Move(source, destination);

			if (listBoxFileList.SelectedIndex < listBoxFileList.Items.Count - 1)
				listBoxFileList.SelectedIndex++;

			//Get text from selected item
			string selection = listBoxFileList.SelectedItem.ToString();
			
			//Refresh
			listBoxFileList.DataSource = videoFolder.GetFiles();

			//Position selector based on saved text
			int selectionIndex = listBoxFileList.FindString(selection);
			if (selectionIndex >= 0)
				listBoxFileList.SelectedIndex = selectionIndex;
			else
				listBoxFileList.SelectedIndex = listBoxFileList.Items.Count - 1;

		}

		private string BuildFileName()
		{
			string newFileName = textBoxVideoDir.Text + "\\";

			newFileName += comboBoxShow.Text + " - ";
			newFileName += (listBoxSeason.SelectedIndex + 1).ToString();
			if (listBoxEpisode.SelectedIndex < 9)
				newFileName += "0";
			newFileName += (listBoxEpisode.SelectedIndex + 1).ToString() + " - ";
			newFileName += selectedShow.seasonList[listBoxSeason.SelectedIndex].epNameList[listBoxEpisode.SelectedIndex];
			newFileName += selectedFile.Extension;

			return newFileName;
		}


		private void buttonLoadShowList_Click(object sender, EventArgs e)
		{
			PopulateShows(comboBoxLetter.SelectedItem.ToString());
			listBoxEpisode.DataSource = null;
		}

		private void buttonLoadSeasonList_Click(object sender, EventArgs e)
		{
			//Create Show object
			selectedShow = new Show(showNameList[comboBoxShow.SelectedIndex].ToString(), showUrlList[comboBoxShow.SelectedIndex].ToString());

			//Populate Season List
			listBoxSeason.Items.Clear();
			foreach (Season s in selectedShow.seasonList)
			{
				listBoxSeason.Items.Add("Season " + s.number);
			}

			//Cleanup
			listBoxEpisode.DataSource = null;
		}

		private void buttonLoadDir_Click(object sender, EventArgs e)
		{
			if (LoadFolderBrowser.ShowDialog() == System.Windows.Forms.DialogResult.OK)
			{
				textBoxVideoDir.Text = LoadFolderBrowser.SelectedPath;
				PopulateFileList();
			}
		}

		private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
		{
			videoFolderKey.SetValue("Video Folder Path", textBoxVideoDir.Text);
		}
	}
}
