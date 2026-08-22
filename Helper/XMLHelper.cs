namespace YiRongMachine
{
    internal class XMLHelper
    {
        ///// <summary>
        ///// 序列化写XML文件
        ///// </summary>
        ///// <param name="sr"></param>
        ///// <param name="path"></param>
        //public static bool writeToXml(ConfigVariable configVariable,out string ErrorMsg)
        //{
        //    ErrorMsg = "";
        //    try
        //    {
        //        System.Xml.Serialization.XmlSerializer writer =
        //        new System.Xml.Serialization.XmlSerializer(typeof(ConfigVariable));

        //        if (!File.Exists(FilePath.XMLFilePath))
        //        {
        //            File.Create(FilePath.XMLFilePath).Close();
        //        }

        //        System.IO.FileStream filestream = System.IO.File.Create(FilePath.XMLFilePath);

        //        writer.Serialize(filestream, configVariable);
        //        filestream.Close();
        //        filestream.Dispose();
        //        return true;
        //    }
        //    catch (Exception e)
        //    {
        //        ErrorMsg = string.Format("序列化写XML文件出现异常，返回的异常信息为{0}",e .Message );
        //        return false;
        //    }

        //}

        ///// <summary>
        ///// 序列化读XML文件
        ///// </summary>
        ///// <param name="path"></param>
        ///// <returns></returns>
        //public static bool readToXml(out ConfigVariable cv ,out string ErrorMsg)
        //{
        //    ErrorMsg = "";
        //    try
        //    {
        //        if (File.Exists(FilePath.XMLFilePath ))
        //        {
        //            System.Xml.Serialization.XmlSerializer reader =
        //                  new System.Xml.Serialization.XmlSerializer(typeof(ConfigVariable));
        //            System.IO.StreamReader filestream = new System.IO.StreamReader(Directory.GetCurrentDirectory() + "\\Config\\SystemConfig.ppk");
        //            cv = (ConfigVariable)reader.Deserialize(filestream);
        //            filestream.Close();
        //            filestream.Dispose();
        //            return true;
        //        }
        //        else
        //        {
        //            ErrorMsg = string.Format("序列化读XML文件中，未在System文件夹下找到SystemConfig.ppk文件");
        //            cv = new ConfigVariable();
        //            return false;
        //        }

        //    }
        //    catch (Exception e)
        //    {
        //        ErrorMsg = string.Format("序列化读XML文件发生异常，返回的异常信息为{0}",e .Message );
        //        cv = new ConfigVariable();
        //        return false;
        //    }
        //}
    }
}