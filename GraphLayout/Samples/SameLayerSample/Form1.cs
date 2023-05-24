using Microsoft.Msagl.Drawing;
using Microsoft.Msagl.GraphViewerGdi;
using Microsoft.Msagl.Layout.Layered;
using System.Windows.Forms;

namespace SameLayerSample
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            GViewer gViewer = new GViewer() { Dock = DockStyle.Fill };
            SuspendLayout();
            Controls.Add(gViewer);
            ResumeLayout();
            Graph graph = new Graph();
            var sugiyamaSettings = (SugiyamaLayoutSettings)graph.LayoutAlgorithmSettings;
            sugiyamaSettings.NodeSeparation *= 2;

            graph.AddEdge("Start\n_____________________\nSave as Draft\nCommit", "Draft\n_____________________\nSubmit for Approval\nCommit");
            graph.AddEdge("Start\n_____________________\nSave as Draft\nCommit", "Pending Approval\n_____________________\nReject\nQuery");
            graph.AddEdge("Draft\n_____________________\nSubmit for Approval\nCommit", "Pending Approval\n_____________________\nReject\nQuery");
            graph.AddEdge("Query\n_____________________\nSubmit for Approval\nCancel", "Cancelled\n_____________________\nNo actions required");
            graph.AddEdge("Query\n_____________________\nSubmit for Approval\nCancel", "Pending Approval\n_____________________\nReject\nQuery");
            graph.AddEdge("Pending Approval\n_____________________\nReject\nQuery", "Pending Approval\n_____________________\nReject\nQuery");
            graph.AddEdge("Pending Approval\n_____________________\nReject\nQuery", "Rejected\n_____________________\nNo actions required");
            graph.AddEdge("Query\n_____________________\nSubmit for Approval\nCancel", "Rejected\n_____________________\nNo actions required");
            graph.AddEdge("Committed\n_____________________\nReverse", "Reversed\n_____________________\nNo actions required");

            //graph.AddEdge("A", "B");
            //graph.AddEdge("A", "C");
            //graph.AddEdge("A", "D");

            //graph.LayerConstraints.PinNodesToSameLayer(new[] { 
            //    graph.FindNode("Start\n_____________________\nSave as Draft\nCommit")
            //});

            //graph.LayerConstraints.PinNodesToSameLayer(new[] {
            //    graph.FindNode("Draft\n_____________________\nSubmit for Approval\nCommit")
            //});

            //graph.LayerConstraints.PinNodesToSameLayer(new[] {
            //    graph.FindNode("Pending Approval\n_____________________\nReject\nQuery")
            //});


            graph.LayerConstraints.PinNodesToSameLayer(new[] {
                graph.FindNode("Reversed\n_____________________\nNo actions required"),
                graph.FindNode("Cancelled\n_____________________\nNo actions required")
            });

            graph.LayerConstraints.PinNodesToMinLayer(new[] {
                graph.FindNode("Rejected\n_____________________\nNo actions required"),
                graph.FindNode("Query\n_____________________\nSubmit for Approval\nCancel"),
                graph.FindNode("Committed\n_____________________\nReverse")
            });

            //graph.LayerConstraints.AddSameLayerNeighbors(
            //    graph.FindNode("Start\n_____________________\nSave as Draft\nCommit"),
            //    graph.FindNode("Query\n_____________________\nSubmit for Approval\nCancel"),
            //    graph.FindNode("Committed\n_____________________\nReverse")
            //);

            gViewer.Graph = graph;

        }
    }
}
