namespace OrderService.NDSColumn
{
    public partial class ColumnStructure : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged
    {

        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;

        private int SEDetailingIDField;

        private int StructureMarkIdField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string StructureMarkingNameField;

        private int ParamSetNumberField;

        private int MemberQtyField;

        private int ColumnWidthField;

        private int ColumnLengthField;

        private int ColumnHeightField;

        private int TotalNoOfLinksField;

        private int ColumnShapeIdField;

        private bool IsCLinkField;

        private int ClinkProductCodeIdAtLengthField;

        private int ClinkShapeCodeIdAtLengthField;

        private int ClinkProductCodeIdAtWidthField;

        private int ClinkShapeCodeIdAtWidthField;

        private bool CLOnlyField;

        private double AreaField;

        private int TotalQtyField;

        private int ColumnProductCodeIdField;

        private int PinSizeField;

        private bool ProduceIndicatorField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private NDSColumn.ColumnProduct[] ColumnProductsField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private NDSColumn.ProductCode ProductCodeField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private NDSColumn.ShapeCode ShapeField;

        private int RowatLengthField;

        private int RowatWidthField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private NDSColumn.ProductCode ClinkProductLengthField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private NDSColumn.ProductCode ClinkProductWidthField;

        private bool ProductGenerationStatusField;

        private int ParentStructureMarkIdField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string SideForCodeField;

