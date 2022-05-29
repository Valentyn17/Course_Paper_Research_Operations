using CourseWorkApplication.Schedulers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CourseWorkApplication
{
    internal class Program
    {
        static void Main(string[] args)
        {
            List<Speaker> speakers;
            Scene scene1;
            Scene scene2;

            GetSpeakers(out speakers);
            GenerateSpeakers(out speakers, 12);

            foreach (Speaker speaker in speakers)
            {
                Console.WriteLine($"Number  {speaker.Number}\t" +
                    $"Start:   {speaker.StartOfSpeech.TimeOfDay}\t" +
                    $"End: {speaker.EndOfSpeech.TimeOfDay}"
                    );
            }

            Console.WriteLine();
            ProbabilityScheduler prScheduler = new ProbabilityScheduler(speakers);
            prScheduler.CalculateScedule(out scene1, out scene2);
            Console.WriteLine();
            Console.WriteLine("First scene shedule");
            scene1.ShowSchedule();
            Console.WriteLine("Second scene schedule");
            scene2.ShowSchedule();
            Console.WriteLine("Value of target function : {0}", scene1.Count + scene2.Count);

        }

        /// <summary>
        /// Gets speakers'info from file
        /// </summary>
        /// <param name="speakers"></param>
        static void GetSpeakers(out List<Speaker> speakers)  
        {
            speakers = new List<Speaker>();
            try
            {
                string[] speakersInfo = File.ReadAllLines(
                    @"E:\\III Course Semester 2\\Курсова Дослідження операцій\\CourseWorkApplication\\Speakers.txt");
                foreach (string speakerInfo in speakersInfo)
                {
                    var str=speakerInfo.Split('-');
                    var dateStringStart = "5/26/2022 "+str[0]+":00";
                    var dateStringEnd = "5/26/2022 "+ str[1] + ":00";
                    DateTime startOfSpeech = DateTime.Parse(dateStringStart, System.Globalization.CultureInfo.InvariantCulture);
                    DateTime endOfSpeech=DateTime.Parse(dateStringEnd, System.Globalization.CultureInfo.InvariantCulture);
                    speakers.Add(new Speaker(speakers.Count + 1, startOfSpeech, endOfSpeech));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// Generates task with speakers' info
        /// </summary>
        /// <param name="speakers"></param>
        /// <param name="speakersCount">Speakers count which we will generate</param>
        static void GenerateSpeakers(out List<Speaker> speakers, int speakersCount) 
        {
            speakers = new List<Speaker>();
            var rand = new Random();

            int minSpeech = 15; //мінімальна довжина виступу 15 хв
            int maxSpeech = 120; //максимальна довжина виступу 120 хв

            DateTime startOfSpeeches = new DateTime(2022, 5, 26, 8, 0, 0);
            DateTime endOfSpeeches = new DateTime(2022, 5, 26, 17, 0, 0);

            /*максимальна кількість хвилин які можна додати(-15 бо доповідь триває  не менше 15 хвилин)*/
            int maxMinutesToAdd = (int)(endOfSpeeches - startOfSpeeches).TotalMinutes - minSpeech;

            for (int i = 0; i < speakersCount; i++)
            {
                /*генерація початку виступу спікера*/
                int minutestoAddForStart = rand.Next(0, maxMinutesToAdd);
                DateTime startOfSpeech = startOfSpeeches.AddMinutes(minutestoAddForStart);

                /*якщо старт доповіді менше як за 2 год до кінця виступів  то кінець проміжку 
                 буде кінець всіх виступів, а якщо ні то обимежуємо доповідь двома годинами при генерації*/
                int endOfrange = Math.Min(minutestoAddForStart + maxSpeech, maxMinutesToAdd + minSpeech);

                /*генерація кінця доповіді спікера*/
                int minutesToAddForEnd = rand.Next(minutestoAddForStart + minSpeech, endOfrange);
                DateTime endOfSpeech = startOfSpeeches.AddMinutes(minutesToAddForEnd);
                speakers.Add(new Speaker(i + 1, startOfSpeech, endOfSpeech));

            }
        }
    }
}
