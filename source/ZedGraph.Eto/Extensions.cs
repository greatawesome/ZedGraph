using Eto.Drawing;

namespace ZedGraph
{
    public static class Extensions
   {
        public static Bitmap ToEto(this System.Drawing.Bitmap bmp)
        {
			using (System.IO.MemoryStream memory = new System.IO.MemoryStream())
			{
				bmp.Save(memory, System.Drawing.Imaging.ImageFormat.Png);
				memory.Position = 0;
				return new Bitmap(memory);
			}
		}
	}
}
