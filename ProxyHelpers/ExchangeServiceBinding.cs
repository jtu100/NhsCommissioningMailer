//  Inside Microsoft Exchange 2007 Web Services
//  ProxyHelpers
//
//  Copyright (c) 2007 David Sterling, Ben Spain, Mark Taylor, Huw Upshall, Michael Mainer.  
//  All rights reserved.
//

using System;
using System.Collections.Generic;
using System.Text;
using System.Web.Services.Protocols;
using System.Globalization;
using System.Security.Principal;

namespace ProxyHelpers.EWS
{
    public partial class ExchangeServiceBinding
    {
        /// <summary>
        /// Sets the mailbox culture based on the passed culture info (Listing 2-10, Listing 2-12)
        /// </summary>
        /// <param name="info">CultureInfo to use</param>
        /// 
        public void SetMailboxCulture(CultureInfo info)
        {
            this.MailboxCulture = new language();
            this.MailboxCulture.CultureName = info.Name;
            
            // NOTE:  If we did not do the language class override in
            // output.cs, we would need to do the following instead of
            // the previous line.
            //
            //this.MailboxCulture.Text = new string[] { info.Name };
        }

        /// <summary>
        /// Returns an up to date Id and change key for the specified item 
        /// </summary>
        /// <param name="oldId">The Id that you have and wish to update</param>
        /// <returns>The shiny new, up to date id and changekey</returns>
        /// 
        public ItemIdType GetCurrentChangeKey(ItemIdType oldId)
        {
            // Create the request type itself and set the response shape.  All we 
            // need is the Id.
            //
            GetItemType getItemRequest = new GetItemType();
            getItemRequest.ItemShape = new ItemResponseShapeType();
            getItemRequest.ItemShape.BaseShape = DefaultShapeNamesType.IdOnly;

            // Set the single Id that we wish to look up
            //
            getItemRequest.ItemIds = new BaseItemIdType[] { oldId };

            // Make the actual web request
            //
            GetItemResponseType response = this.GetItem(getItemRequest);

            // Get the appropriate message.
            //
            ItemInfoResponseMessageType responseMessage =
                response.ResponseMessages.Items[0] as ItemInfoResponseMessageType;

            // If we succeeded, the response class will be success
            //
            if (responseMessage.ResponseClass == ResponseClassType.Success)
            {
                return responseMessage.Items.Items[0].ItemId;
            }
            else
            {
                throw new ArgumentException(
                     String.Format(
                         "Item not found in mailbox.  Error Code: {0}",
                         responseMessage.ResponseCode.ToString()),
                     "oldId");
            }
        }
        
        /// <summary>
		/// A new, improved ContactsView method (Listing 6-16)
		/// </summary>
		/// <param name="folderId">Folder to perform FindItem in</param>
		/// <param name="responseShape">ResponseShape for returned contacts</param>
		/// <param name="pathForRestriction">The property path to compare against</param>
		/// <param name="lowerBounds">lower bounds string (inclusive)</param>
		/// <param name="upperBounds">upper bounds string (exclusive)</param>
		/// <param name="offset">For indexed paging, the offset into the result set to start at</param>
		/// <param name="maxEntries">Max entries to return for each page.  Zero for unbounded</param>
		/// <returns>FindItemResponseMessageType</returns>
		/// 
		public FindItemResponseMessageType SuperContactsView(
										BaseFolderIdType folderId,
										ItemResponseShapeType responseShape,
										BasePathToElementType pathForRestriction,
										string lowerBounds,
										string upperBounds,
										int offset,
										int maxEntries)
		{
			FindItemType request = new FindItemType();
			request.ItemShape = responseShape;
			// If they set a maxEntries > 0, use indexed paging just to limit the results.
			//
			if (maxEntries > 0)
			{
				IndexedPageViewType paging = new IndexedPageViewType();
				paging.BasePoint = IndexBasePointType.Beginning;
				paging.Offset = offset;
				paging.MaxEntriesReturned = maxEntries;
				paging.MaxEntriesReturnedSpecified = true;
				request.Item = paging;
			}
			request.ParentFolderIds = new BaseFolderIdType[] { folderId };
			request.Traversal = ItemQueryTraversalType.Shallow;


			// Build up our restriction
			//
			AndType and = new AndType();
			IsGreaterThanOrEqualToType lowerBoundsFilter = new IsGreaterThanOrEqualToType();
			lowerBoundsFilter.Item = pathForRestriction;
			lowerBoundsFilter.FieldURIOrConstant = new FieldURIOrConstantType();
			ConstantValueType lowerBoundsValue = new ConstantValueType();
			lowerBoundsValue.Value = lowerBounds;
			lowerBoundsFilter.FieldURIOrConstant.Item = lowerBoundsValue;

			IsLessThanType upperBoundsFilter = new IsLessThanType();
			upperBoundsFilter.Item = pathForRestriction;
			upperBoundsFilter.FieldURIOrConstant = new FieldURIOrConstantType();
			ConstantValueType upperBoundsValue = new ConstantValueType();
			upperBoundsValue.Value = upperBounds;
			upperBoundsFilter.FieldURIOrConstant.Item = upperBoundsValue;

			and.Items = new SearchExpressionType[] { lowerBoundsFilter, upperBoundsFilter };
			request.Restriction = new RestrictionType();
			request.Restriction.Item = and;

			// Make the request
			//
			FindItemResponseType response = this.FindItem(request);
			return response.ResponseMessages.Items[0] as FindItemResponseMessageType;
		}

