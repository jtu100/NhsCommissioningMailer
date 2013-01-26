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
	/// Partial class extension for the SetItemFieldType proxy class (Listing 5-34)
	/// </summary>
	public partial class SetItemFieldType
	{
		/// <summary>
		/// Constructor needed for xml serialization
		/// </summary>
		public SetItemFieldType() { }
		
		/// <summary>
		/// Constructor 
		/// </summary>
		/// <param name="propertyPath">PropertyPath for this single change</param>
		/// <param name="itemWithChange">The item along with the change</param>
		/// 
		public SetItemFieldType(BasePathToElementType propertyPath, ItemType itemWithChange)
		{
			this.Item = propertyPath;
			this.Item1 = itemWithChange;
		}
	}
}
