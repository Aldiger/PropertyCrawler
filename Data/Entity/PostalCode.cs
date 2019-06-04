﻿using System.Collections.Generic;

namespace Data
{
    public class PostalCode : Base
    {

        public string Code { get; set; }
        public string OpCode { get; set; }

        public ICollection<Url> Urls { get; set; }
    }
}
