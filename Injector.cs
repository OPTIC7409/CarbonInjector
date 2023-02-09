using DiscordRPC;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CarbonInjector
{

    public partial class Injector : Form
    {
        public DiscordRpcClient client;
        public Config config;

        DiscordRPC.RichPresence presence = new DiscordRPC.RichPresence();
        public Injector()
        {
            config = Utility.config;
            InitializeComponent();
            Utility.Init();
            ApplyConfig();
        }

        private void selectDLLButton_Click(object sender, EventArgs e)
        {
            FileDialog fileDialog = new OpenFileDialog();
            fileDialog.Filter = "DLL Files (*.dll)|*.dll";
            fileDialog.Title = "Select a DLL to inject";
            fileDialog.ShowDialog();

            if (fileDialog.FileName != "")
            {
                dllPathTextBox.Text = "DLL: " + fileDialog.FileName;
            } else
            {
                dllPathTextBox.Text = "DLL: None";
            }
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            if (dllPathTextBox.Text != "DLL: None")
            {
                Utility.Inject(dllPathTextBox.Text.Replace("DLL: ", ""));
            }
            else
            {
                MessageBox.Show("Please select a DLL to inject");
            }
        }

        private void DRPCToggle_CheckedChanged(object sender, EventArgs e)
        {
            if (DRPCToggle.Checked == true)
            {
                Utility.updateRPC();
                config.RPC = true;
            }
            else
            {
                Utility.client.ClearPresence();
                config.RPC = false;
            }
            Config.SaveDaConfig(config);
        }
        private void ApplyConfig()
        {
            /*if(Utility.config.autoInject)
            {
                AutoInjectToggle.Checked = true;
            } else
            {
                AutoInjectToggle.Checked = false;
            }*/
            
            if(Utility.config.RPC)
            {
                DRPCToggle.Checked = true;
            } else
            {
                DRPCToggle.Checked = false;
            }
            
            if(Utility.config.DLLPath != "DLL: None")
            {
                dllPathTextBox.Text = Utility.config.DLLPath;
            } else
            {
                dllPathTextBox.Text = Utility.config.DLLPath = "DLL: None";
            }

           /* if(Utility.config.openGame)
            {
                openGameToggle.Checked = true;
            } else
            {
                openGameToggle.Checked = false;
            }*/
        }

        private void Injector_FormClosing(object sender, FormClosingEventArgs e)
        {
            Config.SaveDaConfig(config);
        }

        private void openGameToggle_CheckedChanged(object sender, EventArgs e)
        {
            /*if (openGameToggle.Checked == true)
            {
                config.openGame = true;
            }
            else
            {
                config.openGame = false;
            }
            Config.SaveConfig(config);*/
        }

        private void AutoInjectToggle_CheckedChanged(object sender, EventArgs e)
        {
            /*if (AutoInjectToggle.Checked == true)
            {
                config.autoInject = true;
            }
            else
            {
                config.autoInject = false;
            }
            Config.SaveConfig(config);*/
        }

        private void label1_Click(object sender, EventArgs e)
        {
            Process.Start("https://discord.gg/zGt3qUB8EG");
        }

        private void guna2PictureBox1_Click(object sender, EventArgs e)
        {
            Process.Start("https://discord.gg/zGt3qUB8EG");
        }
    }
}
