using System;

using Grasshopper.Kernel;
using Printborg.Types.Deforum;

namespace PrintborgGH.GH_Components.ImageAi.Deforum {
    public class C_RunSettings : GH_Component {
        public C_RunSettings()
          : base("Deforum Settings > Run", "Run",
              "Run settings tab in Deforum",
              Labels.PluginName, Labels.Category_ImageAI) {
        }

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager) {
            pManager.AddTextParameter("Sampler", "Sam", "Select a Sampler to be used. (valid inputs: \"Euler a\", ...)", GH_ParamAccess.item, "Euler a");
            pManager.AddIntegerParameter("Steps", "Ste", "Number Steps of steps", GH_ParamAccess.item, 12);
            pManager.AddIntegerParameter("Width", "Wid", "Width of frame", GH_ParamAccess.item, 256);
            pManager.AddIntegerParameter("Height", "Hei", "Height of frame", GH_ParamAccess.item, 256);
            pManager.AddIntegerParameter("Seed", "See", "Seed", GH_ParamAccess.item, 0);
            pManager.AddTextParameter("Batch Name", "Nam", "Batch name (Note: also the name of the output folder)", GH_ParamAccess.item, "");

        }


        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager) {
            pManager.AddGenericParameter("Run Tab Settings", "Run", "Printborg.Types.RunSettings object", GH_ParamAccess.item);

    }

    protected override void SolveInstance(IGH_DataAccess DA) {
            string sampler = "";
            int steps = 0;
            int w = 0;
            int h = 0;
            int seed = 0;
            string batchName = "";

            if (!DA.GetData(0, ref sampler)) { return; }
            if (!DA.GetData(1, ref steps)) { return; }
            if (!DA.GetData(2, ref w)) { return; }
            if (!DA.GetData(3, ref h)) { return; }
            if (!DA.GetData(4, ref seed)) { return; }
            if (!DA.GetData(5, ref batchName)) { return; }

            if (batchName == "") {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Remark, "No batch name provided. It will be generated based on the current DateTime.");
                var currentDateTime = DateTime.Now.ToString("yymmdd_hhmmss");
                batchName = $"Deforum_{currentDateTime}";
            }


            DA.SetData(0, new RunSettings(sampler, steps, w, h, seed, batchName));

        }

    protected override System.Drawing.Bitmap Icon {
        get {
            //You can add image files to your project resources and access them like this:
            // return Resources.IconForThisComponent;
            return null;
        }
    }

    public override Guid ComponentGuid {
        get { return new Guid("4C5E13BA-80D5-4534-9937-C91DE3FDC09E"); }
    }

        public override GH_Exposure Exposure => GH_Exposure.primary;
    }
}