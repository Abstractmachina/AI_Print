using System;
using System.Collections.Generic;
using System.Drawing.Text;
using System.Linq;
using Grasshopper.Kernel;
using GrasshopperAsyncComponent;
using Newtonsoft.Json;
using Printborg.Controllers;
using Printborg.Interfaces;
using Printborg.Types;
using Rhino.Geometry;

namespace PrintborgGH.GH_Components.ImageAi {
    public class C_GetDeforumJobs : GH_AsyncComponent {
        /// <summary>
        /// Initializes a new instance of the C_GetDeforumJobs class.
        /// </summary>
        public C_GetDeforumJobs()
          : base("Get All Deforum Jobs", "Get DJobs",
              "Get a list of all processing jobs on the server",
              Labels.PluginName, Labels.Category_AI) {
            BaseWorker = new GetJobsWorker(this);

        }

        private class GetJobsWorker : WorkerInstance {
            public List<string> _debug = new List<string>();
            private string _baseAddress = "";

            private List<string> _runtimeMessages = new List<string>();
            private Dictionary<string, DeforumJob> _jobs = new Dictionary<string, DeforumJob>();

            public GetJobsWorker() : base(null) { }
            public GetJobsWorker(GH_AsyncComponent parent2) : base(parent2) {

            }

            public override async void DoWork(Action<string, double> ReportProgress, Action Done) {

                // Checking for cancellation
                if (CancellationToken.IsCancellationRequested) { return; }

                if (!_isOn) return;
                _debug.Clear();
                _jobs.Clear();

                try {

                    // sanity check
                    if (_baseAddress == "") throw new Exception("base address is empty");

                    IApiController controller = new DeforumController(_baseAddress);

                    _debug.Add("baseAddress: " + _baseAddress);
                    _debug.Add("... sending GET request");
                    ReportProgress("0", 0.5d);
                    var rawResponse = await controller.GET_Jobs();
                    _debug.Add("> response received");
                    _debug.Add(rawResponse);

                    if (CancellationToken.IsCancellationRequested) { return; }



                    _jobs = JsonConvert.DeserializeObject<Dictionary<string, DeforumJob>>(rawResponse);
                    foreach (KeyValuePair<string, DeforumJob> entry in _jobs) {
                        //_debug.Add($"{entry.Key} : {entry.Value.Status}, {entry.Value.PhaseProgress}");
                    }


                }
                catch (Exception ex) {
                    _debug.Add(ex.ToString());
                }
                _isOn = false;
                Done();
            }

            public override WorkerInstance Duplicate() => new GetJobsWorker();


            private bool _isOn = false;

            public override void GetData(IGH_DataAccess DA, GH_ComponentParamServer Params) {
                if (CancellationToken.IsCancellationRequested) return;

                if (_isOn) return;

                bool isPressed = false;

                if (!DA.GetData(1, ref isPressed)) {
                    //Parent2.AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Toggle Input required");
                    return;
                }
                if (!DA.GetData(0, ref _baseAddress)) {
                    //Parent2.AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "API base address required");
                    return;
                }

                if (isPressed) { _isOn = true; }
            }

            public override void SetData(IGH_DataAccess DA) {
                if (CancellationToken.IsCancellationRequested) return;

                if (_jobs != null && _jobs.Count != 0) {
                    DA.SetDataList("Job ID", _jobs.Select(j => j.Value.Id));
                    DA.SetDataList("Status", _jobs.Select(j => j.Value.Status));
                    DA.SetDataList("Phase", _jobs.Select(j => j.Value.Phase));
                    DA.SetDataList("Phase Progress", _jobs.Select(j => j.Value.PhaseProgress));
                    DA.SetDataList("Started At", _jobs.Select(j => j.Value.StartedAt));
                    DA.SetDataList("Last Updated", _jobs.Select(j => j.Value.LastUpdated));
                    DA.SetDataList("Execution Time", _jobs.Select(j => j.Value.ExecutionTime));
                    DA.SetDataList("Updates", _jobs.Select(j => j.Value.Updates));
                    DA.SetDataList("Outdir", _jobs.Select(j => j.Value.Outdir));

                }
                DA.SetDataList("Debug", _debug);

            }
        }

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager) {
            pManager.AddTextParameter("API address", "A", "Base API address to hosted or local Auto1111 server", GH_ParamAccess.item);
            pManager.AddBooleanParameter("Submit", "S", "Submit a request to the server.", GH_ParamAccess.item);
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager) {
            //        "id": "batch(948023533)-0",
            //        "status": "SUCCEEDED",
            //        "phase": "DONE",
            //        "error_type": "NONE",
            //        "phase_progress": 1.0,
            //        "started_at": 1693060416.648067,
            //        "last_updated": 1693061418.838495,
            //        "execution_time": 1002.19042801857,
            //        "update_interval_time": 1.8368990421295166,
            //        "updates": 35,
            //        "message": null,
            //        "outdir": "D:\\Repos\\stable-diffusion-webui\\outputs\\img2img-images\\Deforum_01",
            //        "timestring": "20230826163336",
            //        "options_overrides": null
            pManager.AddTextParameter("Job ID", "ID", "Job ID", GH_ParamAccess.list);
            pManager.AddTextParameter("Status", "St", "Status", GH_ParamAccess.list);
            pManager.AddTextParameter("Phase", "Ph", "Current Job Phase", GH_ParamAccess.list);
            pManager.AddTextParameter("Phase Progress", "PP", "Progress (1.0 -> complete)", GH_ParamAccess.list);
            pManager.AddNumberParameter("Started At", "SA", "Time", GH_ParamAccess.list);
            pManager.AddNumberParameter("Last Updated", "LU", "Last updated", GH_ParamAccess.list);
            pManager.AddNumberParameter("Execution Time", "Ex", "Execution Time", GH_ParamAccess.list);
            pManager.AddNumberParameter("Updates", "Up", "Updates", GH_ParamAccess.list);
            pManager.AddTextParameter("Outdir", "Out", "Outdir", GH_ParamAccess.list);
            pManager.AddTextParameter("Debug", "De", "Debug", GH_ParamAccess.list);
        }

        protected override System.Drawing.Bitmap Icon {
            get {
                //You can add image files to your project resources and access them like this:
                // return Resources.IconForThisComponent;
                return null;
            }
        }

        public override Guid ComponentGuid {
            get { return new Guid("F29B9ADD-D20F-447F-A99B-4D4578DB0A6C"); }
        }

        public override GH_Exposure Exposure => GH_Exposure.primary;

    }
}