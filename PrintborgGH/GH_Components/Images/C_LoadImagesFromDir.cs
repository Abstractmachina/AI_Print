using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Grasshopper.Kernel;
using Printborg.GH_Types;
using Rhino.Geometry;

namespace PrintborgGH.GH_Components.Images
{
    public class C_LoadImagesFromDir : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the C_LoadImagesFromDir class.
        /// </summary>
        public C_LoadImagesFromDir()
          : base("Load Images From Dir", "LoadDirImg",
              "Load all images from a specified directory.",
              Labels.PluginName, Labels.Category_Images)
        {

        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Directory", "Dir", "Absolute path to directory", GH_ParamAccess.item);
            pManager.AddNumberParameter("Scale", "Sca", "Scale Factor (1.0 if no input)", GH_ParamAccess.item, 1.0d);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Images", "Img", "Bitmap images", GH_ParamAccess.list);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            string path = "";
            if (!DA.GetData("Directory", ref path)) return;

            double scale = 1.0d;
            DA.GetData("Scale", ref scale);

            //get a list of all image files
            var allFiles = Directory.EnumerateFiles(path, "*.*", SearchOption.AllDirectories)
                .Where(s => 
                    s.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase) || 
                    s.EndsWith(".png", StringComparison.OrdinalIgnoreCase)
                );

            // convert file paths to GH_Image and set to output
            DA.SetDataList("Images", allFiles.Select(f => new GH_Image(new Bitmap(f))));
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
            get { return new Guid("F803F645-8ED1-41DE-90CD-B2690C562617"); }
        }
    }
}