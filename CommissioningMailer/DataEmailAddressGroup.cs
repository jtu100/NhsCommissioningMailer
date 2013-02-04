using System.Collections.Generic;
using System.Linq;

namespace CommissioningMailer
{
    public class DataEmailAddressGroup
    {
        public string Key { get; set; }
        public IEnumerable<KeyedData> Data { get; set; }
        public IEnumerable<KeyedEmailAddress> EmailAddresses { get; set; }

        public static IEnumerable<DataEmailAddressGroup> GroupDataAndEmailAddresses(
            IEnumerable<KeyedEmailAddress> keyedEmailAddresses,
            IEnumerable<KeyedData> keyedDatas)
        {
            var groupedData = from keyedData in keyedDatas
                              group keyedData by keyedData.Key;

            var groupedEmailAddresses = from keyedEmailAddress in keyedEmailAddresses
                                        group keyedEmailAddress by keyedEmailAddress.Key;

            var dataEmailAddressGroups = from data in groupedData
                                         join emailAddresses in groupedEmailAddresses
                                             on data.Key equals emailAddresses.Key
                                         select new DataEmailAddressGroup()
                                                    {
                                                        Key = data.Key,
                                                        Data = data,
                                                        EmailAddresses = emailAddresses
                                                    };

            return dataEmailAddressGroups;
        }
    }
}