using System.Collections.Generic;

namespace Data
{
    public class Portal : Base
    {
        public string Name { get; set; }
        public string Url { get; set; }

        public ICollection<Url> Urls { get; set; }
    }
}
