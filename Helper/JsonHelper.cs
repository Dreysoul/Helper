using Newtonsoft.Json;
using System;
using System.IO;

namespace YiRongMachine
{
    public class JsonHelper
    {
        /// <summary>
        /// 写入Json文件
        /// </summary>
        /// <param name="value"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public static bool WriteJsonFile(object value, string path, ref string ErrorMsg)
        {
            try
            {
                string s = JsonConvert.SerializeObject(value);
                string formattedJson = JsonConvert.SerializeObject(JsonConvert.DeserializeObject(s), Formatting.Indented);
                if (!File.Exists(path))
                {
                    File.Create(path).Close();
                }

                File.WriteAllText(path, formattedJson);
                return true;
            }
            catch (Exception e)
            {
                ErrorMsg = e.Message;
                return false;
            }
        }

        /// <summary>
        /// 读取Json文件为字典集合
        /// </summary>
        /// <param name="path"></param>
        public static bool ReadJsonFile<T>(string path, ref T value, ref string ErrorMsg)
        {
            try
            {
                string s = File.ReadAllText(path);
                //FileStream fs = new FileStream(path, System.IO.FileMode.Open, System.IO.FileAccess.Read);
                //StreamReader sr = new StreamReader(fs, Encoding.Default);
                //string s = sr.ReadToEnd();
                value = JsonConvert.DeserializeObject<T>(s);
                //fs.Close();
                //sr.Close();
                return true;
            }
            catch (Exception e)
            {
                ErrorMsg = e.Message;
                return false;
            }
        }
    }
}