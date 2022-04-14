using System;
using System.Collections.Generic;
using System.Text;
using JobSchedule.Shared.Common;

namespace JobSchedule.External.PhoneNumberParser.Product
{
    public class GermanPhoneNumberParser : PhoneNumberParser
    {
        public GermanPhoneNumberParser()
        {
            ParserType = PhoneNumberParserType.German.ToString();
        }
        public override string ParserType { get; set; }
        public override List<string> PhoneNumbers { get; set; }

    }
}
