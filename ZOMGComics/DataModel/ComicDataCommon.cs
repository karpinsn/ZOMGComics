using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace ZOMGComics.DataModel
{
    /// <summary>
    /// Base class for <see cref="Comic"/> and <see cref="ComicStrip"/> that
    /// defines properties common to both.
    /// </summary>
    [Windows.Foundation.Metadata.WebHostHidden]
    public abstract class ComicDataCommon : ZOMGComics.Common.BindableBase
    {
        private static Uri _baseUri = new Uri("ms-appx:///");

        private int _id;
        public int Id
        {
            get { return this._id; }
            set { this.SetProperty(ref this._id, value); }
        }

        private string _title = string.Empty;
        public string Title
        {
            get { return this._title; }
            set { this.SetProperty(ref this._title, value); }
        }

        private ImageSource _image = null;

        public ImageSource Image
        {
            get
            {
                if (this._image == null && this._imagePath != null)
                {
                    this._image = new BitmapImage(new Uri(ComicDataCommon._baseUri, this._imagePath));
                }
                return this._image;
            }
            set
            {
                this._imagePath = null;
                this.SetProperty(ref this._image, value);
            }
        }

        private String _imagePath = null;
        public String ImagePath
        {
            get
            { return this._imagePath; }
            set
            {
                this._image = null;
                this._imagePath = value;
                this.OnPropertyChanged("Image");
            }
        }
    }
}
