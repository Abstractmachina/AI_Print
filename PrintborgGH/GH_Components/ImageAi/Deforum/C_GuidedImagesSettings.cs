using System;
using System.Collections.Generic;
using GH_Dictionary;
using Grasshopper.Documentation;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using Printborg.Types.Deforum;
using Rhino.Geometry;

namespace PrintborgGH.GH_Components.ImageAi.Deforum
{
    public class C_GuidedImagesSettings : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the C_GuidedImagesSettings class.
        /// </summary>
        public C_GuidedImagesSettings()
          : base("Deforum Settings > Guided Images", "Gui",
              "Guided Images settings tab in Deforum",
              Labels.PluginName, Labels.Category_ImageAI)
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Init Images", "Ini", "Init images as GH_Dictionary<string, string>", GH_ParamAccess.item);
            pManager[0].Optional = true;
            pManager.AddTextParameter("Image Strength Schedule", "Str", "Image Strength Schedule", GH_ParamAccess.item, "0:(0.75)");
            pManager.AddTextParameter("Blend Factor Max", "Max", "Blend Factor Max", GH_ParamAccess.item, "0:(0.35)");
            pManager.AddTextParameter("Blend Factor Slope", "Slo", "Blend Factor Slope", GH_ParamAccess.item, "0:(0.25)");
            pManager.AddTextParameter("Tweening Frames Schedule", "Str", "Tweening Frames Schedule", GH_ParamAccess.item, "0:(20)");
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Guided Images Settings", "Img", "Printborg.Types.Deforum.GuidedImagesSettings", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            //try get input data
            GH_Dict initImagesWrapper = new GH_Dict();
            string imageStrengthSchedule = "";
            string blendFactorMax = "";
            string blendFactorSlope ="";
            string tweeningFramesSchedule = "";
            
            if (!DA.GetData(0, ref initImagesWrapper)) return;
            if (!DA.GetData(1, ref imageStrengthSchedule)) return;
            if (!DA.GetData(2, ref blendFactorMax)) return;
            if (!DA.GetData(3, ref blendFactorSlope)) return;
            if (!DA.GetData(4, ref tweeningFramesSchedule)) return;
            
            //try to convert gh_dict to Dictionary<string, string>
            var initImages = new Dictionary<string, string>();
            foreach (KeyValuePair<string, IGH_Goo> entry in initImagesWrapper.Value) {
                try {
                    var val = ((GH_String)entry.Value).Value;
                    initImages.Add(entry.Key, val);
                }
                catch (Exception e) {
                    AddRuntimeMessage(GH_RuntimeMessageLevel.Error, $"Error casting to Dictionary<string, string> :\n{e.Message}");
                    return;
                }

            }
            // assign default values if initImages empty
            if (initImages.Count == 0) {
                initImages.Add("0", "https://deforum.github.io/a1/Gi1.png");
                initImages.Add("max_f/4-5", "https://deforum.github.io/a1/Gi2.png");
                initImages.Add("max_f/2-10", "https://deforum.github.io/a1/Gi3.png");
                initImages.Add("3*max_f/4-15", "https://deforum.github.io/a1/Gi4.png");
                initImages.Add("max_f-20", "https://deforum.github.io/a1/Gi1.png");
            }


            //Set output object
            DA.SetData(0, new GuidedImagesSettings(initImages, imageStrengthSchedule, blendFactorMax, blendFactorSlope, tweeningFramesSchedule));
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
            get { return new Guid("7DFB8328-6B97-4C6B-B50B-E8A7C3E7174C"); }
        }
    }
}