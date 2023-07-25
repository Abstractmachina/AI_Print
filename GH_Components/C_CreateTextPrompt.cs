using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;

namespace AI_Print.Grasshopper
{
    public class C_CreateTextPrompt : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the C_CreateTextPrompt class.
        /// </summary>
        public C_CreateTextPrompt()
          : base("Create Text Prompts", "Prompts",
              "Create one or multiple text prompts with weights.",
              Labels.PluginName, Labels.Category_Param)
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Text", "T", "Text content of prompt.", GH_ParamAccess.item);
            pManager.AddTextParameter("Weights", "W", "Weight as a factor between 0-1. If list length does not match number of text prompt, a default value of 0.5 will be assigned for the remaining weights.", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Text Prompts", "P", "List of text prompts.", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
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
            get { return new Guid("B7F6B172-055C-42F9-ACF3-D478A59D78FE"); }
        }
    }
}