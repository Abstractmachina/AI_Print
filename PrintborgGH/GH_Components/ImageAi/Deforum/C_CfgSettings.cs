using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Printborg.Types.Deforum;
using Rhino.Geometry;

namespace PrintborgGH.GH_Components.ImageAi.Deforum
{
    public class C_CfgSettings : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the C_StrengthSettings class.
        /// </summary>
        public C_CfgSettings()
          : base("Deforum Settings > CFG", "Cfg",
              "Cfg Settings Tab in Deforum",
              Labels.PluginName, Labels.Category_ImageAI) {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager) {
            pManager.AddTextParameter("CFG Scale Schedule", "Sca", "CFG Scale Schedule", GH_ParamAccess.item, "");
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager) {
            pManager.AddGenericParameter("CFG Settings", "Cfg", "Printborg.Types.CfgSettings object", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA) {
            var cfgScaleSchedule = string.Empty;

            if (!DA.GetData(0, ref cfgScaleSchedule)) { return; }

            var output = (cfgScaleSchedule == "") ? new CfgSettings() : new CfgSettings(cfgScaleSchedule);

            DA.SetData(0, output);

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
            get { return new Guid("DE002DE9-FBF2-4DC8-B026-79735A227993"); }
        }
    }
}