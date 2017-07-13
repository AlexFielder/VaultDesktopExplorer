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
using SharpShell.Attributes;

namespace LockedVaultDesktopIconHandler
{
    [ComVisible(true)]
    [COMServerAssociation(SharpShell.Attributes.AssociationType.ClassOfExtension, "*.*")]
    public class LockedVaultDesktopIconHandler : SharpIconOverlayHandler
    {
        protected override bool CanShowOverlay(string path, FILE_ATTRIBUTE attributes)
        {
#if DEBUG
            var result = TestIfFileIsLocked(path);
            var name = Path.GetFileName(path);
            Log(name + " is " + (result ? "locked" : "not locked"));
            return result;
#else
            return TestIfFileIsLocked(path);
#endif
        }

        protected override Icon GetOverlayIcon()
        {
            return Properties.Resources.Locked;
        }

        protected override int GetPriority()
        {
            return 90;
        }

        private static bool TestIfFileIsLocked(string path)
        {
            //  We'll get an exception if the file is locked...

            try
            {
                using (File.Open(path, FileMode.Open, FileAccess.Write, FileShare.None))
                {
                    //  The file is not locked.
                    return false;
                }
            }
            catch (IOException exception)
            {
                //  Get the exception hresult, check for lock exceptions.
                var errorCode = Marshal.GetHRForException(exception) & ((1 << 16) - 1);
                return errorCode == 32 || errorCode == 33;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
