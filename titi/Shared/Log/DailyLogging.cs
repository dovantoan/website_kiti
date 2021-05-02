using System;
using System.IO;
namespace Shared
{
    /// <summary>
    /// Summary description for DailyLogging.
    /// </summary>
    public class DailyLogging : ILogging
    {
        private string strLogFolder;
        private string strFileExtension;
        private DateTime dCreatedDate;
        private ILogCreator logCreator;
        private ILog log;

        public DailyLogging(string strLogFolder, ILogCreator logCreator)
        {
            this.strLogFolder = strLogFolder;
            this.logCreator = logCreator;
            strFileExtension = "log";
            dCreatedDate = DateTime.MinValue.Date;
        }

        public ILog GetLog()
        {
            DateTime dCurrent = DateTime.Today;
            if (dCreatedDate != dCurrent)
            {
                if (this.log != null && !this.log.IsClose)
                {
                    this.log.Close();
                }
                string fileName = string.Format("{0}{1}.{2}", dCurrent.DayOfWeek.ToString(), dCurrent.ToString("dd-MM-yyyy"), this.strFileExtension);
                // Delete file co cung thu nhung khac ngay
                if (!Directory.Exists(this.strLogFolder))
                {
                    Directory.CreateDirectory(this.strLogFolder);
                }

                string[] arrFileName = Directory.GetFiles(this.strLogFolder);
                foreach (string strName in arrFileName)
                {
                    if (strName.ToLower().IndexOf(dCurrent.DayOfWeek.ToString().ToLower()) >= 0 && strName.ToLower().IndexOf(fileName.ToLower()) < 0)
                    {
                        try
                        {
                            File.Delete(strName);
                        }
                        catch
                        {
                        }
                    }
                }

                // Create new log
                fileName = string.Format("{0}{1}{2}", this.strLogFolder, Path.DirectorySeparatorChar, fileName);
                this.log = this.logCreator.Create(fileName);
                this.log.Open();
                this.log.WriteHeader();
                this.log.Close();
                this.dCreatedDate = dCurrent;
            }
            return this.log;
        }
    }
}
