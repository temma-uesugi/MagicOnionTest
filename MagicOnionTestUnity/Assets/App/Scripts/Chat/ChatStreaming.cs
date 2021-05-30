using System.Threading.Tasks;
using App.Shared;
using Grpc.Core;
using MagicOnion.Client;
using MagicOnion.Resolvers;
using MessagePack;
using MessagePack.Resolvers;
using MessagePack.Unity;

namespace App.Chat
{
    
    /// <summary>
    /// ChatStreaming管理
    /// </summary>
    public class ChatStreaming
    {

        //コネクション
        private IChatStreaming _conn;
        public IChatStreaming Conn => _conn;

        private Channel _channel;

        //Receiver これでBroadcastする
        private ChatStreamingReceiver _receiver;
        public ChatStreamingReceiver Receiver => _receiver;

        /// <summary>
        /// 切断
        /// </summary>
        public void Disconnect()
        {
            _conn.DisposeAsync();
        }


        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ChatStreaming()
        {
            _receiver = new ChatStreamingReceiver();
        }
        

        /// <summary>
        /// 接続
        /// </summary>
        public void ConnectServer()
        {

            // MagicOnion, MessagePackの設定
            IFormatterResolver resolver = CompositeResolver.Create
            (
                GeneratedResolver.Instance,
                MagicOnionResolver.Instance,
                BuiltinResolver.Instance,
                PrimitiveObjectResolver.Instance,
                MessagePack.Unity.Extension.UnityBlitResolver.Instance,
                UnityResolver.Instance,
                DynamicGenericResolver.Instance,
                StandardResolver.Instance
            ); 
            MessagePackSerializer.DefaultOptions = MessagePackSerializerOptions.Standard.WithResolver(resolver);
             
            _channel = new Channel("localhost", 5000, ChannelCredentials.Insecure);
            _conn = StreamingHubClient.Connect<IChatStreaming, IChatStreamingReceiver>(_channel, _receiver);
        }


        /// <summary>
        /// 内部のサーバ的な振る舞いをするChatStreamingHubに接続
        /// </summary>
        public void ConnectInternal()
        {
            _conn = new InternalChatStreamingHub(_receiver); 
        }
        
    }
}