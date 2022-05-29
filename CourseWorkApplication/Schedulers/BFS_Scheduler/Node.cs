using System;
using System.Collections.Generic;
using System.Text;

namespace CourseWorkApplication.Schedulers.BFS_Scheduler
{
    public class Node
    {
        public Node AcceptedSpeakerNode { get; set; }  //лівий нащадок - спікер, обраний до виступу
        public Node DeclinedSpeakerNode { get; set; }  //правий нащадок - спікер, не обраний до виступу
        public Speaker speaker { get; set; }           //спікер, що відповідає вузлу дерева станів
        public Node PrewAccepted { get; set; }         //останній спікер, обраний до виступу для цього вузла
        public int func { get; set; }                  //кількість спікерів, обраних до виступу для цього вузла

    }
}
