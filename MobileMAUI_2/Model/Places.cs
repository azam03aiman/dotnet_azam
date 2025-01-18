using SQLite;
using System.ComponentModel;

namespace MobileMAUI_2.Model
{
    [Table("Places")]  // Specifies the table name in the database
    public class Places : INotifyPropertyChanged
    {
        private int id;
        private string name;
        private string description;
        private string city;

        [PrimaryKey, AutoIncrement]  // Marks 'Id' as the primary key with auto-increment
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

        [MaxLength(50)]  // Restricts the 'Name' field to 50 characters
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

        [MaxLength(255)]  // Restricts the 'Description' field to 255 characters
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

        [MaxLength(50)]  // Restricts the 'City' field to 50 characters
        public string City
        {
            get => city;
            set
            {
                if (city != value)
                {
                    city = value;
                    OnPropertyChanged(nameof(City));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        // Optional: Constructor to initialize default values
        public Places()
        {
            name = string.Empty;
            description = string.Empty;
            city = string.Empty;
        }
    }
}
