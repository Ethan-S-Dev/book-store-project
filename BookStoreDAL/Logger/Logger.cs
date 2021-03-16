using System;
using System.IO;

namespace BookStore.DAL.Logger
{
    /// <summary>
    /// Class handling saving log files for the Application.
    /// </summary>
    public class Logger
    {
        public Logger()
        {
            fileName = $"Log-Opened-{DateTime.Now.ToString("dd-MM-yy-(HH.mm.ss)")}.txt";
            DirectoryInfo dir = Directory.CreateDirectory(Path);
            
            using (Stream file = File.Create(FullPath))
            {
                
            }
            Log("Appliction Started!");
            FileInfo[] files = dir.GetFiles();
            if (files.Length > MAXLOGSFILES)
            {
                FileInfo fileToRemove = files[0];
                for (int i = 1; i < files.Length; i++)
                {
                    if (files[i].CreationTime.CompareTo(fileToRemove.CreationTime) < 0)
                    {
                        fileToRemove = files[i];
                    }
                }
                File.Delete(fileToRemove.FullName);
            }
        }
        public void Log(string message)
        {
            File.AppendAllText(FullPath, $"[Core] | {DateTime.Now.ToString("T")} | [Log] | {message}\n");
        }
        public void Warning(string message)
        {
            File.AppendAllText(FullPath, $"[Core] | {DateTime.Now.ToString("T")} | [Warning] | {message}\n");
        }
        public void Fetal(string message)
        {
            File.AppendAllText(FullPath, $"[Core] | {DateTime.Now.ToString("T")} | [Fetal] | {message}\n");
        }
        public void Error(string message)
        {
            File.AppendAllText(FullPath, $"[Core] | {DateTime.Now.ToString("T")} | [Error] | {message}\n");
        }
        public void Exception(Exception ex,string additonalMessage)
        {
            File.AppendAllText(FullPath, $"[Core] | {DateTime.Now.ToString("T")} | [{ex.GetType().Name}] | [{ex.Message}] : {additonalMessage} \n");
            ex = ex.InnerException;
            while (ex != null)
            {
                File.AppendAllText(FullPath, $"      [Core] | {DateTime.Now.ToString("T")} | [{ex.GetType().Name}] | [{ex.Message}] -- Inner\n");
                ex = ex.InnerException;
            }
        }
        public void Exception(Exception ex)
        {
            File.AppendAllText(FullPath, $"[Core] | {DateTime.Now.ToString("T")} | [{ex.GetType().Name}] | [{ex.Message}] \n");
        }
        public void LogClient(string message)
        {
            File.AppendAllText(FullPath, $"[Client] | {DateTime.Now.ToString("T")} | [Log] | {message}\n");
        }
        public void WarningClient(string message)
        {
            File.AppendAllText(FullPath, $"[Client] | {DateTime.Now.ToString("T")} | [Warning] | {message}\n");
        }
        public void FetalClient(string message)
        {
            File.AppendAllText(FullPath, $"[Client] | {DateTime.Now.ToString("T")} | [Fetal] | {message}\n");
        }
        public void ErrorClient(string message)
        {
            File.AppendAllText(FullPath, $"[Client] | {DateTime.Now.ToString("T")} | [Error] | {message}\n");
        }
        public void ExceptionClient(Exception ex, string additonalMessage)
        {
            File.AppendAllText(FullPath, $"[Client] | {DateTime.Now.ToString("T")} | [{ex.GetType().Name}] | [{ex.Message}] : {additonalMessage} \n");
        }
        public void ExceptionClient(Exception ex)
        {
            File.AppendAllText(FullPath, $"[Client] | {DateTime.Now.ToString("T")} | [{ex.GetType().Name}] | [{ex.Message}] \n");
            ex = ex.InnerException;
            while (ex != null)
            {
                File.AppendAllText(FullPath, $"      [Client] | {DateTime.Now.ToString("T")} | [{ex.GetType().Name}] | [{ex.Message}] -- Inner\n");
                ex = ex.InnerException;
            }
        }
        public string Path { get { return path; } }
        public string FileName { get { return fileName; } }
        public string FullPath { get { return $"{Path}/{FileName}"; } }
        
        string path = "./Data/Logs";
        string fileName;
        const int MAXLOGSFILES = 20;
    }
}
