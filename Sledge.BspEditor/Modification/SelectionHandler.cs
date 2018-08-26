﻿using System;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading.Tasks;
using LogicAndTrick.Oy;
using Sledge.BspEditor.Primitives.MapData;

namespace Sledge.BspEditor.Modification
{
    [Export(typeof(IMapDocumentChangeHandler))]
    public class SelectionHandler : IMapDocumentChangeHandler
    {
        public async Task Changed(Change change)
        {
            var sel = change.Document.Map.Data.Get<Selection>().FirstOrDefault();
            if (sel == null)
            {
                sel = new Selection();
                change.Document.Map.Data.Add(sel);
            }
            if (sel.Update(change))
            {
                await Oy.Publish("MapDocument:SelectionChanged", change.Document);
                await Oy.Publish("Menu:Update", String.Empty);
            }
        }
    }
}