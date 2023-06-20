using Grasshopper;
using Grasshopper.Kernel;
using Newtonsoft.Json;
using Rhino.Geometry;
using System;
using System.Collections.Generic;

namespace AI_Print.Grasshopper {
	public class C_Text2ImagePrompt : GH_Component {
		/// <summary>
		/// Each implementation of GH_Component must provide a public 
		/// constructor without any arguments.
		/// Category represents the Tab in which the component will appear, 
		/// Subcategory the panel. If you use non-existing tab or panel names, 
		/// new tabs/panels will automatically be created.
		/// </summary>
		public C_Text2ImagePrompt()
		  : base("Text-to-Image Prompt", "T2I",
			"Generate an image from a text prompt",
			Labels.PluginName, Labels.Category_Image) { }

		/// <summary>
		/// Registers all the input parameters for this component.
		/// </summary>
		protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager) {
			pManager.AddBooleanParameter("Generate", "G", "Send prompt to generate image from text prompt", GH_ParamAccess.item);
			pManager.AddTextParameter("API Key", "Key", "Key for accessing Stable Diffusion API.", GH_ParamAccess.item, "");
			pManager.AddTextParameter("Prompt", "P", "Textual Prompt", GH_ParamAccess.item, "");
			pManager.AddTextParameter("File Directory", "FD", "Location to save image", GH_ParamAccess.item);
			pManager.AddTextParameter("File Name", "N", "Name of saved image. (Note: If multiple images are generated, a number sequence will be appended to the file name)", GH_ParamAccess.item);

			// If you want to change properties of certain parameters, 
			// you can use the pManager instance to access them by index:
			//pManager[0].Optional = true;
		}

		/// <summary>
		/// Registers all the output parameters for this component.
		/// </summary>
		protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager) {
			pManager.AddTextParameter("Raw POST Response", "Res", "Raw Response received from API", GH_ParamAccess.item);
			pManager.AddTextParameter("Base64", "B64", "Base64 encoded image", GH_ParamAccess.list);

			// Sometimes you want to hide a specific parameter from the Rhino preview.
			// You can use the HideParameter() method as a quick way:
			//pManager.HideParameter(0);
		}


		bool processRequest = false;
		SDResponse? lastResponse = null;
		string? lastRawResponse = null;
		/// <summary>
		/// This is the method that actually does the work.
		/// </summary>
		/// <param name="DA">The DA object can be used to retrieve data from input parameters and 
		/// to store data in output parameters.</param>
		protected override async void SolveInstance(IGH_DataAccess DA) {

			string apiKey = "";
			string prompt = "";
			string dir = "";
			string filename = "";

			if (!DA.GetData("Generate", ref processRequest)) return;
			if (!DA.GetData("API Key", ref apiKey)) return;
			if (!DA.GetData("Prompt", ref prompt)) return;
			if (!DA.GetData("File Directory", ref dir)) return;
			if (!DA.GetData("File Name", ref filename)) return;

			if (apiKey == "") {
				AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Stable Diffusion API Key required. Create an account to get one.");
			}
			
			//when user sends of request, splash msg will be displayed. after new response is received, it is saved and solution is expired.
			if (processRequest) {
				DA.SetData("Raw POST Response", "... PROCESSING ...");
				DA.SetData("Link", "... PROCESSING ...");
				lastRawResponse = await ImagePrompter.POST_TextToImage(apiKey);
				//if (lastRawResponse != null && lastRawResponse != "") lastResponse = JsonConvert.DeserializeObject<SDResponse>(lastRawResponse);
				processRequest = false;
				ExpireSolution(true);
				return;
			}

			if (lastRawResponse == null) {
				AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "No output found.");
				return;
			} else {
				DA.SetData("Raw POST Response", lastRawResponse);
			}
			//if (lastResponse != null) {
			//	DA.SetDataList("Link", lastResponse.output);
			//}

		}

		/// <summary>
		/// The Exposure property controls where in the panel a component icon 
		/// will appear. There are seven possible locations (primary to septenary), 
		/// each of which can be combined with the GH_Exposure.obscure flag, which 
		/// ensures the component will only be visible on panel dropdowns.
		/// </summary>
		public override GH_Exposure Exposure => GH_Exposure.primary;

		/// <summary>
		/// Provides an Icon for every component that will be visible in the User Interface.
		/// Icons need to be 24x24 pixels.
		/// You can add image files to your project resources and access them like this:
		/// return Resources.IconForThisComponent;
		/// </summary>
		protected override System.Drawing.Bitmap Icon => null;

		/// <summary>
		/// Each component must have a unique Guid to identify it. 
		/// It is vital this Guid doesn't change otherwise old ghx files 
		/// that use the old ID will partially fail during loading.
		/// </summary>
		public override Guid ComponentGuid => new Guid("46A9FC57-D95F-40A1-9A23-F39D8B8D5C91");
	}
}