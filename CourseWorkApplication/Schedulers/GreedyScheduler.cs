using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CourseWorkApplication.Schedulers
{
    public class GreedyScheduler
    {
        private List<Speaker> _speakers;                         //вхідна множина доступних спікерів (множина Х)
        public GreedyScheduler(List<Speaker> Speakers)
        {
            _speakers = Speakers;
        }
        public void calculateScedule(out Scene scene1,  out Scene scene2)
        {
            scene1 = new Scene();
            scene2 = new Scene();   
            scene1.AddSpeaker(_speakers[0]);                        //першим спікером обираємо першого з відсортованої множини доступних спікерів (множина Х)

            for (int i = 1; i < _speakers.Count; i++)       //розглядаємо доступних по черзі (кожен окремо, враховуємо лише поточного)
            {
                if (scene1.CheckForAddGreedyAlg(_speakers[i])) //якщо час спікера, якого розглядаємо, не накладається на час останнього обраного
                {
                    scene1.AddSpeaker(_speakers[i]);                //то додаємо спікера до вихідної множини F1
                }
                else if(scene2.CheckForAddGreedyAlg(_speakers[i]))//якщо час спікера, якого розглядаємо, не накладається на час останнього обраного
                {
                    scene2.AddSpeaker(_speakers[i]);            //то додаємо спікера до вихідної множини F2
                }
            }
        }
    }
}

