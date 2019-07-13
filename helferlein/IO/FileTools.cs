/*
dnnWerk.at ( https://www.dnnWerk.at )
(C) Michael Tobisch 2009-2019

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
using System.IO;

namespace helferlein.IO
{
   public class FileTools
   {
      public static string File2String(string inputFileName)
      {
         FileStream fs;
         byte[] b;
         long bytesRead;
         string s;

         try
         {
            fs = new FileStream(inputFileName, FileMode.Open, FileAccess.Read);
            b = new byte[fs.Length];
            bytesRead = fs.Read(b, 0, (int)fs.Length);
            fs.Close();
            s = Convert.ToBase64String(b, 0, b.Length);
            return s;
         }
         catch (Exception)
         {
            throw;
         }
      }

      public static void String2File(string s, string outputFileName)
      {
         FileStream fs;
         byte[] b;

         try
         {
            b = Convert.FromBase64String(s);
            fs = new FileStream(outputFileName, FileMode.Create, FileAccess.Write);
            fs.Write(b, 0, b.Length);
            fs.Close();
         }
         catch (Exception)
         {
            throw;
         }
      }

      public static string Stream2String(Stream inputStream)
      {
         byte[] b;
         long bytesRead;
         string s;
         try 
	      {
            b = new byte[inputStream.Length];
            inputStream.Position = 0;
            bytesRead = inputStream.Read(b, 0, (int)inputStream.Length);
            s = Convert.ToBase64String(b, 0, b.Length);
            return s;
         }
	      catch (Exception)
         {
		      throw;
	      }
      }
   }
}
