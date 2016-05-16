using System;
using System.CodeDom;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace Canwell.OrmLite.MSAccess2003Tests.Utilities
{
    public static class FileUtilities
    {

        public static void DeleteFileThenProcessesGetFree(string path, int interval = 1000)
        {
            if (File.Exists(path))
            {
                var exists = true;

                while (exists)
                {
                    try
                    {
                        File.Delete(path);
                        exists = false;
                        Thread.Sleep(new TimeSpan(0,0,0,0,interval));
                    }
                    catch (Exception)
                    {
                        //can not delete at this time
                    }
                }
            }
        }
    }
}
