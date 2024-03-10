using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

namespace Com.Playground.Photon
{
    /// <summary>
    /// MonoBehaviourPunCallbacks 을 상속받고 OnEnable(), OnDisable() 을 override 한다면 base 클래스의 OnEnable(), OnDisable() 호출이 필요
    /// (AddCallbackTarget, RemoveCallbackTarget 처리를 해주고 있기 때문)
    /// </summary>
    public class Launcher : MonoBehaviourPunCallbacks
    {
        #region Private Serializable Fields

        /// <summary>
        /// 방 마다 참여할 수 있는 최대 플레이어 수, 방이 가득 찰 경우 새로운 방이 만들어지게 됨.
        /// </summary>
        [Tooltip("방 마다 참여할 수 있는 최대 플레이어 수, 방이 가득 찰 경우 새로운 방이 만들어지게 됩니다.")] 
        [SerializeField]
        private byte maxPlayerPerRoom = 4;
        
        #endregion

        #region Private Fields

        /// <summary>
        /// 해당 클라이언트의 버전 번호로 해당 값으로 유저를 구분합니다.
        /// </summary>
        private string gameVersion = "1";

        #endregion

        #region MonoBehaviour CallBacks

        private void Awake()
        {
            // #Critical
            // 해당 설정이 true인 경우 클라이언트에서 PhotonNetwork.LoadLevel()을 사용할 수 있으며, 같은 룸에 있는 모든 클라이언트가 자동으로 level을 동기화
            PhotonNetwork.AutomaticallySyncScene = true;
        }

        private void Start()
        {
            Connect();
        }

        #endregion

        #region MonoBehaviourPunCallbacks Callbacks

        /// <summary>
        /// 클라이언트가 마스터 서버에 연결되어 매치메이킹 및 기타 작업을 수행할 준비가 되었을 때 호출
        ///  LoadBalancingClient.OpJoinLobby를 통해 로비에 가입하지 않으면 사용 가능한 룸 목록을 사용할 수 없음.
        /// 로비가 없어도 룸에 참여할 수 있고 룸을 만들 수 있음. 이 경우 default 로비가 사용 됨.
        /// </summary>
        public override void OnConnectedToMaster()
        {
            Debug.Log("OnConnectedToMaster");
            PhotonNetwork.JoinRandomRoom();
        }

        /// <summary>
        /// Photon 서버의 연결이 해제된 뒤 호출 (연결에 실패하거나 , 명시적으로 연결을 해제한 경우 모두 해당)
        /// 
        /// </summary>
        /// <param name="cause">연결 해제 원인에 대한 정보</param>
        public override void OnDisconnected(DisconnectCause cause)
        {
            Debug.Log("OnDisconnected");
        }

        /// <summary>
        /// 서버에서 OnJoinRandom이 실패한 경우 호출
        /// 발생 원인으로는 방이 가득 차거나, 존재하지 않을 경우, 다른 사람이 먼저 방을 닫은 경우 등이 있을 수 있음.
        /// 해당 작업은 마스터 서버에만 전송 되며, 마스터 서버에서 방을 찾으면 지정된 게임 서버로 이동하여 게임 서버 참여 작업을 진행하게 됨.
        /// </summary>
        /// <param name="returnCode">서버에서 내려주는 코드</param>
        /// <param name="message">오류 메시지</param>
        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = maxPlayerPerRoom });
        }

        /// <summary>
        /// 성공적으로 룸에 참여했을 때 호출
        /// </summary>
        public override void OnJoinedRoom()
        {
            Debug.Log("OnJoinedRoom");
        }

        #endregion

        #region Public

        /// <summary>
        /// Photon Cloud Network에 연결
        /// 이미 연결된 경우 랜덤 룸 가입을 시도하고, 연결되어있지 않은 경우 해당 애플리케이션 인스턴스를 Photon Cloud Network에 연결
        /// </summary>
        public void Connect()
        {
            // Photon Cloud Network에 연결되어 있는지 확인
            if (PhotonNetwork.IsConnected)
            {
                // #Critical 랜덤룸에 가입을 시도하여 실패할 경우 OnJoinRandomFailed에서 알림을 받고 룸을? 하나 생성
                PhotonNetwork.JoinRandomRoom();
            }
            else
            {
                PhotonNetwork.GameVersion = gameVersion;
                PhotonNetwork.ConnectUsingSettings();
            }
        }

        #endregion
    }
}