﻿using PacChatServer.Network.Packets.AfterLogin.DataPreparing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacChatServer.Network.Protocol
{
    public class AfterLoginProtocol : PacChatProtocol
    {
        public AfterLoginProtocol() : base("After Login")
        {
            Inbound(0x00, new FriendsListRequest());
            Outbound(0x00, new FriendsListResponse());


            Inbound(0x01, new ShortProfileRequest());
            Outbound(0x01, new ShortProfileResponse());
        }
    }
}
