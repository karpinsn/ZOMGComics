using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZOMGComics.DataModel
{
    /// <summary>
    /// Generic item data model.
    /// </summary>
    public class ComicStrip : ComicDataCommon
    {
        private int _comicId;
        public int ComicId
        {
            get { return this._comicId; }
            set { this.SetProperty(ref this._comicId, value); }
        }

        private string _extra;
        public string Extra
        {
            get { return this._extra; }
            set { this.SetProperty(ref this._extra, value); }
        }
    }
}
