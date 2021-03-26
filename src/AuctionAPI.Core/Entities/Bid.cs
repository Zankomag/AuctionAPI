using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionAPI.Core.Entities {
    public class Bid : Entity<int> {
        public int BidderId { get; set; }
        public int AuctionItemId { get; set; }
        public decimal Price { get; set; }


        public Bidder Bidder { get; set; }
        public AuctionItem AuctionItem { get; set; }
    }
}
