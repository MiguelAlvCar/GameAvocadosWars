using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Principal;
using System.Security.AccessControl;
using System.IO;

namespace InstalerSecurityAction
{
    [RunInstaller(true)]
    public partial class InstallerAction : System.Configuration.Install.Installer
    {
        public InstallerAction()
        {
            InitializeComponent();
        }

        public override void Install(IDictionary stateSaver)
        {
            // Explicitly call the overriden method to properly return control to the installer
            base.Install(stateSaver);

            //if (Context.Parameters.ContainsKey("targetdir"))
            //    Log(Context.Parameters["targetdir"]);
            //else
            //    Log("targetdir doesnt exist");

            // This gets the named parameters passed in from your custom action
            string folder = Context.Parameters["targetdir"]; /*@"C:\Program Files\El españolito productions\Avocados Wars"*/


            // This gets the "Authenticated Users" group, no matter what it's called
            SecurityIdentifier sid = new SecurityIdentifier(WellKnownSidType.BuiltinUsersSid, null);

            // Create the rules
            FileSystemAccessRule writerule = new FileSystemAccessRule(sid, FileSystemRights.Write | FileSystemRights.Modify | 
                FileSystemRights.CreateFiles | FileSystemRights.CreateDirectories | FileSystemRights.AppendData,
                InheritanceFlags.ObjectInherit | InheritanceFlags.ContainerInherit,   // all sub-dirs to inherit
                PropagationFlags.None, AccessControlType.Allow);

            if (!string.IsNullOrEmpty(folder) && Directory.Exists(folder))
            {
                // Get your file's ACL
                DirectorySecurity fsecurity = Directory.GetAccessControl(folder);

                // Add the new rule to the ACL
                fsecurity.AddAccessRule(writerule);

                // Set the ACL back to the file
                Directory.SetAccessControl(folder, fsecurity);
            }

            Log(folder + @"\Avocados Wars.exe", System.Environment.CurrentDirectory);
            new FirewallConfig(folder + @"\Avocados Wars.exe").OpenFirewall();
        }

        public override void Uninstall(IDictionary savedState)
        {
            new FirewallConfig(Context.Parameters["targetdir"] + @"\Avocados Wars.exe").CloseFirewall();

            base.Uninstall(savedState);

            try
            {
                // InstallerSecurityAction.dll will throw an exception that it can be ignore
                Directory.Delete(Context.Parameters["targetdir"], true);

                //DirectoryInfo dirInfo = new DirectoryInfo(Context.Parameters["targetdir"]);

                //foreach (DirectoryInfo dirInfo2 in dirInfo.GetFile  .EnumerateDirectories())
                //{
                //    dirInfo2.Delete();
                //}
                //foreach (FileInfo fileInfo in dirInfo.EnumerateFiles())
                //{
                //    fileInfo.Delete();
                //}
                //dirInfo.Delete();
            }
            catch (UnauthorizedAccessException e)
            {
                //Log(e.ToString(), Context.Parameters["targetdir"]);
                //Log(e.Message, Context.Parameters["targetdir"]);
            }
        }

        public static void Log(string message, string path, Exception e = null)
        {
            string CurrentDirectory = System.Environment.CurrentDirectory;
            FileStream filest = new FileStream(path + @"\Miguel.txt", FileMode.Append);
            BinaryWriter BinWriter = new BinaryWriter(filest);
            //BinWriter.Seek(0, SeekOrigin.Begin);
            BinWriter.Write("\n\n" + message);
            BinWriter.Close();
            if (e != null)
                throw e;
        }
    }
}
