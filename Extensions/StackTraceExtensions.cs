using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace WebmilioCommons.Extensions
{
    public static class StackTraceExtensions
    {
        public static Assembly GetFirstDifferentAssembly(this StackTrace stackTrace)
        {
            StackFrame[] frames = stackTrace.GetFrames();
            Assembly thisCaller = frames[1].GetMethod().DeclaringType.Assembly;

            for (int i = 1; i < frames.Length; i++)
                if (frames[i].GetMethod().DeclaringType.Assembly != thisCaller)
                    return frames[i].GetMethod().DeclaringType.Assembly;

            return null;
        }
    }
}
