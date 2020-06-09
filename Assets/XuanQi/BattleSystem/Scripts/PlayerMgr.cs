using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Battle
{
    public  class PlayerMgr : MonoBehaviour
    {
        public static PlayerMgr playerMgr;
        public PlayerController playerController;
        public BasePlayer basePlayer;
        public GameObject QTE;
        public BulletInit bulletInit;
        private void Awake()
        {
            playerMgr = this;
            basePlayer.enabled = true;
            playerController.enabled = true;
            bulletInit.enabled = true;
        }
        public void EnterQTE()
        {
            playerController.enabled = false;
            QTE.SetActive(true);
            bulletInit.enabled = false;
        }
        public void ExitQTE()
        {
            QTE.SetActive(false);
            playerController.enabled = true;
        }
        public void Dead()
        {
            QTE.SetActive(false);
            playerController.enabled = false;
            basePlayer.enabled = false;
        }
    }
}