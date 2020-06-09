using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Duan1998
{
    public class CameraCtrl : MonoBehaviour
    {
        [SerializeField]
        private Vector3 m_offset;
        [SerializeField]
        private Transform m_playerTrans;

        private void Update()
        {
            transform.position = m_playerTrans.position + m_offset;
        }
    }

}
