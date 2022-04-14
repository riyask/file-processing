using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using phoneNumberParserNamespace= JobSchedule.External.PhoneNumberParser.Product;

namespace JobSchedule.External.PhoneNumberParser.Factory
{
    public class GermanPhoneNumberParserFactory : IPhoneNumberParserFactory
    {
        public phoneNumberParserNamespace.PhoneNumberParser ParsePhoneNumber(List<string> phoneNumbers)
        {
            var phoneNumbersList = new phoneNumberParserNamespace.GermanPhoneNumberParser();
            phoneNumbersList.PhoneNumbers = new List<string>();
            foreach (var line in phoneNumbers)
            {
                if (!string.IsNullOrEmpty(line))
                {
                    string phoneNumber = line.Replace(" ", string.Empty).Trim();
                    if ((phoneNumber.Substring(0, 4) == "0049" && phoneNumber.Substring(3).Count() == 11) || (phoneNumber.Substring(0, 3) == "+49" && phoneNumber.Substring(2).Count() == 11))
                    {
                        phoneNumbersList.PhoneNumbers.Add(phoneNumber);
                    }
                }
            }
            return phoneNumbersList;

        }
    }
}
