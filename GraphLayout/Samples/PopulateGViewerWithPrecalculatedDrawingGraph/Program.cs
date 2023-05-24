using System;
using System.Windows.Forms;
using Microsoft.Msagl.Core.Geometry;
using Microsoft.Msagl.Core.Geometry.Curves;
using Microsoft.Msagl.Core.Routing;
using Microsoft.Msagl.GraphViewerGdi;

namespace PopulateGViwerWithPrecalculatedDrawingGraph
{
    class Program
    {

        private static void SetNodeBoundary(Microsoft.Msagl.Drawing.Node drawingNode) {
            var font = new System.Drawing.Font(drawingNode.Label.FontName, (float)drawingNode.Label.FontSize);
            StringMeasure.MeasureWithFont(drawingNode.LabelText, font, out double width, out double height);
            drawingNode.Label.GeometryLabel = new Microsoft.Msagl.Core.Layout.Label();
            drawingNode.Label.GeometryLabel.Width = width;
            drawingNode.Label.GeometryLabel.Height = width;
            drawingNode.Label.Width = width;
            drawingNode.Label.Height = height;
            int r = drawingNode.Attr.LabelMargin;
            drawingNode.GeometryNode.BoundaryCurve = CurveFactory.CreateRectangleWithRoundedCorners(width + r * 2, height + r * 2, r, r, new Point());
        }
        static void Main(string[] args)
        {
            var gviewer = new GViewer();
            var form = TestFormForGViewer.FormStuff.CreateOrAttachForm(gviewer, null);
            gviewer.NeedToCalculateLayout = false;
            var drawingGraph = new Microsoft.Msagl.Drawing.Graph();
            var a = drawingGraph.AddNode("Start\n_____________________\nSave as Draft\nCommit");
            var b = drawingGraph.AddNode("Draft\n_____________________\nSubmit for Approval\nCommit");
            var c = drawingGraph.AddNode("Query\n_____________________\nSubmit for Approval\nCancel");
            var d = drawingGraph.AddNode("Committed\n_____________________\nReverse");
            var e = drawingGraph.AddNode("Rejected\n_____________________\nNo actions required");
            var f = drawingGraph.AddNode("Cancelled\n_____________________\nNo actions required");
            var g = drawingGraph.AddNode("Reversed\n_____________________\nNo actions required");
            var h = drawingGraph.AddNode("Pending Approval\n_____________________\nReject\nQuery");

            drawingGraph.AddEdge("Start\n_____________________\nSave as Draft\nCommit", "Draft\n_____________________\nSubmit for Approval\nCommit");
            drawingGraph.AddEdge("Start\n_____________________\nSave as Draft\nCommit", "Pending Approval\n_____________________\nReject\nQuery");
            drawingGraph.AddEdge("Draft\n_____________________\nSubmit for Approval\nCommit", "Pending Approval\n_____________________\nReject\nQuery");
            drawingGraph.AddEdge("Query\n_____________________\nSubmit for Approval\nCancel", "Cancelled\n_____________________\nNo actions required");
            drawingGraph.AddEdge("Query\n_____________________\nSubmit for Approval\nCancel", "Pending Approval\n_____________________\nReject\nQuery");
            drawingGraph.AddEdge("Pending Approval\n_____________________\nReject\nQuery", "Pending Approval\n_____________________\nReject\nQuery");
            drawingGraph.AddEdge("Pending Approval\n_____________________\nReject\nQuery", "Rejected\n_____________________\nNo actions required");
            drawingGraph.AddEdge("Query\n_____________________\nSubmit for Approval\nCancel", "Rejected\n_____________________\nNo actions required");
            drawingGraph.AddEdge("Committed\n_____________________\nReverse", "Reversed\n_____________________\nNo actions required");

            var geometryGraph = drawingGraph.CreateGeometryGraph();
            SetNodeBoundary(a);
            SetNodeBoundary(b);
            SetNodeBoundary(c);
            SetNodeBoundary(d);
            SetNodeBoundary(e);
            SetNodeBoundary(f);
            SetNodeBoundary(g);
            SetNodeBoundary(h);

            // leave node "a" at (0, 0), but move "b" to a new spot
            b.GeometryNode.Center = new Point(50, 50);
            var router = new Microsoft.Msagl.Routing.SplineRouter(geometryGraph, new EdgeRoutingSettings());
            router.Run();
            geometryGraph.UpdateBoundingBox();
            gviewer.Graph = drawingGraph;
            Application.Run(form);
        }
    }
}
