# MagicOnionTest

MagicOniont + Unity でサーバを介してのマルチプレイと、サーバを介さないシングルプレイを同じコードでやってみたというサンプルです。
簡易的なチャットアプリで試しています。目的とその為の実装をわかりやすくする為に、ユーザのメッセージに適当に返信するというBotを実装しています。

#### サーバを介したマルチプレイの場合
ローカルでサーバを立ち上げ、複数のクライアントアプリで実行をした場合は、↓のようになります

<img src="https://github.com/temma-uesugi/MagicOnionTest/blob/image/MalutiPlay.gif?raw=true" height="600">

当然、(ローカル)サーバが動いていないと、クライアントアプリも反応しません。


#### サーバを介さないシングルプレイの場合
「一人部屋」にチェックして入室した際は、↓のような感じになります。

<img src="https://github.com/temma-uesugi/MagicOnionTest/blob/image/singlePlayer.gif?raw=true" height="600">

この場合は、(ローカル)サーバが動いていなくても、大丈夫です。繋ぎに行っていないので。  

どちらもRoomから退出する部分は実装していないので、退出したい場合はアプリを落とします。  

#### 実装としては
リアルタイム通信を受けてのサーバ側の処理を、Unityプロジェクト内に置いて、サーバ側はそれを参照するという感じでやってます。
こうすることで、サーバで動かす際も、Unity内で動かす際も、同じコードを使うことができます。
プロジェクト全体の、ざっくりとしたディレクトリ・ファイル構造は下記です  

```
│   //↓ サーバのSolutionファイル
├── MagicOnionTest.sln
│
│   //↓ Unityのプロジェクトディレクトリ
├── MagicOnionTesttUnityA
│   │   
│   │   //↓ UnityのSolutionファイル
│   ├── MagicOnionTestUnity.sln
│   ├── Assets
│   │   ├── App
│   │   │   └── Scripts
│   │   │       ├── Chat
│   │   │       │   ├── ChatClient.cs
│   │   │       │   ├── ChatStreaming.cs
│   │   │       │   └── ChatStreamingReceiver.cs
│   │   │       │
│   │   │       │   //↓ Unityと共有するサーバ側の処理
│   │   │       ├── ServerOnClient
│   │   │       │   └── Room.cs
│   │   │       │
│   │   │       │   //↓ サーバ側と共有するコード
│   │   │       └── Shared
│   │   │           ├── IChatStreaming.cs
│   │   │           └── IChatStreamingReceiver.cs
│   │   ├── MagicOnionGen
│   │   │   ├── MagicOnionGen.cs
│   │   │   └── MessagePackGen.cs
│   │   └── Plugins
│   └── Library
├── Server
│   │ 
│   │   //↓ サーバのChatプロジェクト 今回のサーバのメイン部分
│   └── Chat
│       ├── Chat.csproj
│       ├── ChatStreaming.cs
│       ├── MagicOnionStartup.cs
│       ├── Program.cs
│       └── Startup.cs
│ 
│ 　//↓ Unity内のコードを参照するプロジェクト(サーバ側の処理)
├── ServerOnClient
│ 
│ 　//↓ Unity内のコードを参照するプロジェクト
└── Shared
   ├── Shared.csproj
   └── Program.cs
```

サーバ側からみたプロジェクト構成は下記のようになってます

<img src="https://github.com/temma-uesugi/MagicOnionTest/blob/image/project.jpg?raw=true" height="400">  

Sharedプロジェクトは、Unity側と共通で使うもので、通信定義や共通の定数や関数なんかを置くイメージです。
ServerOnClientプロジェクトには、通信を受けてのサーバ側の処理を置いておくイメージです。  

サーバに繋ぎに行くのか、Unity内部で処理するのかは切り替えは、下記のようにやっています。
リアルタイム通信のコネクションを管理している、App/Scripts/Chat/ChatStreaming.cs の中で

```
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
        
```

こんな感じで、繋ぎ先を分けています。
ConnectServer の方は、MagicOnion.Client.StreamingHubClientを使っての処理で、サーバ側の

```
    /// <summary>
    /// ChatStreamingの設定
    /// </summary>
    public class ChatStreaming : StreamingHubBase<IChatStreaming, IChatStreamingReceiver>, IChatStreaming
    {
        /// 通信を受けた際の処理...
    }
```
これにつなげています。


ConnectInternal の方はUnity内の、
```
    /// <summary>
    /// Client内部でStreamingHubのように振舞う
    /// </summary>
    public class InternalChatStreamingHub : IChatStreaming
    {
        /// 擬似的に通信を受けたようなことにして処理
    }
```

### 動作確認方法
- マルチプレイ
  - サーバを立ち上げる Server/Chatプロジェクトを起動
  - クライアントを起動
- シングルプレイ
  - クライアントを起動して、「一人部屋」にチェックして入室  
  

### 何の役に立つの？
今回はチャットだったので有用性はないですが、オンライン対戦のゲームとかで、サーバに繋がなくても繋いでも同じ処理を使いまわせるのは、下記のような場合に役立つかな？と思っております。
- 基本マルチプレイだが、シングルプレイにしたい時がある。その際に、サーバのリソースを食わないよう、サーバにはつながない
  - マッチングしない場合
  - 一人で練習モード
- マルチプレイにしたいが、サーバ費をかけられないので、一旦シングルプレイだけを実装
  - シングルプレイで手応えを掴んでから、サーバを建ててマルチプレイモードを実装したい
    - Hole.ioという結構人気のゲームはマルチプレイに見せかけて実はシングルプレイなので、マルチプレイっぽいシングルプレイは需要としてはありかも(※プレイしている人の大半はシングルプレイとは認識していないかも。。。)

ServerOnClientプロジェクトには、通信を受けてのサーバ側の処理を置いておくイメージです。  jう
ServerOnClientプロジェクトには、通信を受けてのサーバ側の処理を置いておくイメージです。  
