using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CourseWorkApplication
{
    public class Scene
    {
        private List<Speaker> Speakers { get; set; }

        public int Count { get => Speakers.Count;}

        public Scene()
        {
            Speakers=new List<Speaker>();
        }


        public void AddSpeaker(Speaker speaker)
        {
            Speakers.Add(speaker);
        }

        public bool CheckForAdding(Speaker speaker) {
            if (Speakers.Count == 0)
                return true;
            foreach (var item in Speakers)
            {
                if (item.EndOfSpeech <=speaker.EndOfSpeech && item.EndOfSpeech >= speaker.StartOfSpeech)
                    return false;
                if (item.StartOfSpeech >= speaker.StartOfSpeech && item.StartOfSpeech < speaker.EndOfSpeech)
                    return false;
                if (speaker.StartOfSpeech >= item.StartOfSpeech
                    && speaker.EndOfSpeech <= item.EndOfSpeech) //випадок коли вхідний проміжок знаходиться всередині якогось
                    return false;
            }
            return true;
        } 

        public void ShowSchedule() {
            foreach (Speaker speaker in Speakers)
            {
                Console.WriteLine($"Number  {speaker.Number}\t" +
                    $"Start:   {speaker.StartOfSpeech.TimeOfDay}\t" +
                    $"End: {speaker.EndOfSpeech.TimeOfDay}"
                    );
            }
            Console.WriteLine($"Count of Speakers : {Speakers.Count}");
        }

    }
}
