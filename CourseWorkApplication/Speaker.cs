using System;
using System.Collections.Generic;
using System.Text;

namespace CourseWorkApplication
{
    public class Speaker
    {
        public int Number { get; set; }
        public DateTime StartOfSpeech { get; set; }
        public DateTime EndOfSpeech { get; set; }
        public double Probability { get; set; }

        public Speaker(int number, DateTime startOfSpeech, DateTime endOfSpeech)
        {
            Number = number;
            StartOfSpeech=startOfSpeech;
            EndOfSpeech=endOfSpeech;
            Probability = 1/(endOfSpeech - startOfSpeech).TotalMinutes;
        }
    }
}
