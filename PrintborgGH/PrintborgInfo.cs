using Grasshopper;
using Grasshopper.Kernel;
using System;
using System.Drawing;

namespace Printborg {
	public class PrintborgInfo : GH_AssemblyInfo {
		public override string Name => "Printborg";

		//Return a 24x24 pixel bitmap to represent this GHA library.
		public override Bitmap Icon => null;

		//Return a short string describing the purpose of this GHA library.
		public override string Description => "";

		public override Guid Id => new Guid("496019B9-39F4-415C-B1FC-057728064A8B");

		//Return a string identifying you or your company.
		public override string AuthorName => "";

		//Return a string representing your preferred contact details.
		public override string AuthorContact => "";
	}
}