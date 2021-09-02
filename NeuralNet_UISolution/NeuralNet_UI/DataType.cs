using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace NeuralNet_UI
{
    public class DataType : INotifyPropertyChanged
    {
        float content;
        public float Content 
        {
            get { return content; }
            set 
            {
                // Equality check?
                content = value; 
                OnPropertyChanged(); 
            }
        }

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
