using System.Collections.Generic;

namespace MegaDiscoverWeekly.Models
{
    public class CreateMegaDiscoverWeeklyRequest
    {
        public string UserId { get; set; }
        public List<string> DiscoverWeeklyUserIds { get; set; }
    }
}