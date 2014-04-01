using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;

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

            foreach (Match item in mc)
            {
                Double _lat = double.Parse(item.Groups["lat"].Value.Replace('.', ','));
                Double _lon = double.Parse(item.Groups["lon"].Value.Replace('.', ','));
                Double _unk = double.Parse(item.Groups["unk"].Value.Replace('.', ','));

                result.Add(new GPSTrackObject(_lat, _lon, _unk));
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
