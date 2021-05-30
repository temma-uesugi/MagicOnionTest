using System;
using App.Shared;

namespace App.Chat
{
    
    
    /// <summary>
    /// ChatStreamingのレシーバー
    /// </summary>
    public class ChatStreamingReceiver : IChatStreamingReceiver
    {

        public Action<string> OnMessage;
        
        /// <summary>
        /// メッセージを受診
        /// </summary>
        public void OnReceivedMessage(string message)
        {
            OnMessage?.Invoke(message);
        }
    }
}