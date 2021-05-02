using AutoMapper;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Util
{
    public class Util
    {
        public readonly static CultureInfo cultureInfo = new CultureInfo("en-US");
        public static readonly Dictionary<string, int> NumberDecimal = new Dictionary<string, int>();
        public static string[] mangso = new string[10] { "không", "một", "hai", "ba", "bốn", "năm","sáu","bảy","tám","chín" };
        static Util()
        {
            NumberDecimal.Add("VND", 0);
        }
        public static ArrayList errors = new ArrayList();
        public bool bValid;
        public T Transform<U, T>(U entity)
        {
            return Mapper.Map<T>(entity);
        }
        public IList<T> TransformList<U, T>(IList<U> entity)
        {
            IList<T> res = new List<T>();
            foreach (U u in entity)
            {
                res.Add(Mapper.Map<T>(u));
            }
            return res;
        }
        public T Deserialize<T>(string input) where T : class
        {
            try
            {
                XmlSerializer ser = new XmlSerializer(typeof(T));
                using (StringReader sr = new StringReader(input))
                {
                    return (T)ser.Deserialize(sr);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public T Deserializexml<T>(string input) where T : class
        {
            try
            {
                XmlSerializer ser = new XmlSerializer(typeof(T));
                using (StringReader sr = new StringReader(input))
                {
                    return (T)ser.Deserialize(sr);
                }
            }
            catch (Exception ex)
            {
                string s = errors.ToString();
                throw new InvalidOperationException("Failed to " + "create object from xml string", ex);
            }

        }
        public T DeserializeTime<T>(string input) where T : class
        {
            XmlSerializer ser = new XmlSerializer(typeof(T));

            using (StringReader sr = new StringReader(input))
            {
                return (T)ser.Deserialize(sr);
            }
        }
        public string GetDateTime(string data, string format)
        {
            DateTime dt = DateTime.Parse(data);
            return dt.ToString(format);
        }
        //vi
        public string Serialize<T>(T ObjectToSerialize)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(ObjectToSerialize.GetType());
            using (StringWriter textWriter = new StringWriter())
            {
                xmlSerializer.Serialize(textWriter, ObjectToSerialize);
                return textWriter.ToString();
            }
        }

        //vi
        public string GetXMLFromObject(object o)
        {
            StringWriter sw = new StringWriter();
            XmlTextWriter tw = null;
            try
            {
                XmlSerializer serializer = new XmlSerializer(o.GetType());
                tw = new XmlTextWriter(sw);
                serializer.Serialize(tw, o);
            }
            catch (Exception ex)
            {
                //Handle Exception Code
            }
            finally
            {
                sw.Close();
                if (tw != null)
                {
                    tw.Close();
                }
            }
            return sw.ToString();
        }
        //vi
        public Object ObjectToXML(string xml, Type objectType)
        {
            StringReader strReader = null;
            XmlSerializer serializer = null;
            XmlTextReader xmlReader = null;
            Object obj = null;
            try
            {
                strReader = new StringReader(xml);
                serializer = new XmlSerializer(objectType);
                xmlReader = new XmlTextReader(strReader);
                obj = serializer.Deserialize(xmlReader);
            }
            catch (Exception exp)
            {
                //Handle Exception Code
            }
            finally
            {
                if (xmlReader != null)
                {
                    xmlReader.Close();
                }
                if (strReader != null)
                {
                    strReader.Close();
                }
            }
            return obj;
        }
        private void ValidationCallBack(object sender, ValidationEventArgs args)
        {
            bValid = false;
            errors.Add(args.Message);
        }


        // write xml file (toandv - 26-01-2018)
        public bool WriteXMLFile<T>(T ObjectWrite, string fileName, string path)
        {
            try
            {
                XmlSerializer writer = new XmlSerializer(typeof(T));
                System.IO.FileStream file = System.IO.File.Create(path);
                writer.Serialize(file, ObjectWrite);
                file.Close();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public ArrayList GetErrors()
        {
            return errors;
        }

        private static string GetCurrencyFormat(string currency)
        {
            int dec = 2;
            foreach(var it in NumberDecimal)
            {
                if (it.Key.Equals(currency))
                {
                    dec = it.Value;break;
                }
            }
            string format = "{0:#,##0";
            if(dec > 0)
            {
                format += "." + new string('#', dec);
            }
            return format + "}";
        }
        public static string CurrencyFormat(object ob, string currency,string format = null)
        {
            try
            {
                if (ob == null) return "0";
                if (string.IsNullOrWhiteSpace(format))
                {
                    format = GetCurrencyFormat(currency);
                }
                if(ob.GetType() == typeof(string))
                {
                    decimal d = decimal.Parse(ob.ToString());
                    return String.Format(cultureInfo, format, d);
                }
                return String.Format(cultureInfo, format, ob);
            }
            catch (Exception)
            {
                return ob.ToString();
            }
        }

        public static string NumberFormat(object ob, int decimal_plances = 0)
        {
            try
            {
                if (ob == null) return "0";
                string format = "{0:##0}";
                if (decimal_plances > 0)
                {
                    format = "{0:##0.";
                    for(int i = 0; i < decimal_plances; i++)
                    {
                        format += "#";
                    }
                    format += "}";
                }
                if(ob.GetType() == typeof(string))
                {
                    decimal d = decimal.Parse(ob.ToString());
                    return String.Format(cultureInfo, format, d);
                }
                return String.Format(cultureInfo, format, ob);
            }
            catch (Exception)
            {
                return ob.ToString();
            }
        }

        public static string DocSo(string so, string currency)
        {
            string res = "";
            var phanle = "";
            if (so.IndexOf('.') >= 0)
            {
                phanle = so.Split('.')[1];
            }
            long _so = long.Parse(so.Split('.')[0]);
            res += DocSo(_so);
            if (phanle != "")
            {
                switch (currency)
                {
                    case "VND":
                        res += " đồng và" + DocSo(long.Parse(phanle)) + " xu.";
                        break;
                    case "USD":
                        res += " đô la và" + DocSo(long.Parse(phanle)) + "cent";
                        break;
                    default:
                        return "";
                }
            }
            else
            {
                switch (currency)
                {
                    case "VND":
                        res += " đồng chẵn.";
                        break;
                    case "USD":
                        res += " đô la mỹ";
                        break;
                    default:
                        return "";
                }
            }
            if (res.Length > 1)
            {
                res = res.Trim().Substring(0, 1).ToUpper() + res.Trim().Substring(1).ToLower();
            }
            return res;
        }

        public static string DocSo(long so)
        {
            long zero = 0;
            if (so == zero) { return mangso[0]; }
            string chuoi ="";
            string hauto = "";
            long ty = 0;
            do
            {
                ty = so % 1000000000;
                so = so / 1000000000;
                if (so > zero)
                {
                    chuoi = DocHangTrieu(ty, true) + hauto + chuoi;
                }
                else
                {
                    chuoi = DocHangTrieu(ty, false) + hauto + chuoi;
                }
                hauto = " tỷ";
            } while (so > zero);
            if(chuoi.Length > 1)
            {
                chuoi = chuoi.Substring(0, 2).ToUpper() + chuoi.Substring(2).ToLower();
            }
            return chuoi;
        }

        public static string DocHangChuc(long so, bool daydu)
        {
            string chuoi = "";
            long chuc = so / 10;
            long donvi = so % 10;
            if(chuc > 1)
            {
                chuoi = " " + mangso[chuc] + " mươi";
                if(donvi == 1)
                {
                    chuoi += " mốt";
                }
            }
            else if(chuc==1)
            {
                chuoi = " mười";
                if (donvi == 1)
                {
                    chuoi += " một";
                }
            }else if(daydu && donvi > 0)
            {
                chuoi = " lẻ";
            }if(donvi==5 && chuc > 1)
            {
                chuoi += " lăm";
            }else if(donvi > 1 ||(donvi==1 && chuc == 0))
            {
                chuoi += " " + mangso[donvi];
            }
            return chuoi;
        }

        public static string DocBlock(long so, bool daydu)
        {
            string chuoi = "";
            long tram = so / 100;
            so = so % 100;
            if(daydu || tram > 0)
            {
                chuoi = " " + mangso[tram] + " trăm";
                chuoi += DocHangChuc(so, true);
            }
            else
            {
                chuoi = DocHangChuc(so, false);
            }
            return chuoi;

        }

        public static string DocHangTrieu(long so, bool daydu)
        {
            string chuoi = "";
            long trieu = so / 1000000;
            so = so % 1000000;
            if(trieu > 0)
            {
                chuoi = DocBlock(trieu, daydu) + " triệu";
                daydu = true;
            }
            long nghin = so / 1000;
            if(nghin > 0)
            {
                chuoi += DocBlock(nghin, daydu) + " nghìn";
                daydu = true;
            }if(so > 0)
            {
                chuoi += DocBlock(so, daydu);
            }return chuoi;
        }

        private static string Ones(string number)
        {
            int _number = Convert.ToInt32(number);
            string name = "";
            switch (_number)
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
                    name = "Nice";
                    break;
            }
            return name;
        }
        private static string Tens(string number)
        {
            int _number = Convert.ToInt32(number);
            string name = "";
            switch (_number)
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
                    if (_number > 0)
                    {
                        name = Tens(number.Substring(0, 1) + "0" + " " + Ones(number.Substring(1)));
                    }
                    break;
            }
            return name;
        }

        public static string ConvertWholeNumber(string number)
        {
            string word = "";
            try
            {
                bool isDone = false;
                double dblAmt = Convert.ToDouble(number);
                if(dblAmt > 0)
                {
                    int numDigits = number.Length;
                    int pos = 0;
                    string place = "";
                    switch (numDigits)
                    {
                        case 1:
                            word = Ones(number);
                            isDone = true;
                            break;
                        case 2:
                            word = Tens(number);
                            isDone = true;
                            break;
                        case 3:
                            pos = (numDigits % 3) + 1;
                            place = " Hundred ";
                            break;
                        case 4:
                        case 5:
                        case 6:
                            pos = (numDigits % 4) + 1;
                            place = " Thousand ";
                            break;
                        case 7:
                        case 8:
                        case 9:
                            pos = (numDigits % 7) + 1;
                            place = " Million ";
                            break;
                        case 10:
                        case 11:
                        case 12:
                            pos = (numDigits % 10) + 1;
                            place = " Billion ";
                            break;
                        default:
                            isDone = true;
                            break;
                    }
                    if (!isDone)
                    {
                        if(number.Substring(0,pos) != "0" && number.Substring(pos) != "0")
                        {
                            try {
                                word = ConvertWholeNumber(number.Substring(0, pos) + place + ConvertWholeNumber(number.Substring(pos)));
                            } catch { }
                        }
                        else
                        {
                            word = ConvertWholeNumber(number.Substring(0, pos)) + ConvertWholeNumber(number.Substring(pos));
                        }
                    }
                    if (word.Trim().Equals(place.Trim())) word = "";
                }
                else
                {
                    word = "Zero";
                }
            }
            catch { }
            return word.Trim();
        }
        //public static string ConvertToWords(string numb, string currency)
        //{
        //    string val = "", wholeNo = numb, points = "", andStr = "", pointStr = "";
        //    string endStr = "";
        //    try
        //    {
        //        int decimalPlace = numb.IndexOf(".");
        //        if(decimalPlace > 0)
        //        {
        //            wholeNo = numb.Substring(0, decimalPlace);
        //            points = numb.Substring(decimalPlace + 1);
        //            if (Convert.ToInt32(points) > 0)
        //            {
        //                andStr = "and";
        //                endStr = "Cents" + "Only";
        //                pointStr = " " + ConvertWholeNumber(points).Trim();
        //            }
        //        }
        //        val = string.Format("{0} {1} {2}{3} {4}", ConvertWholeNumber(wholeNo).Trim(), currency, andStr, pointStr, endStr);
        //    }
        //    catch { }
        //    return val.Substring(0, 1).ToUpper() + val.Substring(1).ToLower();
        //}

        public static string GroupToWords(int num)
        {
            string[] one_to_nineteen = { "zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine", "ten", "eleven", "twelve", "thirteen", "fourteen", "fifteen", "sixteen", "seventeen", "eightteen", "nineteen" };
            string[] multiples_of_ten = { "twenty", "thirty", "forty", "fifty", "sixty", "seventy", "eighty", "ninety" };
            if(num == 0) { return ""; }
            int digit;
            string result = "";
            if(num > 99)
            {
                digit = (int)(num / 100);
                num = num % 100;
                result = one_to_nineteen[digit] + " hundred" + (num != 0 ? " and " : "");
            }
            if(num == 0) { return result.Trim(); }
            if (num > 0)
            {
                if(num < 20)
                {
                    result += " " + one_to_nineteen[num];
                }
                else
                {
                    digit = (int)(num / 10);
                    num = num % 10;
                    result += multiples_of_ten[digit - 2];
                    if(num > 0)
                    {
                        result += "-" + one_to_nineteen[num];
                    }
                }
            }
            return result.Trim();
        }

        public static string ToWords(double num)
        {
            num = Math.Truncate(num);
            if(num == 0) { return "zero"; }
            string[] groups = { "", "thousand", "million", "billion", "trillion", "quadrillion", "?", "??", "???", "????" };
            string result = "";
            int group_num = 0;
            while (num > 0)
            {
                double quotient = (int)Math.Truncate(num / 1000);
                int remainder = (int)(Math.Round(num - quotient * 1000));
                num = quotient;
                if (remainder != 0)
                {
                    result =((num > 0 && remainder < 20 && groups[group_num] == "") ? " and " +GroupToWords(remainder) : GroupToWords(remainder)) + " " + groups[group_num] + " "+result;
                    
                }
                group_num++;
            }
            return result.Trim();
        }

        public static string ConvertToWords(string numb, string currency)
        {
            string val = "", wholeNo = numb, points = "", andStr = "", pointStr = "";
            string endStr = "";
            int decimalPlace = numb.IndexOf(".");
            if(decimalPlace > 0)
            {
                wholeNo = numb.Substring(0, decimalPlace);
                points = numb.Substring(decimalPlace + 1);
                if (Convert.ToInt32(points) > 0)
                {
                    andStr = "and";
                    endStr = "Cents " + "Only";
                    points = " " + ToWords(Convert.ToDouble(points)).Trim();
                }
            }
            double num = wholeNo != ""? Convert.ToDouble(wholeNo) : 0;
            val = string.Format("{0} {1} {2}{3} {4}", ToWords(num).Trim(), currency, andStr, pointStr, endStr);
            return (val.Substring(0, 1).ToUpper() + val.Substring(1).ToLower()).Replace("  ", " ");
        }

        public static string RandomString(int size, bool lowerCase)
        {
            StringBuilder builder = new StringBuilder();
            Random random = new Random();
            char ch;
            for (int i = 0; i < size; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                builder.Append(ch);
            }
            if (lowerCase)
                return builder.ToString().ToLower();
            return builder.ToString();
        }

        public static string ToAscii(string unicode)
        {
            unicode = Regex.Replace(unicode, "\\W+", "-"); //Nếu bạn muốn thay dấu khoảng trắng thành dấu "_" hoặc dấu cách " " thì thay kí tự bạn muốn vào đấu "-"
            return unicode;
        }
    }

}
