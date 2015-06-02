using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Text;
using System.Linq.Expressions;
using System.Web.UI;
using System.Reflection;
using System.Data.Common;
using System.Web;
using MongoDB.Driver;
using MongoDB.Driver.Builders;

namespace GridCustom
{
    [Serializable]
    public class PowerDataSource<T> : List<T>
    {
        //SQL SERVER
        public PowerDataSource(IQueryable<T> source, Grid grid, string columnOrder, bool ignorarPaginacao)
        {
            //string PageContext = System.IO.Path.GetFileName(System.Web.HttpContext.Current.Request.Url.AbsolutePath);

            string PageContext = System.Web.HttpContext.Current.Request.Url.AbsolutePath.Replace("http://", string.Empty).Replace("https://", string.Empty);

            if (source.Count() > 1000)
                source = source.Take(1000);

            int index = grid.CurrentPage;

            int pageSize = grid.PageSize;

            if(string.IsNullOrEmpty(grid.OrderBy))
               grid.OrderBy = columnOrder;

            index = (index == 0 ? 1 : index);

            this.TotalCount = source.Count();

            this.PageSize = pageSize;
            this.PageIndex = index;

            if (this.TotalCount < this.PageSize + 1)
            {
                grid.CurrentPage = 1;
                index = 1;
            }

            if (this.TotalCount <= pageSize || ignorarPaginacao)
            {
                var dados = source.OrderBy(grid.OrderBy).Take(10000).ToList();
                this.TotalCount = source.Count();
                this.AddRange(dados);
            }
            else if(this.TotalCount > pageSize && !ignorarPaginacao)
            {
                this.TotalCount = source.Count();
                var dados = source.OrderBy(grid.OrderBy).Skip((index - 1) * pageSize).Take(pageSize).ToList();
                this.AddRange(dados);
            }

            double pageResult = 0;
            for (int counter = 1; pageResult < this.TotalCount; counter++)
            {
                pageResult = counter * this.PageSize;
                this.TotPages = counter;
            }

            double pages = 0;

            pages = (this.TotalCount / this.PageSize);

            if (pages == 0)
                pages = 1;

            grid.TotalPages = Convert.ToInt32(Math.Ceiling(pages));
        }


        public PowerDataSource(MongoCursor<T> source, Grid grid, string columnOrder, bool ignorarPaginacao)
        {

            
            //string PageContext = System.IO.Path.GetFileName(System.Web.HttpContext.Current.Request.Url.AbsolutePath);

            string PageContext = System.Web.HttpContext.Current.Request.Url.AbsolutePath.Replace("http://", string.Empty).Replace("https://", string.Empty);
            

            int index = grid.CurrentPage;

            int pageSize = grid.PageSize;

            if (string.IsNullOrEmpty(grid.OrderBy))
                grid.OrderBy = columnOrder;

            index = (index == 0 ? 1 : index);


            if (source.Query == null)
            {
                var collection = source.Collection;
                this.TotalCount = Convert.ToDouble(collection.Count());
            }
            else
                this.TotalCount = Convert.ToDouble(source.Collection.Count(source.Query));

            this.PageSize = pageSize;
            this.PageIndex = index;

            if (this.TotalCount < this.PageSize + 1)
            {
                grid.CurrentPage = 1;
                index = 1;
            }

            if (this.TotalCount <= pageSize || ignorarPaginacao)
            {
                var dados = source.SetLimit(15000).SetSortOrder(grid.OrderBy.EndsWith(" desc") ? SortBy.Descending(grid.OrderBy.Replace(" desc", string.Empty)) : SortBy.Ascending(grid.OrderBy.Replace(" asc", string.Empty)));
                this.TotalCount = dados.Count();
                this.AddRange(dados);
            }
            else if (this.TotalCount > pageSize && !ignorarPaginacao)
            {
                //this.TotalCount = source.Count();
                var dados = source.SetLimit(pageSize).SetSkip((index - 1) * pageSize).SetSortOrder(grid.OrderBy.EndsWith(" desc") ? SortBy.Descending(grid.OrderBy.Replace(" desc", string.Empty)) : SortBy.Ascending(grid.OrderBy.Replace(" asc", string.Empty)));
                this.AddRange(dados);
            }

            double pageResult = 0;
            for (int counter = 1; pageResult < this.TotalCount; counter++)
            {
                pageResult = counter * this.PageSize;
                this.TotPages = counter;
            }

            double pages = 0;

            pages = (this.TotalCount / this.PageSize);

            if (pages == 0)
                pages = 1;

            grid.TotalPages = Convert.ToInt32(Math.Ceiling(pages));

        }

        
        public double TotPages
        {
            get;
            set;
        }

        public double TotalCount
        {
            get;
            set;
        }

        public int PageIndex
        {
            get;
            set;
        }

        public double PageSize
        {
            get;
            set;
        }

        public bool HasPreviousPage
        {
            get
            {
                return (PageIndex > 1);
            }
        }

        public bool HasNextPage
        {
            get
            {
                return (PageIndex * PageSize) <= TotalCount;
            }
        }
    }
}
