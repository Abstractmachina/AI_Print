using System;
using System.Collections.Generic;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;

namespace GH_Dictionary
{

    public class DictCreateComponent : GH_Component
    {
        public DictCreateComponent()
          : base("Create Dictionary", "CreateDictionary",
              "Create a new dictionary.",
              "Sets", "Dict")
        {
        }

        public override Guid ComponentGuid =>
            new Guid("9d1ad156-1e26-4d5d-8795-63029f6fb476");

        //protected override System.Drawing.Bitmap Icon => Properties.Resources.GHDict_Create;
        protected override System.Drawing.Bitmap Icon => null;

        public override GH_Exposure Exposure =>
            GH_Exposure.primary;


        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Key", "K", "List of keys.", GH_ParamAccess.list);
            pManager.AddGenericParameter("Value", "V", "List of values.", GH_ParamAccess.list);
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddParameter(new GH_DictParam(), "Dict", "D", "New dictionary", GH_ParamAccess.item);
        }


        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var keys = new List<string>();
            var values = new List<IGH_Goo>();

            if (!DA.GetDataList(0, keys)) return;
            if (!DA.GetDataList(1, values)) return;

            var count = keys.Count;

            if (values.Count != count)
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "You must supply an equal number of keys and values.");
                return;
            }

            var dict = new Dictionary<string, IGH_Goo>();
            for (int i = 0; i < count; i++)
            {
                if (!dict.ContainsKey(keys[i]))
                    dict.Add(keys[i], values[i]);
                else
                    AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, $"Key {keys[i]} at index {i} was skipped because it already existed.");
            }
            
            var ghDict = new GH_Dict(dict);
            DA.SetData(0, ghDict);
        }

    }
}
