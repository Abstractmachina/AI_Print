using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using Grasshopper.Kernel;
using Printborg.GH_Types;
using Printborg.Interfaces;
using PrintborgGH.Types;
using Rhino.Geometry;

namespace PrintborgGH.GH_Components.Images
{
    public class C_LoadFrames : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the C_LoadFrames class.
        /// </summary>
        public C_LoadFrames()
          : base("Load Frames", "Load Frames",
              "Load all frames in a job's output folder.",
              Labels.PluginName, Labels.Category_Images)
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("IJob", "J", "IJob object", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Image Frames", "F", "Image Frames", GH_ParamAccess.list);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            GH_Img2ImgJob input = new GH_Img2ImgJob();
            if (!DA.GetData(0, ref input))
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Input invalid. Must be a Img2Img Job type.");
                return;
            }

            IJob job = input.Value;
            if (job == null)
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "No input found.");
                return;
            }


            //get a list of all image files
            var allFiles = Directory.EnumerateFiles(job.Outdir, "*.*", SearchOption.AllDirectories)
                .Where(s =>
                    s.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase) ||
                    s.EndsWith(".png", StringComparison.OrdinalIgnoreCase)
                );

            // convert file paths to GH_Image and set to output
            DA.SetDataList(0, allFiles.Select(f => new GH_Image(new Bitmap(f))));
        }

        /// <summary>
        /// Provides an Icon for the component.
        /// </summary>
        protected override Bitmap Icon
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
            get { return new Guid("95C8DA44-9812-4A51-822B-C5A16B600C7C"); }
        }
    }
}