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
        private float CurrentTime;
        private float Speed;
        public float SpeedNormal, SpeedSlow;
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
        private void Awake()
        {
            Speed = SpeedNormal;
            CurrentTime = WholeTime;
            BufferNum = 1;
            text.text = CommentText[BufferNum - 1];
            slider = GetComponent<Slider>();
        }
        private void Update()
        {
                if (CurrentTime > 0)
                {
                  slider.value = CurrentTime / WholeTime;
                  if (slider.value < 0.3)
                    Speed = SpeedSlow;
                  CurrentTime -= Time.deltaTime * Speed;
                }
                else 
                { 
                    WholeMarvel.SetActive(false);
                }
        }
        /// <summary>
        /// 连续升级
        /// </summary>
        public void LevelUp ()
        {
            BufferNum = BufferNum > 2 ?3:BufferNum + 1;
            CurrentTime = WholeTime;
            text.text = CommentText[BufferNum - 1];
        }
    } 
}
