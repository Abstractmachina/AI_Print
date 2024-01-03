using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Grasshopper.Kernel;
using Printborg.GH_Types;
using Rhino.Geometry;

namespace PrintborgGH.GH_Components.Images {
    public class C_DisplayImage : GH_Component {
        /// <summary>
        /// Initializes a new instance of the C_DisplayImage class.
        /// </summary>
        public C_DisplayImage()
          : base("Display Image", "Image",
              "Display an image in the viewport as Rhino mesh",
              Labels.PluginName, Labels.Category_Images) {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_InputParamManager pManager) {
            pManager.AddGenericParameter("Image", "I", "Image", GH_ParamAccess.item);
            pManager.AddPlaneParameter("Plane", "P", "Plane to orient image in the viewport", GH_ParamAccess.item, Plane.WorldXY);
            pManager.AddNumberParameter("Scale", "S", "Scale factor of image. If unspecified, 1 pixel = 1 rhino unit", GH_ParamAccess.item, 1d);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_OutputParamManager pManager) {
            pManager.AddMeshParameter("Mesh Preview", "M", "Image represented as mesh.", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA) {

            GH_Image gh_image = new GH_Image();
            Plane plane = Plane.Unset;
            double scale = 1d;
            if (!DA.GetData(0, ref gh_image)) {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Invalid input image");
                return;
            }
            if (!DA.GetData(1, ref plane)) {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "No orientation plane provided. Plane will be set to WorldXY");
            }
            DA.GetData(2, ref scale);

            Mesh m = new Mesh();
            var bmp = gh_image.Value;
            int w = bmp.Width;
            int h = bmp.Height;



            BitmapData bData = bmp.LockBits(new Rectangle(0, 0, w, h), ImageLockMode.ReadOnly, bmp.PixelFormat);
            var bitsPerPixel = (byte)System.Drawing.Image.GetPixelFormatSize(bData.PixelFormat);
            unsafe {
                /*This time we convert the IntPtr to a ptr*/
                byte* scan0 = (byte*) bData.Scan0.ToPointer();


                for (int y = 0; y < h; ++y) {
                    for (int x = 0; x < w; ++x) {
                        byte* data = scan0 + y * bData.Stride + x * bitsPerPixel / 8;

                        var col = Color.FromArgb(data[2], data[1], data[0]);
                        //data is a pointer to the first byte of the 3-byte color data
                        //data[0] = blueComponent;
                        //data[1] = greenComponent;
                        //data[2] = redComponent;

                        double xs = x * scale;
                        double ys = y * -scale;
                        m.Vertices.Add(xs, ys, 0);
                        m.VertexColors.Add(col);

                    }

                }
            }
            bmp.UnlockBits(bData);

            for (int i = w; i < m.Vertices.Count; i++) {

                if ((i + 1) % w == 0) continue;

                var face = new MeshFace(i - w, i - w + 1, i + 1, i);
                m.Faces.AddFace(face);

            }

            // throws access violation error 
            //Parallel.ForEach(Partitioner.Create(w, m.Vertices.Count), range => {
            //    for (int i = range.Item1; i < range.Item2; i++) {
            //        if ((i + 1) % w == 0) continue;

            //        var face = new MeshFace(i - w, i - w + 1, i + 1, i);
            //        m.Faces.AddFace(face);
            //    }
            //});

            m.RebuildNormals();
            DA.SetData(0, m);

        }

        /// <summary>
        /// Provides an Icon for the component.
        /// </summary>
        protected override Bitmap Icon {
            get {
                //You can add image files to your project resources and access them like this:
                // return Resources.IconForThisComponent;
                return null;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid {
            get { return new Guid("C6F82EA5-88FB-4D8E-87F6-F4F2744780AA"); }
        }
        public override GH_Exposure Exposure => GH_Exposure.secondary;


        private class DirectBitmap : IDisposable {
            public Bitmap Bitmap { get; private set; }
            public Int32[] Bits { get; private set; }
            public bool Disposed { get; private set; }
            public int Height { get; private set; }
            public int Width { get; private set; }

            protected GCHandle BitsHandle { get; private set; }

            public DirectBitmap(int width, int height) {
                Width = width;
                Height = height;
                Bits = new Int32[width * height];
                BitsHandle = GCHandle.Alloc(Bits, GCHandleType.Pinned);
                Bitmap = new Bitmap(width, height, width * 4, PixelFormat.Format32bppPArgb, BitsHandle.AddrOfPinnedObject());
            }

            public DirectBitmap(Bitmap bmp) {
                Width = bmp.Width;
                Height = bmp.Height;
                Bits = new Int32[Width * Height];
                BitsHandle = GCHandle.Alloc(Bits, GCHandleType.Pinned);
                Bitmap = bmp;
            }

            public void SetPixel(int x, int y, Color colour) {
                int index = x + (y * Width);
                int col = colour.ToArgb();

                Bits[index] = col;
            }

            public Color GetPixel(int x, int y) {
                int index = x + (y * Width);
                int col = Bits[index];
                Color result = Color.FromArgb(col);

                return result;
            }

            public void Dispose() {
                if (Disposed) return;
                Disposed = true;
                Bitmap.Dispose();
                BitsHandle.Free();
            }
        }
    }
}