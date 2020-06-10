using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using XNode;

namespace MPlot
{
    [CreateAssetMenu(menuName = "XNode/Plot/Graph", order = 0)]
    public class PlotGraph : NodeGraph
    {
        [HideInInspector]
        public MPlot.Plot current;

        public void Restart()
        {
            //Find the first DialogueNode without any inputs. This is the starting node.
            current = nodes.Find(x => x is Plot && x.Inputs.All(y => !y.IsConnected)) as Plot;
        }

        public Plot NextPlot()
        {
            current.NextPlot();
            return current;
        }
    }

}

