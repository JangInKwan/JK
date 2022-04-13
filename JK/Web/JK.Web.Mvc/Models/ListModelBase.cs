using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JK.Web.Mvc.Models
{
    public class ListModelBase<T> where T : class
    {
        /// <summary>
        /// 리스트
        /// </summary>
        public IEnumerable<T> List { get; set; }
        /// <summary>
        /// 모든 아이템 카운트
        /// </summary>
        public int TotalCount { get; set; } = 0;
        /// <summary>
        /// 페이지 사이즈
        /// </summary>
        public int PageSize { get; set; } = 10;
        /// <summary>
        /// 현재 페이지
        /// </summary>
        public int Page { get; set; } = 1;
    }
}