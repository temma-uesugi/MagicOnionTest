using UnityEngine;
using UnityEngine.UI;

namespace App.Chat
{
    
    /// <summary>
    /// Chatクライアント
    /// </summary>
    public class ChatClient : MonoBehaviour
    {

        [SerializeField] private InputField nameInput;
        [SerializeField] private InputField messageInput;
        [SerializeField] private Text messageText;
        [SerializeField] private Button joinBtn;
        [SerializeField] private Button messageBtn;
        [SerializeField] private Toggle isSingleRoom;
        
        
        private ChatStreaming _chatStreaming;
        
        /// <summary>
        /// Start
        /// </summary>
        private void Start()
        {

            //入力欄を活性・非活性
            nameInput.interactable = true;
            messageInput.interactable = false;

            //ボタンを活性・非活性
            joinBtn.interactable = true;
            messageBtn.interactable = false;
            
            _chatStreaming = new ChatStreaming();
            
            _chatStreaming.Receiver.OnMessage += (message) =>
            {
                messageText.text = messageText.text + message + "\n";
            };
        }


        /// <summary>
        /// 入室
        /// </summary>
        public void Join() 
        {
            if (isSingleRoom.isOn)
            {
                //内部のサーバ的振る舞いのクラスつなげる
                _chatStreaming.ConnectInternal();                
            }
            else
            {
                //サーバにつなげる
                _chatStreaming.ConnectServer();                
            }
            _chatStreaming.Conn.JoinRoomAsync("1", nameInput.text);
            
            //入力欄を活性・非活性
            nameInput.interactable = false;
            messageInput.interactable = true;
            
            //ボタンを活性・非活性
            joinBtn.interactable = false;
            messageBtn.interactable = true;
        }


        /// <summary>
        /// メッセージを送信
        /// </summary>
        public void SendMessage()
        {
            _chatStreaming.Conn.SendMessageAsync(messageInput.text);
            messageInput.text = "";
        }

        
        /// <summary>
        /// 終了時
        /// </summary>
        private void OnDestroy()
        {
            _chatStreaming.Disconnect();
        }
    }
}