        private bool BendingCheckIndField;

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

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false)]
        public string StructureMarkingName
        {
            get
            {
                return this.StructureMarkingNameField;
            }
            set
            {
                if ((object.ReferenceEquals(this.StructureMarkingNameField, value) != true))
                {
                    this.StructureMarkingNameField = value;
                    this.RaisePropertyChanged("StructureMarkingName");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 3)]
        public int ParamSetNumber
        {
            get
            {
                return this.ParamSetNumberField;
            }
            set
            {
                if ((this.ParamSetNumberField.Equals(value) != true))
                {
                    this.ParamSetNumberField = value;
                    this.RaisePropertyChanged("ParamSetNumber");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 4)]
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

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 5)]
        public int ColumnWidth
        {
            get
            {
                return this.ColumnWidthField;
            }
            set
            {
                if ((this.ColumnWidthField.Equals(value) != true))
                {
                    this.ColumnWidthField = value;
                    this.RaisePropertyChanged("ColumnWidth");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 6)]
        public int ColumnLength
        {
            get
            {
                return this.ColumnLengthField;
            }
            set
            {
                if ((this.ColumnLengthField.Equals(value) != true))
                {
                    this.ColumnLengthField = value;
                    this.RaisePropertyChanged("ColumnLength");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 7)]
        public int ColumnHeight
        {
            get
            {
                return this.ColumnHeightField;
            }
            set
            {
                if ((this.ColumnHeightField.Equals(value) != true))
                {
                    this.ColumnHeightField = value;
                    this.RaisePropertyChanged("ColumnHeight");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 8)]
        public int TotalNoOfLinks
        {
            get
            {
                return this.TotalNoOfLinksField;
            }
            set
            {
                if ((this.TotalNoOfLinksField.Equals(value) != true))
                {
                    this.TotalNoOfLinksField = value;
                    this.RaisePropertyChanged("TotalNoOfLinks");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 9)]
        public int ColumnShapeId
        {
            get
            {
                return this.ColumnShapeIdField;
            }
            set
            {
                if ((this.ColumnShapeIdField.Equals(value) != true))
                {
                    this.ColumnShapeIdField = value;
                    this.RaisePropertyChanged("ColumnShapeId");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 10)]
        public bool IsCLink
        {
            get
            {
                return this.IsCLinkField;
            }
            set
            {
                if ((this.IsCLinkField.Equals(value) != true))
                {
                    this.IsCLinkField = value;
                    this.RaisePropertyChanged("IsCLink");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 11)]
        public int ClinkProductCodeIdAtLength
        {
            get
            {
                return this.ClinkProductCodeIdAtLengthField;
            }
            set
            {
                if ((this.ClinkProductCodeIdAtLengthField.Equals(value) != true))
                {
                    this.ClinkProductCodeIdAtLengthField = value;
                    this.RaisePropertyChanged("ClinkProductCodeIdAtLength");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 12)]
        public int ClinkShapeCodeIdAtLength
        {
            get
            {
                return this.ClinkShapeCodeIdAtLengthField;
            }
            set
            {
                if ((this.ClinkShapeCodeIdAtLengthField.Equals(value) != true))
                {
                    this.ClinkShapeCodeIdAtLengthField = value;
                    this.RaisePropertyChanged("ClinkShapeCodeIdAtLength");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 13)]
        public int ClinkProductCodeIdAtWidth
        {
            get
            {
                return this.ClinkProductCodeIdAtWidthField;
            }
            set
            {
                if ((this.ClinkProductCodeIdAtWidthField.Equals(value) != true))
                {
                    this.ClinkProductCodeIdAtWidthField = value;
                    this.RaisePropertyChanged("ClinkProductCodeIdAtWidth");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 14)]
        public int ClinkShapeCodeIdAtWidth
        {
            get
            {
                return this.ClinkShapeCodeIdAtWidthField;
            }
            set
            {
                if ((this.ClinkShapeCodeIdAtWidthField.Equals(value) != true))
                {
                    this.ClinkShapeCodeIdAtWidthField = value;
                    this.RaisePropertyChanged("ClinkShapeCodeIdAtWidth");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 15)]
        public bool CLOnly
        {
            get
            {
                return this.CLOnlyField;
            }
            set
            {
                if ((this.CLOnlyField.Equals(value) != true))
                {
                    this.CLOnlyField = value;
                    this.RaisePropertyChanged("CLOnly");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 16)]
        public double Area
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

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 17)]
        public int TotalQty
        {
            get
            {
                return this.TotalQtyField;
            }
            set
            {
                if ((this.TotalQtyField.Equals(value) != true))
                {
                    this.TotalQtyField = value;
                    this.RaisePropertyChanged("TotalQty");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 18)]
        public int ColumnProductCodeId
        {
            get
            {
                return this.ColumnProductCodeIdField;
            }
            set
            {
                if ((this.ColumnProductCodeIdField.Equals(value) != true))
                {
                    this.ColumnProductCodeIdField = value;
                    this.RaisePropertyChanged("ColumnProductCodeId");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 19)]
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

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 20)]
        public bool ProduceIndicator
        {
            get
            {
                return this.ProduceIndicatorField;
            }
            set
            {
                if ((this.ProduceIndicatorField.Equals(value) != true))
                {
                    this.ProduceIndicatorField = value;
                    this.RaisePropertyChanged("ProduceIndicator");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 21)]
        public NDSColumn.ColumnProduct[] ColumnProducts
        {
            get
            {
                return this.ColumnProductsField;
            }
            set
            {
                if ((object.ReferenceEquals(this.ColumnProductsField, value) != true))
                {
                    this.ColumnProductsField = value;
                    this.RaisePropertyChanged("ColumnProducts");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 22)]
        public NDSColumn.ProductCode ProductCode
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

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 23)]
        public NDSColumn.ShapeCode Shape
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

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 24)]
        public int RowatLength
        {
            get
            {
                return this.RowatLengthField;
            }
            set
            {
                if ((this.RowatLengthField.Equals(value) != true))
                {
                    this.RowatLengthField = value;
                    this.RaisePropertyChanged("RowatLength");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 25)]
        public int RowatWidth
        {
            get
            {
                return this.RowatWidthField;
            }
            set
            {
                if ((this.RowatWidthField.Equals(value) != true))
                {
                    this.RowatWidthField = value;
                    this.RaisePropertyChanged("RowatWidth");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 26)]
        public NDSColumn.ProductCode ClinkProductLength
        {
            get
            {
                return this.ClinkProductLengthField;
            }
            set
            {
                if ((object.ReferenceEquals(this.ClinkProductLengthField, value) != true))
                {
                    this.ClinkProductLengthField = value;
                    this.RaisePropertyChanged("ClinkProductLength");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 27)]
        public NDSColumn.ProductCode ClinkProductWidth
        {
            get
            {
                return this.ClinkProductWidthField;
            }
            set
            {
                if ((object.ReferenceEquals(this.ClinkProductWidthField, value) != true))
                {
                    this.ClinkProductWidthField = value;
                    this.RaisePropertyChanged("ClinkProductWidth");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 28)]
        public bool ProductGenerationStatus
        {
            get
            {
                return this.ProductGenerationStatusField;
            }
            set
            {
                if ((this.ProductGenerationStatusField.Equals(value) != true))
                {
                    this.ProductGenerationStatusField = value;
                    this.RaisePropertyChanged("ProductGenerationStatus");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 29)]
        public int ParentStructureMarkId
        {
            get
            {
                return this.ParentStructureMarkIdField;
            }
            set
            {
                if ((this.ParentStructureMarkIdField.Equals(value) != true))
                {
                    this.ParentStructureMarkIdField = value;
                    this.RaisePropertyChanged("ParentStructureMarkId");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 30)]
        public string SideForCode
        {
            get
            {
                return this.SideForCodeField;
            }
            set
            {
                if ((object.ReferenceEquals(this.SideForCodeField, value) != true))
                {
                    this.SideForCodeField = value;
                    this.RaisePropertyChanged("SideForCode");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 31)]
        public bool BendingCheckInd
        {
            get
            {
                return this.BendingCheckIndField;
            }
            set
            {
                if ((this.BendingCheckIndField.Equals(value) != true))
                {
                    this.BendingCheckIndField = value;
                    this.RaisePropertyChanged("BendingCheckInd");
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
