using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Battle 
{
    public class Marvel : MonoBehaviour
    {
        /// <summary>
        /// 超绝状态当前时间
        /// </summary>
        public float CurrentTime;
        public float Speed;
        public Text text;
        /// <summary>
        /// 超绝状态的档位
        /// </summary>
        [Range(0,3)]public int BufferNum;
        public float WholeTime;
        public float[] EnergyAbsorb = new float[3];
        public string[] CommentText = new string[3];
        public GameObject WholeMarvel;
        private Slider slider;
        private void Start()
        {
            text.text = CommentText[BufferNum];
            slider = GetComponent<Slider>();
        }
        private void Update()
        {
                if (CurrentTime > 0)
                {
                  slider.value = CurrentTime / WholeTime;
                  CurrentTime -= Time.deltaTime * Speed;
                }
                else 
                { 
                    if(BufferNum>0)
                    {
                        BufferNum--;
                        CurrentTime = WholeTime;
                        BasePlayer.Player.EnergyAbsorb = EnergyAbsorb[BufferNum];
                        text.text = CommentText[BufferNum];
                    }
                    else
                        WholeMarvel.SetActive(false);
                }
        }
    } 
}
