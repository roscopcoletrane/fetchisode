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
		#region Globals

		//Selected folder that houses the files to be renamed.
		DirectoryInfo videoFolder;
		//Location of the registry key that saves the last selected videoFolder.
		RegistryKey videoFolderKey;

		//Form that displays on top while show data is being initially downloaded.
		Loading frmLoading;

		//File being worked on.
		FileInfo selectedFile;

		//Handles all the downloading of the lists of shows, either from the web or xml file.
		ShowGrabber grabber;

		//Once a show is selected from the combobox, this is used for accessing the selected
		//	show's episode info.
		Show selectedShow;

		//Where the ShowGrabber does its business, as not to lock up the interface.
		private BackgroundWorker SecondaryThread = new BackgroundWorker();

		#endregion


		public FormMain()
		{
			InitializeComponent();
		}

		private void FormMain_Load(object sender, EventArgs e)
		{
			List<string> letterList = new List<string>() { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };
			comboBoxLetter.DataSource = letterList;
			comboBoxLetter.Text = "";

			//Start the data collection.
			SecondaryThread.DoWork += SecondaryThread_DoWork;
			if (!SecondaryThread.IsBusy)
				SecondaryThread.RunWorkerAsync();
			SecondaryThread.RunWorkerCompleted += SecondaryThread_RunWorkerCompleted;
		}

		void SecondaryThread_DoWork(object sender, DoWorkEventArgs e)
		{
			this.BeginInvoke((Action)delegate
			{
				frmLoading = new Loading();
				frmLoading.Show();
			});
			grabber = new ShowGrabber();
		}

		void SecondaryThread_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			textBoxVideoDir.Text = GetVideoFolderPath();
			PopulateFileList();
			frmLoading.Close();
		}



		private void buttonLoadDir_Click(object sender, EventArgs e)
		{
			if (LoadFolderBrowser.ShowDialog() == System.Windows.Forms.DialogResult.OK)
			{
				textBoxVideoDir.Text = LoadFolderBrowser.SelectedPath;
				PopulateFileList();
			}
		}

		private void PopulateFileList()
		{
			videoFolder = new DirectoryInfo(textBoxVideoDir.Text);
			if (videoFolder.Exists)
				listBoxFileList.DataSource = videoFolder.GetFiles();
		}

		private string GetVideoFolderPath()
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
				//Key does not exist, I wish there was a better way to handle that.
				Registry.CurrentUser.CreateSubKey(videoFolderKeyPath);
				videoFolderKey = Registry.CurrentUser.OpenSubKey(videoFolderKeyPath, true);
				videoFolderKey.SetValue("Video Folder Path", @"C:\");
			}
			
			videoFolderPath = videoFolderKey.GetValue("Video Folder Path").ToString();

			return videoFolderPath;
		}



		private void listBoxFileList_SelectedIndexChanged(object sender, EventArgs e)
		{
			selectedFile = new FileInfo(textBoxVideoDir.Text + "\\" + listBoxFileList.SelectedItem.ToString());

			if (selectedFile.Name.StartsWith("The"))
			{
				//...assuming the name starts with "The." or "The " (specifically one character after it)
				comboBoxLetter.Text = selectedFile.Name[4].ToString().ToUpper();
			}
			else
			{
				comboBoxLetter.Text = selectedFile.Name[0].ToString().ToUpper();
			}
		}

		private void comboBoxLetter_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (grabber != null)
				comboBoxShow.DataSource = grabber.GetShowList(comboBoxLetter.Text);
		}

		private void buttonLoadSeasonList_Click(object sender, EventArgs e)
		{
			//Create Show object
			selectedShow = grabber.GetShowList(comboBoxLetter.Text)[comboBoxShow.SelectedIndex];

			if (selectedShow.seasonList == null)
				selectedShow.Populate();

			//Populate Season List
			listBoxSeason.Items.Clear();
			foreach (Season s in selectedShow.seasonList)
			{
				listBoxSeason.Items.Add("Season " + s.number);
			}

			//Cleanup
			listBoxEpisode.DataSource = null;
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



		private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
		{
			videoFolderKey.SetValue("Video Folder Path", textBoxVideoDir.Text);
		}
	}
}
