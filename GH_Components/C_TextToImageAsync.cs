using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using AI_Print.GH_Types;
using AI_Print.Types;
using Grasshopper.Kernel;
using GrasshopperAsyncComponent;
using Newtonsoft.Json;

namespace AI_Print.GH_Components {
	public class C_TextToImageAsync : GH_AsyncComponent {


		public C_TextToImageAsync() : base("Sample Async Component", "CYCLOMATRON-X", "Spins uselessly.", Labels.PluginName, Labels.Category_Image) {
			BaseWorker = new FetchImageWorker();
		}

		private class FetchImageWorker : WorkerInstance {
			int MaxIterations { get; set; } = 100;

			private ResponseArtefacts? _responseObject = null;

			public FetchImageWorker() : base(null) { }

			public override async void DoWork(Action<string, double> ReportProgress, Action Done) {
				// Checking for cancellation
				if (CancellationToken.IsCancellationRequested) { return; }

				// if not enough information was collected, exit
				if (_package == null || !_package.Ready) return;

				ReportProgress("Fetching", 0.5);
				var response = await ImagePrompter.POST_TextToImage(_package.ApiKey);

				var responseObject = JsonConvert.DeserializeObject<ResponseArtefacts>(response);

				_debug.Add(responseObject.Artefacts[0].FinishReason);
				//var image = Util.FromBase64String(responseObject.Artefacts[0].Base64);

				try {
					var success = Util.SaveImageFromBase64(responseObject.Artefacts[0].Base64, @"C:\Users\taole\source\repos\AI_Print\user_sketch\output\test4.jpg");
				}
				catch (Exception ex) {
					_debug.Add(ex.Message);
				}


				//await Task.Delay(2000);


				//for (int i = 0; i <= MaxIterations; i++) {
				//	var sw = new SpinWait();
				//	for (int j = 0; j <= 100; j++)
				//		sw.SpinOnce();

				//	ReportProgress(Id, ((double)(i + 1) / (double)MaxIterations));

				//	// Checking for cancellation
				//	if (CancellationToken.IsCancellationRequested) { return; }
				//}

				Done();
			}
			public override WorkerInstance Duplicate() => new FetchImageWorker();

			private class RequestPackage {

				public bool Ready {
					get {
						if (ApiKey == null || Prompts == null || Prompts.Count < 1 || Dir == null || FileName == null) return false;
						return true;
					}
				}

				public string? ApiKey { get; set; }
				public List<TextPrompt>? Prompts { get; set; }
				public string? Dir { get; set; }
				public string? FileName { get; set; }
				public RequestPackage() { }
				public RequestPackage(string apikey, List<TextPrompt> prompts, string dir, string filename) {
					ApiKey = apikey;
					Prompts = prompts;
					Dir = dir;
					FileName = filename;
				}

				public override string ToString() {
					string output = "package {\n";
					output += $"\t ApiKey: {ApiKey}\n";
					output += $"\t Dir: {Dir}\n";
					output += $"\t FileName: {FileName}\n";
					output += "\t Prompts:{\n";
					if (Prompts != null) {
						foreach (var p in Prompts) {
							output += $"\t\t\text: {p.Text}, weight: {p.Weight}";
						}

					}
					output += "\t}";
					output += $"\tReady: {Ready.ToString()}\n}}\n";

					return output;
				}
			}

			private RequestPackage? _package = null;
			private List<string> _debug = new List<string>();

			public override void GetData(IGH_DataAccess DA, GH_ComponentParamServer Params) {
				if (CancellationToken.IsCancellationRequested) return;


				bool processRequest = false;
				string _apiKey = "";
				List<GH_TextPrompt>? _prompts = null;
				string _dir = "";
				string _filename = "";


				if (!DA.GetData("Generate", ref processRequest)) return;
				if (!DA.GetData("API Key", ref _apiKey)) return;
				//if (!DA.GetData("Prompt", ref _prompts)) return;
				if (!DA.GetData("File Directory", ref _dir)) return;
				if (!DA.GetData("File Name", ref _filename)) return;

				if (_apiKey == "" || _dir == null || _filename == null) {
					//AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Stable Diffusion API Key required. Create an account to get one.");
					return;
				}


				_prompts = new List<GH_TextPrompt>();
				_prompts.Add(new GH_TextPrompt(new TextPrompt("red apple with weird human head", 0.5f)));
				_package = new RequestPackage(_apiKey, _prompts.Select(p => p.Value).ToList(), _dir, _filename);

				_debug.Add(_package.ToString());
				//int _maxIterations = 100;
				//DA.GetData(0, ref _maxIterations);
				//if (_maxIterations > 1000) _maxIterations = 1000;
				//if (_maxIterations < 10) _maxIterations = 10;

				//MaxIterations = _maxIterations;
			}

			public override void SetData(IGH_DataAccess DA) {
				if (CancellationToken.IsCancellationRequested) return;


				//if (_responseObject != null && _responseObject.Artefacts != null && _responseObject.Artefacts.Count != 0) {
				//	var image = Util.FromBase64String(_responseObject.Artefacts[0].Base64);

				//	var filepath = _package.Dir +  _package.Filename + ".jpg";
				//	//AddRuntimeMessage(GH_RuntimeMessageLevel.Remark, filepath);
				//	//i2.Save(filepath, ImageFormat.Jpeg);
				//	//var bmp = new Bitmap(image);
				//	//bmp.Save(filepath, ImageFormat.Png);
				//	DA.SetData(0, "Response received.");
				//	return;

				//}


				DA.SetData(0, $"Response not received.");
				DA.SetDataList(1, _debug);
			}
		}


		protected override void RegisterInputParams(GH_InputParamManager pManager) {
			pManager.AddBooleanParameter("Generate", "G", "Send prompt to generate image from text prompt", GH_ParamAccess.item);
			pManager.AddTextParameter("API Key", "Key", "Key for accessing Stable Diffusion API.", GH_ParamAccess.item, "");
			pManager.AddTextParameter("Prompt", "P", "Textual Prompt", GH_ParamAccess.list);
			pManager.AddTextParameter("File Directory", "FD", "Location to save image", GH_ParamAccess.item);
			pManager.AddTextParameter("File Name", "N", "Name of saved image. (Note: If multiple images are generated, a number sequence will be appended to the file name)", GH_ParamAccess.item);
		}

		protected override void RegisterOutputParams(GH_OutputParamManager pManager) {
			pManager.AddTextParameter("Output", "O", "Nothing really interesting.", GH_ParamAccess.item);
			pManager.AddTextParameter("Debug", "D", "Debug Log", GH_ParamAccess.list);
		}

		public override void AppendAdditionalMenuItems(ToolStripDropDown menu) {
			base.AppendAdditionalMenuItems(menu);
			Menu_AppendItem(menu, "Cancel", (s, e) => {
				RequestCancellation();
			});
		}
		public override Guid ComponentGuid {
			get => new Guid("F1E5F78F-242D-44E3-AAD6-AB0257D69256");
		}

		protected override System.Drawing.Bitmap Icon => null;

		public override GH_Exposure Exposure => GH_Exposure.primary;
	}
}
