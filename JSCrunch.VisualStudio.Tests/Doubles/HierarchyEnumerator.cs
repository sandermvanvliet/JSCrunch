using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell.Interop;

namespace JSCrunch.VisualStudio.Tests.Doubles
{
    public class HierarchyEnumerator : IEnumHierarchies
    {
        private readonly IEnumerable<IVsHierarchy> _projects;
        private int pointer = 0;

        private HierarchyEnumerator(IEnumerable<IVsHierarchy> projects)
        {
            _projects = projects;
        }

        public static IEnumHierarchies For(IEnumerable<IVsHierarchy> projects)
        {
            return new HierarchyEnumerator(projects);
        }

        public int Next(uint celt, IVsHierarchy[] rgelt, out uint pceltFetched)
        {
            if (pointer >= _projects.Count())
            {
                pceltFetched = 0;

                return VSConstants.S_FALSE;
            }

            var remainingNumberOfProjects = _projects.Count() - pointer;

            pceltFetched = (uint)Math.Min(remainingNumberOfProjects, celt);

            for (var i = 0; i < pceltFetched; i++)
            {
                rgelt[i] = _projects.Skip(pointer).Take(1).Single();
            }

            pointer += (int)pceltFetched;

            return VSConstants.S_OK;
        }

        public int Skip(uint celt)
        {
            return VSConstants.S_OK;
        }

        public int Reset()
        {
            return VSConstants.S_OK;
        }

        public int Clone(out IEnumHierarchies ppenum)
        {
            ppenum = For(_projects);

            return VSConstants.S_OK;
        }
    }
}