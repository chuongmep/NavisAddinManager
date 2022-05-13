using System.Windows.Forms;
using Autodesk.Navisworks.Api.Plugins;

namespace Ribbon.Navis.DockPanel
{
    public abstract class DockPaneBase : DockPanePlugin
    {
        /// <summary>
        /// Add a control in navis
        /// </summary>
        /// <returns></returns>
        public abstract override Control CreateControlPane();

        /// <summary>
        /// dispose control from created in navis
        /// </summary>
        /// <param name="control"></param>
        public abstract override void DestroyControlPane(Control control);
    }
}
