using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CourseWorkApplication
{
    public class ProbabilityScheduler
    {
        private List<Speaker> _speakers;                         //вхідна множина доступних спікерів (множина Х)
        public ProbabilityScheduler(List<Speaker> speakers)
        {
            _speakers = speakers;
        }

        /// <summary>
        /// Returns two scenes with scheduled speakers.
        /// </summary>
        /// <param name="scene1"></param>
        /// <param name="scene2"></param>
        public void CalculateScedule(out Scene scene1, out Scene scene2) 
        {
            scene1 = new Scene();  //перша сцена
            scene2 = new Scene();  //друга сцена
            var Rand = new Random();
            int counterForRepeating = 0;

            while (counterForRepeating < 30)   //вихід з циклу коли рекордний розвязок не збільшується протягом m разів
            {
                int inCounter = 0;              
                var scene1now = new Scene();
                var scene2now = new Scene();
                var speakers = _speakers.ToList();

                while (inCounter < 10 || speakers.Count == 0)  //вихід з циклу коли ми протягом t разів не додаємо ніякого спікера до сцен
                {
                    var randomDouble = Rand.NextDouble();
                    CountProbabilities(speakers);

                    /*Вибираємо спікера відповідно до згенерованого  числа та ймовірності вибору кожного спікера*/
                    var speaker = speakers.SkipWhile(x => x.Probability <= randomDouble).ToList()[0];  

                    if (scene1now.CheckForAddProbabilityAlg(speaker)) //перевірка чи можемо додати до першої сцени
                    {
                        scene1now.AddSpeaker(speaker);
                        speakers.Remove(speaker);

                        inCounter = 0;
                    }
                    else if (scene2now.CheckForAddProbabilityAlg(speaker))  //перевірка чи можемо додати до другої сцени
                    {
                        scene2now.AddSpeaker(speaker);
                        speakers.Remove(speaker);

                        inCounter = 0;
                    }
                    else   //якщо не додали спікера до жодної зі  сцен то збільшуємо лічильник виходу з циклу
                    {
                        inCounter++;
                    }
                }

                /*якщо поточний розвязок більший за рекордний то робимо  його рекордним*/
                if (scene1now.Count + scene2now.Count > scene1.Count + scene2.Count)  
                {
                    scene1 = scene1now;
                    scene2 = scene2now;
                    counterForRepeating = 0;
                }
                else //якщо поточний не більший за рекордний то збільшуємо лічильник для виходу з циклу 
                {
                    counterForRepeating++;
                }
            }
        }

        /// <summary>
        /// Counts probability for every speaker
        /// </summary>
        /// <param name="speakers">Lit of speakers which probabilities we want to count.</param>
        /// <returns>List of speakers with counted probabilities.</returns>
        private List<Speaker> CountProbabilities(List<Speaker> speakers)
        {
            double probabilitySum = 0;
            foreach (var item in speakers) //рахуємо суму всіх ймовірностей спікерів
            {
                item.Probability = 1 / (item.EndOfSpeech - item.StartOfSpeech).TotalMinutes;
                probabilitySum += item.Probability;
            }

            double tempProbability = speakers[0].Probability;
            speakers[0].Probability = tempProbability / probabilitySum;

            for (int i = 1; i < speakers.Count; i++) //рахуємо відносну накопичувальну ймовірність для кожного спікера
            {
                tempProbability += speakers[i].Probability;
                speakers[i].Probability = tempProbability / probabilitySum;
            }
            return speakers;
        }


    }
}
