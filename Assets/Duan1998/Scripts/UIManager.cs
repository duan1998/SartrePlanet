using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dialogue;
using UnityEngine.UI;

namespace Duan1998
{
    public class UIManager : Singleton<UIManager>
    {
        public bool BShowDialog
        {
            get => m_dialogUI.gameObject.activeSelf;
        }

        [SerializeField]
        private DialogUI m_dialogUI;
        [SerializeField]
        private Text m_expText;
        public void ShowDialog(DialogueGraph dialog)
        {
            if (dialog == null) return;
            m_dialogUI.gameObject.SetActive(true);
            m_dialogUI.ShowDialog(dialog);
        }

        public void UpdateExp(int exp)
        {
            m_expText.text ="EXP:"+exp.ToString();
        }
    }

}
