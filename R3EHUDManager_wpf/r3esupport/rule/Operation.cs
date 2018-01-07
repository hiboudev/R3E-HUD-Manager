using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace R3EHUDManager.r3esupport.rule
{
    public class Operation
    {
        private readonly double referenceValue;
        private readonly OperatorType operatorType;
        private bool matchAny;

        public Operation(double referenceValue, OperatorType operatorType)
        {
            this.referenceValue = referenceValue;
            this.operatorType = operatorType;
        }

        public Operation()
        {
            matchAny = true;
        }

        public bool Matches(double value)
        {
            if (matchAny) return true;

            switch (operatorType)
            {
                case OperatorType.EQUAL:
                    return value == referenceValue;
                case OperatorType.GREATER_OR_EQUAL:
                    return value >= referenceValue;
                case OperatorType.LESS_OR_EQUAL:
                    return value <= referenceValue;
                case OperatorType.GREATER:
                    return value > referenceValue;
                case OperatorType.LESS:
                    return value < referenceValue;
                case OperatorType.NOT_EQUAL:
                    return value != referenceValue;
            }
            throw new Exception("Unsupported operator type.");
        }
    }
}
