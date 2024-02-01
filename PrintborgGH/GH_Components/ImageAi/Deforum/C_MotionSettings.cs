using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Printborg.Types.Deforum;
using Rhino.Geometry;

namespace PrintborgGH.GH_Components.ImageAi.Deforum
{
    public class C_MotionSettings : GH_Component
    {
        public C_MotionSettings()
          : base("Deforum Settings > Motion", "Mot",
              "Motion Settings Tab in Deforum",
              Labels.PluginName, Labels.Category_ImageAI) {
        }

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager) {
            pManager.AddTextParameter("Angle", "Ang", "Angle", GH_ParamAccess.item, "0: (0)");
            pManager.AddTextParameter("Zoom", "Zoo", "Zoom", GH_ParamAccess.item, "0: (1.0025+0.002*sin(1.25*3.14*t/30))");
            // translation
            pManager.AddTextParameter("Translation X", "Tlx", "Translation X", GH_ParamAccess.item, "0: (0)");
            pManager.AddTextParameter("Translation Y", "Tly", "Translation Y", GH_ParamAccess.item, "0: (0)");
            pManager.AddTextParameter("Translation Z", "Tlz", "Translation Z", GH_ParamAccess.item, "0: (0)");
            // transform center
            pManager.AddTextParameter("Transform Center X", "Tcx", "Transform Center X", GH_ParamAccess.item, "0: (0.5)");
            pManager.AddTextParameter("Transform Center Y", "Tcy", "Transform Center Y", GH_ParamAccess.item, "0: (0.5)");
            // rotation 3D
            pManager.AddTextParameter("Rotation3D X", "R3x", "Rotation3D X", GH_ParamAccess.item, "0: (0)");
            pManager.AddTextParameter("Rotation3D Y", "R3y", "Rotation3D Y", GH_ParamAccess.item, "0: (0)");
            pManager.AddTextParameter("Rotation3D Z", "R3z", "Rotation3D Z", GH_ParamAccess.item, "0: (0)");
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager) {
            pManager.AddGenericParameter("Motion Settings", "Mot", "Printborg.Types.MotionSettings object", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA) {
            var angle = string.Empty;
            var zoom = string.Empty;

            var translationX = string.Empty;
            var translationY = string.Empty;
            var translationZ = string.Empty;

            var transformCenterX = string.Empty;
            var transformCenterY = string.Empty;

            var rotation3dX = string.Empty;
            var rotation3dY = string.Empty;
            var rotation3dZ = string.Empty;

            if (!DA.GetData(0, ref angle)) return;
            if (!DA.GetData(1, ref zoom)) return;

            if (!DA.GetData(2, ref translationX)) return;
            if (!DA.GetData(3, ref translationY)) return;
            if (!DA.GetData(4, ref translationZ)) return;

            if (!DA.GetData(5, ref transformCenterX)) return;
            if (!DA.GetData(6, ref transformCenterY)) return;

            if (!DA.GetData(7, ref rotation3dX)) return;
            if (!DA.GetData(8, ref rotation3dY)) return;
            if (!DA.GetData(9, ref rotation3dZ)) return;



            DA.SetData(0, new MotionSettings(angle, zoom, translationX, translationY, translationZ, transformCenterX, transformCenterY, rotation3dX, rotation3dY, rotation3dZ));
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
            get { return new Guid("44279CF7-B760-4A40-8165-3A200E0D303D"); }
        }
    }
}