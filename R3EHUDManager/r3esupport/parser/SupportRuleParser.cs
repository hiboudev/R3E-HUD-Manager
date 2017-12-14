using R3EHUDManager.r3esupport.rule;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace R3EHUDManager.r3esupport.parser
{
    class SupportRuleParser
    {
        public SupportRule[] Parse(string path)
        {
            XmlDocument xml = new XmlDocument();
            xml.Load(path);

            List<SupportRule> rules = new List<SupportRule>();

            foreach (XmlNode ruleNode in xml.SelectNodes("/xml/rule"))
            {
                rules.Add(ParseRuleNode(ruleNode));
            }

            return rules.ToArray();
        }

        private SupportRule ParseRuleNode(XmlNode ruleNode)
        {
            List<RulePart> parts = new List<RulePart>();
            HashSet<string> targets = new HashSet<string>();

            foreach (XmlNode rulePartNode in ruleNode.SelectNodes("invalid"))
            {
                parts.Add(ParseRulePartNode(rulePartNode));
            }

            foreach (XmlNode rulePartNode in ruleNode.SelectNodes("placeholder"))
            {
                targets.Add(rulePartNode.Attributes["name"].Value);
            }

            SupportRule rule = new SupportRule(ruleNode.Attributes["name"].Value);
            rule.SetParts(parts.ToArray());
            rule.SetTargets(targets);
            return rule;
        }

        private RulePart ParseRulePartNode(XmlNode rulePartNode)
        {
            string valueText = rulePartNode["value"].InnerText;

            if (valueText == "ANY")
            {
                return new RulePart(
                    GetPropertyType(rulePartNode["property"].InnerText),
                    rulePartNode["description"].InnerText
                    );
            }

            return new RulePart(
                    GetPropertyType(rulePartNode["property"].InnerText),
                    GetOperatorType(rulePartNode["operator"].InnerText),
                    Convert.ToDouble(rulePartNode["value"].InnerText),
                    rulePartNode["description"].InnerText
                    );
        }

        private PropertyType GetPropertyType(string propertyName)
        {
            switch (propertyName)
            {
                case "X":
                    return PropertyType.X;
                case "Y":
                    return PropertyType.Y;
                case "SIZE":
                    return PropertyType.SIZE;
            }
            throw new Exception($"Invalid property name {propertyName}.");
        }

        private OperatorType GetOperatorType(string operatorName)
        {
            switch (operatorName)
            {
                case "=":
                    return OperatorType.EQUAL;
                case "<":
                    return OperatorType.LESS;
                case ">":
                    return OperatorType.GREATER;
                case "<=":
                    return OperatorType.LESS_OR_EQUAL;
                case ">=":
                    return OperatorType.GREATER_OR_EQUAL;
            }
            throw new Exception($"Invalid operator name {operatorName}.");
        }
    }
}
