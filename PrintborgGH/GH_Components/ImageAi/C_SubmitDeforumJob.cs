using System;
using System.Collections.Generic;
using System.Threading;
using System.Linq;
using System.Windows.Forms;
using Grasshopper.Kernel;
using GrasshopperAsyncComponent;
using Newtonsoft.Json;
using Printborg.Controllers;
using Printborg.Interfaces;
using Printborg.Services;
using Printborg.Types;
using Printborg.Types.Deforum;
using Rhino.Geometry;
using PrintborgGH.Types;

namespace PrintborgGH.GH_Components.ImageAi
{
    public class C_SubmitDeforumJob : GH_AsyncComponent {

        #region COMPONENTDEFINITION
        public C_SubmitDeforumJob()
          : base("Submit Deforum Job", "Go Deforum",
              "Submit job to Deforum server",
              Labels.PluginName, Labels.Category_ImageAI)
        {
            BaseWorker = new SubmitJobWorker(this);
        }

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddBooleanParameter("Generate", "G", "Start Image generation request. (Hint: use boolean button)", GH_ParamAccess.item);
            pManager.AddTextParameter("API Address", "A", "API address of hosted or local Auto1111 server", GH_ParamAccess.item);
            pManager.AddGenericParameter("Payload", "P", "Auto1111 Payload", GH_ParamAccess.item);
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Success Status", "S", "Message whether Batch was submitted successfully.", GH_ParamAccess.item);
            pManager.AddTextParameter("Debug", "D", "Debug Log", GH_ParamAccess.list);
        }

        public override void AppendAdditionalMenuItems(ToolStripDropDown menu) {
            base.AppendAdditionalMenuItems(menu);
            Menu_AppendItem(menu, "Cancel", (s, e) => {
                RequestCancellation();
            });
        }

        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                //You can add image files to your project resources and access them like this:
                // return Resources.IconForThisComponent;
                return null;
            }
        }

        public override Guid ComponentGuid
        {
            get { return new Guid("9AD2EF4F-E0F3-49E2-939C-808627460D1F"); }
        }

        public override GH_Exposure Exposure => GH_Exposure.primary;
        #endregion

        //======================================================
        //======================================================
        //======================================================

        #region WORKER_DEFINITION
        /// <summary>
        /// Worker Definition
        /// </summary>
        private class SubmitJobWorker : WorkerInstance {
            private bool _startRequest = false;
            public List<string> _debug = new List<string>();
            private string _payload = "";
            private string _baseAddress = "";
            private ImageToImageClient _client = null;
            private IJob _output = null;
            private bool _inProgress = false;
            private List<(GH_RuntimeMessageLevel, string)> _runtimeMessages { get; set; }

            //public SubmitJobWorker() : base(null) {
            //    _runtimeMessages = new List<(GH_RuntimeMessageLevel, string)>();
            //}

            public SubmitJobWorker(GH_Component parent) : base(parent) {
                _runtimeMessages = new List<(GH_RuntimeMessageLevel, string)>();
            }


            public override async void DoWork(Action<string, double> ReportProgress, Action Done) {

                // Checking for cancellation
                if (CancellationToken.IsCancellationRequested) { return; }

                if (!_startRequest) {
                    return;
                }

                try {
                    _debug.Clear();

                    // error checking
                    if (_baseAddress == "") throw new Exception("base address is empty");
                    if (_payload == null) throw new Exception("invalid payload");

                    //_debug.Add("baseAddress: " + _baseAddress);
                    _runtimeMessages.Add((GH_RuntimeMessageLevel.Remark, "baseAddress: " + _baseAddress));
                    //_debug.Add("... sending post request");
                    _runtimeMessages.Add((GH_RuntimeMessageLevel.Remark, "... sending post request"));
                    _runtimeMessages.Add((GH_RuntimeMessageLevel.Remark, "... Submitting Job ..."));

                    _client = new ImageToImageClient(new DeforumController(_baseAddress, 30));

                    // TODO: cancel previous jobs if server allows multiple jobs concurrently

                    var response = await _client.SubmitJob(_payload);

                    string jobId = response.Id;
                    ReportProgress(Id, 0.01);

                    if (CancellationToken.IsCancellationRequested) { return; }

                    _debug.Add("> response received");
                    _debug.Add(response.ToString());

                    // response format example
                    //{
                    //    "message": "Job(s) accepted",
                    //    "batch_id": "batch(843362695)",
                    //    "job_ids": [
                    //        "batch(843362695)-0"
                    //    ]
                    //}


                    // start loop. for each loop query server. update progress bar and update output object. 
                    bool isFinished = false;

                    DeforumJob job = new DeforumJob();


                    while (!isFinished) {
                        if (CancellationToken.IsCancellationRequested) { return; }

                        job = (DeforumJob) (await _client.GetJob(jobId))[0];
                        //check status. if status is SUCCESS, it means job is finished. 
                        if (job.Status != Status.ACCEPTED) {
                            ReportProgress(Id, 1.0d);
                            isFinished = true;
                            break;
                        }
                        //get progress
                        ReportProgress(Id, job.Progress);
                        Thread.Sleep(2000); // check every 2sec
                    }

                    // set output object
                    _runtimeMessages.Add((GH_RuntimeMessageLevel.Remark, $"Job finished with status({job.Status})"));
                    _output = job;
                    _inProgress = false; //set boolean gate to false
                    Done();
                    return;
                }
                catch (Exception ex) {
                    //_debug.Add(ex.ToString());
                    _runtimeMessages.Add((GH_RuntimeMessageLevel.Error, ex.ToString()));
                    Done();
                }

                Done();
            }



            public override void GetData(IGH_DataAccess DA, GH_ComponentParamServer Params) {
                if (CancellationToken.IsCancellationRequested) return;

                    if (!DA.GetData(0, ref _startRequest)) {
                        _runtimeMessages.Add((GH_RuntimeMessageLevel.Error, "Input required"));
                        return;
                    }

                if (!DA.GetData(1, ref _baseAddress)) {
                    _runtimeMessages.Add((GH_RuntimeMessageLevel.Error, "API base address required."));
                    return;
                }
                if (!DA.GetData(2, ref _payload)) {
                    _runtimeMessages.Add((GH_RuntimeMessageLevel.Error, "Deforum Payload required. Please refer to official documentation for further details."));
                    return;
                }
            }

            public override void SetData(IGH_DataAccess DA) {
                if (CancellationToken.IsCancellationRequested) return;

                foreach (var (level, message) in _runtimeMessages)
                    Parent.AddRuntimeMessage(level, message);

                DA.SetData(0, new GH_Img2ImgJob(_output));
                DA.SetDataList(1, _debug);
            }

            public override WorkerInstance Duplicate() => new SubmitJobWorker(this.Parent);

        }
        #endregion
    }
}