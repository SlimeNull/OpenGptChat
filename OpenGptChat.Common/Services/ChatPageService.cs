using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using OpenGptChat.Abstraction;

namespace OpenGptChat.Services
{
    /// <summary>
    /// 聊天页面服务
    /// </summary>
    public class ChatPageService
    {
        private Dictionary<Guid, IChatPage> pages =
            new Dictionary<Guid, IChatPage>();


        public ChatPageService(
            IServiceProvider services)
        {
            Services = services;
        }

        public IServiceProvider Services { get; }

        public IChatPage GetPage(Guid sessionId)
        {
            if (!pages.TryGetValue(sessionId, out IChatPage? chatPage))
            {
                using (var scope = Services.CreateScope())
                {
                    chatPage = scope.ServiceProvider.GetRequiredService<IChatPage>();
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
