namespace OrderService.NDSPosting
{
    public partial class StructureElement : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged
    {

        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string StructureElementTypeField;

        private int StructureElementTypeIdField;

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
        public string StructureElementType
        {
            get
            {
                return this.StructureElementTypeField;
            }
            set
            {
                if ((object.ReferenceEquals(this.StructureElementTypeField, value) != true))
                {
                    this.StructureElementTypeField = value;
                    this.RaisePropertyChanged("StructureElementType");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true)]
        public int StructureElementTypeId
        {
            get
            {
                return this.StructureElementTypeIdField;
            }
            set
            {
                if ((this.StructureElementTypeIdField.Equals(value) != true))
                {
                    this.StructureElementTypeIdField = value;
                    this.RaisePropertyChanged("StructureElementTypeId");
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
