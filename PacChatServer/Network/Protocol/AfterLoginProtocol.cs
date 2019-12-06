﻿using PacChatServer.Network.Packets.AfterLogin.DataPreparing;
using PacChatServer.Network.Packets.AfterLogin.Message;
using PacChatServer.Network.Packets.AfterLogin.Notification;
using PacChatServer.Network.Packets.AfterLogin.Search;
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

            Inbound(0x03, new UserSearchRequest());
            Outbound(0x03, new UserSearchResponse());

            Inbound(0x05, new SendMessageRequest());

            Outbound(0x04, new SendMessageResponse());

            // Conversations
            Inbound(0x06, new ConversationFrIDRequest());
            Outbound(0x06, new ConversationFrIDResponse());

            Inbound(0x02, new SingleConversationFrUserIDRequest());
            Outbound(0x02, new SingleConversationFrUserIDResponse());

            // Notifications
            Outbound(0x07, new UserOnline());
            Outbound(0x08, new UserOffline());

            Inbound(0x11, new AddFriendRequest());
            Outbound(0x10, new ForwardedFriendRequest());
            Inbound(0x13, new ResponseAddFriendRequest());
            Outbound(0x12, new AcceptedFriendResponse());

            // Profile
            Inbound(0x09, new DisplayedProfileRequest());
            Outbound(0x09, new DisplayedProfileResponse());
        }
    }
}
