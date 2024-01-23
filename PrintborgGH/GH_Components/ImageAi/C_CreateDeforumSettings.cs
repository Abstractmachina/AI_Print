using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Printborg.Types;
using PrintborgGH.Types;
using Rhino.Geometry;

namespace PrintborgGH.GH_Components.ImageAi
{
    public class C_CreateDeforumSettings : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the C_CreateDeforumSettings class.
        /// </summary>
        public C_CreateDeforumSettings()
          : base("Create Deforum Settings", "Create DSets",
              "Create a Deforum Settings object. This is equivalent to settings tabs in the UI",
              Labels.PluginName, Labels.Category_ImageAI)
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Run Settings", "Run", "General settings.", GH_ParamAccess.item);
            pManager.AddGenericParameter("Key Frames Settings", "Fra", "Key frame settings.", GH_ParamAccess.item);
            pManager.AddGenericParameter("Guided Images Settings", "Img", "Guided Images settings.", GH_ParamAccess.item);
            pManager.AddGenericParameter("Prompt Settings", "Pro", "Prompt settings.", GH_ParamAccess.item);
            pManager.AddGenericParameter("Strength Settings", "Str", "Strength settings.", GH_ParamAccess.item);
            pManager.AddGenericParameter("CFG Settings", "CFG", "CFG settings.", GH_ParamAccess.item);
            pManager.AddGenericParameter("Seed Settings", "Sed", "Seed settings.", GH_ParamAccess.item);
            pManager.AddGenericParameter("Motion Settings", "Mot", "Motion settings.", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("DeforumSettings Object", "Set", "Deforum Settings Object containing payload for generating a job", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            RunSettings run = new RunSettings();
            KeyFramesSettings keyFrames = new KeyFramesSettings();
            GuidedImagesSettings guidedImages = new GuidedImagesSettings();
            PromptSettings prompt = new PromptSettings();
            StrengthSettings strength = new StrengthSettings();
            CfgSettings cfg = new CfgSettings();
            SeedSettings seed = new SeedSettings();
            MotionSettings motion = new MotionSettings();

            if (!DA.GetData(0, ref run)) return;
            if (!DA.GetData(1, ref keyFrames)) return;
            if (!DA.GetData(2, ref guidedImages)) return;
            if (!DA.GetData(3, ref prompt)) return;
            if (!DA.GetData(4, ref strength)) return;
            if (!DA.GetData(5, ref cfg)) return;
            if (!DA.GetData(6, ref seed)) return;
            if (!DA.GetData(7, ref motion)) return;


            var output = new DeforumSettings(run, keyFrames, guidedImages, prompt, strength, cfg, seed, motion);

            DA.SetData(0, new GH_DeforumSettings(output));
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
            get { return new Guid("3AF84B2F-E6E1-498C-A14C-446DF9FADDF1"); }
        }
    }
}