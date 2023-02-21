using System;
using System.Linq;
using System.Management.Automation;

namespace Microsoft.Dism.PowerShell
{
    [Cmdlet((VerbsData.Initialize), "PSDism")]
    public class InitializePSDismCommand : Cmdlet
    {
        [Parameter(Mandatory = true)]
        public DismLogLevel LogLevel;
        [Parameter(Mandatory = false)]
        public string LogFilePath;
        [Parameter(Mandatory = false)]
        public string ScratchDirectory;

        protected override void BeginProcessing()
        {
            // 'LogFilePath' creates the file (if not null) if doesn't exist?
            DismRunner.ThrowOnInvalidDirectory(this, ScratchDirectory, true);
        }

        protected override void ProcessRecord()
        {
            DismRunner.Invoke(this,
                () => DismApi.Initialize(LogLevel, LogFilePath, ScratchDirectory));
        }
    }

    // Since 'Dispose' is not an approved verb
    [Cmdlet((VerbsCommon.Remove), "PSDism")]
    public class RemovePSDismCommand : Cmdlet
    {
        protected override void ProcessRecord()
        {
            DismRunner.Invoke(this,
                () => DismApi.Shutdown());
        }
    }

    [Cmdlet((VerbsCommon.Open), "PSDismSession")]
    public class OpenPSDismSessionCommand : Cmdlet
    {
        [Parameter(Mandatory = true)]
        public string ImagePath;
        [Parameter(Mandatory = false)]
        public string WindowsDirectory = null;
        [Parameter(Mandatory = false)]
        public string SystemDrive = null;

        protected override void BeginProcessing()
        {
            DismRunner.ThrowOnInvalidDirectory(this, ImagePath, false);
            DismRunner.ThrowOnInvalidDirectory(this, WindowsDirectory, true);
            DismRunner.ThrowOnInvalidDirectory(this, SystemDrive, true);
        }

        protected override void ProcessRecord()
        {
            DismSession session = DismRunner.Invoke(this,
                () => DismApi.OpenOfflineSession(ImagePath, WindowsDirectory, SystemDrive));

            WriteObject(session);
        }
    }

    [Cmdlet((VerbsCommon.Close), "PSDismSession")]
    public class ClosePSDismSessionCommand : Cmdlet
    {
        [Parameter(Mandatory = true)]
        public DismSession Session;

        protected override void ProcessRecord()
        {
            DismRunner.Invoke(this,
                () => DismApi.CloseSession(Session));
        }
    }

    [Cmdlet((VerbsData.Mount), "PSDismImage")]
    public class MountPSDismImageCommand : Cmdlet
    {
        [Parameter(Mandatory = true)]
        public string ImageFilePath;
        [Parameter(Mandatory = true)]
        public string MountPath;
        [Parameter(Mandatory = true)]
        [ValidateRange(0, int.MaxValue)]
        public int ImageIndex;
        [Parameter(Mandatory = false)]
        public bool ReadOnly;
        [Parameter(Mandatory = false)]
        public DismMountImageOptions MountOptions;
        [Parameter(Mandatory = false)]
        public DismProgressCallback ProgressCallback;
        [Parameter(Mandatory = false)]
        public object UserData;
        [Parameter(Mandatory = false)]
        public SwitchParameter AllowUnsafeDirectories;

        protected override void BeginProcessing()
        {
            DismRunner.ThrowOnInvalidFile(this, ImageFilePath, false, new string[] { ".wim", ".vhd", ".vhdx" });
            DismRunner.ThrowOnInvalidDirectory(this, MountPath, false);
        }

        protected override void ProcessRecord()
        {
            DismRunner.Invoke(this,
                () => DismApi.MountImage(ImageFilePath, MountPath, ImageIndex, ReadOnly, MountOptions, ProgressCallback, UserData));
        }
    }

    [Cmdlet((VerbsData.Dismount), "PSDismImage")]
    public class DismountPSDismImageCommand : Cmdlet
    {
        [Parameter(Mandatory = true)]
        public string MountPath;
        [Parameter(Mandatory = true)]
        public bool CommitChanges;
        [Parameter(Mandatory = false)]
        public DismProgressCallback ProgressCallback;
        [Parameter(Mandatory = false)]
        public object UserData;

