using System.Collections.Generic;

namespace XDT.PostProcessor
{
    public class ParsedXdtForm
    {
        public Dictionary<string, List<AttributeInfo>> AttributesByTypeName { get; }
        public Dictionary<string, List<ControlInfo>> ControlsByTypeName { get; }
        public Dictionary<string, List<AttributeInfo>> AttributesByXdtType { get; }
        public List<AttributeInfo> AnyAttributes { get; }
        public List<AttributeInfo> BooleanAttributes { get; }
        public List<AttributeInfo> DateAttributes { get; }
        public List<AttributeInfo> LookupAttributes { get; }
        public List<AttributeInfo> NumberAttributes { get; }
        public List<AttributeInfo> OptionSetAttributes { get; }
        public List<AttributeInfo> MultiSelectAttributes { get; }
        public List<AttributeInfo> StringAttributes { get; }
        public List<ControlInfo> AttributeControls { get; }
        public List<ControlInfo> BaseControls { get; }
        public List<ControlInfo> DateControls { get; }
        // ReSharper disable once InconsistentNaming
        public List<ControlInfo> IFrameControls { get; }
        public List<ControlInfo> KbSearchControls { get; }
        public List<ControlInfo> LookupControls { get; }
        public List<ControlInfo> NoteControls { get; }
        public List<ControlInfo> MultiSelectControls { get; }
        public List<ControlInfo> NumberControls { get; }
        public List<ControlInfo> StringControls { get; }
        public List<ControlInfo> SubgridControls { get; }
        public List<ControlInfo> TimerControls { get; }
        public List<ControlInfo> WebResourceControls { get; }

        public ParsedXdtForm()
        {
            AttributesByTypeName = new Dictionary<string, List<AttributeInfo>>();
            ControlsByTypeName = new Dictionary<string, List<ControlInfo>>();
            // Attributes
            AnyAttributes = new List<AttributeInfo>();
            BooleanAttributes = new List<AttributeInfo>();
            DateAttributes = new List<AttributeInfo>();
            LookupAttributes = new List<AttributeInfo>();
            NumberAttributes = new List<AttributeInfo>();
            OptionSetAttributes = new List<AttributeInfo>();
            MultiSelectAttributes = new List<AttributeInfo>();
            StringAttributes = new List<AttributeInfo>();
            // Controls
            AttributeControls = new List<ControlInfo>();
            BaseControls = new List<ControlInfo>();
            DateControls = new List<ControlInfo>();
            IFrameControls = new List<ControlInfo>();
            KbSearchControls = new List<ControlInfo>();
            LookupControls = new List<ControlInfo>();
            NoteControls = new List<ControlInfo>();
            MultiSelectControls = new List<ControlInfo>();
            NumberControls = new List<ControlInfo>();
            StringControls = new List<ControlInfo>();
            SubgridControls = new List<ControlInfo>();
            TimerControls = new List<ControlInfo>();
            WebResourceControls = new List<ControlInfo>();
            AttributesByXdtType = new Dictionary<string, List<AttributeInfo>>()
            {
                {"any", AnyAttributes },
                {"boolean", BooleanAttributes },
                {"DateAttribute", DateAttributes },
                {"LookupAttribute", LookupAttributes },
                {"NumberAttribute", NumberAttributes },
                {"OptionSetAttribute", OptionSetAttributes },
                {"MultiSelectOptionSetAttribute", MultiSelectAttributes },
                {"string", StringAttributes },
            };
        }
    }
}
