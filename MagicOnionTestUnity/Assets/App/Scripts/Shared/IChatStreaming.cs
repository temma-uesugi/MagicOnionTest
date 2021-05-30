using System.Threading.Tasks;
using MagicOnion;

namespace App.Shared
{
    
    /// <summary>
    /// Chatストリーミングのインターフェイス  
    /// </summary>
    public interface IChatStreaming : IStreamingHub<IChatStreaming, IChatStreamingReceiver>
    {

        //ルームに入室
        Task JoinRoomAsync(string roomId, string name);

        //メッセージを送信
        Task SendMessageAsync(string message);
        
    }
}