        protected override void BeginProcessing()
        {
            DismRunner.ThrowOnInvalidMountPath(this, MountPath);
        }

        protected override void ProcessRecord()
        {
            DismRunner.Invoke(this,
                () => DismApi.UnmountImage(MountPath, CommitChanges, ProgressCallback, UserData));
        }
    }

    [Cmdlet((VerbsCommon.Clear), "PSDismMountPoints")]
    public class ClearPSDismMountPointsCommand : Cmdlet
    {
        protected override void ProcessRecord()
        {
            DismRunner.Invoke(this,
                () => DismApi.CleanupMountpoints());
        }
    }

    [Cmdlet((VerbsCommon.Get), "PSDismPackages")]
    public class GetPSDismPackagesCommand : Cmdlet
    {
        [Parameter(Mandatory = true)]
        public DismSession Session;

        protected override void BeginProcessing()
        {
            DismRunner.ThrowOnInvalidSession(this, Session);
        }

        protected override void ProcessRecord()
        {
            DismPackageCollection packages = DismRunner.Invoke(this,
                () => DismApi.GetPackages(Session));
            WriteObject(packages);
        }
    }

    [Cmdlet((VerbsCommon.Add), "PSDismPackage")]
    public class AddPSDismPackageCommand : Cmdlet
    {
        [Parameter(Mandatory = true)]
        public DismSession Session;
        [Parameter(Mandatory = true)]
        public string PackagePath;
        [Parameter(Mandatory = true)]
        public bool IgnoreCheck;
        [Parameter(Mandatory = true)]
        public bool PreventPending;
        [Parameter(Mandatory = false)]
        public DismProgressCallback ProgressCallback;
        [Parameter(Mandatory = false)]
        public object UserData;

        protected override void BeginProcessing()
        {
            DismRunner.ThrowOnInvalidPackagePath(this, PackagePath);
        }

        protected override void ProcessRecord()
        {
            DismRunner.Invoke(this,
                () => DismApi.AddPackage(Session, PackagePath, IgnoreCheck, PreventPending, ProgressCallback, UserData));
        }
    }

    [Cmdlet((VerbsCommon.Get), "PSDismMountedImages")]
    public class GetPSDismMountedImagesCommand : Cmdlet
    {
        protected override void ProcessRecord()
        {
            DismMountedImageInfoCollection mountedImages = DismRunner.Invoke(this,
                () => DismApi.GetMountedImages());
            WriteObject(mountedImages);
        }
    }

    [Cmdlet((VerbsCommon.Get), "PSDismImageInfo")]
    public class GetPSDismImageInfoCommand : Cmdlet
    {
        [Parameter(Mandatory = true)]
        public string ImagePath;

        protected override void BeginProcessing()
        {
            DismRunner.ThrowOnInvalidFile(this, ImagePath, false, new string[] { ".wim", ".vhd" });
        }

        protected override void ProcessRecord()
        {
            DismImageInfoCollection imageInfo = DismRunner.Invoke(this,
                () => DismApi.GetImageInfo(ImagePath));
            WriteObject(imageInfo);
        }
    }

    [Cmdlet((VerbsCommon.Get), "PSDismPackageInfo")]
    public class GetPSDismPackageInfoCommand : Cmdlet
    {
        [Parameter(Mandatory = true)]
        public DismSession Session;
        [Parameter(Mandatory = false)]
        public SwitchParameter IdentifyByPackageName;
        [Parameter(Mandatory = true)]
        public string Identifier;

        protected override void BeginProcessing()
        {
            DismRunner.ThrowOnInvalidSession(this, Session);
            if (!IdentifyByPackageName)
            {
                DismRunner.ThrowOnInvalidPackagePath(this, Identifier);
            }
        }

        protected override void ProcessRecord()
        {
            DismPackageInfo packageInfo = null;
            if (IdentifyByPackageName)
            {
                packageInfo = DismRunner.Invoke(this,
                    () => DismApi.GetPackageInfoByName(Session, Identifier));
            }
            else
            {
                packageInfo = DismRunner.Invoke(this,
                    () => DismApi.GetPackageInfoByPath(Session, Identifier));
            }

            WriteObject(packageInfo);
        }
    }