		/// <summary>
		/// Gets a single item and uses the new EWSException class to report errors (Listing 18-19)
		/// </summary>
		/// <param name="responseShape">response shape to use</param>
		/// <param name="itemId">Id of item to get</param>
		/// <returns>Retrieved ItemType instance</returns>
		/// 
		public ItemType GetSingleItem(ItemResponseShapeType responseShape, ItemIdType itemId)
		{
			GetItemType getItemRequest = new GetItemType();
			getItemRequest.ItemShape = responseShape;
			getItemRequest.ItemIds = new BaseItemIdType[] { itemId };
			GetItemResponseType response = null;
			try
			{
				response = this.GetItem(getItemRequest);
			}
			catch (SoapException soapException)
			{
				throw new EWSException(soapException);
			}

			// if the call was an error, throw.
			//
			ItemInfoResponseMessageType itemResponseMessage = response.ResponseMessages.Items[0] as ItemInfoResponseMessageType;
			EWSException.ThrowIfError(itemResponseMessage);

			// return our single item
			//
			return itemResponseMessage.Items.Items[0];
		}

		/// <summary>
		/// Helper method for performing batched GetItem calls (Listing 18-20)
		/// </summary>
		/// <param name="responseShape">ResponseShape to return</param>
		/// <param name="itemIds">params array of itemIds</param>
		/// <returns>ItemInfoResponseMessageType array</returns>
		/// 
		public ItemInfoResponseMessageType[] GetBatchedItems(
							ItemResponseShapeType responseShape, 
							params ItemIdType[] itemIds)
		{
			GetItemType getItemRequest = new GetItemType();
			getItemRequest.ItemShape = responseShape;
			getItemRequest.ItemIds = itemIds;
			GetItemResponseType response = null;
			try
			{
				response = this.GetItem(getItemRequest);
			}
			catch (SoapException soapException)
			{
				// if we encounter a soap exception, throw an EWS exception instead
				//
				throw new EWSException(soapException);
			}

			ItemInfoResponseMessageType[] results = new ItemInfoResponseMessageType[response.ResponseMessages.Items.Length];
			int index = 0;
			foreach (ItemInfoResponseMessageType responseMessage in response.ResponseMessages.Items)
			{
				results[index++] = responseMessage;
			}
			return results;
		}

        /// <summary>
        /// Creates the batch of items passed in
        /// </summary>
        /// <param name="itemsToCreate">Items to create</param>
        /// <returns>Array of created ids</returns>
        /// 
        public ItemIdType[] CreateBatchedItems(
            ItemType[] itemsToCreate)
        {
            CreateItemType request = new CreateItemType();
            request.MessageDisposition = MessageDispositionType.SaveOnly;
            request.MessageDispositionSpecified = true;

            request.Items = new NonEmptyArrayOfAllItemsType();
            request.Items.Items = itemsToCreate;

            CreateItemResponseType response = this.CreateItem(
                    request);

            ItemIdType[] result = new ItemIdType[itemsToCreate.Length];
            int index = 0;
            foreach (ItemInfoResponseMessageType responseMessage in
                response.ResponseMessages.Items)
            {
                if (responseMessage.ResponseCode == ResponseCodeType.NoError)
                {
                    result[index++] = responseMessage.Items.Items[0].ItemId;
                }
                else
                {
                    result[index++] = null;
                }
            }
            return result;
        }

