namespace OrderService.NDSPosting
{
    public partial class Customer : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged
    {

        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string CustomerNameField;

        private int CustomerIdField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string CustomerNumberField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private NDSPosting.Contract CustomerContractField;

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

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false)]
        public string CustomerName
        {
            get
            {
                return this.CustomerNameField;
            }
            set
            {
                if ((object.ReferenceEquals(this.CustomerNameField, value) != true))
                {
                    this.CustomerNameField = value;
                    this.RaisePropertyChanged("CustomerName");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 1)]
        public int CustomerId
        {
            get
            {
                return this.CustomerIdField;
            }
            set
            {
                if ((this.CustomerIdField.Equals(value) != true))
                {
                    this.CustomerIdField = value;
                    this.RaisePropertyChanged("CustomerId");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 2)]
        public string CustomerNumber
        {
            get
            {
                return this.CustomerNumberField;
            }
            set
            {
                if ((object.ReferenceEquals(this.CustomerNumberField, value) != true))
                {
                    this.CustomerNumberField = value;
                    this.RaisePropertyChanged("CustomerNumber");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 3)]
        public NDSPosting.Contract CustomerContract
        {
            get
            {
                return this.CustomerContractField;
            }
            set
            {
                if ((object.ReferenceEquals(this.CustomerContractField, value) != true))
                {
                    this.CustomerContractField = value;
                    this.RaisePropertyChanged("CustomerContract");
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
