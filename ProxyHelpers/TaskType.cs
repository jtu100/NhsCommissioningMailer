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
	/// Partial class extension for TaskType (Listing 11-4)
	/// </summary>
	public partial class TaskType
	{
        /// <summary>
        /// Holds the correct MAPI property we should be getting
        /// </summary>
        public static PathToExtendedFieldType CorrectedDelegationStatePath;

        /// <summary>
        /// Static constructor for initializing our property path
        /// </summary>
        static TaskType()
        {
            CorrectedDelegationStatePath = new PathToExtendedFieldType();
            CorrectedDelegationStatePath.PropertySetId =
                  "{00062003-0000-0000-C000-000000000046}";
            CorrectedDelegationStatePath.PropertyId = 0x8113;
            CorrectedDelegationStatePath.PropertyType =
                  MapiPropertyTypeType.Integer;
        }

		/// <summary>
		/// Helper to get the corrected enum value and throw if not present
		/// </summary>
		/// <returns>Delegate state</returns>
		/// 
		private TaskDelegateStateType GetCorrectedDelegateState()
		{
			TaskDelegateStateType result;
			if (!TryGetCorrectedDelegateState(out result))
			{
				throw new Exception("Corrected delegate state not present");
			}
			return result;
		}

		/// <summary>
		/// Helper to check for the corrected delegate state presence
		/// </summary>
		private bool IsCorrectedDelegateStateSpecified
		{
			get
			{
				TaskDelegateStateType state;
				return TryGetCorrectedDelegateState(out state);
			}
		}

		/// <summary>
		/// A forgiving method to return the extended prop value if it exists
		/// </summary>
		/// <param name="state">OUT state</param>
		/// <returns>True if it exists, false if not</returns>
		/// 
		private bool TryGetCorrectedDelegateState(out TaskDelegateStateType state)
		{
			state = TaskDelegateStateType.NoMatch;

			if ((this.ExtendedProperty == null) || (this.ExtendedProperty.Length == 0))
			{
				return false;
			}
			foreach (ExtendedPropertyType prop in this.ExtendedProperty)
			{
				if ((prop.ExtendedFieldURI.PropertyId == CorrectedDelegationStatePath.PropertyId) &&
					(prop.ExtendedFieldURI.PropertySetId == CorrectedDelegationStatePath.PropertySetId) &&
					(prop.ExtendedFieldURI.PropertyType == CorrectedDelegationStatePath.PropertyType))
				{
					int intValue = Int32.Parse((string)prop.Item);
					state = (TaskDelegateStateType)intValue;
					return true;
				}
			}
			return false;
		}

		/// <summary>
		/// Adds the corrected delegate state property path to our shape for GetItem and FindItem.
		/// </summary>
		/// <param name="shape">Shape to add to</param>
		/// 
		public static void AddCorrectedDelegationStateToShape(ItemResponseShapeType shape)
		{
			BasePathToElementType[] additionalProps = shape.AdditionalProperties;
			int existingCount = (additionalProps == null) ? 0 : additionalProps.Length;

			List<BasePathToElementType> newAdditionalProps = new List<BasePathToElementType>(existingCount + 1);
			if (additionalProps != null)
			{
				newAdditionalProps.AddRange(additionalProps);
			}
			newAdditionalProps.Add(CorrectedDelegationStatePath);
			shape.AdditionalProperties = newAdditionalProps.ToArray();
		}
	}
}
