using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AuctionAPI.Core.Entities;

namespace AuctionAPI.Core.Repositories {
    public interface IUnitOfWork {

        IAuctionItemCategoryRepository AuctionItemCategoryRepository { get; }

        Task<int> SaveAsync();
    }
}
