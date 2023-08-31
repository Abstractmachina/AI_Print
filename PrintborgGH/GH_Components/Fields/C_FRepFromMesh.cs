using System;
using System.Collections.Generic;
using System.Linq;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using Rhino.Geometry;
using SpatialSlur.SlurField;
using SpatialSlur.SlurMesh;
using SpatialSlur.SlurRhino;

namespace PrintborgGH.GH_Components.Fields
{
    public class C_FRepFromMesh : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the C_FRepFromMesh class.
        /// </summary>
        public C_FRepFromMesh()
          : base("FRep From Mesh", "FrepMesh",
              "Create a frep field from a colored input mesh. Note: RGB values will be converted into a grayscale value.",
              Labels.PluginName, Labels.Category_Fields)
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddMeshParameter("Mesh", "M", "Mesh input", GH_ParamAccess.item);
            pManager.AddNumberParameter("Iso Value", "IV", "Zero Iso Value Adjustment", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("FRep", "F", "FRep object", GH_ParamAccess.item);
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

            if (!DA.GetData(0, ref m)) return;
            if (!DA.GetData(1, ref iso)) return;

            var hem = m.ToHeMesh();
            var field = MeshField3d.Double.Create(hem);

            var vals = m.VertexColors.Select(c => (c.R * 0.3 + c.G * 0.59 + c.B * 0.11)/255d).ToList();

            //get vector of each field pt to the closest point. 
            //List<double> val = Util.SpatialSlur.GetDistanceField(m, pts);

            Interval interval = Util.SpatialSlur.GetInterval(vals);
            //

            for (int i = 0; i < vals.Count; i++) {
                vals[i] = (SpatialSlur.SlurCore.SlurMath.Remap(vals[i], interval.T0, interval.T1, -1, 1)) - iso;
            }


            field.Set(vals);
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
            get { return new Guid("F46897FF-A3B5-4A9A-92D6-A2D5582A0B8E"); }
        }
    }
}