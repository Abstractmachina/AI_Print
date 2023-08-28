using Grasshopper.Kernel.Types;
using Printborg.GH_Types;
using Printborg.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintborgGH.GH_Types {
    public class GH_DeforumJob : GH_Goo<DeforumJob> {

        #region PROPERTIES
        public override bool IsValid {
            get { if (this.Value == null) return false; else return true; }

        }
        public override string TypeName { get { return "DeforumJob"; } }
        public override string TypeDescription { get { return "Deforum Job information regarding queued jobs in the server."; } }
        #endregion

        #region CONSTRUCTORS
        public GH_DeforumJob() {
            this.Value = null;
        }

        public GH_DeforumJob(DeforumJob job) {
            this.Value = job;
        }

        public GH_DeforumJob(GH_DeforumJob source) {
            this.Value = source.Value;
        }
        #endregion


        public override DeforumJob Value { get => base.Value; set => base.Value = value; }

        public override IGH_Goo Duplicate() {
            return new GH_DeforumJob(this);
        }

        public override string ToString() {
            return Value.ToString();
        }

        public override object ScriptVariable() {
            return this.Value;
        }

        public override bool CastTo<Q>(ref Q target) {

            //cast to string = base64
            //if (typeof(Q).IsAssignableFrom(typeof(string))) {
            //    var rawString = Util.ToBase64String(this.Value);
            //    object ptr = new GH_String(rawString);
            //    target = (Q)ptr;
            //    return true;
            //}

            //cast to mesh

            return false;
        }

        public override bool CastFrom(object source) {

            if (source == null) return false;
            //if (source.GetType() == typeof(Mesh)) {
            //    //convert mesh to image
            //}
            //if (source.GetType() == typeof(string)) {
            //    // possibly a base64 encoded image 
            //    var img = Printborg.Util.FromBase64String((string)source);
            //    this.Value = img;
            //    return true;
            //}
            return false;

        }
    }
}
