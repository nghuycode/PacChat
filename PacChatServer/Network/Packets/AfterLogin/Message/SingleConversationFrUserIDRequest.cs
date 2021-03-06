﻿using CNetwork;
using CNetwork.Sessions;
using CNetwork.Utils;
using DotNetty.Buffers;
using PacChatServer.Entity;
using PacChatServer.IO.Message;
using PacChatServer.MessageCore.Conversation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacChatServer.Network.Packets.AfterLogin.Message
{
    public class SingleConversationFrUserIDRequest : IPacket
    {
        public Guid TargetID { get; set; } = Guid.Empty;

        public void Decode(IByteBuffer buffer)
        {
            try
            {
                TargetID = Guid.Parse(ByteBufUtils.ReadUTF8(buffer));
            }
            catch
            {

            }
        }

        public IByteBuffer Encode(IByteBuffer byteBuf)
        {
            throw new NotImplementedException();
        }

        public void Handle(ISession session)
        {
            if (TargetID.Equals(Guid.Empty)) return;

            ChatUser targetUser = ChatUserManager.LoadUser(TargetID);
            if (targetUser == null) return;

            ChatSession chatSession = session as ChatSession;

            ConversationStore store = new ConversationStore();

            Guid resultID = Guid.NewGuid();
            bool flag = true;

            var CommonConversation = targetUser.ConversationID.Intersect(chatSession.Owner.ConversationID);

            foreach (Guid conversationID in CommonConversation)
            {
                AbstractConversation conversation = store.Load(conversationID);
                if (conversation is SingleConversation)
                {
                    resultID = conversation.ID;
                    flag = false;
                    break;
                }
            }

            if (flag)
            {
                chatSession.Owner.ConversationID.Add(resultID); 
                targetUser.ConversationID.Add(resultID);

                store.Save(new SingleConversation()
                {
                    ID = resultID,
                    Members = new HashSet<Guid>() { chatSession.Owner.ID, targetUser.ID },
                    ConversationName = "~"
                });

                chatSession.Owner.Save();
                targetUser.Save();
            }

            SingleConversationFrUserIDResponse response = new SingleConversationFrUserIDResponse();
            response.UserID = TargetID;
            response.ResponseID = resultID;

            chatSession.Send(response);
        }
    }
}
