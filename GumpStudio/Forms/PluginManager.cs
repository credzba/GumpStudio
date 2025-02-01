// Decompiled with JetBrains decompiler
// Type: GumpStudio.PluginManager
// Assembly: GumpStudioCore, Version=1.8.3024.24259, Culture=neutral, PublicKeyToken=null
// MVID: A77D32E5-7519-4865-AA26-DCCB34429732
// Assembly location: C:\GumpStudio_1_8_R3_quinted-02\GumpStudioCore.dll

using GumpStudio.Forms;
using GumpStudio.Plugins;
using System;
using System.Collections;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;

namespace GumpStudio
{
    public class PluginManager : Form
    {
        private Button _cmdAdd;
        private Button _cmdCancel;
        private Button _cmdMoveDown;
        private Button _cmdMoveUp;
        private Button _cmdOK;
        private Button _cmdRemove;
        private GroupBox _GroupBox1;
        private Label _Label1;
        private Label _Label2;
        private Label _Label3;
        private Label _Label4;
        private Label _Label5;
        private Label _Label6;
        private ListBox _lstAvailable;
        private ListBox _lstLoaded;
        private TextBox _txtAuthor;
        private TextBox _txtDescription;
        private TextBox _txtEmail;
        private TextBox _txtVersion;
        public ArrayList AvailablePlugins;
        private IContainer components;
        public ArrayList LoadedPlugins;
        public DesignerForm MainForm;
        public PluginInfo[] OrderList;

        public PluginManager()
        {
            this.Load += new EventHandler(this.PluginManager_Load);
            this.InitializeComponent();
        }

        private void cmdAdd_Click(object sender, EventArgs e)
        {
            PluginInfo pluginInfo = (PluginInfo)this._lstAvailable.Items[this._lstAvailable.SelectedIndex];
            this._lstAvailable.Items.RemoveAt(this._lstAvailable.SelectedIndex);
            this._lstLoaded.Items.Add(pluginInfo);
        }

        private void cmdCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void cmdMoveDown_Click(object sender, EventArgs e)
        {
            int selectedIndex = _lstLoaded.SelectedIndex;
            if (selectedIndex >= _lstLoaded.Items.Count - 2)
                return;

            var item = _lstLoaded.SelectedItem;
            _lstLoaded.Items.RemoveAt(selectedIndex);
            _lstLoaded.Items.Insert(selectedIndex + 1, item);
            _lstLoaded.SelectedIndex = selectedIndex + 1;
        }

        private void cmdMoveUp_Click(object sender, EventArgs e)
        {
            int selectedIndex = _lstLoaded.SelectedIndex;
            if (selectedIndex <= 0)
                return;

            var item = _lstLoaded.SelectedItem;
            _lstLoaded.Items.RemoveAt(selectedIndex);
            _lstLoaded.Items.Insert(selectedIndex - 1, item);
            _lstLoaded.SelectedIndex = selectedIndex - 1;
        }

        private void cmdOK_Click(object sender, EventArgs e)
        {
            MessageBox.Show("You will need to restart the program for plugin changes to take effect.");

            var pluginInfoArray = _lstLoaded.Items.Cast<PluginInfo>().ToArray();

            MainForm.PluginTypesToLoad = pluginInfoArray;
            MainForm.WritePluginsToLoad();
            DialogResult = DialogResult.OK;
        }
        private void cmdRemove_Click(object sender, EventArgs e)
        {
            PluginInfo pluginInfo = (PluginInfo)this._lstLoaded.Items[this._lstLoaded.SelectedIndex];
            this._lstLoaded.Items.RemoveAt(this._lstLoaded.SelectedIndex);
            this._lstAvailable.Items.Add(pluginInfo);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && this.components != null)
                this.components.Dispose();
            base.Dispose(disposing);
        }


