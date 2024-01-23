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
using System.Net.Http;
using System.Diagnostics;
using GH_IO.Serialization;
using System.Drawing;
using Sprache;
using System.IO;
using Grasshopper.Kernel.Types;
using Printborg.Controllers;

namespace PrintborgGH.Components.AI
{
    public class C_Auto1111TextToImage : GH_AsyncComponent {
        public C_Auto1111TextToImage()
            : base("A1111 Text-To-Image Generation", "A1111T2I",
                  "Generate image from a text prompt in Automatic1111.",
                    Labels.PluginName, Labels.Category_ImageAI) {
            BaseWorker = new FetchImageWorker(this);
            BaseWorker.ParentAsync = this;


        }

        private class FetchImageWorker : WorkerInstance {
            public List<string> _debug = new List<string>();
            private Auto1111Payload? _payload = null;
            private string _responseString = "";
            private string _baseAddress = "";
            private string _dir = "";
            private string _filename = "";
            private List<string> _outputImages = new List<string>();

            public FetchImageWorker() : base(null) { }
            public FetchImageWorker(GH_AsyncComponent parent2) : base(parent2) {

            }

            public override async void DoWork(Action<string, double> ReportProgress, Action Done) {
                _debug.Add("parent: ");
                _debug.Add(Parent == null ? "null" : Parent.ToString());
                _debug.Add("parent2: ");
                _debug.Add(ParentAsync == null ? "null" : ParentAsync.ToString());

                // Checking for cancellation
                if (CancellationToken.IsCancellationRequested) { return; }
                if (!_startRequest) {
                    //Parent2.RequestCancellation();
                    ReportProgress("", 0d);
                    return;
                }

                _debug.Clear();
                _responseString = "";
                _outputImages.Clear();

                try {
                    await Auto1111Controller.Skip(_baseAddress); // cancel any previous jobs
                    _debug.Add("... sending post request");

                    // error checking
                    if (_baseAddress == "") throw new Exception("base address is empty");
                    if (_dir == "") throw new Exception("no directory specified");
                    if (_filename == "") throw new Exception("filename invalid");
                    if (_payload == null) throw new Exception("invalid payload");

                    _debug.Add("baseAddress: " + _baseAddress);

                    // send post request with polling
                    _responseString = await Auto1111Controller.Auto1111TextToImageWithProgressPolling(ReportProgress, _baseAddress, _payload);

                    if (CancellationToken.IsCancellationRequested) { return; }

                    // if a response was received, convert images and save to directory
                    if (_responseString != "") {
                        _debug.Add("... response received");
                        var responseObject = convertResponseString(_responseString);
                        _outputImages = SaveResponseToDirectory(responseObject, _dir, _filename);

                        _debug.Add("... creating current directory");
                        var date = DateTime.Now.ToString("yyMMdd_hhmmss");
                        string path = _dir + date;
                        _debug.Add("output path: " + path);
                        Directory.CreateDirectory(path);

                        for (int i = 0; i < responseObject.Images.Count; i++) {
                            if (CancellationToken.IsCancellationRequested) { return; }
                            _debug.Add("... converting image");
                            string fullPath = path + String.Format("\\{0}{1}.jpeg", _filename, i);
                            _debug.Add("will save at: " + fullPath);
                            ConvertAndSaveBase64ToFile(responseObject.Images[i], fullPath);
                        }
                        _debug.Add("... output saved successfully");
                    }
                }
                catch (Exception ex) {
                    _debug.Add(ex.ToString());
                }

                _startRequest = false; //set boolean gate to false
                Done();
            }


            private ResponseObject convertResponseString(string responseString) {
                var responseObject = JsonConvert.DeserializeObject<ResponseObject>(responseString);
                if (responseObject == null) { throw new Exception("Invalid ResponseObject"); }
                if (responseObject.Images == null) throw new Exception("invalid images received");

                return responseObject;
            }

            /// <summary>
            /// Save images to dir and return as list of base64 strings.
            /// </summary>
            /// <param name="obj">ResponseObject converted from a auto1111 message</param>
            /// <param name="dir">Directory where images will be saved</param>
            /// <param name="filePrefix">filename prefix. if left empty, files will be named "imgXX.jpeg"</param>
            /// <returns></returns>
            /// <exception cref="Exception"></exception>
            private List<string> SaveResponseToDirectory(ResponseObject obj, string dir, string filePrefix = "img") {
                _debug.Add("... creating current directory");
                var date = DateTime.Now.ToString("yyMMdd_hhmmss");
                string fullpath = dir + date;
                _debug.Add("output path: " + fullpath);
                System.IO.Directory.CreateDirectory(fullpath);

                for (int i = 0; i < obj.Images.Count; i++) {
                    _debug.Add("... converting image");
                    string fullPath = fullpath + String.Format("\\{0}{1}.jpeg", filePrefix, i);
                    _debug.Add("will save at: " + fullPath);
                    ConvertAndSaveBase64ToFile(obj.Images[i], fullPath);
                }
                _debug.Add("... output saved successfully");
                return obj.Images;
            }

            private void ConvertAndSaveBase64ToFile(string imageString, string path) {
                var image = Printborg.Util.FromBase64String(imageString);
                image.Save(path, System.Drawing.Imaging.ImageFormat.Jpeg);
            }

            public override WorkerInstance Duplicate() => new FetchImageWorker();

            private bool _startRequest = false;

            public override void GetData(IGH_DataAccess DA, GH_ComponentParamServer Params) {
                if (CancellationToken.IsCancellationRequested) return;

                if (!_startRequest) {
                    if (!DA.GetData("Generate", ref _startRequest)) {
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
                        _filename = "img";
                    }
                }
            }

            public override void SetData(IGH_DataAccess DA) {
                if (CancellationToken.IsCancellationRequested) return;

                if (_outputImages == null || _outputImages.Count == 0) {
                    DA.SetData(0, $"No data");
                } else {
                    DA.SetDataList(0, _outputImages.Select(img => new GH_Image(img)));
                    DA.SetDataList(1, _outputImages.Select(img => new GH_String(img)));

                }
                DA.SetDataList(2, _debug);

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
            pManager.AddGenericParameter("Images", "I", "Images generated from Auto1111", GH_ParamAccess.list);
            pManager.AddGenericParameter("Strings", "S", "Base64 strings for the images", GH_ParamAccess.list);
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

        protected override System.Drawing.Bitmap? Icon => null;

        public override GH_Exposure Exposure => GH_Exposure.secondary;
    }
}
