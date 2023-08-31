using Grasshopper.Kernel.Types;
using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Printborg.GH_Types {
    public class GH_Image : GH_Goo<Image> {
        public override bool IsValid {
            get { if (this.Value == null) return false; else return true; }
        
        }

        public override string TypeName { get { return "Image"; } }

        public override string TypeDescription { get { return "Bitmap Image"; } }


        #region Constructors
        public GH_Image() {
            this.Value = null;
        }

        public GH_Image(Image image) {
            this.Value = image;
        }

        public GH_Image(GH_Image source) {
            this.Value = source.Value;
        }
        public GH_Image(string base64String) {
            this.Value = Util.FromBase64String(base64String);
        }
        #endregion


        public override Image? Value { get => base.Value; set => base.Value = value; }

        public override IGH_Goo Duplicate() {
            return new GH_Image(this);
        }

        public override string ToString() {
            string output = $"img: {{{this.Value.Width}; {this.Value.Height}; {this.Value.RawFormat}}}";
            return output;
        }

        public override object ScriptVariable() {
            return this.Value;
        }

        public override bool CastTo<Q>(ref Q target) {

            //cast to string = base64
            if (typeof(Q).IsAssignableFrom(typeof(string))) {
                var rawString = Util.ToBase64String(this.Value);
                object ptr = rawString;
                target = (Q)ptr;
                return true;
            }

            //cast to mesh

            target = default(Q);
            return false;
        }

        public override bool CastFrom(object source) {

            if (source == null) return false;
            if (source.GetType() == typeof(Mesh)) {
                //convert mesh to image
            }
            if (source.GetType() == typeof (string)) { 
                // possibly a base64 encoded image 
                var img = Printborg.Util.FromBase64String((string)source);
                this.Value = img;
                return true;
            }
            return false;

        }


    }
}
