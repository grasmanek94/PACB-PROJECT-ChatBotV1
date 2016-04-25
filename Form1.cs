using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;

namespace ChatBot
{
    public partial class Form1 : Form
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Winapi)]
        internal static extern IntPtr GetFocus();

        private Control GetFocusedControl()
        {
            Control focusedControl = null;
            // To get hold of the focused control:
            IntPtr focusedHandle = GetFocus();
            if (focusedHandle != IntPtr.Zero)
                // Note that if the focused Control is not a .Net control, then this will return null.
                focusedControl = Control.FromHandle(focusedHandle);
            return focusedControl;
        }

        BotManager bm;
        Control focus_maintainer;

        public Form1()
        {
            ServicePointManager.DefaultConnectionLimit = 64;
           
            InitializeComponent();

            bm = new BotManager();

            bm.OnChatBotAdded += Bot_OnChatBotAdded;
            bm.OnBeginChat += Bot_OnBeginChat;
            bm.OnBeginSearching += Bot_OnBeginSearching;
            bm.OnNewMessage += Bot_OnNewMessage;
            bm.OnEndChat += Bot_OnEndChat;

            bm.OnBotManagerBeforeUpdate += Bm_OnBotManagerBeforeUpdate;
            bm.OnBotManagerAfterUpdate += Bm_OnBotManagerAfterUpdate;

            focus_maintainer = null;
        }

        private void Bm_OnBotManagerBeforeUpdate(BotManager manager)
        {
            focus_maintainer = GetFocusedControl();
        }

        private void Bm_OnBotManagerAfterUpdate(BotManager manager)
        {
            //if(browsers.TabPages.Count > 0)
            //{
            //    browsers.SelectedTab.Controls[0].Focus();
            //}
            if(focus_maintainer != null)
            {
                focus_maintainer.Focus();
            }
        }

        private void Bot_OnChatBotAdded(BotManager manager, ChatBot chatbot)
        {
            TabPage tabPage = new TabPage(chatbot.Identifier.ToString());

            chatbot.GetControl().Dock = DockStyle.Fill;
            tabPage.Controls.Add(chatbot.GetControl());

            browsers.TabPages.Add(tabPage);
        }

        private void Bot_OnEndChat(BotManager manager, ChatBot chatbot)
        {

        }

        private void Bot_OnNewMessage(BotManager manager, ChatBot chatbot, Message message, int messageCount)
        {
            
        }

        private void Bot_OnBeginSearching(BotManager manager, ChatBot chatbot)
        {
            
        }

        private void Bot_OnBeginChat(BotManager manager, ChatBot chatbot)
        {
            chatbot.SendMessage("Hoi, jongen van 21 hier. Woon in Eindhoven, zoek een leuk meisje. En jij?");
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
            this.MinimumSize = this.Size;
            this.MaximumSize = this.Size;
        }

        private void btn_AddBot_Click(object sender, EventArgs e)
        {
            bm.AddBot();
        }
    }
}
