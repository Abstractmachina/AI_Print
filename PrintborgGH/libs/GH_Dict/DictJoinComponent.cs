using System;
using System.Collections.Generic;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Data;
using Grasshopper.Kernel.Types;

namespace GH_Dictionary
{

    public class DictJoinComponent : GH_Component
    {
        public DictJoinComponent()
          : base("Dict Join", "DictJoin",
              "Join two dictionaries.",
              "Sets", "Dict")
        {
        }

        public override Guid ComponentGuid =>
            new Guid("10d6a731-de07-480c-a78a-50f05b99c845");

        //protected override System.Drawing.Bitmap Icon =>
        //    Properties.Resources.GHDict_Join;
        protected override System.Drawing.Bitmap Icon => null;

        public override GH_Exposure Exposure =>
            GH_Exposure.primary;


        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddParameter(new GH_DictParam(),"DictA", "A", "First dictionary.", GH_ParamAccess.item);
            pManager.AddParameter(new GH_DictParam(), "DictB", "B", "Second dictionary.", GH_ParamAccess.item);
            pManager.AddBooleanParameter("Replace", "R", "Replace existing keys.", GH_ParamAccess.tree, false);
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddParameter(new GH_DictParam(), "Dict", "D", "Result dictionary.", GH_ParamAccess.item);
        }


        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var ghDictA = new GH_Dict();
            if (!DA.GetData(0, ref ghDictA)) return;
            var dictA = ghDictA.Value;
            if (dictA == null) return;

            var ghDictB = new GH_Dict();
            if (!DA.GetData(1, ref ghDictB)) return;
            var dictB = ghDictB.Value;
            if (dictB == null) return;

            DA.GetDataTree(2, out GH_Structure<GH_Boolean> tree);
            var ghReplace = tree.get_FirstItem(true);
            bool replace = ghReplace.Value;

            var dict = new Dictionary<string, IGH_Goo>(dictA);

            foreach (KeyValuePair<string, IGH_Goo> kvp in dictB)
            {
                if (dict.ContainsKey(kvp.Key))
                {
                    if (replace)
                    {
                        dict[kvp.Key] = kvp.Value;
                    }
                }
                else
                {
                    dict.Add(kvp.Key, kvp.Value);
                }
            }
            DA.SetData(0, new GH_Dict(dict));
        }

    }
}