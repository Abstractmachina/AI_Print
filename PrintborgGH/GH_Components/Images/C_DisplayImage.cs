using System;
using System.Collections.Generic;
using System.Drawing;
using Grasshopper.Kernel;
using Printborg.GH_Types;
using Rhino.Geometry;

namespace PrintborgGH.GH_Components.Images
{
    public class C_DisplayImage : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the C_DisplayImage class.
        /// </summary>
        public C_DisplayImage()
          : base("Display Image", "Image",
              "Display an image in the viewport as Rhino mesh",
              Labels.PluginName, Labels.Category_Images)
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Image", "I", "Image", GH_ParamAccess.item);
            pManager.AddPlaneParameter("Plane", "P", "Plane to orient image in the viewport", GH_ParamAccess.item, Plane.WorldXY);
            pManager.AddNumberParameter("Scale", "S", "Scale factor of image. If unspecified, 1 pixel = 1 rhino unit", GH_ParamAccess.item, 1d);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddMeshParameter("Mesh Preview", "M", "Image represented as mesh.", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {

            GH_Image img = new GH_Image();
            Plane plane = Plane.Unset;
            double scale = 1d;
            if (!DA.GetData(0, ref img))
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Invalid input image");
                return;
            }
            if (!DA.GetData(1, ref plane))
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "No orientation plane provided. Plane will be set to WorldXY");
            }
            DA.GetData(2, ref scale);

            var meshList = new List<Mesh>();
            Mesh m = new Mesh();
            Bitmap i = new Bitmap(img.Value);
            // iterate through pixels
            for (int y = 0; y < img.Value.Height; y++) {
                for (int x = 0; x < img.Value.Width; x++) {
                    // get argb color of pixel
                    // for each pixel, build mesh face with sampled color
                    // add face to mesh
                    var pixel = i.GetPixel(x,y);

                    var mesh = new Mesh();

                    double xs = x * scale;
                    double ys = y * -scale;

                    mesh.Vertices.Add(xs, ys, 0);
                    mesh.Vertices.Add(xs + scale, ys, 0);
                    mesh.Vertices.Add(xs + scale, ys - scale, 0);
                    mesh.Vertices.Add(xs, ys - scale, 0);

                    for (int c = 0; c < 4; c++) {
                        mesh.VertexColors.Add(pixel);
                    }

                    var face = new MeshFace(0, 1, 2, 3);
                    mesh.Faces.AddFace(face);
                    m.Append(mesh);

                }
            }

            DA.SetData(0, m);

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
            get { return new Guid("C6F82EA5-88FB-4D8E-87F6-F4F2744780AA"); }
        }
        public override GH_Exposure Exposure => GH_Exposure.secondary;
    }
}