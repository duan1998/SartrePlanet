using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;
using Duan1998;

namespace Dialogue {
    [NodeTint("#CCFFCC")]
    public class Chat : DialogueBaseNode {

        public CharacterInfo character;
        public CharacterInfo answerCharacter;
        public CharacterInfo leftCharacter;
        public CharacterInfo rightCharacter;


        [TextArea] public string text;
        [Output(dynamicPortList = true)] public List<Answer> answers = new List<Answer>();


        [System.Serializable] public class Answer {
            public string text;
            public PlayerInfo influence;
        }

        public void AnswerQuestion(int index) {
            NodePort port = null;
            if (answers.Count == 0) {
                port = GetOutputPort("output");
            } else {
                if (answers.Count <= index) return;
                port = GetOutputPort("answers " + index);
            }

            if (port == null) return;
            bool flag = false;
            for (int i = 0; i < port.ConnectionCount; i++) {
                NodePort connection = port.GetConnection(i);
                (connection.node as DialogueBaseNode).Trigger();
                flag = true;
            }
            if (!flag) (graph as DialogueGraph).current = null;
        }

        public override void Trigger() {
            (graph as DialogueGraph).current = this;
        }
    }
}