        private void InitializeComponent()
        {
            this._Label1 = new System.Windows.Forms.Label();
            this._cmdMoveUp = new System.Windows.Forms.Button();
            this._cmdMoveDown = new System.Windows.Forms.Button();
            this._cmdOK = new System.Windows.Forms.Button();
            this._GroupBox1 = new System.Windows.Forms.GroupBox();
            this._txtDescription = new System.Windows.Forms.TextBox();
            this._txtVersion = new System.Windows.Forms.TextBox();
            this._txtEmail = new System.Windows.Forms.TextBox();
            this._txtAuthor = new System.Windows.Forms.TextBox();
            this._Label5 = new System.Windows.Forms.Label();
            this._Label4 = new System.Windows.Forms.Label();
            this._Label3 = new System.Windows.Forms.Label();
            this._Label2 = new System.Windows.Forms.Label();
            this._cmdCancel = new System.Windows.Forms.Button();
            this._cmdAdd = new System.Windows.Forms.Button();
            this._cmdRemove = new System.Windows.Forms.Button();
            this._lstAvailable = new System.Windows.Forms.ListBox();
            this._Label6 = new System.Windows.Forms.Label();
            this._lstLoaded = new System.Windows.Forms.ListBox();
            this._GroupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // _Label1
            // 
            this._Label1.AutoSize = true;
            this._Label1.Location = new System.Drawing.Point(8, 8);
            this._Label1.Name = "_Label1";
            this._Label1.Size = new System.Drawing.Size(80, 13);
            this._Label1.TabIndex = 2;
            this._Label1.Text = "Loaded Plugins";
            // 
            // _cmdMoveUp
            // 
            this._cmdMoveUp.Enabled = false;
            this._cmdMoveUp.Image = global::GumpStudio.Properties.Resources.cmdMoveUp_Image;
            this._cmdMoveUp.Location = new System.Drawing.Point(152, 24);
            this._cmdMoveUp.Name = "_cmdMoveUp";
            this._cmdMoveUp.Size = new System.Drawing.Size(24, 32);
            this._cmdMoveUp.TabIndex = 3;
            this._cmdMoveUp.Click += new System.EventHandler(this.cmdMoveUp_Click);
            // 
            // _cmdMoveDown
            // 
            this._cmdMoveDown.Enabled = false;
            this._cmdMoveDown.Image = global::GumpStudio.Properties.Resources.cmdMoveDown_Image;
            this._cmdMoveDown.Location = new System.Drawing.Point(152, 104);
            this._cmdMoveDown.Name = "_cmdMoveDown";
            this._cmdMoveDown.Size = new System.Drawing.Size(24, 32);
            this._cmdMoveDown.TabIndex = 4;
            this._cmdMoveDown.Click += new System.EventHandler(this.cmdMoveDown_Click);
            // 
            // _cmdOK
            // 
            this._cmdOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this._cmdOK.Location = new System.Drawing.Point(176, 304);
            this._cmdOK.Name = "_cmdOK";
            this._cmdOK.Size = new System.Drawing.Size(72, 23);
            this._cmdOK.TabIndex = 6;
            this._cmdOK.Text = "OK";
            this._cmdOK.Click += new System.EventHandler(this.cmdOK_Click);
            // 
            // _GroupBox1
            // 
            this._GroupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this._GroupBox1.Controls.Add(this._txtDescription);
            this._GroupBox1.Controls.Add(this._txtVersion);
            this._GroupBox1.Controls.Add(this._txtEmail);
            this._GroupBox1.Controls.Add(this._txtAuthor);
            this._GroupBox1.Controls.Add(this._Label5);
            this._GroupBox1.Controls.Add(this._Label4);
            this._GroupBox1.Controls.Add(this._Label3);
            this._GroupBox1.Controls.Add(this._Label2);
            this._GroupBox1.Location = new System.Drawing.Point(8, 144);
            this._GroupBox1.Name = "_GroupBox1";
            this._GroupBox1.Size = new System.Drawing.Size(328, 152);
            this._GroupBox1.TabIndex = 7;
            this._GroupBox1.TabStop = false;
            this._GroupBox1.Text = "Description";
            // 
            // _txtDescription
            // 
            this._txtDescription.Location = new System.Drawing.Point(72, 88);
            this._txtDescription.Multiline = true;
            this._txtDescription.Name = "_txtDescription";
            this._txtDescription.ReadOnly = true;
            this._txtDescription.Size = new System.Drawing.Size(248, 20);
            this._txtDescription.TabIndex = 7;
            // 
            // _txtVersion
            // 
            this._txtVersion.Location = new System.Drawing.Point(72, 64);
            this._txtVersion.Name = "_txtVersion";
            this._txtVersion.ReadOnly = true;
            this._txtVersion.Size = new System.Drawing.Size(248, 20);
            this._txtVersion.TabIndex = 6;
            // 
            // _txtEmail
            // 
            this._txtEmail.Location = new System.Drawing.Point(72, 40);
            this._txtEmail.Name = "_txtEmail";
            this._txtEmail.ReadOnly = true;
            this._txtEmail.Size = new System.Drawing.Size(248, 20);
            this._txtEmail.TabIndex = 5;
            // 
            // _txtAuthor
            // 
            this._txtAuthor.Location = new System.Drawing.Point(72, 16);
            this._txtAuthor.Name = "_txtAuthor";
            this._txtAuthor.ReadOnly = true;
            this._txtAuthor.Size = new System.Drawing.Size(248, 20);
            this._txtAuthor.TabIndex = 4;
            // 
            // _Label5
            // 
            this._Label5.AutoSize = true;
            this._Label5.Location = new System.Drawing.Point(8, 88);
            this._Label5.Name = "_Label5";
            this._Label5.Size = new System.Drawing.Size(60, 13);
            this._Label5.TabIndex = 3;
            this._Label5.Text = "Description";
            // 
            // _Label4
            // 
            this._Label4.AutoSize = true;
            this._Label4.Location = new System.Drawing.Point(8, 64);
            this._Label4.Name = "_Label4";
            this._Label4.Size = new System.Drawing.Size(42, 13);
            this._Label4.TabIndex = 2;
            this._Label4.Text = "Version";
            // 
            // _Label3
            // 
            this._Label3.AutoSize = true;
            this._Label3.Location = new System.Drawing.Point(8, 40);
            this._Label3.Name = "_Label3";
            this._Label3.Size = new System.Drawing.Size(32, 13);
            this._Label3.TabIndex = 1;
            this._Label3.Text = "Email";
            // 
            // _Label2
            // 
            this._Label2.AutoSize = true;
            this._Label2.Location = new System.Drawing.Point(8, 16);
            this._Label2.Name = "_Label2";
            this._Label2.Size = new System.Drawing.Size(38, 13);
            this._Label2.TabIndex = 0;
            this._Label2.Text = "Author";
            // 
            // _cmdCancel
            // 
            this._cmdCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this._cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this._cmdCancel.Location = new System.Drawing.Point(264, 304);
            this._cmdCancel.Name = "_cmdCancel";
            this._cmdCancel.Size = new System.Drawing.Size(75, 23);
            this._cmdCancel.TabIndex = 8;
            this._cmdCancel.Text = "Cancel";
            this._cmdCancel.Click += new System.EventHandler(this.cmdCancel_Click);
            // 
            // _cmdAdd
            // 
            this._cmdAdd.Enabled = false;
            this._cmdAdd.Image = global::GumpStudio.Properties.Resources.cmdAdd_Image;
            this._cmdAdd.Location = new System.Drawing.Point(152, 56);
            this._cmdAdd.Name = "_cmdAdd";
            this._cmdAdd.Size = new System.Drawing.Size(40, 23);
            this._cmdAdd.TabIndex = 9;
            this._cmdAdd.Click += new System.EventHandler(this.cmdAdd_Click);
            // 
            // _cmdRemove
            // 
            this._cmdRemove.Enabled = false;
            this._cmdRemove.Image = global::GumpStudio.Properties.Resources.cmdRemove_Image;
            this._cmdRemove.Location = new System.Drawing.Point(152, 80);
            this._cmdRemove.Name = "_cmdRemove";
            this._cmdRemove.Size = new System.Drawing.Size(40, 23);
            this._cmdRemove.TabIndex = 10;
            this._cmdRemove.Click += new System.EventHandler(this.cmdRemove_Click);
            // 
            // _lstAvailable
            // 
            this._lstAvailable.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)));
            this._lstAvailable.IntegralHeight = false;
            this._lstAvailable.Location = new System.Drawing.Point(192, 24);
            this._lstAvailable.Name = "_lstAvailable";
            this._lstAvailable.Size = new System.Drawing.Size(144, 112);
            this._lstAvailable.TabIndex = 11;
            this._lstAvailable.SelectedIndexChanged += new System.EventHandler(this.Plugins_SelectedIndexChanged);
            // 
            // _Label6
            // 
            this._Label6.AutoSize = true;
            this._Label6.Location = new System.Drawing.Point(184, 8);
            this._Label6.Name = "_Label6";
            this._Label6.Size = new System.Drawing.Size(87, 13);
            this._Label6.TabIndex = 12;
            this._Label6.Text = "Available Plugins";
            // 
            // _lstLoaded
            // 
            this._lstLoaded.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)));
            this._lstLoaded.IntegralHeight = false;
            this._lstLoaded.Location = new System.Drawing.Point(8, 24);
            this._lstLoaded.Name = "_lstLoaded";
            this._lstLoaded.Size = new System.Drawing.Size(144, 112);
            this._lstLoaded.TabIndex = 13;
            this._lstLoaded.SelectedIndexChanged += new System.EventHandler(this.Plugins_SelectedIndexChanged);
            // 
            // PluginManager
            // 
            this.AcceptButton = this._cmdOK;
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.CancelButton = this._cmdCancel;
            this.ClientSize = new System.Drawing.Size(336, 336);
            this.Controls.Add(this._lstLoaded);
            this.Controls.Add(this._Label6);
            this.Controls.Add(this._lstAvailable);
            this.Controls.Add(this._cmdCancel);
            this.Controls.Add(this._GroupBox1);
            this.Controls.Add(this._cmdOK);
            this.Controls.Add(this._Label1);
            this.Controls.Add(this._cmdRemove);
            this.Controls.Add(this._cmdAdd);
            this.Controls.Add(this._cmdMoveDown);
            this.Controls.Add(this._cmdMoveUp);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(352, 1200);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(352, 370);
            this.Name = "PluginManager";
            this.Text = "Plugin Manager";
            this.Load += new System.EventHandler(this.PluginManager_Load);
            this._GroupBox1.ResumeLayout(false);
            this._GroupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void PluginManager_Load(object sender, EventArgs e)
        {
            _lstLoaded.Items.Clear();
            _lstAvailable.Items.Clear();

            if (OrderList != null)
            {
                foreach (PluginInfo order in OrderList)
                {
                    bool flag = AvailablePlugins.Cast<BasePlugin>()
                        .Any(plugin => plugin.GetPluginInfo().Equals(order));

                    if (flag)
                    {
                        _lstLoaded.Items.Add(order);
                    }
                }
            }

            foreach (BasePlugin plugin in AvailablePlugins)
            {
                if (!plugin.IsLoaded)
                {
                    _lstAvailable.Items.Add(plugin.GetPluginInfo());
                }
            }
        }

        private void Plugins_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListBox listBox = (ListBox)sender;
            if (listBox.SelectedIndex == -1)
                return;
            PluginInfo selectedItem = (PluginInfo)listBox.SelectedItem;
            this._txtAuthor.Text = selectedItem.AuthorName;
            this._txtEmail.Text = selectedItem.AuthorEmail;
            this._txtVersion.Text = selectedItem.Version;
            this._txtDescription.Text = selectedItem.Description;
            if (this._lstLoaded.SelectedIndex > 0)
                this._cmdMoveUp.Enabled = true;
            else
                this._cmdMoveUp.Enabled = false;
            if (this._lstLoaded.SelectedIndex < listBox.Items.Count - 1)
                this._cmdMoveDown.Enabled = true;
            else
                this._cmdMoveDown.Enabled = false;
            if (this._lstAvailable.SelectedIndex == -1)
                this._cmdAdd.Enabled = false;
            else
                this._cmdAdd.Enabled = true;
            if (this._lstLoaded.SelectedIndex == -1)
                this._cmdRemove.Enabled = false;
            else
                this._cmdRemove.Enabled = true;
        }
    }
}
