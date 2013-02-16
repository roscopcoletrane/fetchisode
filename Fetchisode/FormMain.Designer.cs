namespace Fetchisode
{
	partial class FormMain
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.listBoxFileList = new System.Windows.Forms.ListBox();
			this.textBoxVideoDir = new System.Windows.Forms.TextBox();
			this.buttonLoadDir = new System.Windows.Forms.Button();
			this.comboBoxLetter = new System.Windows.Forms.ComboBox();
			this.comboBoxShow = new System.Windows.Forms.ComboBox();
			this.listBoxSeason = new System.Windows.Forms.ListBox();
			this.listBoxEpisode = new System.Windows.Forms.ListBox();
			this.buttonLoadShowList = new System.Windows.Forms.Button();
			this.buttonLoadSeasonList = new System.Windows.Forms.Button();
			this.LoadFolderBrowser = new System.Windows.Forms.FolderBrowserDialog();
			this.ChangeFolderBrowser = new System.Windows.Forms.FolderBrowserDialog();
			this.SuspendLayout();
			// 
			// listBoxFileList
			// 
			this.listBoxFileList.FormattingEnabled = true;
			this.listBoxFileList.Location = new System.Drawing.Point(12, 38);
			this.listBoxFileList.Name = "listBoxFileList";
			this.listBoxFileList.Size = new System.Drawing.Size(359, 368);
			this.listBoxFileList.TabIndex = 0;
			this.listBoxFileList.SelectedIndexChanged += new System.EventHandler(this.listBoxFileList_SelectedIndexChanged);
			// 
			// textBoxVideoDir
			// 
			this.textBoxVideoDir.Location = new System.Drawing.Point(12, 12);
			this.textBoxVideoDir.Name = "textBoxVideoDir";
			this.textBoxVideoDir.Size = new System.Drawing.Size(300, 20);
			this.textBoxVideoDir.TabIndex = 1;
			this.textBoxVideoDir.Text = "H:\\TV";
			// 
			// buttonLoadDir
			// 
			this.buttonLoadDir.Location = new System.Drawing.Point(318, 10);
			this.buttonLoadDir.Name = "buttonLoadDir";
			this.buttonLoadDir.Size = new System.Drawing.Size(53, 23);
			this.buttonLoadDir.TabIndex = 2;
			this.buttonLoadDir.Text = "Load";
			this.buttonLoadDir.UseVisualStyleBackColor = true;
			this.buttonLoadDir.Click += new System.EventHandler(this.buttonLoadDir_Click);
			// 
			// comboBoxLetter
			// 
			this.comboBoxLetter.FormattingEnabled = true;
			this.comboBoxLetter.Location = new System.Drawing.Point(377, 38);
			this.comboBoxLetter.Name = "comboBoxLetter";
			this.comboBoxLetter.Size = new System.Drawing.Size(37, 21);
			this.comboBoxLetter.TabIndex = 3;
			// 
			// comboBoxShow
			// 
			this.comboBoxShow.FormattingEnabled = true;
			this.comboBoxShow.Location = new System.Drawing.Point(444, 38);
			this.comboBoxShow.Name = "comboBoxShow";
			this.comboBoxShow.Size = new System.Drawing.Size(233, 21);
			this.comboBoxShow.TabIndex = 4;
			// 
			// listBoxSeason
			// 
			this.listBoxSeason.FormattingEnabled = true;
			this.listBoxSeason.Location = new System.Drawing.Point(377, 65);
			this.listBoxSeason.Name = "listBoxSeason";
			this.listBoxSeason.Size = new System.Drawing.Size(107, 342);
			this.listBoxSeason.TabIndex = 5;
			this.listBoxSeason.SelectedIndexChanged += new System.EventHandler(this.listBoxSeason_SelectedIndexChanged);
			// 
			// listBoxEpisode
			// 
			this.listBoxEpisode.FormattingEnabled = true;
			this.listBoxEpisode.Location = new System.Drawing.Point(490, 65);
			this.listBoxEpisode.Name = "listBoxEpisode";
			this.listBoxEpisode.Size = new System.Drawing.Size(219, 342);
			this.listBoxEpisode.TabIndex = 6;
			this.listBoxEpisode.DoubleClick += new System.EventHandler(this.listBoxEpisode_DoubleClick);
			// 
			// buttonLoadShowList
			// 
			this.buttonLoadShowList.Location = new System.Drawing.Point(420, 36);
			this.buttonLoadShowList.Name = "buttonLoadShowList";
			this.buttonLoadShowList.Size = new System.Drawing.Size(18, 23);
			this.buttonLoadShowList.TabIndex = 7;
			this.buttonLoadShowList.Text = ">";
			this.buttonLoadShowList.UseVisualStyleBackColor = true;
			this.buttonLoadShowList.Click += new System.EventHandler(this.buttonLoadShowList_Click);
			// 
			// buttonLoadSeasonList
			// 
			this.buttonLoadSeasonList.Location = new System.Drawing.Point(683, 36);
			this.buttonLoadSeasonList.Name = "buttonLoadSeasonList";
			this.buttonLoadSeasonList.Size = new System.Drawing.Size(25, 23);
			this.buttonLoadSeasonList.TabIndex = 8;
			this.buttonLoadSeasonList.Text = "\\/";
			this.buttonLoadSeasonList.UseVisualStyleBackColor = true;
			this.buttonLoadSeasonList.Click += new System.EventHandler(this.buttonLoadSeasonList_Click);
			// 
			// LoadFolderBrowser
			// 
			this.LoadFolderBrowser.RootFolder = System.Environment.SpecialFolder.MyComputer;
			this.LoadFolderBrowser.SelectedPath = "K:\\DVD\\fin";
			// 
			// ChangeFolderBrowser
			// 
			this.ChangeFolderBrowser.RootFolder = System.Environment.SpecialFolder.MyComputer;
			// 
			// FormMain
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(720, 417);
			this.Controls.Add(this.buttonLoadSeasonList);
			this.Controls.Add(this.buttonLoadShowList);
			this.Controls.Add(this.listBoxEpisode);
			this.Controls.Add(this.listBoxSeason);
			this.Controls.Add(this.comboBoxShow);
			this.Controls.Add(this.comboBoxLetter);
			this.Controls.Add(this.buttonLoadDir);
			this.Controls.Add(this.textBoxVideoDir);
			this.Controls.Add(this.listBoxFileList);
			this.Name = "FormMain";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Fetchisode Copy";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormMain_FormClosing);
			this.Load += new System.EventHandler(this.FormMain_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ListBox listBoxFileList;
		private System.Windows.Forms.TextBox textBoxVideoDir;
		private System.Windows.Forms.Button buttonLoadDir;
		private System.Windows.Forms.ComboBox comboBoxLetter;
		private System.Windows.Forms.ComboBox comboBoxShow;
		private System.Windows.Forms.ListBox listBoxSeason;
		private System.Windows.Forms.ListBox listBoxEpisode;
		private System.Windows.Forms.Button buttonLoadShowList;
		private System.Windows.Forms.Button buttonLoadSeasonList;
		private System.Windows.Forms.FolderBrowserDialog LoadFolderBrowser;
		private System.Windows.Forms.FolderBrowserDialog ChangeFolderBrowser;
	}
}

