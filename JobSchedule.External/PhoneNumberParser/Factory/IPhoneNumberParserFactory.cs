using System;
using System.Collections.Generic;
using System.Text;
using JobSchedule.External.PhoneNumberParser.Product;

namespace JobSchedule.External.PhoneNumberParser.Factory
{
    public interface IPhoneNumberParserFactory
    {
        public JobSchedule.External.PhoneNumberParser.Product.PhoneNumberParser ParsePhoneNumber(List<string> phoneNumber);
    }
}
