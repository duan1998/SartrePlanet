using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.PlayerLoop;

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
        public Vector3 HalfBoxSize;
        public GameObject ColorCorona;
        [Header("超绝三挡的能量")]
        private float XMove, YMove;
        /// <summary>
        /// 技能限制
        /// </summary>
        private bool Skill_1_Limit = true, Skill_2_Limit = true;
        private float[] Energy;
        private float MaxEnergy;
        private Action UpdateMethod;
        private void OnEnable()
        {
            player = BasePlayer.Player;
            Energy = player.Energy;
            MaxEnergy = player.MaxEnergy;
            UpdateMethod=NormalInput;
        }
        private void Update()
        {
            UpdateMethod();
        }
        private void FixedUpdate()
        {
            gameObject.transform.position += new Vector3(XMove, YMove, 0) * speed*Time.deltaTime;
        }
        private void NormalInput()
        {
            XMove = Input.GetAxis("Horizontal");
            YMove = Input.GetAxis("Vertical");
            if (Input.GetKey(KeyCode.Space))
            {
                Time.timeScale = 0.02f;
                ColorCorona.SetActive(true);
                UpdateMethod = AfterSpace;
                return;
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
        private void AfterSpace()
        {

            if (Input.GetKeyDown(KeyCode.A))
            {
                player.WhenColorChange(3);
                ReturnNormal(); return;
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                player.WhenColorChange(0);
                ReturnNormal(); return;
            }

            if (Input.GetKeyDown(KeyCode.W))
            { 
                player.WhenColorChange(1);
                ReturnNormal(); return;
            }
            if (Input.GetKeyDown(KeyCode.S))
            { 
                player.WhenColorChange(2);
                ReturnNormal(); return;
            }
            if (Input.GetMouseButtonDown(0))
            {
                if (CheckEnergy(3))
                {
                    Skill3();
                    ReturnNormal(); return;
                }
            }
            if (Input.GetMouseButtonDown(1))
            {
                if (CheckEnergy(4))
                {
                    Skill4();
                    ReturnNormal(); return;
                }
            }
        }
        void ReturnNormal()
        {
            ColorCorona.SetActive(false);
            UpdateMethod = NormalInput;
            Time.timeScale = 1;
        }
        void Slide()
        {
            player.DefendedTime = 2;
            CostEnergy(DexCost, 1);
            Collider[] bullets = Physics.OverlapBox(transform.position,HalfBoxSize,Quaternion.AngleAxis(0,Vector3.zero),1<<8);
            if (bullets.Length!=0) 
            {
                foreach(Collider bullet in bullets)
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
            if(Marvel_UI.activeSelf==true)
            {
                Marvel marvel = Marvel_UI.GetComponentInChildren<Marvel>();
                marvel.LevelUp();
            }
            Marvel_UI.SetActive(true);
        }
    }
}