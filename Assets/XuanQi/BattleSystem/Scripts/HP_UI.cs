using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace Battle
{
    public class HP_UI : MonoBehaviour
    {
        private BasePlayer player;
        private int Hp;
        private float MaxHp;
        private Slider slider;
        private void Awake()
        {
            player = BasePlayer.Player;
            slider = GetComponent<Slider>();
            MaxHp = Hp = player.MaxHP;
            player.WhenHpChange += SliderChange;
        }
        public void SliderChange(int n)
        {
            Debug.Log("Recieve!");
            slider.value = (Hp += n) / MaxHp;
        }
        private void OnDisable()
        {
            player.WhenHpChange -= SliderChange;
        }
    }
}