// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.VisualStudio.TestPlatform.CrossPlatEngine.Helpers
{
    using System;
    using System.IO;
    using System.Runtime.InteropServices;

    using Microsoft.VisualStudio.TestPlatform.CoreUtilities.Resources;
    using Microsoft.VisualStudio.TestPlatform.CrossPlatEngine.Helpers.Interfaces;
    using Microsoft.VisualStudio.TestPlatform.ObjectModel;
    using Microsoft.VisualStudio.TestPlatform.Utilities.Helpers;
    using Microsoft.VisualStudio.TestPlatform.Utilities.Helpers.Interfaces;

    public class DotnetHostHelper : IDotnetHostHelper
    {
        private readonly IFileHelper fileHelper;

        /// <summary>
        /// Initializes a new instance of the <see cref="DotnetHostHelper"/> class.
        /// </summary>
        public DotnetHostHelper() : this(new FileHelper())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DotnetHostHelper"/> class.
        /// </summary>
        /// <param name="fileHelper">File Helper</param>
        public DotnetHostHelper(IFileHelper fileHelper)
        {
            this.fileHelper = fileHelper;
        }

        /// <inheritdoc />
        public string GetDotnetHostFullPath()
        {
            var dotnetExeName = "dotnet.exe";

#if NET46
            int p = (int) Environment.OSVersion.Platform;
            if ((p == 4) || (p == 6) || (p == 128)) {
                dotnetExeName = "dotnet";
            }
#else
            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                dotnetExeName = "dotnet";
            }
#endif

            var pathString = Environment.GetEnvironmentVariable("PATH");
            foreach (string path in pathString.Split(Path.PathSeparator))
            {
                string exeFullPath = Path.Combine(path.Trim(), dotnetExeName);
                if (this.fileHelper.Exists(exeFullPath))
                {
                    return exeFullPath;
                }
            }

            string errorMessage = string.Format(Resources.NoDotnetExeFound, dotnetExeName);

            EqtTrace.Error(errorMessage);
            throw new FileNotFoundException(errorMessage);
        }
    }
}
