using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dialogue;

namespace Duan1998
{
    public class NPCCtrl : MonoBehaviour
    {
        public Mission m_mission;

        public DialogueGraph[] m_dailyDialog;

        public LayerMask m_npcTriggerMask;
        private void OnTriggerStay(Collider other)
        {
            if (other.CompareTag("Player") && Input.GetMouseButtonDown(0))
            {
                PlayerCtrl playerCtrl = other.GetComponent < PlayerCtrl>();
                PlayerInfo playerInfo = playerCtrl._PlayerInfo;
                //检测是否点击到npc
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, float.MaxValue, m_npcTriggerMask, QueryTriggerInteraction.Collide))
                {
                    if (hit.collider.CompareTag("NPC"))
                    {
                        switch(m_mission.m_curState)
                        {
                            case MissionState.NotAccept:
                                UIManager.Instance.ShowDialog(m_mission.m_introDialog);
                                m_mission.m_curState = MissionState.AcceptedButNotCompete;
                                break;
                            case MissionState.AcceptedButNotCompete:
                                if (CheckMeetingConitions(playerInfo))
                                {
                                    m_mission.m_curState = MissionState.HasGainReward;
                                    //给与奖励
                                    UIManager.Instance.ShowDialog(m_mission.m_competeDialog);
                                }
                                else
                                    UIManager.Instance.ShowDialog(GetRandomDialog()); 
                                break;
                            case MissionState.HasGainReward:
                                UIManager.Instance.ShowDialog(GetRandomDialog());
                                break;
                        }
                    }

                }
            }
        }

        bool CheckMeetingConitions(PlayerInfo playerInfo)
        {
            return m_mission.Check(playerInfo);
        }
        DialogueGraph GetRandomDialog()
        {
            if(m_dailyDialog.Length>0)
            {
                int randomValue = UnityEngine.Random.Range(0, m_dailyDialog.Length);
                return m_dailyDialog[randomValue];
            }
            return null;
        }


    }

}
