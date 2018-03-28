using System;
using System.Collections.Generic;
using MvvmHelpers;
using System.Linq;
using EventList.ViewModels;
using EventList.Models;

namespace EventList.Extensions
{
    public static class POIExtensions
    {
        public static IEnumerable<Grouping<string, POICategory>> GroupBySection(this IEnumerable<POICategory> categories)
        {
            return from cat in categories
                       orderby cat.TableSection
                       group cat by cat.TableSection
                       into catGroup
                                  select new Grouping<string, POICategory>(catGroup.Key, catGroup);
        }

        public static IEnumerable<Grouping<string, POISubCategory>> GroupBySection(this IEnumerable<POISubCategory> subcategories)
        {
            return from cat in subcategories
                   orderby cat.TableSection
                   group cat by cat.TableSection
                       into catGroup
                              select new Grouping<string, POISubCategory>(catGroup.Key, catGroup);
        }
    }
}
