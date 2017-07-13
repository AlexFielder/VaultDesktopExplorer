using SharpShell.SharpIconOverlayHandler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpShell.Interop;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;

namespace VaultDesktopIcons
{
    [ComVisible(true)]
    public class ReadOnlyVaultDesktopIconHandler : SharpIconOverlayHandler
    {
        protected override bool CanShowOverlay(string path, FILE_ATTRIBUTE attributes)
        {
            try
            {
                //  Get the file attributes.
                var fileAttributes = new FileInfo(path);

                //  Return true if the file is read only, meaning we'll show the overlay.
                return fileAttributes.IsReadOnly;
            }
            catch (Exception)
            {
                return false;
            }
        }

        protected override Icon GetOverlayIcon()
        {
            return Properties.Resources.ReadOnly;
        }

        /// <summary>
        /// Returns the priority in case more than one icon could be overlayed on this file.
        /// 0 = highest
        /// 100 = lowest
        /// there might be a conflict with onedrive here but time will tell.
        /// </summary>
        /// <returns></returns>
        protected override int GetPriority()
        {
            return 90;
        }
    }
}
