using System;
using System.Collections.Generic;
using System.Text;

namespace CourseWorkApplication.Schedulers.BFS_Scheduler
{
    public class TreeScheduler
    {
        public Node Root { get; set; }                     //кореневий вузол дерева станів (перший спікер з відсортованої вхідної множини)
        List<Speaker> _speakers;                           //вхідна множина доступних спікерів (множина Х) 

        //public int stageNumber;                            //номер сцени



       // public List<Speaker> res = new List<Speaker>();    //вихідний набір спікерів для певної сцени
       // public List<Speaker> left = new List<Speaker>();   //оновлена множина доступних спікерів (множина Х)
        public TreeScheduler(List<Speaker> Speakers)
        {
            _speakers = Speakers;
            //stageNumber = stage;
            buildTree();
        }
        public void buildTree()                            //ф-ція побудови дерева станів
        {
            foreach (Speaker i in _speakers)
            {
                if (Root == null)                          //кореневим вузлом дерева станів завжди буде перший спікер з відсортованої вхідної множини
                {
                    Root = new Node();
                    Root.speaker = i;
                    Root.PrewAccepted = Root;
                    Root.func = 1;
                }
                else
                {
                    AddToTree(i, Root);                       //викликаємо ф-цію додавання вузлів, що відповідають поточному спікеру до дерева станів
                }
            }
        }
        public void AddToTree(Speaker speaker, Node current)   //ф-ція додавання вузла, що відповідає спікеру до дерева станів
        {
            if (current.DeclinedSpeakerNode != null)           //доходимо до листка вправо
            {
                AddToTree(speaker, current.DeclinedSpeakerNode);
            }
            else
            {
                Node newNodeRight = new Node();                //у будь-якому випадку додаємо спікера у вузол правого нащадку листка (спікера не обрали)
                newNodeRight.speaker = speaker;
                newNodeRight.PrewAccepted = current.PrewAccepted; //оскільки спікера не обрали, останнім обраним спікером для нового вузла буде той, що відповідає останньому обраному для поточного вузла 
                current.DeclinedSpeakerNode = newNodeRight;
                current.DeclinedSpeakerNode.func = current.func;
            }
            if (current.AcceptedSpeakerNode != null)           //доходимо до листка вліво
            {
                AddToTree(speaker, current.AcceptedSpeakerNode);
            }
            else
            {
                if (speaker.StartOfSpeech >= current.PrewAccepted.speaker.EndOfSpeech)  //якщо виступ не накладається на вистууп останньього обраного спікера для листка, то 
                {                                                         //додаємо спікера у вузол правого лівого листка (спікера обрали)
                    Node newNodeLeft = new Node();
                    newNodeLeft.speaker = speaker;
                    newNodeLeft.PrewAccepted = newNodeLeft;               //оскільки спікера обрали, останнім обраним спікером для нового вузла буде цей же спікер
                    current.AcceptedSpeakerNode = newNodeLeft;
                    current.AcceptedSpeakerNode.func = current.func + 1;
                }
            }
        }
        public void calculateShedule(out Scene  scene1, out Scene scene2)                         //функція складання розкладу по дереву станів
        {
            scene1 = new Scene();
            scene2 = new Scene();

            scene1.AddSpeaker(Root.speaker);                             //додаємо виступ спікера, що відповідає кореневому вузлу у розклад
            _speakers.Remove(Root.speaker);
            calculateShedule_Helper(Root, scene1);                     //обираємо інших спікерів та додаємо їх виступи у розклад

            Root = null;   // обнуляємо корневий елемент
            buildTree();   // //будуємо дерево для другої сцени
            scene2.AddSpeaker(Root.speaker);  
            calculateShedule_Helper(Root, scene2);

        }
        public void calculateShedule_Helper(Node parent, Scene scene)       //допоміжна рекурсивна функція для складання розкладу по дереву станів
        {
            if (parent.AcceptedSpeakerNode != null)            //проходимо по дереву станів в крайній лівий листок
            { 
                var speaker=parent.AcceptedSpeakerNode.speaker;
                scene.AddSpeaker(speaker);   //додаємо спікера, що відповідає обраному вузлу у вихідну множину
               
                _speakers.Remove(speaker);
                calculateShedule_Helper(parent.AcceptedSpeakerNode,scene);
            }
        }
    }
}
