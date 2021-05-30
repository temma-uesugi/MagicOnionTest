using System;
using App.Shared;
using MagicOnion.Server.Hubs;
using System.Threading.Tasks;
using App.ServerOnClient;

namespace App.Server.Chat
{
    
    /// <summary>
    /// ChatStreamingの設定
    /// </summary>
    public class ChatStreaming : StreamingHubBase<IChatStreaming, IChatStreamingReceiver>, IChatStreaming
    {

        private int _roomUserId;
        private IGroup _group;
        private Room _room;
        
        /// <summary>
        /// 接続時に呼ばれる
        /// </summary>
        protected override ValueTask OnConnecting()
        {
            Console.WriteLine("OnConnecting");
            return CompletedTask;
        }

        /// <summary>
        /// 切断時に呼ばれる
        /// </summary>
        protected override ValueTask OnDisconnected()
        {
            
            Console.WriteLine("OnDisconnected");
            //MagicOnion.Server.HubsのGroupからぬける
            _group.RemoveAsync(Context);
            
            //退室メッセージをブロードキャスト
            _room.ReceiveLeaveMessage(_roomUserId);
            return CompletedTask;
        }

        /// <summary>
        /// Roomに入室
        /// </summary>
        public async Task JoinRoomAsync(string roomId, string name)
        {
            Console.WriteLine($"JoinRoomAsync {name}");
            //MagicOnion.Server.HubsのGroupにRoomIdを指定して入室する
            _group = await Group.AddAsync(roomId);
            
            //Roomを取得 StreamingReceiverを渡す
            _room = Room.GetOrCreateRoom(roomId, _group.CreateBroadcaster<IChatStreamingReceiver>());
            
            //RoomにJoin
            _roomUserId = await _room.ReceiveJoinMessageAsync(name);
            
        }

        /// <summary>
        /// メッセージの送信
        /// </summary>
        public async Task SendMessageAsync(string message)
        {
            Console.WriteLine($"SendMessageAsync {message}");
            //Room内で処理する
            await _room.ReceiveUserMessageAsync(_roomUserId, message);
        }
        
    }
}