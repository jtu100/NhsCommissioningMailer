//  Inside Microsoft Exchange 2007 Web Services
//  ProxyHelpers
//
//  Copyright (c) 2007 David Sterling, Ben Spain, Mark Taylor, Huw Upshall, Michael Mainer.  
//  All rights reserved.
//

using System;
using System.Collections.Generic;
using System.Text;

namespace ProxyHelpers.EWS
{
    /// <summary>
    /// Partial class extension of FindItemParentType proxy class (Listing 5-27, Listing 15-16)
    /// </summary>
    public partial class FindItemParentType
    {
        /// <summary>
        /// Returns the normal (non-grouped) results from a FindItem query for this RootFolder
        /// </summary>
        /// <returns>Array of items</returns>
        /// 
        public ItemType[] GetNormalResults()
        {
            ArrayOfRealItemsType realItems = this.Item as ArrayOfRealItemsType;

            return realItems.Items;
        }

        /// <summary>
        /// Returns the grouped result from a FindItem query for this RootFolder
        /// </summary>
        /// <returns>Array of groups</returns>
        /// 
        public GroupedItemsType[] GetGroupedResults()
        {
            ArrayOfGroupedItemsType groupedItems = this.Item as ArrayOfGroupedItemsType;

            return groupedItems.Items;
        }
    }
}
