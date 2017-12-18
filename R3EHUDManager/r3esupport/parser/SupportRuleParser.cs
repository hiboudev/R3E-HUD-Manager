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

        private RulePart ParseRulePartNode(XmlNode rulePartNode)
        {
            Fix[] fixes = GetFixes(rulePartNode);

            return new RulePart(
                    GetChecks(rulePartNode),
                    rulePartNode["description"].InnerText,
                    fixes
                    );
        }

        private PropertyCheck[] GetChecks(XmlNode rulePartNode)
        {
            List<PropertyCheck> checks = new List<PropertyCheck>();

            foreach (XmlNode checkNode in rulePartNode.SelectNodes("check"))
            {
                PropertyCheck check = new PropertyCheck(
                    GetCheckPropertyType(checkNode.Attributes["property"].Value),
                    GetOperation(checkNode)
                    );
                checks.Add(check);
            }
            return checks.ToArray();
        }

        private Fix[] GetFixes(XmlNode rulePartNode)
        {
            List<Fix> fixes = new List<Fix>();

            foreach (XmlNode propertyNode in rulePartNode.SelectNodes("fix/property"))
            {
                Fix fix = new Fix(
                    double.Parse(propertyNode.Attributes["value"].Value, CultureInfo.InvariantCulture),
                    GetFixPropertyType(propertyNode.Attributes["name"].Value));
                fixes.Add(fix);
            }
            return fixes.ToArray();
        }

        private Operation GetOperation(XmlNode checkNode)
        {
            if (checkNode.Attributes["value"].Value == "ANY")
                return new Operation();
            else
                return new Operation(double.Parse(checkNode.Attributes["value"].Value, CultureInfo.InvariantCulture), GetOperatorType(checkNode.InnerText));
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

        private CheckPropertyType GetCheckPropertyType(string propertyName)
        {
            switch (propertyName)
            {
                case "x":
                    return CheckPropertyType.X;
                case "y":
                    return CheckPropertyType.Y;
                case "size":
                    return CheckPropertyType.SIZE;
                case "ax":
                    return CheckPropertyType.ANCHOR_X;
                case "ay":
                    return CheckPropertyType.ANCHOR_Y;
            }
            throw new Exception($"Invalid check property name {propertyName}.");
        }

        private FixPropertyType GetFixPropertyType(string propertyName)
        {
            switch (propertyName)
            {
                case "x":
                    return FixPropertyType.X;
                case "y":
                    return FixPropertyType.Y;
                case "size":
                    return FixPropertyType.SIZE;
                case "ax":
                    return FixPropertyType.ANCHOR_X;
                case "ay":
                    return FixPropertyType.ANCHOR_Y;
                case "w":
                    return FixPropertyType.WIDTH;
                case "h":
                    return FixPropertyType.HEIGHT;
            }
            throw new Exception($"Invalid fix property name {propertyName}.");
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
