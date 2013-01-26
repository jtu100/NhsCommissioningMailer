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
	/// Extension of the TwoOperandExpressionType (Listing 14-8)
	/// </summary>
	public partial class TwoOperandExpressionType
	{
		/// <summary>
		/// Initialization method for constant value comparison
		/// </summary>
		/// <param name="path">Property path to check</param>
		/// <param name="constantValue">Constant value to check against</param>
		/// 
		protected virtual void Initialize(BasePathToElementType path, string constantValue)
		{
			this.FieldURIOrConstant = new FieldURIOrConstantType();
			ConstantValueType constantWrapper = new ConstantValueType();
			constantWrapper.Value = constantValue;
			this.FieldURIOrConstant.Item = constantWrapper;

			this.Item = path;
		}

		/// <summary>
		/// Initialization method for property comparison
		/// </summary>
		/// <param name="pathA">First property path to check</param>
		/// <param name="pathB">Property path to check it against</param>
		/// 
		protected virtual void Initialize(BasePathToElementType pathA, BasePathToElementType pathB)
		{
			this.FieldURIOrConstant = new FieldURIOrConstantType();
			this.FieldURIOrConstant.Item = pathB;
			this.itemField = pathA;
		}

		/// <summary>
		/// Factory method for creating constant comparisons
		/// </summary>
		/// <typeparam name="T">Type of comparison.  Must be a TwoOperandExpressionType descendant</typeparam>
		/// <param name="path">Property path to check</param>
		/// <param name="constantValue">Constant value to check against</param>
		/// <returns>Newly constructed comparison of type T</returns>
		/// 
		public static T CreateConstantComparison<T>(BasePathToElementType path, string constantValue) 
			where T: TwoOperandExpressionType, new()
		{
			T result = new T();
			result.Initialize(path, constantValue);
			return result;
		}

		/// <summary>
		/// Factory method for creating comparisons of two properties
		/// </summary>
		/// <typeparam name="T">Type of comparison.  Must be a TwoOperandExpressionType descendant</typeparam>
		/// <param name="pathA">First property path</param>
		/// <param name="pathB">Second property path</param>
		/// <returns>Newly constructed comparison of type T</returns>
		/// 
		public static T CreateConstantComparison<T>(BasePathToElementType pathA, BasePathToElementType pathB)
			where T : TwoOperandExpressionType, new()
		{
			T result = new T();
			result.Initialize(pathA, pathB);
			return result;
		}
	}
}