    public static class DismRunner
    {
        public static void ThrowOnInvalidFile(Cmdlet instance, string FilePath, bool Optional,
            string[] ValidExtensions = null)
        {
            if (Optional & (String.IsNullOrEmpty(FilePath) || String.IsNullOrWhiteSpace(FilePath))) { return; }

            System.IO.FileInfo fi = new System.IO.FileInfo(FilePath);
            if (!fi.Exists)
            {
                instance.ThrowTerminatingError(new ErrorRecord(new System.IO.FileNotFoundException(),
                    "FileNotFound", ErrorCategory.ObjectNotFound, "FilePath"));
            }
            else if (!(fi.Length > 0))
            {
                instance.ThrowTerminatingError(new ErrorRecord(new System.IO.InvalidDataException(),
                    "EmptyFile", ErrorCategory.InvalidData, "FilePath"));
            }
            else if (null != ValidExtensions && !ValidExtensions.Any(x => x.ToLower() == fi.Extension))
            {
                instance.ThrowTerminatingError(new ErrorRecord(new System.IO.InvalidDataException(),
                    "InvalidFileExtension", ErrorCategory.InvalidType, "FilePath"));
            }
        }

        public static void ThrowOnInvalidDirectory(Cmdlet instance, string DirectoryPath, bool Optional)
        {
            if (Optional & (String.IsNullOrEmpty(DirectoryPath) || String.IsNullOrWhiteSpace(DirectoryPath))) { return; }

            if (!System.IO.Directory.Exists(DirectoryPath))
            {
                instance.ThrowTerminatingError(new ErrorRecord(new System.IO.DirectoryNotFoundException(),
                    "DirectoryNotFound", ErrorCategory.ObjectNotFound, "DirectoryPath"));
            }
        }

        // Per the docs...
        // A relative or absolute path to the .cab or .msu file being added, or a folder containing the expanded files of a single .cab file.
        public static void ThrowOnInvalidPackagePath(Cmdlet instance, string PackagePath)
        {
            System.IO.FileInfo fi = new System.IO.FileInfo(PackagePath);
            System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(PackagePath);

            bool validAsFile = fi.Exists && fi.Length > 0 && (fi.Extension.ToLower() == ".cab" | fi.Extension.ToLower() == ".msu");
            bool validAsDirectory = di.Exists && di.GetFiles().Length > 0;

            if (!validAsFile & !validAsDirectory)
            {
                instance.ThrowTerminatingError(new ErrorRecord(new System.IO.InvalidDataException(),
                    "InvalidPackage", ErrorCategory.ObjectNotFound, "PackagePath"));
            }
        }

        public static void ThrowOnInvalidMountPath(Cmdlet instance, string mountPath)
        {
            try
            {
                DismMountedImageInfoCollection mountedImages = DismApi.GetMountedImages();
                if (mountedImages.Where(x => x.MountPath == mountPath).Count() <= 0)
                {
                    instance.ThrowTerminatingError(new ErrorRecord(new System.Exception(),
                        "InvalidMountPath", ErrorCategory.ObjectNotFound, "mountPath"));
                }
            }
            catch (Exception ex)
            {
                instance.ThrowTerminatingError(new ErrorRecord(new System.Exception(),
                    "DismException", ErrorCategory.NotSpecified, ex.Message));
            }
        }
        public static void ThrowOnInvalidSession(Cmdlet instance, DismSession session)
        {
            if (session.IsClosed || session.IsInvalid)
            {
                instance.ThrowTerminatingError(new ErrorRecord(new System.Exception(),
                    "InvalidSession", ErrorCategory.ResourceUnavailable, "session"));
            }
        }

        public static void Invoke(Cmdlet instance, Action method)
        {
            try
            {
                method();
            }
            catch (Exception ex)
            {
                instance.ThrowTerminatingError(new ErrorRecord(new System.Exception(),
                    "DismException", ErrorCategory.NotSpecified, ex));
            }
        }

        public static T Invoke<T>(Cmdlet instance, Func<T> method)
        {
            try
            {
                return method();
            }
            catch (Exception ex)
            {
                instance.ThrowTerminatingError(new ErrorRecord(new System.Exception(),
                    "DismException", ErrorCategory.NotSpecified, ex.Message));
                return default;
            }
        }
    }
}