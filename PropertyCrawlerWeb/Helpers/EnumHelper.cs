using PropertyCrawler.Data;
using PropertyCrawler.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PropertyCrawlerWeb.Helpers
{
    public static class EnumHelper
    {
        public class SelectVM
        {
            public int Id { get; set; }
            public string Text { get; set; }
        }


        public static List<SelectVM> PropertyTypeList()
        {
            var propertyType = Enum.GetValues(typeof(PropertyType)).Cast<int>().ToList();
            var propName = Enum.GetNames(typeof(PropertyType)).ToList();
            var propTypes = new List<SelectVM>();
            for (int i = 0; i < propertyType.Count; i++)
            {
                propTypes.Add(new SelectVM
                {
                    Id = propertyType[i],
                    Text = propName[i]
                });
            }
            return propTypes;
        }


        public static List<SelectVM> ProcessTypeList()
        {
            var processType = Enum.GetValues(typeof(ProcessType)).Cast<int>().ToList();
            var processName = Enum.GetNames(typeof(ProcessType)).ToList();
            var processTypes = new List<SelectVM>();
            for (int i = 0; i < processType.Count; i++)
            {
                processTypes.Add(new SelectVM
                {
                    Id = processType[i],
                    Text = processName[i].Replace("_", " ")
                });
            }
            return processTypes;
        }
        public static List<SelectVM> ScheduleIntervalType()
        {
            var schIntType = Enum.GetValues(typeof(ScheduleInterval)).Cast<int>().ToList();
            var schIntName = Enum.GetNames(typeof(ScheduleInterval)).ToList();
            var schIntTypes = new List<SelectVM>();
            for (int i = 0; i < schIntType.Count; i++)
            {
                schIntTypes.Add(new SelectVM
                {
                    Id = schIntType[i],
                    Text = schIntName[i].Replace("_", " ")
                });
            }
            return schIntTypes;
        }
    }
}
