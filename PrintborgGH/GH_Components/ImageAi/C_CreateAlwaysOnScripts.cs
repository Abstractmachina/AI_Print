using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Printborg.Types;
using Rhino.Geometry;

namespace PrintborgGH.GH_Components.ImageAi
{
    public class C_CreateAlwaysOnScripts : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the C_CreateAlwaysOnScripts class.
        /// </summary>
        public C_CreateAlwaysOnScripts()
          : base("Create AlwaysOn Scripts", "AO Scripts",
              "Create an AlwaysOnScripts object if any extensions for Auto1111 need to be used. Please refer to the official Auto1111 API documentation for more details.",
              Labels.PluginName, Labels.Category_AI)
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Model", "M", "Stable Diffusion extension model used", GH_ParamAccess.item);
            pManager.AddTextParameter("Module", "M", "Stable Diffusion module used", GH_ParamAccess.item);
            pManager.AddTextParameter("Input Image", "I", "Base64-encoded image used for certain extensions (e.g. Scribble)", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("AlwaysOnScripts", "AOS", "AlwaysOnScripts object to be fed into the CreateAuto1111Payload component", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            string model = null;
            string module = null;
            string inputImage = null;

            if (!DA.GetData(0, ref model)) return;
            if (!DA.GetData(1, ref module)) return;
            DA.GetData(2, ref inputImage);

            if (inputImage == null) AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "No input image found");
            var scripts = new AlwaysOnScripts(ControlNetSettingsFactory.Create(model, module, inputImage));

            AddRuntimeMessage(GH_RuntimeMessageLevel.Remark, scripts.ToString());

            DA.SetData(0, scripts);
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
            get { return new Guid("A117F547-E782-46A3-B61D-32093754CDF0"); }
        }

        public override GH_Exposure Exposure => GH_Exposure.secondary;

    }
}