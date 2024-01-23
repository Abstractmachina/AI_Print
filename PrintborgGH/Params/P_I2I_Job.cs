using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using Printborg.GH_Types;
using PrintborgGH.Types;
using Rhino.Geometry;

namespace PrintborgGH.Params
{
    public class P_I2I_Job : GH_Param<GH_Img2ImgJob>
    {
        public P_I2I_Job()
          : base("Image2Image Job", "I2I_Job",
              "Job object containing the information of a job on the server.",
              Labels.PluginName, Labels.Category_Param, GH_ParamAccess.item)
        {
        }


        public override string TypeName {
            get {
                return "Img2Img Job";
            }
        }

        /// <summary>
        /// Since IGH_Goo is an interface rather than a class, we HAVE to override this method. 
        /// For IGH_Goo parameters it's usually good to return a blank GH_ObjectWrapper.
        /// </summary>
        protected override GH_Img2ImgJob InstantiateT() {
            //return new GH_ObjectWrapper();
            return new GH_Img2ImgJob();
        }

        ///// <summary>
        ///// Since our parameter is of type IGH_Goo, it will accept ALL data. 
        ///// We need to remove everything now that is not, GH_Colour, GH_Curve or null.
        ///// </summary>
        //protected override void OnVolatileDataCollected() {
        //    for (int p = 0; p < m_data.PathCount; p++) {
        //        var branch = m_data.Branches[p];
        //        for (int i = 0; i < branch.Count; i++) {
        //            IGH_Goo goo = branch[i];
        //        }
        //    }
        //}
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
            get { return new Guid("9DFCC8E6-49B6-4C65-A220-95BAC7FB1B76"); }
        }

        public override GH_Exposure Exposure => GH_Exposure.primary;

    }
}