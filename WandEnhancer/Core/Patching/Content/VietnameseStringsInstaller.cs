using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using WandEnhancer.Models;
using WandEnhancer.Utils;

namespace WandEnhancer.Core.Patching.Content
{
    /// <summary>
    /// Drops the embedded vi-VN strings file into the extracted static/strings folder so the
    /// repack step packs it into app.asar next to the shipped locales.
    /// </summary>
    internal class VietnameseStringsInstaller
    {
        private const string EmbeddedVietnameseStringsResource = "strings/vi-VN.json";
        private const string StringsDirectoryName = "static";
        private const string LocaleDirectoryName = "strings";
        private const string VietnameseFileName = "vi-VN.json";

        private readonly string _unpackedPath;
        private readonly Action<string, ELogType> _logger;

        internal VietnameseStringsInstaller(string unpackedPath, Action<string, ELogType> logger)
        {
            _unpackedPath = unpackedPath;
            _logger = logger;
        }

        internal void Install(IReadOnlyCollection<EPatchType> patchTypes)
        {
            if (!patchTypes.Contains(EPatchType.VietnameseLocale))
            {
                return;
            }

            string targetDirectory = Path.Combine(_unpackedPath, StringsDirectoryName, LocaleDirectoryName);
            Directory.CreateDirectory(targetDirectory);

            string targetPath = Path.Combine(targetDirectory, VietnameseFileName);
            using (Stream resource = Assembly.GetExecutingAssembly().GetManifestResourceStream(EmbeddedVietnameseStringsResource))
            {
                if (resource == null)
                {
                    throw new FileNotFoundException(
                        "[ENHANCER] Embedded Vietnamese strings resource is missing from the patcher build.",
                        EmbeddedVietnameseStringsResource);
                }

                using (var target = File.Create(targetPath))
                {
                    resource.CopyTo(target);
                }
            }

            _logger("[ENHANCER] Vietnamese locale strings installed.", ELogType.Info);
        }
    }
}
