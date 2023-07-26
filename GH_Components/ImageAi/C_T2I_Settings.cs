using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;

namespace AI_Print.GH_Components.ImageAi {
	public class C_T2I_Settings : GH_Component {
		public C_T2I_Settings()
		  : base("C_T2I_Settings", "Nickname",
			  "Description",
			  Labels.PluginName, Labels.Category_Image) {
		}

		protected override void RegisterInputParams(GH_InputParamManager pManager) {

		}

		/// <summary>
		/// Registers all the output parameters for this component.
		/// </summary>
		protected override void RegisterOutputParams(GH_OutputParamManager pManager) {
		}

		/// <summary>
		/// This is the method that actually does the work.
		/// </summary>
		/// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
		protected override void SolveInstance(IGH_DataAccess DA) {
		}

		/// <summary>
		/// Provides an Icon for the component.
		/// </summary>
		protected override System.Drawing.Bitmap Icon {
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
			get { return new Guid("83117875-4CC6-4EDF-B071-CB1FC8737FC4"); }
		}
	}
}