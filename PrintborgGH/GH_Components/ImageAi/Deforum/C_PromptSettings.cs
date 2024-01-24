using System;
using System.Collections.Generic;
using GH_Dictionary;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using Printborg.Types.Deforum;
using PrintborgGH.libs;
using Rhino.Geometry;

namespace PrintborgGH.GH_Components.ImageAi.Deforum
{
    public class C_PromptSettings : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the C_PromptSettings class.
        /// </summary>
        public C_PromptSettings()
          : base("Deforum Settings > Prompt", "Pro",
              "Prompt settings tab in Deforum",
              Labels.PluginName, Labels.Category_ImageAI)
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Prompts", "Pro", "Text Prompts definable for specific frames (Input must be a GH_Dict type)", GH_ParamAccess.item);
            pManager.AddGenericParameter("Positive Prompts", "Pos", "Positive Prompts", GH_ParamAccess.item);
            pManager.AddGenericParameter("Negative Prompts", "Neg", "Negative Prompts", GH_ParamAccess.item);

            pManager[0].Optional = true;
            pManager[1].Optional = true;
            pManager[2].Optional = true; 
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Prompt Settings tab in Deforum", "Pro", "Printborg.Types.Deforum.PromptSettings", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var promptsWrapper = new GH_Dict();
            var positivePromptsWrapper = new GH_Dict();
            var negativePromptsWrapper = new GH_Dict();

            if (!DA.GetData(0, ref promptsWrapper)) return;
            if (!DA.GetData(1, ref positivePromptsWrapper)) return;
            if (!DA.GetData(2, ref negativePromptsWrapper)) return;
        
            try {
                var prompts = libs.GH_Convert.ToDictionary<int, string>(promptsWrapper);
                var positivePrompt = libs.GH_Convert.ToDictionary<int, string>(positivePromptsWrapper);
                var negativePrompts = libs.GH_Convert.ToDictionary<int, string>(negativePromptsWrapper);

                DA.SetData(0, new PromptSettings(prompts, positivePrompt, negativePrompts));
                return;
            }
            catch (Exception e) {

                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, $"Conversion to Dictionary<int, string> failed:\n{e.Message}");
                return;
            }
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
            get { return new Guid("020CBF4A-E687-4466-8358-3B32C58F9983"); }
        }
    }
}