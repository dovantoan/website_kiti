using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Data;
using System.Windows.Forms;
using System.Drawing;
using System.Security.Cryptography;
using System.Data.OleDb;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Drawing.Printing;
using System.Diagnostics;
using System.Linq;
using System.Net.NetworkInformation;
using System.Web;
using System.Globalization;
using System.Collections;
using System.Reflection;

namespace Shared.Utility
{
    public class FunctionUtility
    {
        //public static List<T> DataTableToList<T>(this DataTable table) where T : class, new()
        //{
        //    try
        //    {
        //        List<T> list = new List<T>();

        //        foreach (var row in table.AsEnumerable())
        //        {
        //            T obj = new T();

        //            foreach (var prop in obj.GetType().GetProperties())
        //            {
        //                try
        //                {
        //                    PropertyInfo propertyInfo = obj.GetType().GetProperty(prop.Name);
        //                    propertyInfo.SetValue(obj, Convert.ChangeType(row[prop.Name], propertyInfo.PropertyType), null);
        //                }
        //                catch
        //                {
        //                    continue;
        //                }
        //            }

        //            list.Add(obj);
        //        }

        //        return list;
        //    }
        //    catch
        //    {
        //        return null;
        //    }
        //}
        private static readonly Regex validIpV4AddressRegex = new Regex(@"^(([0-9]|[1-9][0-9]|1[0-9]{2}|2[0-4][0-9]|25[0-5])\.){3}([0-9]|[1-9][0-9]|1[0-9]{2}|2[0-4][0-9]|25[0-5])$", RegexOptions.IgnoreCase);

        public static string EncodePassword(string originalPassword)
        {
            //Declarations
            Byte[] originalBytes;
            Byte[] encodedBytes;
            MD5 md5;

            //Instantiate MD5CryptoServiceProvider, get bytes for original password and compute hash (encoded password)
            md5 = new MD5CryptoServiceProvider();
            originalBytes = ASCIIEncoding.Default.GetBytes(originalPassword);
            encodedBytes = md5.ComputeHash(originalBytes);

            //Convert encoded bytes back to a 'readable' string
            return BitConverter.ToString(encodedBytes);
        }

        /// <summary>
        /// Get contend of message base on message id, them defined in file Messages.txt at startup folder
        /// </summary>
        /// <param name="strMessageId">Message id</param>
        /// <returns>Contend of message</returns>
        public static string GetMessage(string strMessageId)
        {
            //Đường dẫn tới message
            string strStartupPath = System.Windows.Forms.Application.StartupPath;
            string strPath = string.Format(@"{0}\Utility\Messages.txt", strStartupPath);

            if (File.Exists(strPath))
            {
                StreamReader srReadLine = new StreamReader(strPath, System.Text.Encoding.GetEncoding("Shift-JIS"));
                srReadLine.BaseStream.Seek(0, SeekOrigin.Begin);
                while (true)
                {
                    string str = srReadLine.ReadLine();
                    if (str == null)
                    {
                        break;
                    }
                    try
                    {
                        string[] temp = str.Split('=');
                        string id = temp[0].Replace("var", "").Trim();
                        if (id == strMessageId)
                        {
                            string strMessage = str.Substring(str.IndexOf("=") + 1);
                            srReadLine.Close();
                            return strMessage.Trim();
                        }
                    }
                    catch
                    {
                    }
                }
                srReadLine.Close();
            }
            return string.Empty;
        }

        public static DataSet GetExcelToDataSet(string strPathFile, string strSql)
        {
            OleDbConnection oConnection = new OleDbConnection("provider=Microsoft.Jet.OLEDB.4.0;data source=" + strPathFile + ";Extended Properties=\"Excel 8.0;HDR=YES;IMEX=1;MAXSCANROWS=0;\"");
            string strCommandText = strSql;
            OleDbDataAdapter adp = new OleDbDataAdapter(strCommandText, oConnection);
            DataSet dsXLS = new DataSet();
            try
            {
                adp.Fill(dsXLS);
                return dsXLS;
            }
            catch
            {
                return dsXLS;
            }
        }
        public static DataSet GetExcelToDataSetVersion2(string strPathFile, string strSql)
        {
            OleDbConnection oConnection = new OleDbConnection("provider=Microsoft.Jet.OLEDB.4.0;data source=" + strPathFile + ";Extended Properties=\"Excel 8.0;HDR=NO;IMEX=1;MAXSCANROWS=0;ImportMixedTypes=Text\"");
            string strCommandText = strSql;
            OleDbDataAdapter adp = new OleDbDataAdapter(strCommandText, oConnection);
            DataSet dsXLS = new DataSet();
            DataSet dsReturn = new DataSet();
            try
            {
                adp.Fill(dsXLS);

                if (dsXLS.Tables[0].Rows.Count > 0)
                {
                    DataTable dtReturn = new DataTable();
                    for (int c = 0; c < dsXLS.Tables[0].Columns.Count; c++)
                    {
                        dtReturn.Columns.Add(dsXLS.Tables[0].Rows[0][c].ToString());
                    }
                    for (int r = 1; r < dsXLS.Tables[0].Rows.Count; r++)
                    {
                        DataRow dr = dtReturn.NewRow();
                        for (int i = 0; i < dsXLS.Tables[0].Columns.Count; i++)
                        {
                            dr[i] = dsXLS.Tables[0].Rows[r][i];
                        }
                        dtReturn.Rows.Add(dr);
                    }

                    dsReturn.Tables.Add(dtReturn);
                }
                return dsReturn;
            }
            catch
            {
                return dsXLS;
            }
        }

        /// <summary>
        /// Get Data From Excel File
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="nSheet"></param>
        /// <param name="areaData"></param>
        /// <returns></returns>
        public static DataTable GetDataFromExcel(string fileName, string sheetName, string areaData)
        {
            //try
            //{
            //  object missing = System.Reflection.Missing.Value;
            //  Excel.ApplicationClass xl = new Excel.ApplicationClass();
            //  Excel.Workbook xlBook;
            //  Excel.Sheets xlSheets;
            //  Excel.Worksheet xlSheet;

            //  xlBook = (Excel.Workbook)xl.Workbooks.Open(fileName, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing);
            //  xlSheets = xlBook.Worksheets;
            //  xlSheet = (Excel.Worksheet)xlSheets.get_Item(1);
            //  xlBook.Close(null, null, null);

            //  OleDbConnection connection = new OleDbConnection("provider=Microsoft.Jet.OLEDB.4.0;data source=" + fileName + ";Extended Properties=Excel 8.0;");
            //  string commandText = string.Format(@"SELECT * FROM [{0}${1}]", sheetName, areaData);
            //  OleDbDataAdapter adp = new OleDbDataAdapter(commandText, connection);
            //  System.Data.DataTable dtXLS = new System.Data.DataTable();
            //  adp.Fill(dtXLS);
            //  connection.Close();
            //  if (dtXLS == null)
            //  {
            //    return null;
            //  }
            //  return dtXLS;
            //}
            //catch
            //{
            //  Shared.Utility.WindowUtinity.ShowMessageError("ERR0069");
            //  return null;
            //}

            return null;
        }

        /// <summary>
        /// Convert a number in Milimet to Inch
        /// </summary>
        /// <param name="value">Number in milimet</param>
        /// <returns>Number in Inch</returns>
        public static string ConverMilimetToInch(int value)
        {
            if (value == int.MinValue)
            {
                return string.Empty;
            }
            double result = value * 0.0393700787;
            int integerPart = (int)result;
            double decimalPart = result - integerPart;
            if (decimalPart < 0.125)
            {
                return DBConvert.ParseString(integerPart) + "''";
            }
            else if (decimalPart < 0.375)
            {
                return DBConvert.ParseString(integerPart) + " " + "1/4''";
            }
            else if (decimalPart < 0.635)
            {
                return DBConvert.ParseString(integerPart) + " " + "1/2''";
            }
            else if (decimalPart < 0.875)
            {
                return DBConvert.ParseString(integerPart) + " " + "3/4''";
            }
            return DBConvert.ParseString(integerPart + 1) + "''";
        }

        public static string ConVertStringToAnsiString(string accented)
        {
            Regex regex = new Regex(@"\p{IsCombiningDiacriticalMarks}+");
            string strFormD = accented.Normalize(System.Text.NormalizationForm.FormD);
            return regex.Replace(strFormD, String.Empty).Replace('\u0111', 'd').Replace('\u0110', 'D');
        }


