using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Printborg.GH_Types;
using Printborg.Types;
using Grasshopper.Kernel;
using GrasshopperAsyncComponent;
using Newtonsoft.Json;
using Printborg;

namespace PrintborgGH.Components.AI {
	public class C_Auto1111TextToImage : GH_AsyncComponent {
		public C_Auto1111TextToImage() : base("A1111 Text-To-Image", "A41T2I", "Generate image from a text prompt in Automatic1111.", Labels.PluginName, Labels.Category_AI) {
			BaseWorker = new FetchImageWorker();
		}

		private class FetchImageWorker : WorkerInstance {
			public List<string> _debug = new List<string>();
			private Auto1111Payload? _payload = null;
			int MaxIterations { get; set; } = 100;

			private ResponseArtefacts? _responseObject = null;

			public FetchImageWorker() : base(null) { }

			public override async void DoWork(Action<string, double> ReportProgress, Action Done) {
				// Checking for cancellation
				if (CancellationToken.IsCancellationRequested) { return; }

				try {

					//ReportProgress("Working", 0.5);

					// FOR TESTING
					// TODO: Delete after linked to data input in gh
					//var serverAddress = "http://127.0.0.1:7860"; // local auto1111 server
					//var serverAddress = "https://37af3813aead713f87.gradio.live";
					//var payload = new Auto1111Payload("3d printing clay, layer, toolpath", "bad, worse, low quality, strange, ugly", 20, 7, 598, 624, new AlwaysOnScripts(ControlNetSettingsFactory.Create("control_v11p_sd15_scribble [d4ba51ff]", "scribble_hed", scribble)));


					_debug.Add("... sending post request");




					var response = await ImagePrompter.Auto1111TextToImageWithReport(ReportProgress, _baseAddress, _payload);


					// Console.WriteLine(responseObject.Images.Select(x => Console.WriteLine(x));
					if (response != null) {
						_debug.Add("... response received");
						_debug.Add("... creating current directory");
						if (response.Images == null) throw new Exception("invalid images received");
						var date = DateTime.Now.ToString("yymmdd.hhmmss");
						string path = String.Format("./user_sketch/output/{0}", date);
						System.IO.Directory.CreateDirectory(path);

						for (int i = 0; i < response.Images.Count; i++) {
							_debug.Add("... converting image");

							var image = Printborg.Util.FromBase64String(response.Images[i]);
							image.Save(path + String.Format("/img{0}.png", i), System.Drawing.Imaging.ImageFormat.Png);
						}
						_debug.Add("... output saved successfully");
					}


					//_debug.Add(responseObject.Artefacts[0].FinishReason);
					//var image = Util.FromBase64String(responseObject.Artefacts[0].Base64);

					//var success = Util.SaveImageFromBase64(responseObject.Artefacts[0].Base64, @"C:\Users\taole\source\repos\Printborg\user_sketch\output\test4.jpg");
				}
				catch (Exception ex) {
					_debug.Add(ex.Message);
				}
				Done();


				//await Task.Delay(2000);


				//for (int i = 0; i <= MaxIterations; i++) {
				//	var sw = new SpinWait();
				//	for (int j = 0; j <= 100; j++)
				//		sw.SpinOnce();sla

				//	ReportProgress(Id, ((double)(i + 1) / (double)MaxIterations));

				//	// Checking for cancellation
				//	if (CancellationToken.IsCancellationRequested) { return; }
				//}

				//Done();
			}
			public override WorkerInstance Duplicate() => new FetchImageWorker();


			/// <summary>
			/// OBSOLETE
			/// </summary>
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


			private bool _processRequest = false;
			private string _baseAddress = "";
			private string _dir = "";
			private string _filename = "";

			public override void GetData(IGH_DataAccess DA, GH_ComponentParamServer Params) {
				if (CancellationToken.IsCancellationRequested) return;

				bool startProcess = false;

				if (!DA.GetData("Generate", ref startProcess)) {
					Parent.AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Input required");
					return;
				}
				if (!DA.GetData("API Address", ref _baseAddress)) {
					Parent.AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "API base address required.");
					return;
				}
				if (!DA.GetData("Payload", ref _payload)) {
					Parent.AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Auto1111 Payload required");
					return;
				}
				if (!DA.GetData("File Directory", ref _dir)) {
					Parent.AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Please specify folder where the output will be saved");
					return;
				}
				if (!DA.GetData("File Name", ref _filename)) {
					Parent.AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "File prefix not specified. Files will be prefixed with 'img'");
				}

				//_payload = new Auto1111Payload("3d printing clay, layer, toolpath", "bad, worse, low quality, strange, ugly", 20, 7, 598, 624, new AlwaysOnScripts(ControlNetSettingsFactory.Create("control_v11p_sd15_scribble [d4ba51ff]", "scribble_hed", scribble)));


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
			pManager.AddBooleanParameter("Generate", "G", "Start Image generation request. (Hint: use boolean button)", GH_ParamAccess.item);
			pManager.AddTextParameter("API Address", "A", "API address of hosted or local Auto1111 server", GH_ParamAccess.item, "");
			pManager.AddGenericParameter("Payload", "P", "Auto1111 Payload", GH_ParamAccess.item);
			pManager.AddTextParameter("File Directory", "FD", "Location to save image", GH_ParamAccess.item);
			pManager.AddTextParameter("File Name", "N", "Name of saved image. (Note: If multiple images are generated, a number sequence will be appended to the file name)", GH_ParamAccess.item);
		}

		protected override void RegisterOutputParams(GH_OutputParamManager pManager) {
			pManager.AddTextParameter("Image Link", "L", "Path to saved image on disk.", GH_ParamAccess.item);
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
