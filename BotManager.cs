using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

namespace ChatBot
{
    public class BotManager
    {
        public delegate void OnChatBotAddedDelegate(BotManager manager, ChatBot chatbot);
        public event OnChatBotAddedDelegate OnChatBotAdded;

        public delegate void OnBeginSearchingDelegate(BotManager manager, ChatBot chatbot);
        public event OnBeginSearchingDelegate OnBeginSearching;

        public delegate void OnBeginChatDelegate(BotManager manager, ChatBot chatbot);
        public event OnBeginChatDelegate OnBeginChat;

        public delegate void OnNewMessageDelegate(BotManager manager, ChatBot chatbot, Message message, int messageCount);
        public event OnNewMessageDelegate OnNewMessage;

        public delegate void OnEndChatDelegate(BotManager manager, ChatBot chatbot);
        public event OnEndChatDelegate OnEndChat;

        public delegate void OnBotManagerBeforeUpdateDelegate(BotManager manager);
        public event OnBotManagerBeforeUpdateDelegate OnBotManagerBeforeUpdate;

        public delegate void OnBotManagerAfterUpdateDelegate(BotManager manager);
        public event OnBotManagerAfterUpdateDelegate OnBotManagerAfterUpdate;

        private List<ChatBot> bots;
        private Timer timer;
        private ChatBot searching;

        public BotManager()
        {
            bots = new List<ChatBot>();

            timer = new Timer();
            timer.Tick += Timer_Tick;
            timer.Interval = 500;
            timer.Start();

            searching = null;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if(OnBotManagerBeforeUpdate != null)
            {
                OnBotManagerBeforeUpdate(this);
            }

            foreach(ChatBot bot in bots)
            {
                bot.Process();
                if (searching == null && bot.CanStartSearching())
                {
                    searching = bot;
                    bot.StartNextChat();
                }
            }

            if (OnBotManagerAfterUpdate != null)
            {
                OnBotManagerAfterUpdate(this);
            }
        }

        public ChatBot AddBot()
        {
            ChatBot bot = new ChatBot(bots.Count);

            bot.OnBeginSearching += Bot_OnBeginSearching;
            bot.OnBeginChat += Bot_OnBeginChat;
            bot.OnNewMessage += Bot_OnNewMessage;
            bot.OnEndChat += Bot_OnEndChat;

            bots.Add(bot);

            if(OnChatBotAdded != null)
            {
                OnChatBotAdded(this, bot);
            }
            return bot;
        }

        private void Bot_OnEndChat(ChatBot chatbot)
        {
            if (OnEndChat != null)
            {
                OnEndChat(this, chatbot);
            }
        }

        private void Bot_OnNewMessage(ChatBot chatbot, Message message, int messageCount)
        {
            if (OnNewMessage != null)
            {
                OnNewMessage(this, chatbot, message, messageCount);
            }
        }

        private void Bot_OnBeginChat(ChatBot chatbot)
        {
            if(chatbot == searching)
            {
                searching = null;
            }

            if (OnBeginChat != null)
            {
                OnBeginChat(this, chatbot);
            }
        }

        private void Bot_OnBeginSearching(ChatBot chatbot)
        {
            if (OnBeginSearching != null)
            {
                OnBeginSearching(this, chatbot);
            }
        }

        public List<ChatBot> GetBots()
        {
            return bots;
        }
    }
}
