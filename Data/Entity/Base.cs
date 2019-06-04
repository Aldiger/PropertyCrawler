using System;

namespace Data
{
    public abstract class Base
    {
        public int Id { get; set; }

        public DateTime DateAdded { get; set; }
        public DateTime DateModified { get; set; }
        public bool Active { get; set; }
    }
}
