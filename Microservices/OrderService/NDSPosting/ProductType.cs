namespace OrderService.NDSPosting
{
    public partial class ProductType : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged
    {

        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string ProductTypeNameField;

        private int ProductTypeIdField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string PrefixField;

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
        public string ProductTypeName
        {
            get
            {
                return this.ProductTypeNameField;
            }
            set
            {
                if ((object.ReferenceEquals(this.ProductTypeNameField, value) != true))
                {
                    this.ProductTypeNameField = value;
                    this.RaisePropertyChanged("ProductTypeName");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 1)]
        public int ProductTypeId
        {
            get
            {
                return this.ProductTypeIdField;
            }
            set
            {
                if ((this.ProductTypeIdField.Equals(value) != true))
                {
                    this.ProductTypeIdField = value;
                    this.RaisePropertyChanged("ProductTypeId");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 2)]
        public string Prefix
        {
            get
            {
                return this.PrefixField;
            }
            set
            {
                if ((object.ReferenceEquals(this.PrefixField, value) != true))
                {
                    this.PrefixField = value;
                    this.RaisePropertyChanged("Prefix");
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
