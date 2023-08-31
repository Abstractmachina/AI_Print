using System;
using System.Collections.Generic;
using System.Drawing;
using Grasshopper.Kernel;
using Printborg.GH_Types;
using Rhino.Geometry;

namespace PrintborgGH.GH_Components.Images
{
    public class C_LoadImage : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the C_LoadImage class.
        /// </summary>
        public C_LoadImage()
          : base("Load Image", "LoadImg",
              "Load Image from directory.",
              Labels.PluginName, Labels.Category_Images)
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("File Path", "pth", "File path", GH_ParamAccess.item);
            pManager.AddNumberParameter("Scale", "Sca", "Scale Factor (1.0 if no input)", GH_ParamAccess.item, 1.0d);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Image", "Img", "Bitmap image", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            string path = "";
            if (!DA.GetData("File Path", ref path)) return;

            double scale = 1.0d;
            DA.GetData("Scale", ref scale);


            var image = new Bitmap(path);

            DA.SetData("Image", new GH_Image(image));

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
            get { return new Guid("DEE693EE-8E57-49C9-B90D-C03AF7FFE144"); }
        }
    }
}