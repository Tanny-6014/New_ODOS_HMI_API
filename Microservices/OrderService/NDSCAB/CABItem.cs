namespace OrderService.NDSCAB
{
    public partial class CABItem : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged
    {

        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;

        private int SEDetailingIDField;

        private int StructureMarkIdField;

        private int CABProductMarkIDField;

        private int ProductCodeIDField;

        private int QuantityField;

        private int PinSizeIDField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string StatusField;

        private int DiameterField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string ShapeCodeField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string DescScriptField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string CABProductMarkNameField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private NDSCAB.ShapeParameter[] ShapeParametersListField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private NDSCAB.Accessory[] accListField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private NDSCAB.CABItem CABProductItemField;

        private int MemberQtyField;

        private int PinSizeField;

        private double InvoiceLengthField;

        private double ProductionLengthField;

        private double InvoiceWeightField;

        private double ProductionWeightField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string GradeField;

        private int ShapeTransHeaderIDField;

        private int GroupMarkIdField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string ShapeGroupField;

        private int EndCountField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string ImagePathField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string CustomerRemarksField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string PageNumberField;

        private double EnvLengthField;

        private double EnvWidthField;

        private double EnvHeightField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string ShapeImageField;

        private int NoOfBendsField;

        private int TransportModeIdField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string BVBSField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string CreatedByField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string ProduceIndicatorField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string BarMarkField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string CommercialDescField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string ShapeTypeField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private NDSCAB.Accessory accItemField;

        private bool IsReadOnlyField;

        private int intSEDetailingIdField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string ipAddressField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string ACCProductNameforCABField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string Coupler1Field;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string Coupler2Field;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string Thread1Field;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string Thread2Field;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string Coupler1TypeField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string Coupler1StandardField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string Thread1TypeField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string Thread1StandardField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string Coupler2TypeField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string Coupler2StandardField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string Thread2TypeField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string Thread2StandardField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string Locknut1Field;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string Locknut2Field;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string BBSNoField;

        private bool IsVPNUsersField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string ProduceIndField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private NDSCAB.ShapeCode ShapeField;

        private bool IsVariableBarField;

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

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true)]
        public int StructureMarkId
        {
            get
            {
                return this.StructureMarkIdField;
            }
            set
            {
                if ((this.StructureMarkIdField.Equals(value) != true))
                {
                    this.StructureMarkIdField = value;
                    this.RaisePropertyChanged("StructureMarkId");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 2)]
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

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 3)]
        public int ProductCodeID
        {
            get
            {
                return this.ProductCodeIDField;
            }
            set
            {
                if ((this.ProductCodeIDField.Equals(value) != true))
                {
                    this.ProductCodeIDField = value;
                    this.RaisePropertyChanged("ProductCodeID");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 4)]
        public int Quantity
        {
            get
            {
                return this.QuantityField;
            }
            set
            {
                if ((this.QuantityField.Equals(value) != true))
                {
                    this.QuantityField = value;
                    this.RaisePropertyChanged("Quantity");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 5)]
        public int PinSizeID
        {
            get
            {
                return this.PinSizeIDField;
            }
            set
            {
                if ((this.PinSizeIDField.Equals(value) != true))
                {
                    this.PinSizeIDField = value;
                    this.RaisePropertyChanged("PinSizeID");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 6)]
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

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 7)]
        public int Diameter
        {
            get
            {
                return this.DiameterField;
            }
            set
            {
                if ((this.DiameterField.Equals(value) != true))
                {
                    this.DiameterField = value;
                    this.RaisePropertyChanged("Diameter");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 8)]
        public string ShapeCode
        {
            get
            {
                return this.ShapeCodeField;
            }
            set
            {
                if ((object.ReferenceEquals(this.ShapeCodeField, value) != true))
                {
                    this.ShapeCodeField = value;
                    this.RaisePropertyChanged("ShapeCode");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 9)]
        public string DescScript
        {
            get
            {
                return this.DescScriptField;
            }
            set
            {
                if ((object.ReferenceEquals(this.DescScriptField, value) != true))
                {
                    this.DescScriptField = value;
                    this.RaisePropertyChanged("DescScript");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 10)]
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

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 11)]
        public NDSCAB.ShapeParameter[] ShapeParametersList
        {
            get
            {
                return this.ShapeParametersListField;
            }
            set
            {
                if ((object.ReferenceEquals(this.ShapeParametersListField, value) != true))
                {
                    this.ShapeParametersListField = value;
                    this.RaisePropertyChanged("ShapeParametersList");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 12)]
        public NDSCAB.Accessory[] accList
        {
            get
            {
                return this.accListField;
            }
            set
            {
                if ((object.ReferenceEquals(this.accListField, value) != true))
                {
                    this.accListField = value;
                    this.RaisePropertyChanged("accList");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 13)]
        public NDSCAB.CABItem CABProductItem
        {
            get
            {
                return this.CABProductItemField;
            }
            set
            {
                if ((object.ReferenceEquals(this.CABProductItemField, value) != true))
                {
                    this.CABProductItemField = value;
                    this.RaisePropertyChanged("CABProductItem");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 14)]
        public int MemberQty
        {
            get
            {
                return this.MemberQtyField;
            }
            set
            {
                if ((this.MemberQtyField.Equals(value) != true))
                {
                    this.MemberQtyField = value;
                    this.RaisePropertyChanged("MemberQty");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 15)]
        public int PinSize
        {
            get
            {
                return this.PinSizeField;
            }
            set
            {
                if ((this.PinSizeField.Equals(value) != true))
                {
                    this.PinSizeField = value;
                    this.RaisePropertyChanged("PinSize");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 16)]
        public double InvoiceLength
        {
            get
            {
                return this.InvoiceLengthField;
            }
            set
            {
                if ((this.InvoiceLengthField.Equals(value) != true))
                {
                    this.InvoiceLengthField = value;
                    this.RaisePropertyChanged("InvoiceLength");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 17)]
        public double ProductionLength
        {
            get
            {
                return this.ProductionLengthField;
            }
            set
            {
                if ((this.ProductionLengthField.Equals(value) != true))
                {
                    this.ProductionLengthField = value;
                    this.RaisePropertyChanged("ProductionLength");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 18)]
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

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 19)]
        public double ProductionWeight
        {
            get
            {
                return this.ProductionWeightField;
            }
            set
            {
                if ((this.ProductionWeightField.Equals(value) != true))
                {
                    this.ProductionWeightField = value;
                    this.RaisePropertyChanged("ProductionWeight");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 20)]
        public string Grade
        {
            get
            {
                return this.GradeField;
            }
            set
            {
                if ((object.ReferenceEquals(this.GradeField, value) != true))
                {
                    this.GradeField = value;
                    this.RaisePropertyChanged("Grade");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 21)]
        public int ShapeTransHeaderID
        {
            get
            {
                return this.ShapeTransHeaderIDField;
            }
            set
            {
                if ((this.ShapeTransHeaderIDField.Equals(value) != true))
                {
                    this.ShapeTransHeaderIDField = value;
                    this.RaisePropertyChanged("ShapeTransHeaderID");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 22)]
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

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 23)]
        public string ShapeGroup
        {
            get
            {
                return this.ShapeGroupField;
            }
            set
            {
                if ((object.ReferenceEquals(this.ShapeGroupField, value) != true))
                {
                    this.ShapeGroupField = value;
                    this.RaisePropertyChanged("ShapeGroup");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 24)]
        public int EndCount
        {
            get
            {
                return this.EndCountField;
            }
            set
            {
                if ((this.EndCountField.Equals(value) != true))
                {
                    this.EndCountField = value;
                    this.RaisePropertyChanged("EndCount");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 25)]
        public string ImagePath
        {
            get
            {
                return this.ImagePathField;
            }
            set
            {
                if ((object.ReferenceEquals(this.ImagePathField, value) != true))
                {
                    this.ImagePathField = value;
                    this.RaisePropertyChanged("ImagePath");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 26)]
        public string CustomerRemarks
        {
            get
            {
                return this.CustomerRemarksField;
            }
            set
            {
                if ((object.ReferenceEquals(this.CustomerRemarksField, value) != true))
                {
                    this.CustomerRemarksField = value;
                    this.RaisePropertyChanged("CustomerRemarks");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 27)]
        public string PageNumber
        {
            get
            {
                return this.PageNumberField;
            }
            set
            {
                if ((object.ReferenceEquals(this.PageNumberField, value) != true))
                {
                    this.PageNumberField = value;
                    this.RaisePropertyChanged("PageNumber");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 28)]
        public double EnvLength
        {
            get
            {
                return this.EnvLengthField;
            }
            set
            {
                if ((this.EnvLengthField.Equals(value) != true))
                {
                    this.EnvLengthField = value;
                    this.RaisePropertyChanged("EnvLength");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 29)]
        public double EnvWidth
        {
            get
            {
                return this.EnvWidthField;
            }
            set
            {
                if ((this.EnvWidthField.Equals(value) != true))
                {
                    this.EnvWidthField = value;
                    this.RaisePropertyChanged("EnvWidth");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 30)]
        public double EnvHeight
        {
            get
            {
                return this.EnvHeightField;
            }
            set
            {
                if ((this.EnvHeightField.Equals(value) != true))
                {
                    this.EnvHeightField = value;
                    this.RaisePropertyChanged("EnvHeight");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 31)]
        public string ShapeImage
        {
            get
            {
                return this.ShapeImageField;
            }
            set
            {
                if ((object.ReferenceEquals(this.ShapeImageField, value) != true))
                {
                    this.ShapeImageField = value;
                    this.RaisePropertyChanged("ShapeImage");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 32)]
        public int NoOfBends
        {
            get
            {
                return this.NoOfBendsField;
            }
            set
            {
                if ((this.NoOfBendsField.Equals(value) != true))
                {
                    this.NoOfBendsField = value;
                    this.RaisePropertyChanged("NoOfBends");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 33)]
        public int TransportModeId
        {
            get
            {
                return this.TransportModeIdField;
            }
            set
            {
                if ((this.TransportModeIdField.Equals(value) != true))
                {
                    this.TransportModeIdField = value;
                    this.RaisePropertyChanged("TransportModeId");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 34)]
        public string BVBS
        {
            get
            {
                return this.BVBSField;
            }
            set
            {
                if ((object.ReferenceEquals(this.BVBSField, value) != true))
                {
                    this.BVBSField = value;
                    this.RaisePropertyChanged("BVBS");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 35)]
        public string CreatedBy
        {
            get
            {
                return this.CreatedByField;
            }
            set
            {
                if ((object.ReferenceEquals(this.CreatedByField, value) != true))
                {
                    this.CreatedByField = value;
                    this.RaisePropertyChanged("CreatedBy");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 36)]
        public string ProduceIndicator
        {
            get
            {
                return this.ProduceIndicatorField;
            }
            set
            {
                if ((object.ReferenceEquals(this.ProduceIndicatorField, value) != true))
                {
                    this.ProduceIndicatorField = value;
                    this.RaisePropertyChanged("ProduceIndicator");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 37)]
        public string BarMark
        {
            get
            {
                return this.BarMarkField;
            }
            set
            {
                if ((object.ReferenceEquals(this.BarMarkField, value) != true))
                {
                    this.BarMarkField = value;
                    this.RaisePropertyChanged("BarMark");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 38)]
        public string CommercialDesc
        {
            get
            {
                return this.CommercialDescField;
            }
            set
            {
                if ((object.ReferenceEquals(this.CommercialDescField, value) != true))
                {
                    this.CommercialDescField = value;
                    this.RaisePropertyChanged("CommercialDesc");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 39)]
        public string ShapeType
        {
            get
            {
                return this.ShapeTypeField;
            }
            set
            {
                if ((object.ReferenceEquals(this.ShapeTypeField, value) != true))
                {
                    this.ShapeTypeField = value;
                    this.RaisePropertyChanged("ShapeType");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 40)]
        public NDSCAB.Accessory accItem
        {
            get
            {
                return this.accItemField;
            }
            set
            {
                if ((object.ReferenceEquals(this.accItemField, value) != true))
                {
                    this.accItemField = value;
                    this.RaisePropertyChanged("accItem");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 41)]
        public bool IsReadOnly
        {
            get
            {
                return this.IsReadOnlyField;
            }
            set
            {
                if ((this.IsReadOnlyField.Equals(value) != true))
                {
                    this.IsReadOnlyField = value;
                    this.RaisePropertyChanged("IsReadOnly");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 42)]
        public int intSEDetailingId
        {
            get
            {
                return this.intSEDetailingIdField;
            }
            set
            {
                if ((this.intSEDetailingIdField.Equals(value) != true))
                {
                    this.intSEDetailingIdField = value;
                    this.RaisePropertyChanged("intSEDetailingId");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 43)]
        public string ipAddress
        {
            get
            {
                return this.ipAddressField;
            }
            set
            {
                if ((object.ReferenceEquals(this.ipAddressField, value) != true))
                {
                    this.ipAddressField = value;
                    this.RaisePropertyChanged("ipAddress");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 44)]
        public string ACCProductNameforCAB
        {
            get
            {
                return this.ACCProductNameforCABField;
            }
            set
            {
                if ((object.ReferenceEquals(this.ACCProductNameforCABField, value) != true))
                {
                    this.ACCProductNameforCABField = value;
                    this.RaisePropertyChanged("ACCProductNameforCAB");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 45)]
        public string Coupler1
        {
            get
            {
                return this.Coupler1Field;
            }
            set
            {
                if ((object.ReferenceEquals(this.Coupler1Field, value) != true))
                {
                    this.Coupler1Field = value;
                    this.RaisePropertyChanged("Coupler1");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 46)]
        public string Coupler2
        {
            get
            {
                return this.Coupler2Field;
            }
            set
            {
                if ((object.ReferenceEquals(this.Coupler2Field, value) != true))
                {
                    this.Coupler2Field = value;
                    this.RaisePropertyChanged("Coupler2");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 47)]
        public string Thread1
        {
            get
            {
                return this.Thread1Field;
            }
            set
            {
                if ((object.ReferenceEquals(this.Thread1Field, value) != true))
                {
                    this.Thread1Field = value;
                    this.RaisePropertyChanged("Thread1");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 48)]
        public string Thread2
        {
            get
            {
                return this.Thread2Field;
            }
            set
            {
                if ((object.ReferenceEquals(this.Thread2Field, value) != true))
                {
                    this.Thread2Field = value;
                    this.RaisePropertyChanged("Thread2");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 49)]
        public string Coupler1Type
        {
            get
            {
                return this.Coupler1TypeField;
            }
            set
            {
                if ((object.ReferenceEquals(this.Coupler1TypeField, value) != true))
                {
                    this.Coupler1TypeField = value;
                    this.RaisePropertyChanged("Coupler1Type");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 50)]
        public string Coupler1Standard
        {
            get
            {
                return this.Coupler1StandardField;
            }
            set
            {
                if ((object.ReferenceEquals(this.Coupler1StandardField, value) != true))
                {
                    this.Coupler1StandardField = value;
                    this.RaisePropertyChanged("Coupler1Standard");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 51)]
        public string Thread1Type
        {
            get
            {
                return this.Thread1TypeField;
            }
            set
            {
                if ((object.ReferenceEquals(this.Thread1TypeField, value) != true))
                {
                    this.Thread1TypeField = value;
                    this.RaisePropertyChanged("Thread1Type");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 52)]
        public string Thread1Standard
        {
            get
            {
                return this.Thread1StandardField;
            }
            set
            {
                if ((object.ReferenceEquals(this.Thread1StandardField, value) != true))
                {
                    this.Thread1StandardField = value;
                    this.RaisePropertyChanged("Thread1Standard");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 53)]
        public string Coupler2Type
        {
            get
            {
                return this.Coupler2TypeField;
            }
            set
            {
                if ((object.ReferenceEquals(this.Coupler2TypeField, value) != true))
                {
                    this.Coupler2TypeField = value;
                    this.RaisePropertyChanged("Coupler2Type");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 54)]
        public string Coupler2Standard
        {
            get
            {
                return this.Coupler2StandardField;
            }
            set
            {
                if ((object.ReferenceEquals(this.Coupler2StandardField, value) != true))
                {
                    this.Coupler2StandardField = value;
                    this.RaisePropertyChanged("Coupler2Standard");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 55)]
        public string Thread2Type
        {
            get
            {
                return this.Thread2TypeField;
            }
            set
            {
                if ((object.ReferenceEquals(this.Thread2TypeField, value) != true))
                {
                    this.Thread2TypeField = value;
                    this.RaisePropertyChanged("Thread2Type");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 56)]
        public string Thread2Standard
        {
            get
            {
                return this.Thread2StandardField;
            }
            set
            {
                if ((object.ReferenceEquals(this.Thread2StandardField, value) != true))
                {
                    this.Thread2StandardField = value;
                    this.RaisePropertyChanged("Thread2Standard");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 57)]
        public string Locknut1
        {
            get
            {
                return this.Locknut1Field;
            }
            set
            {
                if ((object.ReferenceEquals(this.Locknut1Field, value) != true))
                {
                    this.Locknut1Field = value;
                    this.RaisePropertyChanged("Locknut1");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 58)]
        public string Locknut2
        {
            get
            {
                return this.Locknut2Field;
            }
            set
            {
                if ((object.ReferenceEquals(this.Locknut2Field, value) != true))
                {
                    this.Locknut2Field = value;
                    this.RaisePropertyChanged("Locknut2");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 59)]
        public string BBSNo
        {
            get
            {
                return this.BBSNoField;
            }
            set
            {
                if ((object.ReferenceEquals(this.BBSNoField, value) != true))
                {
                    this.BBSNoField = value;
                    this.RaisePropertyChanged("BBSNo");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 60)]
        public bool IsVPNUsers
        {
            get
            {
                return this.IsVPNUsersField;
            }
            set
            {
                if ((this.IsVPNUsersField.Equals(value) != true))
                {
                    this.IsVPNUsersField = value;
                    this.RaisePropertyChanged("IsVPNUsers");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 61)]
        public string ProduceInd
        {
            get
            {
                return this.ProduceIndField;
            }
            set
            {
                if ((object.ReferenceEquals(this.ProduceIndField, value) != true))
                {
                    this.ProduceIndField = value;
                    this.RaisePropertyChanged("ProduceInd");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 62)]
        public NDSCAB.ShapeCode Shape
        {
            get
            {
                return this.ShapeField;
            }
            set
            {
                if ((object.ReferenceEquals(this.ShapeField, value) != true))
                {
                    this.ShapeField = value;
                    this.RaisePropertyChanged("Shape");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 63)]
        public bool IsVariableBar
        {
            get
            {
                return this.IsVariableBarField;
            }
            set
            {
                if ((this.IsVariableBarField.Equals(value) != true))
                {
                    this.IsVariableBarField = value;
                    this.RaisePropertyChanged("IsVariableBar");
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
