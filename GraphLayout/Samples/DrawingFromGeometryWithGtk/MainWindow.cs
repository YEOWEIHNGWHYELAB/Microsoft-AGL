using System;

using CommonDrawingUtilsForSamples;

using Gtk;

using Microsoft.Msagl.Core.Geometry.Curves;
using Microsoft.Msagl.Core.Layout;
using Microsoft.Msagl.Core.Routing;
using Microsoft.Msagl.Layout.Layered;

using UI = Gtk.Builder.ObjectAttribute;

namespace DrawingFromGeometryWithGtk
{
    class MainWindow : Window
    {
        [UI] private DrawingArea drawingArea;
        private GeometryGraph graph;
        private double width, height;

        public MainWindow() : this(new Builder("MainWindow.glade")) { }

        private MainWindow(Builder builder) : base(builder.GetRawOwnedObject("MainWindow"))
        {
            builder.Autoconnect(this);

            graph = CreateAndLayoutGraph();

            drawingArea.Drawn += Graph_Drawn;
            drawingArea.SizeAllocated += Graph_SizeAllocated;
            DeleteEvent += Window_DeleteEvent;
        }

        private void Graph_SizeAllocated(object sender, SizeAllocatedArgs args) {
            width = args.Allocation.Width;
            height = args.Allocation.Width;
            RecalculateLayout();
            drawingArea.QueueResize();
        }

        private void Graph_Drawn(object sender, DrawnArgs args) {
            var context = args.Cr;
            DrawingUtilsForSamples.DrawFromGraph(new Cairo.Rectangle(0, 0, width, height), graph, args.Cr);
        }

        private void Window_DeleteEvent(object sender, DeleteEventArgs a)
        {
            Application.Quit();
        }

        static internal GeometryGraph CreateAndLayoutGraph() {
            GeometryGraph graph = new GeometryGraph();

            double width = 140;
            double height = 20;

            string[] nodeArr = new string[] {
                "Start\n_____________________\nSave as Draft\nCommit",
                "Draft\n_____________________\nSubmit for Approval\nCommit",
                "Pending Approval\n_____________________\nReject\nQuery",
                "Query\n_____________________\nSubmit for Approval\nCancel",
                "Committed\n_____________________\nReverse"
            };

            foreach (string currStr in nodeArr)
            {
                int numLine = currStr.Split("\n").Length;

                DrawingUtilsForSamples.AddNode(currStr, graph, width, height * numLine);
            }

            graph.Edges.Add(new Edge(graph.FindNodeByUserData("Start\n_____________________\nSave as Draft\nCommit"), graph.FindNodeByUserData("Draft\n_____________________\nSubmit for Approval\nCommit")));
            graph.Edges.Add(new Edge(graph.FindNodeByUserData("Pending Approval\\n_____________________\\nReject\\nQuery\""), graph.FindNodeByUserData("Draft\n_____________________\nSubmit for Approval\nCommit")));

            var settings = new SugiyamaLayoutSettings {
                Transformation = PlaneTransformation.Rotation(Math.PI/2),
                EdgeRoutingSettings = {EdgeRoutingMode = EdgeRoutingMode.Spline}
            };
            var layout = new LayeredLayout(graph, settings);
            layout.Run();
            return graph;
        }

        internal void RecalculateLayout() {
            var settings = new SugiyamaLayoutSettings {
                Transformation = PlaneTransformation.Rotation(Math.PI / 2),
                EdgeRoutingSettings = {EdgeRoutingMode = EdgeRoutingMode.Spline}
            };
            var layout = new LayeredLayout(graph, settings);
            layout.Run();
        }
    }
}
