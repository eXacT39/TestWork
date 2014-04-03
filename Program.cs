using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using System.Globalization;

namespace TestProject
{
    class Program
    {
        static void Main(string[] args)
        {
            List<GPSTrackObject> resList = new List<GPSTrackObject>();
            Thread [] threads = new Thread[args.Count()];

            for (Int32 i = 0; i < args.Count(); ++i)
            {
                String inputStream = File.ReadAllText(args[i]);
                threads[i] = new Thread(() => resList.AddRange(ParseStream(inputStream)));
                threads[i].IsBackground = true;
                threads[i].Start();
            }

            waitAllThreadEndings(threads);
            outputToFile(resList);
        }

        static List<GPSTrackObject> ParseStream(String inputStream)
        {
            List<GPSTrackObject> result = new List<GPSTrackObject>();
            String pattern = (@".trkpt.lat..(?<lat>\d+\.\d+)..lon..(?<lon>\d+\.\d+)..\s+.ele.(?<unk>\d+\.\d+)");
            Regex regex = new Regex(pattern);
            MatchCollection mc = regex.Matches(inputStream);

            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("en-en");

            foreach (Match item in mc)
            {
                Double latitude = double.Parse(item.Groups["lat"].Value);
                Double longitude = double.Parse(item.Groups["lon"].Value);
                Double altitude = double.Parse(item.Groups["alt"].Value);

                result.Add(new GPSTrackObject(latitude, longitude, altitude));
            }
            return result;
        }

        static void outputToFile(List<GPSTrackObject> resList)
        {
            foreach (var obj in resList)
            {
                File.AppendAllText("output.txt", obj.ToString() + "\n");
            }
        }

        static void waitAllThreadEndings(Thread [] threads)
        {
            foreach (var thread in threads)
            {
                thread.Join();
            }
        }
    }
}
