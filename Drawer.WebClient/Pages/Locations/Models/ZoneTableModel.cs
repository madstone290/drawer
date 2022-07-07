namespace Drawer.WebClient.Pages.Locations.Models
{
    public class ZoneTableModel
    {
        public long Id { get; set; }
        public long WorkPlaceId { get; set; }
        public string WorkPlaceName { get; set; } = string.Empty;
        public string WorkPlaceNote { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Note { get; set; } = string.Empty;
    }
}
