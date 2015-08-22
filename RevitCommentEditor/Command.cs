/*
 * Copyright (c) 2015 Dave French <contact/dot/dave/dot/french3/at/googlemail/dot/com>
 *
 * This file is part of RevitCommentEditor https://github.com/curlymorphic/RevitCommentEditor
 *
 * This program is free software; you can redistribute it and/or
 * modify it under the terms of the GNU General Public
 * License as published by the Free Software Foundation; either
 * version 2 of the License, or (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
 * General Public License for more details.
 *
 * You should have received a copy of the GNU General Public
 * License along with this program (see COPYING); if not, write to the
 * Free Software Foundation, Inc., 51 Franklin Street, Fifth Floor,
 * Boston, MA 02110-1301 USA.
 *
 */

#region Namespaces
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
#endregion

using System.Linq;
using System.Windows;

namespace RevitCommentEditor
{
    [Transaction(TransactionMode.Manual)]
    public class Command : IExternalCommand
    {
        public Result Execute(
          ExternalCommandData commandData,
          ref string message,
          ElementSet elements)
        {
            if (commandData == null || commandData.Application == null || commandData.Application.ActiveUIDocument == null)
            {
                TaskDialog.Show("Revit", "Comment Editor requires an open document.");
                return Result.Failed;
            }

            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Autodesk.Revit.ApplicationServices.Application app = uiapp.Application;
            Document doc = uidoc.Document;

            // Access current selection

            Selection sel = uidoc.Selection;

            if (sel.GetElementIds().Count != 1)
            {
                TaskDialog.Show("Revit", "Comment Editor requires a single element selected.");
                return Result.Failed;
            }

            m_element = doc.GetElement(sel.GetElementIds().FirstOrDefault());
            Parameter comment = m_element.GetParameters("Comments").FirstOrDefault();

            if (comment == null)
            {
                TaskDialog.Show("Revit", "Comment Editor requires the selected element to have a comment parameter");
                return Result.Failed;
            }

            CommentEditorDialog ced = new CommentEditorDialog();
            Window cew = new Window();
            cew.Content = ced;
            cew.Title = "Comment Editor";
            cew.Width = ced.grid.Width;
            cew.Height = ced.grid.Height;
            ced.Text = comment.AsString();
            cew.ResizeMode = ResizeMode.NoResize;
            cew.ShowDialog();

            if (cew.DialogResult.Value == false)
            {
                return Result.Cancelled;
            }

          

            using (Transaction tx = new Transaction(doc))
            {
                tx.Start("Edit Element Comment");
                comment.Set(ced.Text);
                tx.Commit();
            }

            return Result.Succeeded;
        }

        private Element m_element;
    }
}
