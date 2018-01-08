using da2mvc.core.events;
using da2mvc.framework.model;
using R3EHUDManager.coordinates;
using R3EHUDManager.graphics;
using R3EHUDManager.placeholder.events;
using R3EHUDManager.r3esupport.result;
using R3EHUDManager.r3esupport.rule;
using R3EHUDManager.screen.model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace R3EHUDManager.placeholder.model
{
    public class PlaceholderModel : EventDispatcher, IModel
    {
        public static readonly int EVENT_UPDATED = EventId.New();
        public static readonly int EVENT_VALIDATION_CHANGED = EventId.New();
        private readonly SupportRuleValidator layoutValidator;
        private readonly ScreenModel screenModel;

        public PlaceholderModel(SupportRuleValidator layoutValidator, ScreenModel screenModel)
        {
            this.layoutValidator = layoutValidator;
            this.screenModel = screenModel;
        }

        // TODO refaire le parsing xml pour que tous les params soient immutables.
        public int Id { get; internal set; }
        public string Name { get; internal set; }
        public R3ePoint Position { get; internal set; } = new R3ePoint(0, 0);

        public R3ePoint Anchor { get; internal set; } = new R3ePoint(0, 0);
        public R3ePoint Size { get; internal set; } = new R3ePoint(1, 1);
        public IResizeRule ResizeRule { get; internal set; }
        public LayoutValidationResult ValidationResult { get; set; } = LayoutValidationResult.GetValid();

        public void Move(R3ePoint position)
        {
            Position = position;
            DispatchEvent(new PlaceHolderUpdatedEventArgs(EVENT_UPDATED, this, UpdateType.POSITION));
            SetValidationResult(layoutValidator.Matches(this));
        }

        public void MoveAnchor(R3ePoint position)
        {
            Anchor = position;
            DispatchEvent(new PlaceHolderUpdatedEventArgs(EVENT_UPDATED, this, UpdateType.ANCHOR));
            SetValidationResult(layoutValidator.Matches(this));
        }

        public void Resize(R3ePoint position)
        {
            Size = position;
            DispatchEvent(new PlaceHolderUpdatedEventArgs(EVENT_UPDATED, this, UpdateType.SIZE));
            SetValidationResult(layoutValidator.Matches(this));
        }

        private void SetValidationResult(LayoutValidationResult result)
        {
            if (ValidationResult.Equals(result)) return;

            ValidationResult = result;

            DispatchEvent(new ValidationChangedEventArgs(EVENT_VALIDATION_CHANGED, this, result));
        }

        internal void UpdateLayoutValidation()
        {
            SetValidationResult(layoutValidator.Matches(this));
        }

        public void ApplyLayoutFix()
        {
            if (!ValidationResult.HasFix()) return;

            PlaceholderGeom geom = GetGeom();
            ValidationResult.ApplyFixes(geom, screenModel, ResizeRule);

            if (!geom.Position.Equals(Position)) Move(geom.Position);
            if (!geom.Anchor.Equals(Anchor)) MoveAnchor(geom.Anchor);
            if (!geom.Size.Equals(Size)) Resize(geom.Size);
        }

        public PlaceholderGeom GetGeom()
        {
            return new PlaceholderGeom(Position.Clone(), Anchor.Clone(), Size.Clone(), GraphicalAsset.GetPlaceholderSize(Name));
        }

        public PlaceholderModel Clone() // TODO utiliser le Geom
        {
            return new PlaceholderModel(layoutValidator, screenModel)
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

    public class PlaceholderGeom
    {
        public PlaceholderGeom(R3ePoint position, R3ePoint anchor, R3ePoint size, Size bitmapSize)
        {
            Position = position;
            Anchor = anchor;
            Size = size;
            BitmapSize = bitmapSize;
        }

        public void Move(R3ePoint position)
        {
            Position = position;
        }

        public void MoveAnchor(R3ePoint position)
        {
            Anchor = position;
        }

        public void Resize(R3ePoint size)
        {
            Size = size;
        }

        public R3ePoint Position { get; private set; }
        public R3ePoint Anchor { get; private set; }
        public R3ePoint Size { get; private set; }
        public Size BitmapSize { get; }
    }
}
