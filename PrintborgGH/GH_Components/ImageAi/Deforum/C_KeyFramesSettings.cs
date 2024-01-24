using System;

using Grasshopper.Kernel;
using Printborg.Types.Deforum;

namespace PrintborgGH.GH_Components.ImageAi.Deforum
{
    public class C_KeyFramesSettings : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the C_KeyFrameSettings class.
        /// </summary>
        public C_KeyFramesSettings()
          : base("Deforum Settings > KeyFrames", "Key",
              "Keyframes settings tab in Deforum",
              Labels.PluginName, Labels.Category_ImageAI)
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Animation Mode", "Ani", "Animation Mode (accepted values: 2D, 3D)", GH_ParamAccess.item, "2D");
            pManager.AddIntegerParameter("Diffusion Cadence", "Dif", "Diffusion Cadence", GH_ParamAccess.item, 2);
            pManager.AddIntegerParameter("Max Frames", "Fra", "Max Frames", GH_ParamAccess.item, 10);
            pManager.AddIntegerParameter("Frames per Second", "FPS", "Frames per Second", GH_ParamAccess.item, 15);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Keyframes settings", "Key", "Printborg.Types.Deforum.KeyframesSettings", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            string animationMode = "";
            int diffusionCadence = 0;
            int maxFrames = 0;
            int fps = 0;

            if (!DA.GetData(0, ref animationMode)) return;
            if (!DA.GetData(1, ref diffusionCadence)) return;
            if (!DA.GetData(2, ref maxFrames)) return;
            if (!DA.GetData(3, ref fps)) return;


            DA.SetData(0, new KeyFramesSettings(animationMode, diffusionCadence, maxFrames,  fps));
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
            get { return new Guid("FF5F657F-1754-4318-96C1-43C5DDCCD533"); }
        }
    }
}