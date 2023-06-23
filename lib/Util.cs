using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AI_Print {
	internal static class Util {

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
			//this image is a single pixel (black)
			byte[] bytes = Convert.FromBase64String(input);

			Image image;

			var mstream = new MemoryStream(bytes);
			image = Image.FromStream(mstream);
			//image.Save(@"C:\Users\taole\source\repos\AI_Print\user_sketch\output\test3.jpg");
			return (Image)image.Clone();


			//return image;
			//mstream.Write(bytes, 0, bytes.Length);
			////mStream.Write(pData, 0, Convert.ToInt32(pData.Length));
			//image = Image.FromStream(mstream);

			//var bm = new Bitmap(mstream, false);
			//return bm;



			//using (MemoryStream ms = new MemoryStream(bytes)) {

			//	image = Image.FromStream(ms);
			//	//var newImg = new Bitmap(image);

			//	image.Save(@"C:\Users\taole\source\repos\AI_Print\user_sketch\output\test2.jpg");

			//	//return newImg;
			//}

			//return image;
		}
	}
}
