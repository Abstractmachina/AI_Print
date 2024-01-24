using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace GH_Dictionary
{

    public class DictSelectComponent : GH_Component
    {
        public DictSelectComponent()
          : base("Dict Select", "DictSelect",
              "Select value from key.",
              "Sets", "Dict")
        {
        }

        public override Guid ComponentGuid =>
            new Guid("09e21248-17b8-430a-a8f0-1f150592ab41");

        //protected override System.Drawing.Bitmap Icon =>
        //    Properties.Resources.GHDict_Read;
        protected override System.Drawing.Bitmap Icon => null;

        public override GH_Exposure Exposure =>
            GH_Exposure.primary;


        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddParameter(new GH_DictParam(), "Dict", "D", "Dictionary", GH_ParamAccess.item);
            pManager.AddTextParameter("Key", "K", "Key to search.", GH_ParamAccess.item);
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("Key", "K", "Key.", GH_ParamAccess.list);
            pManager.AddGenericParameter("Value", "V", "Value.", GH_ParamAccess.list);
            pManager.AddBooleanParameter("Found", "F", "True if key was found.", GH_ParamAccess.item);
        }


        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var ghDict = new GH_Dict();
            if (!DA.GetData(0, ref ghDict)) return;
            var dict = ghDict.Value;
            if (dict == null) return;

            string key = null;
            if (!DA.GetData(1, ref key)) return;
            if (string.IsNullOrWhiteSpace(key)) return;

            Wildcard wildcard = new Wildcard(key, RegexOptions.IgnoreCase);
            var data = dict.Where(kvp => wildcard.IsMatch(kvp.Key));

            if (data.Count() > 0)
            {
                DA.SetDataList(0, data.Select(kvp => kvp.Key).ToList());
                DA.SetDataList(1, data.Select(kvp => kvp.Value).ToList());
                DA.SetData(2, new GH_Boolean(true));
            }
            else
            {
                DA.SetData(2, new GH_Boolean(false));
            }
        }

    }
}