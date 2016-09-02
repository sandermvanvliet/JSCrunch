using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using EnvDTE;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell.Interop;

namespace JSCrunch.VisualStudio
{
    /// <summary>
    /// VisualStudio extension methods. 
    /// Copied from JsTestAdapter:
    /// https://github.com/MortenHoustonLudvigsen/JsTestAdapter/blob/master/JsTestAdapter/Helpers/VSExtensions.cs
    /// </summary>
    public static class VsExtensions
    {
        private static IVsSolution GetSolution(this IServiceProvider serviceProvider)
        {
            return (IVsSolution) serviceProvider.GetService(typeof(SVsSolution));
        }

        public static IEnumerable<IVsProject> GetLoadedProjects(this IServiceProvider serviceProvider)
        {
            return serviceProvider.GetSolution().GetLoadedProjects();
        }

        public static string GetSolutionDirectory(this IServiceProvider serviceProvider)
        {
            return serviceProvider.GetSolution().GetSolutionDirectory();
        }

        public static bool HasFile(this IServiceProvider serviceProvider, string file)
        {
            return serviceProvider.GetLoadedProjects().Any(p => p.HasFile(file));
        }

        public static IEnumerable<IVsProject> GetLoadedProjects(this IVsSolution solution)
        {
            return solution.EnumerateLoadedProjects(__VSENUMPROJFLAGS.EPF_LOADEDINSOLUTION).OfType<IVsProject>();
        }

        public static string GetProjectName(this IVsProject project)
        {
            return project.GetPropertyValue(__VSHPROPID.VSHPROPID_Name, VSConstants.VSITEMID.Root) as string;
        }

        public static string GetProjectDir(this IVsProject project)
        {
            return project.GetPropertyValue(__VSHPROPID.VSHPROPID_ProjectDir, VSConstants.VSITEMID.Root) as string;
        }

        public static IEnumerable<IVsHierarchy> EnumerateLoadedProjects(this IVsSolution solution,
            __VSENUMPROJFLAGS enumFlags)
        {
            var prjType = Guid.Empty;
            IEnumHierarchies ppHier;

            var hr = solution.GetProjectEnum((uint) enumFlags, ref prjType, out ppHier);
            if (ErrorHandler.Succeeded(hr) && ppHier != null)
            {
                uint fetched = 0;
                var hierarchies = new IVsHierarchy[1];
                while (ppHier.Next(1, hierarchies, out fetched) == VSConstants.S_OK)
                {
                    yield return hierarchies[0];
                }
            }
        }

        public static string GetSolutionDirectory(this IVsSolution solution)
        {
            string solutionDir;
            string solutionFile;
            string userOpsFile;
            if (solution.GetSolutionInfo(out solutionDir, out solutionFile, out userOpsFile) == VSConstants.S_OK)
            {
                return solutionDir;
            }
            return null;
        }

        public static bool HasFile(this IVsSolution solution, string file)
        {
            return solution.GetLoadedProjects().Any(p => p.HasFile(file));
        }

        public static IEnumerable<string> GetProjectItems(this IVsProject project)
        {
            // Each item in VS OM is IVSHierarchy. 
            return GetProjectItems((IVsHierarchy) project, VSConstants.VSITEMID_ROOT);
        }

        public static IEnumerable<string> GetProjectItems(IVsHierarchy hierarchy, uint itemId)
        {
            var pVar = GetPropertyValue(hierarchy, (int) __VSHPROPID.VSHPROPID_FirstChild, itemId);

            var childId = GetItemId(pVar);
            while (childId != VSConstants.VSITEMID_NIL)
            {
                var childPath = GetCanonicalName(childId, hierarchy);
                yield return childPath;

                foreach (var childNodePath in GetProjectItems(hierarchy, childId)) yield return childNodePath;

                pVar = GetPropertyValue(hierarchy, (int) __VSHPROPID.VSHPROPID_NextSibling, childId);
                childId = GetItemId(pVar);
            }
        }

        public static bool HasFile(this IVsProject project, string file)
        {
            int found;
            uint projectItemID;
            var priority = new VSDOCUMENTPRIORITY[1];
            if (ErrorHandler.Succeeded(project.IsDocumentInProject(file, out found, priority, out projectItemID)))
            {
                return found != 0;
            }
            return false;
        }

        public static uint GetItemId(object pvar)
        {
            if (pvar == null) return VSConstants.VSITEMID_NIL;
            if (pvar is int) return (uint) (int) pvar;
            if (pvar is uint) return (uint) pvar;
            if (pvar is short) return (uint) (short) pvar;
            if (pvar is ushort) return (ushort) pvar;
            if (pvar is long) return (uint) (long) pvar;
            return VSConstants.VSITEMID_NIL;
        }

        public static object GetPropertyValue(this IVsProject project, __VSHPROPID propid,
            VSConstants.VSITEMID itemId = VSConstants.VSITEMID.Root)
        {
            return GetPropertyValue((IVsHierarchy) project, propid, itemId);
        }

        public static object GetPropertyValue(this IVsHierarchy vsHierarchy, __VSHPROPID propid,
            VSConstants.VSITEMID itemId = VSConstants.VSITEMID.Root)
        {
            return GetPropertyValue(vsHierarchy, (int) propid, (uint) itemId);
        }

        public static object GetPropertyValue(this IVsHierarchy vsHierarchy, int propid, uint itemId)
        {
            if (itemId == VSConstants.VSITEMID_NIL)
            {
                return null;
            }

            try
            {
                object o;
                ErrorHandler.ThrowOnFailure(vsHierarchy.GetProperty(itemId, propid, out o));

                return o;
            }
            catch (NotImplementedException)
            {
                return null;
            }
            catch (COMException)
            {
                return null;
            }
            catch (ArgumentException)
            {
                return null;
            }
        }

        public static string GetCanonicalName(uint itemId, IVsHierarchy hierarchy)
        {
            var strRet = String.Empty;
            var hr = hierarchy.GetCanonicalName(itemId, out strRet);

            if (hr == VSConstants.E_NOTIMPL)
            {
                // Special case E_NOTIMLP to avoid perf hit to throw an exception.
                return String.Empty;
            }
            try
            {
                ErrorHandler.ThrowOnFailure(hr);
            }
            catch (COMException)
            {
                strRet = String.Empty;
            }

            // This could be in the case of S_OK, S_FALSE, etc.
            return strRet;
        }

        public static Project GetEnvDteProject(this IVsHierarchy projectHierarchy)
        {
            object propertyValue;

            projectHierarchy.GetProperty(VSConstants.VSITEMID_ROOT, (int) __VSHPROPID.VSHPROPID_ExtObject, out propertyValue);

            return propertyValue as Project;
        }
    }
}