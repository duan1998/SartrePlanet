using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Dialogue;

namespace Duan1998
{
    public class DialogUI : MonoBehaviour
    {
        private Image m_leftImage;
        private Image m_rightImage;
        private Text m_charcaterNameText;
        private Text m_dialogContentText;

        private GameObject m_dialogContentObj;
        private GameObject m_dialogChoiceObj;

        private Button[] m_choiceBtns;
        private Text[] m_choiceTexts;

        private DialogueGraph m_curDialog;
        private bool bChoice;

        [SerializeField]
        private PlayerInfo m_playerInfo;

        private PlayerInfo m_curDialogInfluence;
        private bool BCurChoice
        {
            get => m_choiceBtns[0].gameObject.activeInHierarchy;
        }


        private void Awake()
        {
            m_leftImage = transform.Find("LeftImage").GetComponent<Image>();
            m_rightImage = transform.Find("RightImage").GetComponent<Image>();
            m_charcaterNameText = transform.Find("DialogContentBg/CharacterName").GetComponent<Text>();
            m_dialogContentText = transform.Find("DialogContentBg/DialogContent").GetComponent<Text>();
            m_dialogContentObj = transform.Find("DialogContentBg").gameObject;
            m_dialogChoiceObj = transform.Find("DialogChoiceBg").gameObject;
            m_choiceBtns = m_dialogChoiceObj.GetComponentsInChildren<Button>();
            m_choiceTexts = new Text[m_choiceBtns.Length];
            for (int i = 0; i < m_choiceBtns.Length; i++)
            {
                m_choiceTexts[i] = m_choiceBtns[i].GetComponentInChildren<Text>();
            }
        }

        private void Update()
        {
            if (m_curDialog != null && Input.GetKeyDown(KeyCode.Space) && !bNext && !BCurChoice)
            {
                bNext = true;
                if (!bChoice)
                    m_curDialog.AnswerQuestion(0);
            }
        }

        public void ShowDialog(DialogueGraph targetScript,PlayerInfo influence=null)
        {
            if (targetScript == null)
                return;
            Camera.main.GetComponent<CameraCtrl>().ZoomIn();
            bChoice = false;
            m_curDialog = targetScript;
            m_curDialogInfluence = influence;
            m_curDialog.Restart();
            StartCoroutine("Play");

        }

        private void Show(Chat chat)
        {


            if (bChoice)
            {
                m_dialogContentObj.SetActive(false);
                m_dialogChoiceObj.SetActive(true);
                SwitchSprite(chat, true);
                ShowChoice(chat);
                bChoice = false;
            }
            else
            {
                SwitchSprite(chat, false);
                if (chat.answers.Count > 1)
                    bChoice = true;
                m_dialogContentObj.SetActive(true);
                m_dialogChoiceObj.SetActive(false);
                m_dialogContentText.text = chat.text;
                m_charcaterNameText.text = chat.character.m_name;
            }
        }

        private void SwitchSprite(Chat chat, bool answer)
        {
            if (answer)
            {
                chat.answerCharacter.SwitchLightSprite();
                chat.character.SwitchGreySprite();
            }
            else
            {
                chat.character.SwitchLightSprite();
                
                if (chat.character != chat.leftCharacter)
                {
                    chat.leftCharacter?.SwitchGreySprite();
                }   
                else 
                {
                    chat.rightCharacter?.SwitchGreySprite();
                }
                   
            }
            
            m_leftImage.sprite = chat.leftCharacter?.m_curCharacterSprite;
            m_rightImage.sprite = chat.rightCharacter?.m_curCharacterSprite;
        }
        private void ShowChoice(Chat chat)
        {

            int minValue = Mathf.Min(chat.answers.Count, m_choiceBtns.Length);
            for (int i = 0; i < minValue; i++)
            {
                m_choiceBtns[i].gameObject.SetActive(true);
                m_choiceTexts[i].text = chat.answers[i].text;
                int index = i;
                m_choiceBtns[i].onClick.RemoveAllListeners();
                m_choiceBtns[i].onClick.AddListener(() => OnAnswerBtnClick(index,chat.answers[index].influence));
                m_choiceBtns[i].onClick.AddListener(() => { bNext = true; });
            }
            for (int i = minValue; i < m_choiceBtns.Length; i++)
                m_choiceBtns[i].gameObject.SetActive(false);
        }

        void OnAnswerBtnClick(int index,PlayerInfo influence)
        {
            if (influence != null)
                m_playerInfo.UpdateValue(influence);
            m_curDialog.AnswerQuestion(index);
        }


        bool bNext;

        IEnumerator Play()
        {
            while (true)
            {
                if (m_curDialog.current == null)
                {
                    m_curDialog = null;
                    this.gameObject.SetActive(false);
                    if (m_curDialogInfluence != null)
                        m_playerInfo.UpdateValue(m_curDialogInfluence);
                    Camera.main.GetComponent<CameraCtrl>().ZoomOut();
                    break;
                }
                Show(m_curDialog.current);
                bNext = false;
                yield return new WaitUntil(() => bNext);
            }
        }
    }

}
