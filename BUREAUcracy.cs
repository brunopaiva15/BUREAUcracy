using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Threading;

namespace BUREAUcracy
{
    public partial class BUREAUcracy : ServiceBase
    {
        public BUREAUcracy()
        {
            InitializeComponent();
        }

        public static EventLog myLog = new EventLog();

        protected override void OnStart(string[] args)
        {
            try
            {
                if (!EventLog.SourceExists("BUREAUcracy"))
                {
                    EventLog.CreateEventSource("BUREAUcracy", "Log");
                    return;
                }

                myLog.Source = "BUREAUcracy";

                myLog.WriteEntry("Service started.");

                var watcher = new FileSystemWatcher(@"C:\Users\" + "ppaiv" + @"\Desktop");

                watcher.NotifyFilter = NotifyFilters.Attributes
                                    | NotifyFilters.CreationTime
                                    | NotifyFilters.DirectoryName
                                    | NotifyFilters.FileName
                                    | NotifyFilters.LastAccess
                                    | NotifyFilters.LastWrite
                                    | NotifyFilters.Security
                                    | NotifyFilters.Size;

                watcher.Created += OnCreated;
                watcher.Error += OnError;

                watcher.IncludeSubdirectories = true;
                watcher.EnableRaisingEvents = true;

            }
            catch(Exception ex)
            {
                myLog.WriteEntry("An error has occured. Error: " + ex.Message);
                throw new Exception(ex.Message);
            }
            
        }

        private static void OnCreated(object sender, FileSystemEventArgs e)
        {
            try
            {
                FileAttributes attr = File.GetAttributes(e.FullPath);
                myLog.WriteEntry("F/D to delete: " + e.FullPath);

                if ((attr & FileAttributes.Directory) == FileAttributes.Directory)
                {
                    if (Directory.Exists(e.FullPath))
                    {
                        DeleteDirectory(e.FullPath);
                    }              
                }
                else
                {
                    if (File.Exists(e.FullPath))
                    {
                        myLog.WriteEntry("It's a file. Deleting " + e.FullPath);

                        FileInfo file = new FileInfo(e.FullPath);
                        while (IsFileLocked(file))
                            Thread.Sleep(1000);
                        file.Delete();
                    }            
                }
            }
            catch (Exception ex)
            {
                myLog.WriteEntry("An error has occured. Error: " + ex.Message);
                throw new Exception(ex.Message);
            }
                
        }

        private static void OnError(object sender, ErrorEventArgs e)
        {
            PrintException(e.GetException());
        }  

        private static void PrintException(Exception ex)
        {
            if (ex != null)
            {
                myLog.WriteEntry("An error has occured. Error: " + ex.Message);
                PrintException(ex.InnerException);
            }
        }

        protected override void OnStop()
        {
            myLog.WriteEntry("Service stopped.");
        }

        protected static bool IsFileLocked(FileInfo file)
        {
            FileStream stream = null;

            try
            {
                stream = file.Open(FileMode.Open, FileAccess.ReadWrite, FileShare.None);
            }
            catch (IOException)
            {
                //the file is unavailable because it is:
                //still being written to
                //or being processed by another thread
                //or does not exist (has already been processed)
                return true;
            }
            finally
            {
                if (stream != null)
                    stream.Close();
            }

            //file is not locked
            return false;
        }

        protected static void DeleteDirectory(string target_dir)
        {
            string[] files = Directory.GetFiles(target_dir);
            string[] dirs = Directory.GetDirectories(target_dir);

            foreach (string file in files)
            {
                File.SetAttributes(file, FileAttributes.Normal);
                File.Delete(file);
            }

            foreach (string dir in dirs)
            {
                DeleteDirectory(dir);
            }

            Directory.Delete(target_dir, false);
        }
    }
}
