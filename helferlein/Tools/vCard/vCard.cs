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
using System.Text;

namespace helferlein.Tools.vCard
{
   class vCard
   {
      public string FirstName { get; set; }
      public string LastName { get; set; }
      public string Organization { get; set; }
      public string JobTitle { get; set; }
      public string StreetAddress { get; set; }
      public string Zip { get; set; }
      public string City { get; set; }
      public string CountryName { get; set; }
      public string Phone { get; set; }
      public string Mobile { get; set; }
      public string Email { get; set; }
      public string HomePage { get; set; }
      public byte[] Image { get; set; }

      public override string ToString()
      {
         StringBuilder builder = new StringBuilder();
         builder.AppendLine("BEGIN:VCARD");
         builder.AppendLine("VERSION:2.1");

         // Name
         builder.Append("N:");
         builder.Append(string.IsNullOrEmpty(LastName) ? string.Empty : LastName);
         builder.Append(";");
         builder.AppendLine(string.IsNullOrEmpty(FirstName) ? string.Empty : FirstName);
         // Full name
         builder.Append("FN:");
         builder.Append(string.IsNullOrEmpty(FirstName) ? string.Empty : FirstName);
         builder.Append(" ");
         builder.AppendLine(string.IsNullOrEmpty(LastName) ? string.Empty : LastName);

         // Address
         builder.Append("ADR;HOME;PREF:;;");
         builder.Append(string.IsNullOrEmpty(StreetAddress) ? string.Empty : StreetAddress);
         builder.Append(";");
         builder.Append(string.IsNullOrEmpty(City) ? string.Empty : City);
         builder.Append(";;");
         builder.Append(string.IsNullOrEmpty(Zip) ? string.Empty : Zip);
         builder.Append(";");
         builder.AppendLine(string.IsNullOrEmpty(CountryName) ? string.Empty : CountryName);

         // Other data
         builder.Append("ORG:");
         builder.AppendLine(string.IsNullOrEmpty(Organization) ? string.Empty : Organization);
         builder.Append("TITLE:");
         builder.AppendLine(string.IsNullOrEmpty(JobTitle) ? string.Empty : JobTitle);
         builder.Append("TEL;HOME;VOICE:");
         builder.AppendLine(string.IsNullOrEmpty(Phone) ? string.Empty : Phone);
         builder.Append("TEL;CELL;VOICE:");
         builder.AppendLine(string.IsNullOrEmpty(Mobile) ? string.Empty : Mobile);
         builder.Append("URL:");
         builder.AppendLine(string.IsNullOrEmpty(HomePage) ? string.Empty : HomePage);
         builder.Append("EMAIL;PREF;INTERNET:");
         builder.AppendLine(string.IsNullOrEmpty(Email) ? string.Empty : Email);

         // Image
         if (Image != null)
         {
            builder.Append("PHOTO;ENCODING=BASE64;TYPE=JPEG:");
            builder.AppendLine(Convert.ToBase64String(Image));
            builder.AppendLine(string.Empty);
         }

         builder.AppendLine("END:VCARD");

         return builder.ToString();
      }
   }
}
