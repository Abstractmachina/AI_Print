using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Printborg {
	public static class Util {

		public static bool SaveImageFromBase64(string base64, string filepath) {
			var img = FromBase64String(base64);
			try {
				img.Save(filepath);
				return true;
			}
			catch (Exception e) {
				throw e;
			}
		}

		public static Image FromBase64String(string input) {
			//data:image/gif;base64,
			byte[] bytes = Convert.FromBase64String(input);

			Image image;

			var mstream = new MemoryStream(bytes);
			image = Image.FromStream(mstream, true);
			//image.Save(@"C:\Users\taole\source\repos\Printborg\user_sketch\output\test3.jpg");
			return (Image)image.Clone();
		}


		
	}
}
