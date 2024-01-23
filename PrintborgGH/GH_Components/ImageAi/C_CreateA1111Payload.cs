using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Printborg.Types;
using Rhino.Geometry;

namespace PrintborgGH.Components.AI
{
    public class C_CreateA1111Payload : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the C_CreatePayloadA1111 class.
        /// </summary>
        public C_CreateA1111Payload()
          : base("Create Auto1111 Payload", "A1111Payload",
              "Create a payload for Auto1111 API",
              Labels.PluginName, Labels.Category_ImageAI)
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Prompt", "P", "Textual prompt. Be as desscriptive as possible for accurate image generation.", GH_ParamAccess.item, "a red apple");
            pManager.AddTextParameter("Negative Prompt", "NP", "Negative prompts are used to dissuade the AI from generating undesirable results.", GH_ParamAccess.item, "bad, worse, low quality, strange, ugly");
            pManager.AddTextParameter("Sampler Name", "SN", "Choose sampler type. Please refer to Stable Diffusion Documentation for more detailed information.", GH_ParamAccess.item, "Euler");
            pManager.AddIntegerParameter("Steps", "Stp", "Number of iterations.", GH_ParamAccess.item, 20);
            pManager.AddIntegerParameter("CFG Scale", "CFG", "Measure of how closely AI will follow the text prompt.", GH_ParamAccess.item, 7);
            pManager.AddIntegerParameter("Batch Size", "BS", "Number of images to generate.", GH_ParamAccess.item, 1);
            pManager.AddIntegerParameter("Width", "W", "Width of image in pixels", GH_ParamAccess.item, 512);
            pManager.AddIntegerParameter("Height", "H", "Height of image in pixels", GH_ParamAccess.item, 512);
            pManager.AddGenericParameter("Always On Scripts", "AOS", "Optional. Additional settings if extension modules are required.", GH_ParamAccess.item);

            pManager[3].Optional = true;
            pManager[4].Optional = true;
            pManager[5].Optional = true;
            pManager[6].Optional = true;
            pManager[7].Optional = true;
            pManager[8].Optional = true;
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("A1111 Payload", "P", "A A1111Payload object", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            string prompt = null;
            string negativePrompt = null;
            string samplerName = null;
            int steps = 20;
            int cfgScale = 7;
            int batchSize = 1;
            int width = 512;
            int height = 512;

            AlwaysOnScripts? scripts = null;


            DA.GetData(0, ref prompt);
            DA.GetData(1, ref negativePrompt);
            DA.GetData(2, ref samplerName);
            DA.GetData(3, ref steps);
            DA.GetData(4, ref cfgScale);
            DA.GetData(5, ref batchSize);
            DA.GetData(6, ref width);
            DA.GetData(7, ref height);
            DA.GetData(8, ref scripts);

            var payload = new Auto1111Payload(prompt, negativePrompt, steps, cfgScale, width, height, scripts);

            AddRuntimeMessage(GH_RuntimeMessageLevel.Remark, payload.ToString());
            DA.SetData(0, payload);
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
            get { return new Guid("D387CDA1-8280-42E7-8B61-B212CA6114A6"); }
        }

        public override GH_Exposure Exposure => GH_Exposure.secondary;

    }
}