using Grasshopper.Kernel;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace GH_Dictionary
{

    public class GH_DictParam : GH_Param<GH_Dict>, IGH_Param
    {
        public GH_DictParam() : base("Dictionary", "Dict",
              "A dictionary.",
              "Sets", "Dict", GH_ParamAccess.item)
        {
        }

        public override Guid ComponentGuid => new Guid("2bfe9c54-d122-4a46-ab77-a42fb103f200");

        //protected override Bitmap Icon =>
        //    Properties.Resources.GHDict_Param;
        protected override System.Drawing.Bitmap Icon => null;
        public override GH_Exposure Exposure =>
            GH_Exposure.septenary | GH_Exposure.obscure;



        protected override string VolatileDataDescription()
        {
            var _MAX_LENGTH = 10;
            var sb = new StringBuilder();
            sb.AppendLine(base.VolatileDataDescription());
            var dataEnum = VolatileData.AllData(true).GetEnumerator();
            var data = new List<GH_Dict>();
            while (dataEnum.MoveNext())
                data.Add((GH_Dict)dataEnum.Current);
            if (data == null || data.Count == 0 || data.Count > 1) return sb.ToString();
            var gh_dict = data[0];
            if (gh_dict == null) return sb.ToString();
            var dict = gh_dict.Value;
            var lines = dict
                        .Take(Math.Min(_MAX_LENGTH, dict.Count))
                        .Select(kvp => $" {kvp.Key} : {kvp.Value}\n")
                        .Aggregate((a, b) => a + b);
            sb.Append(lines);
            if (dict.Count > _MAX_LENGTH)
                sb.Append(" …");
            return sb.ToString();
        }
    }
}


