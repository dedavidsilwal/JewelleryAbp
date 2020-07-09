using Abp.Application.Services;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Linq;
using System.Linq.Dynamic.Core;

namespace Jewellery.Jewellery
{
    public class JewelleryAppService : ApplicationService
    {
        public SelectList GetAllWeighTypes()
        {
            var EntityState = new SelectList(Enum.GetValues(typeof(WeightType)).Cast<WeightType>().Select(v => new SelectListItem
            {
                Text = v.ToString(),
                Value = ((int)v).ToString()
            }).ToList(), "Value", "Text");

            return EntityState;
        }
    }
}
