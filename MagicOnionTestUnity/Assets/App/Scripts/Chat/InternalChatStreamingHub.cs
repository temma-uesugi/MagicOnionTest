using System.Threading.Tasks;
using App.ServerOnClient;
using App.Shared;


namespace App.Chat
{
    
    /// <summary>
    /// Client内部でStreamingHubのように振舞う
    /// </summary>
    public class InternalChatStreamingHub : IChatStreaming
    {
        
        private Room _room;
        private int _roomUserId = 1;
        private ChatStreamingReceiver _receiver;
        
        
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public InternalChatStreamingHub(ChatStreamingReceiver receiver)
        {
            _receiver = receiver;
        }


        /// <summary>
        /// Roomに入室
        /// </summary>
        public async Task JoinRoomAsync(string roomId, string name)
        {
            _room = new Room(roomId, _receiver);
            //RoomにJoin
            _roomUserId = await _room.ReceiveJoinMessageAsync(name);
        }

        
        /// <summary>
        /// メッセージの送信
        /// </summary>
        public async Task SendMessageAsync(string message)
        {
            //Room内で処理する
            await _room.ReceiveUserMessageAsync(_roomUserId, message);
        } 
       
       
        public IChatStreaming FireAndForget()
        {
            return this;
        }
        
        public Task DisposeAsync()
        {
            return null;
        }

        public Task WaitForDisconnect()
        {
            return null;
        }
        
    }
}