namespace Drawer.Web.Pages.Locations.Models
{
    public class ZoneTableModel
    {
        public long Id { get; set; }
        public string? Name { get; set; }
        public string? Note { get; set; }
        public long WorkplaceId { get; set; }
        public string? WorkplaceName { get; set; } 
    }
}
