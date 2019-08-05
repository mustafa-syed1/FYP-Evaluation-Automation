using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace ConsoleApp3
{
    class Program
    {

        public void printAllXML()
        {
            //Loading XML file
            XmlDocument doc = new XmlDocument();
            doc.Load("file.xml");

            // Getting the root node
                 XmlNode currNode = doc.DocumentElement.FirstChild;
                if (currNode == null)
                    return;

            while (true)
            { 
                
                Console.WriteLine(currNode.InnerXml);
                Console.WriteLine("\n");
                currNode = currNode.NextSibling;
                if (currNode == null) 
                    return;
            }


        }

        public XmlNode GetNode(string uniqueAttribute, XmlDocument doc)
        {
            XmlNamespaceManager nsmgr = new XmlNamespaceManager(doc.NameTable);
            string xPathString = "//FYPAutomation//Classes //Class[@id='4']";
            XmlNode xmlNode = doc.DocumentElement.SelectSingleNode(xPathString);
            return xmlNode;
        }

        public void GetNodeData()
        {
            XmlDocument doc = new XmlDocument();
            doc.Load("file.xml");

            XmlNode node = GetNode("1", doc);
            XmlElement nodeElement = (XmlElement)node;
            XmlAttribute attr = nodeElement.GetAttributeNode("id");

            Console.WriteLine(attr.InnerXml);
            Console.WriteLine(nodeElement["name"].InnerXml);
        }

        public void GetSameNodes()
        {

            XmlDocument doc = new XmlDocument();
            doc.Load("file.xml");
            XmlNode root = doc.DocumentElement;

            XmlNodeList nodelist;
            nodelist = root.SelectNodes("descendant::Classes/Class[name='FYP1']");

            foreach (XmlNode node in nodelist)
            {
                Console.WriteLine(node.InnerXml);
                Console.WriteLine("\n");
            }
        }

        public void GetNodebyTitle()
        {
            XmlDocument doc = new XmlDocument();
            doc.Load("file.xml");

            XmlNodeList nodelist = doc.GetElementsByTagName("Class");
            for (int i = 0; i < nodelist.Count; i++)
            {
                Console.WriteLine(nodelist[i].InnerXml);
            }
        }

        public void AddNode()
        {
            XmlDocument doc = new XmlDocument();
            doc.Load("file.xml");

            XmlElement eClass = doc.CreateElement("Class");

            XmlAttribute eId = doc.CreateAttribute("id");
            eId.Value = "4";
            eClass.Attributes.Append(eId);

            XmlElement ename = doc.CreateElement("name");
            ename.InnerText = "ms-thesis";
            eClass.AppendChild(ename);

            XmlNode currNode = doc.DocumentElement.FirstChild;
            XmlElement currElement = (XmlElement)currNode;
            currNode.AppendChild(eClass);
            doc.Save("file.xml");
            doc.Save(Console.Out);

        }

        public void deleteNode(XmlNode node)
        {
            XmlNode prevNode = node.PreviousSibling;

            node.ParentNode.RemoveChild(node);


            if (prevNode.NodeType == XmlNodeType.Whitespace ||
                prevNode.NodeType == XmlNodeType.SignificantWhitespace)
            {
                prevNode.ParentNode.RemoveChild(prevNode);
            }
            
        }

        //Proper Functions for creation
        public XmlElement AddQuiz(int id, int wtg, string name,XmlDocument doc)
        {
            //Add Node 
            XmlElement eQuiz = doc.CreateElement("quiz");

            //Add quiz ID
            XmlAttribute eId = doc.CreateAttribute("id");
            eId.Value = id.ToString();
            eQuiz.Attributes.Append(eId);

            //Add Quiz Weightage
            XmlElement ename = doc.CreateElement("name");
            ename.InnerText = name;
            eQuiz.AppendChild(ename);

            XmlElement ewtg = doc.CreateElement("weightage");
            ewtg.InnerText = wtg.ToString();
            eQuiz.AppendChild(ewtg);

            return eQuiz;
        }

        public XmlNode MakeTemplate()
        {
            XmlDocument doc = new XmlDocument();
            XmlNode docNode = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
            doc.AppendChild(docNode);

            XmlNode rootNode = doc.CreateElement("FYPAutomation");
            doc.AppendChild(rootNode);

            XmlNode rClasses = doc.CreateElement("Classes");
            XmlNode rSettings = doc.CreateElement("Settings");
            rootNode.AppendChild(rClasses);
            rootNode.AppendChild(rSettings);

            XmlNode sGeneral = doc.CreateElement("GeneralSettings");
            rSettings.AppendChild(sGeneral);

            doc.Save("file.xml");

            return rootNode;


        }
        public void AddClass(int cId, string cName,String cDate, int evlCount, int quizCount)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load("file.xml");

            XmlElement eClass = doc.CreateElement("Class");

            //Adding Id attribute
            XmlAttribute eId = doc.CreateAttribute("id");
            eId.Value = cId.ToString();
            eClass.Attributes.Append(eId);

            //Adding Name
            XmlElement ename = doc.CreateElement("name");
            ename.InnerText = cName;
            eClass.AppendChild(ename);

            //Adding Start Date
            XmlElement edate = doc.CreateElement("startDate");
            edate.InnerText = cDate;
            eClass.AppendChild(edate);

            //Evaluations
            XmlElement eEval = doc.CreateElement("evaluaions");
            eClass.AppendChild(eEval);

                    //No of evaluations as an attribute of evaluations 
                    XmlAttribute evalCount = doc.CreateAttribute("count");
                    evalCount.Value = evlCount.ToString();
                    eEval.Attributes.Append(evalCount);

                    //Add Quizes
                    XmlElement quizzes = doc.CreateElement("quizzes");
                    eEval.AppendChild(quizzes);

                                    XmlAttribute qzCount = doc.CreateAttribute("count");
                                    qzCount.Value = quizCount.ToString();
                                    quizzes.Attributes.Append(qzCount);


                                    //Add Quiz,(loop)
                                    XmlElement quiz = AddQuiz(1, 5, "quiz1",doc);
                                    quizzes.AppendChild(quiz);



            //Formatting
           // eClass.InnerXml = eClass.InnerXml.Replace(eClass.InnerXml,"\n" + eClass.InnerXml + "\n");
            XmlNode currNode = doc.DocumentElement.FirstChild;
            XmlElement currElement = (XmlElement)currNode;
            currNode.AppendChild(eClass);
            doc.Save("file.xml");
            doc.Save(Console.Out);

        }


        static void Main(string[] args)
        {
            Program program = new Program();
            // program.printAllXML();
            //program.GetNodeData();
            //program.GetSameNodes();
            // program.GetNodebyTitle();
            // program.AddNode();

            XmlNode doc = program.MakeTemplate(); 
            program.AddClass(4, "MSThesis", "23/2/2012", 5, 5);


            //delete Node
            //XmlNode eNode=program.GetNode("4", doc);
            //program.deleteNode(eNode);
            //doc.Save("file.xml");
            //doc.Save(Console.Out);













            //Reading elememts  with the same id or attribute


        }


    }
}
