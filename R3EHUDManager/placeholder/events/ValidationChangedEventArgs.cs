using da2mvc.core.events;
using R3EHUDManager.placeholder.model;
using R3EHUDManager.r3esupport.result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace R3EHUDManager.placeholder.events
{
    class ValidationChangedEventArgs : BaseEventArgs
    {
        public ValidationChangedEventArgs(int eventId, PlaceholderModel placeholder, ValidationResult result) : base(eventId)
        {
            Placeholder = placeholder;
            Result = result;
        }

        public PlaceholderModel Placeholder { get; }
        public ValidationResult Result { get; }
    }
}
