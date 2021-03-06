﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web;
using Umbraco.Courier.DataResolvers;

namespace Umbraco.Courier.uComponents.XpathLists
{
    public class XpathCheckboxList : MultiPropertyDataResolverProvider
    {
        public override IEnumerable<Guid> DataTypeIds
        {
            get
            {
                return new Collection<Guid>()
                           {
                                new Guid("d2d46927-f4f8-4b1b-add7-661cc09a0539"), new Guid("34451d92-d270-49ba-8c7f-ee55bfeee1cb")
                           };
            }
        }


        string dataXpath = "//nodeId";

        public override void ExtractingProperty(Core.Item item, ItemProviders.ContentProperty propertyData)
        {
            propertyData.Value = Umbraco.Courier.Core.Helpers.XmlDependencies.ReplaceIds(propertyData.Value.ToString(), dataXpath, Core.Enums.IdentifierReplaceDirection.FromGuidToNodeId);
        }

        public override void PackagingProperty(Core.Item item, ItemProviders.ContentProperty propertyData)
        {
            List<string> replacedIds = new List<string>();
            propertyData.Value = Umbraco.Courier.Core.Helpers.XmlDependencies.ReplaceIds(propertyData.Value.ToString(), dataXpath, Core.Enums.IdentifierReplaceDirection.FromNodeIdToGuid, out replacedIds);

            //these are the IDs we found in the picker, those documents are a dependency
            foreach (string guid in replacedIds)
            {
                if (!string.IsNullOrEmpty(guid.Trim()))
                    item.Dependencies.Add(guid, Courier.ItemProviders.ProviderIDCollection.documentItemProviderGuid);
            }
        }

    }
}