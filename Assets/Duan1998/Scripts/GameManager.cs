using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Duan1998
{
    public class GameManager : Singleton<GameManager>
    {
        private CameraCtrl m_cameraCtrl;

        public bool IsCanCtrlPlayer
        {
            get =>!(m_cameraCtrl.bZooming || UIManager.Instance.BShowDialog);
        }
        protected override void Awake()
        {
            base.Awake();
            m_cameraCtrl = Camera.main.GetComponent<CameraCtrl>();
        }
    }

}