        /// <summary>
        /// Copy items to a destination folder and return the new ids for these items (Listing 5-14)
        /// </summary>
        /// <param name="binding">Exchange binding to use for the call</param>
        /// <param name="destinationFolderId">Destination for the items</param>
        /// <param name="itemsToCopy">Items to copy</param>
        /// <returns>List of new item ids</returns>
        /// 
        public List<ItemIdType> CopyItemEx(
                             BaseFolderIdType destinationFolderId,
                             List<BaseItemIdType> itemsToCopy)
        {
            // STEP 1:  First, we need to retrieve some unique information about 
            // each item.  Let's use the PR_SEARCH_KEY. Note that extended properties are 
            // discussed in Chapter 13, "Extended Properties"
            //
            GetItemType getSearchKeyRequest = new GetItemType();

            PathToExtendedFieldType searchKeyPath = new PathToExtendedFieldType();
            searchKeyPath.PropertyTag = "0x300B";
            searchKeyPath.PropertyType = MapiPropertyTypeType.Binary;

            // Use ItemResponseShapeType overload from chapter 3. We want the Id and the
            // search key
            //
            ItemResponseShapeType idAndSearchKeyShape = new ItemResponseShapeType(
                                     DefaultShapeNamesType.IdOnly,
                                     searchKeyPath);
            getSearchKeyRequest.ItemShape = idAndSearchKeyShape;
            getSearchKeyRequest.ItemIds = itemsToCopy.ToArray();

            // Get the items
            // 
            GetItemResponseType getSearchKeyResponse =
                       this.GetItem(getSearchKeyRequest);
            List<string> base64SearchKeys = new List<string>(
                         getSearchKeyResponse.ResponseMessages.Items.Length);

            // For each item, add the search keys to our list 
            // 
            foreach (ItemInfoResponseMessageType searchKeyMessage in
                          getSearchKeyResponse.ResponseMessages.Items)
            {
                ExtendedPropertyType searchKeyProperty =
                        searchKeyMessage.Items.Items[0].ExtendedProperty[0];
                base64SearchKeys.Add((string)searchKeyProperty.Item);
            }

            // Now we have a list of the search keys for the items that we want to 
            // copy.
            // STEP 2:  Perform the copy

            CopyItemType copyItemRequest = new CopyItemType();
            copyItemRequest.ToFolderId = new TargetFolderIdType();
            copyItemRequest.ToFolderId.Item = destinationFolderId;

            // just copy the array from our GetItem request rather than building a 
            // new one.
            //
            copyItemRequest.ItemIds = getSearchKeyRequest.ItemIds;
            CopyItemResponseType copyResponse = this.CopyItem(copyItemRequest);

            // Now, we know that we do not get new ids from the above request, but 
            // we (read: you) SHOULD check the response code for each of the copies 
            // operations.
            //
            // STEP 3:  For each successful copy, we want to find the items by 
            // search key.
            //
            FindItemType findBySearchKey = new FindItemType();
            findBySearchKey.ItemShape = idAndSearchKeyShape;
            findBySearchKey.ParentFolderIds = new BaseFolderIdType[] { 
              destinationFolderId };
            findBySearchKey.Traversal = ItemQueryTraversalType.Shallow;
            findBySearchKey.Restriction = new RestrictionType();

            // Here we need to build up our query.  Rather than issuing several 
            // FindItem calls, let's build up a single OR restriction here with a 
            // bunch of items. Note that EWS restricts filter depths, so we
            // might need to break this up depending on how many items we are 
            // copying...
            //
            if (base64SearchKeys.Count > 1)
            {
                OrType or = new OrType();
                List<IsEqualToType> orChildren = new List<IsEqualToType>();
                foreach (string searchKey in base64SearchKeys)
                {
                    // Note that CreateIsEqualToSearchKey is implemented on the partial class
                    // extension of RestrictionType.
                    //
                    IsEqualToType isEqualTo = RestrictionType.CreateIsEqualToSearchKey(
                          searchKeyPath, searchKey);
                    orChildren.Add(isEqualTo);
                }
                or.Items = orChildren.ToArray();

                findBySearchKey.Restriction.Item = or;
            }
            else
            {
                // we only have one item.  No need for the OR clause
                //
                IsEqualToType isEqualTo = RestrictionType.CreateIsEqualToSearchKey(
                                searchKeyPath, base64SearchKeys[0]);
                findBySearchKey.Restriction.Item = isEqualTo;
            }

            FindItemResponseType findResponse = this.FindItem(findBySearchKey);

            // Since we searched in a single target folder, we will have a single 
            // response message
            //
            FindItemResponseMessageType findResponseMessage =
                findResponse.ResponseMessages.Items[0] as FindItemResponseMessageType;
            ItemType[] foundItems = (findResponseMessage.RootFolder.Item as
                                        ArrayOfRealItemsType).Items;
            List<ItemIdType> newIds = new List<ItemIdType>();
            foreach (ItemType item in foundItems)
            {
                newIds.Add(item.ItemId);
            }
            return newIds;
        }

        /// <summary>
        /// Sets the ExchangeImpersonation soap header on the binding prior to a 
        /// call
        /// </summary>
        /// <param name="sid">SID of user to "Act As"</param>
        /// 
        public void SetExchangeImpersonation(SecurityIdentifier sid)
        {
            ExchangeImpersonationType header = new ExchangeImpersonationType();
            header.ConnectingSID = new ConnectingSIDType();
            header.ConnectingSID.SID = sid.ToString();
            this.ExchangeImpersonation = header;
        }

	}
}
