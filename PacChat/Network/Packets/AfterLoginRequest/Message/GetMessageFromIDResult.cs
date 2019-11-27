﻿using CNetwork;
using CNetwork.Sessions;
using CNetwork.Utils;
using DotNetty.Buffers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacChat.Network.Packets.AfterLoginRequest.Message
{
    public class GetMessageFromIDResult : IPacket
    {
        //List of user seen this message
        public HashSet<string> SeenBy { get; private set; } = new HashSet<string>();
        //if Revoked = true, the message will be hidden to all users.
        public bool Revoked { get; set; } = false;
        //IDs of user who reacted and react id;
        public Dictionary<string, int> Reacts { get; private set; } = new Dictionary<string, int>();

        //Type of message
        /// <summary>
        /// 1: Attachment
        /// 2: Image
        /// 3: Sticker
        /// 4: Text
        /// 5: Video
        /// </summary>
        public int MessType { get; set; }

        // Text content is content of text message (if preview code is equal to 4)
        public string Content { get; set; }

        public void Decode(IByteBuffer buffer)
        {
            // Get ids that have seen this message
            string temp = ByteBufUtils.ReadUTF8(buffer);
            while (temp != "~")
            {
                SeenBy.Add(temp);
                temp = ByteBufUtils.ReadUTF8(buffer);
            }

            Revoked = buffer.ReadInt() == 1 ? true : false;
            
            // Get ids that have reacted this message & their reaction id (int)
            // Please write to buffer in this format: user_id_0-reaction_id_0-user_id_1-reaction_id_1-...
            temp = ByteBufUtils.ReadUTF8(buffer);
            int reactionID = buffer.ReadInt();
            while (temp != "~")
            {
                Reacts.Add(temp, reactionID);
                temp = ByteBufUtils.ReadUTF8(buffer);
                reactionID = buffer.ReadInt();
            }

            // Get message type
            this.MessType = buffer.ReadInt();

            // Get message content
            Content = ByteBufUtils.ReadUTF8(buffer);
        }

        public IByteBuffer Encode(IByteBuffer byteBuf)
        {
            throw new NotImplementedException();
        }

        public void Handle(ISession session)
        {
            // Create a message instance and put information into it
        }
    }
}