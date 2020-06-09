using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dialogue;

namespace Duan1998
{
    [CreateAssetMenu(menuName = "ScriptableObject/Mission"),System.Serializable]
    public class Mission : ScriptableObject
    {
        public string m_missionName;
        public string m_missionDesc;
        public DialogueGraph m_introDialog;
        public DialogueGraph m_competeDialog;
        public MissionState m_curState;

        public PlayerInfo m_conditions;
        public PlayerInfo m_influence;
        private void OnEnable()
        {
            m_curState = MissionState.NotAccept;
        }

        public bool Check(PlayerInfo info)
        {
            if (info.Exp >= m_conditions.Exp)
            {
                return true;
            }
            return false;
        }
    }

    public enum MissionState
    {
        NotAccept,
        AcceptedButNotCompete,
        HasGainReward
    }

}
