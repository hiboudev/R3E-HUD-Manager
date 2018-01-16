using R3EHUDManager.motec.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace R3EHUDManager.motec.parser
{
    class MotecParser
    {
        private static int motecId = 0;

        public MotecParser()
        {
        }

        public List<MotecModel> Parse(string path)
        {
            var motecs = new List<MotecModel>();

            XmlDocument xml = new XmlDocument();
            xml.Load(path);

            foreach (XmlNode motecNode in xml.SelectNodes("/data/motec"))
                motecs.Add(ParseMotecNode(motecNode));

            return motecs;
        }

        private MotecModel ParseMotecNode(XmlNode motecNode)
        {
            return new MotecModel(Convert.ToInt32(motecNode.Attributes["id"].Value), motecNode["fileName"].InnerText, GetCarIds(motecNode));
        }

        private int[] GetCarIds(XmlNode motecNode)
        {
            var ids = new List<int>();

            foreach (XmlNode idNode in motecNode.SelectNodes("carId"))
                ids.Add(Convert.ToInt32(idNode.InnerText));

            return ids.ToArray();
        }
    }
}
