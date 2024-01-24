using System;
using Grasshopper.Kernel;

namespace GH_Dictionary
{

    public class DictReadComponent : GH_Component
    {
        public DictReadComponent()
          : base("Dict Read", "DictRead",
              "Read key-value pairs.",
              "Sets", "Dict")
        {
        }

        public override Guid ComponentGuid =>
            new Guid("35ec435a-c718-4fe3-a3ce-4a302750e67b");

        //protected override System.Drawing.Bitmap Icon =>
        //    Properties.Resources.GHDict_KeyValue;
        protected override System.Drawing.Bitmap Icon => null;

        public override GH_Exposure Exposure =>
            GH_Exposure.primary;


        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddParameter(new GH_DictParam(), "Dict", "D", "Dictionary", GH_ParamAccess.item);
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("Key", "K", "List of keys.", GH_ParamAccess.list);
            pManager.AddGenericParameter("Value", "V", "List of values.", GH_ParamAccess.list);
        }


        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var ghDict = new GH_Dict();
            if (!DA.GetData(0, ref ghDict)) return;
            DA.SetDataList(0, ghDict.Value.Keys);
            DA.SetDataList(1, ghDict.Value.Values);
        }

    }
}