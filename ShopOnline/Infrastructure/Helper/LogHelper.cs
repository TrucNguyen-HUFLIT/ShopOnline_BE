using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.IO;

namespace ShopOnline.Infrastructure.Helper
{
    public static class LogHelper
    {
        private static object Locker = new Object();
        public static void Log(string msg, string fileName)
        {
            try
            {
                lock (Locker)
                {
                    var path = $@"{Directory.GetCurrentDirectory()}/LogFile/{fileName}.txt";


                    if (File.Exists(path) == false)
                    {
                        var fs = File.Create(path);
                        fs.Close();
                    }

                    System.Console.WriteLine(msg);
                    using (var file = File.AppendText(path))
                    {
                        file.WriteLine(":--:" + DateTimeOffset.Now + ":--:" + msg);
                    }
                }

            }
            catch (System.Exception e)
            {
                Console.WriteLine(e);
            }
        }
        public static void LogText(string msg, string fileName)
        {
            try
            {
                lock (Locker)
                {
                    var path = $@"{Directory.GetCurrentDirectory()}/LogFile/{fileName}.txt";
                    if (File.Exists(path) == false)
                    {
                        var fs = File.Create(path);
                        fs.Close();
                    }

                    System.Console.WriteLine(msg);
                    using (var file = File.AppendText(path))
                    {
                        file.WriteLine(msg);
                    }
                }

            }
            catch (System.Exception e)
            {
                Console.WriteLine(e);
            }
        }
        public static void LogFileWithPath(string msg, string path)
        {
            try
            {
                lock (Locker)
                {
                    if (File.Exists(path) == false)
                    {
                        var fs = File.Create(path);
                        fs.Close();
                    }

                    System.Console.WriteLine(msg);
                    using (var file = File.AppendText(path))
                    {
                        file.WriteLine(msg);
                    }
                }

            }
            catch (System.Exception e)
            {
                Console.WriteLine(e);
            }
        }
        public static void Log(object obj)
        {
            var t = JsonConvert.SerializeObject(obj);
            Log(t, "logObject" + obj.GetType().Name);
            //Log(t, "logObject" + obj.GetType().Name, "C:/Donezo/Api");
        }
        public static void Log(System.Exception ex, string fileName = null, string note = "")
        {
            var date = DateTimeOffset.Now;
            if (ex is DbUpdateException dbEx)
            {
                Log(note +
                    $"-Log {date}: {dbEx.Message}.StackTrace: {dbEx.StackTrace}. InnerExceptionMessage: {dbEx.InnerException?.Message}. InnerExceptionMessage: {dbEx.InnerException?.InnerException}.", $"{date.Day}-{date.Month}UpdateException");
            }

            if (fileName != null)
            {
                Log(note + $"-Log {date}: {ex.Message}.StackTrace: {ex.StackTrace}. InnerExceptionMessage: {ex.InnerException?.InnerException}.", "Exception" + fileName);

            }
            else
                Log(note +
                $"-Log {date}: {ex.Message}.StackTrace: {ex.StackTrace}. InnerExceptionMessage: {ex.InnerException?.InnerException}.", $"{date.Day}-{date.Month}SystemException");
        }

    }
}
