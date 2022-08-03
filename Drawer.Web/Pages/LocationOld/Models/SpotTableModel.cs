namespace Drawer.Web.Pages.LocationOld.Models
{
    public class SpotTableModel
    {
        public long Id { get; set; }
        public string? Name { get; set; } 
        public string? Note { get; set; } 
        public long WorkplaceId { get; set; }
        public string? WorkplaceName { get; set; } 
        public long ZoneId { get; set; }
        public string? ZoneName { get; set; } 
    }
}
