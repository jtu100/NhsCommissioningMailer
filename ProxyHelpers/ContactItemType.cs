//  Inside Microsoft Exchange 2007 Web Services
//  ProxyHelpers
//
//  Copyright (c) 2007 David Sterling, Ben Spain, Mark Taylor, Huw Upshall, Michael Mainer.  
//  All rights reserved.
//

using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace ProxyHelpers.EWS
{
	/// <summary>
	/// Partial class extension on ContactItemType (Listing 6-7)
	/// </summary>
	public partial class ContactItemType
	{
		// Let's set up our property paths as we will need them in several places.
		//
		private static readonly PathToExtendedFieldType TitlePath = PathToExtendedFieldType.BuildPropertyTag(14917, MapiPropertyTypeType.String);
		private static readonly PathToUnindexedFieldType GivenNamePath = new PathToUnindexedFieldType(UnindexedFieldURIType.contactsGivenName);
		private static readonly PathToUnindexedFieldType MiddleNamePath = new PathToUnindexedFieldType(UnindexedFieldURIType.contactsMiddleName);
		private static readonly PathToUnindexedFieldType SurnamePath = new PathToUnindexedFieldType(UnindexedFieldURIType.contactsSurname);
		private static readonly PathToUnindexedFieldType GenerationPath = new PathToUnindexedFieldType(UnindexedFieldURIType.contactsGeneration);
		private static readonly PathToUnindexedFieldType InitialsPath = new PathToUnindexedFieldType(UnindexedFieldURIType.contactsInitials);
		private static readonly PathToUnindexedFieldType DisplayNamePath = new PathToUnindexedFieldType(UnindexedFieldURIType.contactsDisplayName);
		private static readonly PathToUnindexedFieldType NicknamePath = new PathToUnindexedFieldType(UnindexedFieldURIType.contactsNickname);
		private static readonly PathToExtendedFieldType YomiFirstNamePath = PathToExtendedFieldType.BuildGuidId(
			DistinguishedPropertySetType.Address, 32812, MapiPropertyTypeType.String);
		private static readonly PathToExtendedFieldType YomiLastNamePath = PathToExtendedFieldType.BuildGuidId(
			DistinguishedPropertySetType.Address, 32813, MapiPropertyTypeType.String);

		
		/// <summary>
		/// Helper method for setting the underlying fields represented by the CompleteName property
		/// </summary>
		/// <param name="binding">ExchangeServiceBinding to use for the call</param>
		/// <param name="contactId">Id and change key of the contact to update</param>
		/// <param name="completeName">The complete name to set on the contact</param>
		/// <returns>ItemInfoResponse message due to UpdateItem call</returns>
		/// 
		public static ItemInfoResponseMessageType SetCompleteName(
									ExchangeServiceBinding binding, 
									ItemIdType contactId,
									CompleteNameType completeName)
		{
			// Create our request.  We will do a single UpdateItem call with a bunch of change descriptions.
			//
			UpdateItemType updateRequest = new UpdateItemType();

			// We are updating a single item
			//
			ItemChangeType itemChange = new ItemChangeType();
			itemChange.Item = contactId;
			updateRequest.ItemChanges = new ItemChangeType[] { itemChange };

			// We will only set those props that are not null in the complete name.  So right now, we
			// don't know how many that will be, so let's create a list to hold the change descriptions.
			//
			List<ItemChangeDescriptionType> changeList = new List<ItemChangeDescriptionType>();

			// Now, for each possible property, let's check to make sure it is not null, then we will set the
			// value on a ContactItem instance needed for our change description and add it to our change list.
			//
			// Title
			if (completeName.Title != null)
			{
				ContactItemType titleContact = new ContactItemType();
				ExtendedPropertyType titleProp = new ExtendedPropertyType(
					TitlePath,
					completeName.Title);

				titleContact.ExtendedProperty = new ExtendedPropertyType[] { titleProp };
				changeList.Add(new SetItemFieldType(TitlePath, titleContact));
			}

			// GivenName
			if (completeName.FirstName != null)
			{
				ContactItemType givenNameContact = new ContactItemType();
				givenNameContact.GivenName = completeName.FirstName;
				changeList.Add(new SetItemFieldType(GivenNamePath, givenNameContact));
			}

			// MiddleName
			if (completeName.MiddleName != null)
			{
				ContactItemType middleNameContact = new ContactItemType();
				middleNameContact.MiddleName = completeName.MiddleName;
				changeList.Add(new SetItemFieldType(MiddleNamePath, middleNameContact));
			}

			// Surname
			if (completeName.LastName != null)
			{
				ContactItemType surnameContact = new ContactItemType();
				surnameContact.Surname = completeName.LastName;
				changeList.Add(new SetItemFieldType(SurnamePath, surnameContact));
			}

			// Generation
			if (completeName.Suffix != null)
			{
				ContactItemType generationContact = new ContactItemType();
				generationContact.Generation = completeName.Suffix;
				changeList.Add(new SetItemFieldType(GenerationPath, generationContact));
			}

			// Initials
			if (completeName.Initials != null)
			{
				ContactItemType initialsContact = new ContactItemType();
				initialsContact.Initials = completeName.Initials;
				changeList.Add(new SetItemFieldType(InitialsPath, initialsContact));
			}

			// DisplayName
			if (completeName.FullName != null)
			{
				ContactItemType displayNameContact = new ContactItemType();
				displayNameContact.DisplayName = completeName.FullName;
				changeList.Add(new SetItemFieldType(DisplayNamePath, displayNameContact));
			}

			// Nickname
			if (completeName.Nickname != null)
			{
				ContactItemType nicknameContact = new ContactItemType();
				nicknameContact.Nickname = completeName.Nickname;
				changeList.Add(new SetItemFieldType(NicknamePath, nicknameContact));
			}

			// YomiFirstName
			if (completeName.YomiFirstName != null)
			{
				ContactItemType yomiFirstContact = new ContactItemType();
				ExtendedPropertyType yomiFirstProp = new ExtendedPropertyType(
					YomiFirstNamePath,
					completeName.YomiFirstName);

				yomiFirstContact.ExtendedProperty = new ExtendedPropertyType[] { yomiFirstProp };
				changeList.Add(new SetItemFieldType(YomiFirstNamePath, yomiFirstContact));
			}

			// YomiLastName
			if (completeName.YomiLastName != null)
			{
				ContactItemType yomiLastContact = new ContactItemType();
				ExtendedPropertyType yomiLastProp = new ExtendedPropertyType(
					YomiLastNamePath,
					completeName.YomiLastName);

				yomiLastContact.ExtendedProperty = new ExtendedPropertyType[] { yomiLastProp };
				changeList.Add(new SetItemFieldType(YomiLastNamePath, yomiLastContact));
			}

			// If they passed in a CompleteName with all NULL props, we should fail.
			//
			if (changeList.Count == 0)
			{
				throw new ArgumentException("No parts of CompleteName were set", "completeName");
			}

			itemChange.Updates = changeList.ToArray();
			updateRequest.ConflictResolution = ConflictResolutionType.AlwaysOverwrite;
			
			// Make the call and return the response message
			//
			return binding.UpdateItem(updateRequest).ResponseMessages.Items[0] as ItemInfoResponseMessageType;
		}
	}
}
