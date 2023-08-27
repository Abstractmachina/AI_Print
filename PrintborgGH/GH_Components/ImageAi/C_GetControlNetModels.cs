using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Grasshopper.Kernel;
using GrasshopperAsyncComponent;
using Newtonsoft.Json;
using Printborg.Controllers;
using Printborg.Types;
using PrintborgGH.GH_Types;
using Rhino.Geometry;

namespace PrintborgGH.Components.AI
{
    public class C_GetControlNetModels : GH_AsyncComponent
    {
        public C_GetControlNetModels()
          : base("Get Controlnet Models", "Models",
              "Get currently installed models from server",
              Labels.PluginName, Labels.Category_AI)
        {
            BaseWorker = new fetchAuto1111ModelWorker();
        }

        private class fetchAuto1111ModelWorker : WorkerInstance
        {
            public List<string> _debug = new List<string>();
            private bool _processRequest = false;
            private string _baseAddress = "";
            private string _resultString = "";


            public fetchAuto1111ModelWorker() : base(null) { }

            public override async void DoWork(Action<string, double> ReportProgress, Action Done)
            {
                // Checking for cancellation
                if (CancellationToken.IsCancellationRequested) { return; }

                if (!_processRequest) { return; }

                _debug.Clear();

                try
                {
                    if (_baseAddress == "") throw new Exception("base address is empty");

                    _debug.Add("baseAddress: " + _baseAddress);
                    _resultString = await Auto1111Controller.GetControlnetModels(_baseAddress, ReportProgress);
                }
                catch (Exception ex)
                {
                    _debug.Add(ex.ToString());
                }

                _processRequest = false;
                Done();

            }
            public override WorkerInstance Duplicate() => new fetchAuto1111ModelWorker();


            public override void GetData(IGH_DataAccess DA, GH_ComponentParamServer Params)
            {
                if (CancellationToken.IsCancellationRequested) return;

                if (!_processRequest)
                {
                    if (!DA.GetData("Send", ref _processRequest))
                    {
                        Parent.AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Input required");
                        return;
                    }

                    if (!DA.GetData("API Address", ref _baseAddress))
                    {
                        Parent.AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "API base address required.");
                        return;
                    }
                }
            }

            public override void SetData(IGH_DataAccess DA)
            {
                if (CancellationToken.IsCancellationRequested) return;


                if (_resultString == "") DA.SetData(0, $"Response not received.");
                else
                {
                    var models = JsonConvert.DeserializeObject<ModelList>(_resultString).Models;
                    DA.SetDataList(0, models);

                }

                DA.SetDataList(1, _debug);

            }
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddBooleanParameter("Send", "S", "Send out API request", GH_ParamAccess.item);
            pManager.AddTextParameter("API Address", "A", "API address of hosted or local Auto1111 server", GH_ParamAccess.item, "");

        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("Models", "M", "Currently installed models", GH_ParamAccess.list);
            pManager.AddTextParameter("Debug", "D", "Debug Log", GH_ParamAccess.list);

        }

        public override void AppendAdditionalMenuItems(ToolStripDropDown menu)
        {
            base.AppendAdditionalMenuItems(menu);
            Menu_AppendItem(menu, "Cancel", (s, e) =>
            {
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
            get { return new Guid("BBA0CCBB-78DD-4301-A21A-6FA31D9D5012"); }
        }

        public override GH_Exposure Exposure => GH_Exposure.secondary;


        private class ModelList
        {
            [JsonProperty("model_list")]
            public List<string> Models { get; set; }

            public ModelList() { }


        }
    }
}