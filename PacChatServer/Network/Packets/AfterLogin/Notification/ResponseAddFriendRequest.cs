﻿using CNetwork;
using CNetwork.Sessions;
using CNetwork.Utils;
using DotNetty.Buffers;
using PacChatServer.Command;
using PacChatServer.Entity;
using PacChatServer.IO.Entity;
using PacChatServer.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacChatServer.Network.Packets.AfterLogin.Notification
{
    public class ResponseAddFriendRequest : IPacket
    {
        public Guid TargetID { get; set; }
        public bool Accepted { get; set; }
        public int NotiPosition { get; set; }

        public void Decode(IByteBuffer buffer)
        {
            TargetID = Guid.Parse(ByteBufUtils.ReadUTF8(buffer));
            Accepted = buffer.ReadBoolean();
            NotiPosition = buffer.ReadInt();
        }

        public IByteBuffer Encode(IByteBuffer byteBuf)
        {
            throw new NotImplementedException();
        }

        public void Handle(ISession session)
        {
            ChatSession chatSession = session as ChatSession;

            if (Accepted)
            {
                string command = "sample mkfriend " +
                    chatSession.Owner.Email + " " +
                    new ChatUserStore().Load(TargetID).Email;

                Command.CommandManager.Instance.ExecuteCommand(ConsoleSender.Instance, command);

                AcceptedFriendResponse packet = new AcceptedFriendResponse();
                packet.UserID = chatSession.Owner.ID.ToString();
                packet.Name = chatSession.Owner.FirstName + " " + chatSession.Owner.LastName;

                ChatUser user;
                if (ChatUserManager.OnlineUsers.TryGetValue(TargetID, out user))
                {
                    user.Send(packet);
                    chatSession.Send(new FinalizeAcceptedFriendRequest());
                }

                // string encNoti = "acfriend:" + chatSession.Owner.ID + ":" +
                //    chatSession.Owner.FirstName + " " + chatSession.Owner.LastName;

                string name = chatSession.Owner.FirstName + " " + chatSession.Owner.LastName;
                string encNoti = NotificationEncoder.Assemble(
                    NotificationPrefixes.AcceptedFriend,
                    chatSession.Owner.ID.ToString(),
                    name, name, "accepted your friend request.",
                    false);

                user = new ChatUserStore().Load(TargetID);
                user.Notifications.Add(encNoti);
                user.Save();

                chatSession.Owner.Notifications.RemoveAt(NotiPosition);
                chatSession.Owner.Save();
            }
        }
    }
}
