using Dialogue;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.Events;
using XNode;

namespace MPlot
{
    [NodeTint("#CCFFDC")]
    public class Plot : DialogueBaseNode
    {
        public Sprite bgSprite;
        [TextArea] public string text;


        public void NextPlot()
        {
            NodePort port = GetOutputPort("output");
            bool flag = false;
            for (int i = 0; i < port.ConnectionCount; i++)
            {
                NodePort connection = port.GetConnection(i);
                (connection.node as DialogueBaseNode).Trigger();
                flag = true;
            }
            if (!flag) (graph as PlotGraph).current = null;
        }
        public override void Trigger()
        {
            (graph as PlotGraph).current = this;
        }
    }
}

