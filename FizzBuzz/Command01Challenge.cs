#region Namespaces
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

#endregion

namespace FizzBuzz
{
    [Transaction(TransactionMode.Manual)]
    public class Command01Challenge : IExternalCommand
    {
        public Result Execute(
          ExternalCommandData commandData,
          ref string message,
          ElementSet elements)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Application app = uiapp.Application;
            Document doc = uidoc.Document;

            // variables (string, int, double, XYZ)

            string myString = "This number's result is: ";

            // Filtered Element Collectors - this looks for the text notes in the active model (doc)
            FilteredElementCollector collector = new FilteredElementCollector(doc);
            // Alternate way to collect text notes
            // collector.OfCategory(BuiltInCategory.OST_TextNotes);
            // collector.WhereElementIsElementType();
            collector.OfClass(typeof(TextNoteType));

            // Transaction happens outside of loop if you want one undo for whole series
            // When making or creating a Revit element, you have to create a "transaction" to "lock" Revit and make point where this command will begin
            // You don't need to do a transaction if you are just reporting info
            Transaction t = new Transaction(doc);
            t.Start("Create Fizz-Buzz-Number text");

            XYZ myPoint = new XYZ(0, 0, 0);
            XYZ myNextPoint = new XYZ();

            // Create the point for the text notes that will offset for each new instance
            XYZ offset = new XYZ(0, -1, 0);
            XYZ newPoint = myPoint;

            // Set up the For Loop

            for (int i = 1; i <= 100; i++)
            {
                newPoint = newPoint.Add(offset);

                // Add conditional logic (<, >, ==, &&, ||) - put most restrictive condition first
                string result = "";
                result = i.ToString();
                if (i % 3 == 0 && i % 5 == 0)
                {
                    result = "FizzBuzz";
                }
                else if (i % 5 == 0)
                {
                    result = "Buzz";
                }
                else if (i % 3 == 0)
                {
                    result = "Fizz";
                }
                else
                {
                    result = result.ToString();
                }
                // Create the Text Notes - asks for four things - the ones below set active file, active view, position at point, first text note type based on filtered element collector
                TextNote myTextNote = TextNote.Create(doc,
                doc.ActiveView.Id,
                newPoint,
                myString + result,
                collector.FirstElementId());
            }

            // Place commit outside of loop
            t.Commit();

            TaskDialog.Show("Cheers", "All done! Go catch a buzz with a sloe gin fizz!");

            return Result.Succeeded;
        }
    }
}