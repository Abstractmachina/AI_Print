using Grasshopper.Kernel.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Printborg.Types;

namespace PrintborgGH.GH_Types
{
    public class GH_Payload : GH_Goo<Auto1111Payload>
    {
        public override bool IsValid => true;

        public override string TypeName { get { return "Image"; } }

        public override string TypeDescription { get { return "Bitmap Image"; } }


        #region Constructors
        public GH_Payload() { }

        public GH_Payload(Auto1111Payload image)
        {
            this.Value = image;
        }

        public GH_Payload(GH_Payload source)
        {
            this.Value = source.Value;
        }
        #endregion


        public override Auto1111Payload Value { get => base.Value; set => base.Value = value; }

        public override IGH_Goo Duplicate()
        {
            return new GH_Payload(this);
        }

        public override string ToString()
        {
            return Value.ToString();
        }

        public override bool CastTo<Q>(ref Q target)
        {
            return base.CastTo(ref target);
        }

        public override bool CastFrom(object source)
        {
            return base.CastFrom(source);
        }
    }
}
