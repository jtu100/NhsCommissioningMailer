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
	/// Partial class extension to RootItemIdType
	/// </summary>
	public partial class RootItemIdType
	{
		/// <summary>
		/// Converts this instance to an ItemIdType
		/// </summary>
		/// <returns>ItemIdType containing data from this instance</returns>
		/// 
		public ItemIdType ToItemId()
		{
			return new ItemIdType(this);
		}
	}
}
