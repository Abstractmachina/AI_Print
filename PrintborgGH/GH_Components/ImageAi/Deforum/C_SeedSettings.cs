using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Printborg.Types.Deforum;
using Rhino.Geometry;

namespace PrintborgGH.GH_Components.ImageAi.Deforum
{
    public class C_SeedSettings : GH_Component
    {
        public C_SeedSettings()
          : base("Deforum Settings > Seed", "See",
              "Seed Settings Tab in Deforum",
              Labels.PluginName, Labels.Category_ImageAI) {
        }

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager) {
            pManager.AddTextParameter("Seed Behaviour", "Beh", "Seed Behaviour", GH_ParamAccess.item, "iter");
            pManager.AddTextParameter("Seed Schedule", "Sch", "Seed Schedule", GH_ParamAccess.item, "0:(s), 1:(-1), \"max_f-2\":(-1), \"max_f-1\":(s)");
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager) {
            pManager.AddGenericParameter("Seed Settings", "See", "Printborg.Types.SeedSettings object", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA) {
            var seedBehaviour = string.Empty;
            var seedSchedule = string.Empty;

            if (!DA.GetData(0, ref seedBehaviour)) { return; }
            if (!DA.GetData(1, ref seedSchedule)) { return; }

            DA.SetData(0, new SeedSettings(seedBehaviour, seedSchedule));

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
            get { return new Guid("90E210C9-C692-4C6A-AADF-A0FD6ECAE65C"); }
        }
    }
}