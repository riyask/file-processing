using System;
using System.Collections.Generic;
using System.Text;

namespace JobSchedule.External.PhoneNumberParser.Product
{
    public abstract class PhoneNumberParser
    {
        public abstract string ParserType { get; set; }
        public abstract List<string> PhoneNumbers { get; set; }

    }
}
