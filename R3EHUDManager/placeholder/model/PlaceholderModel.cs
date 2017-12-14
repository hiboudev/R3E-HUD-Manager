﻿using da2mvc.core.events;
using da2mvc.framework.model;
using R3EHUDManager.coordinates;
using R3EHUDManager.placeholder.events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace R3EHUDManager.placeholder.model
{
    class PlaceholderModel : EventDispatcher, IModel
    {
        public static readonly int EVENT_UPDATED = EventId.New();

        // TODO refaire le parsing xml pour que tous les params soient immutables.
        public int Id { get; internal set; }
        public string Name { get; internal set; }
        public R3ePoint Position { get; internal set; }
        public R3ePoint Anchor { get; internal set; }
        public R3ePoint Size { get; internal set; }
        public IResizeRule ResizeRule { get; internal set; }


        public void Move(R3ePoint position)
        {
            // TODO limiter la position au background
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
    }
}
