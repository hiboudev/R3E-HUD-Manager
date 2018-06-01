using R3EHUDManager.placeholder.model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using System.Collections;
using System.Globalization;
using R3EHUDManager.coordinates;
using R3EHUDManager.huddata.model;

namespace R3EHUDManager.huddata.parser
{
    /**
     * TODO quand on sauvegarde un profile, le faire dans un format perso pour ne pas dépendre des changements de formats quand on recharge un profile.
     */
    class HudOptionsParser
    {
        private const int CURRENT_XML_VERSION = 7;

        private static Regex ITEM_NAME_EXP = new Regex($"(.*) ({ItemType.POSITION}|{ItemType.SIZE}|{ItemType.SIZE_VERSION_7}|{ItemType.ANCHOR})"); // TODO gérer le changement de nom de la version 7 "ITEM SIZE" -> "ITEM SIZE SCALE"
        private readonly PlaceholderBlackListModel blackList;

        public HudOptionsParser(PlaceholderBlackListModel blackList)
        {
            this.blackList = blackList;
        }

        internal List<PlaceholderModel> Parse(string hudOptionsPath)
        {
            int xmlVersion = GetXmlVersion(hudOptionsPath);
            if (xmlVersion < CURRENT_XML_VERSION)
                UpdateXmlVersion(xmlVersion, hudOptionsPath);

            Dictionary<string, PlaceholderModel> placeHolders = new Dictionary<string, PlaceholderModel>();

            string fileContent = File.ReadAllText(hudOptionsPath);

            using (XmlReader xmlReader = XmlReader.Create(new StringReader(fileContent)))
            {
                while (xmlReader.ReadToFollowing("name"))
                {
                    string name = xmlReader.ReadElementContentAsString();

                    if (!IsGeometricItem(name)) continue;

                    GeometricItem item = GetGeometricItem(name);

                    if (blackList.IsFiltered(item.Name))
                        continue;

                    if (!placeHolders.ContainsKey(item.Name))
                    {
                        PlaceholderModel placeholder = PlaceholderFactory.NewPlaceholder(item.Name);
                        placeHolders.Add(placeholder.Name, placeholder);
                    }

                    xmlReader.ReadToFollowing("value");

                    xmlReader.MoveToFirstAttribute();
                    string valueType = xmlReader.Value;

                    if (valueType != ValueType.VECTOR) throw new Exception($"Expected value is a {ValueType.VECTOR}");

                    xmlReader.MoveToContent();
                    string value = xmlReader.ReadElementContentAsString();

                    switch (item.ItemType)
                    {
                        case ItemType.POSITION:
                            placeHolders[item.Name].Position = ParseVector(value);
                            break;
                        case ItemType.SIZE:
                            placeHolders[item.Name].Size = ParseVector(value);
                            break;
                        case ItemType.ANCHOR:
                            placeHolders[item.Name].Anchor = ParseVector(value);
                            break;
                    }
                }
            }

            return placeHolders.Values.ToList();
        }

        private void UpdateXmlVersion(int xmlVersion, string hudOptionsPath)
        {
            switch (xmlVersion)
            {
                case 6:
                    UpdateXmlVersion6To7(hudOptionsPath);
                    break;
            }
        }

        private void UpdateXmlVersion6To7(string hudOptionsPath)
        {
            // TODO replace Size by SizeScale
            XmlDocument doc = new XmlDocument();
            doc.Load(hudOptionsPath);
            XmlNode root = doc.DocumentElement;
            XmlNode version = root.SelectSingleNode("latestVersion");
            version.InnerText = "7";

            XmlElement show = doc.CreateElement("name", null);
            show.SetAttribute("type", "string");
            show.AppendChild(doc.CreateCDataSection("Input Meter Show"));
            XmlElement showValue = doc.CreateElement("value", null);
            showValue.SetAttribute("type", "bool");
            showValue.InnerText = "false";
            XmlElement showInCockpit = doc.CreateElement("name", null);
            showInCockpit.SetAttribute("type", "string");
            showInCockpit.AppendChild(doc.CreateCDataSection("Input Meter Show In Cockpit"));
            XmlElement showInCockpitValue = doc.CreateElement("value", null);
            showInCockpitValue.SetAttribute("type", "bool");
            showInCockpitValue.InnerText = "true";

            XmlElement positionName = doc.CreateElement("name", null);
            positionName.SetAttribute("type", "string");
            positionName.AppendChild(doc.CreateCDataSection("Input Meter Position"));
            XmlElement positionValue = doc.CreateElement("value", null);
            positionValue.SetAttribute("type", "Vector2");
            positionValue.InnerText = "{0.000000 0.100000}";

            XmlElement scaleName = doc.CreateElement("name", null);
            scaleName.SetAttribute("type", "string");
            scaleName.AppendChild(doc.CreateCDataSection("Input Meter Size Scale"));
            XmlElement scaleValue = doc.CreateElement("value", null);
            scaleValue.SetAttribute("type", "Vector2");
            scaleValue.InnerText = "{1.000000 1.000000}";

            XmlElement anchorName = doc.CreateElement("name", null);
            anchorName.SetAttribute("type", "string");
            anchorName.AppendChild(doc.CreateCDataSection("Input Meter Anchor"));
            XmlElement anchorValue = doc.CreateElement("value", null);
            anchorValue.SetAttribute("type", "Vector2");
            anchorValue.InnerText = "{-1.000000 -1.000000}";

            root.InsertAfter(show, root.LastChild);
            root.InsertAfter(showValue, root.LastChild);
            root.InsertAfter(showInCockpit, root.LastChild);
            root.InsertAfter(showInCockpitValue, root.LastChild);
            root.InsertAfter(positionName, root.LastChild);
            root.InsertAfter(positionValue, root.LastChild);
            root.InsertAfter(scaleName, root.LastChild);
            root.InsertAfter(scaleValue, root.LastChild);
            root.InsertAfter(anchorName, root.LastChild);
            root.InsertAfter(anchorValue, root.LastChild);

            doc.Save(hudOptionsPath);
        }

