namespace OrderService.NDSPosting
{
    public partial class NDSStatus : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged
    {

        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;

        private int StatusIdField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string StatusCodeField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string StatusField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string DescriptionField;

        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData
        {
            get
            {
                return this.extensionDataField;
            }
            set
            {
                this.extensionDataField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true)]
        public int StatusId
        {
            get
            {
                return this.StatusIdField;
            }
            set
            {
                if ((this.StatusIdField.Equals(value) != true))
                {
                    this.StatusIdField = value;
                    this.RaisePropertyChanged("StatusId");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 1)]
        public string StatusCode
        {
            get
            {
                return this.StatusCodeField;
            }
            set
            {
                if ((object.ReferenceEquals(this.StatusCodeField, value) != true))
                {
                    this.StatusCodeField = value;
                    this.RaisePropertyChanged("StatusCode");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 2)]
        public string Status
        {
            get
            {
                return this.StatusField;
            }
            set
            {
                if ((object.ReferenceEquals(this.StatusField, value) != true))
                {
                    this.StatusField = value;
                    this.RaisePropertyChanged("Status");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 3)]
        public string Description
        {
            get
            {
                return this.DescriptionField;
            }
            set
            {
                if ((object.ReferenceEquals(this.DescriptionField, value) != true))
                {
                    this.DescriptionField = value;
                    this.RaisePropertyChanged("Description");
                }
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
