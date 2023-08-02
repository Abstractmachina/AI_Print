using System;
using System.Collections.Generic;
using System.Drawing;
using Grasshopper.Kernel;
using Rhino.Geometry;
using SpatialSlur.SlurCore;
using SpatialSlur.SlurField;
using SpatialSlur.SlurRhino;

namespace Printborg.Components.Display {
	public class C_DisplayField : GH_Component {
		/// <summary>
		/// Initializes a new instance of the C_FieldDisplay class.
		/// </summary>
		public C_DisplayField()
		  : base("Field Display", "FDisp",
			  "Displays scalar field with specified color range.",
			  Labels.PluginName, Labels.Category_Display) {
		}

		/// <summary>
		/// Registers all the input parameters for this component.
		/// </summary>
		protected override void RegisterInputParams(GH_InputParamManager pManager) {
			pManager.AddMeshParameter("Mesh", "M", "Base mesh.", GH_ParamAccess.item);
			pManager.AddGenericParameter("Field", "F", "Field.", GH_ParamAccess.item);
			pManager.AddColourParameter("Colors", "C", "User color range", GH_ParamAccess.list);
		}

		/// <summary>
		/// Registers all the output parameters for this component.
		/// </summary>
		protected override void RegisterOutputParams(GH_OutputParamManager pManager) {
			pManager.AddMeshParameter("Colored Mesh", "M", "Colored mesh.", GH_ParamAccess.item);
		}

		/// <summary>
		/// This is the method that actually does the work.
		/// </summary>
		/// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
		protected override void SolveInstance(IGH_DataAccess DA) {
			IField3d<double> f = null;
			Mesh mesh = null;
			List<Color> colors = new List<Color>();

			if (!DA.GetData(0, ref mesh)) return;
			if (!DA.GetData(1, ref f)) return;
			if (!DA.GetDataList(2, colors)) return;

			mesh.ColorVertices(i => { return colors.Lerp(f.ValueAt(mesh.Vertices[i])); }, true);

			DA.SetData(0, mesh);
		}

		/// <summary>
		/// Provides an Icon for the component.
		/// </summary>
		protected override Bitmap Icon {
			get {
				return null;
				//return Resources.FDisplay;

			}
		}

		/// <summary>
		/// Gets the unique ID for this component. Do not change this ID after release.
		/// </summary>
		public override Guid ComponentGuid {
			get { return new Guid("9A8C11E0-64F3-48EB-BF25-59829A84285D"); }
		}
	}
}