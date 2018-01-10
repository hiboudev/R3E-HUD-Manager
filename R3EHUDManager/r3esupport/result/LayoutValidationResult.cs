using R3EHUDManager.placeholder.model;
using R3EHUDManager.r3esupport.rule;
using R3EHUDManager.screen.model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace R3EHUDManager.r3esupport.result
{
    public class LayoutValidationResult
    {
        private readonly List<Fix> fixes;

        public static LayoutValidationResult GetValid()
        {
            return new LayoutValidationResult(ResultType.VALID);
        }

        public static LayoutValidationResult GetInvalid(string description, List<Fix> fixes)
        {
            return new LayoutValidationResult(ResultType.INVALID, description, fixes);
        }

        public LayoutValidationResult(ResultType type, string description, List<Fix> fixes)
        {
            Type = type;
            Description = description;
            this.fixes = fixes;
        }

        public LayoutValidationResult(ResultType type)
        {
            Type = type;
            Description = "";
        }

        public void ApplyFixes(PlaceholderGeom geom, ScreenModel screenModel, IResizeRule resizeRule)
        {
            foreach (Fix fix in fixes)
                fix.Apply(geom, screenModel, resizeRule);
        }

        public bool HasFix()
        {
            return fixes != null && fixes.Count > 0;
        }

        public bool Equals(LayoutValidationResult result)
        {
            return
                Type == result.Type &&
                Description == result.Description; // TODO bofbof le Equals
        }

        public ResultType Type { get; }
        public string Description { get; }
    }
}
