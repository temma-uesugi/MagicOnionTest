
namespace App.Shared
{
   
    /// <summary>
    /// ChatストリーミングのReceiver
    /// </summary>
    public interface IChatStreamingReceiver
    {
        
        //メッセージを受け取る
        void OnReceivedMessage(string message);

    }
}