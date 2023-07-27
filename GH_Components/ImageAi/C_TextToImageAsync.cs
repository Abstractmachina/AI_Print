using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using AI_Print.GH_Types;
using AI_Print.Types;
using Grasshopper.Kernel;
using GrasshopperAsyncComponent;
using Newtonsoft.Json;
using static System.Net.WebRequestMethods;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace AI_Print.GH_Components.ImageAi {
	public class C_TextToImageAsync : GH_AsyncComponent {


		public C_TextToImageAsync() : base("Text To Image Async", "T2IA", "Does not block GH while fetching API response.", Labels.PluginName, Labels.Category_Image) {
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

                // prepare scribble image as base64 string
                // prepare scribble image as base64 string
                byte[] imageArray = System.IO.File.ReadAllBytes(@"D:\Repos\AI_Print\user_sketch\scribbleTest_01.png");
                var scribble = Convert.ToBase64String(imageArray);

				_debug.Add(scribble);

                // FOR TESTING
                // TODO: Delete
                var serverAddress = "http://127.0.0.1:7860";
                var payload = new Auto1111Payload("3d printing clay, layer, toolpath", "bad, worse, low quality, strange, ugly", 20, 7, 598, 624, new AlwaysOnScripts(ControlNetSettingsFactory.Create("control_v11p_sd15_scribble [d4ba51ff]", "scribble_hed", scribble)));

                var response = await ImagePrompter.Auto1111_T2I(serverAddress, "admin", "admin", payload);

                var responseObject = JsonConvert.DeserializeObject<ResponseObject>(response);



                // Console.WriteLine(responseObject.Images.Select(x => Console.WriteLine(x));
                if (responseObject != null)
                {
                    for (int i = 0; i < responseObject.Images.Count; i++)
                    {
                        var image = Util.FromBase64String(responseObject.Images[i]);
                        var date = DateTime.Now.ToString("yymmddhhmmss");
                        string path = String.Format("./user_sketch/output/{0}", date);
                        System.IO.Directory.CreateDirectory(path);
                        image.Save(path + String.Format("/img{0}.png", i), System.Drawing.Imaging.ImageFormat.Png);
                    }
                }


                //_debug.Add(responseObject.Artefacts[0].FinishReason);
                //var image = Util.FromBase64String(responseObject.Artefacts[0].Base64);

                try {
					//var success = Util.SaveImageFromBase64(responseObject.Artefacts[0].Base64, @"C:\Users\taole\source\repos\AI_Print\user_sketch\output\test4.jpg");
				}
				catch (Exception ex) {
					_debug.Add(ex.Message);
				}


				//await Task.Delay(2000);


				//for (int i = 0; i <= MaxIterations; i++) {
				//	var sw = new SpinWait();
				//	for (int j = 0; j <= 100; j++)
				//		sw.SpinOnce();sla

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
			private Auto1111Payload? _payload = null;

			public override void GetData(IGH_DataAccess DA, GH_ComponentParamServer Params) {
				if (CancellationToken.IsCancellationRequested) return;


				bool processRequest = false;
				string _apiKey = "";
				List<GH_TextPrompt>? _prompts = null;
				string _dir = "";
				string _filename = "";


				if (!DA.GetData("Generate", ref processRequest)) return;
				if (!DA.GetData("API Address", ref _apiKey)) return;
				//if (!DA.GetData("Auto1111 Payload", ref _prompts)) return;
				if (!DA.GetData("File Directory", ref _dir)) return;
				if (!DA.GetData("File Name", ref _filename)) return;


				//_prompts = new List<GH_TextPrompt>();
				//_prompts.Add(new GH_TextPrompt(new TextPrompt("red apple with weird human head", 0.5f)));
				//_package = new RequestPackage(_apiKey, _prompts.Select(p => p.Value).ToList(), _dir, _filename);


				


				//_payload = new Auto1111Payload("3d printing clay, layer, toolpath", "bad, worse, low quality, strange, ugly", 20, 7, 598, 624, new AlwaysOnScripts(ControlNetSettingsFactory.Create("control_v11p_sd15_scribble [d4ba51ff]", "scribble_hed", scribble)));



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
			pManager.AddTextParameter("API Address", "A", "API address of hosted or local Auto1111 server", GH_ParamAccess.item, "");
			pManager.AddTextParameter("Payload", "P", "Auto1111 Payload", GH_ParamAccess.list);
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
