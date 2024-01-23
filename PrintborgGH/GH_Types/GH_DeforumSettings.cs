using Grasshopper.Kernel.Types;
using Printborg.Interfaces;
using Printborg.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintborgGH.Types {
    public class GH_DeforumSettings :GH_Goo<DeforumSettings> {
        #region PROPERTIES
        public override bool IsValid => true;
        public override string TypeName { get { return "Printborg.Types.DeforumSettings"; } }
        public override string TypeDescription { get { return "Settings for DeforumJob"; } }

        public override DeforumSettings Value { get => base.Value; set => base.Value = value; }
        #endregion


        public GH_DeforumSettings() {
            this.Value = null;
        }

        public GH_DeforumSettings(DeforumSettings settings) {
            this.Value = settings;
        }

        public GH_DeforumSettings(GH_DeforumSettings source) {
            this.Value = source.Value;
        }

        public override IGH_Goo Duplicate() {
            return new GH_DeforumSettings(this);
        }

        public override string ToString() {
            if (this.Value != null) {
                string output = "Printborg.Types.DeforumSettings";
                return output;
            }
            return "No settings specified!";
        }

        public override bool CastTo<Q>(ref Q target) {
            return false;
        }

        public override bool CastFrom(object source) {
            return false;
        }
    }
}
