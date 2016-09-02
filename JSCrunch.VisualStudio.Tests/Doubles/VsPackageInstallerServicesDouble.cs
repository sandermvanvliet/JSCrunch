using System;
using System.Collections.Generic;
using System.Linq;
using EnvDTE;
using NuGet;
using NuGet.VisualStudio;

namespace JSCrunch.VisualStudio.Tests.Doubles
{
    public class VsPackageInstallerServicesDouble : IVsPackageInstallerServices
    {
        private readonly List<IVsPackageMetadata> _packages = new List<IVsPackageMetadata>();

        public IEnumerable<IVsPackageMetadata> GetInstalledPackages()
        {
            return _packages;
        }

        public bool IsPackageInstalled(Project project, string id)
        {
            return _packages.Any(p => p.Id == id);
        }

        public bool IsPackageInstalledEx(Project project, string id, string versionString)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IVsPackageMetadata> GetInstalledPackages(Project project)
        {
            return _packages;
        }

        public bool IsPackageInstalled(Project project, string id, SemanticVersion version)
        {
            throw new NotImplementedException();
        }

        public void Add(string id, string version, string title, string installPath)
        {
            _packages.Add(new VsPackageMetadata {Id = id, Version = SemanticVersion.Parse(version), Title = title, InstallPath = installPath});
        }

        private class VsPackageMetadata : IVsPackageMetadata
        {
            public string Id { get; set; }
            public SemanticVersion Version { get; set; }
            public string Title { get; set; }
            public string Description { get; }
            public IEnumerable<string> Authors { get; }
            public string InstallPath { get; set; }
            public string VersionString { get; }
        }
    }
}