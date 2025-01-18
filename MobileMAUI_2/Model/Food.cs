using SQLite;
using System.ComponentModel;

namespace MobileMAUI_2.Model
{
    [Table("Food")]
    public class Food : INotifyPropertyChanged
    {
        private int id;
        private string name;
        private string description;
        private int rating;

        [PrimaryKey, AutoIncrement]
        public int Id
        {
            get => id;
            set
            {
                if (id != value)
                {
                    id = value;
                    OnPropertyChanged(nameof(Id));
                }
            }
        }

        [MaxLength(50), Unique]
        public string Name
        {
            get => name;
            set
            {
                if (name != value)
                {
                    name = value;
                    OnPropertyChanged(nameof(Name));
                }
            }
        }

        [MaxLength(200)]
        public string Description
        {
            get => description;
            set
            {
                if (description != value)
                {
                    description = value;
                    OnPropertyChanged(nameof(Description));
                }
            }
        }

        public int Rating
        {
            get => rating;
            set
            {
                if (rating != value)
                {
                    rating = value;
                    OnPropertyChanged(nameof(Rating));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
