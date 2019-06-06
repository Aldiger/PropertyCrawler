namespace RightMove.Data
{
    public class Url : Base
    {

        public string PropertyUrl { get; set; }
        public int Type { get; set; }

        public int PostalCodeId { get; set; }
        public int PortalId { get; set; }
        
        public virtual PostalCode PostalCode { get; set; }
        public virtual Portal Portal { get; set; }
    }
}
