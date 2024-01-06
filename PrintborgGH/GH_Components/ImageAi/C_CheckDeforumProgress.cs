using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using GrasshopperAsyncComponent;
using Newtonsoft.Json;
using Printborg.Controllers;
using Printborg.Interfaces;
using Printborg.Types;
using Printborg.Types.Deforum;
using Rhino.Geometry;

namespace PrintborgGH.GH_Components.ImageAi {
    public class C_CheckDeforumProgress : GH_AsyncComponent {
        /// <summary>
        /// Initializes a new instance of the C_CheckDeforumProgress class.
        /// </summary>
        public C_CheckDeforumProgress()
          : base("Check Deforum Progress", "Check Deforum",
              "Request status update from server on current Deforum job.",
              Labels.PluginName, Labels.Category_AI) {
            BaseWorker = new CheckStatusWorker(this);
        }

        private class CheckStatusWorker : WorkerInstance {
            public List<string> _debug = new List<string>();
            private string _baseAddress = "";

            private string _batchId = "";
            private int _queryInterVal = 2;
            private bool _isOn = false;
            private List<string> _runtimeMessages = new List<string>();
            private List<DeforumJob> _jobStatusList = new List<DeforumJob>();

            public CheckStatusWorker() : base(null) { }
            public CheckStatusWorker(GH_AsyncComponent parent2) : base(parent2) {

            }

            public override async void DoWork(Action<string, double> ReportProgress, Action Done) {

                // Checking for cancellation
                if (CancellationToken.IsCancellationRequested) { return; }

                if (!_isOn) return;

                _debug.Clear();
                _jobStatusList.Clear();

                try {

                    // sanity checking
                    if (_baseAddress == "") throw new Exception("base address is empty");

                    IApiController controller = new DeforumController(_baseAddress);

                    _debug.Add("baseAddress: " + _baseAddress);
                    _debug.Add("... sending GET request");
                    ReportProgress(_batchId, 0.5d);
                    var rawResponse = await controller.GET_Batch(_batchId);
                    _debug.Add("> response received");
                    _debug.Add(rawResponse);

                    if (CancellationToken.IsCancellationRequested) { return; }



                    _jobStatusList = JsonConvert.DeserializeObject<List<DeforumJob>>(rawResponse);
                    _debug.Add(_jobStatusList[0].ToString());

                }
                catch (Exception ex) {
                    _debug.Add(ex.ToString());
                }

                _isOn = false; //set boolean gate to false
                Done();
            }

            public override WorkerInstance Duplicate() => new CheckStatusWorker();


            public override void GetData(IGH_DataAccess DA, GH_ComponentParamServer Params) {
                if (CancellationToken.IsCancellationRequested) return;

                if (!DA.GetData(0, ref _isOn)) {
                    //Parent2.AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Toggle Input required");
                    return;
                }
                if (!DA.GetData(1, ref _baseAddress)) {
                    //Parent2.AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "API base address required");
                    return;
                }
                if (!DA.GetData(2, ref _batchId)) {
                    //Parent2.AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "batch id required.");
                    return;
                }
                if (!DA.GetData(3, ref _queryInterVal)) {
                    //Parent2.AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Deforum Payload required. Please refer to official documentation for further details.");
                    return;
                }

            }

            public override void SetData(IGH_DataAccess DA) {
                if (CancellationToken.IsCancellationRequested) return;

                if (_jobStatusList != null && _jobStatusList.Count != 0) {
                    DA.SetData(0, _jobStatusList[0].Status);
                    DA.SetData(1, _jobStatusList[0].Phase);
                    DA.SetData(2, _jobStatusList[0].PhaseProgress);
                    DA.SetData(3, _jobStatusList[0].Updates);
                }
                DA.SetDataList(4, _debug);

            }
        }



        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager) {

            pManager.AddBooleanParameter("On", "On", "Toggle on Progress Querying.", GH_ParamAccess.item);
            pManager.AddTextParameter("API Address", "A", "API address of hosted or local Auto1111 server", GH_ParamAccess.item);
            pManager.AddTextParameter("Batch ID", "BID", "Batch Id to be queried", GH_ParamAccess.item);
            pManager.AddNumberParameter("Query Interval", "QI", "Time interval (in sec) to query server", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager) {
            pManager.AddTextParameter("Status", "S", "Jobs Status", GH_ParamAccess.item);
            pManager.AddTextParameter("Phase", "P", "Jobs Phase", GH_ParamAccess.item);
            pManager.AddNumberParameter("Progress", "PR", "Jobs Progress", GH_ParamAccess.item);
            pManager.AddNumberParameter("Updates Count", "U", "Number of Updates completed", GH_ParamAccess.item);
            pManager.AddTextParameter("Debug", "D", "Debug", GH_ParamAccess.list);

        }

        /// <summary>
        /// Provides an Icon for the component.
        /// </summary>
        protected override System.Drawing.Bitmap Icon {
            get {
                //You can add image files to your project resources and access them like this:
                // return Resources.IconForThisComponent;
                return null;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid {
            get { return new Guid("70ABEC2B-9D8B-4345-8091-715FF3BA2364"); }
        }

        public override GH_Exposure Exposure => GH_Exposure.primary;
    }


}