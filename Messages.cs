using System.Collections.Generic;
using System.Windows.Forms;

namespace ChatBot
{
    public class Message
    {
        public bool Mine { get; set; }
        public string Text { get; set; }

        public Message(bool mine, string text)
        {
            Mine = mine;
            Text = text;
        }
    }

    public class Messages
    {
        public delegate void OnNewMessageDelegate(Messages sender, Message message, int messageCount);
        public event OnNewMessageDelegate OnNewMessage;

        private List<Message> messages;
        private int currentMessage;

        public Messages()
        {
            messages = new List<Message>();
            currentMessage = 0;
        }

        public int ProcessNewMessages(HtmlElementCollection divs)
        {
            int messagesAdded = 0;
            int oldSize = messages.Count;
            foreach (HtmlElement div in divs)
            {
                if(div.GetAttribute("className").CompareTo("messageblock") == 0)
                {
                    if(++messagesAdded > currentMessage)
                    {
                        currentMessage = messagesAdded;
                        HtmlElementCollection spans = div.GetElementsByTagName("span");
                        Message message = new Message(
                                spans[0].InnerText[0] == 'J',
                                spans[2].InnerText
                            );
                        messages.Add(message);
                        if (OnNewMessage != null)
                        {
                            OnNewMessage(this, message, messages.Count);
                        }
                    }
                }
            }
            return messages.Count - oldSize;
        }

        public void Reset()
        {
            messages.Clear();
            currentMessage = 0;
        }

        public List<Message> GetMessages()
        {
            return messages;
        }
    }
}
