using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Duan1998
{
    [CreateAssetMenu(menuName = "ScriptableObject/Player Information"), System.Serializable]
    public class PlayerInfo : ScriptableObject
    {
        [SerializeField]
        private int m_exp;
        public int Exp
        {
            get => m_exp;
            set
            {
                m_exp = Mathf.Clamp(value, 0, int.MaxValue);
                UIManager.Instance.UpdateExp(m_exp);
            }
        }

        public void UpdateValue(PlayerInfo playerInfo)
        {
            Exp += playerInfo.Exp;

        }
    }

}