        private int GetXmlVersion(string hudOptionsPath)
        {
            int xmlVersion;
            using (XmlReader xmlReader = XmlReader.Create(new StringReader(File.ReadAllText(hudOptionsPath))))
            {
                xmlReader.ReadToFollowing("latestVersion");
                xmlVersion = xmlReader.ReadElementContentAsInt();
            }
            return xmlVersion;
        }

        internal void Write(string hudOptionsPath, List<PlaceholderModel> placeholders)
        {
            string fileContent = File.ReadAllText(hudOptionsPath);

            Dictionary<string, string> elementNames = new Dictionary<string, string>();
            foreach (PlaceholderModel model in placeholders)
            {
                elementNames.Add($"{model.Name} {ItemType.POSITION}", GetVector(model.Position));
                elementNames.Add($"{model.Name} {ItemType.SIZE_VERSION_7}", GetVector(model.Size));
                elementNames.Add($"{model.Name} {ItemType.ANCHOR}", GetVector(model.Anchor));
            }

            XmlDocument doc = new XmlDocument();
            doc.Load(hudOptionsPath);
            XmlNode root = doc.DocumentElement;
            XmlNodeList list = root.ChildNodes;

            string nextVector = null;

            foreach (XmlNode node in list)
            {
                if (node.Name == "name")
                {
                    // TODO Cause of a mistake in options file, where "Car Status" is also written "CarStatus" (for Anchor).
                    string innerText = node.InnerText == $"CarStatus {ItemType.ANCHOR}" ? $"{PlaceholderName.CAR_STATUS} {ItemType.ANCHOR}" : node.InnerText;
                    if (elementNames.ContainsKey(innerText))
                    {
                        nextVector = elementNames[innerText];
                    }
                }
                else if (nextVector != null && node.Name == "value" && node.Attributes["type"].Value == ValueType.VECTOR)
                {
                    node.InnerText = nextVector;
                    nextVector = null;
                }
            }

            doc.Save(hudOptionsPath);
        }

        internal int GetVersion(string path)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(path);
            int versionNumber = Convert.ToInt32(doc.DocumentElement.GetElementsByTagName("latestVersion")[0].InnerText);
            return versionNumber;
        }

        private GeometricItem GetGeometricItem(string xmlName)
        {
            MatchCollection matches = ITEM_NAME_EXP.Matches(xmlName);
            string name = matches[0].Groups[1].Value;

            // TODO Cause of a mistake in options file, where "Car Status" is also written "CarStatus" (for Anchor).
            if (name == "CarStatus") name = PlaceholderName.CAR_STATUS;
            return new GeometricItem(name, matches[0].Groups[2].Value);
        }

        private bool IsGeometricItem(string name)
        {
            return ITEM_NAME_EXP.IsMatch(name);
        }

        private R3ePoint ParseVector(string vector)
        {
            Regex regEx = new Regex("[0-9.-]+");
            MatchCollection mc = regEx.Matches(vector);

            return new R3ePoint(
                    double.Parse(mc[0].Value, CultureInfo.InvariantCulture),
                    double.Parse(mc[1].Value, CultureInfo.InvariantCulture));
        }

        private string GetVector(R3ePoint point)
        {
            return $"{{{point.X.ToString(CultureInfo.InvariantCulture)} {point.Y.ToString(CultureInfo.InvariantCulture)}}}";
        }

        class GeometricItem
        {
            public GeometricItem(string name, string itemType)
            {
                Name = name;
                ItemType = itemType;
            }

            public string Name { get; }
            public string ItemType { get; }
        }

        class ItemType
        {
            public const string POSITION = "Position";
            public const string SIZE = "Size";
            public const string SIZE_VERSION_7 = "Size Scale";
            public const string ANCHOR = "Anchor";
        }

        class ValueType
        {
            public const string BOOL = "bool";
            public const string VECTOR = "Vector2";
            public const string INT_32 = "int32";
        }
    }
}
