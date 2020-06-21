using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Battle
{
    public class QTE : MonoBehaviour
    {
        public bool IsGray;
        /// <summary>
        /// 0 Left
        /// 1 Up
        /// 2 Down
        /// 3 Right
        /// </summary>
        public GameObject[] Arrows = new GameObject[4], ColorArrows = new GameObject[4];
        public Timer timer;
        [Header("按键成功恢复的时间")]
        public float RecoverTime;
        /// <summary>
        /// 当前箭头的列表
        /// </summary>
        private List<GameObject> ArrowList = new List<GameObject>();
        private string TempInput = "";
        private GameObject CurrentArrow;
        [Header("失败时受到的伤害")]
        public int WrongDamage;
        /// <summary>
        /// 检测输入
        /// </summary>
        private Func<string> GetInput;
        /// <summary>
        /// 检测是否正确
        /// </summary>
        private Func<bool> CheckInput;
        BasePlayer player;
        public int NeededTimes;
        public GameObject ColorCorona;
        private Action UpdateMethod;
        private void Awake()
        {
            player = BasePlayer.Player;
            UpdateMethod = NormalInput;
            for (int i = 0; i < 4; i++)
                CreateNotes();
            Lay();
            CurrentArrow = ArrowList[0];
            timer.gameObject.SetActive(true);
            timer.OnTimeOut = Mistake;
            if (IsGray)
            {
                GetInput = GrayInput;
                CheckInput = GrayCheck;
            }
            else
            {
                GetInput = ColorInput;
                CheckInput = ColorCheck;
            }
        }
        private void Update()
        {
            UpdateMethod();
        }
        void CreateNotes()
        {
            if (IsGray)
            {
                GameObject temp = Instantiate(Arrows[UnityEngine.Random.Range(0, 4)], gameObject.transform);
                ArrowList.Add(temp);
            }
            else
            {
                GameObject temp = Instantiate(ColorArrows[UnityEngine.Random.Range(0, 4)], gameObject.transform);
                ArrowList.Add(temp);
            }
        }
        private string GrayInput()
        {
            if (Input.GetKeyDown(KeyCode.W))
                return "Up(Clone)";
            if (Input.GetKeyDown(KeyCode.S))
                return "Down(Clone)";
            if (Input.GetKeyDown(KeyCode.A))
                return "Left(Clone)";
            if (Input.GetKeyDown(KeyCode.D))
                return "Right(Clone)";
            return "";
        }
        private string ColorInput()
        {
            if (Input.GetKeyDown(KeyCode.W))
                return "Up_C(Clone)";
            if (Input.GetKeyDown(KeyCode.S))
                return "Down_C(Clone)";
            if (Input.GetKeyDown(KeyCode.A))
                return "Left_C(Clone)";
            if (Input.GetKeyDown(KeyCode.D))
                return "Right_C(Clone)";
            return "";
        }
        private bool GrayCheck()
        {
            return TempInput == CurrentArrow.name;
        }
        private bool ColorCheck()
        {
            return player.colorStatus == CurrentArrow.GetComponent<ArrowColor>().ColorIndex && TempInput == CurrentArrow.name;
        }
        void NormalInput()
        {
            if (Input.GetKey(KeyCode.Space))
            {
                Time.timeScale = 0.1f;
                ColorCorona.SetActive(true);
                UpdateMethod = AfterSpace;
                return;
            }
            TempInput = GetInput();
            if (TempInput != "")
            {
                if (CheckInput())
                    Clear();
                else
                { Mistake(); }
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
        }
        void ReturnNormal()
        {
            ColorCorona.SetActive(false);
            UpdateMethod = NormalInput;
            Time.timeScale = 1;
        }
        /// <summary>
        /// 排版
        /// </summary>
        void Lay()
        {
            foreach (GameObject arrow in ArrowList)
            {
                arrow.transform.position = new Vector3(ArrowList.IndexOf(arrow) * 3, 0, 0) + transform.position;
            }
        }
        private void Clear()
        {
            timer.CurrentTime += RecoverTime;
            GameObject temp = ArrowList[0];
            ArrowList.Remove(temp);
            Destroy(temp);
            if (ArrowList.Count == 0)
            {
                if (--NeededTimes < 0)
                    PlayerMgr.playerMgr.ExitQTE();
                for (int i = 0; i < 4; i++)
                    CreateNotes();
            }
            Lay();
            CurrentArrow = ArrowList[0];
        }
        /// <summary>
        /// 失误时调用
        /// </summary>
        private void Mistake()
        {
            for (int i = 0, k = ArrowList.Count; i < k; i++)
            {
                GameObject temp = ArrowList[0];
                ArrowList.RemoveAt(0);
                Destroy(temp);
            }
            for (int i = 0; i < 4; i++)
                CreateNotes();
            CurrentArrow = ArrowList[0];
            Lay();
            player.Hurt(2);
            timer.CurrentTime = timer.WholeTime;
            timer.IsOut = false;
        }
        private void OnDisable()
        {
            if (timer != null)
                timer.gameObject.SetActive(false);
        }
    }
}