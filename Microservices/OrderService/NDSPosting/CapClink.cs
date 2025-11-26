namespace OrderService.NDSPosting
{
    public partial class CapClink : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged
    {

        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;

        private int WBSElementIdField;

        private int ProductTypeIdField;

        private int StructureElementTypeIdField;

        private bool ExistField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private NDSPosting.Customer CustomerField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private NDSPosting.Contract ContractField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private NDSPosting.Project ProjectField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string WBS1Field;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string WBS2Field;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string WBS3Field;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private NDSPosting.StructureElement StructureElementField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private NDSPosting.ProductType ProductTypeField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private NDSPosting.ShapeCode ShapeCodeField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private NDSPosting.ProductCode ProductCodeField;

        private int ParentIdField;

        private int WidthField;

        private int DepthField;

        private int MWLengthField;

        private int CWLengthField;

        private int QtyField;

        private int RevNoField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string AddFlagField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string ShapeIdField;

        private int MO1Field;

        private int MO2Field;

        private int CO1Field;

        private int CO2Field;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string CapProductField;

        private int CountField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string TypeField;

        private int MWQtyField;

        private int CWQtyField;

        private int MWSpaceField;

        private int CWSpaceField;

        private decimal InvoiceMWWeightField;

        private decimal InvoiceCWWeightField;

        private decimal TheoriticalWeightField;

        private int AreaField;

        private int SMIDField;

        private int PMIDField;

        private int ShapeCodesField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string ShapeCodeNameField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string SlabParentValueField;

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
        public int WBSElementId
        {
            get
            {
                return this.WBSElementIdField;
            }
            set
            {
                if ((this.WBSElementIdField.Equals(value) != true))
                {
                    this.WBSElementIdField = value;
                    this.RaisePropertyChanged("WBSElementId");
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

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 2)]
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

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 3)]
        public bool Exist
        {
            get
            {
                return this.ExistField;
            }
            set
            {
                if ((this.ExistField.Equals(value) != true))
                {
                    this.ExistField = value;
                    this.RaisePropertyChanged("Exist");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 4)]
        public NDSPosting.Customer Customer
        {
            get
            {
                return this.CustomerField;
            }
            set
            {
                if ((object.ReferenceEquals(this.CustomerField, value) != true))
                {
                    this.CustomerField = value;
                    this.RaisePropertyChanged("Customer");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 5)]
        public NDSPosting.Contract Contract
        {
            get
            {
                return this.ContractField;
            }
            set
            {
                if ((object.ReferenceEquals(this.ContractField, value) != true))
                {
                    this.ContractField = value;
                    this.RaisePropertyChanged("Contract");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 6)]
        public NDSPosting.Project Project
        {
            get
            {
                return this.ProjectField;
            }
            set
            {
                if ((object.ReferenceEquals(this.ProjectField, value) != true))
                {
                    this.ProjectField = value;
                    this.RaisePropertyChanged("Project");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 7)]
        public string WBS1
        {
            get
            {
                return this.WBS1Field;
            }
            set
            {
                if ((object.ReferenceEquals(this.WBS1Field, value) != true))
                {
                    this.WBS1Field = value;
                    this.RaisePropertyChanged("WBS1");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 8)]
        public string WBS2
        {
            get
            {
                return this.WBS2Field;
            }
            set
            {
                if ((object.ReferenceEquals(this.WBS2Field, value) != true))
                {
                    this.WBS2Field = value;
                    this.RaisePropertyChanged("WBS2");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 9)]
        public string WBS3
        {
            get
            {
                return this.WBS3Field;
            }
            set
            {
                if ((object.ReferenceEquals(this.WBS3Field, value) != true))
                {
                    this.WBS3Field = value;
                    this.RaisePropertyChanged("WBS3");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 10)]
        public NDSPosting.StructureElement StructureElement
        {
            get
            {
                return this.StructureElementField;
            }
            set
            {
                if ((object.ReferenceEquals(this.StructureElementField, value) != true))
                {
                    this.StructureElementField = value;
                    this.RaisePropertyChanged("StructureElement");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 11)]
        public NDSPosting.ProductType ProductType
        {
            get
            {
                return this.ProductTypeField;
            }
            set
            {
                if ((object.ReferenceEquals(this.ProductTypeField, value) != true))
                {
                    this.ProductTypeField = value;
                    this.RaisePropertyChanged("ProductType");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 12)]
        public NDSPosting.ShapeCode ShapeCode
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

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 13)]
        public NDSPosting.ProductCode ProductCode
        {
            get
            {
                return this.ProductCodeField;
            }
            set
            {
                if ((object.ReferenceEquals(this.ProductCodeField, value) != true))
                {
                    this.ProductCodeField = value;
                    this.RaisePropertyChanged("ProductCode");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 14)]
        public int ParentId
        {
            get
            {
                return this.ParentIdField;
            }
            set
            {
                if ((this.ParentIdField.Equals(value) != true))
                {
                    this.ParentIdField = value;
                    this.RaisePropertyChanged("ParentId");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 15)]
        public int Width
        {
            get
            {
                return this.WidthField;
            }
            set
            {
                if ((this.WidthField.Equals(value) != true))
                {
                    this.WidthField = value;
                    this.RaisePropertyChanged("Width");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 16)]
        public int Depth
        {
            get
            {
                return this.DepthField;
            }
            set
            {
                if ((this.DepthField.Equals(value) != true))
                {
                    this.DepthField = value;
                    this.RaisePropertyChanged("Depth");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 17)]
        public int MWLength
        {
            get
            {
                return this.MWLengthField;
            }
            set
            {
                if ((this.MWLengthField.Equals(value) != true))
                {
                    this.MWLengthField = value;
                    this.RaisePropertyChanged("MWLength");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 18)]
        public int CWLength
        {
            get
            {
                return this.CWLengthField;
            }
            set
            {
                if ((this.CWLengthField.Equals(value) != true))
                {
                    this.CWLengthField = value;
                    this.RaisePropertyChanged("CWLength");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 19)]
        public int Qty
        {
            get
            {
                return this.QtyField;
            }
            set
            {
                if ((this.QtyField.Equals(value) != true))
                {
                    this.QtyField = value;
                    this.RaisePropertyChanged("Qty");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 20)]
        public int RevNo
        {
            get
            {
                return this.RevNoField;
            }
            set
            {
                if ((this.RevNoField.Equals(value) != true))
                {
                    this.RevNoField = value;
                    this.RaisePropertyChanged("RevNo");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 21)]
        public string AddFlag
        {
            get
            {
                return this.AddFlagField;
            }
            set
            {
                if ((object.ReferenceEquals(this.AddFlagField, value) != true))
                {
                    this.AddFlagField = value;
                    this.RaisePropertyChanged("AddFlag");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 22)]
        public string ShapeId
        {
            get
            {
                return this.ShapeIdField;
            }
            set
            {
                if ((object.ReferenceEquals(this.ShapeIdField, value) != true))
                {
                    this.ShapeIdField = value;
                    this.RaisePropertyChanged("ShapeId");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 23)]
        public int MO1
        {
            get
            {
                return this.MO1Field;
            }
            set
            {
                if ((this.MO1Field.Equals(value) != true))
                {
                    this.MO1Field = value;
                    this.RaisePropertyChanged("MO1");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 24)]
        public int MO2
        {
            get
            {
                return this.MO2Field;
            }
            set
            {
                if ((this.MO2Field.Equals(value) != true))
                {
                    this.MO2Field = value;
                    this.RaisePropertyChanged("MO2");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 25)]
        public int CO1
        {
            get
            {
                return this.CO1Field;
            }
            set
            {
                if ((this.CO1Field.Equals(value) != true))
                {
                    this.CO1Field = value;
                    this.RaisePropertyChanged("CO1");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 26)]
        public int CO2
        {
            get
            {
                return this.CO2Field;
            }
            set
            {
                if ((this.CO2Field.Equals(value) != true))
                {
                    this.CO2Field = value;
                    this.RaisePropertyChanged("CO2");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 27)]
        public string CapProduct
        {
            get
            {
                return this.CapProductField;
            }
            set
            {
                if ((object.ReferenceEquals(this.CapProductField, value) != true))
                {
                    this.CapProductField = value;
                    this.RaisePropertyChanged("CapProduct");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 28)]
        public int Count
        {
            get
            {
                return this.CountField;
            }
            set
            {
                if ((this.CountField.Equals(value) != true))
                {
                    this.CountField = value;
                    this.RaisePropertyChanged("Count");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 29)]
        public string Type
        {
            get
            {
                return this.TypeField;
            }
            set
            {
                if ((object.ReferenceEquals(this.TypeField, value) != true))
                {
                    this.TypeField = value;
                    this.RaisePropertyChanged("Type");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 30)]
        public int MWQty
        {
            get
            {
                return this.MWQtyField;
            }
            set
            {
                if ((this.MWQtyField.Equals(value) != true))
                {
                    this.MWQtyField = value;
                    this.RaisePropertyChanged("MWQty");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 31)]
        public int CWQty
        {
            get
            {
                return this.CWQtyField;
            }
            set
            {
                if ((this.CWQtyField.Equals(value) != true))
                {
                    this.CWQtyField = value;
                    this.RaisePropertyChanged("CWQty");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 32)]
        public int MWSpace
        {
            get
            {
                return this.MWSpaceField;
            }
            set
            {
                if ((this.MWSpaceField.Equals(value) != true))
                {
                    this.MWSpaceField = value;
                    this.RaisePropertyChanged("MWSpace");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 33)]
        public int CWSpace
        {
            get
            {
                return this.CWSpaceField;
            }
            set
            {
                if ((this.CWSpaceField.Equals(value) != true))
                {
                    this.CWSpaceField = value;
                    this.RaisePropertyChanged("CWSpace");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 34)]
        public decimal InvoiceMWWeight
        {
            get
            {
                return this.InvoiceMWWeightField;
            }
            set
            {
                if ((this.InvoiceMWWeightField.Equals(value) != true))
                {
                    this.InvoiceMWWeightField = value;
                    this.RaisePropertyChanged("InvoiceMWWeight");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 35)]
        public decimal InvoiceCWWeight
        {
            get
            {
                return this.InvoiceCWWeightField;
            }
            set
            {
                if ((this.InvoiceCWWeightField.Equals(value) != true))
                {
                    this.InvoiceCWWeightField = value;
                    this.RaisePropertyChanged("InvoiceCWWeight");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 36)]
        public decimal TheoriticalWeight
        {
            get
            {
                return this.TheoriticalWeightField;
            }
            set
            {
                if ((this.TheoriticalWeightField.Equals(value) != true))
                {
                    this.TheoriticalWeightField = value;
                    this.RaisePropertyChanged("TheoriticalWeight");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 37)]
        public int Area
        {
            get
            {
                return this.AreaField;
            }
            set
            {
                if ((this.AreaField.Equals(value) != true))
                {
                    this.AreaField = value;
                    this.RaisePropertyChanged("Area");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 38)]
        public int SMID
        {
            get
            {
                return this.SMIDField;
            }
            set
            {
                if ((this.SMIDField.Equals(value) != true))
                {
                    this.SMIDField = value;
                    this.RaisePropertyChanged("SMID");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 39)]
        public int PMID
        {
            get
            {
                return this.PMIDField;
            }
            set
            {
                if ((this.PMIDField.Equals(value) != true))
                {
                    this.PMIDField = value;
                    this.RaisePropertyChanged("PMID");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 40)]
        public int ShapeCodes
        {
            get
            {
                return this.ShapeCodesField;
            }
            set
            {
                if ((this.ShapeCodesField.Equals(value) != true))
                {
                    this.ShapeCodesField = value;
                    this.RaisePropertyChanged("ShapeCodes");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 41)]
        public string ShapeCodeName
        {
            get
            {
                return this.ShapeCodeNameField;
            }
            set
            {
                if ((object.ReferenceEquals(this.ShapeCodeNameField, value) != true))
                {
                    this.ShapeCodeNameField = value;
                    this.RaisePropertyChanged("ShapeCodeName");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 42)]
        public string SlabParentValue
        {
            get
            {
                return this.SlabParentValueField;
            }
            set
            {
                if ((object.ReferenceEquals(this.SlabParentValueField, value) != true))
                {
                    this.SlabParentValueField = value;
                    this.RaisePropertyChanged("SlabParentValue");
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
