using GH_Dictionary;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintborgGH.libs {
    public static class GH_Convert {
        //if (GH_Convert.ToGHCurve(goo, GH_Conversion.Both, ref castCurve)) {
        //ToGHImage(IGH_Goo goo, )

        public static Dictionary<K,V> ToDictionary<K,V>(GH_Dict input) {
            //try to convert gh_dict to Dictionary<string, string>
            var out_dict = new Dictionary<K,V>();
            foreach (KeyValuePair<string, IGH_Goo> entry in input.Value) {
                var key =  (K)Convert.ChangeType(entry.Key, typeof(K));
                var val = (V)Convert.ChangeType(entry.Value.ScriptVariable(), typeof(V));

                out_dict.Add(key, val);

            }
            return out_dict;
        }

    }
}
