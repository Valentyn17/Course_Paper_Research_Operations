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
                    $"End: {speaker.EndOfSpeech.TimeOfDay}\tProbability: {speaker.Probability: 0.000}"
                    );
            }
            ProbabilityAlgorithm(speakers, out scene1, out scene2);
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("First scene shedule");
            scene1.ShowSchedule();
            Console.WriteLine("Second scene schedule");
            scene2.ShowSchedule();
            Console.WriteLine("Value of target function : {0}", scene1.Count+scene2.Count);

        }




        static void ProbabilityAlgorithm(List<Speaker> speakersList, out Scene scene1, out Scene scene2)
        {
            scene1 = new Scene();
            scene2 = new Scene();
            var Rand = new Random();
            int counterForRepeating = 0;

            while (counterForRepeating<30) {
                int inCounter = 0;
                var scene1now = new Scene();
                var scene2now = new Scene();
                var speakers = speakersList.ToList();

                while (inCounter<10 || speakers.Count==0)
                {
                    var randomDouble = Rand.NextDouble();
                    CountProbability(speakers);
                    var speaker = speakers.SkipWhile(x => x.Probability <= randomDouble).ToList()[0];
                    if (scene1now.CheckForAdding(speaker))
                    {
                        scene1now.AddSpeaker(speaker);
                        speakers.Remove(speaker);
                        
                        inCounter = 0;
                    }
                    else if(scene2now.CheckForAdding(speaker))
                    {
                        scene2now.AddSpeaker(speaker); 
                        speakers.Remove(speaker);

                        inCounter = 0;
                    }
                    else
                    {
                        inCounter++;
                    }
                }
                if (scene1now.Count + scene2now.Count > scene1.Count + scene2.Count)
                {
                    scene1 = scene1now;
                    scene2 = scene2now;
                    counterForRepeating = 0;
                }
                else {
                    counterForRepeating++;
                }
            }
            
        }


        static List<Speaker> CountProbability(List<Speaker> speakers)
        {
            double probabilitySum = 0;
            foreach (var item in speakers)
            {
                item.Probability = 1 / (item.EndOfSpeech - item.StartOfSpeech).TotalMinutes;
                probabilitySum += item.Probability;
            }
            double tempProbability = speakers[0].Probability;
            speakers[0].Probability = tempProbability / probabilitySum;
            for (int i = 1; i < speakers.Count; i++)
            {
                tempProbability += speakers[i].Probability;
                speakers[i].Probability = tempProbability / probabilitySum;
            }
            return speakers;
        }

        
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
                int minutestoAddForStart = rand.Next(0, maxMinutesToAdd);
                DateTime startOfSpeech = startOfSpeeches.AddMinutes(minutestoAddForStart);

                /*якщо старт доповіді менше як за 2 год до кінця виступів  то кінець проміжку 
                 буде кінець всіх виступів, а якщо ні то обимежуємо доповідь двома годинами при генерації*/
                int endOfrange = Math.Min(minutestoAddForStart + maxSpeech, maxMinutesToAdd + minSpeech);
                int minutesToAddForEnd = rand.Next(minutestoAddForStart + minSpeech, endOfrange);
                DateTime endOfSpeech = startOfSpeeches.AddMinutes(minutesToAddForEnd);
                speakers.Add(new Speaker(i + 1, startOfSpeech, endOfSpeech));

            }
        }
    }
}
