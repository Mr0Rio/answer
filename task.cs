using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SongsAnalyzer
{
    class Program
    {
        static List<Song_Record> Song_Record_List = new List<Song_Record>();
        static List<DistinctivePlayCount> DistinctivePlayCount_List_ = new List<DistinctivePlayCount>();
        static List<DistinctivePlayCountFinal> DistinctivePlayCountFinal_List = new List<DistinctivePlayCountFinal>();
        static Dictionary<string, Song_Record> TempQuickerChecker = new Dictionary<string, Song_Record>();
        static void Main(string[] args)
        {

            // load the Song Records into new list of Song Records...

            var lines = File.ReadAllLines("exhibitA-input.csv");

            //var lines = File.ReadAllLines("test2.csv"); //File.ReadAllLines("only1008.csv");

            foreach (string line in lines)
            {

                string[] Sub_Data = Regex.Split(line, "\t");

                if (Sub_Data[0].Contains("44BB") & Sub_Data[3].Contains("10/08"))
                {

                    Song_Record Song_Record_ = new Song_Record();



                    //Song_Record_.PLAY_ID = Sub_Data[0].Trim('"').Trim('\'');

                    Song_Record_.SONG_ID = Sub_Data[1].Trim('"').Trim('\'');

                    Song_Record_.CLIENT_ID = Sub_Data[2].Trim('"').Trim('\'');

                    //Song_Record_.PLAY_TS = Sub_Data[3].Trim('"').Trim('\'');

                    // now I filtered all records that have the date of 10/08/2016
                    // now before inserting the data in the list I can also check if it exists for the same Client ID
                    // and ignore it if it exists already for the same day and same song


                    //if (Song_Record_List.Count!=0 && Song_Record_List.Any(x => x.CLIENT_ID == Song_Record_.CLIENT_ID) && Song_Record_List.Any(y => y.SONG_ID == Song_Record_.SONG_ID))
                    if (TempQuickerChecker.Count != 0)
                    {
                        // adding two condition to check for existing client id with the same song id, before adding.
                        //if (!Song_Record_List.Any(x => x.CLIENT_ID == Song_Record_.CLIENT_ID && x.SONG_ID == Song_Record_.SONG_ID))
                        //quicker way to check is to use the hashset!!!
                        if (!TempQuickerChecker.ContainsKey(Song_Record_.CLIENT_ID +" "+ Song_Record_.SONG_ID))
                        {
                            //Song_Record_List.Add(Song_Record_);
                            TempQuickerChecker.Add(Song_Record_.CLIENT_ID + " " + Song_Record_.SONG_ID, Song_Record_);
                        }
                    }
                    else
                    {
                        //Song_Record_List.Add(Song_Record_);
                        TempQuickerChecker.Add(Song_Record_.CLIENT_ID + " " + Song_Record_.SONG_ID,Song_Record_);
                    }



                }



            }

            Console.WriteLine("Data Loaded Successfully for the specific date of 10/08/2016");

            Console.WriteLine();

            Console.WriteLine("Now to get table that shows the distribution distinct song play counts per user on date of August 10, 2016");

            Console.ReadLine();


            //before your loop
            var csv = new StringBuilder();

            csv.AppendLine(string.Format("{0},{1}", "Client ID", "Song ID"));

            foreach (var SongRecord___ in TempQuickerChecker.Values)
            {
                //in your loop
                //var first = SongRecord___.PLAY_ID;

                var first = SongRecord___.CLIENT_ID;
                var second = SongRecord___.SONG_ID;
                
                //var forth = SongRecord___.PLAY_TS;

                var newLine = string.Format("{0},{1}", first, second);
                csv.AppendLine(newLine);
            }

            //after your loop
            File.WriteAllText("Output.csv", csv.ToString());

            

            foreach (Song_Record SR in TempQuickerChecker.Values)
            {
                if (TempQuickerChecker.Count != 0)
                {
                    DistinctivePlayCount DistinctivePlayCount_ = new DistinctivePlayCount();

                    // 
                    if (DistinctivePlayCount_List_.Any(x => x.CLIENT_ID == SR.CLIENT_ID))
                    {
                        
                        int count = DistinctivePlayCount_List_.First(x => x.CLIENT_ID == SR.CLIENT_ID).PLAY_Count;
                        DistinctivePlayCount_List_.First(x => x.CLIENT_ID == SR.CLIENT_ID).PLAY_Count = count + 1;

                    }

                    else
                    {
                        DistinctivePlayCount_.CLIENT_ID = SR.CLIENT_ID;
                        DistinctivePlayCount_.PLAY_Count = 1;
                        DistinctivePlayCount_List_.Add(DistinctivePlayCount_);
                    }
                }
            }


            //before your loop
            var csv_ = new StringBuilder();

            csv_.AppendLine(string.Format("{0},{1}", "Client ID", "Play Count"));

            foreach (var DistinctivePlayCount__ in DistinctivePlayCount_List_)
            {
                //in your loop
                var first_ = DistinctivePlayCount__.CLIENT_ID;
                var second_ = DistinctivePlayCount__.PLAY_Count;
                

                var newLine = string.Format("{0},{1}", first_, second_);
                csv_.AppendLine(newLine);
            }

            //after your loop
            File.WriteAllText("Output-CountsPerClient.csv", csv_.ToString());

            //==================
            // Final output

            foreach (DistinctivePlayCount DPC in DistinctivePlayCount_List_)
            {
                if (DistinctivePlayCount_List_.Count != 0)
                {
                    DistinctivePlayCountFinal DistinctivePlayCountFinal_ = new DistinctivePlayCountFinal();

                    // 
                    if (DistinctivePlayCountFinal_List.Any(x => x.PLAY_Count == DPC.PLAY_Count))
                    {
                        
                        DistinctivePlayCountFinal_.PLAY_Count = DPC.PLAY_Count;
                       
                        int Client_count_ = DistinctivePlayCountFinal_List.First(x => x.PLAY_Count == DPC.PLAY_Count).CLIENT_Count;
                        DistinctivePlayCountFinal_List.First(x => x.PLAY_Count == DPC.PLAY_Count).CLIENT_Count = Client_count_ + 1;
                        

                    }

                    else
                    {
                        DistinctivePlayCountFinal_.PLAY_Count = DPC.PLAY_Count;
                        DistinctivePlayCountFinal_.CLIENT_Count = 1;
                        DistinctivePlayCountFinal_List.Add(DistinctivePlayCountFinal_);
                    }
                }
            }


            // getting final output

            //before your loop
            var csv_final = new StringBuilder();

            csv_final.AppendLine(string.Format("{0},{1}", "Play Count", "Client Count"));

            foreach (var DistinctivePlayCountFinal__ in DistinctivePlayCountFinal_List)
            {
                //in your loop
                var first_final = DistinctivePlayCountFinal__.PLAY_Count;
                var second_final = DistinctivePlayCountFinal__.CLIENT_Count;


                var newLine = string.Format("{0},{1}", first_final, second_final);
                csv_final.AppendLine(newLine);
            }

            //after your loop
            File.WriteAllText("Output-CountsPerClientFinal.csv", csv_final.ToString());


            // end of main class

        }

        class Song_Record
        {


            //public string PLAY_ID;
            public string SONG_ID;
            public string CLIENT_ID;
            //public string PLAY_TS;


        }
        class DistinctivePlayCount
        {
           
            public string CLIENT_ID;
            public int PLAY_Count;

        }

        class DistinctivePlayCountFinal
        {

            public int CLIENT_Count;
            public int PLAY_Count;

        }



    }
}
