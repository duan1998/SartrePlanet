using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
namespace Battle
{
    public class BasePlayer : MonoBehaviour
    {
        public static BasePlayer Player;
        public int MaxHP;
        public int Hp;
        private bool IsDead = false;
        public Color[] colors = new Color[4] { Color.red, Color.blue, Color.yellow, Color.black };
        public int colorStatus;

        public float MaxEnergy;
        /// <summary>
        /// 能量条
        /// </summary>
        public float[] Energy = new float[4] { 0, 0, 0, 0 };
        public float EnergyAbsorb = 1;
        /// <summary>
        /// 无敌时间
        /// </summary>
        public float DefendedTime;

        private SpriteRenderer appearance;

        public Action<int, float> WhenEnergyChange;
        public Action<int> WhenHpChange;
        public Action<int> WhenColorChange;
        private void Awake()
        {
            Player = this;
            appearance = GetComponent<SpriteRenderer>();
            appearance.color = colors[colorStatus];
            WhenEnergyChange += EnergyChange;
            WhenHpChange += HpChange;
            WhenColorChange += ColorChange;
        }
        private void Update()
        {
            if (Hp < 0 || IsDead)
                Dead();
            if (DefendedTime > 0)
                DefendedTime -= Time.deltaTime;
        }
        private void HpChange(int changeValue)
        {
            Hp += changeValue;
        }
        private void Dead()
        {
            IsDead = true;
            Time.timeScale = 0;
            PlayerMgr.playerMgr.Dead();
        }

        public void Hurt(int num)
        {
            if (DefendedTime > 0)
                return;
            WhenHpChange(num * (-1));
            for (int i = 0; i < 4; i++)
                WhenEnergyChange(i, -Energy[i]);
            MainCamera.main.StartCoroutine("Shake");
        }
        public void EnergyChange(int color, float value)
        {
            Energy[color] += value*EnergyAbsorb;
            if (Energy[color] > MaxEnergy)
            {
                if (DefendedTime > 0)
                {
                    Energy[color] = MaxEnergy;
                    return;
                }
                Hurt(2);
            }
        }

        private void ColorChange(int ColorNum)
        {
            colorStatus = ColorNum;
            appearance.color = colors[ColorNum];
        }
    }
}