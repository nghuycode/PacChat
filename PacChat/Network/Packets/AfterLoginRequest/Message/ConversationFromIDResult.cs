﻿using CNetwork;
using CNetwork.Sessions;
using DotNetty.Buffers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PacChat.MessageCore;
using CNetwork.Utils;
using System.Windows;

namespace PacChat.Network.Packets.AfterLoginRequest.Message
{
    public class ConversationFromIDResult : IPacket
    {
        public string ConversationID { get; set; }
        public string ConversationName { get; set; }
        public int StatusCode { get; set; }
        public long LastActive { get; set; }
        public HashSet<string> Members { get; set; } = new HashSet<string>();
        public int LastMessID { get; set; }
        public int LastMediaID { get; set; }
        public int LastAttachmentID { get; set; }
        public int PreviewCode { get; set; }
        public string PreviewContent { get; set; }

        public void Decode(IByteBuffer buffer)
        {
            ConversationID = ByteBufUtils.ReadUTF8(buffer);
            ConversationName = ByteBufUtils.ReadUTF8(buffer);
            StatusCode = buffer.ReadInt();
            LastActive = buffer.ReadLong();

            // Get the number of members in this conversation
            string temp = ByteBufUtils.ReadUTF8(buffer);
            while (temp != "~")
            {
                Members.Add(temp);
                temp = ByteBufUtils.ReadUTF8(buffer);
            }

            LastMessID = buffer.ReadInt();
            LastMediaID = buffer.ReadInt();
            LastAttachmentID = buffer.ReadInt();
            PreviewCode = buffer.ReadInt();
            PreviewContent = ByteBufUtils.ReadUTF8(buffer);
        }

        public IByteBuffer Encode(IByteBuffer byteBuf)
        {
            throw new NotImplementedException();
        }

        public void Handle(ISession session)
        {
            // Load message to UI
            Application.Current.Dispatcher.Invoke(() =>
            {
                var app = MainWindow.chatApplication;

                app.model.Conversations[ConversationID].LastMessID = LastMessID;
                app.model.Conversations[ConversationID].LastMediaID = LastMediaID;
                app.model.Conversations[ConversationID].LastAttachmentID = LastAttachmentID;
                app.model.Conversations[ConversationID].ConversationName = ConversationName;
                app.model.Conversations[ConversationID].Members = Members.ToList();
                ChatPage.Instance.LoadMessages(ConversationID);
                ChatPage.Instance.LoadMedia(ConversationID);

                Console.WriteLine("Conversation load");

                if (LastActive > 0)
                {
                    string active = "Active ";
                    string timeUnit = "minute";

                    if (LastActive > 1) timeUnit += "s";
                    
                    if (LastActive > 59)
                    {
                        timeUnit = "hour";
                        LastActive /= 60;

                        if (LastActive > 1) timeUnit += "s";
                    }

                    active += LastActive + " " + timeUnit + " ago";
                    ChatPage.Instance.LastActive.Text = active;
                    ChatPage.Instance.OnlineDot.Visibility = Visibility.Hidden;
                }
                else
                {
                    ChatPage.Instance.LastActive.Text = "Active Now";
                    ChatPage.Instance.OnlineDot.Visibility = Visibility.Visible;
                }
            });
        }
    }
}
