using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using OpenGptChat.Views;

namespace OpenGptChat.Services
{
    /// <summary>
    /// 聊天页面服务
    /// </summary>
    public class ChatPageService
    {
        private Dictionary<Guid, ChatPage> pages =
            new Dictionary<Guid, ChatPage>();


        public ChatPageService(
            IServiceProvider services)
        {
            Services = services;
        }

        public IServiceProvider Services { get; }

        public ChatPage GetPage(Guid sessionId)
        {
            if (!pages.TryGetValue(sessionId, out ChatPage? chatPage))
            {
                using (var scope = Services.CreateScope())
                {
                    chatPage = scope.ServiceProvider.GetRequiredService<ChatPage>();
                    chatPage.InitSession(sessionId);

                    pages[sessionId] = chatPage;
                }
            }

            return chatPage;
        }

        public bool RemovePage(Guid sessionId)
        {
            return pages.Remove(sessionId);
        }
    }
}
