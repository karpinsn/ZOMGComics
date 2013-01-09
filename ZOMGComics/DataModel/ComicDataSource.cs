using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Windows.ApplicationModel.Resources.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using System.Collections.Specialized;
using ZOMGComics.DataModel;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;

// The data model defined by this file serves as a representative example of a strongly-typed
// model that supports notification when members are added, removed, or modified.  The property
// names chosen coincide with data bindings in the standard item templates.
//
// Applications may use this model as a starting point and build on it, or discard it entirely and
// replace it with something appropriate to their needs.

namespace ZOMGComics.Data
{
    /// <summary>
    /// Creates a collection of groups and items with hard-coded content.
    /// 
    /// SampleDataSource initializes with placeholder data rather than live production
    /// data so that sample data is provided at both design-time and run-time.
    /// </summary>
    public sealed class ComicDataSource
    {
        private static ComicDataSource _dataSource = new ComicDataSource();

        private readonly IMobileServiceTable<Comic> _comicTable;
        private readonly IMobileServiceTable<ComicStrip> _comicStripTable;

        private ObservableCollection<Comic> _allComics = new ObservableCollection<Comic>();
        public ObservableCollection<Comic> AllComics
        {
            get { return this._allComics; }
        }

        public static IEnumerable<Comic> GetAllComics()
        {
            return _dataSource.AllComics;
        }

        public static Comic GetComic(int comicId)
        {
            // Simple linear search is acceptable for small data sets
            var matches = _dataSource.AllComics.Where((group) => group.Id.Equals(comicId));
            if (matches.Count() == 1) return matches.First();
            return null;
        }

        public static ComicStrip GetComicStrip(int stripId)
        {
            // Simple linear search is acceptable for small data sets
            var matches = _dataSource.AllComics.SelectMany(group => group.Items).Where((item) => item.Id.Equals(stripId));
            if (matches.Count() == 1) return matches.First();
            return null;
        }

        public ComicDataSource()
        {
            _comicTable = App.MobileService.GetTable<Comic>();
            _comicStripTable = App.MobileService.GetTable<ComicStrip>();
            this.PopulateData();
        }

        public async void PopulateData()
        {
            var query = (from comic in _comicTable
                         select comic);
            var results = await query.ToEnumerableAsync();

            foreach (var comic in results)
            {
                var stripQuery = (from comicStrip in _comicStripTable
                                    where comicStrip.ComicId == comic.Id
                                    select comicStrip).Take(10);

                foreach (var strip in await stripQuery.ToEnumerableAsync())
                {
                    comic.Items.Add(strip);
                }

                this.AllComics.Add(comic);
            }        
        }
    }
}
