using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
namespace Battle
{
    public  class PlayerMgr : MonoBehaviour
    {
        public static PlayerMgr playerMgr;
        public PlayerController playerController;
        public BasePlayer basePlayer;
        public GameObject QTE , UI;
        public GameObject tutorial ;
        private void Awake()
        {
            playerMgr = this;
            basePlayer.enabled = true;
            playerController.enabled = true;
            tutorial.SetActive(true);
            UI.SetActive(true);
        }
        public void EnterQTE()
        {
            playerController.enabled = false;
            QTE.SetActive(true);
            tutorial.SetActive(true);
        }
        public void ExitQTE()
        {
            QTE.SetActive(false);
            playerController.enabled = true;
            tutorial.SetActive(true);
        }
        public void Dead()
        {
            QTE.SetActive(false);
            playerController.enabled = false;
            basePlayer.enabled = false;
        }
    }
}