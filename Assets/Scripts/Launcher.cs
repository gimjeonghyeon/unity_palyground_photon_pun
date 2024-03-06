using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

namespace Com.Playground.Photon
{
    public class Launcher : MonoBehaviour
    {
        #region Private Serializable Fields

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