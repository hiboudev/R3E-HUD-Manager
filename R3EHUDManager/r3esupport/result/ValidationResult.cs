using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace R3EHUDManager.r3esupport.result
{
    class ValidationResult
    {
        public static ValidationResult GetValid()
        {
            return new ValidationResult(ResultType.VALID);
        }

        public static ValidationResult GetInvalid(string description)
        {
            return new ValidationResult(ResultType.INVALID, description);
        }

        public ValidationResult(ResultType type, string description)
        {
            Type = type;
            Description = description;
        }

        public ValidationResult(ResultType type)
        {
            Type = type;
            Description = "";
        }

        public bool Equals(ValidationResult result)
        {
            return
                Type == result.Type &&
                Description == result.Description;
        }

        public ResultType Type { get; }
        public string Description { get; }
    }
}
