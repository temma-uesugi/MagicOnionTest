//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading;
//using System.Threading.Tasks;
//using App.Shared;
//
//namespace App.Server.Chat
//{
//    
//    /// <summary>
//    /// Room
//    /// </summary>
//    public class Room
//    {
//        
//        //room毎にBot管理
//        private static readonly Dictionary<string, Room> Rooms =
//            new Dictionary<string, Room>();
//
//
//        /// <summary>
//        /// Roomを作成or取得
//        /// </summary>
//        public static Room GetOrCreateRoom(string roomId, IChatStreamingReceiver broadcaster)
//        {
//            if (!Rooms.ContainsKey(roomId))
//            {
//                Rooms.Add(roomId, new Room(roomId, broadcaster));
//            }
//            return Rooms[roomId];
//        }
//
//        
//        //botのメッセージ
//        private static readonly string[] BotMessages = new string[]
//        {
//            "それな",
//            "りょ",
//            "きゅんです",
//            "そま",
//            "草生える"
//        };
//
//        
//        //そのRoomのroomId
//        private string _roomId;
//        
//        //ユーザを簡易的に名前で管理
//        private Dictionary<int, string> _users = new Dictionary<int, string>();
//        
//        //RoomへのBroadcaster
//        private readonly IChatStreamingReceiver _roomBroadcaster;
//
//        //Room内でシーケンシャルなUserIDを付与するためのもの
//        private int _seqUserId = 0;
//        private int SeqUserId {
//            get
//            {
//                int incId = Interlocked.Increment(ref _seqUserId);
//                return incId;
//            }
//        } 
//        
//
//        /// <summary>
//        /// コンストラクタ
//        /// </summary>
//        private Room(string roomId, IChatStreamingReceiver broadcaster)
//        {
//            _roomId = roomId;
//            _roomBroadcaster = broadcaster;
//        }
//
//       
//        /// <summary>
//        /// 入室メッセージを受け取る
//        /// </summary>
//        public async Task<int> ReceiveJoinMessageAsync(string name)
//        {
//            //userIdを付与してDictionaryに入れて管理
//            int userId = SeqUserId;
//            _users.Add(userId, name);
//            
//            //入室メッセージを送信 
//            _roomBroadcaster.OnReceivedMessage($"{name}が入室しました");
//            
//            //0.5秒後にBotがメッセージを送信
//            await Task.Delay(500);
//            _roomBroadcaster.OnReceivedMessage($"Bot: {name}さん、とりまよろしく");
//
//            return userId;
//        }
//        
//
//        /// <summary>
//        /// ユーザからのメッセージを受け取る
//        /// </summary>
//        public async Task ReceiveUserMessageAsync(int userId, string message)
//        {
//            string userName = _users[userId];
//            _roomBroadcaster.OnReceivedMessage($"{userName}: {message}");
//            
//            //0.5秒後にBotがメッセージを送信
//            await Task.Delay(500);
//            string botMessage = BotMessages
//                .OrderBy(i => Guid.NewGuid())
//                .First();
//            _roomBroadcaster.OnReceivedMessage($"Bot: {botMessage}");
//        }
//
//
//       
//        /// <summary>
//        /// ユーザの離脱メッセージを受け取る
//        /// </summary>
//        public void ReceiveLeaveMessage(int userId)
//        {
//            string userName = _users[userId];
//            _users.Remove(userId);
//            
//            if (_users.Count > 0)
//            {
//                //まだユーザがいれば退室メッセージ
//                _roomBroadcaster.OnReceivedMessage($"{userName}が退室しました");
//            }
//            else
//            {
//                //ユーザ数が0になったらRoomを消滅させる               
//                _users.Clear();
//                Rooms.Remove(_roomId);
//            }
//        }
//        
//    }
//}