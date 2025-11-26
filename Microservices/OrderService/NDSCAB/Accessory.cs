namespace OrderService.NDSCAB
{
    public partial class Accessory : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged
    {

        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;

        private int SEDetailingIDField;

        private int AccProductMarkIDField;

        private int SAPMaterialCodeIDField;

        private int NoOfPiecesField;

        private int CABProductMarkIDField;

        private int GroupMarkIdField;

        private int ActualWeightField;

        private int ExternalWidthField;

        private int ExternalHeightField;

        private int ExternalLengthField;

        private int IsCouplerField;

        private int OrderQtyField;

        private int CentralizerFlagField;

        private int LengthField;

        private int BitIsCouplerField;

        private double UnitWeightField;

        private double InvoiceWeightField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string UOMField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string CouplerTypeField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string SAPMaterialCodeField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string CABProductMarkNameField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string AccProductMarkingNameField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string standardField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string MaterialTypeField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private NDSCAB.CABItem CABParentItemField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private NDSCAB.Accessory[] AccessoriesListField;

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
        public int SEDetailingID
        {
            get
            {
                return this.SEDetailingIDField;
            }
            set
            {
                if ((this.SEDetailingIDField.Equals(value) != true))
                {
                    this.SEDetailingIDField = value;
                    this.RaisePropertyChanged("SEDetailingID");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 1)]
        public int AccProductMarkID
        {
            get
            {
                return this.AccProductMarkIDField;
            }
            set
            {
                if ((this.AccProductMarkIDField.Equals(value) != true))
                {
                    this.AccProductMarkIDField = value;
                    this.RaisePropertyChanged("AccProductMarkID");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 2)]
        public int SAPMaterialCodeID
        {
            get
            {
                return this.SAPMaterialCodeIDField;
            }
            set
            {
                if ((this.SAPMaterialCodeIDField.Equals(value) != true))
                {
                    this.SAPMaterialCodeIDField = value;
                    this.RaisePropertyChanged("SAPMaterialCodeID");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 3)]
        public int NoOfPieces
        {
            get
            {
                return this.NoOfPiecesField;
            }
            set
            {
                if ((this.NoOfPiecesField.Equals(value) != true))
                {
                    this.NoOfPiecesField = value;
                    this.RaisePropertyChanged("NoOfPieces");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 4)]
        public int CABProductMarkID
        {
            get
            {
                return this.CABProductMarkIDField;
            }
            set
            {
                if ((this.CABProductMarkIDField.Equals(value) != true))
                {
                    this.CABProductMarkIDField = value;
                    this.RaisePropertyChanged("CABProductMarkID");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 5)]
        public int GroupMarkId
        {
            get
            {
                return this.GroupMarkIdField;
            }
            set
            {
                if ((this.GroupMarkIdField.Equals(value) != true))
                {
                    this.GroupMarkIdField = value;
                    this.RaisePropertyChanged("GroupMarkId");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 6)]
        public int ActualWeight
        {
            get
            {
                return this.ActualWeightField;
            }
            set
            {
                if ((this.ActualWeightField.Equals(value) != true))
                {
                    this.ActualWeightField = value;
                    this.RaisePropertyChanged("ActualWeight");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 7)]
        public int ExternalWidth
        {
            get
            {
                return this.ExternalWidthField;
            }
            set
            {
                if ((this.ExternalWidthField.Equals(value) != true))
                {
                    this.ExternalWidthField = value;
                    this.RaisePropertyChanged("ExternalWidth");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 8)]
        public int ExternalHeight
        {
            get
            {
                return this.ExternalHeightField;
            }
            set
            {
                if ((this.ExternalHeightField.Equals(value) != true))
                {
                    this.ExternalHeightField = value;
                    this.RaisePropertyChanged("ExternalHeight");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 9)]
        public int ExternalLength
        {
            get
            {
                return this.ExternalLengthField;
            }
            set
            {
                if ((this.ExternalLengthField.Equals(value) != true))
                {
                    this.ExternalLengthField = value;
                    this.RaisePropertyChanged("ExternalLength");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 10)]
        public int IsCoupler
        {
            get
            {
                return this.IsCouplerField;
            }
            set
            {
                if ((this.IsCouplerField.Equals(value) != true))
                {
                    this.IsCouplerField = value;
                    this.RaisePropertyChanged("IsCoupler");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 11)]
        public int OrderQty
        {
            get
            {
                return this.OrderQtyField;
            }
            set
            {
                if ((this.OrderQtyField.Equals(value) != true))
                {
                    this.OrderQtyField = value;
                    this.RaisePropertyChanged("OrderQty");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 12)]
        public int CentralizerFlag
        {
            get
            {
                return this.CentralizerFlagField;
            }
            set
            {
                if ((this.CentralizerFlagField.Equals(value) != true))
                {
                    this.CentralizerFlagField = value;
                    this.RaisePropertyChanged("CentralizerFlag");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 13)]
        public int Length
        {
            get
            {
                return this.LengthField;
            }
            set
            {
                if ((this.LengthField.Equals(value) != true))
                {
                    this.LengthField = value;
                    this.RaisePropertyChanged("Length");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 14)]
        public int BitIsCoupler
        {
            get
            {
                return this.BitIsCouplerField;
            }
            set
            {
                if ((this.BitIsCouplerField.Equals(value) != true))
                {
                    this.BitIsCouplerField = value;
                    this.RaisePropertyChanged("BitIsCoupler");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 15)]
        public double UnitWeight
        {
            get
            {
                return this.UnitWeightField;
            }
            set
            {
                if ((this.UnitWeightField.Equals(value) != true))
                {
                    this.UnitWeightField = value;
                    this.RaisePropertyChanged("UnitWeight");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 16)]
        public double InvoiceWeight
        {
            get
            {
                return this.InvoiceWeightField;
            }
            set
            {
                if ((this.InvoiceWeightField.Equals(value) != true))
                {
                    this.InvoiceWeightField = value;
                    this.RaisePropertyChanged("InvoiceWeight");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 17)]
        public string UOM
        {
            get
            {
                return this.UOMField;
            }
            set
            {
                if ((object.ReferenceEquals(this.UOMField, value) != true))
                {
                    this.UOMField = value;
                    this.RaisePropertyChanged("UOM");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 18)]
        public string CouplerType
        {
            get
            {
                return this.CouplerTypeField;
            }
            set
            {
                if ((object.ReferenceEquals(this.CouplerTypeField, value) != true))
                {
                    this.CouplerTypeField = value;
                    this.RaisePropertyChanged("CouplerType");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 19)]
        public string SAPMaterialCode
        {
            get
            {
                return this.SAPMaterialCodeField;
            }
            set
            {
                if ((object.ReferenceEquals(this.SAPMaterialCodeField, value) != true))
                {
                    this.SAPMaterialCodeField = value;
                    this.RaisePropertyChanged("SAPMaterialCode");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 20)]
        public string CABProductMarkName
        {
            get
            {
                return this.CABProductMarkNameField;
            }
            set
            {
                if ((object.ReferenceEquals(this.CABProductMarkNameField, value) != true))
                {
                    this.CABProductMarkNameField = value;
                    this.RaisePropertyChanged("CABProductMarkName");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 21)]
        public string AccProductMarkingName
        {
            get
            {
                return this.AccProductMarkingNameField;
            }
            set
            {
                if ((object.ReferenceEquals(this.AccProductMarkingNameField, value) != true))
                {
                    this.AccProductMarkingNameField = value;
                    this.RaisePropertyChanged("AccProductMarkingName");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 22)]
        public string standard
        {
            get
            {
                return this.standardField;
            }
            set
            {
                if ((object.ReferenceEquals(this.standardField, value) != true))
                {
                    this.standardField = value;
                    this.RaisePropertyChanged("standard");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 23)]
        public string MaterialType
        {
            get
            {
                return this.MaterialTypeField;
            }
            set
            {
                if ((object.ReferenceEquals(this.MaterialTypeField, value) != true))
                {
                    this.MaterialTypeField = value;
                    this.RaisePropertyChanged("MaterialType");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 24)]
        public NDSCAB.CABItem CABParentItem
        {
            get
            {
                return this.CABParentItemField;
            }
            set
            {
                if ((object.ReferenceEquals(this.CABParentItemField, value) != true))
                {
                    this.CABParentItemField = value;
                    this.RaisePropertyChanged("CABParentItem");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 25)]
        public NDSCAB.Accessory[] AccessoriesList
        {
            get
            {
                return this.AccessoriesListField;
            }
            set
            {
                if ((object.ReferenceEquals(this.AccessoriesListField, value) != true))
                {
                    this.AccessoriesListField = value;
                    this.RaisePropertyChanged("AccessoriesList");
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
