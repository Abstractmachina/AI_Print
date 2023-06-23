using AI_Print.Types;
using Grasshopper.Kernel.Types;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AI_Print.GH_Types {
    public class GH_TextPrompt : GH_Goo<TextPrompt> {
        public override bool IsValid => true;

        public override string TypeName { get { return "Text Prompt"; } }

        public override string TypeDescription { get { return "Represents a simple text prompt and an associated weight."; } }


        #region Constructors
        public GH_TextPrompt() { }

        public GH_TextPrompt(TextPrompt prompt) {
            this.Value = prompt;
        }

        public GH_TextPrompt(GH_TextPrompt source) {
            this.Value = source.Value;
        }
        #endregion


        public override TextPrompt Value { get => base.Value; set => base.Value = value; }

        public override IGH_Goo Duplicate() {
            return new GH_TextPrompt(this);
        }

        public override string ToString() {
            return Value.ToString();
        }

        public override bool CastTo<Q>(ref Q target) {
            return base.CastTo(ref target);
        }

        public override bool CastFrom(object source) {
            return base.CastFrom(source);
        }
    }
}
