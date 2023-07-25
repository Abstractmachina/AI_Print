using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using Rhino.Geometry;
using SpatialSlur.SlurField;
using SpatialSlur.SlurRhino;

namespace AI_Print.GH_Components.Fields
{
    public class C_MeshField : GH_Component
    {
        public C_MeshField()
          : base("Mesh Field", "MField",
              "Create a field from points or curves on a base mesh.",
              Labels.PluginName, Labels.Category_Fields)
        {
        }

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddMeshParameter("Base Mesh", "M", "Base mesh of the field.", GH_ParamAccess.item);
            pManager.AddGeometryParameter("Drawing Input", "IN", "points or curves that describe field.", GH_ParamAccess.list);
            pManager.AddNumberParameter("Iso Value", "IV", "Zero Iso Value Adjustment", GH_ParamAccess.item);
            pManager.AddNumberParameter("Curve Resolution", "CR", "Curve Resolution Adjustment (optional).", GH_ParamAccess.item, 1d);
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Mesh Field", "F", "Mesh Field Result", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            Mesh m = null;
            List<GH_GeometricGooWrapper> obj = new List<GH_GeometricGooWrapper>();
            double iso = 0d;
            double res = 0d;

            List<Point3d> pts = new List<Point3d>();
            List<Curve> crvs = new List<Curve>();


            if (!DA.GetData(0, ref m)) return;
            if (!DA.GetDataList<GH_GeometricGooWrapper>(1, obj)) return;
            if (!DA.GetData(2, ref iso)) return;
            if (!DA.GetData(3, ref res)) return;

            var hem = m.ToHeMesh();
            var field = MeshField3d.Double.Create(hem);

            //cast objects to geometry types
            foreach (GH_GeometricGooWrapper g in obj) {
                if (g.TypeName == "{Point}") {
                    Point3d p = new Point3d();
                    g.CastTo<Point3d>(ref p);
                    pts.Add(p);
                }
                else if (g.TypeName == "{Curve}") {
                    Curve c = null;
                    g.CastTo<Curve>(ref c);
                    crvs.Add(c);
                }
                else {
                    AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Input must be points or curves.");
                    return;
                }
            }

            //if curves are inputted, convert them into points
            Util.SpatialSlur.ConvertCrvsToPts(crvs, ref pts, res);

            //get vector of each field pt to the closest point. 
            List<double> val = Util.SpatialSlur.GetDistanceField(m, pts);

            Interval interval = Util.SpatialSlur.GetInterval(val);
            //

            for (int i = 0; i < val.Count; i++) {
                val[i] = (SpatialSlur.SlurCore.SlurMath.Remap(val[i], interval.T0, interval.T1, -1, 1)) - iso;
            }


            field.Set(val);
            DA.SetData(0, FuncField3d.Create(i => field.ValueAt(i)));
        }

        /// <summary>
        /// Provides an Icon for the component.
        /// </summary>
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                //You can add image files to your project resources and access them like this:
                // return Resources.IconForThisComponent;
                return null;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("BD5D693A-12A4-401E-AA3F-7278753CC6F3"); }
        }
    }
}