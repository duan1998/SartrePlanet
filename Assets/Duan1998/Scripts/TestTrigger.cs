using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Duan1998
{
    public class TestTrigger : MonoBehaviour
    {
        public PlayerInfo influence;
        [SerializeField]
        private float m_rotateSpeed;

        public AudioClip m_audioClip;

     
        private void Update()
        {
            transform.Rotate(Vector3.up*(m_rotateSpeed * Time.deltaTime),Space.World);
        }
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                AudioSource.PlayClipAtPoint(m_audioClip,transform.position);
                Destroy(this.gameObject);
                other.GetComponent<PlayerCtrl>()._PlayerInfo.UpdateValue(influence);
                
            }
        }
    }

}
