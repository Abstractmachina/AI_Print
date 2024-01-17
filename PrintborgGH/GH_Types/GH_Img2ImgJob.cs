using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using Printborg.Interfaces;
using Rhino.Geometry;

namespace PrintborgGH.GH_Types
{
    public class GH_Img2ImgJob : GH_Goo<IJob>
    {
        #region PROPERTIES
        public override bool IsValid => true;
        public override string TypeName { get { return "ImageToImage Job Package"; } }
        public override string TypeDescription { get { return "Job information regarding queued jobs on the server."; } }

        public override IJob? Value { get => base.Value; set => base.Value = value; }
        #endregion


        public GH_Img2ImgJob()
        {
            this.Value = null;
        }

        public GH_Img2ImgJob(IJob job) {
            this.Value = job;
        }

        public GH_Img2ImgJob(GH_Img2ImgJob job) {
            this.Value = job.Value;
        }

        public override IGH_Goo Duplicate() {
            return new GH_Img2ImgJob(this);
        }

        public override string ToString() {
            if (this.Value != null) {
               string output = "IJob {\n\t";

                output += $"ID: {this.Value.Id}\n\t"
                 + $"Status: {this.Value.Status}\n\t"
                 + $"Phase: {this.Value.Phase}\\tn"
                 + $"Progress: {this.Value.Progress}\n"
                + "}";

                return output;
            }
            return "Invalid Job";
        }

        public override bool CastTo<Q>(ref Q target) {
            return false;
        }

        public override bool CastFrom(object source) {
            return false;
        }
    }
}