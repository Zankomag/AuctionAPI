using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionAPI.Core.Entities {
    public class Seller : Entity<int> {
        public int UserId { get; set; }
        
        public User User { get; set; }
        public List<AuctionItem> AuctionItems { get; set; }
    }
}
