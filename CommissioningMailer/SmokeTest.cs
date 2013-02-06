using System;
using System.Collections.Generic;
using System.Linq;
using Comissioning.Data;
using NUnit.Framework;

namespace Commissioning.Data
{
    [TestFixture]
    public class SmokeTest
    {
        private static class SampleData
        {
            public const string SurgeriesPath = "SampleData\\Surgeries.csv";
            public const string SusExtractForSurgeriesPath = "SampleData\\SUS Extract for Surgeries.csv";
        }

        [Test]
        public void GetAll_Returns_All_KeyedEmailAddresses()
        {
            var keyedEmailAddresses = new KeyedEmailAddressRepository(SampleData.SurgeriesPath).GetAll();
            Assert.That(keyedEmailAddresses.Count(), Is.EqualTo(21));
        }

        [Test]
        public void GetAll_Returns_All_KeyedData()
        {
            var keyedDatas = new KeyedDataRepository(SampleData.SusExtractForSurgeriesPath).GetAll();
            Assert.That(keyedDatas.Count(), Is.EqualTo(693));
        }
    }

    [TestFixture]
    public class Tests
    {
       

        [Test]
        public void CanJoinRecords()
        {
            var keyedEmailAddresses = new List<KeyedEmailAddress>
                                {
                                    new KeyedEmailAddress {Key = "KEY1", EmailAddress = "person1@nhs.net"},
                                    new KeyedEmailAddress {Key = "KEY1", EmailAddress = "person2@nhs.net"},
                                };
            var keyedDatas = new List<KeyedData>()
                                 {
                                     new KeyedData() {Data = new string[] {"KEY1", "VALUE"}},
                                 };


            var result = DataEmailAddressGroup.CreateGroups(keyedEmailAddresses, keyedDatas);
            result.Count();
        }

 

        // Test scenarios:
        // One file per key which has at least one email and one data record
        // Keys present in data but not present in emails
        // Keys present in emails but not present in data
    }
}
