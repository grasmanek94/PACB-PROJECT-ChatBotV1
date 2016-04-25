using System.Windows.Forms;

namespace ChatBot
{
    public class ChatBot
    {
        public delegate void OnBeginSearchingDelegate(ChatBot chatbot);
        public event OnBeginSearchingDelegate OnBeginSearching;

        public delegate void OnBeginChatDelegate(ChatBot chatbot);
        public event OnBeginChatDelegate OnBeginChat;

        public delegate void OnNewMessageDelegate(ChatBot chatbot, Message message, int messageCount);
        public event OnNewMessageDelegate OnNewMessage;

        public delegate void OnEndChatDelegate(ChatBot chatbot);
        public event OnEndChatDelegate OnEndChat;

        public enum State
        {
            Unknown,
            Loading,
            Searching,
            Chatting,
            Stopped
        };

        private WebBrowser browser;
        private bool loaded;
        private bool chatBegun;
        private Messages messages;
        private bool ignoreUntilEnd;

        private HtmlElement logwrapper { get { return browser.Document.GetElementById("logwrapper"); } }
        private HtmlElement chatStartStopButton { get { return browser.Document.GetElementById("chatStartStopButton"); } }
        private HtmlElement chatMessageInput { get { return browser.Document.GetElementById("chatMessageInput"); } }
        public int Identifier { get; private set; }

        private State lastKnownState;

        public ChatBot(int id = 0)
        {
            Identifier = id;

            browser = new WebBrowser();
            loaded = false;
            chatBegun = false;
            ignoreUntilEnd = false;
            messages = new Messages();
            lastKnownState = State.Unknown;

            browser.DocumentCompleted += DocumentCompleted;
            messages.OnNewMessage += OnNewMessageHandler;
            browser.Navigate("http://www.praatanoniem.nl/");
            browser.Visible = true;

            browser.ScriptErrorsSuppressed = true;
        }

        private void OnNewMessageHandler(Messages sender, Message message, int messageCount)
        {
            //and then just pass on to the creator delegate:
            if (OnNewMessage != null)
            {
                OnNewMessage(this, message, messageCount);
            }
        }

        public void Process()
        {
            State newState = GetState();
            if(newState != lastKnownState)
            {
                lastKnownState = newState;
                switch (lastKnownState)
                {
                    case State.Searching:
                        if (OnBeginSearching != null)
                        {
                            OnBeginSearching(this);
                        }
                        break;
                    case State.Chatting:
                        ignoreUntilEnd = false;
                        messages.Reset();
                        if (OnBeginChat != null)
                        {
                            OnBeginChat(this);
                        }
                        break;
                    case State.Stopped:
                        if (OnEndChat != null)
                        {
                            OnEndChat(this);
                        }
                        break;
                }
            }

            if (lastKnownState == State.Chatting && !ignoreUntilEnd)
            {
                messages.ProcessNewMessages(logwrapper.GetElementsByTagName("div"));
            }
        }

        public bool StartNextChat()
        {
            if (lastKnownState != State.Stopped)
            {
                return false;
            }

            if (chatBegun == false)
            {
                chatBegun = true;
                browser.Document.GetElementById("rosetta-init").InvokeMember("click");
            }
            else
            {
                chatStartStopButton.InvokeMember("click");
            }
            return true;
        }

        private bool IsSearching()
        {
            var collection = logwrapper.GetElementsByTagName("div");
            if(collection.Count == 1)
            {
                return collection[0].InnerText[0] == 'W';
            }
            return false;
        }

        private bool IsChatting()
        {
            return chatMessageInput.GetAttribute("disabled")[0] == 'F';
        }

        private bool IsStopped()
        {
            return browser.Document.GetElementById("rosetta-chat-end-banner") != null;
        }

        private void DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            loaded = true;
        }

        public void SendMessage(string message)
        {
            if (message != null)
            {
                chatMessageInput.InnerText = message;
                browser.Document.GetElementById("chatSendButton").InvokeMember("click");
            }
        }

        private State GetState()
        {
            if (!loaded)
            {
                return State.Loading;
            }

            if (IsStopped() || chatBegun == false)
            {
                return State.Stopped;
            }

            if (IsSearching())
            {
                return State.Searching;
            }

            if (IsChatting())
            {
                return State.Chatting;
            }

            return State.Unknown;
        }

        public State GetLastKnownState()
        {
            return lastKnownState;
        }

        public WebBrowser GetControl()
        {
            return browser;
        }

        public bool CountForSearching()
        {
            return lastKnownState != State.Chatting;
        }

        public bool CanStartSearching()
        {
            return lastKnownState == State.Stopped;
        }

        public void IgnoreUntilEnd()
        {
            ignoreUntilEnd = true;
        }

        public override string ToString()
        {
            return Identifier.ToString();
        }
    }
}
