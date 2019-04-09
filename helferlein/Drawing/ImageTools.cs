/*
dnnWerk.at ( https://www.dnnWerk.at )
Michael Tobisch, 2009-2019

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated 
documentation files (the "Software"), to deal in the Software without restriction, including without limitation 
the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and 
to permit persons to whom the Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions 
of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
DEALINGS IN THE SOFTWARE.
*/

using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace helferlein.Drawing
{
   public class ImageTools
   {
      public static MemoryStream ResizeFromStream(Stream inputStream, int maxWidth, int maxHeight)
      {
         int imageWidth = maxWidth;
         int imageHeight = maxHeight;

         Image image = Image.FromStream(inputStream);
         ImageFormat imageFormat = image.RawFormat;

         int originalWidth = image.Width;
         int originalHeight = image.Height;

         bool resizeByWidth = false;
         bool resizeByHeight = false;

         if ((originalWidth > maxWidth) || (originalHeight > maxHeight))
         {
            double widthFactor = originalWidth / maxWidth;
            double heightFactor = originalHeight / maxHeight;

            if (widthFactor >= heightFactor)
               resizeByWidth = true;
            else
               resizeByHeight = true;

            if (resizeByWidth)
            {
               imageWidth = maxWidth;
               imageHeight = Convert.ToInt32(originalHeight / widthFactor);
            }

            if (resizeByHeight)
            {
               imageWidth = Convert.ToInt32(originalWidth / heightFactor);
               imageHeight = maxHeight;
            }
         }
         else
         {
            imageWidth = originalWidth;
            imageHeight = originalHeight;
         }

         Bitmap resizedImage = new Bitmap(image, imageWidth, imageHeight);
         MemoryStream result = new MemoryStream();
         resizedImage.Save(result, imageFormat);
         return result;
      }
   }
}
