using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
namespace Battle
{
    public class PlayerController : MonoBehaviour
    {
        /// <summary>
        /// 超绝系统的UI
        /// </summary>
        public GameObject Marvel_UI;
        private BasePlayer player;
        /// <summary>
        /// 闪避消耗
        /// </summary>
        public int DexCost;
        public float speed;

        public Timer timer;
        public float SlideRadius;

        [Header("超绝三挡的能量")]
        private float XMove, YMove;
        /// <summary>
        /// 技能限制
        /// </summary>
        private bool Skill_1_Limit = true, Skill_2_Limit = true;
        private float[] Energy;
        private float MaxEnergy;
        private void OnEnable()
        {
            player = BasePlayer.Player;
            Energy = player.Energy;
            MaxEnergy = player.MaxEnergy;
        }
        private void Update()
        {
            XMove = Input.GetAxis("Horizontal");
            YMove = Input.GetAxis("Vertical");
            if (Input.GetKey(KeyCode.Space))
            {
                if (XMove != 0)
                {
                    if (XMove > 0)
                        player.WhenColorChange(3);
                    else
                        player.WhenColorChange(0);
                    XMove = 0;
                    return;
                }
                if (YMove != 0)
                {
                    if (YMove > 0)
                        player.WhenColorChange(1);
                    else
                        player.WhenColorChange(2);
                    YMove = 0;
                    return;
                }
                if (Input.GetMouseButtonDown(0))
                {
                    if (CheckEnergy(3))
                    {
                        Skill3();
                        return;
                    }
                }
                if (Input.GetMouseButtonDown(1))
                {
                    if (CheckEnergy(4))
                    {
                        Skill4();
                        return;
                    }
                }
            }
            if (Input.GetMouseButtonDown(1))
            {
                if (CheckEnergy(1, DexCost) || !Skill_2_Limit)
                {
                    Slide();
                    return;
                }
            }
            if (Input.GetMouseButtonDown(0))
            {
                if (CheckEnergy(1) || !Skill_1_Limit)
                    Skill1();
            }
        }
        private void FixedUpdate()
        {
            gameObject.transform.position += new Vector3(XMove, YMove, 0) * speed;
        }
        void Slide()
        {
            player.DefendedTime = 2;
            CostEnergy(DexCost, 1);
            Collider2D[] bullets = Physics2D.OverlapCircleAll(transform.position, SlideRadius, 1<<8);
            if (bullets.Length!=0) 
            {
                foreach(Collider2D bullet in bullets)
                {
                    bullet.GetComponent<Bullet>().Absorb();
                    Debug.Log("极限闪避吸收成功");
                }
            }
            transform.position = Vector3.Lerp(transform.position, transform.position + new Vector3(XMove, YMove), 0.4f);
            Debug.Log("翻滚发动!");
            XMove = 0; YMove = 0;
        }
        bool CheckEnergy(int n)
        {
            foreach (float energy in Energy)
            {
                if (energy >= MaxEnergy)
                    n--;
            }
            return n < 0;
        }
        bool CheckEnergy(int n, float value)
        {
            foreach (float energy in Energy)
            {
                if (energy >= value)
                    n--;
            }
            return n <= 0;
        }
        void CostEnergy(float cost, int times)
        {
            for (int i = 0, j = 0; i < 4 && j < times; i++)
            {
                if (Energy[i] >= cost)
                {
                    player.WhenEnergyChange(i, -cost);
                    j++;
                }
            }
        }
        private void Skill1()
        {
            
            CostEnergy(MaxEnergy, 1);
            Debug.Log("技能1发动!");
            CleanBullet();
            Marvelous();
        }
        private void Skill3()
        {
            CostEnergy(MaxEnergy, 3);
            CleanBullet();
            Debug.Log("技能三发动！");
            Marvelous();
        }
        private void Skill4()
        {
            CostEnergy(MaxEnergy, 4);
            CleanBullet();
            player.DefendedTime = 4;
            Debug.LogError("技能四发动！");
            StartCoroutine("BackNormal", 4f);
            Marvelous();
        }
        private IEnumerator BackNormal(float time)
        {
            Skill_1_Limit = true;
            Skill_2_Limit = true;
            yield return new WaitForSeconds(time);
        }
        private void CleanBullet()
        {
            foreach (Bullet bullet in FindObjectsOfType<Bullet>())
            {
                bullet.FadeAway();
            }
        }
        /// <summary>
        /// 超绝状态
        /// </summary>
        private void Marvelous()
        {
            Marvel_UI.SetActive(true);
            Marvel marvel = Marvel_UI.GetComponentInChildren<Marvel>();
            marvel.CurrentTime = marvel.WholeTime;
            marvel.BufferNum = 2;
            player.EnergyAbsorb = 2;
        }
    }
}