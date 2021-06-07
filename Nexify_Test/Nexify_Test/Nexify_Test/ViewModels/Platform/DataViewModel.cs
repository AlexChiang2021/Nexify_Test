using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Nexify_Test.ViewModels.Platform
{

    public class DataViewModel
    {
        /// <summary>
        /// 流水號
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 名字
        /// </summary>
        [DisplayName("名字")]
        public string Name { get; set; }

        /// <summary>
        /// 出生日期
        /// </summary>
        [Display(Name = "出生日期")]
        public string DateOfBirth { get; set; }

        /// <summary>
        /// 工資
        /// </summary>
        [Display(Name = "工資")]
        public int Salary{ get; set; }

        /// <summary>
        /// 住址
        /// </summary>
        [Display(Name = "住址")]
        public string Address { get; set; }
        
    }
}