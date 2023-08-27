using Printborg.Types;
using Grasshopper;
using Grasshopper.Kernel;
using Newtonsoft.Json;
using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

using Printborg;

namespace PrintborgGH.Components.AI {

	/// <summary>
	/// OBSOLETE
	/// </summary>
	public class C_Text2ImagePrompt : GH_Component {
		/// <summary>
		/// Each implementation of GH_Component must provide a public 
		/// constructor without any arguments.
		/// Category represents the Tab in which the component will appear, 
		/// Subcategory the panel. If you use non-existing tab or panel names, 
		/// new tabs/panels will automatically be created.
		/// </summary>
		public C_Text2ImagePrompt()
		  : base("Text-to-Image", "T2I",
			"Generate images from a text prompt",
			Labels.PluginName, Labels.Category_AI) { }

		protected override void RegisterInputParams(GH_InputParamManager pManager) {
			pManager.AddBooleanParameter("Generate", "G", "Send prompt to generate image from text prompt", GH_ParamAccess.item);
			pManager.AddTextParameter("Payload", "P", "Auto1111 Payload", GH_ParamAccess.item, "");
			pManager.AddTextParameter("File Directory", "FD", "Location to save image", GH_ParamAccess.item);
			pManager.AddTextParameter("File Name", "N", "Name of saved image. (Note: If multiple images are generated, a number sequence will be appended to the file name)", GH_ParamAccess.item);

			// If you want to change properties of certain parameters, 
			// you can use the pManager instance to access them by index:
			//pManager[0].Optional = true;
		}

		/// <summary>
		/// Registers all the output parameters for this component.
		/// </summary>
		protected override void RegisterOutputParams(GH_OutputParamManager pManager) {
			pManager.AddTextParameter("Raw POST Response", "Res", "Raw Response received from API", GH_ParamAccess.item);
			pManager.AddTextParameter("Base64", "B64", "Base64 encoded image", GH_ParamAccess.item);

			// Sometimes you want to hide a specific parameter from the Rhino preview.
			// You can use the HideParameter() method as a quick way:
			//pManager.HideParameter(0);
		}


		bool processRequest = false;
		//SDResponse? lastResponse = null;
		string? lastRawResponse = null;
		/// <summary>
		/// This is the method that actually does the work.
		/// </summary>
		/// <param name="DA">The DA object can be used to retrieve data from input parameters and 
		/// to store data in output parameters.</param>
		protected override void SolveInstance(IGH_DataAccess DA) {

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
				//lastRawResponse = await Auto1111Controller.POST_TextToImage(apiKey);
				//if (lastRawResponse != null && lastRawResponse != "") lastResponse = JsonConvert.DeserializeObject<SDResponse>(lastRawResponse);
				processRequest = false;
				ExpireSolution(true);
				return;
			}

			if (lastRawResponse == null) {
				AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "No output found.");
			}
			else {
				Console.WriteLine(lastRawResponse);
				var responseObject = JsonConvert.DeserializeObject<ResponseArtefacts>(lastRawResponse);
				AddRuntimeMessage(GH_RuntimeMessageLevel.Remark, responseObject.Artefacts.ToString());
				AddRuntimeMessage(GH_RuntimeMessageLevel.Remark, responseObject.Artefacts[0].FinishReason);


				if (responseObject != null && responseObject.Artefacts != null && responseObject.Artefacts.Count != 0) {
					var image = Printborg.Util.FromBase64String(responseObject.Artefacts[0].Base64);

					var filepath = dir + filename + ".jpg";
					AddRuntimeMessage(GH_RuntimeMessageLevel.Remark, filepath);
					//i2.Save(filepath, ImageFormat.Jpeg);
					var bmp = new Bitmap(image);
					bmp.Save(filepath, ImageFormat.Png);
				}
				//image.Save(dir + filename + ".jpg");

				//if (responseObject == null) throw new Exception("API request failed. ResponseObject is null");
				// var rr = JsonConvert.DeserializeObject<Dictionary<string, object>>(responseObject.artifacts[0]);
				// foreach(KeyValuePair<string, object>pair in rr) {
				//     Console.WriteLine(pair.Key);
				//     Console.WriteLine(pair.Value);
				// }
				// Console.WriteLine(responseObject.artifacts.Count);
				// foreach (var v in responseObject.artifacts)
				// Console.WriteLine(v);
				// foreach(KeyValuePair<string, object>item in responseObject) Console.WriteLine(item.Key.GetType());
				DA.SetData("Raw POST Response", lastRawResponse.Length);
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
		public override GH_Exposure Exposure => GH_Exposure.hidden;

		/// <summary>
		/// Provides an Icon for every component that will be visible in the User Interface.
		/// Icons need to be 24x24 pixels.
		/// You can add image files to your project resources and access them like this:
		/// return Resources.IconForThisComponent;
		/// </summary>
		protected override Bitmap Icon => null;

		/// <summary>
		/// Each component must have a unique Guid to identify it. 
		/// It is vital this Guid doesn't change otherwise old ghx files 
		/// that use the old ID will partially fail during loading.
		/// </summary>
		public override Guid ComponentGuid => new Guid("46A9FC57-D95F-40A1-9A23-F39D8B8D5C91");
	}
}