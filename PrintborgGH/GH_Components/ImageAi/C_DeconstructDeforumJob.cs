using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Printborg.Interfaces;
using Printborg.Types;
using Printborg.Types.Deforum;
using PrintborgGH.GH_Types;
using Rhino.Geometry;

namespace PrintborgGH.GH_Components.ImageAi
{
    public class C_DeconstructDeforumJob : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the C_DeconstructDeforumJob class.
        /// </summary>
        public C_DeconstructDeforumJob()
          : base("Deconstruct Deforum Job", "Decon DJob",
              "Deconstruct a Deforum Job into its individual properties.",
              Labels.PluginName, Labels.Category_AI)
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Deforum Job", "Job", "A Deforum Job Object", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            //      Format:
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

            pManager.AddTextParameter("ID", "ID", "ID", GH_ParamAccess.item);
            pManager.AddTextParameter("Status", "Sta", "Status", GH_ParamAccess.item);
            pManager.AddTextParameter("Phase", "Pha", "Phase", GH_ParamAccess.item);
            pManager.AddTextParameter("Error", "Err", "Error (if any)", GH_ParamAccess.item);
            pManager.AddNumberParameter("Progress", "Pro", "Job Progress (1.0 for complete).", GH_ParamAccess.item);
            pManager.AddNumberParameter("Started At", "StA", "Time", GH_ParamAccess.item);
            pManager.AddNumberParameter("Last Updated", "LUp", "Last updated", GH_ParamAccess.item);
            pManager.AddNumberParameter("Execution Time", "Exe", "Execution Time", GH_ParamAccess.item);
            pManager.AddNumberParameter("Updates", "Upd", "Updates", GH_ParamAccess.item);
            pManager.AddTextParameter("Outdir", "Out", "Outdir", GH_ParamAccess.item);
        }


        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var input = new GH_Img2ImgJob();
            if (!DA.GetData(0, ref input)) return;


            if (input.Value == null) {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Invalid Input");
            }
            IJob job = input.Value;
            DA.SetData("ID", job.Id);
            DA.SetData("Status", job.Status);
            DA.SetData("Phase", job.Phase);
            DA.SetData("Error", job.ErrorType);
            DA.SetData("Progress", job.Progress);
            DA.SetData("Started At", job.StartedAt);
            DA.SetData("Last Updated", job.LastUpdated);
            DA.SetData("Execution Time", job.ExecutionTime);
            DA.SetData("Updates", job.Updates);
            DA.SetData("Outdir",job.Outdir);
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
        public override Guid ComponentGuid => new Guid("7F12FB90-2A0B-4393-8FBB-90102A001ED4");
        public override GH_Exposure Exposure => GH_Exposure.primary;
    }
}