﻿using Drawer.Domain.Models.Inventory;

namespace Drawer.Web.Pages.Layout.Models
{
    public class LayoutModel
    {
        public long LocationId { get; set; }

        public List<LayoutItem> ItemList { get; set; } = new List<LayoutItem>();
    }
}
