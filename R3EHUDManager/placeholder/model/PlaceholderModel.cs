using da2mvc.core.events;
using da2mvc.framework.model;
using R3EHUDManager.coordinates;
using R3EHUDManager.placeholder.events;
using R3EHUDManager.r3esupport.result;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace R3EHUDManager.placeholder.model
{
    class PlaceholderModel : EventDispatcher, IModel
    {
        public static readonly int EVENT_UPDATED = EventId.New();
        public static readonly int EVENT_VALIDATION_CHANGED = EventId.New();

        // TODO refaire le parsing xml pour que tous les params soient immutables.
        public int Id { get; internal set; }
        public string Name { get; internal set; }
        public R3ePoint Position { get; internal set; } = new R3ePoint(0,0);
        public R3ePoint Anchor { get; internal set; } = new R3ePoint(0, 0);
        public R3ePoint Size { get; internal set; } = new R3ePoint(1, 1);
        public IResizeRule ResizeRule { get; internal set; }
        public ValidationResult ValidationResult { get; private set; } = ValidationResult.GetValid();

        public void Move(R3ePoint position)
        {
            Position = position;
            DispatchEvent(new PlaceHolderUpdatedEventArgs(EVENT_UPDATED, this, UpdateType.POSITION));
        }

        public void MoveAnchor(R3ePoint position)
        {
            Anchor = position;
            DispatchEvent(new PlaceHolderUpdatedEventArgs(EVENT_UPDATED, this, UpdateType.ANCHOR));
        }

        public void Resize(R3ePoint position)
        {
            Size = position;
            DispatchEvent(new PlaceHolderUpdatedEventArgs(EVENT_UPDATED, this, UpdateType.SIZE));
        }

        internal void SetValidationResult(ValidationResult result)
        {
            if (ValidationResult.Equals(result)) return;

            ValidationResult = result;
            DispatchEvent(new ValidationChangedEventArgs(EVENT_VALIDATION_CHANGED, this, result));
        }

        public PlaceholderModel Clone()
        {
            return new PlaceholderModel()
            {
                Id = Id,
                Name = Name,
                Position = Position.Clone(),
                Anchor = Anchor.Clone(),
                Size = Size.Clone(),
                ResizeRule = ResizeRule,
            };
        }
    }
}
