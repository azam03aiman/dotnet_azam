using System;
using SQLite;
using System.ComponentModel;

namespace MobileMAUI_2.Model
{
    [Table("Friend")]
    public class Friend : INotifyPropertyChanged
    {
        private int id;
        private string name;
        private string description;
        private string phone;
        private int age;

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

        [MaxLength(50)]
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

        [MaxLength(15)]
        public string Phone
        {
            get => phone;
            set
            {
                if (phone != value)
                {
                    phone = value;
                    OnPropertyChanged(nameof(Phone));
                }
            }
        }

        public int Age
        {
            get => age;
            set
            {
                if (age != value)
                {
                    age = value;
                    OnPropertyChanged(nameof(Age));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        // Optional: Constructor to initialize default values
        public Friend()
        {
            name = string.Empty;
            description = string.Empty;
            phone = string.Empty;
            age = 0;  // Default age can be 0 or any valid value.
        }
    }
}
