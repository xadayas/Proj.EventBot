using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;


namespace EventBot.Entities.Service
{
    public class ImageHelpers
    {
        public static System.Drawing.Image FixedSize(Image image, int width, int height, bool needToFill)
        {
            var sourceWidth = image.Width;
            var sourceHeight = image.Height;
            var sourceX = 0;
            var sourceY = 0;
            double destX = 0;
            double destY = 0;

            double nScale = 0;
            double nScaleW = 0;
            double nScaleH = 0;

            nScaleW = ((double)width / (double)sourceWidth);
            nScaleH = ((double)height / (double)sourceHeight);
            if (!needToFill)
            {
                nScale = Math.Min(nScaleH, nScaleW);
            }
            else
            {
                if(sourceHeight < height || sourceWidth < width)
                    nScale = Math.Min(nScaleH, nScaleW);
                else
                    nScale = Math.Max(nScaleH, nScaleW);
                destY = (height - sourceHeight * nScale) / 2;
                destX = (width - sourceWidth * nScale) / 2;
            }

            if (nScale > 1)
                nScale = 1;

            var destWidth = (int)Math.Round(sourceWidth * nScale);
            var destHeight = (int)Math.Round(sourceHeight * nScale);


            System.Drawing.Bitmap bmPhoto = null;
            try
            {
                bmPhoto = new System.Drawing.Bitmap(destWidth + (int)Math.Round(2 * destX), destHeight + (int)Math.Round(2 * destY));
            }
            catch (Exception ex)
            {
                throw new ApplicationException(string.Format("destWidth:{0}, destX:{1}, destHeight:{2}, desxtY:{3}, Width:{4}, Height:{5}",
                    destWidth, destX, destHeight, destY, width, height), ex);
            }
            using (System.Drawing.Graphics grPhoto = System.Drawing.Graphics.FromImage(bmPhoto))
            {
                grPhoto.InterpolationMode = InterpolationMode.HighQualityBicubic;
                grPhoto.CompositingQuality = CompositingQuality.HighQuality;
                grPhoto.SmoothingMode = SmoothingMode.HighQuality;

                Rectangle to = new System.Drawing.Rectangle((int)Math.Round(destX), (int)Math.Round(destY), destWidth, destHeight);
                Rectangle from = new System.Drawing.Rectangle(sourceX, sourceY, sourceWidth, sourceHeight);
                //Console.WriteLine("From: " + from.ToString());
                //Console.WriteLine("To: " + to.ToString());
                grPhoto.DrawImage(image, to, from, System.Drawing.GraphicsUnit.Pixel);

                return bmPhoto;
            }
        }
        public static ImageCodecInfo GetEncoderInfo(String mimeType)
        {
            int j;
            ImageCodecInfo[] encoders;
            encoders = ImageCodecInfo.GetImageEncoders();
            for (j = 0; j < encoders.Length; ++j)
            {
                if (encoders[j].MimeType == mimeType)
                    return encoders[j];
            }
            return null;
        }

    }
}
