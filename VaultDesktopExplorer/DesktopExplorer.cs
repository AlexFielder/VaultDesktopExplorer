using Autodesk.Connectivity.WebServices;
using Autodesk.Connectivity.WebServicesTools;
using SharpShell.Attributes;
using SharpShell.SharpContextMenu;
using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Vault = Autodesk.DataManagement.Client.Framework.Vault;

namespace VaultDesktopExplorer
{
    [ComVisible(true)]
    [COMServerAssociation(SharpShell.Attributes.AssociationType.ClassOfExtension, "*.*")]
    public class DesktopExplorer : SharpContextMenu
    {
        public WebServiceManager ServiceManager { get; set; }
        public Vault.Currency.Connections.Connection m_conn = null;
        public DirectoryInfo InventorProjectRootFolder = null;

        protected override bool CanShowMenu()
        {
            return true;
        }

        protected override ContextMenuStrip CreateMenu()
        {
            var menu = new ContextMenuStrip();

            var HelloWorld = new ToolStripMenuItem
            {
                Text = "Check FilePath ≠ Vault Working Folder"
            };

            HelloWorld.Click += (sender, args) => HelloVault();

            menu.Items.Add(HelloWorld);

            return menu;
        }

        private void HelloVault()
        {
            m_conn = Vault.Forms.Library.Login(null);
            ServiceManager = m_conn.WebServiceManager;
            InventorProjectRootFolder = new DirectoryInfo(ServiceManager.DocumentService.GetRequiredWorkingFolderLocation()); //need this for later.
            if(m_conn != null)
            {
                foreach (var filePath in SelectedItemPaths)
                {
                    if(filePath.Contains(InventorProjectRootFolder.ToString()))
                    {
                        MessageBox.Show("File is within the Vault Working Folder");
                    }
                    else
                    {
                        MessageBox.Show("File is NOT within the Vault Working Folder");
                    }
                }
            }
        }
    }
}
