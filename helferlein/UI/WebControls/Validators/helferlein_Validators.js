/*
helferlein.com ( http://www.helferlein.com )
Michael Tobisch

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
function CheckBoxValidatorEvaluateIsValid(val) {
   var control = document.getElementById(val.controltovalidate);
   return control.checked == Boolean(val.mustBeChecked.toLowerCase() == "true");
}

function CheckBoxListValidatorEvaluateIsValid(val) {
   var control = document.getElementById(val.controltovalidate);

   var selectedItemCount = 0;
   var liIndex = 0;
   var currentListItem = document.getElementById(control.id + '_' + liIndex.toString());
   while (currentListItem != null) {
      if (currentListItem.checked) selectedItemCount++;
      liIndex++;
      currentListItem = document.getElementById(control.id + '_' + liIndex.toString());
   }

   return selectedItemCount >= parseInt(val.minimumNumberOfSelectedCheckBoxes);
}

function ClientValidateExpression(val) {
   var control = document.getElementById(val.controltovalidate);
   var validationExpression = new RegExp(val.validationExpression);
   var result = true;

   if (Boolean(val.required.toLowerCase() == "true")) {
      if (control.value.length == 0) {
         result = false;
         val.errormessage = val.requiredErrorMessage;
      }
      else {
         result = validationExpression.test(control.value);
         val.errormessage = val.invalidErrorMessage;
      }
   }
   else {
      if (control.value.length > 0)
         result = validationExpression.test(control.value);
      val.errormessage = val.invalidErrorMessage;
   }

   return result;
}

function ClientValidateDate(val) {
   var control = document.getElementById(val.controltovalidate);
   var result = true;

   if (Boolean(val.required.toLowerCase() == "true")) {
      if (control.value.length == 0) {
         result = false;
         val.errormessage = val.requiredErrorMessage;
      }
      else {
         result = CheckDate(control, val.dateSeparator, val.shortDatePattern.split(val.dateSeparator), val.minDate, val.maxDate);
         val.errormessage = val.invalidErrorMessage;
      }
   }
   else {
      if (control.value.length > 0) {
         result = CheckDate(control, val.dateSeparator, val.shortDatePattern.split(val.dateSeparator), val.minDate, val.maxDate);
         val.errormessage = val.invalidErrorMessage;
      }
   }
   return result;
}

function CheckDate(control, dateSeparator, arrPattern, minDate, maxDate) {
   var arrDate = control.value.split(dateSeparator);
   var arrMinDate = minDate.split(dateSeparator);
   var arrMaxDate = maxDate.split(dateSeparator);
   var day, month, year;
   var minDay, minMonth, minYear;
   var maxDay, maxMonth, maxYear;

   var result = true;
   var scratch, minScratch, maxScratch;

   for (i = 0; i < 3; i++) {
      switch (arrPattern[i]) {
         case "d":
         case "dd":
            day = arrDate[i];
            minDay = arrMinDate[i];
            maxDay = arrMaxDate[i];
            break;
         case "M":
         case "MM":
            month = arrDate[i];
            minMonth = arrMinDate[i];
            maxMonth = arrMaxDate[i];
            break;
         case "yy":
         case "yyyy":
            year = arrDate[i];
            minYear = arrMinDate[i];
            maxYear = arrMaxDate[i];
            break;
         default:
            result = false;
            break;
      }
   }

   if (result) {
      if (isNaN(day) || isNaN(month) || isNaN(year)) {
         result = false;
      }
   }

   if (result) {
      if (parseInt(year) < 100) {
         if (parseInt(year) > 30) {
            now = new Date();
            if (now.getFullYear() < 2030) {
               year = "19" + year;
            }
            else {
               year = "20" + year;
            }
         }
         else {
            year = "20" + year;
         }
      }

      while (month.charAt(0) == "0") {
         month = month.substring(1, month.length);
      }
      while (day.charAt(0) == "0") {
         day = day.substring(1, day.length);
      }

      scratch = new Date(parseInt(year), parseInt(month) - 1, parseInt(day));
      // Well, if you enter 12/32/2007, the date will be 01/01/2008, and that's valid...
      // No, should not be! Therefore compare to the original values...
      if ((scratch.getDate() != day) || ((scratch.getMonth() + 1) != month) || (scratch.getFullYear() != year)) {
         result = false;
      }
   }

   if (result) {
      // Last check: Date must be between minDate and maxDate
      minScratch = new Date(parseInt(minYear), parseInt(minMonth) - 1, parseInt(minDay));
      maxScratch = new Date(parseInt(maxYear), parseInt(maxMonth) - 1, parseInt(maxDay));

      if ((scratch < minScratch) || (scratch > maxScratch))
         result = false;
   }
   return result;
}

function ClientValidateNumber(val) {
   var control = document.getElementById(val.controltovalidate);
   var result = true;

   var specialChars = "\.\+\*\{\}\[\]\-\$\|\\";
   var r1, r2;

   if (specialChars.indexOf(val.groupSeparator) > -1)
      r1 = new RegExp("\\" + val.groupSeparator, "g");
   else
      r1 = new RegExp(val.groupSeparator, "g");

   if (specialChars.indexOf(val.decimalSeparator) > -1)
      r2 = new RegExp("\\" + val.decimalSeparator);
   else
      r2 = new RegExp(val.decimalSeparator);

   // Don't change the value in the input field, so use a copy...
   var nValue = control.value;

   nValue = nValue.replace(r1, "");
   nValue = nValue.replace(r2, ".");

   if (isNaN(nValue)) {
      result = false;
      val.errormessage = val.invalidErrorMessage;
   }
   else {
      if (Boolean(val.required.toLowerCase() == "true")) {
         if (nValue.length == 0) {
            result = false;
            val.errormessage = val.requiredErrorMessage;
         }
         else {
            result = CheckNumber(nValue, val.minvalue, val.maxvalue, val.numbertype);
            val.errormessage = val.invalidErrorMessage;
         }
      }
      else {
         if (nValue.length == 0) {
            result = true;
         }
         else {
            result = CheckNumber(nValue, val.minvalue, val.maxvalue, val.numbertype);
            val.errormessage = val.invalidErrorMessage;
         }
      }
   }
   return result;
}

function CheckNumber(value, minvalue, maxvalue, numbertype) {
   var number;
   var min;
   var max;
   var result;

   if (numbertype == "Integer") {
      if (minvalue == "")
         min = -2147483648;
      else
         min = parseInt(minvalue);
      if (maxvalue == "")
         max = 2147483648;
      else
         max = parseInt(maxvalue);

      number = parseInt(value);

      result = ((number >= min) && (number <= max) && (value == number.toString()));
   }
   else {
      if (minvalue == "")
         min = -1.79769313486232e308;
      else
         min = parseFloat(minvalue);
      if (maxvalue == "")
         max = 1.79769313486232e308;
      else
         max = parseFloat(maxvalue);

      number = parseFloat(value);

      result = ((number >= min) && (number <= max));
   }
   return result;
}
