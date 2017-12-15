using R3EHUDManager.r3esupport.rule;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
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

            SupportRule rule = new SupportRule(ruleNode.Attributes["name"].Value, GetLayoutType(ruleNode.Attributes["layout"].Value));
            rule.SetParts(parts.ToArray());
            rule.SetTargets(targets);
            return rule;
        }

        private RuleLayoutType GetLayoutType(string value)
        {
            switch (value)
            {
                case "single":
                    return RuleLayoutType.SINGLE;
                case "triple":
                    return RuleLayoutType.TRIPLE;
                case "ANY":
                    return RuleLayoutType.ANY;
            }
            throw new Exception($"Invalid layout name {value}.");
        }

        private RulePart ParseRulePartNode(XmlNode rulePartNode)
        {
            Fix[] fixes = GetFixes(rulePartNode);

            return new RulePart(
                    GetPropertyType(rulePartNode["property"].InnerText),
                    GetOperations(rulePartNode.SelectNodes("check")),
                    rulePartNode["description"].InnerText,
                    fixes
                    );
        }

        private Fix[] GetFixes(XmlNode rulePartNode)
        {
            List<Fix> fixes = new List<Fix>();

            foreach (XmlNode propertyNode in rulePartNode.SelectNodes("fix/property"))
            {
                Fix fix = new Fix(
                    double.Parse(propertyNode.Attributes["value"].Value, CultureInfo.InvariantCulture),
                    GetPropertyType(propertyNode.InnerText));
                fixes.Add(fix);
            }
            return fixes.ToArray();
        }

        private Operation[] GetOperations(XmlNodeList xmlNodeList)
        {
            List<Operation> operations = new List<Operation>();

            foreach (XmlNode checkNode in xmlNodeList)
            {
                if(checkNode.Attributes["value"].Value == "ANY")
                    operations.Add(new Operation());
                else
                    operations.Add(new Operation(double.Parse(checkNode.Attributes["value"].Value, CultureInfo.InvariantCulture), GetOperatorType(checkNode.InnerText)));
            }

            return operations.ToArray();
        }

        private PropertyType GetPropertyType(string propertyName)
        {
            switch (propertyName)
            {
                case "x":
                    return PropertyType.X;
                case "y":
                    return PropertyType.Y;
                case "size":
                    return PropertyType.SIZE;
                case "ax":
                    return PropertyType.ANCHOR_X;
                case "ay":
                    return PropertyType.ANCHOR_Y;
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
                case "!=":
                    return OperatorType.NOT_EQUAL;
            }
            throw new Exception($"Invalid operator name {operatorName}.");
        }
    }
}
