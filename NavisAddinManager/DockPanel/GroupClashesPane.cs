using System.Windows.Forms.Integration;
using Autodesk.Navisworks.Api.Plugins;
using NavisAddinManager.View.Control;
using Ribbon.Navis.DockPanel;

namespace NavisAddinManager.DockPanel
{
    [Plugin($"AddinManager.ShowHidePanel", "ChuongMep", DisplayName = "Trace/Debug Output", ToolTip = "Add-in-Manager")]
    [DockPanePlugin(350, 600, AutoScroll = true, MinimumHeight = 300)]
    class GroupClashesPane : DockPaneBase
    {
        public override Control CreateControlPane()
        {
            //create an ElementHost
            ElementHost elementHost = new ElementHost
            {
                //assign the control
                AutoSize = true,
                Child = new LogControl(),
                

            };
            elementHost.CreateControl();
            elementHost.ParentChanged += this.ElementHost_ParentChanged;
            //return the ElementHost
            return elementHost;
        }

        public override void DestroyControlPane(System.Windows.Forms.Control pane)
        {
            pane.Dispose();
        }

        /// <summary>
        /// Resizing DockPane implies resizing WPF-based ElementHost
        /// </summary>
        /// <param name="sender">ElementHost</param>
        /// <param name="e"></param>
        private void ElementHost_ParentChanged(object sender, EventArgs e)
        {
            if (sender is ElementHost elementHost)
            {
                elementHost.Dock = DockStyle.Fill;
            }
        }
    }
}