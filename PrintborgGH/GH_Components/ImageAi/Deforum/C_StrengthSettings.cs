using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Printborg.Types.Deforum;
using Rhino.Geometry;

namespace PrintborgGH.GH_Components.ImageAi.Deforum
{
    public class C_StrengthSettings : GH_Component
    {
        public C_StrengthSettings()
          : base("Deforum Settings > Strength", "Str",
              "Strength Settings Tab in Deforum",
              Labels.PluginName, Labels.Category_ImageAI)
        {
        }

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Strengt Schedule", "Str", "Strength Schedule", GH_ParamAccess.item, "");
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Strength Settings", "Str", "Printborg.Types.StrengthSettings object", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var strengthSchedule = string.Empty;

            if(!DA.GetData(0, ref strengthSchedule)) { return; }    

            DA.SetData(0, new StrengthSettings(strengthSchedule));

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
            get { return new Guid("3128645F-F152-4A60-82A6-B014A36259BB"); }
        }
    }
}