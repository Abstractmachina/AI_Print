using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Grasshopper.Kernel;
using GrasshopperAsyncComponent;
using Newtonsoft.Json;
using Printborg.Controllers;
using Printborg.Interfaces;
using Printborg.Types;
using Rhino.Geometry;

namespace PrintborgGH.GH_Components.ImageAi
{
    public class C_SendDeforumJob : GH_AsyncComponent {
        /// <summary>
        /// Initializes a new instance of the C_SendDeforumJob class.
        /// </summary>
        public C_SendDeforumJob()
          : base("Send Deforum Job", "Deforum",
              "Send a video generation job to Deforum server",
              Labels.PluginName, Labels.Category_AI)
        {
            BaseWorker = new InitJobWorker(this);
        }

        private class InitJobWorker : WorkerInstance {
            private bool _startRequest = false;
            public List<string> _debug = new List<string>();
            private string _payload = "";
            private JobStatus _jobStatus = null;
            private string _baseAddress = "";

            public InitJobWorker() : base(null) { }
            public InitJobWorker(GH_AsyncComponent parent2) : base(parent2) {

            }

            public override async void DoWork(Action<string, double> ReportProgress, Action Done) {

                // Checking for cancellation
                if (CancellationToken.IsCancellationRequested) { return; }
                if (!_startRequest) {
                    //Parent2.RequestCancellation();
                    ReportProgress("", 0d);
                    return;
                }

                _debug.Clear();
                _jobStatus = null;

                try {

                    // error checking
                    if (_baseAddress == "") throw new Exception("base address is empty");
                    if (_payload == null) throw new Exception("invalid payload");

                    IApiController controller = new DeforumController(_baseAddress, 30);

                    _debug.Add("baseAddress: " + _baseAddress);
                    _debug.Add("... sending post request");
                    var rawResponse = await controller.POST_Job(_payload);
                    _debug.Add(rawResponse);

                    if (CancellationToken.IsCancellationRequested) { return; }



                    _jobStatus = JsonConvert.DeserializeObject<JobStatus>(rawResponse);
                    _debug.Add("> response received");
                    _debug.Add(_jobStatus.ToString());
                    
                    // response format example
                    //{
                    //    "message": "Job(s) accepted",
                    //    "batch_id": "batch(843362695)",
                    //    "job_ids": [
                    //        "batch(843362695)-0"
                    //    ]
                    //}

                }
                catch (Exception ex) {
                    _debug.Add(ex.ToString());
                }

                _startRequest = false; //set boolean gate to false
                Done();
            }

            public override WorkerInstance Duplicate() => new InitJobWorker();


            public override void GetData(IGH_DataAccess DA, GH_ComponentParamServer Params) {
                if (CancellationToken.IsCancellationRequested) return;

                if (!_startRequest) {
                    if (!DA.GetData(0, ref _startRequest)) {
                        Parent.AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Input required");
                        return;
                    }
                    if (!DA.GetData(1, ref _baseAddress)) {
                        Parent.AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "API base address required.");
                        return;
                    }
                    if (!DA.GetData(2, ref _payload)) {
                        Parent.AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Deforum Payload required. Please refer to official documentation for further details.");
                        return;
                    }
                }
            }

            public override void SetData(IGH_DataAccess DA) {
                if (CancellationToken.IsCancellationRequested) return;

                if (_jobStatus != null) {
                    DA.SetData(0, _jobStatus.Message);
                    DA.SetData(1, _jobStatus.BatchId);
                    DA.SetDataList(2, _jobStatus.JobIds);
                }
                DA.SetDataList(3, _debug);

            }
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddBooleanParameter("Generate", "G", "Start Image generation request. (Hint: use boolean button)", GH_ParamAccess.item);
            pManager.AddTextParameter("API Address", "A", "API address of hosted or local Auto1111 server", GH_ParamAccess.item, "");
            pManager.AddGenericParameter("Payload", "P", "Auto1111 Payload", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("Success Status", "S", "Message whether Batch was submitted successfully.", GH_ParamAccess.item);
            pManager.AddGenericParameter("Batch ID", "B", "Batch ID", GH_ParamAccess.item);
            pManager.AddGenericParameter("Job IDs", "J", "Job IDs", GH_ParamAccess.list);
            pManager.AddTextParameter("Debug", "D", "Debug Log", GH_ParamAccess.list);
        }

        public override void AppendAdditionalMenuItems(ToolStripDropDown menu) {
            base.AppendAdditionalMenuItems(menu);
            Menu_AppendItem(menu, "Cancel", (s, e) => {
                RequestCancellation();
            });
        }

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
            get { return new Guid("9AD2EF4F-E0F3-49E2-939C-808627460D1F"); }
        }

        public override GH_Exposure Exposure => GH_Exposure.primary;

        private class JobStatus {
            [JsonProperty("message")]
            public string Message { get; set; }
            [JsonProperty("batch_id")]
            public string BatchId { get; set; }
            [JsonProperty("job_ids")]
            public List<string> JobIds { get; set; }
        }

    }
}