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

        [SerializeField]
        private Vector3 m_zoomOffset;
        [SerializeField]
        private float m_zoomSpeed;

        public bool bZooming;

        private void Update()
        {
            if (!bZooming)
            {
                transform.position = m_playerTrans.position + m_offset;
            }

        }

        Vector3 targetPosition;
        public void ZoomIn()
        {
            bZooming = true;
            targetPosition = m_playerTrans.position + m_offset+m_zoomOffset;
            StopCoroutine("ZoomingOut");
            StopCoroutine("ZoomingIn");
            StartCoroutine("ZoomingIn");

        }
        public void ZoomOut()
        {
            targetPosition = m_playerTrans.position + m_offset;
            StopCoroutine("ZoomingOut");
            StopCoroutine("ZoomingIn");
            StartCoroutine("ZoomingOut");
        }
        IEnumerator ZoomingIn()
        {
            while(true)
            {
                if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
                {
                    transform.position = targetPosition;
                    break;
                }
                transform.position = Vector3.Lerp(transform.position, targetPosition, m_zoomSpeed * Time.deltaTime);
                yield return null;
            }
        }
        IEnumerator ZoomingOut()
        {
            while (true)
            {
                if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
                {
                    transform.position = targetPosition;
                    bZooming = false;
                    break;
                }
                transform.position = Vector3.Lerp(transform.position, targetPosition, m_zoomSpeed * Time.deltaTime);
                yield return null;
            }

            
        }
    }

}
