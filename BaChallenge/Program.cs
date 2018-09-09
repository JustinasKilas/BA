using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace BaChallenge
{
    public class ListEntry
    {
        public int DARZ_ID;
        public string SCHOOL_NAME;
        public int TYPE_ID;
        public string TYPE_LABEL;
        public int LAN_ID;
        public string LAN_LABEL;
        public int CHILDS_COUNT;
        public int FREE_SPACE;

        public ListEntry(string var0, string var1, string var2, string var3, string var4, string var5, string var6,
            string var7)
        {
            DARZ_ID = int.Parse(var0);
            SCHOOL_NAME = var1;
            TYPE_ID = int.Parse(var2);
            TYPE_LABEL = var3;
            LAN_ID = int.Parse(var4);
            LAN_LABEL = var5;
            CHILDS_COUNT = int.Parse(var6);
            FREE_SPACE = int.Parse(var7);
        }
    }

    public class Lans
    {
        public string Lan;
        public int Total;
        public double Percent;

        public Lans(string lan, int total)
        {
            Lan = lan;
            Total = total;
        }

        public void AddToTotal(int i)
        {
            Total += i;
        }

        public void calc(int i)
        {
            Percent = Math.Round(Total * 1.0 / i * 100, 2);
        }
    }

    internal class Program
    {
        private static List<ListEntry> List;

        private static List<string> Output = new List<string>();

        public static void Main(string[] args)
        {
            int listSize = ReadCSV("Darzeliu galimu priimti ir lankantys vaikai2018.csv") - 1;
            int childsCountMin = FindMin();
            int childsCountMax = FindMax();

            Output.Add("CHILDS_COUNT min value: " + childsCountMin);
            Output.Add("CHILDS_COUNT max value: " + childsCountMax);

            Output.Add("\nSchools with max CHILD_COUNT");
            FindNeededEntries(listSize, childsCountMax);
            Output.Add("\nSchools with min CHILD_COUNT");
            FindNeededEntries(listSize, childsCountMin);

            LanCount();

            Output.Add("\nSchools with 2 to 4 free spots:");
            List<string> schoolsSubList = FilterSchools(listSize);

            foreach (string entry in schoolsSubList)
            {
                Output.Add(entry);
            }

            Output.ForEach(Console.WriteLine);

            using (StreamWriter outputFile = new StreamWriter("Result.txt"))
            {
                Output.ForEach(outputFile.WriteLine);
            }
        }

        public static void LanCount()
        {
            int freeSum = 0;
            List<Lans> lans = new List<Lans>();

            foreach (ListEntry entry in List)
            {
                freeSum += entry.FREE_SPACE;
                bool inLanList = false;
                foreach (Lans lan in lans)
                {
                    if (lan.Lan == entry.LAN_LABEL)
                    {
                        lan.AddToTotal(entry.FREE_SPACE);
                        inLanList = true;
                    }
                }

                if (!inLanList)
                {
                    lans.Add(new Lans(entry.LAN_LABEL, entry.FREE_SPACE));
                }
            }

            Output.Add("\nFree spots by language:");
            foreach (Lans lan in lans)
            {
                lan.calc(freeSum);
                Output.Add(lan.Lan + " " + lan.Percent + "%");
            }
        }

        public static List<string> FilterSchools(int n)
        {
            List<string> list = new List<string>();
            for (int i = 0; i < n; i++)
            {
                if (List[i].FREE_SPACE >= 2 && List[i].FREE_SPACE < 5)
                {
                    if (!list.Contains(List[i].SCHOOL_NAME))
                        list.Add(List[i].SCHOOL_NAME);
                }
            }

            list.Sort();
            list.Reverse();
            return list;
        }

        public static int ReadCSV(String fileName)
        {
            List = new List<ListEntry>();

            int size = 0;

            using (var reader = new StreamReader(fileName))
            {
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    if (size == 0)
                    {
                        size++;
                        continue;
                    }

                    var values = line.Split(';');

                    List.Add(new ListEntry(values[0], values[1].Trim('"'), values[2], values[3], values[4], values[5],
                        values[6], values[7]));
                    size++;
                }
            }

            return size;
        }

        public static int FindMin()
        {
            int min = List[0].CHILDS_COUNT;
            foreach (ListEntry i in List)
            {
                if (i.CHILDS_COUNT < min)
                {
                    min = i.CHILDS_COUNT;
                }
            }

            return min;
        }

        public static int FindMax()
        {
            int min = List[0].CHILDS_COUNT;
            foreach (ListEntry i in List)
            {
                if (i.CHILDS_COUNT > min)
                {
                    min = i.CHILDS_COUNT;
                }
            }

            return min;
        }

        public static void FindNeededEntries(int n, int num)
        {
            for (int i = 0; i < n; i++)
            {
                if (List[i].CHILDS_COUNT == num)
                {
                    Output.Add(FormWord(i));
                }
            }
        }

        public static string FormWord(int i)
        {
            string part1 = List[i].SCHOOL_NAME.Trim('"').Substring(0, 3);
            string part2 = Regex.Replace(List[i].TYPE_LABEL.Replace("iki", "-"), "[^0-9.,-]", "");
            string part3 = List[i].LAN_LABEL.Substring(0, 4);
            string word = part1 + "_" + part2 + "_" + part3;
            return word;
        }
    }
}