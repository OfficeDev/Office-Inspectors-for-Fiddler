namespace MAPIAutomationTest
{
    using System.Collections.Generic;
    using System.Configuration;
    using System.IO;
    using System.Text;
    using System.Xml;

    /// <summary>
    /// The class is used to generate the HTML report.
    /// </summary>
    public class GenerateReport
    {
        /// <summary>
        /// The method is used to generate the message coverage report
        /// </summary>
        public static void GenerateCoverageReport()
        {
            string resultXml = TestBase.TestingfolderPath + Path.DirectorySeparatorChar + "RopsCopy.xml";
            List<string> results = ReadXML(resultXml);
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(resultXml);
            XmlNodeList nodeTrue = xmlDoc.SelectNodes(string.Format("data-set/structure/covered[text()='True']"));
            XmlNodeList nodeFalse = xmlDoc.SelectNodes(string.Format("data-set/structure/covered[text()='false']"));

            string coverage = ((double)nodeTrue.Count / (double)(nodeTrue.Count + nodeFalse.Count)).ToString("p");
            StringBuilder result = new StringBuilder();
            result.Append("<html>");
            result.Append("<style>");
            result.Append("table, tr, td {border: 1px solid black;}");
            result.Append("</style>");
            result.Append("<body>");
            result.Append("<h2>MAPI Inspector Message Coverage Statistics</h2>");
            result.Append("<hr>");
            result.Append(string.Format("<h2>Checked Messages:{0}</h2>", coverage));
            result.Append("<table>");

            // Add the rows got from XML file
            foreach (string s in results)
            {
                string[] xmlResult = s.Split(':');
                result.Append("<tr>");
                for (int i = 0; i < xmlResult.Length; i++)
                {
                    result.Append("<td>");
                    result.Append(xmlResult[i]);
                    result.Append("</td>");
                }

                result.Append("</tr>");
            }

            result.Append("</table>");
            result.Append("<hr>");
            result.Append("</body>");
            result.Append("</html>");
            //using (FileStream fs = new FileStream(TestBase.testingfolderPath + Path.DirectorySeparatorChar + "test.html", FileMode.Create))
            using (FileStream fs = new FileStream("test.html", FileMode.Create))
            {
                using (StreamWriter w = new StreamWriter(fs, Encoding.UTF8))
                {
                    w.Write(result.ToString());
                }
            }
        }

        /// <summary>
        /// Read the XML file to get the details message coverage information.
        /// </summary>
        /// <param name="filePath">The XML file path</param>
        /// <returns>The list of string contains the details message coverage information</returns>
        public static List<string> ReadXML(string filePath)
        {
            List<string> resultList = new List<string>();
            string result = string.Empty;
            string xmlElements = string.Empty;

            XmlDocument xmlDoc = new XmlDocument();

            if (File.Exists(filePath))
            {
                XmlTextReader rdrXml = new XmlTextReader(filePath);
                do
                {
                    switch (rdrXml.NodeType)
                    {
                        case XmlNodeType.Text:
                            result += rdrXml.Value + ":";
                            break;
                        case XmlNodeType.Element:
                            if (resultList.Count == 0 && rdrXml.Name != "data-set" && rdrXml.Name != "structure")
                            {
                                xmlElements += rdrXml.Name + ":";
                            }

                            break;
                        case XmlNodeType.EndElement:
                            if (result != string.Empty && rdrXml.Name == "testcase")
                            {
                                if (resultList.Count == 0)
                                {
                                    xmlElements = xmlElements.Remove(xmlElements.LastIndexOf(':'));
                                    resultList.Add(xmlElements);
                                }

                                result = result.Remove(result.LastIndexOf(':'));
                                resultList.Add(result);
                                result = string.Empty;
                            }

                            break;
                    }
                }
                while (rdrXml.Read());
            }

            return resultList;
        }
    }
}
