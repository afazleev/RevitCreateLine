// This code is transferred from my blog. Original post date: October 22, 2012
// CAUTION: This is probably the first thing I did in the Revit API and C#.
//          Never use it as an example!

using System;
using System.Collections.Generic;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;

namespace Draw_Line
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Automatic)]
    [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
    public class Draw_Line : IExternalCommand
    {
        Application app;
        Document doc;
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {

            UIApplication rvtUIApp = commandData.Application;
            UIDocument uiDoc = rvtUIApp.ActiveUIDocument;
            app = rvtUIApp.Application;
            doc = uiDoc.Document;

            Draw_Annotation_Line(100, 200, 1000, 3000); //Здесь задаются точки х1, у1, х2, у2
            return Result.Succeeded;
        }
        public void Draw_Annotation_Line(double X_1, double Y_1, double X_2, double Y_2)
        {
            bool line_created;
            double X_Rd = doc.ActiveView.RightDirection.X;
            double Y_Rd = doc.ActiveView.RightDirection.Y;
            double Y_Ud = doc.ActiveView.UpDirection.Y;
            double Z_Ud = doc.ActiveView.UpDirection.Z;
            double x1 = Math.Round(Constant.MmToFeet(X_1 * (X_Rd)), 10);
            double y1 = Math.Round(Constant.MmToFeet(X_1 * Y_Rd + Y_1 * Y_Ud), 10);
            double z1 = Math.Round(Constant.MmToFeet(Y_1 * (Z_Ud)), 10);
            double x2 = Math.Round(Constant.MmToFeet(X_2 * (X_Rd)), 10);
            double y2 = Math.Round(Constant.MmToFeet(X_2 * Y_Rd + Y_2 * Y_Ud), 10);
            double z2 = Math.Round(Constant.MmToFeet(Y_2 * (Z_Ud)), 10);
            XYZ point1 = app.Create.NewXYZ(x1, y1, z1);
            XYZ point2 = app.Create.NewXYZ(x2, y2, z2);
            Line line = app.Create.NewLineBound(point1, point2);
            try
            {
                DetailCurve detailCurve = doc.Create.NewDetailCurve(doc.ActiveView, line);
                line_created = true;
            }
            catch
            {
                line_created = false;
            }
            if (line_created == true)
                TaskDialog.Show("Done", "Line Created");
            else TaskDialog.Show("Error", "Line Not Created");
        }

        public class Constant
        {
            /// <summary>
            /// Conversion factor to convert millimetres to feet.
            /// </summary>
            const double _mmToFeet = 0.0032808399;
            public static double MmToFeet(double mmValue)
            {
                return mmValue * _mmToFeet;
            }
        }
    }
}
