using Duan1998;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.XR.WSA.Input;
using UnityScript.Scripting.Pipeline;
namespace Battle
{
    public class Tutorial : MonoBehaviour
    {
        public BulletInit producer;
        public GameObject[] UI_Tips;
        public List<string> OrderList = new List<string>();
        private int CurrentEvent = 0;
        private float WaitTime = 0;
        private void Update()
        {
            if (WaitTime > 0)
                WaitTime -= Time.deltaTime;
            else
            {
                if(CurrentEvent == OrderList.Count - 1)
                {
                    Debug.Log("教程结束");
                    gameObject.SetActive(false);
                }
                switch (OrderList[CurrentEvent][0] - '0')
                {
                    case 0:
                        Init(OrderList[CurrentEvent][1]-'0');
                        break;
                    case 1:
                        ShowTips(OrderList[CurrentEvent][1] - '0');
                        break;
                    case 2:
                        ShowDialogue(OrderList[CurrentEvent][1] - '0');
                        break;
                    case 3:
                        EnterQTE(OrderList[CurrentEvent][1] - '0');
                        break;
                }
                CurrentEvent++;
                WaitTime = OrderList[CurrentEvent][2] - '0';
            }
        }
        public void Init(int order)
        {
            switch (order)
            {
                case 0:
                    producer.StartCoroutine(producer.SectorBullet(0, transform.position));
                    break;
                case 1:
                    producer.StartCoroutine(producer.CircleBullet(0, transform.position));
                    break;
                case 2:
                    producer.StartCoroutine(producer.Colorful(producer.SectorBullet, Random.Range(0, 4), transform.position, 1));
                    break;
                case 3:
                    producer.StartCoroutine(producer.Colorful(producer.CircleBullet, Random.Range(0, 4), transform.position, 1));
                    break;
                case 4:
                    producer.StartCoroutine(producer.Colorful(producer.TwirlBullet, Random.Range(0, 4), transform.position, 1));
                    break;
                case 5:
                    producer.StartCoroutine(producer.Colorful(producer.CircleBullet, Random.Range(0, 4), transform.position, 1));
                    break;
            }
            Debug.Log("生成！"+order.ToString());
            return;
        }
        public void ShowTips(int order)
        {
            UI_Tips[order].SetActive(true);
            Debug.Log("显示第" + order.ToString() + "个教程");
            return;
        }
        public void ShowDialogue(int order)
        {
            Debug.Log("显示第" + order.ToString() + "个对话");
            return;
        }
        public void EnterQTE(int order)
        {
            PlayerMgr.playerMgr.EnterQTE();
            return;
        }
    }
}