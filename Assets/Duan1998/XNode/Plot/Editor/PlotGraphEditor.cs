using System.Collections;
using System.Collections.Generic;
using Dialogue;
using MPlot;
using UnityEngine;
using XNodeEditor;

namespace MPlotEditor {
	[CustomNodeGraphEditor(typeof(PlotGraph))]
	public class PlotGraphEditor : NodeGraphEditor {
		
		public override string GetNodeMenuName(System.Type type) {
			if (type.Namespace == "MPlot") return base.GetNodeMenuName(type).Replace("Plot/","");
			else return null;
		}
	}
}