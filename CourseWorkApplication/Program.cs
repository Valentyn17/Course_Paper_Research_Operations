using CourseWorkApplication.Schedulers;
using CourseWorkApplication.Schedulers.BFS_Scheduler;
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
            Scene scene1=new Scene();
            Scene scene2=new Scene();
            GreedyScheduler grScheduler;
            ProbabilityScheduler probScheduler;
            TreeScheduler treeScheduler;
            int[] experimentDimensions = { 10, 20, 30, 40 };


            int menuNumber = Menu();
            while (menuNumber > 0)
            {
                switch (menuNumber)
                {
                    case 1:
                        {
                            GetSpeakers(out speakers, "Speakers");
                            speakers = speakers.OrderBy(x => x.EndOfSpeech).ToList(); //сортування за кінцем виступу(для жадібного та пошуку вшир)
                            grScheduler = new GreedyScheduler(speakers);
                            probScheduler = new ProbabilityScheduler(speakers);
                            treeScheduler = new TreeScheduler(speakers);
                            OutputSpeakers(speakers);

                            SolveProblemAndOutput(grScheduler, probScheduler, treeScheduler, scene1, scene2, speakers); //solve problem by all algorithms 
                            break;
                        }
                    case 2:
                        {
                            GetSpeakers(out speakers, "Speakers_Zinkova");
                            speakers = speakers.OrderBy(x => x.EndOfSpeech).ToList(); //сортування за кінцем виступу(для жадібного та пошуку вшир)
                            grScheduler = new GreedyScheduler(speakers);
                            probScheduler = new ProbabilityScheduler(speakers);
                            treeScheduler = new TreeScheduler(speakers);
                            OutputSpeakers(speakers);

                            SolveProblemAndOutput(grScheduler, probScheduler, treeScheduler, scene1, scene2, speakers); //solve problem by all algorithms 
                            break;
                        }
                    case 3:
                        {
                            foreach (var item in experimentDimensions)
                            {
                                Console.WriteLine();
                                Console.WriteLine("Count of speakers: {0}", item);
                                for (int i = 0; i <5; i++)
                                {
                                    GenerateSpeakers(out speakers, item);
                                    speakers = speakers.OrderBy(x => x.EndOfSpeech).ToList(); //сортування за кінцем виступу(для жадібного та пошуку вшир)
                                    grScheduler = new GreedyScheduler(speakers);
                                    probScheduler = new ProbabilityScheduler(speakers);
                                    treeScheduler = new TreeScheduler(speakers);


                                    Console.WriteLine($"\nExperiment {i + 1}\n");

                                    var watch1 = System.Diagnostics.Stopwatch.StartNew();
                                    grScheduler.CalculateSchedule(out scene1, out scene2);
                                    watch1.Stop();
                                    var elapsedMsGr = watch1.ElapsedMilliseconds;
                                    
                                    Console.ForegroundColor= ConsoleColor.DarkMagenta;
                                    Console.WriteLine("Greedy Algrithm ");
                                    Console.WriteLine($"Value of target function: {scene1.Count + scene2.Count}");
                                    Console.ResetColor();
                                    Console.WriteLine($"Time of executing(milliseconds): {elapsedMsGr}");

                                    var watch2 = System.Diagnostics.Stopwatch.StartNew();
                                    probScheduler.CalculateSchedule(out scene1, out scene2);
                                    watch2.Stop();
                                    var elapsedMsPr = watch2.ElapsedMilliseconds;

                                    Console.ForegroundColor = ConsoleColor.Cyan;
                                    Console.WriteLine("Probability Algrithm ");
                                    Console.WriteLine($"Value of target function: {scene1.Count + scene2.Count}");
                                    Console.ResetColor();
                                    Console.WriteLine($"Time of executing(milliseconds): {elapsedMsPr}");

                                    var watch3 = System.Diagnostics.Stopwatch.StartNew();
                                    treeScheduler.CalculateSchedule(out scene1, out scene2);
                                    watch3.Stop();
                                    var elapsedMsTr = watch3.ElapsedMilliseconds;
                                    Console.ForegroundColor = ConsoleColor.Green;
                                    Console.WriteLine("BFS Algrithm ");
                                    Console.WriteLine($"Value of target function: {scene1.Count + scene2.Count}");
                                    Console.ResetColor();
                                    Console.WriteLine($"Time of executing(milliseconds): {elapsedMsTr}");
                                }
                            }
                            
                            break;
                        }
                    default:
                        break;
                }
                menuNumber = Menu();
                

                

                }
        }

        /// <summary>
        /// Output  text console menu.
        /// </summary>
        /// <returns>Number of actioon which you  should to do</returns>
        static int Menu()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("It is an application for solving speakers' scheduling problem!\n");

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Select number for action which you want to choose:");
            Console.WriteLine("1 - Solve first individual problem(Zahrai)\t " +
                "2 - Solve second individual problem(Zinkova)\t " +
                "3-Do the experiment");
            Console.ResetColor();
            Console.Write("Input number here: ");
            int number = Convert.ToInt32(Console.ReadLine());
            if (number < 1 || number > 3)
                return 0;
            return number;

        }


        /// <summary>
        /// Gets speakers'info from file
        /// </summary>
        /// <param name="speakers"></param>
        static void GetSpeakers(out List<Speaker> speakers, string fileName)
        {
            speakers = new List<Speaker>();
            try
            {
                string[] speakersInfo = File.ReadAllLines(
                    @"E:\\III Course Semester 2\\Курсова Дослідження операцій\\CourseWorkApplication\\" + fileName + ".txt");
                foreach (string speakerInfo in speakersInfo)
                {
                    var str = speakerInfo.Split('-');
                    var dateStringStart = "5/26/2022 " + str[0] + ":00";
                    var dateStringEnd = "5/26/2022 " + str[1] + ":00";
                    DateTime startOfSpeech = DateTime.Parse(dateStringStart, System.Globalization.CultureInfo.InvariantCulture);
                    DateTime endOfSpeech = DateTime.Parse(dateStringEnd, System.Globalization.CultureInfo.InvariantCulture);
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

            int minSpeech = 15;        //мінімальна довжина виступу 15 хв
            int maxSpeech = 120;       //максимальна довжина виступу 120 хв

            DateTime startOfSpeeches = new DateTime(2022, 5, 26, 8, 0, 0);
            DateTime endOfSpeeches = new DateTime(2022, 5, 26, 17, 0, 0);

 
            int maxMinutesToAdd = (int)(endOfSpeeches - startOfSpeeches).TotalMinutes - minSpeech;  /*максимальна кількість хвилин які можна додати(-15 бо доповідь триває  не менше 15 хвилин)*/

            for (int i = 0; i < speakersCount; i++)
            {
                int minutestoAddForStart = rand.Next(0, maxMinutesToAdd);     /*генерація початку виступу спікера*/
                DateTime startOfSpeech = startOfSpeeches.AddMinutes(minutestoAddForStart);

                 /*якщо старт доповіді менше як за 2 год до кінця виступів  то кінець проміжку 
                 буде кінець всіх виступів, а якщо ні то обимежуємо доповідь двома годинами при генерації*/
                int endOfrange = Math.Min(minutestoAddForStart + maxSpeech, maxMinutesToAdd + minSpeech);                
                int minutesToAddForEnd = rand.Next(minutestoAddForStart + minSpeech, endOfrange);   /*генерація кінця доповіді спікера*/
                DateTime endOfSpeech = startOfSpeeches.AddMinutes(minutesToAddForEnd);
                speakers.Add(new Speaker(i + 1, startOfSpeech, endOfSpeech));

            }
        }


        /// <summary>
        /// Outputs speakers' table info
        /// </summary>
        /// <param name="speakers">List of speakers</param>
        static  void OutputSpeakers(List<Speaker> speakers)
        {
            Console.WriteLine();
            Console.WriteLine("Our Speakers:\n");
            foreach (Speaker speaker in speakers)
            {
                Console.WriteLine($"Number  {speaker.Number}\t" +
                    $"Start:   {speaker.StartOfSpeech.TimeOfDay}\t" +
                    $"End: {speaker.EndOfSpeech.TimeOfDay}"
                    );
            }
        }


        /// <summary>
        /// Solves problems by all algorithms and makes output of result
        /// </summary>
        /// <param name="grScheduler">Greedy algorithm scheduler</param>
        /// <param name="probScheduler">Probability algorithm scheduler</param>
        /// <param name="treeScheduler">BFS algorithm scheduler</param>
        /// <param name="scene1">First scene</param>
        /// <param name="scene2">Second scene</param>
        /// <param name="speakers">List of speakers which wee should to schedule</param>
        static void SolveProblemAndOutput(GreedyScheduler grScheduler, ProbabilityScheduler probScheduler, 
            TreeScheduler treeScheduler, Scene scene1, Scene scene2, List<Speaker> speakers)
        {
            grScheduler.CalculateSchedule(out scene1, out scene2); //складання розкладу жадібним
            Console.WriteLine("---------------------------");
            Console.WriteLine("Greedy Algorithm: ");
            Console.WriteLine("First scene shedule");
            scene1.ShowSchedule();
            Console.WriteLine("Second scene schedule");
            scene2.ShowSchedule();
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("Value of target function : {0}\n", scene1.Count + scene2.Count);
            Console.ResetColor();

            probScheduler.CalculateSchedule(out scene1, out scene2); //складання розкладу ймовірнісним алгоритмом
            Console.WriteLine("---------------------------");
            Console.WriteLine("Probability Algorithm: ");
            Console.WriteLine("First scene shedule");
            scene1.ShowSchedule();
            Console.WriteLine("Second scene schedule");
            scene2.ShowSchedule();
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("Value of target function : {0} \n", scene1.Count + scene2.Count);
            Console.ResetColor();

            treeScheduler.CalculateSchedule(out scene1, out scene2); //складання розкладу пошуком вшир 
            Console.WriteLine("---------------------------");
            Console.WriteLine("BFS Algorithm: ");
            Console.WriteLine("First scene shedule");
            scene1.ShowSchedule();
            Console.WriteLine("Second scene schedule");
            scene2.ShowSchedule();
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("Value of target function : {0}\n", scene1.Count + scene2.Count);
            Console.ResetColor();
        }
    }
}