        public static DialogResult SaveBeforeClosing()
        {
            return MessageBox.Show(GetMessage("MSG0008"), "Save data", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
        }

        #region VB Report, Excel
        public static void InitializeOutputdirectory(string strPathOutputFile)
        {
            if (Directory.Exists(strPathOutputFile))
            {
                string[] files = Directory.GetFiles(strPathOutputFile);
                foreach (string file in files)
                {
                    try
                    {
                        File.Delete(file);
                    }
                    catch { }
                }
            }
            else
            {
                Directory.CreateDirectory(strPathOutputFile);
            }
        }

        /// <summary>
        /// Get Source from excel file
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="sheetName"></param>
        /// <param name="cellFromTo"></param>
        /// <returns>DataTable</returns>
        public static DataTable GetSourceFromExcelFile(string filePath, string sheetName, string cellFromTo)
        {
            DataTable dt = new DataTable();
            return dt;
        }
        #endregion VB Report, Excel

        #region Image Proccess
        public static Image GetThumbnailImage(string fileName, string fileFrame)
        {
            Image image = Image.FromFile(fileName);
            Image frame = Image.FromFile(fileFrame);

            Image dest = null;

            int size = frame.Height;
            if (image != null)
            {
                if (image.Width > image.Height)
                    dest = image.GetThumbnailImage(size, image.Height * size / image.Width, null, new IntPtr());
                else
                    dest = image.GetThumbnailImage(image.Width * size / image.Height, size, null, new IntPtr());
            }

            using (Graphics grfx = Graphics.FromImage(frame))
            {
                grfx.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                if (image.Width > image.Height)
                    grfx.DrawImage(dest, 0, frame.Height / 2 - dest.Height / 2, dest.Width, dest.Height);
                else
                    grfx.DrawImage(dest, frame.Width / 2 - dest.Width / 2, 0, dest.Width, dest.Height);
            }

            return frame;
        }

        /// <summary>
        /// thumbnail a image how to fix with frame that it's width and height in CM
        /// </summary>
        /// <param name="fileName">Image file source</param>
        /// <param name="width">Width of frame with cm</param>
        /// <param name="height">height of frame with cm</param>
        /// <returns>Destination image</returns>
        public static Image GetThumbnailImage(string fileName, double width, double height)
        {
            if (!File.Exists(fileName))
                return null;
            Image image = Image.FromFile(fileName);
            string frameFileName = System.Windows.Forms.Application.StartupPath + @"\\frame.JPG";
            Image frame = Image.FromFile(frameFileName);

            int dpix = 600;
            int dpiy = 600;
            System.Drawing.Printing.PrinterSettings oPS = new System.Drawing.Printing.PrinterSettings();
            foreach (System.Drawing.Printing.PrinterResolution resolution in oPS.PrinterResolutions)
            {
                if (resolution.X > 0)
                {
                    dpix = resolution.X;
                    dpiy = resolution.Y;
                }
            }

            int frameWidth = (int)((width / 2.54) * dpix);
            int frameHeight = (int)((height / 2.54) * dpiy);

            //Bitmap newFrame = new Bitmap(frame, new Size(frameWidth, frameHeight));
            Image newFrame = frame.GetThumbnailImage(frameWidth, frameHeight, null, new IntPtr());

            Image dest = null;

            if (image != null)
            {
                if (image.Width > frameWidth || image.Height > frameHeight)
                {
                    if (image.Width > frameWidth && image.Height <= frameHeight)
                    {
                        dest = image.GetThumbnailImage(frameWidth, image.Height * frameWidth / image.Width, null, new IntPtr());
                    }
                    else if (image.Width > frameWidth && image.Height > frameHeight)
                    {
                        if ((float)image.Width / frameWidth < (float)image.Height / frameHeight)
                        {
                            dest = image.GetThumbnailImage(image.Width * frameHeight / image.Height, frameHeight, null, new IntPtr());
                        }
                        else
                        {
                            dest = image.GetThumbnailImage(frameWidth, image.Height * frameWidth / image.Width, null, new IntPtr());
                        }
                    }
                    else if (image.Width < frameWidth && image.Height > frameHeight)
                    {
                        dest = image.GetThumbnailImage(image.Width * frameHeight / image.Height, frameHeight, null, new IntPtr());
                    }
                }
                else
                {
                    dest = image;
                }
            }
            try
            {
                using (Graphics grfx = Graphics.FromImage(newFrame))
                {
                    grfx.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                    grfx.DrawImage(dest, (frameWidth - dest.Width) / 2, (frameHeight - dest.Height) / 2, dest.Width, dest.Height);
                }
            }
            catch (Exception ex)
            {
                string logFileName = "c:\\BOMLog.txt";
                if (!File.Exists(logFileName))
                    File.Create(logFileName);

                StreamWriter SW;
                SW = File.AppendText(logFileName);
                SW.WriteLine(ex.Message);
                SW.Close();
            }
            finally
            {
                image.Dispose();
                frame.Dispose();
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }

            return newFrame;
        }

        public static byte[] ImageToByteArray(System.Drawing.Image imageIn)
        {
            if (imageIn != null)
            {
                MemoryStream ms = new MemoryStream();
                imageIn.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                ms.Close();
                return ms.ToArray();
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// If image <> null => return array byte of image other wise return white image
        /// </summary>
        /// <param name="imageIn"></param>
        /// <returns></returns>
        public static byte[] ImageToByteArray_Always(System.Drawing.Image imageIn)
        {
            if (imageIn != null)
            {
                MemoryStream ms = new MemoryStream();
                imageIn.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                ms.Close();
                return ms.ToArray();
            }
            else
            {
                return ImagePathToByteArray(System.Windows.Forms.Application.ExecutablePath + "\frame.JPG");
            }
        }

        public static Byte[] ImagePathToByteArray(string imagePath)
        {
            try
            {
                FileStream fs = new FileStream(imagePath, FileMode.Open, FileAccess.Read);
                BinaryReader br = new BinaryReader(fs);
                byte[] imgbyte = new byte[fs.Length + 1];
                imgbyte = br.ReadBytes(Convert.ToInt32((fs.Length)));
                br.Close();
                fs.Close();
                return imgbyte;
            }
            catch { }
            return null;
        }

        /// <summary>
        /// If image path is exist => return array byte of image other wise return white image
        /// </summary>
        /// <param name="imagePath"></param>
        /// <returns></returns>
        public static Byte[] ImagePathToByteArray_Always(string imagePath)
        {
            try
            {
                if (!File.Exists(imagePath))
                    imagePath = System.Windows.Forms.Application.StartupPath + "\\frame.JPG";
                FileStream fs = new FileStream(imagePath, FileMode.Open, FileAccess.Read);
                BinaryReader br = new BinaryReader(fs);
                byte[] imgbyte = new byte[fs.Length + 1];
                imgbyte = br.ReadBytes(Convert.ToInt32((fs.Length)));
                br.Close();
                fs.Close();
                return imgbyte;
            }
            catch { }
            return null;
        }

        /// <summary>
        /// Show popup image (can move)
        /// </summary>
        /// <param name="imagePath"></param>
        /// <param name="picture"></param>
        public static void ShowImagePopup(string imagePath)
        {
            Process p = new Process();
            p.StartInfo.FileName = "rundll32.exe";
            if (File.Exists(imagePath))
            {
                p.StartInfo.Arguments = @"C:\WINDOWS\System32\shimgvw.dll,ImageView_Fullscreen " + imagePath;
            }
            else
            {
                p.StartInfo.Arguments = @"C:\WINDOWS\System32\shimgvw.dll,ImageView_Fullscreen ";
            }
            p.Start();
        }
        #endregion Image Proccess

        #region Number Methods

        /// <summary>
        /// Doi so double sang string theo format "999,999.99" (Sau dau '.' là _phanLe chu so thap phan)
        /// Neu _phanLe < 0 thì không định dạng theo format nào cả mà chỉ tra về giá trị _number.ToString().
        /// </summary>
        /// <param name="_number">So double can doi sang string</param>
        /// <param name="_phanLe">So cac so le</param>
        /// <returns></returns>
        public static string NumericFormat(double _number, int _phanLe)
        {
            if (_number == double.MinValue)
            {
                return string.Empty;
            }
            if (_phanLe < 0)
            {
                return _number.ToString();
            }
            System.Globalization.NumberFormatInfo formatInfo = new System.Globalization.NumberFormatInfo();
            double t = Math.Truncate(_number);
            formatInfo.NumberDecimalDigits = _phanLe;
            return _number.ToString("N", formatInfo);
        }

        /// <summary>
        /// Doi so long sang string theo format "999,999"
        /// </summary>
        /// <param name="_number">So long can doi sang string</param>
        /// <returns></returns>
        public static string NumericFormat(long _number)
        {
            if (_number == long.MinValue)
            {
                return string.Empty;
            }
            System.Globalization.NumberFormatInfo formatInfo = new System.Globalization.NumberFormatInfo();
            formatInfo.NumberDecimalDigits = 0;
            return _number.ToString("N", formatInfo);
        }
        /// <summary>
        /// Doi so int sang string theo format "999,999"
        /// </summary>
        /// <param name="_number">So int can doi sang string</param>
        /// <returns></returns>
        public static string NumericFormat(int _number)
        {
            if (_number == int.MinValue)
            {
                return string.Empty;
            }
            System.Globalization.NumberFormatInfo formatInfo = new System.Globalization.NumberFormatInfo();
            formatInfo.NumberDecimalDigits = 0;
            return _number.ToString("N", formatInfo);
        }
        /// <summary>
        /// Doi so Int16 sang string theo format "9,999"
        /// </summary>
        /// <param name="_number">So Int16 can doi sang string</param>
        /// <returns></returns>
        public static string NumericFormat(Int16 _number)
        {
            if (_number == Int16.MinValue)
            {
                return string.Empty;
            }
            System.Globalization.NumberFormatInfo formatInfo = new System.Globalization.NumberFormatInfo();
            formatInfo.NumberDecimalDigits = 0;
            return _number.ToString("N", formatInfo);
        }

        /// <summary>
        /// Đổi số double sang kiểu string.
        /// Trong phần nguyên cứ 3 số thì thêm dấu phẩy vào
        /// </summary>
        /// <param name="_number"></param>
        /// <returns></returns>
        public static string AddSeparator(double _number)
        {
            if (_number < 0)
            {
                return string.Empty;
            }
            string parseString = _number.ToString();
            int index = parseString.IndexOf('.');
            string partFloat = string.Empty;
            string partInt = parseString;
            if (index > 0)
            {
                partFloat = parseString.Substring(index, parseString.Length - index);
                partInt = parseString.Substring(0, index);
            }
            partInt = NumericFormat(DBConvert.ParseDouble(partInt), 0);
            return string.Format("{0}{1}", partInt, partFloat);
        }

        /// <summary>
        /// Get Default Printer
        /// </summary>
        /// <returns></returns>
        public static string GetDefaultPrinter()
        {
            PrinterSettings settings = new PrinterSettings();
            foreach (string printer in PrinterSettings.InstalledPrinters)
            {
                settings.PrinterName = printer;
                if (settings.IsDefaultPrinter)
                    return printer;
            }
            return string.Empty;
        }
        #endregion Number Methods

        public static DataTable ConvertToDataTable<T>(IList<T> data)
        {
            try
            {
                if (data == null)
                {
                    return null;
                }

                PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(T));
                DataTable table = new DataTable("Datatable");
                foreach (PropertyDescriptor prop in properties)
                    table.Columns.Add(prop.Name, System.Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
                foreach (T item in data)
                {
                    DataRow row = table.NewRow();
                    foreach (PropertyDescriptor prop in properties)
                        row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                    table.Rows.Add(row);
                }

                table.Columns.RemoveAt(0);

                return table;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool CheckMemberDataTable(DataTable dt, string colName, string inputvalue)
        {
            bool check = false;
            if (dt != null)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows[i][colName].ToString() == inputvalue)
                    {
                        check = true;
                        break;
                    }
                }
            }
            return check;
        }

        public static void DataTableToCSV(DataTable dt)
        {
            if (dt == null)
            {
                return;
            }

            StringBuilder sb = new StringBuilder();

            var columnNames = dt.Columns.Cast<DataColumn>().Select(column => "\"" + column.ColumnName.Replace("\"", "\"\"") + "\"").ToArray();
            sb.AppendLine(string.Join(",", columnNames));

            foreach (DataRow row in dt.Rows)
            {
                var fields = row.ItemArray.Select(field => "\"" + field.ToString().Replace("\"", "\"\"") + "\"").ToArray();
                sb.AppendLine(string.Join(",", fields));
            }

            File.WriteAllText("GridExport.csv", sb.ToString(), Encoding.Default);

            OpenFileCSV(System.Environment.CurrentDirectory + @"\GridExport.csv");
        }

        static void OpenFileCSV(string path)
        {
            var ExcelApp = new Microsoft.Office.Interop.Excel.Application();
            ExcelApp.Workbooks.OpenText(path, Comma: true);

            ExcelApp.Visible = true;
        }

        public static void ApplicationWriteErrorLog(string folder, Exception ex)
        {
            TransactionException te = new TransactionException("", ex);
            ILogging logging = new DailyLogging(folder, new TraceLogCreator()); ;
            ILog log = logging.GetLog();
            lock (log)
            {
                log.Open();
                log.WriteLine("---------------- BEGIN EXCEPTION ----------------------------------");
                log.Write(te.InnerException);
                log.WriteLine("---------------- END EXCEPTION ------------------------------------");
                log.Close();
            }
        }

        public static void ApplicationWriteLog(string folder, string lockKey)
        {
            ILogging logging = new DailyLogging(folder, new TraceLogCreator()); ;
            ILog log = logging.GetLog();
            lock (log)
            {
                log.Open();
                log.WriteLine("---------------- BEGIN EXCEPTION ----------------------------------");
                log.WriteLine(lockKey);
                log.WriteLine("---------------- END EXCEPTION ------------------------------------");
                log.Close();
            }
        }

        /// <summary>
        /// Check send IP Address
        /// </summary>
        /// <param name="ipAddress"></param>
        /// <returns></returns>
        public static bool IsValidIP(string ipAddress)
        {
            PingReply reply;
            Ping pingSender = new Ping();

            try
            {
                reply = pingSender.Send(ipAddress);
            }
            catch (Exception)
            {
                return false;
            }

            return reply.Status == IPStatus.Success;
        }

        /// <summary>
        /// Validates an IPv4 address.
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        public static bool IsIpV4AddressValid(string address)
        {
            if (!string.IsNullOrWhiteSpace(address))
            {
                return validIpV4AddressRegex.IsMatch(address.Trim());
            }
            return false;
        }

        public static bool IsValidTime(string time)
        {
            DateTime dummyDate;
            return DateTime.TryParseExact(time, new[] { "HH:mm", "H:mm" },
                CultureInfo.InvariantCulture,
                DateTimeStyles.NoCurrentDateDefault, out dummyDate);
        }

        /// <summary>
        /// Xuất dữ liệu từ datatable ra csv Website
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="fileName">With extention .csv</param>
        /// <param name="response"></param>
        /// <returns></returns>
        public static void DataTableToCSV(DataTable dt, string fileName, HttpResponseBase response)
        {
            try
            {
                response.Clear();
                response.ClearHeaders();
                response.ClearContent();
                response.AddHeader("Content-Disposition", "attachment; filename=" + fileName);//fileName with extention .csv
                response.ContentType = "text/csv";
                response.ContentEncoding = System.Text.Encoding.UTF8;

                StringBuilder sb = new StringBuilder();
                var columnNames = dt.Columns.Cast<DataColumn>().Select(column => "\"" + column.ColumnName.Replace("\"", "\"\"") + "\"").ToArray();
                sb.AppendLine(string.Join(",", columnNames));

                foreach (DataRow row in dt.Rows)
                {
                    var fields = row.ItemArray.Select(field => "\"" + field.ToString().Replace("\"", "\"\"") + "\"").ToArray();
                    sb.AppendLine(string.Join(",", fields));
                }
                byte[] BOM = new byte[] { 0xef, 0xbb, 0xbf };
                response.BinaryWrite(BOM);//write the BOM first
                response.BinaryWrite(Encoding.UTF8.GetBytes(sb.ToString()));
                //Response.Write(sb.ToString());
                response.Flush();
                response.End();
            }
            catch
            {
            }
        }

        //DataTableToExcel
        public static void DataTableToExcel(DataTable table, string fileName, HttpResponseBase response)
        {
            System.Web.UI.WebControls.GridView gv = new System.Web.UI.WebControls.GridView();
            gv.DataSource = table;
            gv.DataBind();
            response.ClearContent();
            response.Buffer = true;
            response.AddHeader("content-disposition", "attachment; filename=" + fileName);
            //response.ContentType = "application/ms-excel";
            response.ContentType = "application/vnd.ms-excel";
            byte[] BOM = new byte[] { 0xef, 0xbb, 0xbf };
            response.BinaryWrite(BOM);//write the BOM first
            response.Write(@"<style> TD { mso-number-format:\@; } </style>");
            response.Charset = "";
            response.ContentEncoding = System.Text.Encoding.UTF8;
            StringWriter sw = new StringWriter();
            System.Web.UI.HtmlTextWriter htw = new System.Web.UI.HtmlTextWriter(sw);
            //htw.WriteLine("<meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\">");
            htw.WriteLine("<meta http-equiv=\"Content-Type\" content=\"application/vnd.ms-excel; charset=utf-8\">");
            gv.RenderControl(htw);
            response.Output.Write(sw.ToString());
            response.Flush();
            response.End();
        }

        /// <summary>
        /// Method export excel có định dạng row and column 
        /// Author: ToanDV 
        /// Create date 17/02/2017
        /// </summary>
        //public static void DataTableToExcel(DataTable table, string fileName, HttpResponseBase response)
        //{
        //    var xlApp = new Microsoft.Office.Interop.Excel.Application();
        //    try
        //    {
        //        System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;

        //        List<String> Columns = new List<string>();

        //        ///Setting the Current Culture...Can remove this
        //        System.Threading.Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");

        //        Microsoft.Office.Interop.Excel.Workbooks workbooks = xlApp.Workbooks;
        //        Microsoft.Office.Interop.Excel.Range range;
        //        Microsoft.Office.Interop.Excel.Workbook workbook = workbooks.Add();
        //        Microsoft.Office.Interop.Excel.Worksheet worksheet = workbook.Worksheets[1];

        //        long totalCount = table.Rows.Count;
        //        long rowRead = 0;

        //        foreach (DataColumn col in table.Columns)
        //        {
        //            Columns.Add(col.Caption);
        //        }

        //        ///Create Columns Object and Select those Columns only
        //        for (var i = 0; i < Columns.Count; i++)
        //        {

        //            worksheet.Cells[1, i + 1] = Columns[i];
        //            range = (Microsoft.Office.Interop.Excel.Range)worksheet.Cells[1, i + 1];
        //            range.Interior.ColorIndex = 20;
        //            range.Font.Bold = true;
        //            range.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeLeft].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
        //        }

        //        ///Now fill in the Rows to Excel
        //        for (var r = 0; r < table.Rows.Count; r++)
        //        {
        //            for (var i = 0; i < Columns.Count; i++)
        //            {
        //                if (r % 2 == 0)
        //                {
        //                    range = (Microsoft.Office.Interop.Excel.Range)worksheet.Cells[r + 2, i + 1];
        //                    range.Interior.ColorIndex = 15;
        //                    range.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeLeft].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
        //                    range.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
        //                    range.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeRight].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
        //                }
        //                else
        //                {
        //                    range = (Microsoft.Office.Interop.Excel.Range)worksheet.Cells[r + 2, i + 1];
        //                    range.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeLeft].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
        //                    range.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
        //                    range.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeRight].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
        //                }

        //                if (table.Columns[i].DataType.Name.ToLower() == "decimal")
        //                {
        //                    worksheet.Cells[1, i + 1].EntireColumn.NumberFormat = "#,###;(#,###);-";
        //                    worksheet.Cells[r + 2, i + 1] = table.Rows[r][i];
        //                }
        //                else
        //                if (table.Columns[i].DataType.Name.ToLower() == "datetime")
        //                {
        //                    worksheet.Cells[1, i + 1].EntireColumn.NumberFormat = "dd/MM/yyyy";
        //                    worksheet.Cells[r + 2, i + 1] = table.Rows[r][i];
        //                }
        //                else
        //                {
        //                    worksheet.Cells[r + 2, i + 1] = table.Rows[r][i];
        //                }
        //            }

        //            rowRead++;
        //        }

        //        Microsoft.Office.Interop.Excel.Range columns = worksheet.UsedRange.Columns;
        //        columns.AutoFit();

        //        ///Display the Excel Component to User
        //        xlApp.Visible = true;
        //    }
        //    catch (Exception ex)
        //    {
        //        System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;
        //        System.Windows.Forms.MessageBox.Show("Exception occured while exporting rows to Excel. Exception Details : " + ex.Message.ToString(), "Excel Export", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);
        //    }
        //    finally
        //    {
        //        ///Force Clean up the Com Component to remove any traces
        //        GC.Collect();
        //        GC.WaitForPendingFinalizers();
        //        foreach (Microsoft.Office.Interop.Excel.Workbook wb in xlApp.Workbooks)
        //        {
        //            foreach (Microsoft.Office.Interop.Excel.Worksheet ws in wb.Worksheets)
        //            {
        //                System.Runtime.InteropServices.Marshal.FinalReleaseComObject(ws);
        //            }
        //            //wb.Close(false, Type.Missing, Type.Missing);

        //            System.Runtime.InteropServices.Marshal.FinalReleaseComObject(wb);
        //        }
        //        xlApp.Quit();
        //        System.Runtime.InteropServices.Marshal.FinalReleaseComObject(xlApp);

        //        System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;
        //    }
        //}

        public static string DocTien(decimal NumCurrency)
        {
            string SoRaChu = "";
            decimal _soAm = NumCurrency;
            if (NumCurrency == 0)
            {
                SoRaChu = "Không đồng";
                return SoRaChu;
            }
            if (NumCurrency < 0)
                NumCurrency = Math.Abs(NumCurrency);

            string[] CharVND = new string[10] { "không", "một", "hai", "ba", "bốn", "năm", "sáu", "bảy", "tám", "chín" };
            string BangChu;
            int I;
            //As String, BangChu As String, I As Integer
            int SoLe, SoDoi;
            string PhanChan, Ten;
            string DonViTien, DonViLe;
            int NganTy, Ty, Trieu, Ngan;
            int Dong, Tram, Muoi, DonVi;

            SoDoi = 0;
            Muoi = 0;
            Tram = 0;
            DonVi = 0;

            Ten = "";
            //Dim SoLe, SoDoi As Integer, PhanChan, Ten As String
            //Dim DonViTien As String, DonViLe As String
            //Dim NganTy As Integer, Ty As Integer, Trieu As Integer, Ngan As Integer
            //Dim Dong As Integer, Tram As Integer, Muoi As Integer, DonVi As Integer

            DonViTien = "đồng";
            DonViLe = "xu";


            SoLe = (int)((NumCurrency - (int)NumCurrency) * 100); //'2 kí so^' le?
            PhanChan = ((int)NumCurrency).ToString().Trim();

            int khoangtrang = 15 - PhanChan.Length;
            for (int i = 0; i < khoangtrang; i++)
                PhanChan = "0" + PhanChan;
            //PhanChan = Space(15 - PhanChan.Length) + PhanChan;

            NganTy = int.Parse(PhanChan.Substring(0, 3));
            Ty = int.Parse(PhanChan.Substring(3, 3));
            Trieu = int.Parse(PhanChan.Substring(6, 3));
            Ngan = int.Parse(PhanChan.Substring(9, 3));
            Dong = int.Parse(PhanChan.Substring(12, 3));
            //Ty = Val(Mid$(PhanChan, 4, 3))
            //Trieu = Val(Mid$(PhanChan, 7, 3))
            //Ngan = Val(Mid$(PhanChan, 10, 3))
            //Dong = Val(Mid$(PhanChan, 13, 3))

            if (NganTy == 0 & Ty == 0 & Trieu == 0 & Ngan == 0 & Dong == 0)
            {
                BangChu = "không " + DonViTien + " ";
                I = 5;
            }
            else
            {
                BangChu = "";
                I = 0;
            }

            while (I <= 5)
            {
                switch (I)
                {
                    case 0:
                        SoDoi = NganTy;
                        Ten = "ngàn";
                        break;
                    case 1:
                        SoDoi = Ty;
                        Ten = "tỷ";
                        break;
                    case 2:
                        SoDoi = Trieu;
                        Ten = "triệu";
                        break;
                    case 3:
                        SoDoi = Ngan;
                        Ten = "ngàn";
                        break;
                    case 4:
                        SoDoi = Dong;
                        Ten = DonViTien;
                        break;
                    case 5:
                        SoDoi = SoLe;
                        Ten = DonViLe;
                        break;
                }

                if (SoDoi != 0)
                {
                    Tram = (int)(SoDoi / 100);
                    Muoi = (int)((SoDoi - Tram * 100) / 10);
                    DonVi = (SoDoi - Tram * 100) - Muoi * 10;
                    BangChu = BangChu.Trim() + (BangChu.Length == 0 ? "" : " ") + (Tram != 0 ? CharVND[Tram].Trim() + " trăm " : "");
                    if (Muoi == 0 & Tram != 0 & DonVi != 0)
                        BangChu = BangChu + "lẻ ";
                    else if (Muoi != 0)
                        BangChu = BangChu + ((Muoi != 0 & Muoi != 1) ? CharVND[Muoi].Trim() + " mươi " : "mười ");

                    if (Muoi != 0 & DonVi == 5)
                        BangChu = BangChu + "lăm " + Ten + " ";
                    else if (Muoi > 1 & DonVi == 1)
                        BangChu = BangChu + "mốt " + Ten + " ";
                    else
                        BangChu = BangChu + ((DonVi != 0) ? CharVND[DonVi].Trim() + " " + Ten + " " : Ten + " ");
                }
                else
                    BangChu = BangChu + ((I == 4) ? DonViTien + " " : "");

                I = I + 1;
            }
            if (SoLe == 0)
                //BangChu = BangChu + "chẵn";
                BangChu = BangChu + "";
            BangChu = BangChu[0].ToString().ToUpper() + BangChu.Substring(1);
            SoRaChu = BangChu;

            if (_soAm < 0)
            {
                SoRaChu = "Âm " + SoRaChu.ToLower();
            }

            return SoRaChu;
        }

        // dieu modify (06/01/2017) add function 

        #region Đổ dữ liệu từ file csv vào DataTable
        /// <summary>
        /// Đổ dữ liệu từ file csv vào DataTable
        /// </summary>
        public static DataTable GetDataTableFromCSVFile(string csv_file_path)
        {
            DataTable csvData = new DataTable();
            try
            {
                using (Microsoft.VisualBasic.FileIO.TextFieldParser csvReader = new Microsoft.VisualBasic.FileIO.TextFieldParser(csv_file_path))
                {
                    csvReader.SetDelimiters(new string[] { "," });
                    csvReader.HasFieldsEnclosedInQuotes = true;
                    string[] colFields = csvReader.ReadFields();

                    foreach (string column in colFields)
                    {
                        DataColumn datecolumn = new DataColumn(column);
                        datecolumn.AllowDBNull = true;
                        csvData.Columns.Add(datecolumn);
                    }

                    while (!csvReader.EndOfData)
                    {
                        string[] fieldData = csvReader.ReadFields();
                        //Making empty value as null
                        for (int i = 0; i < fieldData.Length; i++)
                        {
                            if (fieldData[i] == "")
                            {
                                fieldData[i] = null;
                            }
                        }
                        csvData.Rows.Add(fieldData);
                    }
                }
            }
            catch (Exception ex) { }

            return csvData;
        }
        #endregion

        // Cắt dấu phẩy ngăn cách tiền tệ
        public static decimal SplitSeparatorInMoney(string money)
        {
            try
            {
                if (money.ToLower().Contains(@","))
                {
                    money = money.Replace(@",", "");
                }
                return DBConvert.ParseDecimal(money);
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        /// <summary>
        /// Convert excel to datatable
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static System.Data.DataTable ConvertExcelToDataTable(string path)
        {
            System.Data.DataTable table = null;
            try
            {
                object rowIndex = 1;
                table = new System.Data.DataTable();
                DataRow row;
                Microsoft.Office.Interop.Excel.Application app = new Microsoft.Office.Interop.Excel.Application();
                Microsoft.Office.Interop.Excel.Workbooks workBooks = app.Workbooks;
                Microsoft.Office.Interop.Excel.Workbook workBook = workBooks.Open(path, 0, true, 5, "", "", true,
                    Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "\t", false, false, 0, true, 1, 0);
                Microsoft.Office.Interop.Excel.Worksheet workSheet = (Microsoft.Office.Interop.Excel.Worksheet)workBook.ActiveSheet;
                int temp = 1;
                while (((Microsoft.Office.Interop.Excel.Range)workSheet.Cells[rowIndex, temp]).Value2 != null)
                {
                    table.Columns.Add(Convert.ToString(((Microsoft.Office.Interop.Excel.Range)workSheet.Cells[rowIndex, temp]).Value2));
                    temp++;
                }
                rowIndex = Convert.ToInt32(rowIndex) + 1;
                int columnCount = temp;
                temp = 1;
                while (((Microsoft.Office.Interop.Excel.Range)workSheet.Cells[rowIndex, temp]).Value2 != null)
                {
                    row = table.NewRow();
                    for (int i = 1; i < columnCount; i++)
                    {
                        row[i - 1] = Convert.ToString(((Microsoft.Office.Interop.Excel.Range)workSheet.Cells[rowIndex, i]).Value2);
                    }
                    table.Rows.Add(row);
                    rowIndex = Convert.ToInt32(rowIndex) + 1;
                    temp = 1;
                }
                GC.Collect();
                GC.WaitForPendingFinalizers();

                workBook.Close(0);
                workBooks.Close();
                app.Quit();

                System.Runtime.InteropServices.Marshal.FinalReleaseComObject(workBook);
                System.Runtime.InteropServices.Marshal.FinalReleaseComObject(workBooks);
                System.Runtime.InteropServices.Marshal.FinalReleaseComObject(app);
            }
            catch (Exception ex)
            {
                return new DataTable();
            }
            return table;
        }

        /// <summary>
        /// đọc dữ liệu trong file
        /// </summary>
        public static byte[] GetFile(string fullPath)
        {
            System.IO.FileStream fs = System.IO.File.OpenRead(fullPath);
            byte[] data = new byte[fs.Length];
            int br = fs.Read(data, 0, data.Length);
            if (br != fs.Length)
                throw new System.IO.IOException(fullPath);
            return data;
        }

        /// <summary>
        /// convert chuổi Json sang datatable 
        /// </summary>
        /// <param name="jsonString"></param>
        /// <returns></returns>
        public static DataTable JsonStringToDataTable(string jsonString)
        {
            try
            {
                DataTable dt = new DataTable();
                string[] jsonStringArray = Regex.Split(jsonString.Replace("[", "").Replace("]", ""), "},{");
                List<string> ColumnsName = new List<string>();
                foreach (string jSA in jsonStringArray)
                {
                    string[] jsonStringData = Regex.Split(jSA.Replace("{", "").Replace("}", ""), "\",\"");
                    foreach (string ColumnsNameData in jsonStringData)
                    {
                        try
                        {
                            if (ColumnsNameData != "null")
                            {
                                int idx = ColumnsNameData.IndexOf(":");
                                string ColumnsNameString = ColumnsNameData.Substring(0, idx - 1).Replace("\"", "");
                                if (!ColumnsName.Contains(ColumnsNameString))
                                {
                                    ColumnsName.Add(ColumnsNameString);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            throw new Exception(string.Format("Error parsing column name : {0}", ColumnsNameData));
                        }
                    }
                    break;
                }
                foreach (string AddColumnName in ColumnsName)
                {
                    dt.Columns.Add(AddColumnName);
                }
                foreach (string jSA in jsonStringArray)
                {
                    string[] RowData = Regex.Split(jSA.Replace("{", "").Replace("}", ""), "\",\"");
                    DataRow nr = dt.NewRow();
                    foreach (string rowData in RowData)
                    {
                        try
                        {
                            int idx = rowData.IndexOf(":");
                            string RowColumns = rowData.Substring(0, idx - 1).Replace("\"", "");
                            string RowDataString = rowData.Substring(idx + 1).Replace("\"", "");
                            nr[RowColumns] = RowDataString;
                        }
                        catch (Exception ex)
                        {
                            continue;
                        }
                    }
                    dt.Rows.Add(nr);
                }
                return dt;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        #region =========== XỬ LÝ CHUỖI (TOANDV - 10/03/2017) =================

        private static Hashtable mCharacterTable;
        private static void InitializeCharacterTable()
        {
            mCharacterTable = new Hashtable();
            mCharacterTable.Add(ToUnichar("0041"), "A");
            mCharacterTable.Add(ToUnichar("0042"), "B");
            mCharacterTable.Add(ToUnichar("0043"), "C");
            mCharacterTable.Add(ToUnichar("0044"), "D");
            mCharacterTable.Add(ToUnichar("0045"), "E");
            mCharacterTable.Add(ToUnichar("0046"), "F");
            mCharacterTable.Add(ToUnichar("0047"), "G");
            mCharacterTable.Add(ToUnichar("0048"), "H");
            mCharacterTable.Add(ToUnichar("0049"), "I");
            mCharacterTable.Add(ToUnichar("004A"), "J");
            mCharacterTable.Add(ToUnichar("004B"), "K");
            mCharacterTable.Add(ToUnichar("004C"), "L");
            mCharacterTable.Add(ToUnichar("004D"), "M");
            mCharacterTable.Add(ToUnichar("004E"), "N");
            mCharacterTable.Add(ToUnichar("004F"), "O");
            mCharacterTable.Add(ToUnichar("0050"), "P");
            mCharacterTable.Add(ToUnichar("0051"), "Q");
            mCharacterTable.Add(ToUnichar("0052"), "R");
            mCharacterTable.Add(ToUnichar("0053"), "S");
            mCharacterTable.Add(ToUnichar("0054"), "T");
            mCharacterTable.Add(ToUnichar("0055"), "U");
            mCharacterTable.Add(ToUnichar("0056"), "V");
            mCharacterTable.Add(ToUnichar("0057"), "W");
            mCharacterTable.Add(ToUnichar("0058"), "X");
            mCharacterTable.Add(ToUnichar("0059"), "Y");
            mCharacterTable.Add(ToUnichar("005A"), "Z");
            mCharacterTable.Add(ToUnichar("0061"), "a");
            mCharacterTable.Add(ToUnichar("0062"), "b");
            mCharacterTable.Add(ToUnichar("0063"), "c");
            mCharacterTable.Add(ToUnichar("0064"), "d");
            mCharacterTable.Add(ToUnichar("0065"), "e");
            mCharacterTable.Add(ToUnichar("0066"), "f");
            mCharacterTable.Add(ToUnichar("0067"), "g");
            mCharacterTable.Add(ToUnichar("0068"), "h");
            mCharacterTable.Add(ToUnichar("0069"), "i");
            mCharacterTable.Add(ToUnichar("006A"), "j");
            mCharacterTable.Add(ToUnichar("006B"), "k");
            mCharacterTable.Add(ToUnichar("006C"), "l");
            mCharacterTable.Add(ToUnichar("006D"), "m");
            mCharacterTable.Add(ToUnichar("006E"), "n");
            mCharacterTable.Add(ToUnichar("006F"), "o");
            mCharacterTable.Add(ToUnichar("0070"), "p");
            mCharacterTable.Add(ToUnichar("0071"), "q");
            mCharacterTable.Add(ToUnichar("0072"), "r");
            mCharacterTable.Add(ToUnichar("0073"), "s");
            mCharacterTable.Add(ToUnichar("0074"), "t");
            mCharacterTable.Add(ToUnichar("0075"), "u");
            mCharacterTable.Add(ToUnichar("0076"), "v");
            mCharacterTable.Add(ToUnichar("0077"), "w");
            mCharacterTable.Add(ToUnichar("0078"), "x");
            mCharacterTable.Add(ToUnichar("0079"), "y");
            mCharacterTable.Add(ToUnichar("007A"), "z");
            mCharacterTable.Add(ToUnichar("00AA"), "a");
            mCharacterTable.Add(ToUnichar("00BA"), "o");
            mCharacterTable.Add(ToUnichar("00C0"), "A");
            mCharacterTable.Add(ToUnichar("00C1"), "A");
            mCharacterTable.Add(ToUnichar("00C2"), "A");
            mCharacterTable.Add(ToUnichar("00C3"), "A");
            mCharacterTable.Add(ToUnichar("00C4"), "A");
            mCharacterTable.Add(ToUnichar("00C5"), "A");
            mCharacterTable.Add(ToUnichar("00C6"), "AE");
            mCharacterTable.Add(ToUnichar("00C7"), "C");
            mCharacterTable.Add(ToUnichar("00C8"), "E");
            mCharacterTable.Add(ToUnichar("00C9"), "E");
            mCharacterTable.Add(ToUnichar("00CA"), "E");
            mCharacterTable.Add(ToUnichar("00CB"), "E");
            mCharacterTable.Add(ToUnichar("00CC"), "I");
            mCharacterTable.Add(ToUnichar("00CD"), "I");
            mCharacterTable.Add(ToUnichar("00CE"), "I");
            mCharacterTable.Add(ToUnichar("00CF"), "I");
            mCharacterTable.Add(ToUnichar("00D0"), "D");
            mCharacterTable.Add(ToUnichar("00D1"), "N");
            mCharacterTable.Add(ToUnichar("00D2"), "O");
            mCharacterTable.Add(ToUnichar("00D3"), "O");
            mCharacterTable.Add(ToUnichar("00D4"), "O");
            mCharacterTable.Add(ToUnichar("00D5"), "O");
            mCharacterTable.Add(ToUnichar("00D6"), "O");
            mCharacterTable.Add(ToUnichar("00D8"), "O");
            mCharacterTable.Add(ToUnichar("00D9"), "U");
            mCharacterTable.Add(ToUnichar("00DA"), "U");
            mCharacterTable.Add(ToUnichar("00DB"), "U");
            mCharacterTable.Add(ToUnichar("00DC"), "U");
            mCharacterTable.Add(ToUnichar("00DD"), "Y");
            mCharacterTable.Add(ToUnichar("00DE"), "Th");
            mCharacterTable.Add(ToUnichar("00DF"), "s");
            mCharacterTable.Add(ToUnichar("00E0"), "a");
            mCharacterTable.Add(ToUnichar("00E1"), "a");
            mCharacterTable.Add(ToUnichar("00E2"), "a");
            mCharacterTable.Add(ToUnichar("00E3"), "a");
            mCharacterTable.Add(ToUnichar("00E4"), "a");
            mCharacterTable.Add(ToUnichar("00E5"), "a");
            mCharacterTable.Add(ToUnichar("00E6"), "ae");
            mCharacterTable.Add(ToUnichar("00E7"), "c");
            mCharacterTable.Add(ToUnichar("00E8"), "e");
            mCharacterTable.Add(ToUnichar("00E9"), "e");
            mCharacterTable.Add(ToUnichar("00EA"), "e");
            mCharacterTable.Add(ToUnichar("00EB"), "e");
            mCharacterTable.Add(ToUnichar("00EC"), "i");
            mCharacterTable.Add(ToUnichar("00ED"), "i");
            mCharacterTable.Add(ToUnichar("00EE"), "i");
            mCharacterTable.Add(ToUnichar("00EF"), "i");
            mCharacterTable.Add(ToUnichar("00F0"), "d");
            mCharacterTable.Add(ToUnichar("00F1"), "n");
            mCharacterTable.Add(ToUnichar("00F2"), "o");
            mCharacterTable.Add(ToUnichar("00F3"), "o");
            mCharacterTable.Add(ToUnichar("00F4"), "o");
            mCharacterTable.Add(ToUnichar("00F5"), "o");
            mCharacterTable.Add(ToUnichar("00F6"), "o");
            mCharacterTable.Add(ToUnichar("00F8"), "o");
            mCharacterTable.Add(ToUnichar("00F9"), "u");
            mCharacterTable.Add(ToUnichar("00FA"), "u");
            mCharacterTable.Add(ToUnichar("00FB"), "u");
            mCharacterTable.Add(ToUnichar("00FC"), "u");
            mCharacterTable.Add(ToUnichar("00FD"), "y");
            mCharacterTable.Add(ToUnichar("00FE"), "th");
            mCharacterTable.Add(ToUnichar("00FF"), "y");
            mCharacterTable.Add(ToUnichar("0100"), "A");
            mCharacterTable.Add(ToUnichar("0101"), "a");
            mCharacterTable.Add(ToUnichar("0102"), "A");
            mCharacterTable.Add(ToUnichar("0103"), "a");
            mCharacterTable.Add(ToUnichar("0104"), "A");
            mCharacterTable.Add(ToUnichar("0105"), "a");
            mCharacterTable.Add(ToUnichar("0106"), "C");
            mCharacterTable.Add(ToUnichar("0107"), "c");
            mCharacterTable.Add(ToUnichar("0108"), "C");
            mCharacterTable.Add(ToUnichar("0109"), "c");
            mCharacterTable.Add(ToUnichar("010A"), "C");
            mCharacterTable.Add(ToUnichar("010B"), "c");
            mCharacterTable.Add(ToUnichar("010C"), "C");
            mCharacterTable.Add(ToUnichar("010D"), "c");
            mCharacterTable.Add(ToUnichar("010E"), "D");
            mCharacterTable.Add(ToUnichar("010F"), "d");
            mCharacterTable.Add(ToUnichar("0110"), "D");
            mCharacterTable.Add(ToUnichar("0111"), "d");
            mCharacterTable.Add(ToUnichar("0112"), "E");
            mCharacterTable.Add(ToUnichar("0113"), "e");
            mCharacterTable.Add(ToUnichar("0114"), "E");
            mCharacterTable.Add(ToUnichar("0115"), "e");
            mCharacterTable.Add(ToUnichar("0116"), "E");
            mCharacterTable.Add(ToUnichar("0117"), "e");
            mCharacterTable.Add(ToUnichar("0118"), "E");
            mCharacterTable.Add(ToUnichar("0119"), "e");
            mCharacterTable.Add(ToUnichar("011A"), "E");
            mCharacterTable.Add(ToUnichar("011B"), "e");
            mCharacterTable.Add(ToUnichar("011C"), "G");
            mCharacterTable.Add(ToUnichar("011D"), "g");
            mCharacterTable.Add(ToUnichar("011E"), "G");
            mCharacterTable.Add(ToUnichar("011F"), "g");
            mCharacterTable.Add(ToUnichar("0120"), "G");
            mCharacterTable.Add(ToUnichar("0121"), "g");
            mCharacterTable.Add(ToUnichar("0122"), "G");
            mCharacterTable.Add(ToUnichar("0123"), "g");
            mCharacterTable.Add(ToUnichar("0124"), "H");
            mCharacterTable.Add(ToUnichar("0125"), "h");
            mCharacterTable.Add(ToUnichar("0126"), "H");
            mCharacterTable.Add(ToUnichar("0127"), "h");
            mCharacterTable.Add(ToUnichar("0128"), "I");
            mCharacterTable.Add(ToUnichar("0129"), "i");
            mCharacterTable.Add(ToUnichar("012A"), "I");
            mCharacterTable.Add(ToUnichar("012B"), "i");
            mCharacterTable.Add(ToUnichar("012C"), "I");
            mCharacterTable.Add(ToUnichar("012D"), "i");
            mCharacterTable.Add(ToUnichar("012E"), "I");
            mCharacterTable.Add(ToUnichar("012F"), "i");
            mCharacterTable.Add(ToUnichar("0130"), "I");
            mCharacterTable.Add(ToUnichar("0131"), "i");
            mCharacterTable.Add(ToUnichar("0132"), "I");
            mCharacterTable.Add(ToUnichar("0133"), "i");
            mCharacterTable.Add(ToUnichar("0134"), "J");
            mCharacterTable.Add(ToUnichar("0135"), "j");
            mCharacterTable.Add(ToUnichar("0136"), "K");
            mCharacterTable.Add(ToUnichar("0137"), "k");
            mCharacterTable.Add(ToUnichar("0138"), "k");
            mCharacterTable.Add(ToUnichar("0139"), "L");
            mCharacterTable.Add(ToUnichar("013A"), "l");
            mCharacterTable.Add(ToUnichar("013B"), "L");
            mCharacterTable.Add(ToUnichar("013C"), "l");
            mCharacterTable.Add(ToUnichar("013D"), "L");
            mCharacterTable.Add(ToUnichar("013E"), "l");
            mCharacterTable.Add(ToUnichar("013F"), "L");
            mCharacterTable.Add(ToUnichar("0140"), "l");
            mCharacterTable.Add(ToUnichar("0141"), "L");
            mCharacterTable.Add(ToUnichar("0142"), "l");
            mCharacterTable.Add(ToUnichar("0143"), "N");
            mCharacterTable.Add(ToUnichar("0144"), "n");
            mCharacterTable.Add(ToUnichar("0145"), "N");
            mCharacterTable.Add(ToUnichar("0146"), "n");
            mCharacterTable.Add(ToUnichar("0147"), "N");
            mCharacterTable.Add(ToUnichar("0148"), "n");
            mCharacterTable.Add(ToUnichar("0149"), "'n");
            mCharacterTable.Add(ToUnichar("014A"), "NG");
            mCharacterTable.Add(ToUnichar("014B"), "ng");
            mCharacterTable.Add(ToUnichar("014C"), "O");
            mCharacterTable.Add(ToUnichar("014D"), "o");
            mCharacterTable.Add(ToUnichar("014E"), "O");
            mCharacterTable.Add(ToUnichar("014F"), "o");
            mCharacterTable.Add(ToUnichar("0150"), "O");
            mCharacterTable.Add(ToUnichar("0151"), "o");
            mCharacterTable.Add(ToUnichar("0152"), "OE");
            mCharacterTable.Add(ToUnichar("0153"), "oe");
            mCharacterTable.Add(ToUnichar("0154"), "R");
            mCharacterTable.Add(ToUnichar("0155"), "r");
            mCharacterTable.Add(ToUnichar("0156"), "R");
            mCharacterTable.Add(ToUnichar("0157"), "r");
            mCharacterTable.Add(ToUnichar("0158"), "R");
            mCharacterTable.Add(ToUnichar("0159"), "r");
            mCharacterTable.Add(ToUnichar("015A"), "S");
            mCharacterTable.Add(ToUnichar("015B"), "s");
            mCharacterTable.Add(ToUnichar("015C"), "S");
            mCharacterTable.Add(ToUnichar("015D"), "s");
            mCharacterTable.Add(ToUnichar("015E"), "S");
            mCharacterTable.Add(ToUnichar("015F"), "s");
            mCharacterTable.Add(ToUnichar("0160"), "S");
            mCharacterTable.Add(ToUnichar("0161"), "s");
            mCharacterTable.Add(ToUnichar("0162"), "T");
            mCharacterTable.Add(ToUnichar("0163"), "t");
            mCharacterTable.Add(ToUnichar("0164"), "T");
            mCharacterTable.Add(ToUnichar("0165"), "t");
            mCharacterTable.Add(ToUnichar("0166"), "T");
            mCharacterTable.Add(ToUnichar("0167"), "t");
            mCharacterTable.Add(ToUnichar("0168"), "U");
            mCharacterTable.Add(ToUnichar("0169"), "u");
            mCharacterTable.Add(ToUnichar("016A"), "U");
            mCharacterTable.Add(ToUnichar("016B"), "u");
            mCharacterTable.Add(ToUnichar("016C"), "U");
            mCharacterTable.Add(ToUnichar("016D"), "u");
            mCharacterTable.Add(ToUnichar("016E"), "U");
            mCharacterTable.Add(ToUnichar("016F"), "u");
            mCharacterTable.Add(ToUnichar("0170"), "U");
            mCharacterTable.Add(ToUnichar("0171"), "u");
            mCharacterTable.Add(ToUnichar("0172"), "U");
            mCharacterTable.Add(ToUnichar("0173"), "u");
            mCharacterTable.Add(ToUnichar("0174"), "W");
            mCharacterTable.Add(ToUnichar("0175"), "w");
            mCharacterTable.Add(ToUnichar("0176"), "Y");
            mCharacterTable.Add(ToUnichar("0177"), "y");
            mCharacterTable.Add(ToUnichar("0178"), "Y");
            mCharacterTable.Add(ToUnichar("0179"), "Z");
            mCharacterTable.Add(ToUnichar("017A"), "z");
            mCharacterTable.Add(ToUnichar("017B"), "Z");
            mCharacterTable.Add(ToUnichar("017C"), "z");
            mCharacterTable.Add(ToUnichar("017D"), "Z");
            mCharacterTable.Add(ToUnichar("017E"), "z");
            mCharacterTable.Add(ToUnichar("017F"), "s");
            mCharacterTable.Add(ToUnichar("0180"), "b");
            mCharacterTable.Add(ToUnichar("0181"), "B");
            mCharacterTable.Add(ToUnichar("0182"), "B");
            mCharacterTable.Add(ToUnichar("0183"), "b");
            mCharacterTable.Add(ToUnichar("0184"), "6");
            mCharacterTable.Add(ToUnichar("0185"), "6");
            mCharacterTable.Add(ToUnichar("0186"), "O");
            mCharacterTable.Add(ToUnichar("0187"), "C");
            mCharacterTable.Add(ToUnichar("0188"), "c");
            mCharacterTable.Add(ToUnichar("0189"), "D");
            mCharacterTable.Add(ToUnichar("018A"), "D");
            mCharacterTable.Add(ToUnichar("018B"), "D");
            mCharacterTable.Add(ToUnichar("018C"), "d");
            mCharacterTable.Add(ToUnichar("018D"), "d");
            mCharacterTable.Add(ToUnichar("018E"), "E");
            mCharacterTable.Add(ToUnichar("018F"), "E");
            mCharacterTable.Add(ToUnichar("0190"), "E");
            mCharacterTable.Add(ToUnichar("0191"), "F");
            mCharacterTable.Add(ToUnichar("0192"), "f");
            mCharacterTable.Add(ToUnichar("0193"), "G");
            mCharacterTable.Add(ToUnichar("0194"), "G");
            mCharacterTable.Add(ToUnichar("0195"), "hv");
            mCharacterTable.Add(ToUnichar("0196"), "I");
            mCharacterTable.Add(ToUnichar("0197"), "I");
            mCharacterTable.Add(ToUnichar("0198"), "K");
            mCharacterTable.Add(ToUnichar("0199"), "k");
            mCharacterTable.Add(ToUnichar("019A"), "l");
            mCharacterTable.Add(ToUnichar("019B"), "l");
            mCharacterTable.Add(ToUnichar("019C"), "M");
            mCharacterTable.Add(ToUnichar("019D"), "N");
            mCharacterTable.Add(ToUnichar("019E"), "n");
            mCharacterTable.Add(ToUnichar("019F"), "O");
            mCharacterTable.Add(ToUnichar("01A0"), "O");
            mCharacterTable.Add(ToUnichar("01A1"), "o");
            mCharacterTable.Add(ToUnichar("01A2"), "OI");
            mCharacterTable.Add(ToUnichar("01A3"), "oi");
            mCharacterTable.Add(ToUnichar("01A4"), "P");
            mCharacterTable.Add(ToUnichar("01A5"), "p");
            mCharacterTable.Add(ToUnichar("01A6"), "YR");
            mCharacterTable.Add(ToUnichar("01A7"), "2");
            mCharacterTable.Add(ToUnichar("01A8"), "2");
            mCharacterTable.Add(ToUnichar("01A9"), "S");
            mCharacterTable.Add(ToUnichar("01AA"), "s");
            mCharacterTable.Add(ToUnichar("01AB"), "t");
            mCharacterTable.Add(ToUnichar("01AC"), "T");
            mCharacterTable.Add(ToUnichar("01AD"), "t");
            mCharacterTable.Add(ToUnichar("01AE"), "T");
            mCharacterTable.Add(ToUnichar("01AF"), "U");
            mCharacterTable.Add(ToUnichar("01B0"), "u");
            mCharacterTable.Add(ToUnichar("01B1"), "u");
            mCharacterTable.Add(ToUnichar("01B2"), "V");
            mCharacterTable.Add(ToUnichar("01B3"), "Y");
            mCharacterTable.Add(ToUnichar("01B4"), "y");
            mCharacterTable.Add(ToUnichar("01B5"), "Z");
            mCharacterTable.Add(ToUnichar("01B6"), "z");
            mCharacterTable.Add(ToUnichar("01B7"), "Z");
            mCharacterTable.Add(ToUnichar("01B8"), "Z");
            mCharacterTable.Add(ToUnichar("01B9"), "Z");
            mCharacterTable.Add(ToUnichar("01BA"), "z");
            mCharacterTable.Add(ToUnichar("01BB"), "2");
            mCharacterTable.Add(ToUnichar("01BC"), "5");
            mCharacterTable.Add(ToUnichar("01BD"), "5");
            mCharacterTable.Add(ToUnichar("01BE"), "\x00b4");
            mCharacterTable.Add(ToUnichar("01BF"), "w");
            mCharacterTable.Add(ToUnichar("01C0"), "!");
            mCharacterTable.Add(ToUnichar("01C1"), "!");
            mCharacterTable.Add(ToUnichar("01C2"), "!");
            mCharacterTable.Add(ToUnichar("01C3"), "!");
            mCharacterTable.Add(ToUnichar("01C4"), "DZ");
            mCharacterTable.Add(ToUnichar("01C5"), "DZ");
            mCharacterTable.Add(ToUnichar("01C6"), "d");
            mCharacterTable.Add(ToUnichar("01C7"), "Lj");
            mCharacterTable.Add(ToUnichar("01C8"), "Lj");
            mCharacterTable.Add(ToUnichar("01C9"), "lj");
            mCharacterTable.Add(ToUnichar("01CA"), "NJ");
            mCharacterTable.Add(ToUnichar("01CB"), "NJ");
            mCharacterTable.Add(ToUnichar("01CC"), "nj");
            mCharacterTable.Add(ToUnichar("01CD"), "A");
            mCharacterTable.Add(ToUnichar("01CE"), "a");
            mCharacterTable.Add(ToUnichar("01CF"), "I");
            mCharacterTable.Add(ToUnichar("01D0"), "i");
            mCharacterTable.Add(ToUnichar("01D1"), "O");
            mCharacterTable.Add(ToUnichar("01D2"), "o");
            mCharacterTable.Add(ToUnichar("01D3"), "U");
            mCharacterTable.Add(ToUnichar("01D4"), "u");
            mCharacterTable.Add(ToUnichar("01D5"), "U");
            mCharacterTable.Add(ToUnichar("01D6"), "u");
            mCharacterTable.Add(ToUnichar("01D7"), "U");
            mCharacterTable.Add(ToUnichar("01D8"), "u");
            mCharacterTable.Add(ToUnichar("01D9"), "U");
            mCharacterTable.Add(ToUnichar("01DA"), "u");
            mCharacterTable.Add(ToUnichar("01DB"), "U");
            mCharacterTable.Add(ToUnichar("01DC"), "u");
            mCharacterTable.Add(ToUnichar("01DD"), "e");
            mCharacterTable.Add(ToUnichar("01DE"), "A");
            mCharacterTable.Add(ToUnichar("01DF"), "a");
            mCharacterTable.Add(ToUnichar("01E0"), "A");
            mCharacterTable.Add(ToUnichar("01E1"), "a");
            mCharacterTable.Add(ToUnichar("01E2"), "AE");
            mCharacterTable.Add(ToUnichar("01E3"), "ae");
            mCharacterTable.Add(ToUnichar("01E4"), "G");
            mCharacterTable.Add(ToUnichar("01E5"), "g");
            mCharacterTable.Add(ToUnichar("01E6"), "G");
            mCharacterTable.Add(ToUnichar("01E7"), "g");
            mCharacterTable.Add(ToUnichar("01E8"), "K");
            mCharacterTable.Add(ToUnichar("01E9"), "k");
            mCharacterTable.Add(ToUnichar("01EA"), "O");
            mCharacterTable.Add(ToUnichar("01EB"), "o");
            mCharacterTable.Add(ToUnichar("01EC"), "O");
            mCharacterTable.Add(ToUnichar("01ED"), "o");
            mCharacterTable.Add(ToUnichar("01EE"), "Z");
            mCharacterTable.Add(ToUnichar("01EF"), "Z");
            mCharacterTable.Add(ToUnichar("01F0"), "j");
            mCharacterTable.Add(ToUnichar("01F1"), "DZ");
            mCharacterTable.Add(ToUnichar("01F2"), "DZ");
            mCharacterTable.Add(ToUnichar("01F3"), "dz");
            mCharacterTable.Add(ToUnichar("01F4"), "G");
            mCharacterTable.Add(ToUnichar("01F5"), "g");
            mCharacterTable.Add(ToUnichar("01F6"), "hv");
            mCharacterTable.Add(ToUnichar("01F7"), "w");
            mCharacterTable.Add(ToUnichar("01F8"), "N");
            mCharacterTable.Add(ToUnichar("01F9"), "n");
            mCharacterTable.Add(ToUnichar("01FA"), "A");
            mCharacterTable.Add(ToUnichar("01FB"), "a");
            mCharacterTable.Add(ToUnichar("01FC"), "AE");
            mCharacterTable.Add(ToUnichar("01FD"), "ae");
            mCharacterTable.Add(ToUnichar("01FE"), "O");
            mCharacterTable.Add(ToUnichar("01FF"), "o");
            mCharacterTable.Add(ToUnichar("0200"), "A");
            mCharacterTable.Add(ToUnichar("0201"), "a");
            mCharacterTable.Add(ToUnichar("0202"), "A");
            mCharacterTable.Add(ToUnichar("0203"), "a");
            mCharacterTable.Add(ToUnichar("0204"), "E");
            mCharacterTable.Add(ToUnichar("0205"), "e");
            mCharacterTable.Add(ToUnichar("0206"), "E");
            mCharacterTable.Add(ToUnichar("0207"), "e");
            mCharacterTable.Add(ToUnichar("0208"), "I");
            mCharacterTable.Add(ToUnichar("0209"), "i");
            mCharacterTable.Add(ToUnichar("020A"), "I");
            mCharacterTable.Add(ToUnichar("020B"), "i");
            mCharacterTable.Add(ToUnichar("020C"), "O");
            mCharacterTable.Add(ToUnichar("020D"), "o");
            mCharacterTable.Add(ToUnichar("020E"), "O");
            mCharacterTable.Add(ToUnichar("020F"), "o");
            mCharacterTable.Add(ToUnichar("0210"), "R");
            mCharacterTable.Add(ToUnichar("0211"), "r");
            mCharacterTable.Add(ToUnichar("0212"), "R");
            mCharacterTable.Add(ToUnichar("0213"), "r");
            mCharacterTable.Add(ToUnichar("0214"), "U");
            mCharacterTable.Add(ToUnichar("0215"), "u");
            mCharacterTable.Add(ToUnichar("0216"), "U");
            mCharacterTable.Add(ToUnichar("0217"), "u");
            mCharacterTable.Add(ToUnichar("0218"), "S");
            mCharacterTable.Add(ToUnichar("0219"), "s");
            mCharacterTable.Add(ToUnichar("021A"), "T");
            mCharacterTable.Add(ToUnichar("021B"), "t");
            mCharacterTable.Add(ToUnichar("021C"), "Z");
            mCharacterTable.Add(ToUnichar("021D"), "z");
            mCharacterTable.Add(ToUnichar("021E"), "H");
            mCharacterTable.Add(ToUnichar("021F"), "h");
            mCharacterTable.Add(ToUnichar("0220"), "N");
            mCharacterTable.Add(ToUnichar("0221"), "d");
            mCharacterTable.Add(ToUnichar("0222"), "OU");
            mCharacterTable.Add(ToUnichar("0223"), "ou");
            mCharacterTable.Add(ToUnichar("0224"), "Z");
            mCharacterTable.Add(ToUnichar("0225"), "z");
            mCharacterTable.Add(ToUnichar("0226"), "A");
            mCharacterTable.Add(ToUnichar("0227"), "a");
            mCharacterTable.Add(ToUnichar("0228"), "E");
            mCharacterTable.Add(ToUnichar("0229"), "e");
            mCharacterTable.Add(ToUnichar("022A"), "O");
            mCharacterTable.Add(ToUnichar("022B"), "o");
            mCharacterTable.Add(ToUnichar("022C"), "O");
            mCharacterTable.Add(ToUnichar("022D"), "o");
            mCharacterTable.Add(ToUnichar("022E"), "O");
            mCharacterTable.Add(ToUnichar("022F"), "o");
            mCharacterTable.Add(ToUnichar("0230"), "O");
            mCharacterTable.Add(ToUnichar("0231"), "o");
            mCharacterTable.Add(ToUnichar("0232"), "Y");
            mCharacterTable.Add(ToUnichar("0233"), "y");
            mCharacterTable.Add(ToUnichar("0234"), "l");
            mCharacterTable.Add(ToUnichar("0235"), "n");
            mCharacterTable.Add(ToUnichar("0236"), "t");
            mCharacterTable.Add(ToUnichar("0250"), "a");
            mCharacterTable.Add(ToUnichar("0251"), "a");
            mCharacterTable.Add(ToUnichar("0252"), "a");
            mCharacterTable.Add(ToUnichar("0253"), "b");
            mCharacterTable.Add(ToUnichar("0254"), "o");
            mCharacterTable.Add(ToUnichar("0255"), "c");
            mCharacterTable.Add(ToUnichar("0256"), "d");
            mCharacterTable.Add(ToUnichar("0257"), "d");
            mCharacterTable.Add(ToUnichar("0258"), "e");
            mCharacterTable.Add(ToUnichar("0259"), "e");
            mCharacterTable.Add(ToUnichar("025A"), "e");
            mCharacterTable.Add(ToUnichar("025B"), "e");
            mCharacterTable.Add(ToUnichar("025C"), "e");
            mCharacterTable.Add(ToUnichar("025D"), "e");
            mCharacterTable.Add(ToUnichar("025E"), "e");
            mCharacterTable.Add(ToUnichar("025F"), "j");
            mCharacterTable.Add(ToUnichar("0260"), "g");
            mCharacterTable.Add(ToUnichar("0261"), "g");
            mCharacterTable.Add(ToUnichar("0262"), "G");
            mCharacterTable.Add(ToUnichar("0263"), "g");
            mCharacterTable.Add(ToUnichar("0264"), "y");
            mCharacterTable.Add(ToUnichar("0265"), "h");
            mCharacterTable.Add(ToUnichar("0266"), "h");
            mCharacterTable.Add(ToUnichar("0267"), "h");
            mCharacterTable.Add(ToUnichar("0268"), "i");
            mCharacterTable.Add(ToUnichar("0269"), "i");
            mCharacterTable.Add(ToUnichar("026A"), "I");
            mCharacterTable.Add(ToUnichar("026B"), "l");
            mCharacterTable.Add(ToUnichar("026C"), "l");
            mCharacterTable.Add(ToUnichar("026D"), "l");
            mCharacterTable.Add(ToUnichar("026E"), "lz");
            mCharacterTable.Add(ToUnichar("026F"), "m");
            mCharacterTable.Add(ToUnichar("0270"), "m");
            mCharacterTable.Add(ToUnichar("0271"), "m");
            mCharacterTable.Add(ToUnichar("0272"), "n");
            mCharacterTable.Add(ToUnichar("0273"), "n");
            mCharacterTable.Add(ToUnichar("0274"), "N");
            mCharacterTable.Add(ToUnichar("0275"), "o");
            mCharacterTable.Add(ToUnichar("0276"), "OE");
            mCharacterTable.Add(ToUnichar("0277"), "o");
            mCharacterTable.Add(ToUnichar("0278"), "ph");
            mCharacterTable.Add(ToUnichar("0279"), "r");
            mCharacterTable.Add(ToUnichar("027A"), "r");
            mCharacterTable.Add(ToUnichar("027B"), "r");
            mCharacterTable.Add(ToUnichar("027C"), "r");
            mCharacterTable.Add(ToUnichar("027D"), "r");
            mCharacterTable.Add(ToUnichar("027E"), "r");
            mCharacterTable.Add(ToUnichar("027F"), "r");
            mCharacterTable.Add(ToUnichar("0280"), "R");
            mCharacterTable.Add(ToUnichar("0281"), "r");
            mCharacterTable.Add(ToUnichar("0282"), "s");
            mCharacterTable.Add(ToUnichar("0283"), "s");
            mCharacterTable.Add(ToUnichar("0284"), "j");
            mCharacterTable.Add(ToUnichar("0285"), "s");
            mCharacterTable.Add(ToUnichar("0286"), "s");
            mCharacterTable.Add(ToUnichar("0287"), "y");
            mCharacterTable.Add(ToUnichar("0288"), "t");
            mCharacterTable.Add(ToUnichar("0289"), "u");
            mCharacterTable.Add(ToUnichar("028A"), "u");
            mCharacterTable.Add(ToUnichar("028B"), "u");
            mCharacterTable.Add(ToUnichar("028C"), "v");
            mCharacterTable.Add(ToUnichar("028D"), "w");
            mCharacterTable.Add(ToUnichar("028E"), "y");
            mCharacterTable.Add(ToUnichar("028F"), "Y");
            mCharacterTable.Add(ToUnichar("0290"), "z");
            mCharacterTable.Add(ToUnichar("0291"), "z");
            mCharacterTable.Add(ToUnichar("0292"), "z");
            mCharacterTable.Add(ToUnichar("0293"), "z");
            mCharacterTable.Add(ToUnichar("0294"), "'");
            mCharacterTable.Add(ToUnichar("0295"), "'");
            mCharacterTable.Add(ToUnichar("0296"), "'");
            mCharacterTable.Add(ToUnichar("0297"), "C");
            mCharacterTable.Add(ToUnichar("0298"), "O˜");
            mCharacterTable.Add(ToUnichar("0299"), "B");
            mCharacterTable.Add(ToUnichar("029A"), "e");
            mCharacterTable.Add(ToUnichar("029B"), "G");
            mCharacterTable.Add(ToUnichar("029C"), "H");
            mCharacterTable.Add(ToUnichar("029D"), "j");
            mCharacterTable.Add(ToUnichar("029E"), "k");
            mCharacterTable.Add(ToUnichar("029F"), "L");
            mCharacterTable.Add(ToUnichar("02A0"), "q");
            mCharacterTable.Add(ToUnichar("02A1"), "'");
            mCharacterTable.Add(ToUnichar("02A2"), "'");
            mCharacterTable.Add(ToUnichar("02A3"), "dz");
            mCharacterTable.Add(ToUnichar("02A4"), "dz");
            mCharacterTable.Add(ToUnichar("02A5"), "dz");
            mCharacterTable.Add(ToUnichar("02A6"), "ts");
            mCharacterTable.Add(ToUnichar("02A7"), "ts");
            mCharacterTable.Add(ToUnichar("02A8"), "");
            mCharacterTable.Add(ToUnichar("02A9"), "fn");
            mCharacterTable.Add(ToUnichar("02AA"), "ls");
            mCharacterTable.Add(ToUnichar("02AB"), "lz");
            mCharacterTable.Add(ToUnichar("02AC"), "w");
            mCharacterTable.Add(ToUnichar("02AD"), "t");
            mCharacterTable.Add(ToUnichar("02AE"), "h");
            mCharacterTable.Add(ToUnichar("02AF"), "h");
            mCharacterTable.Add(ToUnichar("02B0"), "h");
            mCharacterTable.Add(ToUnichar("02B1"), "h");
            mCharacterTable.Add(ToUnichar("02B2"), "j");
            mCharacterTable.Add(ToUnichar("02B3"), "r");
            mCharacterTable.Add(ToUnichar("02B4"), "r");
            mCharacterTable.Add(ToUnichar("02B5"), "r");
            mCharacterTable.Add(ToUnichar("02B6"), "R");
            mCharacterTable.Add(ToUnichar("02B7"), "w");
            mCharacterTable.Add(ToUnichar("02B8"), "y");
            mCharacterTable.Add(ToUnichar("02E1"), "l");
            mCharacterTable.Add(ToUnichar("02E2"), "s");
            mCharacterTable.Add(ToUnichar("02E3"), "x");
            mCharacterTable.Add(ToUnichar("02E4"), "'");
            mCharacterTable.Add(ToUnichar("1D00"), "A");
            mCharacterTable.Add(ToUnichar("1D01"), "AE");
            mCharacterTable.Add(ToUnichar("1D02"), "ae");
            mCharacterTable.Add(ToUnichar("1D03"), "B");
            mCharacterTable.Add(ToUnichar("1D04"), "C");
            mCharacterTable.Add(ToUnichar("1D05"), "D");
            mCharacterTable.Add(ToUnichar("1D06"), "TH");
            mCharacterTable.Add(ToUnichar("1D07"), "E");
            mCharacterTable.Add(ToUnichar("1D08"), "e");
            mCharacterTable.Add(ToUnichar("1D09"), "i");
            mCharacterTable.Add(ToUnichar("1D0A"), "J");
            mCharacterTable.Add(ToUnichar("1D0B"), "K");
            mCharacterTable.Add(ToUnichar("1D0C"), "L");
            mCharacterTable.Add(ToUnichar("1D0D"), "M");
            mCharacterTable.Add(ToUnichar("1D0E"), "N");
            mCharacterTable.Add(ToUnichar("1D0F"), "O");
            mCharacterTable.Add(ToUnichar("1D10"), "O");
            mCharacterTable.Add(ToUnichar("1D11"), "o");
            mCharacterTable.Add(ToUnichar("1D12"), "o");
            mCharacterTable.Add(ToUnichar("1D13"), "o");
            mCharacterTable.Add(ToUnichar("1D14"), "oe");
            mCharacterTable.Add(ToUnichar("1D15"), "ou");
            mCharacterTable.Add(ToUnichar("1D16"), "o");
            mCharacterTable.Add(ToUnichar("1D17"), "o");
            mCharacterTable.Add(ToUnichar("1D18"), "P");
            mCharacterTable.Add(ToUnichar("1D19"), "R");
            mCharacterTable.Add(ToUnichar("1D1A"), "R");
            mCharacterTable.Add(ToUnichar("1D1B"), "T");
            mCharacterTable.Add(ToUnichar("1D1C"), "U");
            mCharacterTable.Add(ToUnichar("1D1D"), "u");
            mCharacterTable.Add(ToUnichar("1D1E"), "u");
            mCharacterTable.Add(ToUnichar("1D1F"), "m");
            mCharacterTable.Add(ToUnichar("1D20"), "V");
            mCharacterTable.Add(ToUnichar("1D21"), "W");
            mCharacterTable.Add(ToUnichar("1D22"), "Z");
            mCharacterTable.Add(ToUnichar("1D23"), "EZH");
            mCharacterTable.Add(ToUnichar("1D24"), "'");
            mCharacterTable.Add(ToUnichar("1D25"), "L");
            mCharacterTable.Add(ToUnichar("1D2C"), "A");
            mCharacterTable.Add(ToUnichar("1D2D"), "AE");
            mCharacterTable.Add(ToUnichar("1D2E"), "B");
            mCharacterTable.Add(ToUnichar("1D2F"), "B");
            mCharacterTable.Add(ToUnichar("1D30"), "D");
            mCharacterTable.Add(ToUnichar("1D31"), "E");
            mCharacterTable.Add(ToUnichar("1D32"), "E");
            mCharacterTable.Add(ToUnichar("1D33"), "G");
            mCharacterTable.Add(ToUnichar("1D34"), "H");
            mCharacterTable.Add(ToUnichar("1D35"), "I");
            mCharacterTable.Add(ToUnichar("1D36"), "J");
            mCharacterTable.Add(ToUnichar("1D37"), "K");
            mCharacterTable.Add(ToUnichar("1D38"), "L");
            mCharacterTable.Add(ToUnichar("1D39"), "M");
            mCharacterTable.Add(ToUnichar("1D3A"), "N");
            mCharacterTable.Add(ToUnichar("1D3B"), "N");
            mCharacterTable.Add(ToUnichar("1D3C"), "O");
            mCharacterTable.Add(ToUnichar("1D3D"), "OU");
            mCharacterTable.Add(ToUnichar("1D3E"), "P");
            mCharacterTable.Add(ToUnichar("1D3F"), "R");
            mCharacterTable.Add(ToUnichar("1D40"), "T");
            mCharacterTable.Add(ToUnichar("1D41"), "U");
            mCharacterTable.Add(ToUnichar("1D42"), "W");
            mCharacterTable.Add(ToUnichar("1D43"), "a");
            mCharacterTable.Add(ToUnichar("1D44"), "a");
            mCharacterTable.Add(ToUnichar("1D46"), "ae");
            mCharacterTable.Add(ToUnichar("1D47"), "b");
            mCharacterTable.Add(ToUnichar("1D48"), "d");
            mCharacterTable.Add(ToUnichar("1D49"), "e");
            mCharacterTable.Add(ToUnichar("1D4A"), "e");
            mCharacterTable.Add(ToUnichar("1D4B"), "e");
            mCharacterTable.Add(ToUnichar("1D4C"), "e");
            mCharacterTable.Add(ToUnichar("1D4D"), "g");
            mCharacterTable.Add(ToUnichar("1D4E"), "i");
            mCharacterTable.Add(ToUnichar("1D4F"), "k");
            mCharacterTable.Add(ToUnichar("1D50"), "m");
            mCharacterTable.Add(ToUnichar("1D51"), "g");
            mCharacterTable.Add(ToUnichar("1D52"), "o");
            mCharacterTable.Add(ToUnichar("1D53"), "o");
            mCharacterTable.Add(ToUnichar("1D54"), "o");
            mCharacterTable.Add(ToUnichar("1D55"), "o");
            mCharacterTable.Add(ToUnichar("1D56"), "p");
            mCharacterTable.Add(ToUnichar("1D57"), "t");
            mCharacterTable.Add(ToUnichar("1D58"), "u");
            mCharacterTable.Add(ToUnichar("1D59"), "u");
            mCharacterTable.Add(ToUnichar("1D5A"), "m");
            mCharacterTable.Add(ToUnichar("1D5B"), "v");
            mCharacterTable.Add(ToUnichar("1D62"), "i");
            mCharacterTable.Add(ToUnichar("1D63"), "r");
            mCharacterTable.Add(ToUnichar("1D64"), "u");
            mCharacterTable.Add(ToUnichar("1D65"), "v");
            mCharacterTable.Add(ToUnichar("1D6B"), "ue");
            mCharacterTable.Add(ToUnichar("1E00"), "A");
            mCharacterTable.Add(ToUnichar("1E01"), "a");
            mCharacterTable.Add(ToUnichar("1E02"), "B");
            mCharacterTable.Add(ToUnichar("1E03"), "b");
            mCharacterTable.Add(ToUnichar("1E04"), "B");
            mCharacterTable.Add(ToUnichar("1E05"), "b");
            mCharacterTable.Add(ToUnichar("1E06"), "B");
            mCharacterTable.Add(ToUnichar("1E07"), "b");
            mCharacterTable.Add(ToUnichar("1E08"), "C");
            mCharacterTable.Add(ToUnichar("1E09"), "c");
            mCharacterTable.Add(ToUnichar("1E0A"), "D");
            mCharacterTable.Add(ToUnichar("1E0B"), "d");
            mCharacterTable.Add(ToUnichar("1E0C"), "D");
            mCharacterTable.Add(ToUnichar("1E0D"), "d");
            mCharacterTable.Add(ToUnichar("1E0E"), "D");
            mCharacterTable.Add(ToUnichar("1E0F"), "d");
            mCharacterTable.Add(ToUnichar("1E10"), "D");
            mCharacterTable.Add(ToUnichar("1E11"), "d");
            mCharacterTable.Add(ToUnichar("1E12"), "D");
            mCharacterTable.Add(ToUnichar("1E13"), "d");
            mCharacterTable.Add(ToUnichar("1E14"), "E");
            mCharacterTable.Add(ToUnichar("1E15"), "e");
            mCharacterTable.Add(ToUnichar("1E16"), "E");
            mCharacterTable.Add(ToUnichar("1E17"), "e");
            mCharacterTable.Add(ToUnichar("1E18"), "E");
            mCharacterTable.Add(ToUnichar("1E19"), "e");
            mCharacterTable.Add(ToUnichar("1E1A"), "E");
            mCharacterTable.Add(ToUnichar("1E1B"), "e");
            mCharacterTable.Add(ToUnichar("1E1C"), "E");
            mCharacterTable.Add(ToUnichar("1E1D"), "e");
            mCharacterTable.Add(ToUnichar("1E1E"), "F");
            mCharacterTable.Add(ToUnichar("1E1F"), "f");
            mCharacterTable.Add(ToUnichar("1E20"), "G");
            mCharacterTable.Add(ToUnichar("1E21"), "g");
            mCharacterTable.Add(ToUnichar("1E22"), "H");
            mCharacterTable.Add(ToUnichar("1E23"), "h");
            mCharacterTable.Add(ToUnichar("1E24"), "H");
            mCharacterTable.Add(ToUnichar("1E25"), "h");
            mCharacterTable.Add(ToUnichar("1E26"), "H");
            mCharacterTable.Add(ToUnichar("1E27"), "h");
            mCharacterTable.Add(ToUnichar("1E28"), "H");
            mCharacterTable.Add(ToUnichar("1E29"), "h");
            mCharacterTable.Add(ToUnichar("1E2A"), "H");
            mCharacterTable.Add(ToUnichar("1E2B"), "h");
            mCharacterTable.Add(ToUnichar("1E2C"), "I");
            mCharacterTable.Add(ToUnichar("1E2D"), "i");
            mCharacterTable.Add(ToUnichar("1E2E"), "I");
            mCharacterTable.Add(ToUnichar("1E2F"), "i");
            mCharacterTable.Add(ToUnichar("1E30"), "K");
            mCharacterTable.Add(ToUnichar("1E31"), "k");
            mCharacterTable.Add(ToUnichar("1E32"), "K");
            mCharacterTable.Add(ToUnichar("1E33"), "k");
            mCharacterTable.Add(ToUnichar("1E34"), "K");
            mCharacterTable.Add(ToUnichar("1E35"), "k");
            mCharacterTable.Add(ToUnichar("1E36"), "L");
            mCharacterTable.Add(ToUnichar("1E37"), "l");
            mCharacterTable.Add(ToUnichar("1E38"), "L");
            mCharacterTable.Add(ToUnichar("1E39"), "l");
            mCharacterTable.Add(ToUnichar("1E3A"), "L");
            mCharacterTable.Add(ToUnichar("1E3B"), "l");
            mCharacterTable.Add(ToUnichar("1E3C"), "L");
            mCharacterTable.Add(ToUnichar("1E3D"), "l");
            mCharacterTable.Add(ToUnichar("1E3E"), "M");
            mCharacterTable.Add(ToUnichar("1E3F"), "m");
            mCharacterTable.Add(ToUnichar("1E40"), "M");
            mCharacterTable.Add(ToUnichar("1E41"), "m");
            mCharacterTable.Add(ToUnichar("1E42"), "M");
            mCharacterTable.Add(ToUnichar("1E43"), "m");
            mCharacterTable.Add(ToUnichar("1E44"), "N");
            mCharacterTable.Add(ToUnichar("1E45"), "n");
            mCharacterTable.Add(ToUnichar("1E46"), "N");
            mCharacterTable.Add(ToUnichar("1E47"), "n");
            mCharacterTable.Add(ToUnichar("1E48"), "N");
            mCharacterTable.Add(ToUnichar("1E49"), "n");
            mCharacterTable.Add(ToUnichar("1E4A"), "N");
            mCharacterTable.Add(ToUnichar("1E4B"), "n");
            mCharacterTable.Add(ToUnichar("1E4C"), "O");
            mCharacterTable.Add(ToUnichar("1E4D"), "o");
            mCharacterTable.Add(ToUnichar("1E4E"), "O");
            mCharacterTable.Add(ToUnichar("1E4F"), "o");
            mCharacterTable.Add(ToUnichar("1E50"), "O");
            mCharacterTable.Add(ToUnichar("1E51"), "o");
            mCharacterTable.Add(ToUnichar("1E52"), "O");
            mCharacterTable.Add(ToUnichar("1E53"), "o");
            mCharacterTable.Add(ToUnichar("1E54"), "P");
            mCharacterTable.Add(ToUnichar("1E55"), "p");
            mCharacterTable.Add(ToUnichar("1E56"), "P");
            mCharacterTable.Add(ToUnichar("1E57"), "p");
            mCharacterTable.Add(ToUnichar("1E58"), "R");
            mCharacterTable.Add(ToUnichar("1E59"), "r");
            mCharacterTable.Add(ToUnichar("1E5A"), "R");
            mCharacterTable.Add(ToUnichar("1E5B"), "r");
            mCharacterTable.Add(ToUnichar("1E5C"), "R");
            mCharacterTable.Add(ToUnichar("1E5D"), "r");
            mCharacterTable.Add(ToUnichar("1E5E"), "R");
            mCharacterTable.Add(ToUnichar("1E5F"), "r");
            mCharacterTable.Add(ToUnichar("1E60"), "S");
            mCharacterTable.Add(ToUnichar("1E61"), "s");
            mCharacterTable.Add(ToUnichar("1E62"), "S");
            mCharacterTable.Add(ToUnichar("1E63"), "s");
            mCharacterTable.Add(ToUnichar("1E64"), "S");
            mCharacterTable.Add(ToUnichar("1E65"), "s");
            mCharacterTable.Add(ToUnichar("1E66"), "S");
            mCharacterTable.Add(ToUnichar("1E67"), "s");
            mCharacterTable.Add(ToUnichar("1E68"), "S");
            mCharacterTable.Add(ToUnichar("1E69"), "s");
            mCharacterTable.Add(ToUnichar("1E6A"), "T");
            mCharacterTable.Add(ToUnichar("1E6B"), "t");
            mCharacterTable.Add(ToUnichar("1E6C"), "T");
            mCharacterTable.Add(ToUnichar("1E6D"), "t");
            mCharacterTable.Add(ToUnichar("1E6E"), "T");
            mCharacterTable.Add(ToUnichar("1E6F"), "t");
            mCharacterTable.Add(ToUnichar("1E70"), "T");
            mCharacterTable.Add(ToUnichar("1E71"), "t");
            mCharacterTable.Add(ToUnichar("1E72"), "U");
            mCharacterTable.Add(ToUnichar("1E73"), "u");
            mCharacterTable.Add(ToUnichar("1E74"), "U");
            mCharacterTable.Add(ToUnichar("1E75"), "u");
            mCharacterTable.Add(ToUnichar("1E76"), "U");
            mCharacterTable.Add(ToUnichar("1E77"), "u");
            mCharacterTable.Add(ToUnichar("1E78"), "U");
            mCharacterTable.Add(ToUnichar("1E79"), "u");
            mCharacterTable.Add(ToUnichar("1E7A"), "U");
            mCharacterTable.Add(ToUnichar("1E7B"), "u");
            mCharacterTable.Add(ToUnichar("1E7C"), "V");
            mCharacterTable.Add(ToUnichar("1E7D"), "v");
            mCharacterTable.Add(ToUnichar("1E7E"), "V");
            mCharacterTable.Add(ToUnichar("1E7F"), "v");
            mCharacterTable.Add(ToUnichar("1E80"), "W");
            mCharacterTable.Add(ToUnichar("1E81"), "w");
            mCharacterTable.Add(ToUnichar("1E82"), "W");
            mCharacterTable.Add(ToUnichar("1E83"), "w");
            mCharacterTable.Add(ToUnichar("1E84"), "W");
            mCharacterTable.Add(ToUnichar("1E85"), "w");
            mCharacterTable.Add(ToUnichar("1E86"), "W");
            mCharacterTable.Add(ToUnichar("1E87"), "w");
            mCharacterTable.Add(ToUnichar("1E88"), "W");
            mCharacterTable.Add(ToUnichar("1E89"), "w");
            mCharacterTable.Add(ToUnichar("1E8A"), "X");
            mCharacterTable.Add(ToUnichar("1E8B"), "x");
            mCharacterTable.Add(ToUnichar("1E8C"), "X");
            mCharacterTable.Add(ToUnichar("1E8D"), "x");
            mCharacterTable.Add(ToUnichar("1E8E"), "Y");
            mCharacterTable.Add(ToUnichar("1E8F"), "y");
            mCharacterTable.Add(ToUnichar("1E90"), "Z");
            mCharacterTable.Add(ToUnichar("1E91"), "z");
            mCharacterTable.Add(ToUnichar("1E92"), "Z");
            mCharacterTable.Add(ToUnichar("1E93"), "z");
            mCharacterTable.Add(ToUnichar("1E94"), "Z");
            mCharacterTable.Add(ToUnichar("1E95"), "z");
            mCharacterTable.Add(ToUnichar("1E96"), "h");
            mCharacterTable.Add(ToUnichar("1E97"), "t");
            mCharacterTable.Add(ToUnichar("1E98"), "w");
            mCharacterTable.Add(ToUnichar("1E99"), "y");
            mCharacterTable.Add(ToUnichar("1E9A"), "a");
            mCharacterTable.Add(ToUnichar("1E9B"), "s");
            mCharacterTable.Add(ToUnichar("1EA0"), "A");
            mCharacterTable.Add(ToUnichar("1EA1"), "a");
            mCharacterTable.Add(ToUnichar("1EA2"), "A");
            mCharacterTable.Add(ToUnichar("1EA3"), "a");
            mCharacterTable.Add(ToUnichar("1EA4"), "A");
            mCharacterTable.Add(ToUnichar("1EA5"), "a");
            mCharacterTable.Add(ToUnichar("1EA6"), "A");
            mCharacterTable.Add(ToUnichar("1EA7"), "a");
            mCharacterTable.Add(ToUnichar("1EA8"), "A");
            mCharacterTable.Add(ToUnichar("1EA9"), "a");
            mCharacterTable.Add(ToUnichar("1EAA"), "A");
            mCharacterTable.Add(ToUnichar("1EAB"), "a");
            mCharacterTable.Add(ToUnichar("1EAC"), "A");
            mCharacterTable.Add(ToUnichar("1EAD"), "a");
            mCharacterTable.Add(ToUnichar("1EAE"), "A");
            mCharacterTable.Add(ToUnichar("1EAF"), "a");
            mCharacterTable.Add(ToUnichar("1EB0"), "A");
            mCharacterTable.Add(ToUnichar("1EB1"), "a");
            mCharacterTable.Add(ToUnichar("1EB2"), "A");
            mCharacterTable.Add(ToUnichar("1EB3"), "a");
            mCharacterTable.Add(ToUnichar("1EB4"), "A");
            mCharacterTable.Add(ToUnichar("1EB5"), "a");
            mCharacterTable.Add(ToUnichar("1EB6"), "A");
            mCharacterTable.Add(ToUnichar("1EB7"), "a");
            mCharacterTable.Add(ToUnichar("1EB8"), "E");
            mCharacterTable.Add(ToUnichar("1EB9"), "e");
            mCharacterTable.Add(ToUnichar("1EBA"), "E");
            mCharacterTable.Add(ToUnichar("1EBB"), "e");
            mCharacterTable.Add(ToUnichar("1EBC"), "E");
            mCharacterTable.Add(ToUnichar("1EBD"), "e");
            mCharacterTable.Add(ToUnichar("1EBE"), "E");
            mCharacterTable.Add(ToUnichar("1EBF"), "e");
            mCharacterTable.Add(ToUnichar("1EC0"), "E");
            mCharacterTable.Add(ToUnichar("1EC1"), "e");
            mCharacterTable.Add(ToUnichar("1EC2"), "E");
            mCharacterTable.Add(ToUnichar("1EC3"), "e");
            mCharacterTable.Add(ToUnichar("1EC4"), "E");
            mCharacterTable.Add(ToUnichar("1EC5"), "e");
            mCharacterTable.Add(ToUnichar("1EC6"), "E");
            mCharacterTable.Add(ToUnichar("1EC7"), "e");
            mCharacterTable.Add(ToUnichar("1EC8"), "I");
            mCharacterTable.Add(ToUnichar("1EC9"), "i");
            mCharacterTable.Add(ToUnichar("1ECA"), "I");
            mCharacterTable.Add(ToUnichar("1ECB"), "i");
            mCharacterTable.Add(ToUnichar("1ECC"), "O");
            mCharacterTable.Add(ToUnichar("1ECD"), "o");
            mCharacterTable.Add(ToUnichar("1ECE"), "O");
            mCharacterTable.Add(ToUnichar("1ECF"), "o");
            mCharacterTable.Add(ToUnichar("1ED0"), "O");
            mCharacterTable.Add(ToUnichar("1ED1"), "o");
            mCharacterTable.Add(ToUnichar("1ED2"), "O");
            mCharacterTable.Add(ToUnichar("1ED3"), "o");
            mCharacterTable.Add(ToUnichar("1ED4"), "O");
            mCharacterTable.Add(ToUnichar("1ED5"), "o");
            mCharacterTable.Add(ToUnichar("1ED6"), "O");
            mCharacterTable.Add(ToUnichar("1ED7"), "o");
            mCharacterTable.Add(ToUnichar("1ED8"), "O");
            mCharacterTable.Add(ToUnichar("1ED9"), "o");
            mCharacterTable.Add(ToUnichar("1EDA"), "O");
            mCharacterTable.Add(ToUnichar("1EDB"), "o");
            mCharacterTable.Add(ToUnichar("1EDC"), "O");
            mCharacterTable.Add(ToUnichar("1EDD"), "o");
            mCharacterTable.Add(ToUnichar("1EDE"), "O");
            mCharacterTable.Add(ToUnichar("1EDF"), "o");
            mCharacterTable.Add(ToUnichar("1EE0"), "O");
            mCharacterTable.Add(ToUnichar("1EE1"), "o");
            mCharacterTable.Add(ToUnichar("1EE2"), "O");
            mCharacterTable.Add(ToUnichar("1EE3"), "o");
            mCharacterTable.Add(ToUnichar("1EE4"), "U");
            mCharacterTable.Add(ToUnichar("1EE5"), "u");
            mCharacterTable.Add(ToUnichar("1EE6"), "U");
            mCharacterTable.Add(ToUnichar("1EE7"), "u");
            mCharacterTable.Add(ToUnichar("1EE8"), "U");
            mCharacterTable.Add(ToUnichar("1EE9"), "u");
            mCharacterTable.Add(ToUnichar("1EEA"), "U");
            mCharacterTable.Add(ToUnichar("1EEB"), "u");
            mCharacterTable.Add(ToUnichar("1EEC"), "U");
            mCharacterTable.Add(ToUnichar("1EED"), "u");
            mCharacterTable.Add(ToUnichar("1EEE"), "U");
            mCharacterTable.Add(ToUnichar("1EEF"), "u");
            mCharacterTable.Add(ToUnichar("1EF0"), "U");
            mCharacterTable.Add(ToUnichar("1EF1"), "u");
            mCharacterTable.Add(ToUnichar("1EF2"), "Y");
            mCharacterTable.Add(ToUnichar("1EF3"), "y");
            mCharacterTable.Add(ToUnichar("1EF4"), "Y");
            mCharacterTable.Add(ToUnichar("1EF5"), "y");
            mCharacterTable.Add(ToUnichar("1EF6"), "Y");
            mCharacterTable.Add(ToUnichar("1EF7"), "y");
            mCharacterTable.Add(ToUnichar("1EF8"), "Y");
            mCharacterTable.Add(ToUnichar("1EF9"), "y");
            mCharacterTable.Add(ToUnichar("2071"), "i");
            mCharacterTable.Add(ToUnichar("207F"), "n");
            mCharacterTable.Add(ToUnichar("212A"), "K");
            mCharacterTable.Add(ToUnichar("212B"), "A");
            mCharacterTable.Add(ToUnichar("212C"), "B");
            mCharacterTable.Add(ToUnichar("212D"), "C");
            mCharacterTable.Add(ToUnichar("212F"), "e");
            mCharacterTable.Add(ToUnichar("2130"), "E");
            mCharacterTable.Add(ToUnichar("2131"), "F");
            mCharacterTable.Add(ToUnichar("2132"), "F");
            mCharacterTable.Add(ToUnichar("2133"), "M");
            mCharacterTable.Add(ToUnichar("2134"), "0");
            mCharacterTable.Add(ToUnichar("213A"), "0");
            mCharacterTable.Add(ToUnichar("2141"), "G");
            mCharacterTable.Add(ToUnichar("2142"), "L");
            mCharacterTable.Add(ToUnichar("2143"), "L");
            mCharacterTable.Add(ToUnichar("2144"), "Y");
            mCharacterTable.Add(ToUnichar("2145"), "D");
            mCharacterTable.Add(ToUnichar("2146"), "d");
            mCharacterTable.Add(ToUnichar("2147"), "e");
            mCharacterTable.Add(ToUnichar("2148"), "i");
            mCharacterTable.Add(ToUnichar("2149"), "j");
            mCharacterTable.Add(ToUnichar("FB00"), "ff");
            mCharacterTable.Add(ToUnichar("FB01"), "fi");
            mCharacterTable.Add(ToUnichar("FB02"), "fl");
            mCharacterTable.Add(ToUnichar("FB03"), "ffi");
            mCharacterTable.Add(ToUnichar("FB04"), "ffl");
            mCharacterTable.Add(ToUnichar("FB05"), "st");
            mCharacterTable.Add(ToUnichar("FB06"), "st");
            mCharacterTable.Add(ToUnichar("FF21"), "A");
            mCharacterTable.Add(ToUnichar("FF22"), "B");
            mCharacterTable.Add(ToUnichar("FF23"), "C");
            mCharacterTable.Add(ToUnichar("FF24"), "D");
            mCharacterTable.Add(ToUnichar("FF25"), "E");
            mCharacterTable.Add(ToUnichar("FF26"), "F");
            mCharacterTable.Add(ToUnichar("FF27"), "G");
            mCharacterTable.Add(ToUnichar("FF28"), "H");
            mCharacterTable.Add(ToUnichar("FF29"), "I");
            mCharacterTable.Add(ToUnichar("FF2A"), "J");
            mCharacterTable.Add(ToUnichar("FF2B"), "K");
            mCharacterTable.Add(ToUnichar("FF2C"), "L");
            mCharacterTable.Add(ToUnichar("FF2D"), "M");
            mCharacterTable.Add(ToUnichar("FF2E"), "N");
            mCharacterTable.Add(ToUnichar("FF2F"), "O");
            mCharacterTable.Add(ToUnichar("FF30"), "P");
            mCharacterTable.Add(ToUnichar("FF31"), "Q");
            mCharacterTable.Add(ToUnichar("FF32"), "R");
            mCharacterTable.Add(ToUnichar("FF33"), "S");
            mCharacterTable.Add(ToUnichar("FF34"), "T");
            mCharacterTable.Add(ToUnichar("FF35"), "U");
            mCharacterTable.Add(ToUnichar("FF36"), "V");
            mCharacterTable.Add(ToUnichar("FF37"), "W");
            mCharacterTable.Add(ToUnichar("FF38"), "X");
            mCharacterTable.Add(ToUnichar("FF39"), "Y");
            mCharacterTable.Add(ToUnichar("FF3A"), "Z");
            mCharacterTable.Add(ToUnichar("FF41"), "a");
            mCharacterTable.Add(ToUnichar("FF42"), "b");
            mCharacterTable.Add(ToUnichar("FF43"), "c");
            mCharacterTable.Add(ToUnichar("FF44"), "d");
            mCharacterTable.Add(ToUnichar("FF45"), "e");
            mCharacterTable.Add(ToUnichar("FF46"), "f");
            mCharacterTable.Add(ToUnichar("FF47"), "g");
            mCharacterTable.Add(ToUnichar("FF48"), "h");
            mCharacterTable.Add(ToUnichar("FF49"), "i");
            mCharacterTable.Add(ToUnichar("FF4A"), "j");
            mCharacterTable.Add(ToUnichar("FF4B"), "k");
            mCharacterTable.Add(ToUnichar("FF4C"), "l");
            mCharacterTable.Add(ToUnichar("FF4D"), "m");
            mCharacterTable.Add(ToUnichar("FF4E"), "n");
            mCharacterTable.Add(ToUnichar("FF4F"), "o");
            mCharacterTable.Add(ToUnichar("FF50"), "p");
            mCharacterTable.Add(ToUnichar("FF51"), "q");
            mCharacterTable.Add(ToUnichar("FF52"), "r");
            mCharacterTable.Add(ToUnichar("FF53"), "s");
            mCharacterTable.Add(ToUnichar("FF54"), "t");
            mCharacterTable.Add(ToUnichar("FF55"), "u");
            mCharacterTable.Add(ToUnichar("FF56"), "v");
            mCharacterTable.Add(ToUnichar("FF57"), "w");
            mCharacterTable.Add(ToUnichar("FF58"), "x");
            mCharacterTable.Add(ToUnichar("FF59"), "y");
            mCharacterTable.Add(ToUnichar("FF5A"), "z");
        }
        public static string LatinToAscii(string InString)
        {
            string str = "";
            if (mCharacterTable == null)
            {
                InitializeCharacterTable();
            }
            for (int i = 0; i < InString.Length; i++)
            {
                string key = InString.Substring(i, 1);
                if (!mCharacterTable.Contains(key))
                {
                    str = str + key;
                }
                else
                {
                    str = str + mCharacterTable[key];
                }
            }
            return str;
        }

        public static string ToUnichar(string HexString)
        {
            byte[] bytes = new byte[2];
            UnicodeEncoding encoding = new UnicodeEncoding();
            bytes[0] = Convert.ToByte(HexString.Substring(2, 2), 0x10);
            bytes[1] = Convert.ToByte(HexString.Substring(0, 2), 0x10);
            return encoding.GetString(bytes);
        }
        public static string ToAscii(string unicode)
        {
            unicode = Regex.Replace(unicode, "\\W+", "-"); //Nếu bạn muốn thay dấu khoảng trắng thành dấu "_" hoặc dấu cách " " thì thay kí tự bạn muốn vào đấu "-"
            return unicode;
        }
        #endregion

        /// <summary>
        /// Bỏ số 0 phía sau phần thập phân
        /// </summary>
        public static string RemoveZero(string dec)
        {
            try
            {
                if (dec.Contains(".") && dec[dec.Length - 1] == '0')
                {
                    dec = dec.TrimEnd('0').TrimEnd('.');

                    return RemoveZero(dec);
                }
                else
                {
                    return dec;
                }
            }
            catch (Exception ex)
            {
                return dec;
            }

        }



        /// <summary>
        /// create folder
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string CreateFolder(string path)
        {
            bool folderExists = Directory.Exists(path);
            if (!folderExists)
                Directory.CreateDirectory(path);
            return path;
        }

        /// <summary>
        /// Tải file template từ server về
        /// </summary> 
        public static void DownloadFile(string fileName, string filePath)
        {
            try
            {
                System.Web.HttpContext.Current.Response.ContentType = "APPLICATION/OCTET-STREAM";
                String Header = "Attachment; Filename=" + fileName;
                System.Web.HttpContext.Current.Response.AppendHeader("Content-Disposition", Header);
                System.IO.FileInfo Dfile = new System.IO.FileInfo(filePath);
                System.Web.HttpContext.Current.Response.WriteFile(Dfile.FullName);
                System.Web.HttpContext.Current.Response.End();
            }
            catch (Exception ex)
            {
            }
        }

        /// <summary>
        /// xem trước file
        /// </summary>
        /// <param name="filePath"></param>
        public static void PreviewFile(string filePath)
        {
            try
            {
                using (Process prc = new Process())
                {
                    prc.StartInfo = new ProcessStartInfo(filePath);
                    prc.Start();
                    prc.WaitForExit(1 * 60 * 1000);
                    if (!prc.HasExited)
                    {
                        prc.Kill();
                    }
                }
            }
            catch (Exception ex)
            { }
        }


        /// <summary>
        /// Upload hình ảnh
        /// </summary>
        public static void UploadImage(HttpRequestBase request, string uploadFileName, int maxSize, string path, out string imageUrl)
        {
            imageUrl = "";

            try
            {
                if (request.Files.Count > 0)
                {
                    var file = request.Files[0];

                    //Kiểm tra file dung lượng <= 2Mb thì upload
                    if (file != null && file.ContentLength > 0 && file.ContentLength <= maxSize)
                    {
                        //Nếu không phải file hình mặc định avatar.png thì mới lưu
                        if (Path.GetFileName(file.FileName) != "avatar.png")
                        {
                            //Tên hình = ngày tháng năm giờ phút giây + mã nhân viên + tên nhân viên
                            var fileName = uploadFileName + ".jpg";//Path.GetExtension(file.FileName);

                            file.SaveAs(path);

                            imageUrl = "/Image/ProfileImage/" + fileName;
                        }
                    }
                }
            }
            catch
            { }
        }
        /// <summary>
        /// Kiểu dữ liệu của cột trong DataTable
        /// </summary>
        public static string GetDataColumnType(DataColumn col)
        {
            try
            {
                if (col.DataType.Name.ToLower() == "decimal"
                || col.DataType.Name.ToLower() == "int"
                || col.DataType.Name.ToLower() == "float"
                || col.DataType.Name.ToLower() == "double"
                || col.DataType.Name.ToLower() == "long")
                {
                    return "number";
                }
                else if (col.DataType.Name.ToLower() == "datetime")
                {
                    return "datetime";
                }
                else
                {
                    return "string";
                }
            }
            catch (Exception ex)
            {
                return "string";
            }

        }

        /// <summary>
        /// delete folder
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static bool DeleteFolder(string path)
        {
            try
            {
                bool folderExists = Directory.Exists(path);
                if (folderExists)
                    Directory.Delete(path, true);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// Đổi số double sang kiểu string có cả số âm.
        /// Trong phần nguyên cứ 3 số thì thêm dấu phẩy vào
        /// </summary>
        /// <param name="_number"></param>
        /// <returns></returns>
        public static string AddSeparatorWithNegative(double _number, int roundNumber)
        {
            string parseString = Math.Round(_number, roundNumber).ToString();
            int index = parseString.IndexOf('.');
            string partFloat = string.Empty;
            string partInt = parseString;
            if (index > 0)
            {
                partFloat = parseString.Substring(index, parseString.Length - index);
                partInt = parseString.Substring(0, index);
            }
            partInt = NumericFormat(DBConvert.ParseDouble(partInt), 0);
            return string.Format("{0}{1}", partInt, partFloat);
        }

        public static bool RemoveFileFromServer(string path)
        {
            System.IO.DirectoryInfo di = new DirectoryInfo(path);

            foreach (FileInfo file in di.GetFiles())
            {
                try
                {
                    file.Delete();
                }
                catch (Exception e)
                {
                }
            }

            return false;
        }

        public static string MD5Hash(string text)
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            md5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(text));
            byte[] result = md5.Hash;
            StringBuilder strBuilder = new StringBuilder();
            for (int i = 0; i < result.Length; i++)
            {
                strBuilder.Append(result[i].ToString("x2"));
            }

            return strBuilder.ToString();
        }

        private static string _AddPrefixErrMsg(string prefix, bool addErrorWithPrefix, bool isErr, string errMsg)
        {
            return addErrorWithPrefix && isErr ? prefix + errMsg : errMsg; 
        }
        public static string _AddPrefixErrMsg(string prefix, bool addErrorWithPrefix, string errCode, string errMsg)
        {
            return _AddPrefixErrMsg(prefix, addErrorWithPrefix, errCode != "", errMsg);
        }

        private static string ConvertWholeNumber(string Number)
        {
            string word = "";
            try
            {
                bool beginsZero = false;//tests for 0XX    
                bool isDone = false;//test if already translated    
                double dblAmt = (Convert.ToDouble(Number));
                //if ((dblAmt > 0) && number.StartsWith("0"))    
                if (dblAmt > 0)
                {//test for zero or digit zero in a nuemric    
                    beginsZero = Number.StartsWith("0");

                    int numDigits = Number.Length;
                    int pos = 0;//store digit grouping    
                    String place = "";//digit grouping name:hundres,thousand,etc...    
                    switch (numDigits)
                    {
                        case 1://ones' range    

                            word = ones(Number);
                            isDone = true;
                            break;
                        case 2://tens' range    
                            word = tens(Number);
                            isDone = true;
                            break;
                        case 3://hundreds' range    
                            pos = (numDigits % 3) + 1;
                            place = " Hundred ";
                            break;
                        case 4://thousands' range    
                        case 5:
                        case 6:
                            pos = (numDigits % 4) + 1;
                            place = " Thousand ";
                            break;
                        case 7://millions' range    
                        case 8:
                        case 9:
                            pos = (numDigits % 7) + 1;
                            place = " Million ";
                            break;
                        case 10://Billions's range    
                        case 11:
                        case 12:

                            pos = (numDigits % 10) + 1;
                            place = " Billion ";
                            break;
                        //add extra case options for anything above Billion...    
                        default:
                            isDone = true;
                            break;
                    }
                    if (!isDone)
                    {//if transalation is not done, continue...(Recursion comes in now!!)    
                        if (Number.Substring(0, pos) != "0" && Number.Substring(pos) != "0")
                        {
                            try
                            {
                                word = ConvertWholeNumber(Number.Substring(0, pos)) + place + ConvertWholeNumber(Number.Substring(pos));
                            }
                            catch { }
                        }
                        else
                        {
                            word = ConvertWholeNumber(Number.Substring(0, pos)) + ConvertWholeNumber(Number.Substring(pos));
                        }

                        //check for trailing zeros    
                        //if (beginsZero) word = " and " + word.Trim();    
                    }
                    //ignore digit grouping names    
                    if (word.Trim().Equals(place.Trim())) word = "";
                }
            }
            catch { }
            return word.Trim();
        }
        private static String ones(String Number)
        {
            int _Number = Convert.ToInt32(Number);
            String name = "";
            switch (_Number)
            {

                case 1:
                    name = "One";
                    break;
                case 2:
                    name = "Two";
                    break;
                case 3:
                    name = "Three";
                    break;
                case 4:
                    name = "Four";
                    break;
                case 5:
                    name = "Five";
                    break;
                case 6:
                    name = "Six";
                    break;
                case 7:
                    name = "Seven";
                    break;
                case 8:
                    name = "Eight";
                    break;
                case 9:
                    name = "Nine";
                    break;
            }
            return name;
        }
        private static String tens(String Number)
        {
            int _Number = Convert.ToInt32(Number);
            String name = null;
            switch (_Number)
            {
                case 10:
                    name = "Ten";
                    break;
                case 11:
                    name = "Eleven";
                    break;
                case 12:
                    name = "Twelve";
                    break;
                case 13:
                    name = "Thirteen";
                    break;
                case 14:
                    name = "Fourteen";
                    break;
                case 15:
                    name = "Fifteen";
                    break;
                case 16:
                    name = "Sixteen";
                    break;
                case 17:
                    name = "Seventeen";
                    break;
                case 18:
                    name = "Eighteen";
                    break;
                case 19:
                    name = "Nineteen";
                    break;
                case 20:
                    name = "Twenty";
                    break;
                case 30:
                    name = "Thirty";
                    break;
                case 40:
                    name = "Fourty";
                    break;
                case 50:
                    name = "Fifty";
                    break;
                case 60:
                    name = "Sixty";
                    break;
                case 70:
                    name = "Seventy";
                    break;
                case 80:
                    name = "Eighty";
                    break;
                case 90:
                    name = "Ninety";
                    break;
                default:
                    if (_Number > 0)
                    {
                        name = tens(Number.Substring(0, 1) + "0") + " " + ones(Number.Substring(1));
                    }
                    break;
            }
            return name;
        }
        public static String ConvertToWords(String numb)
        {
            String val = "", wholeNo = numb, points = "", andStr = "", pointStr = "";
            String endStr = "Only";
            try
            {
                int decimalPlace = numb.IndexOf(".");
                if (decimalPlace > 0)
                {
                    wholeNo = numb.Substring(0, decimalPlace);
                    points = numb.Substring(decimalPlace + 1);
                    if (Convert.ToInt32(points) > 0)
                    {
                        andStr = "and";// just to separate whole numbers from points/cents   
                        endStr = "Paisa " + endStr;//Cents   
                        pointStr = ConvertDecimals(points);
                    }
                }
                val = String.Format("{0} {1}{2} {3}", ConvertWholeNumber(wholeNo).Trim(), andStr, pointStr, endStr);
            }
            catch { }
            return val;
        }
        public static String ConvertDecimals(String number)
        {
            String cd = "", digit = "", engOne = "";
            for (int i = 0; i < number.Length; i++)
            {
                digit = number[i].ToString();
                if (digit.Equals("0"))
                {
                    engOne = "Zero";
                }
                else
                {
                    engOne = ones(digit);
                }
                cd += " " + engOne;
            }
            return cd;
        }
    }
}
